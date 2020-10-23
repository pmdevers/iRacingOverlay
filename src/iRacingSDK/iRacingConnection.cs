using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using iRacingSDK.Logging;

namespace iRacingSDK
{
	    public class iRacingConnection
    {
	    private readonly CrossThreadEvents _connected = new CrossThreadEvents();
	    private readonly CrossThreadEvents _disconnected = new CrossThreadEvents();
	    private readonly CrossThreadEvents<DataSample> _newSessionData = new CrossThreadEvents<DataSample>();

	    private DataFeed _dataFeed = null;
	    private bool _isRunning = false;
	    readonly iRacingMemory _iRacingMemory;

        internal bool IsRunning => _isRunning;

        private long _processingTime;
        private long _waitingTime;
        private long _yieldTime;

		public long ProcessingTime => _processingTime * 1000000L / Stopwatch.Frequency;
        public long WaitingTime => _waitingTime * 1000000L / Stopwatch.Frequency;
        public long YieldTime => (_yieldTime * 1000000L / Stopwatch.Frequency);

        public readonly Replay Replay;
        public readonly PitCommand PitCommand;

        public bool IsConnected { get; private set; }

        public event Action Connected
        {
            add => _connected.Event += value;
            remove => _connected.Event -= value;
        }

        public event Action Disconnected
        {
            add => _disconnected.Event += value;
            remove => _disconnected.Event -= value;
        }

        public event Action<DataSample> NewSessionData
        {
            add => _newSessionData.Event += value;
            remove => _newSessionData.Event -= value;
        }

        public iRacingConnection()
        {
            Replay = new Replay(this);
            PitCommand = new PitCommand();
            _iRacingMemory = new iRacingMemory();
        }

		public IEnumerable<DataSample> GetDataFeed(bool logging = true)
		{
			return GetRawDataFeed(logging).WithLastSample().WithEvents(_connected, _disconnected, _newSessionData);
		}

		internal IEnumerable<DataSample> GetRawDataFeed(bool logging = true)
        {
            if (_isRunning)
                throw new Exception("Can not call GetDataFeed concurrently.");

            _isRunning = true;
            try
            {
                foreach (var notConnectedSample in WaitForInitialConnection())
                {
                    IsConnected = false;
                    yield return notConnectedSample;
                }

                foreach (var sample in AllSamples(logging))
                {
                    IsConnected = sample.IsConnected;
                    yield return sample;
                }
            }
            finally
            {
                _isRunning = false;
            }
        }

        IEnumerable<DataSample> WaitForInitialConnection()
        {
            bool wasConnected = _iRacingMemory.Accessor != null;
            TraceInfo.WriteLineIf(!wasConnected, "Waiting to connect to iRacing application");

            while (!_iRacingMemory.IsConnected())
            {
                yield return DataSample.YetToConnected;
                Thread.Sleep(10);
            }

            TraceInfo.WriteLineIf(!wasConnected, "Connected to iRacing application");
        }

        IEnumerable<DataSample> AllSamples(bool logging)
        {
            if (_dataFeed == null)
                _dataFeed = new DataFeed(_iRacingMemory.Accessor);

            var nextTickCount = 0;
            var lastTickTime = DateTime.Now;

            var watchProcessingTime = new Stopwatch();
            var watchWaitingTime = new Stopwatch();

            while (true)
            {
                watchWaitingTime.Restart();
                _iRacingMemory.WaitForData();
                _waitingTime = watchWaitingTime.ElapsedTicks;

                watchProcessingTime.Restart();

                var data = _dataFeed.GetNextDataSample(nextTickCount, logging);
                if (data != null)
                {
                    if (data.IsConnected)
                    {
                        if (data.Telemetry.TickCount == nextTickCount - 1)
                            continue; //Got the same sample - try again.

                        if (logging && data.Telemetry.TickCount != nextTickCount && nextTickCount != 0)
                            Debug.WriteLine("Dropped DataSample from {0} to {1}. Over time of {2}",
                                nextTickCount, data.Telemetry.TickCount - 1, (DateTime.Now - lastTickTime).ToString(@"s\.fff"), "WARN");

                        nextTickCount = data.Telemetry.TickCount + 1;
                        lastTickTime = DateTime.Now;
                    }
                    _processingTime = watchProcessingTime.ElapsedTicks;

                    watchProcessingTime.Restart();
                    yield return data;
                    _yieldTime = watchProcessingTime.ElapsedTicks;
                }
            }
        }
    }
}

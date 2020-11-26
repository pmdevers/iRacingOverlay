using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iRacingTimings.Data
{
    public class TimeDelta
    {
        
        private readonly int _splitdistance;
        private readonly int _maxCars;

        private Time _prevTime;
        private int _arraySize;
        private float _splitLenght;
        private int _followed;
        private LapTime[] _bestlap;
        private LapTime[] _currentlap;
        private bool _validbestlap;
        private Dictionary<int, LapTime[]> _splits;
        private int[] _splitPointer;
        private LapTime _lapstarttime;

        public TimeDelta(Distance lenght, int splitdistance, int maxCars)
        {
            _splitdistance = splitdistance;
            _maxCars = maxCars;

            _arraySize = lenght / _splitdistance;
            _splitLenght = (float)(1.0 / _arraySize);

            _followed = -1;
            _bestlap = new LapTime[_arraySize];
            _currentlap = new LapTime[_arraySize];
            _validbestlap = false;

            _splits = new Dictionary<int, LapTime[]>();
            _splitPointer = new int[_maxCars];
            for (int i = 0; i < _maxCars; i++)
            {
                _splits[i] = new LapTime[_arraySize];
            }
        }

        public void Update(Time sessionTime, float[] trackPosition)
        {
            if (sessionTime > _prevTime)
            {
                int currentSplitPointer;

                for (int i = 0; i < trackPosition.Length; i++)
                {
                    currentSplitPointer = (int) Math.Floor((trackPosition[i]) % 1 / _splitLenght);

                    if (currentSplitPointer != _splitPointer[i])
                    {
                        double distance = trackPosition[i] - (currentSplitPointer * _splitLenght);
                        double correction = distance / _splitLenght;
                        LapTime currentSplitTime = sessionTime - ((sessionTime - _prevTime) * correction);

                        bool newLap = currentSplitPointer < (100 / _splitdistance) &&
                                      _splitPointer[i] > _arraySize - (100 / _splitdistance);

                        int splithop = currentSplitPointer - _splitPointer[i];
                        LapTime splitcumulator = (currentSplitTime - (double)_prevTime) / splithop;

                        int k = 1;

                        if (splithop < 0 && newLap)
                        {
                            splithop = _arraySize - _splitPointer[i] + currentSplitPointer;

                            if (_followed >= 0 && i == _followed)
                            {
                                for (int j = _splitPointer[i] + 1; j < _arraySize; j++)
                                {
                                    _splits[i][j % _arraySize] = _splits[i][_splitPointer[i]] + k++ * splitcumulator;
                                }
                            }
                        }

                        if (_followed >= 0 && i == _followed)
                        {
                            // check new lap
                            if (newLap)
                            {
                                if ((currentSplitTime - _splits[i][0]) < _bestlap[_bestlap.Length - 1] || _bestlap[_bestlap.Length - 1] == 0)
                                {
                                    _validbestlap = true;
                                    // save lap and substract session time offset
                                    for (int j = 0; j < _bestlap.Length - 1; j++)
                                    {
                                        _bestlap[j] = _splits[i][j + 1] - _splits[i][0];
                                        if (_splits[i][j + 1] == 0.0)
                                            _validbestlap = false;
                                    }

                                    _bestlap[_bestlap.Length - 1] = currentSplitTime - _splits[i][0];
                                }
                            }

                            _lapstarttime = _currentlap[currentSplitPointer];
                            _currentlap[currentSplitPointer] = (double)currentSplitTime;
                        }


                        if (splithop > 1)
                        {
                            k = 1;
                            for (int j = _splitPointer[i] + 1; j % _arraySize != currentSplitPointer; j++)
                            {
                                _splits[i][j % _arraySize] = _splits[i][_splitPointer[i]] + (k++ * splitcumulator);
                            }
                        }

                        // save
                        _splits[i][currentSplitPointer] = (double)currentSplitTime;
                        _splitPointer[i] = currentSplitPointer;
                    }
                }
            }

            _prevTime = sessionTime;
        }

        public LapTime GetDelta(int carIdx1, int carIdx2)
        {
            var comparedSplit = _splitPointer[carIdx1];

            if (comparedSplit < 0)
                comparedSplit = _splits[carIdx1].Length - 1;

            var delta = _splits[carIdx1][comparedSplit] - _splits[carIdx2][comparedSplit];

            if (_splits[carIdx1][comparedSplit] == 0 || _splits[carIdx2][comparedSplit] == 0)
            {
                return new LapTime(0);
            }
            else
            {
                return delta;
            }
        }
    }
}

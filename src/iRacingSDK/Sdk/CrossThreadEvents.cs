using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace iRacingSDK
{
	internal class CrossThreadEvents<T1, T2>
    {
	    private event Action<T1, T2> _event;

        private readonly Dictionary<Action<T1, T2>, Action<T1, T2>> _eventDelegates = new Dictionary<Action<T1, T2>, Action<T1, T2>>();

        public void Invoke(T1 t1, T2 t2)
        {
	        _event?.Invoke(t1, t2);
        }

        public event Action<T1, T2> Event
        {
            add
            {
                var context = SynchronizationContext.Current;
                var newDelgate = context != null ?
	                (t1, t2) => context.Send(i => value(t1, t2), null) :
	                value;
                _eventDelegates.Add(value, newDelgate);
                _event += newDelgate;
            }

            remove
            {
                var context = SynchronizationContext.Current;

                var delgate = _eventDelegates[value];
                _eventDelegates.Remove(value);

                _event -= delgate;
            }
        }
    }

    internal class CrossThreadEvents<T>
    {
	    private event Action<T> _event;

        readonly Dictionary<Action<T>, Action<T>> _eventDelegates = new Dictionary<Action<T>, Action<T>>();

        public void Invoke(T t)
        {
	        _event?.Invoke(t);
        }

        public event Action<T> Event 
        {
            add
            {
                var context = SynchronizationContext.Current;
				var newDelgate = context != null ?
					(d) => context.Send(i => value(d), null) :
					value;
				_eventDelegates.Add(value, newDelgate);
                _event += newDelgate;
            }

            remove
            {
                var context = SynchronizationContext.Current;
				var delgate = _eventDelegates[value];
                _eventDelegates.Remove(value);
				_event -= delgate;
            }
        }
    }

    internal class CrossThreadEvents
    {
	    private event Action _event;
		readonly Dictionary<Action, Action> _eventDelegates = new Dictionary<Action, Action>();

        public void Invoke()
        {
	        _event?.Invoke();
        }
        public event Action Event
        {
            add
            {
                var context = SynchronizationContext.Current;

                Action newDelgate = context != null ?
	                () => context.Send(i => value(), null) :
	                value;

                _eventDelegates.Add(value, newDelgate);
                _event += newDelgate;
            }

            remove
            {
                var context = SynchronizationContext.Current;
				var delgate = _eventDelegates[value];
                _eventDelegates.Remove(value);

                _event -= delgate;
            }
        }
    }
}

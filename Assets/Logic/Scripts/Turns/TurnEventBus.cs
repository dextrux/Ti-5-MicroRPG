using System;
using System.Collections.Generic;

namespace Logic.Scripts.Turns
{
    public interface ITurnEventBus
    {
        void Publish<T>(T signal);
        void Subscribe<T>(Action<T> handler);
        void Unsubscribe<T>(Action<T> handler);
    }

    public class TurnEventBus : ITurnEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();

        public void Publish<T>(T signal)
        {
            Type type = typeof(T);
            if (!_handlers.TryGetValue(type, out List<Delegate> list)) return;
            Delegate[] snapshot = list.ToArray();
            for (int i = 0; i < snapshot.Length; i++)
            {
                Action<T> del = snapshot[i] as Action<T>;
                del?.Invoke(signal);
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            Type type = typeof(T);
            if (!_handlers.TryGetValue(type, out List<Delegate> list))
            {
                list = new List<Delegate>();
                _handlers[type] = list;
            }
            if (!list.Contains(handler))
            {
                list.Add(handler);
            }
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            Type type = typeof(T);
            if (_handlers.TryGetValue(type, out List<Delegate> list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                {
                    _handlers.Remove(type);
                }
            }
        }
    }
}

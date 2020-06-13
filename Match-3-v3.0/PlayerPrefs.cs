using System;
using System.Collections.Generic;

namespace Match_3_v3._0
{
    internal static class PlayerPrefs
    {
        private static Dictionary<string, object> _preferencies = new Dictionary<string, object>();
        private static object locker = new object();

        public static void ClearAll()
        {
            lock (locker)
            {
                _preferencies.Clear();
            }
        }

        public static T Get<T>(string key)
        {
            if (_preferencies.ContainsKey(key))
            {
                lock (locker)
                {
                    var value = _preferencies[key];
                    if (typeof(T).IsAssignableFrom(value.GetType()))
                    {
                        return (T)value;
                    }
                    else
                    {
                        throw new InvalidCastException();
                    }
                }
            }
            return default(T);
        }

        public static bool Has(string key)
        {
            return _preferencies.ContainsKey(key);
        }

        public static bool Remove(string key)
        {
            lock (locker)
            {
                return _preferencies.Remove(key);
            }
        }

        public static void Set<T>(string key, T value)
        {
            lock (locker)
            {
                _preferencies[key] = value;
            }
        }
    }
}
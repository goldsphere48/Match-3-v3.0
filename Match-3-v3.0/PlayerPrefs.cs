using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0
{
    static class PlayerPrefs
    {
        private static Dictionary<string, object> _preferencies = new Dictionary<string, object>();
        private static object locker = new object();

        public static void Set<T>(string key, T value)
        {
            lock (locker)
            {
                _preferencies[key] = value;
            }
        }

        public static T Get<T>(string key)
        {
            if (_preferencies.ContainsKey(key))
            {
                lock (locker) {
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

        public static void ClearAll()
        {
            lock (locker)
            {
                _preferencies.Clear();
            }
        }
    }
}

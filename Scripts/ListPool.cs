using System;
using System.Collections;
using System.Collections.Generic;

namespace JamesGames.ReadmeMaker
{
    public class ListPool
    {
        private static Dictionary<Type, IList> m_poolList = new Dictionary<Type,IList>();
        
        public static List<T> Pull<T>()
        {
            Type type = typeof(T);
            if (!m_poolList.TryGetValue(type, out IList pool))
            {
                pool = new List<List<T>>();
                m_poolList[type] = pool;
            }

            if (pool.Count == 0)
            {
                return new List<T>();
            }

            List<T> element = (List<T>)pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            return element;
        }

        public static void Push<T>(List<T> list)
        {
            Type type = typeof(T);
            if (!m_poolList.TryGetValue(type, out IList pool))
            {
                pool = new List<List<T>>();
                m_poolList[type] = pool;
            }

            list.Clear();
            pool.Add(list);
        }
    }
}
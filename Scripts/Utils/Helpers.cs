using System;
using System.Collections.Generic;
using System.Reflection;
using DiskCardGame;
using InscryptionAPI.Guid;

namespace JamesGames.ReadmeMaker
{
    public static class Helpers
    {
        public static T GetStaticPrivateField<T>(Type classType, string fieldName)
        {
            FieldInfo info = classType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            object value = info.GetValue(null);
            return (T)value;
        }

        public static List<T> RemoveDuplicates<T>(List<T> list, ref Dictionary<T, int> count)
        {
            List<T> newList = new List<T>();
            for (int i = 0; i < list.Count; i++)
            {
                T value = list[i];
                if (count.TryGetValue(value, out int elementCount))
                {
                    count[value] = elementCount + 1;
                }
                else
                {
                    newList.Add(value);
                    count[value] = 1;
                }
            }
            
            return newList;
        }
    }
}
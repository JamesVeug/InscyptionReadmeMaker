using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using APIPlugin;
using BepInEx;
using BepInEx.Bootstrap;
using InscryptionAPI.Guid;
using InscryptionAPI.Saves;
using UnityEngine;

namespace JamesGames.ReadmeMaker
{
    public static class Helpers
    {
        private static Dictionary<object, Tuple<string, string>> GUIDNameLookup = null;

        public static string GetGUID(object value)
        {
            Tuple<string,string> tuple = GetGUIDAndNameFromEnum(value);
            if (tuple == null)
            {
                return "unknown guid";
            }

            return tuple.Item1;
        }

        public static string GetName(object value)
        {
            Tuple<string,string> tuple = GetGUIDAndNameFromEnum(value);
            if (tuple == null)
            {
                return "unknown name";
            }

            return tuple.Item2;
        }
        
        public static Tuple<string, string> GetGUIDAndNameFromEnum(object value)
        {
            if (GUIDNameLookup == null)
            {
                // Init
                GUIDNameLookup = new Dictionary<object, Tuple<string, string>>();
                foreach (KeyValuePair<string, Dictionary<string, object>> pluginData in ModdedSaveManager.SaveData.SaveData)
                {
                    foreach (KeyValuePair<string, object> savedData in pluginData.Value)
                    {
                        string entry = savedData.Key;
                        object entryValue = savedData.Value;
                        
                        // format: {typeof(T).Name}_{guid}_{value}
                        foreach (PluginInfo infosValue in Chainloader.PluginInfos.Values)
                        {
                            string pluginGUID = infosValue.Metadata.GUID;
                            int indexOf = entry.IndexOf(pluginGUID, StringComparison.Ordinal);
                            if (indexOf >= 0)
                            {
                                string entryName = entry.Substring(indexOf + pluginGUID.Length + 1);
                                GUIDNameLookup[entryValue] = new Tuple<string, string>(pluginGUID, entryName);
                                Plugin.Log.LogInfo($"{pluginGUID} {entryName} = {entryValue}");
                                break;
                            }
                        }
                    }
                }
            }

            if (GUIDNameLookup.TryGetValue(value, out Tuple<string, string> pair))
            {
                return pair;
            }

            return null;
        }

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
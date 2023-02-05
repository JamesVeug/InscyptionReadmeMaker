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
        private static Dictionary<string, Tuple<string, string>> GUIDNameLookup = null;

        public static string GetGUID(string value)
        {
            Tuple<string,string> tuple = GetGUIDAndNameFromEnum(value);
            if (tuple == null)
            {
                //Plugin.Log.LogInfo($"GetGUID: {value} = null");
                return null;
            }

            //Plugin.Log.LogInfo($"GetGUID: {value} = '{tuple.Item1}'");
            return tuple.Item1;
        }

        public static string GetName(string value)
        {
            Tuple<string,string> tuple = GetGUIDAndNameFromEnum(value);
            if (tuple == null)
            {
                //Plugin.Log.LogInfo($"GetGUID: {value} = null");
                return null;
            }

            //Plugin.Log.LogInfo($"GetGUID: {value} = '{tuple.Item1}'");
            return tuple.Item2;
        }
        
        public static Tuple<string, string> GetGUIDAndNameFromEnum(string value)
        {
            // NOTE: Fast lookup disabled because the GUIDManager can have new content at any time.
            // So disabled until there is a better solution.
            
            //if (GUIDNameLookup == null)
            {
                //Plugin.Log.LogInfo($"[GetGUIDAndNameFromEnum] Building fast lookup");
                
                // Init
                GUIDNameLookup = new Dictionary<string, Tuple<string, string>>();
                foreach (KeyValuePair<string, Dictionary<string, object>> pluginData in ModdedSaveManager.SaveData.SaveData)
                {
                    foreach (KeyValuePair<string, object> savedData in pluginData.Value)
                    {
                        string entry = savedData.Key;
                        string entryValue = Convert.ToString(savedData.Value);
                        
                        // format: {typeof(T).Name}_{guid}_{value}
                        foreach (PluginInfo infosValue in Chainloader.PluginInfos.Values)
                        {
                            string pluginGUID = infosValue.Metadata.GUID;
                            int indexOf = entry.IndexOf(pluginGUID, StringComparison.Ordinal);
                            if (indexOf >= 0)
                            {
                                string entryName = entry.Substring(indexOf + pluginGUID.Length + 1);
                                GUIDNameLookup[entryValue] = new Tuple<string, string>(pluginGUID, entryName);
                                //Plugin.Log.LogInfo($"{pluginGUID} {entryName} = {entryValue} ({entryValue.GetType().FullName})");
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
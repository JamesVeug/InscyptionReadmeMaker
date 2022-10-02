using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JamesGames.ReadmeMaker.Sections;

namespace JamesGames.ReadmeMaker.ExternalHelpers
{
    public static class Helpers
    {
        private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic;
        
        public static string GetExternalGUID(object instance, Type CustomSectionType, object row)
        {
            string guid = (string)CustomSectionType.GetMethod("GetGUID", Flags)?.Invoke(instance, new object[]{row});
            if (guid == null)
            {
                Plugin.Log.LogError("GUID for type '" + CustomSectionType.ToString() + "' is null!");
            }
            else if (guid == Plugin.PluginGuid)
            {
                Plugin.Log.LogError("GUID for type '" + CustomSectionType.ToString() + "' has not been changed! This should be the plugin guid for your mod. eg: Plugin.PluginGuid.");
                guid += "_" + CustomSectionType;
            }
            
            return guid;
        }
        
        public static void GetExternalTableDump(object instance, Type CustomSectionType, out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows)
        {
            MethodInfo method = CustomSectionType.GetMethod("GetTableDump", Flags);
                
            object[] args = new object[]{null, null};
            method?.Invoke(instance, args);
            
            // Convert CustomTableheader to TableHeader
            tableHeaders = new List<TableHeader>();
            IEnumerable enumerable = (IEnumerable)args[0];
            foreach (object header in enumerable)
            {
                string HeaderName = (string)header.GetType().GetField("HeaderName", Flags).GetValue(header);
                object alignmentData = header.GetType().GetField("Alignment", Flags).GetValue(header);
                Enum.TryParse(alignmentData.ToString(), out Alignment alignment);
                TableHeader tableHeader = new TableHeader(HeaderName, alignment);
                tableHeaders.Add(tableHeader);
            }
            
            // Rows
            rows = args[1] as List<Dictionary<string, string>>;
        }
    }
}
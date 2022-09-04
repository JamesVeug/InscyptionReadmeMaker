using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JamesGames.ReadmeMaker.Sections;
using UnityEngine;

namespace JamesGames.ReadmeMaker.ExternalHelpers
{
    public class ExternalSectionReader : ISection
    {
        public static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic;
        
        public string SectionName => CustomSectionType.GetMethod("SectionName", Flags)?.Invoke(CustomSection, null) as string;
        public bool Enabled => (bool)CustomSectionType.GetMethod("Enabled", Flags)?.Invoke(CustomSection, null);

        private Type CustomSectionType = null;
        private object CustomSection = null;

        public ExternalSectionReader(object instance)
        {
            CustomSectionType = instance.GetType();
            CustomSection = instance;
        }

        public string GetGUID(object o)
        {
            string guid = (string)CustomSectionType.GetMethod("GetGUID", Flags)?.Invoke(CustomSection, new object[]{o});
            if (guid == null)
            {
                Plugin.Log.LogError("GUID for custom section '" + SectionName + "' is null!");
            }
            else if (guid == Plugin.PluginGuid)
            {
                Plugin.Log.LogError("GUID for custom section '" + SectionName + "' has not been changed! This should be the plugin guid for your mod. eg: Plugin.PluginGuid.");
                guid += "_" + CustomSectionType;
            }
            
            return guid;
        }

        public void Initialize()
        {
            CustomSectionType.GetMethod("Initialize", Flags).Invoke(CustomSection, null);
        }

        public void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows)
        {
            MethodInfo tableHeadersField = CustomSectionType.GetMethod("TableHeaders", Flags);
            tableHeaders = new List<TableHeader>();
            IEnumerable headers = (IEnumerable)tableHeadersField?.Invoke(CustomSection, null);
            foreach (object header in headers)
            {
                string HeaderName = (string)header.GetType().GetField("HeaderName", Flags).GetValue(header);
                object alignmentData = header.GetType().GetField("Alignment", Flags).GetValue(header);
                Enum.TryParse(alignmentData.ToString(), out Alignment alignment);
                TableHeader tableHeader = new TableHeader(HeaderName, alignment);
                tableHeaders.Add(tableHeader);
            }
            
            MethodInfo rowsMethod = CustomSectionType.GetMethod("GetRows", Flags);
            object rowData = rowsMethod?.Invoke(CustomSection, null);
            rows = (List<Dictionary<string, string>>)rowData;
            
        }

        public void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows)
        {
            MethodInfo rowsField = CustomSectionType.GetMethod("DumpSummary", Flags);
            rowsField?.Invoke(CustomSection, new object[]{stringBuilder, rows});
        }
    }
}
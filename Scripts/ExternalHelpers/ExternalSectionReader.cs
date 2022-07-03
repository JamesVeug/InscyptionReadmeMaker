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
        
        public void Initialize()
        {
            CustomSectionType.GetMethod("Initialize", Flags).Invoke(CustomSection, null);
        }

        public void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows)
        {
            MethodInfo tableHeadersField = CustomSectionType.GetMethod("TableHeaders", Flags);
            tableHeaders = new List<TableHeader>();
            Debug.Log("Getting headers");
            IEnumerable headers = (IEnumerable)tableHeadersField?.Invoke(CustomSection, null);
            Debug.Log("Got Headers");
            foreach (object header in headers)
            {
                Debug.Log("\tGetting HeaderName " + header);
                string HeaderName = (string)header.GetType().GetField("HeaderName", Flags).GetValue(header);
                Debug.Log("\tGetting Alignment " + header);
                object alignmentData = header.GetType().GetField("Alignment", Flags).GetValue(header);
                Debug.Log("\tCasting Alignment " + alignmentData);
                Enum.TryParse(alignmentData.ToString(), out Alignment alignment);
                Debug.Log("\tCreating Header");
                TableHeader tableHeader = new TableHeader(HeaderName, alignment);
                tableHeaders.Add(tableHeader);
            }
            
            Debug.Log("\tGetting Rows");
            MethodInfo rowsMethod = CustomSectionType.GetMethod("GetRows", Flags);
            Debug.Log("\tGot Method");
            object rowData = rowsMethod?.Invoke(CustomSection, null);
            Debug.Log("\tGot Rows");
            IEnumerable<Dictionary<string,string>> enumerable = ((IEnumerable)rowData).Cast<Dictionary<string, string>>();
            Debug.Log("\tCasted to dictionary");
            rows = enumerable.ToList();
            Debug.Log("\tFinished");
            
        }

        public void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows)
        {
            MethodInfo rowsField = CustomSectionType.GetMethod("DumpSummary", Flags);
            rowsField?.Invoke(CustomSection, new object[]{stringBuilder, rows});
        }
    }
}
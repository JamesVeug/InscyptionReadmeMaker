using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using JamesGames.ReadmeMaker.Sections;

namespace JamesGames.ReadmeMaker.ExternalHelpers
{
    public class ExternalSectionReader : ASection<object>
    {
        private static readonly BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic;

        public override string SectionName => CustomSectionType.GetMethod("SectionName", Flags)?.Invoke(CustomSection, null) as string;
        public override bool Enabled => (bool)CustomSectionType.GetMethod("Enabled", Flags)?.Invoke(CustomSection, null);

        private Type CustomSectionType = null;
        private object CustomSection = null;

        public ExternalSectionReader(object instance)
        {
            CustomSectionType = instance.GetType();
            CustomSection = instance;

            FieldInfo field = CustomSectionType.BaseType.GetField("m_readmeExternalSectionReference", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(instance, this);
        }

        public override void Initialize()
        {
            object data = CustomSectionType.GetMethod("Initialize", Flags).Invoke(CustomSection, null);
            IList list = (IList)data;
            rawData = list.Cast<object>().ToList();
        }

        public override string GetGUID(object o)
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

        public override void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows)
        {
            Plugin.Log.LogError($"[GetTableDump] '{SectionName}'");
            MethodInfo method = CustomSectionType.GetMethod("GetTableDump", Flags);
                
            object[] args = new object[]{null, null};
            method?.Invoke(CustomSection, args);
            Plugin.Log.LogError($"[GetTableDump] args done");
            
            // Convert CustomTableheader to TableHeader
            tableHeaders = new List<TableHeader>();
            IEnumerable enumerable = (IEnumerable)args[0];
            Plugin.Log.LogError($"[GetTableDump] tableHeaders enumerable " + enumerable);
            foreach (object header in enumerable)
            {
                string HeaderName = (string)header.GetType().GetField("HeaderName", Flags).GetValue(header);
                Plugin.Log.LogError($"[GetTableDump] tableHeaders HeaderName " + HeaderName);
                object alignmentData = header.GetType().GetField("Alignment", Flags).GetValue(header);
                Plugin.Log.LogError($"[GetTableDump] tableHeaders alignmentData " + alignmentData);
                Enum.TryParse(alignmentData.ToString(), out Alignment alignment);
                Plugin.Log.LogError($"[GetTableDump] tableHeaders alignment " + alignment);
                TableHeader tableHeader = new TableHeader(HeaderName, alignment);
                Plugin.Log.LogError($"[GetTableDump] tableHeaders tableHeader " + tableHeader);
                tableHeaders.Add(tableHeader);
            }
            Plugin.Log.LogError($"[GetTableDump] tableHeaders done");
            
            // Rows
            rows = args[1] as List<Dictionary<string, string>>;
            Plugin.Log.LogError($"[GetTableDump] rows done");
        }

        public override void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows)
        {
            MethodInfo rowsField = CustomSectionType.GetMethod("DumpSummary", Flags);
            rowsField?.Invoke(CustomSection, new object[]{stringBuilder, rows});
        }

        protected override int Sort(object a, object b)
        {
            MethodInfo rowsField = CustomSectionType.GetMethod("Sort", Flags);
            object result = rowsField?.Invoke(CustomSection, new object[]{a, b});
            return (int)result;
        }

        protected List<Dictionary<string, string>> BreakdownForTableExternal(out List<TableHeader> headers, object[] grouping)
        {
            // Convert grouping to TableColumn<object>[]
            TableColumn<object>[] columns = new TableColumn<object>[grouping.Length];
            for (int i = 0; i < grouping.Length; i++)
            {
                object group = grouping[i];
                Type groupType = group.GetType();
                
                // Misc
                string HeaderName = (string)groupType.GetField("HeaderName", Flags).GetValue(group);
                bool Enabled = (bool)groupType.GetField("Enabled", Flags).GetValue(group);

                // Alignment
                object alignmentData = groupType.GetField("Alignment", Flags).GetValue(group);
                Enum.TryParse(alignmentData.ToString(), out Alignment alignment);
                
                // Getter
                Plugin.Log.LogError("[BreakdownForTableExternal] Starting Getter");
                Func<object, string> Getter = (Func<object, string>)groupType.GetField("Getter", Flags).GetValue(group);
                Plugin.Log.LogError("[BreakdownForTableExternal] Made Wrapper");

                TableColumn<object> column = new TableColumn<object>(HeaderName, Getter, Enabled, alignment);
                columns[i] = column;
            }

            Plugin.Log.LogError("[BreakdownForTableExternal] Calling");
            return base.BreakdownForTable(out headers, columns);
        }
    }
    
}
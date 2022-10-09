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
        private string PluginGUID = null;

        public ExternalSectionReader(object instance, string guid)
        {
            CustomSectionType = instance.GetType();
            CustomSection = instance;
            PluginGUID = guid;

            FieldInfo field = CustomSectionType.BaseType.GetField("m_readmeExternalSectionReference", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(instance, this);
        }

        public override void Initialize(RegisteredMod mod)
        {
            // Hack maybe? - Will change whe nwe actually use templates
            if (mod.PluginGUID != PluginGUID)
            {
                rawData = new List<object>();
                return;
            }
            
            // TODO: Pass mod to external initialize
            object data = CustomSectionType.GetMethod("Initialize", Flags).Invoke(CustomSection, null);
            IList list = (IList)data;
            rawData = list.Cast<object>().ToList();
        }

        public override string GetGUID(object o)
        {
            return Helpers.GetExternalGUID(CustomSection, CustomSectionType, o);
        }

        public override void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows)
        {
            Helpers.GetExternalTableDump(CustomSection, CustomSectionType, out tableHeaders, out rows);
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
                Func<object, string> Getter = (Func<object, string>)groupType.GetField("Getter", Flags).GetValue(group);

                TableColumn<object> column = new TableColumn<object>(HeaderName, Getter, Enabled, alignment);
                columns[i] = column;
            }

            return base.BreakdownForTable(out headers, columns);
        }
    }
    
}
using System;
using System.Collections.Generic;
using System.Text;

namespace JamesGames.ReadmeMaker.Sections
{
    public enum Alignment
    {
        Left, Middle, Right
    }

    public class TableHeader
    {
        public readonly string HeaderName;
        public readonly Alignment Alignment;

        public TableHeader(string headerName, Alignment alignment)
        {
            HeaderName = headerName;
            Alignment = alignment;
        }
    }

    public class TableColumn<T>
    {
        public string HeaderName;
        public readonly Alignment Alignment;
        public readonly Func<T, string> Getter;
        public readonly bool Enabled;

        public TableColumn(string headerName, Func<T, string> getter, bool enabled=true, Alignment alignment=Alignment.Left)
        {
            HeaderName = headerName;
            Getter = getter;
            Alignment = alignment;
            Enabled = enabled;
        }
    }
    
    public abstract class ASection<T> : ISection
    {
        public abstract string SectionName { get; }
        public virtual bool Enabled => true;

        public abstract string GetGUID(T o);

        protected List<T> rawData = new List<T>();

        public abstract void Initialize(RegisteredMod mod);
        protected abstract int Sort(T a, T b);
        
        public abstract void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows);

        public virtual void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows)
        {
            if (rows.Count > 0)
            {
                stringBuilder.Append($"\n{rows.Count} {SectionName}\n");
            }
        }

        protected virtual bool Filter(T o)
        {
            string guid = GetGUID(o);
            if (guid == null)
            {
                return true;
            }
            guid = guid.Trim();
            
            if (ReadmeConfig.Instance.ModsToIgnore.Count > 0)
            {
                bool ignoreMod = ReadmeConfig.Instance.ModsToIgnore.Contains(guid);
                if (ignoreMod)
                {
                    // Ignore this mod
                    return false;
                }
            }

            if (ReadmeConfig.Instance.FilterByModsGUID.Count == 0)
            {
                // Show everything
                return true;
            }

            bool isPluginGuidFiltered = ReadmeConfig.Instance.FilterByModsGUID.Contains(guid);
            return isPluginGuidFiltered;
        }
        
        protected virtual List<Dictionary<string, string>> BreakdownForTable(out List<TableHeader> headers, params TableColumn<T>[] grouping)
        {
            headers = new List<TableHeader>();
            Dictionary<string, bool> columnUseCount = new Dictionary<string, bool>();
            if (ReadmeConfig.Instance.ShowGUIDS)
            {
                headers.Add(new TableHeader("GUID", Alignment.Middle));
                columnUseCount["GUID"] = true;
            }
            
            for (int i = 0; i < grouping.Length; i++)
            {
                TableColumn<T> column = grouping[i];
                if (column.Enabled)
                {
                    headers.Add(new TableHeader(column.HeaderName, column.Alignment));
                }
            }

            List<Dictionary<string, string>> split = new List<Dictionary<string, string>>();
            rawData.Sort(Sort);
            if (!ReadmeConfig.Instance.GeneralSortAscending)
            {
                rawData.Reverse();
            }
            foreach (T t in rawData)
            {
                string guid = GetGUID(t);
                if (!Filter(t))
                {
                    continue;
                }
                
                Dictionary<string, string> data = new Dictionary<string, string>();
                if (ReadmeConfig.Instance.ShowGUIDS)
                {
                    data["GUID"] = guid;
                }
                
                foreach (TableColumn<T> column in grouping)
                {
                    if (column.Enabled)
                    {
                        string columnData = column.Getter(t);
                        string columnName = column.HeaderName;
                        data[columnName] = columnData;
                        if (!string.IsNullOrEmpty(columnData))
                        {
                            columnUseCount[columnName] = true;
                        }
                    }
                }
                split.Add(data);
            }

            // Remove headers that have no data
            if (ReadmeConfig.Instance.IgnoreEmptyColumns)
            {
                for (int i = 0; i < headers.Count; i++)
                {
                    if (!columnUseCount.ContainsKey(headers[i].HeaderName))
                    {
                        headers.RemoveAt(i--);
                    }
                }
            }

            return split;
        }
    }
}
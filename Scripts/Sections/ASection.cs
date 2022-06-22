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
        public readonly string HeaderName;
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

        public abstract void Initialize();
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
            if (ReadmeConfig.Instance.ModsToIgnore.Count > 0)
            {
                bool ignoreMod = ReadmeConfig.Instance.ModsToIgnore.Contains(guid.Trim());
                if (ignoreMod)
                {
                    // Ignore this mod
                    return false;
                }
            }

            if (string.IsNullOrEmpty(ReadmeConfig.Instance.FilterByModGUID))
            {
                // Show everything
                return true;
            }

            bool isPluginGuidFiltered = ReadmeConfig.Instance.FilterByModGUID.Trim().Contains(guid.Trim());
            return isPluginGuidFiltered;
        }
        
        protected List<Dictionary<string, string>> BreakdownForTable(out List<TableHeader> headers, params TableColumn<T>[] grouping)
        {
            headers = new List<TableHeader>();
            if (ReadmeConfig.Instance.ShowGUIDS)
            {
                headers.Add(new TableHeader("GUID", Alignment.Middle));
            }
            
            for (int i = 0; i < grouping.Length; i++)
            {
                TableColumn<T> column = grouping[i];
                if (column.Enabled)
                {
                    headers.Add(new TableHeader(column.HeaderName, column.Alignment));
                }
            }

            var split = new List<Dictionary<string, string>>();
            rawData.Sort(Sort);
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
                        data[column.HeaderName] = column.Getter(t);
                    }
                }
                split.Add(data);
            }

            return split;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace ReadmeMaker.Sections
{
    public abstract class ASection
    {
        public enum Alignment
        {
            Left, Middle, Right
        }

        public class TableHeader
        {
            public string HeaderName;
            public Alignment Alignment;

            public TableHeader(string headerName, Alignment alignment)
            {
                HeaderName = headerName;
                Alignment = alignment;
            }
        }

        public class TableColumn<T>
        {
            public string HeaderName;
            public Alignment Alignment;
            public Func<T, string> Getter;
            public bool Enabled;

            public TableColumn(string headerName, Func<T, string> getter, bool enabled=true, Alignment alignment=Alignment.Left)
            {
                HeaderName = headerName;
                Getter = getter;
                Alignment = alignment;
                Enabled = enabled;
            }
        }

        public abstract string SectionName { get; }
        public virtual bool Enabled => true;

        public abstract void Initialize();

        public abstract void DumpSummary(StringBuilder stringBuilder);
        public abstract void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows);
        
        protected static void RemoveHeaderIfDisabled(List<string> headerList, string header, bool enabled)
        {
            if (!enabled)
            {
                headerList.Remove(header);
            }
        }
        
        protected List<Dictionary<string, string>> BreakdownForTable<T>(List<T> objects, out List<TableHeader> headers, params TableColumn<T>[] grouping)
        {
            headers = new List<TableHeader>();
            for (int i = 0; i < grouping.Length; i++)
            {
                TableColumn<T> column = grouping[i];
                if (column.Enabled)
                {
                    headers.Add(new TableHeader(column.HeaderName, column.Alignment));
                }
            }

            var split = new List<Dictionary<string, string>>();
            for (int i = 0; i < objects.Count; i++)
            {
                T o = objects[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                for (int j = 0; j < grouping.Length; j++)
                {
                    TableColumn<T> column = grouping[j];
                    if (column.Enabled)
                    {
                        data[column.HeaderName] = column.Getter(o);
                    }
                }
                split.Add(data);
            }

            return split;
        }
    }
}
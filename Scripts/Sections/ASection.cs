﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JamesGames.ReadmeMaker.Sections
{
    public abstract class ASection
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

        public abstract string SectionName { get; }
        public virtual bool Enabled => true;
        public abstract string GetGUID(object o);

        public abstract void Initialize();
        
        public abstract void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows);

        public virtual void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows)
        {
            if (rows.Count > 0)
            {
                stringBuilder.Append($"\n{rows.Count} {SectionName}\n");
            }
        }
        
        protected static void RemoveHeaderIfDisabled(List<string> headerList, string header, bool enabled)
        {
            if (!enabled)
            {
                headerList.Remove(header);
            }
        }

        protected virtual bool Filter(object o)
        {
            if (string.IsNullOrEmpty(ReadmeConfig.Instance.LimitToGUID))
            {
                // Show everything
                return true;
            }

            string guid = GetGUID(o);
            bool isPluginGuidFiltered = ReadmeConfig.Instance.LimitToGUID.Trim().Contains(guid.Trim());
            return isPluginGuidFiltered;
        }
        
        protected List<Dictionary<string, string>> BreakdownForTable<T>(List<T> objects, out List<TableHeader> headers, params TableColumn<T>[] grouping)
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
            foreach (T t in objects)
            {
                string guid = GetGUID(t);
                if (!Filter(t))
                {
                    Plugin.Log.LogInfo(guid + " Not Filtered");
                    continue;
                }
                Plugin.Log.LogInfo(guid + " Filtered");
                
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
using System.Collections.Generic;
using System.Text;

namespace JamesGames.ReadmeMaker.Sections
{
    public interface ISection
    {
        string SectionName { get; }
        bool Enabled { get; }
        string GetGUID(object o);
        void Initialize();
        void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows);
        void DumpSummary(StringBuilder stringBuilder, List<Dictionary<string, string>> rows);
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Boons;
using InscryptionAPI.Encounters;
using InscryptionAPI.Nodes;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewBoonsSection : ASection<BoonManager.FullBoon>
    {
        public override string SectionName => "New Boons";
        public override bool Enabled => ReadmeConfig.Instance.BoonsShow;
        
        public override void Initialize()
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(BoonManager.NewBoons);
            rawData.Sort((a, b) => string.Compare(a.boon.displayedName, b.boon.displayedName, StringComparison.Ordinal));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            rows = BreakdownForTable(out headers, new[]
            {
                new TableColumn<BoonManager.FullBoon>("Name", (a)=>a.boon.displayedName),
                new TableColumn<BoonManager.FullBoon>("Description", (a)=>a.boon.description),
            });
        }

        public override string GetGUID(BoonManager.FullBoon o)
        {
            return Helpers.GetGUID(((int)o.boon.type).ToString());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Boons;
using InscryptionAPI.Encounters;
using InscryptionAPI.Nodes;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewBoonsSection : ASection
    {
        public override string SectionName => "New Boons";
        public override bool Enabled => ReadmeConfig.Instance.BoonsShow;

        private List<BoonManager.FullBoon> boons = new List<BoonManager.FullBoon>();
        
        public override void Initialize()
        {
            boons.Clear(); // Clear so when we re-dump everything we don't double up
            boons.AddRange(BoonManager.NewBoons);
            boons.Sort((a, b) => string.Compare(a.boon.displayedName, b.boon.displayedName, StringComparison.Ordinal));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            rows = BreakdownForTable(boons, out headers, new TableColumn<BoonManager.FullBoon>[]
            {
                new TableColumn<BoonManager.FullBoon>("Name", (a)=>a.boon.displayedName),
                new TableColumn<BoonManager.FullBoon>("Description", (a)=>a.boon.description),
            });
        }

        public override string GetGUID(object o)
        {
            BoonManager.FullBoon casted = (BoonManager.FullBoon)o;
            return Helpers.GetGUID(((int)casted.boon.type).ToString());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using InscryptionAPI.Boons;
using InscryptionAPI.Guid;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewBoonsSection : ASection<BoonManager.FullBoon>
    {
        public override string SectionName => "New Boons";
        public override bool Enabled => ReadmeConfig.Instance.BoonsShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(BoonManager.NewBoons.Where(x=>GetGUID(x) == mod.PluginGUID));
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
            if (GuidManager.TryGetGuidAndKeyEnumValue(o.boon.type, out string guid, out string key))
            {
                return guid;
            }
            return "";
        }

        protected override int Sort(BoonManager.FullBoon a, BoonManager.FullBoon b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.boon.displayedName, b.boon.displayedName, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
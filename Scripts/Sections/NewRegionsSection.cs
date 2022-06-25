using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Regions;
using ReadmeMaker.Patches;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewRegionsSection : ASection<Part1RegionData>
    {
        public override string SectionName => "New Regions";
        public override bool Enabled => ReadmeConfig.Instance.RegionsShow;
        
        public override void Initialize()
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(RegionManager.NewRegions);
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<Part1RegionData>("Name", (a)=>a.region.name),
                new TableColumn<Part1RegionData>("Tier", (a)=>a.tier.ToString()),
            });
        }

        public override string GetGUID(Part1RegionData o)
        {
            return o.GetModTag();
        }

        protected override int Sort(Part1RegionData a, Part1RegionData b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.region.name, b.region.name, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Regions;
using ReadmeMaker.Patches;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewRegionsSection : ASection
    {
        public override string SectionName => "New Regions";
        public override bool Enabled => ReadmeConfig.Instance.RegionsShow;
        
        private List<Part1RegionData> regions = new List<Part1RegionData>();
        
        public override void Initialize()
        {
            regions.Clear(); // Clear so when we re-dump everything we don't double up
            regions.AddRange(RegionManager.NewRegions);
            regions.Sort((a,b)=>string.Compare(a.region.name, b.region.name, StringComparison.Ordinal));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(regions, out headers, new TableColumn<Part1RegionData>[]
            {
                new TableColumn<Part1RegionData>("Name", (a)=>a.region.name),
                new TableColumn<Part1RegionData>("Tier", (a)=>a.tier.ToString()),
            });
        }

        public override string GetGUID(object o)
        {
            Part1RegionData casted = (Part1RegionData)o;
            return casted.GetModTag();
        }
    }
}
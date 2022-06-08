using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Regions;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewRegionsSection : ASection
    {
        public override string SectionName => "New Regions";
        public override bool Enabled => ReadmeConfig.Instance.RegionsShow;
        
        private List<Part1RegionData> regions = null;
        
        public override void Initialize()
        {
            if (!ReadmeConfig.Instance.RegionsShow)
            {
                regions = new List<Part1RegionData>();
                return;
            }

            List<Part1RegionData> nodes = new List<Part1RegionData>(RegionManager.NewRegions);
            nodes.Sort((a,b)=>String.Compare(a.region.name, b.region.name, StringComparison.Ordinal));
            regions = nodes;
        }

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (regions.Count > 0)
            {
                stringBuilder.Append($"\n{regions.Count} {SectionName}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(regions, out headers, new TableColumn<Part1RegionData>[]
            {
                new TableColumn<Part1RegionData>("Name", (a)=>a.region.name),
                new TableColumn<Part1RegionData>("Tier", (a)=>a.tier.ToString()),
            });
        }
    }
}
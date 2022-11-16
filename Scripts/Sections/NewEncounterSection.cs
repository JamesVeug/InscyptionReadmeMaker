using System;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using ReadmeMaker.Patches;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewEncounterSection : ASection<EncounterBlueprintData>
    {
        public override string SectionName => "New Encounters";
        public override bool Enabled => ReadmeConfig.Instance.EncountersShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(EncounterManager.NewEncounters.Where(x=>GetGUID(x) == mod.PluginGUID));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            rows = BreakdownForTable(out headers, new[]
            {
                new TableColumn<EncounterBlueprintData>("Name", (a)=>a.name),
                new TableColumn<EncounterBlueprintData>("Min Difficulty", (a)=>a.minDifficulty.ToString()),
                new TableColumn<EncounterBlueprintData>("Max Difficulty", (a)=>a.maxDifficulty.ToString()),
                new TableColumn<EncounterBlueprintData>("Regions", GetRegionNames),
                new TableColumn<EncounterBlueprintData>("Main Tribes", GetDominantTribeNames),
                new TableColumn<EncounterBlueprintData>("Turns", (a)=>a.turns.Count.ToString()),
            });
        }

        private string GetDominantTribeNames(EncounterBlueprintData arg)
        {
            string tribes = "";
            for (int i = 0; i < arg.dominantTribes.Count; i++)
            {
                Tribe tribe = arg.dominantTribes[i];
                string tribeName = ReadmeHelpers.GetTribeName(tribe);
                if (i == 0)
                {
                    tribes = tribeName;
                }
                else
                {
                    tribes += "," + tribeName;
                }
            }

            return tribes;
        }

        private string GetRegionNames(EncounterBlueprintData a)
        {
            if (!a.regionSpecific)
            {
                return "All";
            }

            List<RegionData> regions = new List<RegionData>();
            foreach (RegionData regionData in RegionManager.AllRegionsCopy)
            {
                foreach (EncounterBlueprintData encounter in regionData.encounters)
                {
                    if (encounter.name == a.name)
                    {
                        regions.Add(regionData);
                        break;
                    }
                }
            }

            string regionNames = "None";
            for (int i = 0; i < regions.Count; i++)
            {
                RegionData regionData = regions[i];
                if (i == 0)
                {
                    regionNames = regionData.name;
                }
                else
                {
                    regionNames += "," + regionData.name;
                }
            }

            return regionNames;
        }

        public override string GetGUID(EncounterBlueprintData o)
        {
            return o.GetModTag();
        }

        protected override int Sort(EncounterBlueprintData a, EncounterBlueprintData b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.name, b.name, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
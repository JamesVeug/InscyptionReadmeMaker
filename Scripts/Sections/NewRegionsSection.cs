using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Regions;
using ReadmeMaker.Patches;
using UnityEngine;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewRegionsSection : ASection<Part1RegionData>
    {
        public override string SectionName => "New Regions";
        public override bool Enabled => ReadmeConfig.Instance.RegionsShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(RegionManager.NewRegions.Where(x=>GetGUID(x) == mod.PluginGUID));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            List<TableColumn<Part1RegionData>> columns = new List<TableColumn<Part1RegionData>>
            {
                new TableColumn<Part1RegionData>("Name", (a) => a.region.name),
                new TableColumn<Part1RegionData>("Tier", (a) => a.tier.ToString()),
                new TableColumn<Part1RegionData>("Main Tribes", GetDominantTribeNames),
                new TableColumn<Part1RegionData>("Opponents", GetOpponents),
                new TableColumn<Part1RegionData>("Items", GetItems),
                new TableColumn<Part1RegionData>("Encounters", GetEncounters)
            };

            splitCards = BreakdownForTable(out headers, columns.ToArray());
        }

        private string GetItems(Part1RegionData region)
        {
            string items = "";
            for (int i = 0; i < region.region.consumableItems.Count; i++)
            {
                ConsumableItemData opponentType = region.region.consumableItems[i];
                string itemName = opponentType.rulebookName;
                if (i == 0)
                {
                    items = itemName;
                }
                else
                {
                    items = "," + itemName;
                }
            }

            return items;
        }

        private string GetOpponents(Part1RegionData region)
        {
            string opponents = "";
            for (int i = 0; i < region.region.bosses.Count; i++)
            {
                Opponent.Type opponentType = region.region.bosses[i];
                string opponent = opponentType.ToString();
                if (i == 0)
                {
                    opponents = opponent;
                }
                else
                {
                    opponents += "," + opponent;
                }
            }

            return opponents;
        }

        private string GetEncounters(Part1RegionData region)
        {
            string encounters = "";
            for (int i = 0; i < region.region.encounters.Count; i++)
            {
                EncounterBlueprintData encounter = region.region.encounters[i];
                string tribeName = encounter.name;
                if (i == 0)
                {
                    encounters = tribeName;
                }
                else
                {
                    encounters += "," + tribeName;
                }
            }

            return encounters;
        }

        private string GetDominantTribeNames(Part1RegionData region)
        {
            string tribes = "";
            for (int i = 0; i < region.region.dominantTribes.Count; i++)
            {
                Tribe tribe = region.region.dominantTribes[i];
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
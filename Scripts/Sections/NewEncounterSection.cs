using System;
using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Encounters;
using ReadmeMaker.Patches;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewEncounterSection : ASection<EncounterBlueprintData>
    {
        public override string SectionName => "New Encounters";
        public override bool Enabled => ReadmeConfig.Instance.EncountersShow;
        
        public override void Initialize()
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(EncounterManager.NewEncounters);
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            rows = BreakdownForTable(out headers, new[]
            {
                new TableColumn<EncounterBlueprintData>("Name", (a)=>a.name),
            });
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
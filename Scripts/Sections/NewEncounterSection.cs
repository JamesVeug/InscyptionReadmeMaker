using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Encounters;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewEncounterSection : ASection
    {
        public override string SectionName => "New Encounters";
        public override bool Enabled => ReadmeConfig.Instance.EncountersShow;

        private List<EncounterBlueprintData> allEncounters = null;
        
        public override void Initialize()
        {
            if (!Enabled)
            {
                allEncounters = new List<EncounterBlueprintData>();
                return;
            }

            allEncounters = new List<EncounterBlueprintData>(EncounterManager.NewEncounters);
        }
        
        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (allEncounters.Count > 0)
            {
                stringBuilder.Append($"\n{allEncounters.Count} {SectionName}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            rows = BreakdownForTable(allEncounters, out headers, new TableColumn<EncounterBlueprintData>[]
            {
                new TableColumn<EncounterBlueprintData>("Name", (a)=>a.name),
            });
        }
    }
}
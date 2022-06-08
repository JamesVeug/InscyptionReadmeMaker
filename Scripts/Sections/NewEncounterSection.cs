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

        private List<EncounterBlueprintData> allEncounters = new List<EncounterBlueprintData>();
        
        public override void Initialize()
        {
            allEncounters.AddRange(EncounterManager.NewEncounters);
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
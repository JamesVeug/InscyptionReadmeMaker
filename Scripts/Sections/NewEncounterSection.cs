using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Encounters;
using ReadmeMaker.Patches;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewEncounterSection : ASection
    {
        public override string SectionName => "New Encounters";
        public override bool Enabled => ReadmeConfig.Instance.EncountersShow;

        private List<EncounterBlueprintData> allEncounters = new List<EncounterBlueprintData>();
        
        public override void Initialize()
        {
            allEncounters.Clear(); // Clear so when we re-dump everything we don't double up
            allEncounters.AddRange(EncounterManager.NewEncounters);
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            rows = BreakdownForTable(allEncounters, out headers, new TableColumn<EncounterBlueprintData>[]
            {
                new TableColumn<EncounterBlueprintData>("Name", (a)=>a.name),
            });
        }

        public override string GetGUID(object o)
        {
            EncounterBlueprintData casted = (EncounterBlueprintData)o;
            return casted.GetModTag();
        }
    }
}
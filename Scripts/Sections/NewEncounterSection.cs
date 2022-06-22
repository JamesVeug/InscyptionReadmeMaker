using System.Collections.Generic;
using System.Text;
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
            EncounterBlueprintData casted = (EncounterBlueprintData)o;
            return casted.GetModTag();
        }
    }
}
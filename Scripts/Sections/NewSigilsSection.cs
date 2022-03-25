using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Card;
using FullAbility = InscryptionAPI.Card.AbilityManager.FullAbility;

namespace ReadmeMaker.Sections
{
    public class NewSigilsSection : ASection
    {
        public override string SectionName => "New Sigils";
        public override bool Enabled => ReadmeConfig.Instance.SigilsShow;

        private List<FullAbility> allAbilities = null;
        
        public override void Initialize()
        {
            if (!ReadmeConfig.Instance.SigilsShow)
            {
                allAbilities = new List<FullAbility>();
                return;
            }
	        
            allAbilities = new List<FullAbility>(AbilityManager.NewAbilities);
            allAbilities.RemoveAll((a) => a.Info == null || string.IsNullOrEmpty(a.Info.rulebookName));
            allAbilities.Sort((a, b) => String.Compare(a.Info.rulebookName, b.Info.rulebookName, StringComparison.Ordinal));
        }

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (allAbilities.Count > 0)
            {
                stringBuilder.Append($"- {allAbilities.Count} {SectionName}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(allAbilities, out headers, new TableColumn<FullAbility>[]
            {
                new TableColumn<FullAbility>("Name", ReadmeHelpers.GetAbilityName),
                new TableColumn<FullAbility>("Description", ReadmeHelpers.GetAbilityDescription)
            });
        }
    }
}
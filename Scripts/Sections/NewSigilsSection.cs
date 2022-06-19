using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Card;
using FullAbility = InscryptionAPI.Card.AbilityManager.FullAbility;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewSigilsSection : ASection
    {
        public override string SectionName => "New Sigils";
        public override bool Enabled => ReadmeConfig.Instance.SigilsShow;

        private List<FullAbility> allAbilities = new List<FullAbility>();
        
        public override void Initialize()
        {
            allAbilities.Clear(); // Clear so when we re-dump everything we don't double up
            allAbilities.AddRange(AbilityManager.NewAbilities);
            allAbilities.RemoveAll((a) => a.Info == null || string.IsNullOrEmpty(a.Info.rulebookName));
            allAbilities.Sort((a, b) => String.Compare(a.Info.rulebookName, b.Info.rulebookName, StringComparison.Ordinal));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(allAbilities, out headers, new TableColumn<FullAbility>[]
            {
                new TableColumn<FullAbility>("Name", ReadmeHelpers.GetAbilityName),
                new TableColumn<FullAbility>("Description", ReadmeHelpers.GetAbilityDescription)
            });
        }

        public override string GetGUID(object o)
        {
            FullAbility casted = (FullAbility)o;
            return Helpers.GetGUID(((int)casted.Id).ToString());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Card;
using FullAbility = InscryptionAPI.Card.AbilityManager.FullAbility;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewSigilsSection : ASection<FullAbility>
    {
        public override string SectionName => "New Sigils";
        public override bool Enabled => ReadmeConfig.Instance.SigilsShow;
        
        public override void Initialize()
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(AbilityManager.NewAbilities);
            rawData.RemoveAll((a) => a.Info == null || string.IsNullOrEmpty(a.Info.rulebookName));
            rawData.Sort((a, b) => String.Compare(a.Info.rulebookName, b.Info.rulebookName, StringComparison.Ordinal));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<FullAbility>("Name", ReadmeHelpers.GetAbilityName),
                new TableColumn<FullAbility>("Description", ReadmeHelpers.GetAbilityDescription)
            });
        }

        public override string GetGUID(FullAbility o)
        {
            string guid = Helpers.GetGUID(((int)o.Id).ToString());
            return guid;
        }
    }
}
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

        protected override int Sort(FullAbility a, FullAbility b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(ReadmeHelpers.GetAbilityName(a), ReadmeHelpers.GetAbilityName(b), StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
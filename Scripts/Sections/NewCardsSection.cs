using System.Collections.Generic;
using DiskCardGame;

namespace ReadmeMaker.Sections
{
    public class NewCardsSection : AllNewCards
    {
        public override string SectionName => "New Cards";
        public override bool Enabled => ReadmeConfig.Instance.AscensionStarterDecks;

        protected override List<CardInfo> GetCards()
        {
            List<CardInfo> allCards = base.GetCards();
            allCards.RemoveAll((a) => a.metaCategories.Contains(CardMetaCategory.Rare));
            return allCards;
        }
    }
}
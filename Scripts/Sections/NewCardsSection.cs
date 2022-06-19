using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewCardsSection : AllNewCards
    {
        public override string SectionName => "New Cards";

        protected override List<CardInfo> GetCards()
        {
            List<CardInfo> allCards = base.GetCards();
            allCards.RemoveAll((a) => a.metaCategories.Contains(CardMetaCategory.Rare));
            return allCards;
        }
    }
}
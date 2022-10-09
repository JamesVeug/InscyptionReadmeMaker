using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewCardsSection : AllNewCards
    {
        public override string SectionName => "New Cards";

        protected override List<CardInfo> GetCards(RegisteredMod mod)
        {
            List<CardInfo> allCards = base.GetCards(mod);
            allCards.RemoveAll((a) => a.metaCategories.Contains(CardMetaCategory.Rare));
            return allCards;
        }
    }
}
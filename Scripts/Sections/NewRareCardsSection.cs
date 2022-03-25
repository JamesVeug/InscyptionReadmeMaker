using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;

namespace ReadmeMaker.Sections
{
    public class NewRareCardsSection : NewCardsSection
    {
        public override string GetSectionName()
        {
            return "New Rare Cards";
        }

        protected override List<CardInfo> GetCards()
        {
            List<CardInfo> allCards = base.GetCards();
            allCards.RemoveAll((a) => !a.metaCategories.Contains(CardMetaCategory.Rare));
            return allCards;
        }
    }
}
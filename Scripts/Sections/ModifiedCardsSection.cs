using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;

namespace ReadmeMaker.Sections
{
    public class ModifiedCardsSection : NewCardsSection
    {
        public override string GetSectionName()
        {
            return "Modified Cards";
        }

        protected override List<CardInfo> GetCards()
        {
            List<CardInfo> modifiedCards = new List<CardInfo>();
	        
            if (!ReadmeConfig.Instance.ModifiedCardsShow)
            {
                return modifiedCards;
            }
	        
	        
            /*List<CardInfo> originalCards = CardManager.BaseGameCards.Concat(CardManager.NewCards).Select(x => CardLoader.Clone(x)).ToList();
            List<CardInfo> allCards = CardManager.AllCardsCopy;

            foreach (CardInfo originalCard in originalCards)
            {
                foreach (CardInfo card in allCards)
                {
                    if (card.name == originalCard.name)
                    {
                        if (card.displayedName != originalCard.displayedName)
                        {
                            modifiedCards.Add(card);
                        }
                    }
                }
            }*/
	     
            // Not supported in v2.0 at the moment
            return modifiedCards;
        }
    }
}
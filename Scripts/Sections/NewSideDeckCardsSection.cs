using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;

namespace ReadmeMaker.Sections
{
    public class NewSideDeckCardsSection : NewCardsSection
    {
        public override string GetSectionName()
        {
            return "New Side Deck Cards";
        }

        protected override List<CardInfo> GetCards()
        {
            if (!ReadmeConfig.Instance.SideDeckShow)
            {
                return new List<CardInfo>();
            }
	        
            List<CardInfo> allCards = ScriptableObjectLoader<CardInfo>.AllData;
            List<CardInfo> sideDeckCards = allCards.FindAll((a) => a.HasTrait((Trait)5103));
            sideDeckCards.Sort(SortCards);
	        
            return sideDeckCards;
        }
    }
}
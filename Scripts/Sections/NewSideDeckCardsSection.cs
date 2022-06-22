using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewSideDeckCardsSection : NewCardsSection
    {
        public override string SectionName => "New Side Deck Cards";
        public override bool Enabled => ReadmeConfig.Instance.SideDeckShow;

        protected override List<CardInfo> GetCards()
        {
            if (!ReadmeConfig.Instance.SideDeckShow)
            {
                return new List<CardInfo>();
            }
	        
            List<CardInfo> allCards = ScriptableObjectLoader<CardInfo>.AllData;
            List<CardInfo> sideDeckCards = allCards.FindAll((a) => a.HasTrait((Trait)5103));
            return sideDeckCards;
        }
    }
}
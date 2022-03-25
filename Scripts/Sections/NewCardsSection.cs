using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;

namespace ReadmeMaker.Sections
{
    public class NewCardsSection : ACardsSection
    {
        public override string GetSectionName()
        {
            return "New Cards";
        }

        protected override List<CardInfo> GetCards()
        {
            List<CardInfo> allCards = new List<CardInfo>(CardManager.NewCards);

            HashSet<string> evolutionFrozenAwayCards = new HashSet<string>();
            for (int i = 0; i < allCards.Count; i++)
            {
                CardInfo cardInfo = allCards[i];
                if (cardInfo.evolveParams != null && cardInfo.evolveParams != null)
                {
                    evolutionFrozenAwayCards.Add(cardInfo.evolveParams.evolution.name);
                }

                if (cardInfo.iceCubeParams != null && cardInfo.iceCubeParams.creatureWithin != null)
                {
                    evolutionFrozenAwayCards.Add(cardInfo.iceCubeParams.creatureWithin.name);
                }
            }

            if (!ReadmeConfig.Instance.CardShowUnobtainable)
            {
                allCards.RemoveAll((a) => a.metaCategories.Count == 0 && !evolutionFrozenAwayCards.Contains(a.name));
            }

            return allCards;
        }
    }
}
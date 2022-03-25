using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;

namespace ReadmeMaker.Sections
{
    public abstract class AllNewCards : ACardsSection
    {
        protected override List<CardInfo> GetCards()
        {
            List<CardInfo> allCards = new List<CardInfo>(CardManager.NewCards);

            HashSet<string> referencesCardNames = new HashSet<string>();
            for (int i = 0; i < allCards.Count; i++)
            {
                CardInfo cardInfo = allCards[i];
                if (cardInfo.evolveParams != null && cardInfo.evolveParams != null)
                {
                    referencesCardNames.Add(cardInfo.evolveParams.evolution.name);
                }

                if (cardInfo.iceCubeParams != null && cardInfo.iceCubeParams.creatureWithin != null)
                {
                    referencesCardNames.Add(cardInfo.iceCubeParams.creatureWithin.name);
                }

                if (cardInfo.tailParams != null && cardInfo.tailParams.tail != null)
                {
                    referencesCardNames.Add(cardInfo.tailParams.tail.name);
                }
            }

            if (!ReadmeConfig.Instance.CardShowUnobtainable)
            {
                allCards.RemoveAll((a) => a.metaCategories.Count == 0 && !referencesCardNames.Contains(a.name));
            }

            return allCards;
        }
    }
}
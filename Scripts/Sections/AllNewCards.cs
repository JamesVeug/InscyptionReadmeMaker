using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker.Sections
{
    public abstract class AllNewCards : ACardsSection
    {
        protected override List<CardInfo> GetCards(RegisteredMod mod)
        {
            List<CardInfo> allCards = new List<CardInfo>(CardManager.NewCards.Where((x)=>mod.PluginCardModPrefixes.Contains(x.GetModPrefix())));

            HashSet<string> referencesCardNames = new HashSet<string>();
            foreach (var cardInfo in allCards)
            {
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
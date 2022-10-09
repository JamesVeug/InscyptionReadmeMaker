﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Ascension;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewAscensionStarterDecksSection : ASection<StarterDeckManager.FullStarterDeck>
    {
        public override string SectionName => "New Ascension Starter Decks";
        public override bool Enabled => ReadmeConfig.Instance.AscensionStarterDecksShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(StarterDeckManager.NewDecks.Where(x=>GetGUID(x) == mod.PluginGUID));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<StarterDeckManager.FullStarterDeck>("Name", (a)=>a.Info.title),
                new TableColumn<StarterDeckManager.FullStarterDeck>("Unlock Level", (a)=>a.UnlockLevel.ToString()),
                new TableColumn<StarterDeckManager.FullStarterDeck>("Cards", GetCardNames)
            });
        }

        public override string GetGUID(StarterDeckManager.FullStarterDeck o)
        {
            string guid = o.Info.name.Substring(0, o.Info.name.LastIndexOf("_"));
            return guid;
        }

        protected override int Sort(StarterDeckManager.FullStarterDeck a, StarterDeckManager.FullStarterDeck b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.Info.title, b.Info.title, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetCardNames(StarterDeckManager.FullStarterDeck deck)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            Dictionary<string, int> cardCount = new Dictionary<string, int>();
            List<string> cardNames = deck.CardNames;
            if (ReadmeConfig.Instance.CardSigilsJoinDuplicates)
            {
                cardNames = Helpers.RemoveDuplicates(deck.CardNames, ref cardCount);
            }

            // Show all abilities one after the other
            int totalShownCards = 0;
            foreach (var cardName in cardNames)
            {
                CardInfo cardInfo = CardLoader.GetCardByName(cardName);
                if (cardInfo == null)
                {
                    Plugin.Log.LogWarning($"Could not get Card from {SectionName}: '{cardName}'");
                    continue;
                }
                
                string cardDisplayName = cardInfo.displayedName;
                if (string.IsNullOrEmpty(cardDisplayName))
                {
                    Plugin.Log.LogWarning("Card does not have display Name: '" + cardName + "'");
                    continue;
                }
                
                if (totalShownCards++ > 0)
                {
                    stringBuilder.Append(", ");
                }
                
                if (cardCount.TryGetValue(cardName, out int count) && count > 1)
                {
                    // Show all abilities, but combine duplicates into Waterborne(x2)
                    stringBuilder.Append($" {cardDisplayName}(x{count})");
                }
                else
                {
                    // Show all abilities 1 by 1 (Waterborne, Waterborne, Waterborne)
                    stringBuilder.Append($" {cardDisplayName}");
                }
            }
            
            return stringBuilder.ToString();
        }
    }
}
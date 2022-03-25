﻿using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Ascension;
using InscryptionAPI.Card;
using FullAbility = InscryptionAPI.Card.AbilityManager.FullAbility;

namespace ReadmeMaker.Sections
{
    public class NewAscensionStarterDecksSection : ASection
    {
        public override string SectionName => "Ascension Starter Decks";
        public override bool Enabled => ReadmeConfig.Instance.AscensionStarterDecks;
        
        private List<StarterDeckManager.FullStarterDeck> decks = null;
        
        public override void Initialize()
        {
            if (!ReadmeConfig.Instance.AscensionStarterDecks)
            {
                decks = new List<StarterDeckManager.FullStarterDeck>();
            }

            List<StarterDeckManager.FullStarterDeck> nodes = new List<StarterDeckManager.FullStarterDeck>(StarterDeckManager.NewDecks);
            nodes.Sort((a,b)=>String.Compare(a.Info.title, b.Info.title, StringComparison.Ordinal));
            decks = nodes;
        }

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (decks.Count > 0)
            {
                stringBuilder.Append($"- {decks.Count} {SectionName}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(decks, out headers, new TableColumn<StarterDeckManager.FullStarterDeck>[]
            {
                new TableColumn<StarterDeckManager.FullStarterDeck>("Name", (a)=>a.Info.title),
                new TableColumn<StarterDeckManager.FullStarterDeck>("Unlock Level", (a)=>a.UnlockLevel.ToString()),
                new TableColumn<StarterDeckManager.FullStarterDeck>("Cards", GetCardNames)
            });
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
            for (int i = 0; i < cardNames.Count; i++)
            {
                string cardName = cardNames[i];
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
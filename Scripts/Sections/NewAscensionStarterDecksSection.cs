using System;
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
        
        public override string GetSectionName()
        {
            return "Ascension Starter Decks";
        }

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (decks.Count > 0)
            {
                stringBuilder.Append($"- {decks.Count} {GetSectionName()}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(decks, out headers, new TableColumn<StarterDeckManager.FullStarterDeck>[]
            {
                new TableColumn<StarterDeckManager.FullStarterDeck>("Name", (a)=>a.Info.title),
                new TableColumn<StarterDeckManager.FullStarterDeck>("Unlock Level", (a)=>a.UnlockLevel.ToString()),
                new TableColumn<StarterDeckManager.FullStarterDeck>("Cards", (a)=>string.Join(",", a.CardNames))
            });
        }
    }
}
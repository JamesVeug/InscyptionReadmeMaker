using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;

namespace ReadmeMaker
{
    public static class ReadmeTableMaker
    {
        public static string Dump(List<CardInfo> allCards, List<CardInfo> cards, List<CardInfo> rareCards, List<NewAbility> abilities, List<NewSpecialAbility> specialAbilities)
        {
            StringBuilder stringBuilder = new StringBuilder();
            ReadmeDump.AppendSummary(stringBuilder, allCards, abilities, specialAbilities);

            // Cards
            if (cards.Count > 0)
            {
                // Headers
                //|Left columns|Right columns|
                BreakdownCards(cards, out var headers, out var splitCards);
                for (int i = 0; i < headers.Count; i++)
                {
                    stringBuilder.Append("|" + headers[i]);
                    if (i == headers.Count - 1)
                    {
                        stringBuilder.Append("|");
                    }
                }
                
                // Sorting types
                //|-------------|:-------------:|
                for (int i = 0; i < headers.Count; i++)
                {
                    if (i == 0)
                    {
                        stringBuilder.Append("|-" + headers[i]);
                    }
                    else
                    {
                        stringBuilder.Append("|:-:" + headers[i]);
                    }
                    
                    if (i == headers.Count - 1)
                    {
                        stringBuilder.Append("|");
                    }
                }
                
                // Cards
                //|alien|thingy|
                //|baby|other thing|
                for (int i = 0; i < cards.Count; i++)
                {
                    for (int j = 0; j < headers.Count; j++)
                    {
                        Dictionary<string, string> cardData = splitCards[i];
                        cardData.TryGetValue(headers[j], out string value);
                        string parsedValue = string.IsNullOrEmpty(value) ? "" : value;
                        stringBuilder.Append("|" + parsedValue);
                        
                        if (j == headers.Count - 1)
                        {
                            stringBuilder.Append("|");
                        }
                    }
                }
            }

            // Rare Cards

            // Sigils

            // Special Abilities

            return stringBuilder.ToString();
        }

        private static void BreakdownCards(List<CardInfo> cards, out List<string> headers, out List<Dictionary<string, string>> splitCards)
        {
            headers = null;
            splitCards = null;
        }
    }
}
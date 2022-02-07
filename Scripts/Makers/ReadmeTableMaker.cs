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
                stringBuilder.Append("\n### Cards:\n");
                BuildCardTable(cards, stringBuilder);
            }

            // Rare Cards
            if (rareCards.Count > 0)
            {
                stringBuilder.Append("\n### Rare Cards:\n");
                BuildCardTable(rareCards, stringBuilder);
            }

            // Sigils
            if (abilities.Count > 0)
            {
                stringBuilder.Append("\n### Sigils:\n");
                BuildAbilityTable(abilities, stringBuilder);
            }

            // Special Abilities
            if (specialAbilities.Count > 0)
            {
                stringBuilder.Append("\n### Special Abilities:\n");
                BuildSpecialAbilityTable(specialAbilities, stringBuilder);
            }

            return stringBuilder.ToString();
        }

        private static void BuildCardTable(List<CardInfo> cards, StringBuilder stringBuilder)
        {
            // Headers
            //|Left columns|Right columns|
            BreakdownCards(cards, out var headers, out var splitCards);
            for (int i = 0; i < headers.Count; i++)
            {
                stringBuilder.Append("|" + headers[i]);
                if (i == headers.Count - 1)
                {
                    stringBuilder.Append("|\n");
                }
            }

            // Sorting types
            //|-------------|:-------------:|
            for (int i = 0; i < headers.Count; i++)
            {
                if (i == 0)
                {
                    stringBuilder.Append("|-");
                }
                else
                {
                    stringBuilder.Append("|:-:");
                }

                if (i == headers.Count - 1)
                {
                    stringBuilder.Append("|\n");
                }
            }

            // Cards
            //|alien|thingy|
            //|baby|other thing|
            for (int i = 0; i < splitCards.Count; i++)
            {
                for (int j = 0; j < headers.Count; j++)
                {
                    Dictionary<string, string> cardData = splitCards[i];
                    cardData.TryGetValue(headers[j], out string value);
                    string parsedValue = string.IsNullOrEmpty(value) ? "" : value;
                    stringBuilder.Append("|" + parsedValue);

                    if (j == headers.Count - 1)
                    {
                        stringBuilder.Append("|\n");
                    }
                }
            }
        }

        private static void BreakdownCards(List<CardInfo> cards, out List<string> headers, out List<Dictionary<string, string>> splitCards)
        {
            headers = new List<string>()
            {
                "Name",
                "Power",
                "Health",
                "Cost",
                "Evolution",
                "Sigils",
                "Specials",
                "Traits",
                "Tribes",
            };
            
            splitCards = new List<Dictionary<string, string>>();
            for (int i = 0; i < cards.Count; i++)
            {
                CardInfo info = cards[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                splitCards.Add(data);
                
                data["Name"] = info.displayedName;
                data["Power"] = info.Attack.ToString();
                data["Health"] = info.Health.ToString();

                // Cost
                StringBuilder costBuilder = new StringBuilder();
                ReadmeDump.AppendAllCosts(info, costBuilder);
                data["Cost"] = costBuilder.ToString();
                
                // Evolution
                if (info.evolveParams != null && info.evolveParams.evolution != null)
                {
                    data["Evolution"] = info.evolveParams.evolution.displayedName;
                }
                else
                {
                    data["Evolution"] = "";
                }

                // Sigils
                StringBuilder sigilBuilder = new StringBuilder();
                for (int j = 0; j < info.abilities.Count; j++)
                {
                    if (j > 0)
                    {
                        sigilBuilder.Append(", ");
                    }
                    
                    string abilityName = AbilitiesUtil.GetInfo(info.abilities[j]).rulebookName;
                    sigilBuilder.Append($" {abilityName}");
                }
                data["Sigils"] = sigilBuilder.ToString();

                // Specials
                StringBuilder specialsBuilder = new StringBuilder();
                for (int j = 0; j < info.specialAbilities.Count; j++)
                {
                    if (j > 0)
                    {
                        specialsBuilder.Append(", ");
                    }
                    
                    string specialAbilityName = ReadmeDump.GetSpecialAbilityName(info.specialAbilities[j]);
                    if (specialAbilityName != null)
                    {
                        specialsBuilder.Append($" {specialAbilityName}");
                    }
                }
                data["Specials"] = specialsBuilder.ToString();
                
                // Traits
                StringBuilder traitsBuilder = new StringBuilder();
                for (int j = 0; j < info.traits.Count; j++)
                {
                    if (j > 0)
                    {
                        traitsBuilder.Append(", ");
                    }
                    traitsBuilder.Append(info.traits[j]);
                }
                data["Traits"] = traitsBuilder.ToString();
                
                // Tribes
                StringBuilder tribesBuilder = new StringBuilder();
                for (int j = 0; j < info.tribes.Count; j++)
                {
                    if (j > 0)
                    {
                        tribesBuilder.Append(", ");
                    }
                    tribesBuilder.Append(info.tribes[j]);
                }
                data["Tribes"] = tribesBuilder.ToString();
            }
        }

        private static void BuildAbilityTable(List<NewAbility> abilities, StringBuilder stringBuilder)
        {
            BreakdownAbilities(abilities, out var headers, out var data);
            
            // Headers
            //|Left columns|Right columns|
            for (int i = 0; i < headers.Count; i++)
            {
                stringBuilder.Append("|" + headers[i]);
                if (i == headers.Count - 1)
                {
                    stringBuilder.Append("|\n");
                }
            }

            // Sorting types
            //|-------------|:-------------:|
            for (int i = 0; i < headers.Count; i++)
            {
                stringBuilder.Append("|-");
                if (i == headers.Count - 1)
                {
                    stringBuilder.Append("|\n");
                }
            }

            // Cards
            //|alien|thingy|
            //|baby|other thing|
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < headers.Count; j++)
                {
                    Dictionary<string, string> cardData = data[i];
                    cardData.TryGetValue(headers[j], out string value);
                    string parsedValue = string.IsNullOrEmpty(value) ? "" : value;
                    stringBuilder.Append("|" + parsedValue);

                    if (j == headers.Count - 1)
                    {
                        stringBuilder.Append("|\n");
                    }
                }
            }
        }

        private static void BreakdownAbilities(List<NewAbility> cards, out List<string> headers, out List<Dictionary<string, string>> splitCards)
        {
            headers = new List<string>()
            {
                "Name",
                "Description",
            };

            splitCards = new List<Dictionary<string, string>>();
            for (int i = 0; i < cards.Count; i++)
            {
                NewAbility newAbility = cards[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                splitCards.Add(data);
                data["Name"] = ReadmeDump.GetAbilityName(newAbility);
                data["Description"] = ReadmeDump.GetAbilityDescription(newAbility);
            }
        }
        
        private static void BuildSpecialAbilityTable(List<NewSpecialAbility> abilities, StringBuilder stringBuilder)
        {
            BreakdownSpecialAbilities(abilities, out var headers, out var data);
            
            // Headers
            //|Left columns|Right columns|
            for (int i = 0; i < headers.Count; i++)
            {
                stringBuilder.Append("|" + headers[i]);
                if (i == headers.Count - 1)
                {
                    stringBuilder.Append("|\n");
                }
            }

            // Sorting types
            //|-------------|:-------------:|
            for (int i = 0; i < headers.Count; i++)
            {
                stringBuilder.Append("|-");
                if (i == headers.Count - 1)
                {
                    stringBuilder.Append("|\n");
                }
            }

            // Cards
            //|alien|thingy|
            //|baby|other thing|
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < headers.Count; j++)
                {
                    Dictionary<string, string> cardData = data[i];
                    cardData.TryGetValue(headers[j], out string value);
                    string parsedValue = string.IsNullOrEmpty(value) ? "" : value;
                    stringBuilder.Append("|" + parsedValue);

                    if (j == headers.Count - 1)
                    {
                        stringBuilder.Append("|\n");
                    }
                }
            }
        }

        private static void BreakdownSpecialAbilities(List<NewSpecialAbility> cards, out List<string> headers, out List<Dictionary<string, string>> splitCards)
        {
            headers = new List<string>()
            {
                "Name",
                "Description",
            };

            splitCards = new List<Dictionary<string, string>>();
            for (int i = 0; i < cards.Count; i++)
            {
                NewSpecialAbility newAbility = cards[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                splitCards.Add(data);
                data["Name"] = newAbility.statIconInfo.rulebookName;
                data["Description"] = newAbility.statIconInfo.rulebookDescription;
            }
        }
    }
}
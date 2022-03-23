using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using ReadmeMaker.Configs;

namespace ReadmeMaker
{
    public static class ReadmeTableMaker
    {
        public static string Dump(MakerData makerData)
        {
            StringBuilder stringBuilder = new StringBuilder();
            ReadmeDump.AppendSummary(stringBuilder, makerData);

            // Cards
            if (makerData.cards.Count > 0)
            {
                using (new HeaderScope("Cards:\n", stringBuilder, true))
                {
                    BuildCardTable(makerData.cards, stringBuilder);
                }
            }

            // Rare Cards
            if (makerData.rareCards.Count > 0)
            {
                using (new HeaderScope("Rare Cards:\n", stringBuilder, true))
                {
                    BuildCardTable(makerData.rareCards, stringBuilder);
                }
            }

            // Modified Cards
            if (makerData.modifiedCards.Count > 0)
            {
                using (new HeaderScope("Modified Cards:\n", stringBuilder, true))
                {
                    BuildCardTable(makerData.modifiedCards, stringBuilder);
                }
            }

            // Side Deck Cards
            if (makerData.sideDeckCards.Count > 0)
            {
                using (new HeaderScope("Side Deck Cards:\n", stringBuilder, true))
                {
                    BuildCardTable(makerData.sideDeckCards, stringBuilder);
                }
            }

            // Sigils
            if (makerData.abilities.Count > 0)
            {
                using (new HeaderScope("Sigils:\n", stringBuilder, true))
                {
                    BuildAbilityTable(makerData.abilities, stringBuilder);
                }
            }

            // Special Abilities
            if (makerData.specialAbilities.Count > 0)
            {
                using (new HeaderScope("Special Abilities:\n", stringBuilder, true))
                {
                    BuildSpecialAbilityTable(makerData.specialAbilities, stringBuilder);
                }
            }

            // Ascension Challenges
            if (makerData.newAscensionChallenges.Count > 0)
            {
                using (new HeaderScope("Ascension Challenges:\n", stringBuilder, true))
                {
                    BuildAscensionChallengesTable(makerData.newAscensionChallenges, stringBuilder);
                }
            }

            // Configs
            if (makerData.configs.Count > 0)
            {
                using (new HeaderScope("Configs:\n", stringBuilder, true))
                {
                    BuildConfigTable(makerData.configs, stringBuilder);
                }
            }
            return stringBuilder.ToString();
        }

        private static void BuildConfigTable(List<ConfigData> configs, StringBuilder builder)
        {
            // Headers
            //|Left columns|Right columns|
            BreakdownConfigs(configs, out var headers, out var splitConfigs);
            for (int i = 0; i < headers.Count; i++)
            {
                builder.Append("|" + headers[i]);
                if (i == headers.Count - 1)
                {
                    builder.Append("|\n");
                }
            }

            // Sorting types
            //|-------------|:-------------:|
            for (int i = 0; i < headers.Count; i++)
            {
                builder.Append("|-");
                if (i == headers.Count - 1)
                {
                    builder.Append("|\n");
                }
            }

            // Cards
            //|alien|thingy|
            //|baby|other thing|
            for (int i = 0; i < splitConfigs.Count; i++)
            {
                for (int j = 0; j < headers.Count; j++)
                {
                    Dictionary<string, string> cardData = splitConfigs[i];
                    cardData.TryGetValue(headers[j], out string value);
                    string parsedValue = string.IsNullOrEmpty(value) ? "" : value;
                    builder.Append("|" + parsedValue);

                    if (j == headers.Count - 1)
                    {
                        builder.Append("|\n");
                    }
                }
            }
        }

        private static void BreakdownConfigs(List<ConfigData> configs, out List<string> headers, out List<Dictionary<string, string>> splitConfigs)
        {
            headers = new List<string>()
            {
                "GUID",
                "Section",
                "Key",
                "Description",
            };
            RemoveHeaderIfDisabled(headers, "GUID", Plugin.ReadmeConfig.ConfigShowGUID);
            
            splitConfigs = new List<Dictionary<string, string>>();
            for (int i = 0; i < configs.Count; i++)
            {
                ConfigData config = configs[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                splitConfigs.Add(data);

                if (Plugin.ReadmeConfig.ConfigShowGUID)
                {
                    data["GUID"] = config.PluginGUID;
                }

                data["Section"] = config.Entry.Definition.Section;
                data["Key"] = config.Entry.Definition.Key;
                data["Description"] = config.Entry.Description.Description;
            }
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

        private static void RemoveHeaderIfDisabled(List<string> headerList, string header, bool enabled)
        {
            if (!enabled)
            {
                headerList.Remove(header);
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
                "Frozen Away",
                "Tail",
                "Sigils",
                "Specials",
                "Traits",
                "Tribes",
            };
            RemoveHeaderIfDisabled(headers, "Evolution", Plugin.ReadmeConfig.CardShowEvolutions);
            RemoveHeaderIfDisabled(headers, "Frozen Away", Plugin.ReadmeConfig.CardShowFrozenAway);
            RemoveHeaderIfDisabled(headers, "Tail", Plugin.ReadmeConfig.CardShowTail);
            RemoveHeaderIfDisabled(headers, "Specials", Plugin.ReadmeConfig.CardShowSpecials);
            RemoveHeaderIfDisabled(headers, "Traits", Plugin.ReadmeConfig.CardShowTraits);
            RemoveHeaderIfDisabled(headers, "Tribes", Plugin.ReadmeConfig.CardShowTribes);
            RemoveHeaderIfDisabled(headers, "Sigils", Plugin.ReadmeConfig.CardShowSigils);
            
            splitCards = new List<Dictionary<string, string>>();
            for (int i = 0; i < cards.Count; i++)
            {
                CardInfo info = cards[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                splitCards.Add(data);
                
                data["Name"] = info.displayedName;
                data["Power"] = ReadmeHelpers.GetPower(info);
                data["Health"] = ReadmeHelpers.GetHealth(info);

                // Cost
                StringBuilder costBuilder = new StringBuilder();
                ReadmeDump.AppendAllCosts(info, costBuilder);
                data["Cost"] = costBuilder.ToString();
                
                // Evolution
                if (Plugin.ReadmeConfig.CardShowEvolutions)
                {
                    if (info.evolveParams != null && info.evolveParams.evolution != null)
                    {
                        data["Evolution"] = info.evolveParams.evolution.displayedName;
                    }
                    else
                    {
                        data["Evolution"] = "";
                    }
                }
                
                // Frozen
                if (Plugin.ReadmeConfig.CardShowFrozenAway)
                {
                    if (info.iceCubeParams != null && info.iceCubeParams.creatureWithin != null)
                    {
                        data["Frozen Away"] = info.iceCubeParams.creatureWithin.displayedName;
                    }
                    else
                    {
                        data["Frozen Away"] = "";
                    }
                }
                
                // Tail
                if (Plugin.ReadmeConfig.CardShowTail)
                {
                    if (info.tailParams != null && info.tailParams.tail != null)
                    {
                        data["Tail"] = info.tailParams.tail.displayedName;
                    }
                    else
                    {
                        data["Tail"] = "";
                    }
                }

                // Sigils
                if (Plugin.ReadmeConfig.CardShowSigils)
                {
                    AppendSigils(info, data);
                }

                // Specials
                if (Plugin.ReadmeConfig.CardShowSpecials)
                {
                    StringBuilder specialsBuilder = new StringBuilder();
                    for (int j = 0; j < info.specialAbilities.Count; j++)
                    {
                        if (j > 0)
                        {
                            specialsBuilder.Append(", ");
                        }

                        string specialAbilityName = ReadmeHelpers.GetSpecialAbilityName(info.specialAbilities[j]);
                        if (specialAbilityName != null)
                        {
                            specialsBuilder.Append($" {specialAbilityName}");
                        }
                    }

                    data["Specials"] = specialsBuilder.ToString();
                }

                // Traits
                if (Plugin.ReadmeConfig.CardShowTraits)
                {
                    StringBuilder traitsBuilder = new StringBuilder();
                    for (int j = 0; j < info.traits.Count; j++)
                    {
                        if (j > 0)
                        {
                            traitsBuilder.Append(", ");
                        }

                        string traitName = ReadmeHelpers.GetTraitName(info.traits[j]);
                        traitsBuilder.Append(traitName);
                    }

                    data["Traits"] = traitsBuilder.ToString();
                }

                // Tribes
                if (Plugin.ReadmeConfig.CardShowTribes)
                {
                    StringBuilder tribesBuilder = new StringBuilder();
                    for (int j = 0; j < info.tribes.Count; j++)
                    {
                        if (j > 0)
                        {
                            tribesBuilder.Append(", ");
                        }

                        tribesBuilder.Append(ReadmeHelpers.GetTribeName(info.tribes[j]));
                    }

                    data["Tribes"] = tribesBuilder.ToString();
                }
            }
        }

        private static void AppendSigils(CardInfo info, Dictionary<string, string> data)
        {
            StringBuilder sigilBuilder = new StringBuilder();
            
            Dictionary<Ability, int> abilityCount = new Dictionary<Ability, int>();
            List<Ability> infoAbilities = info.abilities;
            if (Plugin.ReadmeConfig.CardSigilsJoinDuplicates)
            {
                infoAbilities = Helpers.RemoveDuplicates(info.abilities, ref abilityCount);
            }

            // Show all abilities one after the other
            int totalShownAbilities = 0;
            for (int i = 0; i < infoAbilities.Count; i++)
            {
                Ability ability = infoAbilities[i];
                AbilityInfo abilityInfo = ReadmeHelpers.GetAbilityInfo(ability);
                if (abilityInfo == null)
                {
                    Plugin.Log.LogWarning("Could not get Ability info for ability: '" + ability + "'");
                    continue;
                }
                
                string abilityName = abilityInfo.rulebookName;
                if (string.IsNullOrEmpty(abilityName))
                {
                    Plugin.Log.LogWarning("Ability does not have rulebookName: '" + ability + "'");
                    continue;
                }
                
                if (totalShownAbilities++ > 0)
                {
                    sigilBuilder.Append(", ");
                }
                
                if (Plugin.ReadmeConfig.CardSigilsJoinDuplicates && abilityCount.TryGetValue(ability, out int count) && count > 1)
                {
                    // Show all abilities, but combine duplicates into Waterborne(x2)
                    sigilBuilder.Append($" {abilityName}(x{count})");
                }
                else
                {
                    // Show all abilities 1 by 1 (Waterborne, Waterborne, Waterborne)
                    sigilBuilder.Append($" {abilityName}");
                }
            }
            
            data["Sigils"] = sigilBuilder.ToString();
        }

        private static void BuildAbilityTable(List<AbilityManager.FullAbility> abilities, StringBuilder stringBuilder)
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

        private static void BreakdownAbilities(List<AbilityManager.FullAbility> cards, out List<string> headers, out List<Dictionary<string, string>> splitCards)
        {
            headers = new List<string>()
            {
                "Name",
                "Description",
            };

            splitCards = new List<Dictionary<string, string>>();
            for (int i = 0; i < cards.Count; i++)
            {
                AbilityManager.FullAbility ability = cards[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                splitCards.Add(data);
                data["Name"] = ReadmeHelpers.GetAbilityName(ability);
                data["Description"] = ReadmeHelpers.GetAbilityDescription(ability);
            }
        }
        
        private static void BuildSpecialAbilityTable(List<SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility> abilities, StringBuilder stringBuilder)
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
        
        private static void BuildAscensionChallengesTable(List<AscensionChallengeInfo> abilities, StringBuilder stringBuilder)
        {
            BreakdownAscensionChallenges(abilities, out var headers, out var data);
            
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

        private static void BreakdownSpecialAbilities(List<SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility> cards, out List<string> headers, out List<Dictionary<string, string>> splitCards)
        {
            headers = new List<string>()
            {
                "Name",
                "Description",
            };

            splitCards = new List<Dictionary<string, string>>();
            for (int i = 0; i < cards.Count; i++)
            {
                SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility ability = cards[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                splitCards.Add(data);
                data["Name"] = ReadmeHelpers.GetSpecialAbilityName(ability);
                data["Description"] = ReadmeHelpers.GetSpecialAbilityDescription(ability);
            }
        }

        private static void BreakdownAscensionChallenges(List<AscensionChallengeInfo> cards, out List<string> headers, out List<Dictionary<string, string>> splitCards)
        {
            headers = new List<string>()
            {
                "Name",
                "Points",
                "Description",
            };

            splitCards = new List<Dictionary<string, string>>();
            for (int i = 0; i < cards.Count; i++)
            {
                AscensionChallengeInfo ability = cards[i];
                Dictionary<string, string> data = new Dictionary<string, string>();
                splitCards.Add(data);
                data["Name"] = ability.title;
                data["Points"] = ability.pointValue.ToString();
                data["Description"] = ability.description;
            }
        }
    }
}
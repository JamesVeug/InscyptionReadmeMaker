using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;

namespace ReadmeMaker
{
    public static class ReadmeListMaker
    {
        public static string Dump(List<CardInfo> allCards, 
	        List<CardInfo> cards, 
	        List<CardInfo> rareCards, 
	        List<CardInfo> modifiedCards, 
	        List<CardInfo> sideDeckCards, 
	        List<NewAbility> abilities, 
	        List<NewSpecialAbility> specialAbilities)
        {
            StringBuilder stringBuilder = new StringBuilder();
            ReadmeDump.AppendSummary(stringBuilder, allCards, modifiedCards, sideDeckCards, abilities, specialAbilities);

            // Cards
            if (cards.Count > 0)
            {
                stringBuilder.Append("\n### Cards:\n");
                for (int i = 0; i < cards.Count; i++)
                {
                    stringBuilder.Append(GetCardInfo(cards[i]) + "\n");
                }
            }

            // Rare Cards
            if (rareCards.Count > 0)
            {
                stringBuilder.Append("\n### Rare Cards:\n");
                for (int i = 0; i < rareCards.Count; i++)
                {
                    stringBuilder.Append(GetCardInfo(rareCards[i]) + "\n");
                }
            }

            // Modified Cards
            if (modifiedCards.Count > 0)
            {
	            stringBuilder.Append("\n### Modified Cards:\n");
	            for (int i = 0; i < modifiedCards.Count; i++)
	            {
		            stringBuilder.Append(GetCardInfo(modifiedCards[i]) + "\n");
	            }
            }

            // Side Deck Cards
            if (sideDeckCards.Count > 0)
            {
	            stringBuilder.Append("\n### Side Deck Cards:\n");
	            for (int i = 0; i < sideDeckCards.Count; i++)
	            {
		            stringBuilder.Append(GetCardInfo(sideDeckCards[i]) + "\n");
	            }
            }

            // Sigils
            if (abilities.Count > 0)
            {
                stringBuilder.Append("\n### Sigils:\n");
                for (int i = 0; i < abilities.Count; i++)
                {
                    stringBuilder.Append(GetAbilityInfo(abilities[i]) + "\n");
                }
            }

            // Special Abilities
            if (specialAbilities.Count > 0)
            {
                stringBuilder.Append("\n### Special Abilities:\n");
                for (int i = 0; i < specialAbilities.Count; i++)
                {
                    stringBuilder.Append(GetSpecialAbilityInfo(specialAbilities[i]) + "\n");
                }
            }

            return stringBuilder.ToString();
        }
        
        private static string GetSpecialAbilityInfo(NewSpecialAbility newAbility)
        {
	        return $" - **{newAbility.statIconInfo.rulebookName}** - {newAbility.statIconInfo.rulebookDescription}";
        }

        private static string GetCardInfo(CardInfo info)
        {
	        StringBuilder builder = new StringBuilder();
	        builder.Append($" - **{info.displayedName}** - ");
	        builder.Append($"{info.baseAttack},{info.baseHealth} -");

	        // Cost
	        ReadmeDump.AppendAllCosts(info, builder);

	        // Abilities
	        if (Plugin.ReadmeConfig.CardShowSigils)
	        {
		        AppendSigilInfo(info, builder);
	        }

	        // Evolution
	        if (Plugin.ReadmeConfig.CardShowEvolutions)
	        {
		        if (info.evolveParams != null && info.evolveParams.evolution != null)
		        {
			        builder.Append($" Evolves into {info.evolveParams.evolution.displayedName}");
		        }
	        }
	        
	        // Specials
	        if (Plugin.ReadmeConfig.CardShowSpecials)
	        {
		        for (int i = 0; i < info.specialAbilities.Count; i++)
		        {
			        if (i == 0)
			        {
				        builder.Append($" Specials:");
			        }
			        else
			        {
				        builder.Append($",");
			        }

			        // TODO: Do this by getting the info from the rulebook?
			        string abilityName = ReadmeDump.GetSpecialAbilityName(info.specialAbilities[i]);
			        if (abilityName != null)
			        {
				        builder.Append($" {abilityName}");
			        }

			        if (i == info.abilities.Count - 1)
			        {
				        builder.Append($".");
			        }
		        }
	        }

	        // Traits
	        if (Plugin.ReadmeConfig.CardShowTraits)
	        {
		        for (int i = 0; i < info.traits.Count; i++)
		        {
			        if (i == 0)
			        {
				        builder.Append($" Traits:");
			        }
			        else
			        {
				        builder.Append($",");
			        }

			        builder.Append($" {ReadmeDump.GetTraitName(info.traits[i])}");
			        if (i == info.traits.Count - 1)
			        {
				        builder.Append($".");
			        }
		        }
	        }

	        // Tribes
	        if (Plugin.ReadmeConfig.CardShowTribes)
	        {
		        for (int i = 0; i < info.tribes.Count; i++)
		        {
			        if (i == 0)
			        {
				        builder.Append($" Tribes:");
			        }
			        else
			        {
				        builder.Append($",");
			        }

			        builder.Append($" {info.tribes[i]}");
			        if (i == info.tribes.Count - 1)
			        {
				        builder.Append($".");
			        }
		        }
	        }

	        // End with a .
	        if (builder[builder.Length - 1] != '.')
	        {
		        builder.Append(".");
	        }
	        
	        return builder.ToString();
        }

        private static void AppendSigilInfo(CardInfo info, StringBuilder builder)
        {
	        Dictionary<Ability, int> abilityCount = new Dictionary<Ability, int>();
	        List<Ability> infoAbilities = info.abilities;
	        if (Plugin.ReadmeConfig.CardSigilsJoinDuplicates)
	        {
		        infoAbilities = Utils.RemoveDuplicates(info.abilities, ref abilityCount);
	        }
	        
	        // Show all abilities one after the other
	        for (int i = 0; i < infoAbilities.Count; i++)
	        {
		        if (i == 0)
		        {
			        builder.Append($" Sigils:");
		        }
		        else
		        {
			        builder.Append($",");
		        }

		        Ability ability = infoAbilities[i];
		        string abilityName = AbilitiesUtil.GetInfo(ability).rulebookName;
		        if (Plugin.ReadmeConfig.CardSigilsJoinDuplicates && abilityCount.TryGetValue(ability, out int count) && count > 1)
		        {
			        // Show all abilities, but combine duplicates into Waterborne(x2)
			        builder.Append($" {abilityName}(x{count}");
		        }
		        else
		        {
			        // Show all abilities 1 by 1 (Waterborne, Waterborne, Waterborne)
			        builder.Append($" {abilityName}");
		        }

		        if (i == infoAbilities.Count - 1)
		        {
			        builder.Append($".");
		        }
	        }
        }

        public static string GetAbilityInfo(NewAbility newAbility)
        {
	        string nam = ReadmeDump.GetAbilityName(newAbility);
	        string desc = ReadmeDump.GetAbilityDescription(newAbility);
	        return $" - **{nam}** - {desc}";
        }
    }
}
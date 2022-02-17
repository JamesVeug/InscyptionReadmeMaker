using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;

namespace ReadmeMaker
{
    public static class ReadmeListMaker
    {
        public static string Dump(List<CardInfo> allCards, List<CardInfo> cards, List<CardInfo> rareCards, List<NewAbility> abilities, List<NewSpecialAbility> specialAbilities)
        {
            StringBuilder stringBuilder = new StringBuilder();
            ReadmeDump.AppendSummary(stringBuilder, allCards, abilities, specialAbilities);

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
	        for (int i = 0; i < info.abilities.Count; i++)
	        {
		        if (i == 0)
		        {
			        builder.Append($" Sigils:");
		        }
		        else
		        {
			        builder.Append($",");
		        }

		        string abilityName = AbilitiesUtil.GetInfo(info.abilities[i]).rulebookName;
		        builder.Append($" {abilityName}");

		        if (i == info.abilities.Count - 1)
		        {
			        builder.Append($".");
		        }
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

			        builder.Append($" {info.traits[i]}");
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

        public static string GetAbilityInfo(NewAbility newAbility)
        {
	        string nam = ReadmeDump.GetAbilityName(newAbility);
	        string desc = ReadmeDump.GetAbilityDescription(newAbility);
	        return $" - **{nam}** - {desc}";
        }
    }
}
using System.Collections.Generic;
using System.Text;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using ReadmeMaker.Configs;

namespace ReadmeMaker
{
    public static class ReadmeListMaker
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
		            for (int i = 0; i < makerData.cards.Count; i++)
		            {
			            stringBuilder.Append(GetCardInfo(makerData.cards[i]) + "\n");
		            }
	            }
            }

            // Rare Cards
            if (makerData.rareCards.Count > 0)
            {
	            using (new HeaderScope("Rare Cards:\n", stringBuilder, true))
	            {
		            for (int i = 0; i < makerData.rareCards.Count; i++)
		            {
			            stringBuilder.Append(GetCardInfo(makerData.rareCards[i]) + "\n");
		            }
	            }
            }

            // Modified Cards
            if (makerData.modifiedCards.Count > 0)
            {
	            using (new HeaderScope("Modified Cards:\n", stringBuilder, true))
	            {
		            for (int i = 0; i < makerData.modifiedCards.Count; i++)
		            {
			            stringBuilder.Append(GetCardInfo(makerData.modifiedCards[i]) + "\n");
		            }
	            }
            }

            // Side Deck Cards
            if (makerData.sideDeckCards.Count > 0)
            {
	            using (new HeaderScope("Side Deck Cards:\n", stringBuilder, true))
	            {
		            for (int i = 0; i < makerData.sideDeckCards.Count; i++)
		            {
			            stringBuilder.Append(GetCardInfo(makerData.sideDeckCards[i]) + "\n");
		            }
	            }
            }

            // Sigils
            if (makerData.abilities.Count > 0)
            {
	            using (new HeaderScope("Sigils:\n", stringBuilder, true))
	            {
		            for (int i = 0; i < makerData.abilities.Count; i++)
		            {
			            stringBuilder.Append(GetAbilityInfo(makerData.abilities[i]) + "\n");
		            }
	            }
            }

            // Special Abilities
            if (makerData.specialAbilities.Count > 0)
            {
	            using (new HeaderScope("Special Abilities:\n", stringBuilder, true))
	            {
		            for (int i = 0; i < makerData.specialAbilities.Count; i++)
		            {
			            stringBuilder.Append(GetSpecialAbilityInfo(makerData.specialAbilities[i]) + "\n");
		            }
	            }
            }

            // Configs
            if (makerData.configs.Count > 0)
            {
	            using (new HeaderScope("Configs:\n", stringBuilder, true))
	            {
		            for (int i = 0; i < makerData.configs.Count; i++)
		            {
			            stringBuilder.Append(GetConfigInfo(makerData.configs[i]) + "\n");
		            }
	            }
            }

            return stringBuilder.ToString();
        }

        private static string GetConfigInfo(ConfigData makerDataConfig)
        {
	        if (Plugin.ReadmeConfig.ConfigShowGUID)
	        {
		        return $" - **[{makerDataConfig.PluginGUID}][{makerDataConfig.Entry.Definition.Section}][{makerDataConfig.Entry.Definition.Key}]** - {makerDataConfig.Entry.Description.Description}";
	        }
	        else
	        {
		        return $" - **[{makerDataConfig.Entry.Definition.Section}][{makerDataConfig.Entry.Definition.Key}]** - {makerDataConfig.Entry.Description.Description}";
	        }
        }

        private static string GetSpecialAbilityInfo(SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility ability)
        {
	        return $" - **{ReadmeHelpers.GetSpecialAbilityName(ability)}** - {ReadmeHelpers.GetSpecialAbilityDescription(ability)}";
        }

        private static string GetCardInfo(CardInfo info)
        {
	        StringBuilder builder = new StringBuilder();
	        builder.Append($" - **{info.displayedName}** - ");
	        builder.Append($"{ReadmeHelpers.GetPower(info)},{ReadmeHelpers.GetHealth(info)} -");

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

	        // Frozen Away
	        if (Plugin.ReadmeConfig.CardShowFrozenAway)
	        {
		        if (info.iceCubeParams != null && info.iceCubeParams.creatureWithin != null)
		        {
			        builder.Append($" Creature Within: {info.iceCubeParams.creatureWithin.displayedName}");
		        }
	        }
                
	        // Tail
	        if (Plugin.ReadmeConfig.CardShowTail)
	        {
		        if (info.tailParams != null && info.tailParams.tail != null)
		        {
			        builder.Append($" Tail: {info.tailParams.tail.displayedName}");
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
			        string abilityName = ReadmeHelpers.GetSpecialAbilityName(info.specialAbilities[i]);
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

			        builder.Append($" {ReadmeHelpers.GetTraitName(info.traits[i])}");
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

			        builder.Append($" {ReadmeHelpers.GetTribeName(info.tribes[i])}");
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
		        infoAbilities = Helpers.RemoveDuplicates(info.abilities, ref abilityCount);
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
		        AbilityInfo abilityInfo = ReadmeHelpers.GetAbilityInfo(ability);
		        if (abilityInfo == null)
		        {
			        continue;
		        }
                
		        string abilityName = abilityInfo.rulebookName;
		        if (string.IsNullOrEmpty(abilityName))
		        {
			        continue;
		        }
		        
		        if (Plugin.ReadmeConfig.CardSigilsJoinDuplicates && abilityCount.TryGetValue(ability, out int count) && count > 1)
		        {
			        // Show all abilities, but combine duplicates into Waterborne(x2)
			        builder.Append($" {abilityName}(x{count})");
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

        public static string GetAbilityInfo(AbilityManager.FullAbility ability)
        {
	        string nam = ReadmeHelpers.GetAbilityName(ability);
	        string desc = ReadmeHelpers.GetAbilityDescription(ability);
	        return $" - **{nam}** - {desc}";
        }
    }
}
﻿using System.Collections.Generic;
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
	        bool hasCost = false;
	        hasCost |= AppendCost(info.BloodCost, ReadmeDump.bloodIcon, ReadmeDump.bloodIcon0, builder);
	        hasCost |= AppendCost(info.bonesCost, ReadmeDump.boneIcon, ReadmeDump.boneIcon0, builder);
	        hasCost |= AppendCost(info.energyCost, ReadmeDump.GetEnergyIcon(), ReadmeDump.energyIcon0, builder);
	        hasCost |= AppendCost(info.gemsCost.Contains(GemType.Blue) ? 1 : 0, ReadmeDump.moxIconB, null, builder);
	        hasCost |= AppendCost(info.gemsCost.Contains(GemType.Green) ? 1 : 0, ReadmeDump.moxIconG, null, builder);
	        hasCost |= AppendCost(info.gemsCost.Contains(GemType.Orange) ? 1 : 0, ReadmeDump.moxIconO, null, builder);
	        if (!hasCost)
	        {
		        builder.Append($" Free.");
	        }

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
	        if (info.evolveParams != null && info.evolveParams.evolution != null)
	        {
		        builder.Append($" Evolves into {info.evolveParams.evolution.displayedName}");
	        }
	        
	        // Specials
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
		        string abilityName = GetSpecialAbilityName(info.specialAbilities[i]);
		        if (abilityName != null)
		        {
			        builder.Append($" {abilityName}");
		        }
		        
		        if (i == info.abilities.Count - 1)
		        {
			        builder.Append($".");
		        }
	        }
	        
	        // Traits
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
	        
	        // Traits
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

	        // End with a .
	        if (builder[builder.Length - 1] != '.')
	        {
		        builder.Append(".");
	        }
	        
	        return builder.ToString();
        }
        
        private static string GetSpecialAbilityName(SpecialTriggeredAbility ability)
        {
	        if (ability <= SpecialTriggeredAbility.NUM_ABILITIES)
	        {
		        return ability.ToString();
	        }

	        for (int i = 0; i < NewSpecialAbility.specialAbilities.Count; i++)
	        {
		        if (NewSpecialAbility.specialAbilities[i].specialTriggeredAbility == ability)
		        {
			        return NewSpecialAbility.specialAbilities[i].statIconInfo.rulebookName;
		        }
	        }

	        return null;
        }
        
        private static bool AppendCost(int cost, string icon, string numberFormat, StringBuilder builder)
        {
	        if (cost <= 0)
		        return false;

	        string formattedIcon = string.Format("<img align=\"center\" src=\"{0}\">", icon);
	        if (cost <= Plugin.ReadmeConfig.CostMinCollapseAmount)
	        {
		        // Bone Bone Bone Bone
		        for (int i = 0; i < cost; i++)
		        {
			        builder.Append($" {formattedIcon}");
		        }
	        }
	        else
	        {
		        builder.Append($" {formattedIcon}");
		        
		        string costString = cost.ToString();
		        foreach (char c in costString)
		        {
			        string formattedNumberIcon = string.Format(numberFormat, c);
			        string formattedNumber = string.Format("<img align=\"center\" src=\"{0}\">", formattedNumberIcon);
		        
		        
			        // Bone4
			        builder.Append($"{formattedNumber}");
		        }
	        }

	        return true;
        }
        
        private static string GetAbilityInfo(NewAbility newAbility)
        {
	        // Seeing "[creature]" appear in the readme looks jarring, sigil descriptions should appear exactly as they do in the rulebook for consistency
	        string desc = ParseAbilityInfo(newAbility.info.rulebookDescription);
	        return $" - **{newAbility.info.rulebookName}** - {desc}";
        }
        
        // In-game, when the rulebook description for a sigil is being displyed all instances of "[creature]" are replaced with "A card bearing this sigil".
        // We do this when generating the readme as well for the sake of consistency.
        private static string ParseAbilityInfo(string desc)
        {
	        return desc.Replace("[creature]", "A card bearing this sigil");
        }
    }
}
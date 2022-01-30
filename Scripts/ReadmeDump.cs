using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace ReadmeMaker
{
    public static class ReadmeDump
    {
	    const string bloodIcon = "https://tinyurl.com/34daekbw";
	    const string boneIcon = "https://tinyurl.com/2p86btxk";
	    const string energyIconManta = "https://tinyurl.com/yc3vhhba";
	    const string energyIconZepht = "https://tinyurl.com/24r7pve3";
	    const string energyIconEri = "https://tinyurl.com/3xxfer5f";
	    const string moxIconB = "https://tinyurl.com/mr3wd88d";
	    const string moxIconG = "https://tinyurl.com/a2b7zhmt";
	    const string moxIconO = "https://tinyurl.com/ybsfz23h";
	    
	    // TODO: Shorten somehow
	    const string bloodIcon0 = "https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_blood_{0}.png";
	    const string boneIcon0 = "https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_{0}.png";
	    const string energyIcon0 = "https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_energy_{0}.png";
	    
        public static void Dump()
        {
	        Plugin.Log.LogInfo("Generating Readme...");
	        string text = GetDumpString();

	        string path = Path.Combine(Plugin.Directory, "GENERATED_README.md");
	        File.WriteAllText(path, text);
	        
	        Plugin.Log.LogInfo("Readme Dumped to " + path);
        }

        private static string GetDumpString()
        {
	        //
	        // Initialize everything for the Summary
	        //
	        List<CardInfo> allCards = NewCard.cards.FindAll((a) => a.metaCategories.Count > 0);
	        
	        List<CardInfo> cards = allCards.FindAll((a) => !a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        cards.Sort(SortCards);
	        
	        List<CardInfo> rareCards = allCards.FindAll((a) => a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        rareCards.Sort(SortCards);

	        List<NewAbility> abilities = NewAbility.abilities;
	        abilities.Sort((a, b)=>String.Compare(a.info.rulebookName, b.info.rulebookName, StringComparison.Ordinal));
	        
	        List<NewSpecialAbility> specialAbilities = NewSpecialAbility.specialAbilities;
	        specialAbilities.Sort((a, b)=>String.Compare(a.statIconInfo.rulebookName, b.statIconInfo.rulebookName, StringComparison.Ordinal));

	        //
	        // Build string
	        //
	        
	        // Summary
	        StringBuilder stringBuilder = new StringBuilder();
	        stringBuilder.Append("### Includes:\n");
	        stringBuilder.Append($"- {allCards.Count} New Cards:\n");
	        stringBuilder.Append($"- {abilities.Count} New Sigils:\n");
	        stringBuilder.Append($"- {specialAbilities.Count} New Special Abilities:\n");

	        // Cards
	        stringBuilder.Append("\n### Cards:\n");
	        for (int i = 0; i < cards.Count; i++)
	        {
		        stringBuilder.Append(GetCardInfo(cards[i]) + "\n");
	        }

	        // Rare Cards
	        stringBuilder.Append("\n### Rare Cards:\n");
	        for (int i = 0; i < rareCards.Count; i++)
	        {
		        stringBuilder.Append(GetCardInfo(rareCards[i]) + "\n");
	        }
	        
	        // Sigils
	        stringBuilder.Append("\n### Sigils:\n");
	        for (int i = 0; i < abilities.Count; i++)
	        {
		        stringBuilder.Append(GetAbilityInfo(abilities[i]) + "\n");
	        }
	        
	        // Special Abilities
	        stringBuilder.Append("\n### Special Abilities:\n");
	        for (int i = 0; i < specialAbilities.Count; i++)
	        {
		        stringBuilder.Append(GetSpecialAbilityInfo(specialAbilities[i]) + "\n");
	        }

	        return stringBuilder.ToString();
        }

        private static int SortCards(CardInfo a, CardInfo b)
        {
	        int sorted = 0;
	        switch (Plugin.ReadmeConfig.CardSortBy)
	        {
		        case ReadmeConfig.SortByType.Cost:
			        sorted = CompareByCost(a, b); 
			        break;
		        case ReadmeConfig.SortByType.Name:
			        sorted = CompareByDisplayName(a, b); 
			        break;
	        }

	        if (!Plugin.ReadmeConfig.CardSortAscending)
	        {
		        return sorted * -1;
	        }
	        
	        return sorted;
        }

        private static int CompareByDisplayName(CardInfo a, CardInfo b)
        {
	        return String.Compare(a.displayedName.ToLower(), b.displayedName.ToLower(), StringComparison.Ordinal);
        }

        private static int CompareByCost(CardInfo a, CardInfo b)
        {
	        List<Tuple<int, int>> aCosts = GetCostType(a);
	        List<Tuple<int, int>> bCosts = GetCostType(b);

	        // Show least amount of costs at the top (Blood, Bone, Blood&Bone)
	        if (aCosts.Count != bCosts.Count)
	        {
		        return aCosts.Count - bCosts.Count;
	        }
	        
	        // Show lowest cost first (Blood, Bone, Energy)
	        for (var i = 0; i < aCosts.Count; i++)
	        {
		        Tuple<int, int> aCost = aCosts[i];
		        Tuple<int, int> bCost = bCosts[i];
		        if (aCost.Item1 != bCost.Item1)
		        {
			        return aCost.Item1 - bCost.Item1;
		        }
	        }

	        // Show lowest amounts first (1 Blood, 2 Blood)
	        for (var i = 0; i < aCosts.Count; i++)
	        {
		        Tuple<int, int> aCost = aCosts[i];
		        Tuple<int, int> bCost = bCosts[i];
		        if (aCost.Item2 != bCost.Item2)
		        {
			        return aCost.Item2 - bCost.Item2;
		        }
	        }

	        ListPool.Push(aCosts);
	        ListPool.Push(bCosts);

	        // Same Costs
	        // Default to Name
	        return CompareByDisplayName(a, b);
        }
        
        private static List<Tuple<int, int>> GetCostType(CardInfo a)
        {
	        List<Tuple<int, int>> list = ListPool.Pull<Tuple<int, int>>();
	        if (a.BloodCost > 0)
	        {
		        list.Add(new Tuple<int, int>(0, a.BloodCost));
	        }
	        if (a.bonesCost > 0)
	        {
		        list.Add(new Tuple<int, int>(1, a.bonesCost));
	        }
	        if (a.energyCost > 0)
	        {
		        list.Add(new Tuple<int, int>(2, a.energyCost));
	        }
	        if (a.gemsCost.Count > 0)
	        {
		        for (int i = 0; i < a.gemsCost.Count; i++)
		        {
			        switch (a.gemsCost[i])
			        {
				        case GemType.Green:
					        list.Add(new Tuple<int, int>(3, 1));
					        break;
				        case GemType.Orange:
					        list.Add(new Tuple<int, int>(4, 1));
					        break;
				        case GemType.Blue:
					        list.Add(new Tuple<int, int>(5, 1));
					        break;
				        default:
					        throw new ArgumentOutOfRangeException();
			        }
		        }
	        }

	        return list;
        }

	// In-game, when the rulebook description for a sigil is being displyed all instances of "[creature]" are replaced with "A card bearing this sigil".
	// We do this when generating the readme as well for the sake of consistency.
	private static string ParseAbilityInfo(string desc)
	{
		return desc.Replace("[creature]", "A card bearing this sigil");
	}

        private static string GetAbilityInfo(NewAbility newAbility)
        {
			// Seeing "[creature]" appear in the readme looks jarring, sigil descriptions should appear exactly as they do in the rulebook for consistency
			string desc = ParseAbilityInfo(newAbility.info.rulebookDescription);
	        return $" - **{newAbility.info.rulebookName}** - {desc}";
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
	        hasCost |= AppendCost(info.BloodCost, bloodIcon, bloodIcon0, builder);
	        hasCost |= AppendCost(info.bonesCost, boneIcon, boneIcon0, builder);
	        hasCost |= AppendCost(info.energyCost, GetEnergyIcon(), energyIcon0, builder);
	        hasCost |= AppendCost(info.gemsCost.Contains(GemType.Blue) ? 1 : 0, moxIconB, null, builder);
	        hasCost |= AppendCost(info.gemsCost.Contains(GemType.Green) ? 1 : 0, moxIconG, null, builder);
	        hasCost |= AppendCost(info.gemsCost.Contains(GemType.Orange) ? 1 : 0, moxIconO, null, builder);
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

        private static string GetEnergyIcon()
        {
	        switch (Plugin.ReadmeConfig.CostEnergyIconType)
	        {
		        case ReadmeConfig.EnergyCostType.Manta:
			        return energyIconManta;
		        case ReadmeConfig.EnergyCostType.Zepht:
			        return energyIconZepht;
		        case ReadmeConfig.EnergyCostType.Eri:
			        return energyIconEri;
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
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
    }
}

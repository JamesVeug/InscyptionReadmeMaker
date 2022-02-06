using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using APIPlugin;
using DiskCardGame;

namespace ReadmeMaker
{
    public static class ReadmeDump
    {
	    // URL's that point to the ReadmeMaker's GitBub.
	    // Shortened so the Readme is less likely to hit the max size.
	    public const string bloodIcon = "https://tinyurl.com/34daekbw";
	    public const string boneIcon = "https://tinyurl.com/2p86btxk";
	    public const string energyIconManta = "https://tinyurl.com/yc3vhhba";
	    public const string energyIconZepht = "https://tinyurl.com/24r7pve3";
	    public const string energyIconEri = "https://tinyurl.com/3xxfer5f";
	    public const string moxIconB = "https://tinyurl.com/mr3wd88d";
	    public const string moxIconG = "https://tinyurl.com/a2b7zhmt";
	    public const string moxIconO = "https://tinyurl.com/ybsfz23h";
	    
	    // TODO: Shorten somehow
	    public const string bloodIcon0 = "https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_blood_{0}.png";
	    public const string boneIcon0 = "https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone_{0}.png";
	    public const string energyIcon0 = "https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_energy_{0}.png";
	    
        public static void Dump()
        {
	        Plugin.Log.LogInfo("Generating Readme...");
	        string text = GetDumpString();

	        string fullPath = GetOutputFullPath();
	        Plugin.Log.LogInfo("Dumping Readme to '" + fullPath + "'");
	        
	        File.WriteAllText(fullPath, text);
	    }

        private static string GetOutputFullPath()
        {
	        string defaultPath = Path.Combine(Plugin.Directory, "GENERATED_README.md");
	        string path = Plugin.ReadmeConfig.SavePath;
	        if (string.IsNullOrEmpty(path))
	        {
		        path = defaultPath;
		        return path;
	        }
	        
	        string directory = Path.GetDirectoryName(path);
	        
	        // Create directory if it doesn't exist
	        if (!Directory.Exists(directory))
	        {
		        Directory.CreateDirectory(directory);
	        }
	        
	        // Append file name if there is none
	        if (path.IndexOf('.') < 0)
	        {
		        path = Path.Combine(path, "GENERATED_README.md");
	        }
	        
	        
	        return path;
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

	        switch (Plugin.ReadmeConfig.DisplayByType)
	        {
		        case ReadmeConfig.DisplayType.List:
			        return ReadmeListMaker.Dump(allCards, cards, rareCards, abilities, specialAbilities);
		        case ReadmeConfig.DisplayType.Table:
			        return ReadmeTableMaker.Dump(allCards, cards, rareCards, abilities, specialAbilities);
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
        }

        public static void AppendSummary(StringBuilder stringBuilder, List<CardInfo> allCards, List<NewAbility> abilities, List<NewSpecialAbility> specialAbilities)
        {
	        stringBuilder.Append("### Includes:\n");
	        if (allCards.Count > 0)
	        {
		        stringBuilder.Append($"- {allCards.Count} New Cards:\n");
	        }

	        if (abilities.Count > 0)
	        {
		        stringBuilder.Append($"- {abilities.Count} New Sigils:\n");
	        }

	        if (specialAbilities.Count > 0)
	        {
		        stringBuilder.Append($"- {specialAbilities.Count} New Special Abilities:\n");
	        }
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

		public static string GetEnergyIcon()
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

		public static void AppendAllCosts(CardInfo info, StringBuilder builder)
		{
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
		}
		
		public static bool AppendCost(int cost, string icon, string numberFormat, StringBuilder builder)
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
		
		public static string GetSpecialAbilityName(SpecialTriggeredAbility ability)
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

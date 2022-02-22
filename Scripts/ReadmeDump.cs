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
	    // List if different costs the mod supports
	    // Ordered by what will be shown. Vanilla first then Custom last
	    public static List<ACost> Costs = new List<ACost>()
	    {
		    new BloodCost(),
		    new BoneCost(),
		    new EnergyCost(),
		    new MoxBlueCost(),
		    new MoxGreenCost(),
		    new MoxOrangeCost(),
		    
		    // List Custom costs here
		    new LifeCost(),
	    };
	    
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
			// Fix Life Cost Manual
			// Ability(x2)
			// TODO: Display all Side Deck cards
			// TODO: Display all non-new Cards
			// TODO: Display modified Cards
			// TODO: Sun Cost
        
	        //
	        // Initialize everything for the Summary
	        //
	        List<CardInfo> allCards = NewCard.cards;
	        if (!Plugin.ReadmeConfig.CardShowUnobtainable)
	        {
		        allCards = allCards.FindAll((a) => a.metaCategories.Count > 0);
	        }
	        
	        List<CardInfo> modifiedCards = Plugin.ReadmeConfig.ModifiedCardsShow ? GetModifiedCards() : new List<CardInfo>();
	        modifiedCards.Sort(SortCards);

	        List<CardInfo> newCards = allCards.FindAll((a) => !a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        newCards.Sort(SortCards);
	        
	        List<CardInfo> newRareCards = allCards.FindAll((a) => a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        newRareCards.Sort(SortCards);
	        
	        List<CardInfo> sideDeckCards = Plugin.ReadmeConfig.SideDeckShow ? allCards.FindAll((a) => a.traits.Contains((Trait)5103)) : new List<CardInfo>();
	        sideDeckCards.Sort(SortCards);

	        
	        List<NewAbility> abilities = Plugin.ReadmeConfig.SigilsShow ? NewAbility.abilities : new List<NewAbility>();
	        abilities.Sort((a, b)=>String.Compare(a.info.rulebookName, b.info.rulebookName, StringComparison.Ordinal));
	        
	        List<NewSpecialAbility> specialAbilities = Plugin.ReadmeConfig.SpecialAbilitiesShow ? NewSpecialAbility.specialAbilities : new List<NewSpecialAbility>();
	        specialAbilities.Sort((a, b)=>String.Compare(a.statIconInfo.rulebookName, b.statIconInfo.rulebookName, StringComparison.Ordinal));

	        //
	        // Build string
	        //

	        switch (Plugin.ReadmeConfig.CardDisplayByType)
	        {
		        case ReadmeConfig.DisplayType.List:
			        return ReadmeListMaker.Dump(allCards, newCards, newRareCards, modifiedCards, sideDeckCards, abilities, specialAbilities);
		        case ReadmeConfig.DisplayType.Table:
			        return ReadmeTableMaker.Dump(allCards, newCards, newRareCards, modifiedCards, sideDeckCards, abilities, specialAbilities);
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
        }

        private static List<CardInfo> GetModifiedCards()
        {
	        List<CardInfo> allData = ScriptableObjectLoader<CardInfo>.AllData;
	        
	        List<CardInfo> modifiedCards = new List<CardInfo>();
	        foreach (CustomCard card in CustomCard.cards)
	        {
		        int index = allData.FindIndex((Predicate<CardInfo>)(x => x.name == card.name));
		        if (index >= 0)
		        {
			        modifiedCards.Add(allData[index]);
		        }
	        }

	        return modifiedCards;
        }

        public static void AppendSummary(StringBuilder stringBuilder, List<CardInfo> newCards, List<CardInfo> modifiedCards, List<CardInfo> sideDeckCards, List<NewAbility> abilities, List<NewSpecialAbility> specialAbilities)
        {
	        stringBuilder.Append("### Includes:\n");
	        if (newCards.Count > 0)
	        {
		        stringBuilder.Append($"- {newCards.Count} New Cards:\n");
	        }
	        
	        if (modifiedCards.Count > 0)
	        {
		        stringBuilder.Append($"- {modifiedCards.Count} Modified Cards:\n");
	        }
	        
	        if (sideDeckCards.Count > 0)
	        {
		        stringBuilder.Append($"- {sideDeckCards.Count} Side Deck Cards:\n");
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

		public static void AppendAllCosts(CardInfo info, StringBuilder builder)
		{
			bool hasCost = false;
			for (int i = 0; i < Costs.Count; i++)
			{
				hasCost |= Costs[i].AppendCost(info, builder);
			}
			
			// Add Free if we don't get a cost
			if (!hasCost)
			{
				if (Plugin.ReadmeConfig.CardDisplayByType == ReadmeConfig.DisplayType.Table)
				{
					builder.Append($"Free");
				}
				else
				{
					builder.Append($" Free.");
				}
			}
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
		
		public static string GetAbilityName(NewAbility newAbility)
		{
			return newAbility.info.rulebookName;
		}
        
		// In-game, when the rulebook description for a sigil is being displyed all instances of "[creature]" are replaced with "A card bearing this sigil".
		// We do this when generating the readme as well for the sake of consistency.
		public static string GetAbilityDescription(NewAbility newAbility)
		{
			// Seeing "[creature]" appear in the readme looks jarring, sigil descriptions should appear exactly as they do in the rulebook for consistency
			string description = newAbility.info.rulebookDescription;
			return description.Replace("[creature]", "A card bearing this sigil");
		}

		public static string GetTraitName(Trait trait)
		{
			switch (trait)
			{
				case (Trait)5103:
					return "Side Deck";
				default:
					return trait.ToString();
			}
		}
    }
}

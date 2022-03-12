using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using APIPlugin;
using BepInEx;
using BepInEx.Configuration;
using DiskCardGame;
using ReadmeMaker.Configs;
using UnityEngine;

namespace ReadmeMaker
{
    public static class ReadmeDump
    {
	    // List if different costs the mod supports
	    // Ordered by what will be shown. Vanilla first then Custom last
	    private static List<ACost> Costs = new List<ACost>()
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

	    public static void AddCustomCost(ACost cost)
	    {
		    Costs.Add(cost);
	    }

	    // Custom Traits made by mods that we want to show the name of instead of a number
	    private static Dictionary<Trait, string> TraitToName = new Dictionary<Trait, string>()
	    {
		    { (Trait)5103, "Side Deck" }
	    };

	    public static void RenameTrait(Trait trait, string traitName)
	    {
		    TraitToName[trait] = traitName;
	    }

	    // Custom Tribes made by mods that we want to show the name of instead of a number
	    private static Dictionary<Tribe, string> TribeToName = new Dictionary<Tribe, string>()
	    {
		    
	    };

	    public static void RenameTribe(Tribe trait, string traitName)
	    {
		    TribeToName[trait] = traitName;
	    }
	    
	    private static Dictionary<SpecialTriggeredAbility, string> PowerModifyingSpecials = new Dictionary<SpecialTriggeredAbility, string>()
	    {
		    { SpecialTriggeredAbility.Ant, null }, 
		    { SpecialTriggeredAbility.BellProximity, null }, 
		    { SpecialTriggeredAbility.CardsInHand, null }, 
		    { SpecialTriggeredAbility.Mirror, null }, 
			{ SpecialTriggeredAbility.Lammergeier, null }
	    };
	    
	    private static Dictionary<SpecialTriggeredAbility, string> HealthModifyingSpecials = new Dictionary<SpecialTriggeredAbility, string>()
	    {
		    { SpecialTriggeredAbility.Lammergeier, null } 
	    };

	    /// <summary>
	    /// When the ReadmeMaker shows a card that has a SpecialStatIcon that changes the Power & Health of a card, this function allows the card to show the proper name of a number for the SpecialStatIcon.
	    /// If you have a SpecialStatIcon on a card and this is not applied then it will not be shown in the Power and/or Health column on the card.
	    /// If the SpecialStatIcon has an entry in the rule book then Name can be null and the Readme Maker will use that instead.
	    /// </summary>
	    public static void RenameSpecialStatIcon(SpecialTriggeredAbility specialTriggeredAbility, string name, bool powerModifier, bool healthModifier)
	    {
		    if (powerModifier)
		    {
			    PowerModifyingSpecials[specialTriggeredAbility] = name;
		    }
		    
		    if (healthModifier)
		    {
			    HealthModifyingSpecials[specialTriggeredAbility] = name;
		    }
	    }

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
			// TODO: Fix vanilla Special abilities not using their rulebook name 
			// TODO: Support for mods to add their own names and descriptions for costs/tribes/trait... etc renames 
			
	        //
	        // Initialize everything for the Summary
	        //
	        List<CardInfo> allCards = GetAllCards();
	        Plugin.Log.LogInfo(allCards.Count + " All New Cards");

	        List<CardInfo> modifiedCards = GetModifiedCards();
	        Plugin.Log.LogInfo(modifiedCards.Count + " Modified Cards");

	        List<CardInfo> newCards = GetNewCards(allCards);
	        Plugin.Log.LogInfo(newCards.Count + " New Cards");
	        
	        List<CardInfo> newRareCards = GetNewRareCards(allCards);
	        Plugin.Log.LogInfo(newRareCards.Count + " New Rare Cards");
	        
	        List<CardInfo> sideDeckCards = GetSideDeckCards();
	        Plugin.Log.LogInfo(sideDeckCards.Count + " Side Deck Cards");

	        
	        List<NewAbility> abilities = GetNewAbilities();
	        Plugin.Log.LogInfo(abilities.Count + " New Abilities");
	        
	        List<NewSpecialAbility> specialAbilities = GetNewSpecialAbilities();
	        Plugin.Log.LogInfo(specialAbilities.Count + " New Special Abilities");
		        
	        
	        List<ConfigData> configs = GetNewConfigs();
	        Plugin.Log.LogInfo(configs.Count + " New Configs");
	        
	        //
	        // Build string
	        //

	        MakerData makerData = new MakerData()
	        {
		        allCards=allCards, 
		        cards=newCards, 
		        rareCards= newRareCards, 
		        modifiedCards=modifiedCards, 
		        sideDeckCards=sideDeckCards, 
		        abilities=abilities, 
		        specialAbilities=specialAbilities,
		        configs = configs
	        };

	        switch (Plugin.ReadmeConfig.CardDisplayByType)
	        {
		        case ReadmeConfig.DisplayType.List:
			        return ReadmeListMaker.Dump(makerData);
		        case ReadmeConfig.DisplayType.Table:
			        return ReadmeTableMaker.Dump(makerData);
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
        }

        private static List<ConfigData> GetNewConfigs()
        {
	        List<ConfigData> configDefinitions = new List<ConfigData>();
	        if (!Plugin.ReadmeConfig.ConfigSectionEnabled)
	        {
		        return configDefinitions;
	        }

	        List<string> validModGUIDS = null;
	        if (!string.IsNullOrEmpty(Plugin.ReadmeConfig.ConfigOnlyShowModGUID))
	        {
		        string[] guids = Plugin.ReadmeConfig.ConfigOnlyShowModGUID.Split(',');
		        validModGUIDS = new List<string>(guids);
	        }
	        
	        foreach (BaseUnityPlugin plugin in GameObject.FindObjectsOfType<BaseUnityPlugin>())
	        {
		        if (plugin.Config == null)
		        {
			        continue;
		        }

		        string guid = plugin.Info.Instance.Info.Metadata.GUID;
		        if (validModGUIDS != null && !validModGUIDS.Contains(guid))
		        {
			        continue;
		        }
		        
		        ConfigEntryBase[] entries = plugin.Config.GetConfigEntries();
		        foreach (ConfigEntryBase definition in entries)
		        {
			        configDefinitions.Add(new ConfigData()
			        {
				        PluginGUID = guid,
				        Entry = definition,
			        });
		        }
	        }

	        // Sort by
	        // GUID
	        // Section
	        // Key
	        configDefinitions.Sort((a, b) =>
	        {
		        int guid = String.Compare(a.PluginGUID, b.PluginGUID, StringComparison.Ordinal);
		        if (guid != 0)
		        {
			        return guid;
		        }
		        
		        int section = String.Compare(a.Entry.Definition.Section, b.Entry.Definition.Section, StringComparison.Ordinal);
		        if (section != 0)
		        {
			        return section;
		        }
		        
		        int key = String.Compare(a.Entry.Definition.Key, b.Entry.Definition.Key, StringComparison.Ordinal);
		        if (key != 0)
		        {
			        return key;
		        }

		        return 0;
	        });
	        return configDefinitions;
        }

        private static List<NewSpecialAbility> GetNewSpecialAbilities()
        {
	        if (!Plugin.ReadmeConfig.SpecialAbilitiesShow)
		        return new List<NewSpecialAbility>();
	        
		    List<NewSpecialAbility> specialAbilities = NewSpecialAbility.specialAbilities;
	        specialAbilities.RemoveAll((a) => a.statIconInfo == null || string.IsNullOrEmpty(a.statIconInfo.rulebookName));
	        specialAbilities.Sort((a, b) => String.Compare(a.statIconInfo.rulebookName, b.statIconInfo.rulebookName, StringComparison.Ordinal));
	        return specialAbilities;
        }

        private static List<CardInfo> GetAllCards()
        {
	        List<CardInfo> allCards = NewCard.cards;
	        if (!Plugin.ReadmeConfig.CardShowUnobtainable)
	        {
		        allCards = allCards.FindAll((a) => a.metaCategories.Count > 0);
	        }

	        return allCards;
        }

        private static List<CardInfo> GetNewCards(List<CardInfo> allCards)
        {
	        List<CardInfo> newCards = allCards.FindAll((a) =>
		        !a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        newCards.Sort(SortCards);
	        return newCards;
        }

        private static List<CardInfo> GetNewRareCards(List<CardInfo> allCards)
        {
	        List<CardInfo> newRareCards = allCards.FindAll((a) =>
		        a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        newRareCards.Sort(SortCards);
	        return newRareCards;
        }

        private static List<CardInfo> GetSideDeckCards()
        {
	        if (!Plugin.ReadmeConfig.SideDeckShow)
	        {
		        return new List<CardInfo>();
	        }
	        
	        List<CardInfo> allCards = ScriptableObjectLoader<CardInfo>.AllData;
	        List<CardInfo> sideDeckCards = allCards.FindAll((a) => a.HasTrait((Trait)5103));
	        sideDeckCards.Sort(SortCards);
	        return sideDeckCards;
        }

        private static List<NewAbility> GetNewAbilities()
        {
	        List<NewAbility> abilities = Plugin.ReadmeConfig.SigilsShow ? NewAbility.abilities : new List<NewAbility>();
	        abilities.RemoveAll((a) => a.info == null || string.IsNullOrEmpty(a.info.rulebookName));
	        abilities.Sort((a, b) => String.Compare(a.info.rulebookName, b.info.rulebookName, StringComparison.Ordinal));
	        return abilities;
        }

        private static List<CardInfo> GetModifiedCards()
        {
	        List<CardInfo> modifiedCards = new List<CardInfo>();
	        if (!Plugin.ReadmeConfig.ModifiedCardsShow)
	        {
		        return modifiedCards;
	        }
	     
	        List<CardInfo> allData = ScriptableObjectLoader<CardInfo>.AllData;   
	        foreach (CustomCard card in CustomCard.cards)
	        {
		        int index = allData.FindIndex((Predicate<CardInfo>)(x => x.name == card.name));
		        if (index >= 0)
		        {
			        modifiedCards.Add(allData[index]);
		        }
	        }

	        modifiedCards.Sort(SortCards);
	        return modifiedCards;
        }

        public static void AppendSummary(StringBuilder stringBuilder, MakerData makerData)
        {
	        stringBuilder.Append("### Includes:\n");
	        if (makerData.allCards.Count > 0)
	        {
		        stringBuilder.Append($"- {makerData.allCards.Count} New Cards:\n");
	        }
	        
	        if (makerData.modifiedCards.Count > 0)
	        {
		        stringBuilder.Append($"- {makerData.modifiedCards.Count} Modified Cards:\n");
	        }
	        
	        if (makerData.sideDeckCards.Count > 0)
	        {
		        stringBuilder.Append($"- {makerData.sideDeckCards.Count} Side Deck Cards:\n");
	        }

	        if (makerData.abilities.Count > 0)
	        {
		        stringBuilder.Append($"- {makerData.abilities.Count} New Sigils:\n");
	        }

	        if (makerData.specialAbilities.Count > 0)
	        {
		        stringBuilder.Append($"- {makerData.specialAbilities.Count} New Special Abilities:\n");
	        }

	        if (makerData.configs.Count > 0)
	        {
		        stringBuilder.Append($"- {makerData.configs.Count} New Config Options:\n");
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
			if (TraitToName.TryGetValue(trait, out string name))
			{
				return name;
			}
			
			return trait.ToString();
		}

		public static string GetTribeName(Tribe tribe)
		{
			if (TribeToName.TryGetValue(tribe, out string name))
			{
				return name;
			}
			
			return tribe.ToString();
		}

		public static string GetPower(CardInfo info)
		{
			string power = "";
			foreach (KeyValuePair<SpecialTriggeredAbility,string> pair in PowerModifyingSpecials)
			{
				if(info.SpecialAbilities.Contains(pair.Key))
				{
					if (!string.IsNullOrEmpty(power))
					{
						power += ", ";
					}

					if (string.IsNullOrEmpty(pair.Value))
					{
						power += GetSpecialAbilityName(pair.Key);
					}
					else
					{
						power += pair.Value;
					}
				}
			}

			if (string.IsNullOrEmpty(power))
			{
				return info.baseAttack.ToString();
			}
			else if (info.baseAttack > 0)
			{
				return power + " + " + info.baseAttack;
			}
			else
			{
				return power;
			}
		}

		public static string GetHealth(CardInfo info)
		{
			string health = "";
			foreach (KeyValuePair<SpecialTriggeredAbility,string> pair in HealthModifyingSpecials)
			{
				if(info.SpecialAbilities.Contains(pair.Key))
				{
					if (!string.IsNullOrEmpty(health))
					{
						health += ", ";
					}

					if (string.IsNullOrEmpty(pair.Value))
					{
						health += GetSpecialAbilityName(pair.Key);
					}
					else
					{
						health += pair.Value;
					}
				}
			}

			if (string.IsNullOrEmpty(health))
			{
				return info.baseHealth.ToString();
			}
			else if (info.baseHealth > 0)
			{
				return health + " + " + info.baseHealth;
			}
			else
			{
				return health;
			}
		}
    }
}

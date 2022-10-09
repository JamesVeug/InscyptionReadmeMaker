using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiskCardGame;
using JamesGames.ReadmeMaker.ExternalHelpers;
using JamesGames.ReadmeMaker.Sections;

namespace JamesGames.ReadmeMaker
{
    public static class ReadmeDump
    {
	    // List of different sections of data to show listed in order displayed in the dump
	    private static List<ISection> Sections = new List<ISection>()
	    {
		    new NewCardsSection(),
		    new ModifiedCardsSection(),
		    new NewRareCardsSection(),
		    new NewBoonsSection(),
		    new NewSideDeckCardsSection(),
		    new NewSigilsSection(),
		    new NewSpecialAbilitiesSection(),
		    new NewAscensionChallengesSection(),
		    new NewAscensionStarterDecksSection(),
		    new NewMapNodesSection(),
		    new NewEncounterSection(),
		    new NewRegionsSection(),
		    new NewTribesSection(),
		    new NewConfigsSection(),
	    };
	    
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
		    new MoneyCost(),
		    new LifeMoneyCost(),
	    };

	    public static void AddCustomCost(object cost)
	    {
		    if (cost == null)
		    {
			    Plugin.Log.LogError("Could not add Custom Cost. null not acceptable.");
			    return;
		    }
		    
		    ExternalCostReader externalSectionReader = new ExternalCostReader(cost);
		    Costs.Add(externalSectionReader);
		    Plugin.Log.LogInfo($"Added Custom Cost '{externalSectionReader.CostName}'");
	    }

	    public static void AddSection(object section)
	    {
		    if (section == null)
		    {
			    Plugin.Log.LogError("Could not add Custom Section. null not acceptable.");
			    return;
		    }

		    string guid = PluginManager.Instance.ModBeingProcess.PluginGUID;
		    ExternalSectionReader externalSectionReader = new ExternalSectionReader(section, guid);
		    Sections.Add(externalSectionReader);
		    Plugin.Log.LogInfo($"Added Custom Section '{externalSectionReader.SectionName}'");
	    }

	    public static void AddCardSection(object section)
	    {
		    if (section == null)
		    {
			    Plugin.Log.LogError("Could not add Custom Card Section. null not acceptable.");
			    return;
		    }
		    
		    string guid = PluginManager.Instance.ModBeingProcess.PluginGUID;
		    ExternalCardSectionReader externalSectionReader = new ExternalCardSectionReader(section, guid);
		    Sections.Add(externalSectionReader);
		    Plugin.Log.LogInfo($"Added Custom Card Section '{externalSectionReader.SectionName}'");
	    }

	    internal static Dictionary<SpecialTriggeredAbility, string> PowerModifyingSpecials = new Dictionary<SpecialTriggeredAbility, string>()
	    {
		    { SpecialTriggeredAbility.Ant, null }, 
		    { SpecialTriggeredAbility.BellProximity, null }, 
		    { SpecialTriggeredAbility.CardsInHand, null }, 
		    { SpecialTriggeredAbility.Mirror, null }, 
			{ SpecialTriggeredAbility.Lammergeier, null },
			{ SpecialTriggeredAbility.Ouroboros, null },
			{ SpecialTriggeredAbility.SacrificesThisTurn, null }
	    };

	    internal static Dictionary<SpecialTriggeredAbility, string> HealthModifyingSpecials = new Dictionary<SpecialTriggeredAbility, string>()
	    {
		    { SpecialTriggeredAbility.Lammergeier, null },
		    { SpecialTriggeredAbility.Ouroboros, null }
	    };

        public static void Dump()
        {
	        Plugin.Log.LogInfo("Generating Readme...");

	        foreach (RegisteredMod mod in PluginManager.Instance.RegisteredMods)
	        {
		        Plugin.Log.LogInfo("Generating Readme for " + mod.PluginName);
	        
		        string text = GetDumpString(mod);

		        string fullPath = GetOutputFullPath(mod);
		        Plugin.Log.LogInfo($"Dumping {mod.PluginName} Readme to '{fullPath}'");
		        File.WriteAllText(fullPath, text);
	        }
	        
	    }

        private static string GetOutputFullPath(RegisteredMod mod)
        {
	        string defaultPath = Path.Combine(Plugin.Directory, $"GENERATED_README_{mod.PluginName.Replace(' ', '_')}.md");
	        string path = ReadmeConfig.Instance.ReadmeMakerSavePath;
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

        private static string GetDumpString(RegisteredMod registeredMod)
        {
	        // Initialize everything for the Summary
	        foreach (ISection section in Sections)
	        {
		        if (section.Enabled)
		        {
			        section.Initialize(registeredMod);
		        }
	        }

	        // Build everything
	        switch (ReadmeConfig.Instance.GeneralDisplayType)
	        {
		        case ReadmeConfig.DisplayType.Table:
			        return ReadmeTableMaker.Dump(Sections);
		        default:
			        throw new ArgumentOutOfRangeException();
	        }
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
				if (ReadmeConfig.Instance.GeneralDisplayType == ReadmeConfig.DisplayType.Table)
				{
					builder.Append($"Free");
				}
				else
				{
					builder.Append($" Free.");
				}
			}
		}
    }
}

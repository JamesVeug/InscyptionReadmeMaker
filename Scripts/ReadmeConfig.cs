using System;
using BepInEx.Configuration;

namespace JamesGames.ReadmeMaker
{
    public class ReadmeConfig
    {
        public static ReadmeConfig Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ReadmeConfig();
                    if (!m_instance.ReadmeMakerEnabled)
                    {
                        Plugin.Log.LogInfo($"ReadmeMaker disabled in the Config so it will not generate a Readme.");
                    }
                }

                return m_instance;
            }
        }

        private static ReadmeConfig m_instance = null;

        public enum HeaderType
        {
            Label,
            Foldout,
        }
        public enum HeaderSize
        {
            Biggest,
            Bigger,
            Big,
            Small,
            Smaller,
            Smallest,
        }
        
        public enum DisplayType
        {
            Table,
        }
        
        public enum SortByType
        {
            Name,
            Cost
        }

        /*private static string DefaultModsToIgnore =
            "extraVoid.inscryption.voidSigils," +
            "extraVoid.inscryption.LifeCost," +
            "org.memez4life.inscryption.customsigils," +
            "jamesgames.inscryption.zergmod," +
            "AnthonyPython.inscryption.AnthonysSigils";

        public List<string> ModsToIgnore()
        {
            string mods = IgnoreMods;
            return new List<string>(mods.Split(','));
        }*/
        
        public bool ReadmeMakerEnabled = Plugin.Instance.Config.Bind("_ReadmeMaker", "Enabled", false, new ConfigDescription("Should the ReadmeMaker create a GeneratedReadme?", null, Array.Empty<object>())).Value;

        public HeaderType GeneralHeaderType = Plugin.Instance.Config.Bind("General", "Header Type", HeaderType.Foldout, new ConfigDescription("How should the header be shown? (Unaffected by Size)", null, Array.Empty<object>())).Value;
        public HeaderSize GeneralHeaderSize = Plugin.Instance.Config.Bind("General", "Header Size", HeaderSize.Big, new ConfigDescription("How big should the header be? (Does not work for type Foldout!", null, Array.Empty<object>())).Value;
        
        public DisplayType CardDisplayByType = Plugin.Instance.Config.Bind("Cards", "Display By", DisplayType.Table, new ConfigDescription("Changes how the cards, abilities and special abilities are displayed.", null, Array.Empty<object>())).Value;
        public SortByType CardSortBy = Plugin.Instance.Config.Bind("Cards", "Sort Type", SortByType.Name, new ConfigDescription("Changes the order that the cards will be displayed in.", null, Array.Empty<object>())).Value;
        public bool CardShowUnobtainable = Plugin.Instance.Config.Bind("Cards", "Show Unobtainable Cards", true, new ConfigDescription("Show cards that can not be added to your deck.  (Trail cards, Frozen Away Cards, Evolutions... etc)", null, Array.Empty<object>())).Value;
        public bool CardSortAscending = Plugin.Instance.Config.Bind("Cards", "Sort by Ascending", true, new ConfigDescription("True=Names will be ordered from A-Z, False=Z-A... etc", null, Array.Empty<object>())).Value;
        public int CostMinCollapseAmount = Plugin.Instance.Config.Bind("Cards", "Show Cost Min Collapse Amount", 4, new ConfigDescription("Minimum amount before costs are shown as (icon)5 instead of (icon)(icon)...etc", null, Array.Empty<object>())).Value;
        public bool CostAlignImages = Plugin.Instance.Config.Bind("Cards", "Align Cost", true, new ConfigDescription("Centers the cost of the costs. (Adds a lot of characters)", null, Array.Empty<object>())).Value;
        public bool CardShowTribes = Plugin.Instance.Config.Bind("Cards", "Show Tribes", true, new ConfigDescription("Show what Tribes each card has (Insect, Canine... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowTraits = Plugin.Instance.Config.Bind("Cards", "Show Traits", true, new ConfigDescription("Show what Traits each card has (KillSurvivors, Ant, Goat, Pelt, Terrain... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowEvolutions = Plugin.Instance.Config.Bind("Cards", "Show Evolutions", true, new ConfigDescription("Show what each card can evolve into when given Fledgling. (Wolf Cub -> Wolf, Elf Fawn -> Elf... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowFrozenAway = Plugin.Instance.Config.Bind("Cards", "Show Frozen Away", true, new ConfigDescription("Show what each card turns into when killed given the Frozen Away sigil. (Frozen Possum -> Possum... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowTail = Plugin.Instance.Config.Bind("Cards", "Show Tail", true, new ConfigDescription("Show what each card will leave behind before attacked. (Skink -> Skink Tail... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowSpecials = Plugin.Instance.Config.Bind("Cards", "Show Specials", true, new ConfigDescription("Show what each cards Special Abilities are. (Ouroboros, Mirror, CardsInHand... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowSigils = Plugin.Instance.Config.Bind("Cards", "Show Sigils", true, new ConfigDescription("Show what each cards Sigils are. (Waterborne, Fledgling... etc).", null, Array.Empty<object>())).Value;
        public bool CardSigilsJoinDuplicates = Plugin.Instance.Config.Bind("Cards", "Join duplicate Sigils", true, new ConfigDescription("If a card has 2 of the same sigil, it will show as Fledgling(x2) instead of Fledgling, Fledgling.", null, Array.Empty<object>())).Value;
        
        public bool ModifiedCardsShow = Plugin.Instance.Config.Bind("Modified Cards", "Show Modified Cards Section", true, new ConfigDescription("Show a section that lists all the cards modified.", null, Array.Empty<object>())).Value;
        
        public bool SideDeckShow = Plugin.Instance.Config.Bind("Side Deck", "Show Side Deck Section", true, new ConfigDescription("Show a section that lists all the custom side deck cards.", null, Array.Empty<object>())).Value;
        
        public bool SigilsShow = Plugin.Instance.Config.Bind("Sigils", "Show Sigils", true, new ConfigDescription("Show all new sigils listed on cards in its own section.", null, Array.Empty<object>())).Value;
        
        public bool TribesShow = Plugin.Instance.Config.Bind("Tribes", "Show Tribes", true, new ConfigDescription("Show all new tribes added in its own section.", null, Array.Empty<object>())).Value;
        public bool TribesShowGUID = Plugin.Instance.Config.Bind("Tribes", "Show GUID", false, new ConfigDescription("Show the GUID for the mod that added the tribe", null, Array.Empty<object>())).Value;
        
        public bool NodesShow = Plugin.Instance.Config.Bind("Nodes", "Show Nodes", true, new ConfigDescription("Show all new map nodes added in its own section..", null, Array.Empty<object>())).Value;
        
        public bool BoonsShow = Plugin.Instance.Config.Bind("Boons", "Show Boons", true, new ConfigDescription("Show all new Boons added in its own section..", null, Array.Empty<object>())).Value;
        
        public bool EncountersShow = Plugin.Instance.Config.Bind("Encounters", "Show Encounters", true, new ConfigDescription("Show all new encounters added in its own section..", null, Array.Empty<object>())).Value;
        
        public bool AscensionChallengesShow = Plugin.Instance.Config.Bind("Ascension", "Show Ascension Challenges", true, new ConfigDescription("Show all new challenges added for Kaycees mod.", null, Array.Empty<object>())).Value;
        public bool AscensionStarterDecks = Plugin.Instance.Config.Bind("Ascension", "Show Starter Decks", true, new ConfigDescription("Show all new starter decks for Kaycees mod.", null, Array.Empty<object>())).Value;
        
        public bool RegionsShow = Plugin.Instance.Config.Bind("Regions", "Show Regions", true, new ConfigDescription("Show all new regions.", null, Array.Empty<object>())).Value;
        
        public bool SpecialAbilitiesShow = Plugin.Instance.Config.Bind("Special Abilities", "Show Special Abilities", true, new ConfigDescription("Show all new special abilities listed on cards in its own section.", null, Array.Empty<object>())).Value;
        
        public bool ConfigSectionEnabled = Plugin.Instance.Config.Bind("Config", "Show Configs", true, new ConfigDescription("Should the Readme Maker show a section listing all the new configs added?", null, Array.Empty<object>())).Value;
        public string ConfigOnlyShowModGUID = Plugin.Instance.Config.Bind("Config", "Only Show Plugin", "", new ConfigDescription("If you only want the make to show configs from a specific Mod, put the guid of that mod here. To lsit more than 1 mod separate them with a comma. eg: \"jamesgames.inscryption.readmemaker,jamesgames.inscryption.zergmod\"", null, Array.Empty<object>())).Value;
        public bool ConfigShowGUID = Plugin.Instance.Config.Bind("Config", "Show GUID", true, new ConfigDescription("Do you want the Readme Maker to show a column showing the GUID of the mod that the config came from?", null, Array.Empty<object>())).Value;
        
        public string SavePath = Plugin.Instance.Config.Bind("Saving", "Path", "", new ConfigDescription("Where to save this location to. If blank will be same folder as ReadmeMaker.dll. See console for exact location after making a readme", null, Array.Empty<object>())).Value;

        public ReadmeConfig()
        {
            
        }
    }
}
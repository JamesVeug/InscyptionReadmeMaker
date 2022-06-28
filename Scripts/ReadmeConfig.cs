using System;
using System.Collections.Generic;
using System.Linq;
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
            GUID,
            Name,
        }
        
        public enum CardSortByType
        {
            GUID,
            Name,
            Cost,
            Power,
            Health,
        }

        private const string DefaultIgnoreByModGUIDs =
            "_jamesgames.inscryption.readmemaker," +
            "community.inscryption.patch," +
            "extraVoid.inscryption.voidSigils," +
            "extraVoid.inscryption.LifeCost," +
            "zzzzVoid.inscryption.sigil_patcher," +
            "extraVoid.inscryption.void_life_pack";

        private const string ReadmeMakerHeader = "1. Readme Maker";
        private const string GeneralHeader = "2. General";
        private const string SectionsHeader = "3. Toggle Sections";
        private const string CardsHeader = "4. Card Section Options";

        public List<string> ModsToIgnore
        {
            get
            {
                if (m_modsToIgnore == null)
                {
                    m_modsToIgnore = new List<string>(IgnoreByModGUID.Split(','));
                    m_modsToIgnore.RemoveAll(string.IsNullOrEmpty);
                    for (int i = 0; i < m_modsToIgnore.Count; i++)
                    {
                        m_modsToIgnore[i] = m_modsToIgnore[i].Trim();
                    }
                    Plugin.Log.LogInfo("FilterByModsGUID: " + m_modsToIgnore.Count);
                }

                return m_modsToIgnore;
            }
        }
        private List<string> m_modsToIgnore;

        public List<string> FilterByModsGUID
        {
            get
            {
                if (m_filterByModGUID == null)
                {
                    m_filterByModGUID = new List<string>(FilterByModGUID.Split(','));
                    m_filterByModGUID.RemoveAll(string.IsNullOrEmpty);
                    for (int i = 0; i < m_filterByModGUID.Count; i++)
                    {
                        m_filterByModGUID[i] = m_filterByModGUID[i].Trim();
                    }
                    Plugin.Log.LogInfo("FilterByModsGUID: " + m_filterByModGUID.Count);
                }

                return m_filterByModGUID;
            }
        }
        private List<string> m_filterByModGUID;
        
        public readonly bool ReadmeMakerEnabled = Bind(ReadmeMakerHeader, "Enabled", false, "Should the ReadmeMaker create a GeneratedReadme?");
        public readonly string ReadmeMakerSavePath = Bind(ReadmeMakerHeader, "Save To", "", "Where to save the generated readme to. If blank will be same folder as ReadmeMaker.dll. See console for exact location after making a readme.");
        
        public readonly HeaderType GeneralHeaderType = Bind(GeneralHeader, "Header Type", HeaderType.Foldout, "How should the header be shown? (Unaffected by Size)");
        public readonly HeaderSize GeneralHeaderSize = Bind(GeneralHeader, "Header Size", HeaderSize.Big, "How big should the header be? (Does not work for type Foldout!");
        public readonly DisplayType GeneralDisplayType = Bind(GeneralHeader, "Display By", DisplayType.Table, "Changes how the cards, abilities and special abilities are displayed.");
        public readonly SortByType GeneralSortBy = Bind(GeneralHeader, "Sort By", SortByType.Name, "Changes the order of how rows in sections are displayed.");
        public readonly bool GeneralSortAscending = Bind(GeneralHeader, "Sort by Ascending", true, "True=Names will be ordered from A-Z, False=Z-A... etc.");
        private readonly string IgnoreByModGUID = Bind(GeneralHeader, "Ignore Mod by GUID", DefaultIgnoreByModGUIDs, "Ignore mods using these guids. Separate multiple guids by a comma. Disable by leaving blank.");
        private readonly string FilterByModGUID = Bind(GeneralHeader, "Filter by Mod GUID", "", "Only cards, sigils... etc related to this mods GUID. Disable by leaving blank.");
        public readonly string FilterByJSONLoaderModPrefix = Bind(GeneralHeader, "Filter by JSONLoader Mod Prefix", "", "Show .jdlr cards with a specific Mod Prefix. Disable by leaving blank.");
        public readonly bool ShowGUIDS = Bind(GeneralHeader, "Show GUIDs", false, "Show the mod GUID for each sigils, tribes... etc.");
        
        public readonly bool BoonsShow = Bind(SectionsHeader, "Show Boons", true, "Show all new Boons added in its own section..");
        public readonly bool ModifiedCardsShow = Bind(SectionsHeader, "Show Cards Modified", true, "Show a section that lists all the cards modified.");
        public readonly bool ConfigSectionShow = Bind(SectionsHeader, "Show Configs", true, "Should the Readme Maker show a section listing all the new configs added?");
        public readonly bool EncountersShow = Bind(SectionsHeader, "Show Encounters", true, "Show all new encounters added in its own section..");
        public readonly bool AscensionChallengesShow = Bind(SectionsHeader, "Show Kaycees Mod Challenges", true, "Show all new challenges added for Kaycee's mod.");
        public readonly bool AscensionStarterDecksShow = Bind(SectionsHeader, "Show Kaycees Mod Starter Decks", true, "Show all new starter decks for Kaycee's mod.");
        public readonly bool MapNodesShow = Bind(SectionsHeader, "Show Map Nodes", true, "Show all new map nodes added in its own section..");
        public readonly bool RegionsShow = Bind(SectionsHeader, "Show Regions", true, "Show all new regions.");
        public readonly bool SideDeckShow = Bind(SectionsHeader, "Show Side Decks", true, "Show a section that lists all the custom side deck cards.");
        public readonly bool SigilsShow = Bind(SectionsHeader, "Show Sigils", true, "Show all new sigils listed on cards in its own section.");
        public readonly bool SpecialAbilitiesShow = Bind(SectionsHeader, "Show Special Abilities", true, "Show all new special abilities listed on cards in its own section.");
        public readonly bool TribesShow = Bind(SectionsHeader, "Show Tribes", true, "Show all new tribes added in its own section.");

        public readonly CardSortByType CardSortBy = Bind(CardsHeader, "Sort Type", CardSortByType.Name, "Changes the order that the cards will be displayed in.");
        public readonly bool CardShowUnobtainable = Bind(CardsHeader, "Show Unobtainable Cards", true, "Show cards that can not be added to your deck.  (Trail cards, Frozen Away Cards, Evolutions... etc)");
        public readonly bool CardSortAscending = Bind(CardsHeader, "Sort by Ascending", true, "True=Names will be ordered from A-Z, False=Z-A... etc");
        public readonly int CostMinCollapseAmount = Bind(CardsHeader, "Show Cost Min Collapse Amount", 4, "Minimum amount before costs are shown as (icon)5 instead of (icon)(icon)...etc");
        public readonly bool CostAlignImages = Bind(CardsHeader, "Align Cost", true, "Centers the cost of the costs. (Adds a lot of characters)");
        public readonly bool CardShowTribes = Bind(CardsHeader, "Show Tribes", true, "Show what Tribes each card has (Insect, Canine... etc).");
        public readonly bool CardShowTraits = Bind(CardsHeader, "Show Traits", true, "Show what Traits each card has (KillSurvivors, Ant, Goat, Pelt, Terrain... etc).");
        public readonly bool CardShowEvolutions = Bind(CardsHeader, "Show Evolutions", true, "Show what each card can evolve into when given Fledgling. (Wolf Cub -> Wolf, Elf Fawn -> Elf... etc).");
        public readonly bool CardShowFrozenAway = Bind(CardsHeader, "Show Frozen Away", true, "Show what each card turns into when killed given the Frozen Away sigil. (Frozen Possum -> Possum... etc).");
        public readonly bool CardShowTail = Bind(CardsHeader, "Show Tail", true, "Show what each card will leave behind before attacked. (Skink -> Skink Tail... etc).");
        public readonly bool CardShowSpecials = Bind(CardsHeader, "Show Specials", true, "Show what each cards Special Abilities are. (Ouroboros, Mirror, CardsInHand... etc).");
        public readonly bool CardShowSigils = Bind(CardsHeader, "Show Sigils", true, "Show what each cards Sigils are. (Waterborne, Fledgling... etc).");
        public readonly bool CardSigilsJoinDuplicates = Bind(CardsHeader, "Join duplicate Sigils", true, "If a card has 2 of the same sigil, it will show as Fledgling(x2) instead of Fledgling, Fledgling.");
        
        private static T Bind<T>(string section, string key, T defaultValue, string description)
        {
            return Plugin.Instance.Config.Bind(section, key, defaultValue, new ConfigDescription(description, null, Array.Empty<object>())).Value;
        }
    }
}
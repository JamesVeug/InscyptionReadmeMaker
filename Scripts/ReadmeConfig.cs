using System;
using System.Reflection;
using BepInEx.Configuration;

namespace ReadmeMaker
{
    public class ReadmeConfig
    {
        public enum DisplayType
        {
            Table,
            List,
        }
        
        public enum SortByType
        {
            Name,
            Cost
        }
        
        public DisplayType CardDisplayByType => Plugin.Instance.Config.Bind("Cards", "Display By", DisplayType.Table, new ConfigDescription("Changes how the cards, abilities and special abilities are displayed.", null, Array.Empty<object>())).Value;
        public SortByType CardSortBy => Plugin.Instance.Config.Bind("Cards", "Sort Type", SortByType.Cost, new ConfigDescription("Changes the order that the cards will be displayed in.", null, Array.Empty<object>())).Value;
        public bool CardShowUnobtainable => Plugin.Instance.Config.Bind("Cards", "Show Unobtainable Cards", false, new ConfigDescription("Show cards that can not be added to your deck.  (Trail cards, Frozen Away Cards, Evolutions... etc)", null, Array.Empty<object>())).Value;
        public bool CardSortAscending => Plugin.Instance.Config.Bind("Cards", "Sort by Ascending", true, new ConfigDescription("True=Names will be ordered from A-Z, False=Z-A... etc", null, Array.Empty<object>())).Value;
        public int CostMinCollapseAmount => Plugin.Instance.Config.Bind("Cards", "Show Cost Min Collapse Amount", 4, new ConfigDescription("Minimum amount before costs are shown as (icon)5 instead of (icon)(icon)...etc", null, Array.Empty<object>())).Value;
        public bool CostAlignImages => Plugin.Instance.Config.Bind("Cards", "Align Cost", true, new ConfigDescription("Centers the cost of the costs. (Adds a lot of characters)", null, Array.Empty<object>())).Value;
        public bool CardShowTribes => Plugin.Instance.Config.Bind("Cards", "Show Tribes", true, new ConfigDescription("Show what Tribes each card has (Insect, Canine... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowTraits => Plugin.Instance.Config.Bind("Cards", "Show Traits", true, new ConfigDescription("Show what Traits each card has (KillSurvivors, Ant, Goat, Pelt, Terrain... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowEvolutions => Plugin.Instance.Config.Bind("Cards", "Show Evolutions", true, new ConfigDescription("Show what each card can evolve into when given Fledgling. (Wolf Cub -> Wolf, Elf Fawn -> Elf... etc).", null, Array.Empty<object>())).Value;
        public bool CardShowSpecials => Plugin.Instance.Config.Bind("Cards", "Show Specials", true, new ConfigDescription("Show what each cards Special Abilities are. (Ouroboros, Mirror, CardsInHand... etc).", null, Array.Empty<object>())).Value;
        
        public bool SigilsShow => Plugin.Instance.Config.Bind("Sigils", "Show Sigils", true, new ConfigDescription("Show all new sigils listed on cards in its own section.", null, Array.Empty<object>())).Value;
        
        public bool SpecialAbilitiesShow => Plugin.Instance.Config.Bind("Special Abilities", "Show Special Abilities", true, new ConfigDescription("Show all new special abilities listed on cards in its own section.", null, Array.Empty<object>())).Value;
        
        public string SavePath => Plugin.Instance.Config.Bind("Saving", "Path", "", new ConfigDescription("Where to save this location to. If blank will be same folder as ReadmeMaker.dll. See console for exact location after making a readme", null, Array.Empty<object>())).Value;

        public ReadmeConfig()
        {
            // Initialize Config
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                bool flag = propertyInfo.DeclaringType == typeof(Plugin);
                if (flag)
                {
                    propertyInfo.GetValue(this, null);
                }
            }
        }
    }
}
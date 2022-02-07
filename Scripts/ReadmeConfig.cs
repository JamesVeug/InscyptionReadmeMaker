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
        
        public enum EnergyCostType
        {
            Manta,
            Zepht,
            Eri
        }
        
        public DisplayType DisplayByType => Plugin.Instance.Config.Bind("Display By", "Type", DisplayType.Table, new ConfigDescription("Changes how the cards, abilities and special abilities are displayed.", null, Array.Empty<object>())).Value;
        public SortByType CardSortBy => Plugin.Instance.Config.Bind("Card Sorting", "Sort Type", SortByType.Cost, new ConfigDescription("Changes the order that the cards will be displayed in.", null, Array.Empty<object>())).Value;
        public bool CardSortAscending => Plugin.Instance.Config.Bind("Card Sorting", "Ascending Order", true, new ConfigDescription("True=Names will be ordered from A-Z, False=Z-A... etc", null, Array.Empty<object>())).Value;
        public EnergyCostType CostEnergyIconType => Plugin.Instance.Config.Bind("Card Cost", "Display Type", EnergyCostType.Manta, new ConfigDescription("Different icon styles", null, Array.Empty<object>())).Value;
        public int CostMinCollapseAmount => Plugin.Instance.Config.Bind("Card Cost", "Min Collapse Amount", 4, new ConfigDescription("Minimum amount before costs are shown as (icon)5 instead of (icon)(icon)...etc", null, Array.Empty<object>())).Value;
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
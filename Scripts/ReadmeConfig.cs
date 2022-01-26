using System;
using System.Reflection;
using BepInEx.Configuration;

namespace ReadmeMaker
{
    public class ReadmeConfig
    {
        public enum SortByType
        {
            Name,
            Cost
        }
        
        public enum EnergyCostType
        {
            Manta,
            Zepht
        }
        
        public SortByType CardSortBy => Plugin.Instance.Config.Bind("Card Sorting", "Sort Type", SortByType.Name, new ConfigDescription("Changes the order that the cards will be displayed in.", null, Array.Empty<object>())).Value;
        public bool CardSortAscending => Plugin.Instance.Config.Bind("Card Sorting", "Ascending Order", true, new ConfigDescription("True=Names will be ordered from A-Z, False=Z-A... etc", null, Array.Empty<object>())).Value;
        public EnergyCostType CostEnergyIconType => Plugin.Instance.Config.Bind("Card Cost", "Display Type", EnergyCostType.Manta, new ConfigDescription("Different icon styles", null, Array.Empty<object>())).Value;
        public int CostMinCollapseAmount => Plugin.Instance.Config.Bind("Card Cost", "Min Collapse Amount", 4, new ConfigDescription("Minimum amount before costs are shown as (icon)5 instead of (icon)(icon)...etc", null, Array.Empty<object>())).Value;

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
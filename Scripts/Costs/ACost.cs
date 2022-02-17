using System.Collections.Generic;
using System.Text;
using DiskCardGame;

namespace ReadmeMaker
{
    public abstract class ACost
    {
        // Most optimal way to add a cost
        protected Dictionary<int, string> CostToSingleImage = null; // 2 -> image of 2 x blood, 5 -> image of 5 x blood... etc
        
        // We don't have an image for X amount. So we need to build it separately
        // We don't want to do this really because it makes the readme a lot bigger
        protected string CustomIconX = null; // x
        protected Dictionary<int, string> IntToImage = null; // 1, 2, 3
        protected string CostName = null; // Blood
        
        public abstract int GetCost(CardInfo cardInfo);
        
        public bool AppendCost(CardInfo cardInfo, StringBuilder builder)
        {
            int cost = GetCost(cardInfo);
            if (cost <= 0)
                return false;

            // Try displaying a single image
            if (CostToSingleImage.TryGetValue(cost, out string path))
            {
                // Blood x5
                string fullImage = FormatUrl(path);
                builder.Append(" " + fullImage);
                return true;
            }

            CostToSingleImage.TryGetValue(1, out string singleIconPath);
            
            // No way to show the icon... so just show the name
            if (string.IsNullOrEmpty(singleIconPath))
            {
                builder.Append(CostName);
            }
            
            // Try displaying multiple icons
            if (cost <= Plugin.ReadmeConfig.CostMinCollapseAmount)
            {
                ShowMultipleIcons(cost, builder);
            }

            //
            // Try showing blood x 13
            //
            
            // Make sure we have images for all numbers 
            string costString = cost.ToString();
            bool canShowNumbers = true;
            foreach (char c in costString)
            {
                int numberValue = c;
                if (!IntToImage.ContainsKey(numberValue))
                {
                    canShowNumbers = false;
                    Plugin.Log.LogWarning($"Cost '{CostName}' is missing an image for number {numberValue}!");
                    // Do not break. We want to see logs of all missing numbers
                }
            }

            if (canShowNumbers)
            {
                string formattedIcon = FormatUrl(singleIconPath);
                
                // Blood
                builder.Append(formattedIcon);

                // x
                if (!string.IsNullOrEmpty(CustomIconX))
                {
                    string formattedX = FormatUrl(CustomIconX);
                    builder.Append(formattedX);
                }
                
                // 13
                foreach (char c in costString)
                {
                    int numberValue = c;
                    string formattedNumberIcon = IntToImage[numberValue];
                    string formattedNumber = FormatUrl(formattedNumberIcon);


                    // 1 or 3
                    builder.Append(formattedNumber);
                }
            }
            else
            {
                // We are missing numbers. So just show 'blood blood blood... etc'
                ShowMultipleIcons(cost, builder);
            }


            return true;
        }

        private string FormatUrl(string url)
        {
            if (Plugin.ReadmeConfig.CostAlignImages)
            {
                return string.Format("<img align=\"center\" src=\"{0}\">", url);
            }
            else
            {
                return string.Format("<img src=\"{0}\">", url);
            }
        }

        private void ShowMultipleIcons(int cost, StringBuilder builder)
        {
            CostToSingleImage.TryGetValue(1, out string singleIconPath);
            string formattedIcon = FormatUrl(singleIconPath);
            
            // Bone Bone Bone Bone
            for (int i = 0; i < cost; i++)
            {
                builder.Append($" {formattedIcon}");
            }
        }
    }
}
using System.Collections.Generic;
using DiskCardGame;

namespace ReadmeMaker
{
    public class BoneCost : ACost
    {
        public BoneCost()
        {
            CostName = "Bone";
            CustomIconX = "https://tinyurl.com/2p8e9tz5";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://tinyurl.com/yd7pkf8k" },
                { 1, "https://tinyurl.com/3e6vmy8w" },
                { 2, "https://tinyurl.com/3bmsh9ax" },
                { 3, "https://tinyurl.com/ynxxamy7" },
                { 4, "https://tinyurl.com/33n7er5c" },
                { 5, "https://tinyurl.com/4nhcutsf" },
                { 6, "https://tinyurl.com/r5n49kst" },
                { 7, "https://tinyurl.com/2s3pwt9d" },
                { 8, "https://tinyurl.com/2p9vfuk4" },
                { 9, "https://tinyurl.com/bdecxmby" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://tinyurl.com/ycku3kab" },
                { 2, "https://tinyurl.com/kt3f8tnw" },
                { 3, "https://tinyurl.com/mta2ah9y" },
                { 4, "https://tinyurl.com/276rt5yx" },
                { 5, "https://tinyurl.com/2p97kn33" },
                { 6, "https://tinyurl.com/2p8wpx7f" },
                { 7, "https://tinyurl.com/58ksh7sk" },
                { 8, "https://tinyurl.com/mv6tnc5e" },
                { 9, "https://tinyurl.com/ks2sbsr3" },
                { 10, "https://tinyurl.com/3usw5tn2" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.bonesCost;
        }
    }
}
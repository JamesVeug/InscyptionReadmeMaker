using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker
{
    public class LifeCost : ACost
    {
        public LifeCost()
        {            
            CostName = "Life";
            CustomIconX = "https://tinyurl.com/2p96t6x5";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://tinyurl.com/2p9ynesd" },
                { 1, "https://tinyurl.com/5n78f946" },
                { 2, "https://tinyurl.com/4fku2vzv" },
                { 3, "https://tinyurl.com/ythkkk4n" },
                { 4, "https://tinyurl.com/2p9heenz" },
                { 5, "https://tinyurl.com/3wckkxrz" },
                { 6, "https://tinyurl.com/5aw5eb4x" },
                { 7, "https://tinyurl.com/ynfw44zt" },
                { 8, "https://tinyurl.com/32y52ab5" },
                { 9, "https://tinyurl.com/54c26ss5" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://tinyurl.com/kx98nhby" },
                { 2, "https://tinyurl.com/2p8fwa8w" },
                { 3, "https://tinyurl.com/yr33696a" },
                { 4, "https://tinyurl.com/mr7n8jz7" },
                { 5, "https://tinyurl.com/36ter6y5" },
                { 6, "https://tinyurl.com/4uaa7znv" },
                { 7, "https://tinyurl.com/5efbr2de" },
                { 8, "https://tinyurl.com/3mdm2ufs" },
                { 9, "https://tinyurl.com/zekh2x5m" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            int? cost = cardInfo.GetExtendedPropertyAsInt("LifeCost");
            if (!cost.HasValue)
            {
                return 0;
            }
            
            return cost.Value;
        }
    }
}
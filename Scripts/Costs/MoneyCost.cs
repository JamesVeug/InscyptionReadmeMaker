using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker
{
    public class MoneyCost : ACost
    {
        public MoneyCost()
        {            
            CostName = "Money";
            CustomIconX = "https://tinyurl.com/bdefpyvb";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://tinyurl.com/5n7bzaru" },
                { 1, "https://tinyurl.com/bp5rudyv" },
                { 2, "https://tinyurl.com/mrxmjbnm" },
                { 3, "https://tinyurl.com/4z46mdjd" },
                { 4, "https://tinyurl.com/y2crc4ds" },
                { 5, "https://tinyurl.com/yc6jknuc" },
                { 6, "https://tinyurl.com/2p84ffvs" },
                { 7, "https://tinyurl.com/2p9y2swm" },
                { 8, "https://tinyurl.com/mryu7ntj" },
                { 9, "https://tinyurl.com/2j83adb4" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://tinyurl.com/2p986twn" },
                { 2, "https://tinyurl.com/d9bhc4uy" },
                { 3, "https://tinyurl.com/2sm7zw6n" },
                { 4, "https://tinyurl.com/mrpawmry" },
                { 5, "https://tinyurl.com/9xafycb8" },
                { 6, "https://tinyurl.com/23s3pe5t" },
                { 7, "https://tinyurl.com/bdeh47uf" },
                { 8, "https://tinyurl.com/2p8sf5me" },
                { 9, "https://tinyurl.com/2p8kum23" },
                { 10, "https://tinyurl.com/5f8yptte" },
                { 11, "https://tinyurl.com/24sfrazc" },
                { 12, "https://tinyurl.com/y82nvsn7" },
                { 13, "https://tinyurl.com/cd69fv6w" },
                { 14, "https://tinyurl.com/29drbhv2" },
                { 15, "https://tinyurl.com/ms8sajyz" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            int? cost = cardInfo.GetExtendedPropertyAsInt("MoneyCost");
            if (!cost.HasValue)
            {
                return 0;
            }
            
            return cost.Value;
        }
    }
}
using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class BloodCost : ACost
    {
        public BloodCost()
        {
            CostName = "Blood";
            CustomIconX = "https://tinyurl.com/2s726mms";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://tinyurl.com/586zzmeh" },
                { 1, "https://tinyurl.com/3j755fu6" },
                { 2, "https://tinyurl.com/2p9c2bc3" },
                { 3, "https://tinyurl.com/59yt7dcc" },
                { 4, "https://tinyurl.com/4dab6vnz" },
                { 5, "https://tinyurl.com/v3pd9uen" },
                { 6, "https://tinyurl.com/2p8fj8ef" },
                { 7, "https://tinyurl.com/4hc573as" },
                { 8, "https://tinyurl.com/bdcm5tvc" },
                { 9, "https://tinyurl.com/mr2rjb3j" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://tinyurl.com/2p8ev3yj" },
                { 2, "https://tinyurl.com/42jumw7s" },
                { 3, "https://tinyurl.com/yrt8hwxr" },
                { 4, "https://tinyurl.com/49mpbsvc" },
                { 5, "https://tinyurl.com/2wsnr9nb" },
                { 6, "https://tinyurl.com/2p9d2vrh" },
                { 7, "https://tinyurl.com/yckjya8c" },
                { 8, "https://tinyurl.com/2bpf2ds8" },
                { 9, "https://tinyurl.com/mt6xrxn2" },
                { 10, "https://tinyurl.com/2p8u6dzy" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.BloodCost;
        }
    }
}
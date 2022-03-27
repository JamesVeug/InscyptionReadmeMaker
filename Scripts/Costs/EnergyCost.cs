using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class EnergyCost : ACost
    {
        public EnergyCost()
        {
            CostName = "Energy";
            CustomIconX = "https://tinyurl.com/2p86hx5b";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://tinyurl.com/278vzx25" },
                { 1, "https://tinyurl.com/4bnet92w" },
                { 2, "https://tinyurl.com/mr2py8aj" },
                { 3, "https://tinyurl.com/mrjjdbn9" },
                { 4, "https://tinyurl.com/2p9ebbrf" },
                { 5, "https://tinyurl.com/y6236drz" },
                { 6, "https://tinyurl.com/339r9r4e" },
                { 7, "https://tinyurl.com/yc3uftuu" },
                { 8, "https://tinyurl.com/2p9heput" },
                { 9, "https://tinyurl.com/7bfxbuxw" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://tinyurl.com/yckvkw24" },
                { 2, "https://tinyurl.com/2p8dub8y" },
                { 3, "https://tinyurl.com/2p8r86p2" },
                { 4, "https://tinyurl.com/45hh3s44" },
                { 5, "https://tinyurl.com/ycyt8r2t" },
                { 6, "https://tinyurl.com/ytvkwtdd" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.energyCost;
        }
    }
}
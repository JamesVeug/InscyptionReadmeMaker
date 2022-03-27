using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class MoxOrangeCost : ACost
    {
        public MoxOrangeCost()
        {
            CostName = "Orange Mox";
            CustomIconX = "";
            IntToImage = new Dictionary<int, string>() {};
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://tinyurl.com/ybsfz23h" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.gemsCost.Contains(GemType.Orange) ? 1 : 0;
        }
    }
}
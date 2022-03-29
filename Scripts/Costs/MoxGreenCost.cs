using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class MoxGreenCost : ACost
    {
        public MoxGreenCost()
        {
            CostName = "Green Mox";
            CustomIconX = "";
            IntToImage = new Dictionary<int, string>() {};
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://i.imgur.com/cVoD0p2.png" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.gemsCost.Contains(GemType.Green) ? 1 : 0;
        }
    }
}
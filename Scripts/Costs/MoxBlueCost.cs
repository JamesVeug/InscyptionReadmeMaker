using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class MoxBlueCost : ACost
    {
        public MoxBlueCost()
        {
            CostName = "Blue Mox";
            CustomIconX = "";
            IntToImage = new Dictionary<int, string>() {};
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://i.imgur.com/VohLYjY.png" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.gemsCost.Contains(GemType.Blue) ? 1 : 0;
        }
    }
}
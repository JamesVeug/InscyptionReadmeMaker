using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class EnergyCost : ACost
    {
        public EnergyCost()
        {
            CostName = "Energy";
            CustomIconX = "https://i.imgur.com/hox8zlk.png";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://i.imgur.com/3mXHK5K.png" },
                { 1, "https://i.imgur.com/NcdGqIZ.png" },
                { 2, "https://i.imgur.com/3ngvEdK.png" },
                { 3, "https://i.imgur.com/Aem0MCG.png" },
                { 4, "https://i.imgur.com/P1yr67p.png" },
                { 5, "https://i.imgur.com/qeakSS4.png" },
                { 6, "https://i.imgur.com/lgHEMp9.png" },
                { 7, "https://i.imgur.com/h6f8Y6a.png" },
                { 8, "https://i.imgur.com/WcDo2N1.png" },
                { 9, "https://i.imgur.com/81nqsA4.png" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://i.imgur.com/C22peXt.png" },
                { 2, "https://i.imgur.com/vUBgPOO.png" },
                { 3, "https://i.imgur.com/9tZzgbv.png" },
                { 4, "https://i.imgur.com/OYmdUg3.png" },
                { 5, "https://i.imgur.com/JmoIiwV.png" },
                { 6, "https://i.imgur.com/mBwmFpx.png" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.energyCost;
        }
    }
}
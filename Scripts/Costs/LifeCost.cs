using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker
{
    public class LifeCost : ACost
    {
        public LifeCost()
        {            
            CostName = "Life";
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
                { 1, "https://i.imgur.com/ewWAXzT.png" },
                { 2, "https://i.imgur.com/0Lbz2Sv.png" },
                { 3, "https://i.imgur.com/9qF5brY.png" },
                { 4, "https://i.imgur.com/ajsynkV.png" },
                { 5, "https://i.imgur.com/ywMATqz.png" },
                { 6, "https://i.imgur.com/iuXQy4f.png" },
                { 7, "https://i.imgur.com/L9YyzSE.png" },
                { 8, "https://i.imgur.com/CGFrK9s.png" },
                { 9, "https://i.imgur.com/dd2p9an.png" },
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
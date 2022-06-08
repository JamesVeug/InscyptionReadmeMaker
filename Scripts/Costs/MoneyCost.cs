using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker
{
    public class MoneyCost : ACost
    {
        public MoneyCost()
        {            
            CostName = "Money";
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
                { 1,  "https://i.imgur.com/AvSFjqE.png" },
                { 2,  "https://i.imgur.com/XkjzNGs.png" },
                { 3,  "https://i.imgur.com/CFUOyGF.png" },
                { 4,  "https://i.imgur.com/XycYXqJ.png" },
                { 5,  "https://i.imgur.com/FZhTjPv.png" },
                { 6,  "https://i.imgur.com/yntWPHa.png" },
                { 7,  "https://i.imgur.com/p0gd8tq.png" },
                { 8,  "https://i.imgur.com/4KPbD9s.png" },
                { 9,  "https://i.imgur.com/2NE54Qt.png" },
                { 10, "https://i.imgur.com/c9Lqcqs.png" },
                { 11, "https://i.imgur.com/7upNyss.png" },
                { 12, "https://i.imgur.com/KLYdE3U.png" },
                { 13, "https://i.imgur.com/LfECumH.png" },
                { 14, "https://i.imgur.com/0dwwqNf.png" },
                { 15, "https://i.imgur.com/DQ4t4PQ.png" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            int? cost = cardInfo.GetExtendedPropertyAsInt("MoneyCost");
            return cost ?? 0;
        }
    }
}
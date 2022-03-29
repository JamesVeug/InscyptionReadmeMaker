using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class BloodCost : ACost
    {
        public BloodCost()
        {
            CostName = "Blood";
            CustomIconX = "https://i.imgur.com/3L8GdcW.png";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://i.imgur.com/Vj074IX.png" },
                { 1, "https://i.imgur.com/UENa3ep.png" },
                { 2, "https://i.imgur.com/vIrzRRC.png" },
                { 3, "https://i.imgur.com/nR7Ce9J.png" },
                { 4, "https://i.imgur.com/1c6PTpq.png" },
                { 5, "https://i.imgur.com/LtYqiWy.png" },
                { 6, "https://i.imgur.com/37hwVbk.png" },
                { 7, "https://i.imgur.com/E1oVYzb.png" },
                { 8, "https://i.imgur.com/VHffwIV.png" },
                { 9, "https://i.imgur.com/mf6YK1A.png" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://i.imgur.com/H6vESv7.png" },
                { 2, "https://i.imgur.com/62GUUAC.png" },
                { 3, "https://i.imgur.com/Ckvc6Ww.png" },
                { 4, "https://i.imgur.com/8SvThbo.png" },
                { 5, "https://i.imgur.com/MRVtKXQ.png" },
                { 6, "https://i.imgur.com/uZQCJoi.png" },
                { 7, "https://i.imgur.com/px7LIxe.png" },
                { 8, "https://i.imgur.com/SyQnVu6.png" },
                { 9, "https://i.imgur.com/8oNXh3q.png" },
                { 10,"https://i.imgur.com/uSrb4Rx.png" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.BloodCost;
        }
    }
}
using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class BoneCost : ACost
    {
        public BoneCost()
        {
            CostName = "Bone";
            CustomIconX = "https://i.imgur.com/UMfuFFS.png";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://i.imgur.com/puwP3dD.png" },
                { 1, "https://i.imgur.com/g6cUUvP.png" },
                { 2, "https://i.imgur.com/czecyiH.png" },
                { 3, "https://i.imgur.com/jnK5NEz.png" },
                { 4, "https://i.imgur.com/iJN52Ow.png" },
                { 5, "https://i.imgur.com/o1qsSmA.png" },
                { 6, "https://i.imgur.com/r1Q62Ck.png" },
                { 7, "https://i.imgur.com/mKxovtH.png" },
                { 8, "https://i.imgur.com/cEvPoTk.png" },
                { 9, "https://i.imgur.com/Vh7xWuE.png" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1, "https://i.imgur.com/GeMgIce.png" },
                { 2, "https://i.imgur.com/beJhD7d.png" },
                { 3, "https://i.imgur.com/XmTnHld.png" },
                { 4, "https://i.imgur.com/UvtK0PY.png" },
                { 5, "https://i.imgur.com/i9oPLUJ.png" },
                { 6, "https://i.imgur.com/5EnVPo0.png" },
                { 7, "https://i.imgur.com/azfV5m8.png" },
                { 8, "https://i.imgur.com/66XMPEU.png" },
                { 9, "https://i.imgur.com/aH4bIq4.png" },
                { 10,"https://i.imgur.com/ogwt58w.png" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            return cardInfo.bonesCost;
        }
    }
}
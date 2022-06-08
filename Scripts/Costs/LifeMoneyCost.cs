using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker
{
    public class LifeMoneyCost : ACost
    {
        public LifeMoneyCost()
        {            
            CostName = "Life Money";
            CustomIconX = "https://i.imgur.com/63lODFb.png";
            IntToImage = new Dictionary<int, string>()
            {
                { 0, "https://i.imgur.com/JKvHkH1.png" },
                { 1, "https://i.imgur.com/5SViWhU.png" },
                { 2, "https://i.imgur.com/sdY2lUk.png" },
                { 3, "https://i.imgur.com/qBs0skf.png" },
                { 4, "https://i.imgur.com/m0xub69.png" },
                { 5, "https://i.imgur.com/mL7JptS.png" },
                { 6, "https://i.imgur.com/92cWR6a.png" },
                { 7, "https://i.imgur.com/oAowKf6.png" },
                { 8, "https://i.imgur.com/hOC0PPO.png" },
                { 9, "https://i.imgur.com/rS8gnnt.png" },
            };
            CostToSingleImage = new Dictionary<int, string>()
            {
                { 1,  "https://i.imgur.com/K8gMLn8.png" },
                { 2,  "https://i.imgur.com/bkbirSk.png" },
                { 3,  "https://i.imgur.com/KMixuDU.png" },
                { 4,  "https://i.imgur.com/1DIS76C.png" },
                { 5,  "https://i.imgur.com/HAYwiry.png" },
                { 6,  "https://i.imgur.com/MHr0jId.png" },
                { 7,  "https://i.imgur.com/mZNicS1.png" },
                { 8,  "https://i.imgur.com/pERajGP.png" },
                { 9,  "https://i.imgur.com/rZRwKuA.png" },
                { 10, "https://i.imgur.com/z6E6qNj.png" },
                { 11, "https://i.imgur.com/CArixaj.png" },
                { 12, "https://i.imgur.com/7xZyKSB.png" },
                { 13, "https://i.imgur.com/Ugzm0tk.png" },
                { 14, "https://i.imgur.com/Pdwt3EG.png" },
                { 15, "https://i.imgur.com/llUKttI.png" },
            };
        }
        
        public override int GetCost(CardInfo cardInfo)
        {
            int? cost = cardInfo.GetExtendedPropertyAsInt("LifeMoneyCost");
            return cost ?? 0;
        }
    }
}
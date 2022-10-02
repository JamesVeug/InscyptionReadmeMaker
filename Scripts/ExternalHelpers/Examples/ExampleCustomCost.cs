using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;

public class ExampleCustomCost : CustomCost
{
    public ExampleCustomCost()
    {
        CostName = "Smiles";
        CustomIconXURL = "https://i.imgur.com/pSioVfN.png";
        IntToImageURL = new Dictionary<int, string>()
        {
            { 0, "https://i.imgur.com/d4Q2mJU.png" },
            { 1, "https://i.imgur.com/3bwt587.png" },
            { 2, "https://i.imgur.com/IFvNiVx.png" },
            { 3, "https://i.imgur.com/QIbF9gk.png" },
            { 4, "https://i.imgur.com/1TZkXOo.png" },
            { 5, "https://i.imgur.com/GRmYdTT.png" },
            { 6, "https://i.imgur.com/7YSOSR6.png" },
            { 7, "https://i.imgur.com/Fj9MB7j.png" },
            { 8, "https://i.imgur.com/zLLqTum.png" },
            { 9, "https://i.imgur.com/3rCwzfc.png" },
        };
        CostToSingleImageURL = new Dictionary<int, string>()
        {
            { 1, "https://i.imgur.com/eQf312X.png" },
            { 2, "https://i.imgur.com/RI162D7.png" },
            { 3, "https://i.imgur.com/chnKIU5.png" },
            { 4, "https://i.imgur.com/3x9W6qR.png" },
            { 5, "https://i.imgur.com/Vew5FSR.png" },
        };
    }
    
    public override int GetCost(CardInfo cardInfo)
    {
        int? extendedPropertyAsInt = cardInfo.GetExtendedPropertyAsInt("ExampleCost");
        int propertyAsInt = (extendedPropertyAsInt != null) ? extendedPropertyAsInt.Value : 0;
        return propertyAsInt;
    }
}
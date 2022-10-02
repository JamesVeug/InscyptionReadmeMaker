using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;
using JamesGames.ReadmeMaker.ExternalHelpers;


public class ExampleCustomCardSection : CustomCardSection
{
    public override string SectionName() => "All Bone Cards";
    public override bool Enabled() => true;
    
    /// <summary>
    /// Get all cards that have a bone cost
    /// </summary>
    public override List<CardInfo> Initialize()
    {
        return CardManager.AllCardsCopy.FindAll(a => a.bonesCost > 0);
    }
}
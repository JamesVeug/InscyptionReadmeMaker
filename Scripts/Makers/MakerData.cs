using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using ReadmeMaker.Configs;

namespace ReadmeMaker
{
    public class MakerData
    {
        public List<CardInfo> allCards;
        public List<CardInfo> cards;
        public List<CardInfo> rareCards;
        public List<CardInfo> modifiedCards;
        public List<CardInfo> sideDeckCards;
        public List<AbilityManager.FullAbility> abilities;
        public List<SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility> specialAbilities;
        public List<ConfigData> configs;
    }
}
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
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
        public List<NewAbility> abilities;
        public List<NewSpecialAbility> specialAbilities;
        public List<ConfigData> configs;
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker.Sections
{
    public abstract class ACardsSection : ASection<CardInfo>
    {
        public override void Initialize()
        {
            rawData = GetCards();
        }

        protected abstract List<CardInfo> GetCards();

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<CardInfo>("Name", (a)=>a.displayedName),
                new TableColumn<CardInfo>("Power", ReadmeHelpers.GetPower),
                new TableColumn<CardInfo>("Health", ReadmeHelpers.GetHealth),
                new TableColumn<CardInfo>("Cost", GetCost),
                new TableColumn<CardInfo>("Evolution", GetEvolutionName, ReadmeConfig.Instance.CardShowEvolutions),
                new TableColumn<CardInfo>("Frozen Away", GetFrozenAway, ReadmeConfig.Instance.CardShowFrozenAway),
                new TableColumn<CardInfo>("Tail", GetTail, ReadmeConfig.Instance.CardShowTail),
                new TableColumn<CardInfo>("Sigils", GetSigils, ReadmeConfig.Instance.CardShowSigils),
                new TableColumn<CardInfo>("Specials", GetSpecialAbilities, ReadmeConfig.Instance.CardShowSpecials),
                new TableColumn<CardInfo>("Traits", GetTraits, ReadmeConfig.Instance.CardShowTraits),
                new TableColumn<CardInfo>("Tribes", GetTribes, ReadmeConfig.Instance.CardShowTribes)
            });
        }

        protected override bool Filter(CardInfo o)
        {
            if (!string.IsNullOrEmpty(ReadmeConfig.Instance.FilterByJSONLoaderModPrefix))
            {
                // Show everything
                string modPrefix = o.GetModPrefix();
                if (!string.IsNullOrEmpty(modPrefix))
                {
                    if (modPrefix.Trim() == ReadmeConfig.Instance.FilterByJSONLoaderModPrefix.Trim())
                    {
                        return true;
                    }
                }
            }
            
            return base.Filter(o);
        }

        protected override int Sort(CardInfo a, CardInfo b)
        {
            switch (ReadmeConfig.Instance.CardSortBy)
            {
                case ReadmeConfig.CardSortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.CardSortByType.Name:
                    return ReadmeHelpers.CompareByDisplayName(a, b);
                case ReadmeConfig.CardSortByType.Cost:
                    return ReadmeHelpers.CompareByCost(a, b);
                case ReadmeConfig.CardSortByType.Power:
                    return a.Attack - b.Attack;
                case ReadmeConfig.CardSortByType.Health:
                    return a.Health - b.Health;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string GetGUID(CardInfo o)
        {
            return o.GetModTag();
        }

        private string GetCost(CardInfo info)
        {
            StringBuilder costBuilder = new StringBuilder();
            ReadmeDump.AppendAllCosts(info, costBuilder);
            return costBuilder.ToString();
        }
        
        private static string GetTribes(CardInfo info)
        {
            StringBuilder tribesBuilder = new StringBuilder();
            for (int j = 0; j < info.tribes.Count; j++)
            {
                if (j > 0)
                {
                    tribesBuilder.Append(", ");
                }

                tribesBuilder.Append(ReadmeHelpers.GetTribeName(info.tribes[j]));
            }

            return tribesBuilder.ToString();
        }

        private string GetTraits(CardInfo info)
        {
            StringBuilder traitsBuilder = new StringBuilder();
            for (int j = 0; j < info.traits.Count; j++)
            {
                if (j > 0)
                {
                    traitsBuilder.Append(", ");
                }

                string traitName = ReadmeHelpers.GetTraitName(info.traits[j]);
                traitsBuilder.Append(traitName);
            }

            return traitsBuilder.ToString();
        }

        private static string GetSpecialAbilities(CardInfo info)
        {
            StringBuilder specialsBuilder = new StringBuilder();
            for (int j = 0; j < info.specialAbilities.Count; j++)
            {
                if (j > 0)
                {
                    specialsBuilder.Append(", ");
                }

                string specialAbilityName = ReadmeHelpers.GetSpecialAbilityName(info.specialAbilities[j]);
                if (specialAbilityName != null)
                {
                    specialsBuilder.Append($" {specialAbilityName}");
                }
            }

            return specialsBuilder.ToString();
        }

        private string GetTail(CardInfo info)
        {
            if (info.tailParams != null && info.tailParams.tail != null)
            {
                return info.tailParams.tail.displayedName;
            }

            return "";
        }

        private string GetFrozenAway(CardInfo info)
        {
            if (info.iceCubeParams != null && info.iceCubeParams.creatureWithin != null)
            {
                return info.iceCubeParams.creatureWithin.displayedName;
            }

            return "";
        }

        private string GetEvolutionName(CardInfo info)
        {
            if (info.evolveParams != null && info.evolveParams.evolution != null)
            {
                return info.evolveParams.evolution.displayedName;
            }

            return "";
        }

        private static string GetSigils(CardInfo info)
        {
            StringBuilder sigilBuilder = new StringBuilder();
            
            Dictionary<Ability, int> abilityCount = new Dictionary<Ability, int>();
            List<Ability> infoAbilities = info.abilities;
            if (ReadmeConfig.Instance.CardSigilsJoinDuplicates)
            {
                infoAbilities = Helpers.RemoveDuplicates(info.abilities, ref abilityCount);
            }

            // Show all abilities one after the other
            int totalShownAbilities = 0;
            foreach (Ability ability in infoAbilities)
            {
                AbilityInfo abilityInfo = ReadmeHelpers.GetAbilityInfo(ability);
                if (abilityInfo == null)
                {
                    continue;
                }
                
                string abilityName = abilityInfo.rulebookName;
                if (string.IsNullOrEmpty(abilityName))
                {
                    Plugin.Log.LogWarning("Ability does not have rulebookName: '" + ability + "'");
                    continue;
                }
                
                if (totalShownAbilities++ > 0)
                {
                    sigilBuilder.Append(", ");
                }
                
                if (ReadmeConfig.Instance.CardSigilsJoinDuplicates && abilityCount.TryGetValue(ability, out int count) && count > 1)
                {
                    // Show all abilities, but combine duplicates into Waterborne(x2)
                    sigilBuilder.Append($" {abilityName}(x{count})");
                }
                else
                {
                    // Show all abilities 1 by 1 (Waterborne, Waterborne, Waterborne)
                    sigilBuilder.Append($" {abilityName}");
                }
            }
            
            return sigilBuilder.ToString();
        }
    }
}
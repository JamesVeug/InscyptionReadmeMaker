using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker.Sections
{
    public abstract class ACardsSection : ASection<CardInfo>
    {
        public override void Initialize(RegisteredMod mod)
        {
            rawData = GetCards(mod);
        }

        protected abstract List<CardInfo> GetCards(RegisteredMod mod);

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            List<TableColumn<CardInfo>> tableColumns = new List<TableColumn<CardInfo>>()
            {
                new TableColumn<CardInfo>("Name", (a)=>a.displayedName),
                new TableColumn<CardInfo>("Power", ReadmeHelpers.GetPower),
                new TableColumn<CardInfo>("Health", ReadmeHelpers.GetHealth),
                new TableColumn<CardInfo>("Cost", GetCost),
                new TableColumn<CardInfo>("Sigils", GetSigils, ReadmeConfig.Instance.CardShowSigils),
                new TableColumn<CardInfo>("Evolution", GetEvolutionName, ReadmeConfig.Instance.CardShowEvolutions),
                new TableColumn<CardInfo>("Frozen Away", GetFrozenAway, ReadmeConfig.Instance.CardShowFrozenAway),
                new TableColumn<CardInfo>("Tail", GetTail, ReadmeConfig.Instance.CardShowTail),
                new TableColumn<CardInfo>("Specials", GetSpecialAbilities, ReadmeConfig.Instance.CardShowSpecials),
                new TableColumn<CardInfo>("Traits", GetTraits, ReadmeConfig.Instance.CardShowTraits),
                new TableColumn<CardInfo>("Tribes", GetTribes, ReadmeConfig.Instance.CardShowTribes)
            };
            
            if (ReadmeConfig.Instance.ShowGUIDS)
            {
                tableColumns.Insert(0, new TableColumn<CardInfo>("Mod Prefix", (a) =>
                {
                    return a.GetModPrefix();
                }));
            }
            
            splitCards = BreakdownForTable(out headers, tableColumns.ToArray());
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
            int compare = 0;
            switch (ReadmeConfig.Instance.CardSortBy)
            {
                case ReadmeConfig.CardSortByType.GUID:
                    compare = string.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                    if (compare == 0)
                    {
                        compare = ReadmeHelpers.CompareByDisplayName(a, b);
                    }
                    break;
                case ReadmeConfig.CardSortByType.Name:
                    compare = ReadmeHelpers.CompareByDisplayName(a, b);
                    break;
                case ReadmeConfig.CardSortByType.Cost:
                    compare = ReadmeHelpers.CompareByCost(a, b);
                    break;
                case ReadmeConfig.CardSortByType.Power:
                    compare = a.Attack - b.Attack;
                    break;
                case ReadmeConfig.CardSortByType.Health:
                    compare = a.Health - b.Health;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!ReadmeConfig.Instance.CardSortAscending)
            {
                compare *= -1;
            }

            return compare;
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
            int abilities = 0;
            StringBuilder specialsBuilder = new StringBuilder();
            for (int i = 0; i < info.specialAbilities.Count; i++)
            {
                if (abilities > 0)
                {
                    specialsBuilder.Append(", ");
                }

                string specialAbilityName = ReadmeHelpers.GetSpecialAbilityName(info.specialAbilities[i]);
                if (!string.IsNullOrEmpty(specialAbilityName))
                {
                    specialsBuilder.Append($"{specialAbilityName}");
                    abilities++;
                }
            }

            string specialAbilities = specialsBuilder.ToString();
            return specialAbilities;
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
                    sigilBuilder.Append($"{abilityName}(x{count})");
                }
                else
                {
                    // Show all abilities 1 by 1 (Waterborne, Waterborne, Waterborne)
                    sigilBuilder.Append(abilityName);
                }
            }
            
            return sigilBuilder.ToString();
        }
    }
}
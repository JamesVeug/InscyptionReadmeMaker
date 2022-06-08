﻿using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using HarmonyLib;

namespace JamesGames.ReadmeMaker.Sections
{
    public abstract class ACardsSection : ASection
    {
        private List<CardInfo> allCards;

        public override void Initialize()
        {
            allCards = GetCards();
            allCards.Sort(SortCards);
        }

        protected abstract List<CardInfo> GetCards();

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (allCards.Count > 0)
            {
                stringBuilder.Append($"\n{allCards.Count} {SectionName}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(allCards, out headers, new TableColumn<CardInfo>[]
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

        private string GetCost(CardInfo info)
        {
            StringBuilder costBuilder = new StringBuilder();
            ReadmeDump.AppendAllCosts(info, costBuilder);
            return costBuilder.ToString();
        }
        
        private static string GetTribes(CardInfo info)
        {
            return info.tribes.Join(ReadmeHelpers.GetTribeName, ",<br/>");
        }

        private string GetTraits(CardInfo info)
        {
            return info.traits.Join(ReadmeHelpers.GetTraitName, ",<br/>");
        }

        private static string GetSpecialAbilities(CardInfo info)
        {
            return info.specialAbilities.Join(ReadmeHelpers.GetSpecialAbilityName, ",<br/>");
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
                    sigilBuilder.Append(",<br/>");
                }
                
                if (ReadmeConfig.Instance.CardSigilsJoinDuplicates && abilityCount.TryGetValue(ability, out int count) && count > 1)
                {
                    // Show all abilities, but combine duplicates into Waterborne(x2)
                    sigilBuilder.Append($"{abilityName}(x{count})");
                }
                else
                {
                    // Show all abilities 1 by 1 (Waterborne, Waterborne, Waterborne)
                    sigilBuilder.Append($"{abilityName}");
                }
            }

            return sigilBuilder.ToString();
        }
        
        protected int SortCards(CardInfo a, CardInfo b)
        {
            int sorted = 0;
            switch (ReadmeConfig.Instance.CardSortBy)
            {
        	    case ReadmeConfig.SortByType.Cost:
        		    sorted = CompareByCost(a, b); 
        		    break;
        	    case ReadmeConfig.SortByType.Name:
        		    sorted = CompareByDisplayName(a, b); 
        		    break;
            }

            if (!ReadmeConfig.Instance.CardSortAscending)
            {
        	    return sorted * -1;
            }
            
            return sorted;
        }
        
        private static int CompareByDisplayName(CardInfo a, CardInfo b)
        {
            if (a.displayedName == null)
            {
                if (b.displayedName != null)
                {
                    return -1;
                }
                return 0;
            }

            return b.displayedName == null 
                       ? 1 
                       : string.Compare(a.displayedName.ToLower(), b.displayedName.ToLower(), StringComparison.Ordinal);
        }

        private static int CompareByCost(CardInfo a, CardInfo b)
        {
            List<Tuple<int, int>> aCosts = GetCostType(a);
            List<Tuple<int, int>> bCosts = GetCostType(b);

            // Show least amount of costs at the top (Blood, Bone, Blood&Bone)
            if (aCosts.Count != bCosts.Count)
            {
                return aCosts.Count - bCosts.Count;
            }
	        
            // Show lowest cost first (Blood, Bone, Energy)
            for (var i = 0; i < aCosts.Count; i++)
            {
                Tuple<int, int> aCost = aCosts[i];
                Tuple<int, int> bCost = bCosts[i];
                if (aCost.Item1 != bCost.Item1)
                {
                    return aCost.Item1 - bCost.Item1;
                }
            }

            // Show lowest amounts first (1 Blood, 2 Blood)
            for (var i = 0; i < aCosts.Count; i++)
            {
                Tuple<int, int> aCost = aCosts[i];
                Tuple<int, int> bCost = bCosts[i];
                if (aCost.Item2 != bCost.Item2)
                {
                    return aCost.Item2 - bCost.Item2;
                }
            }

            ListPool.Push(aCosts);
            ListPool.Push(bCosts);

            // Same Costs
            // Default to Name
            return CompareByDisplayName(a, b);
        }
        
        private static List<Tuple<int, int>> GetCostType(CardInfo a)
        {
            List<Tuple<int, int>> list = ListPool.Pull<Tuple<int, int>>();
            if (a.BloodCost > 0)
            {
                list.Add(new Tuple<int, int>(0, a.BloodCost));
            }
            if (a.bonesCost > 0)
            {
                list.Add(new Tuple<int, int>(1, a.bonesCost));
            }
            if (a.energyCost > 0)
            {
                list.Add(new Tuple<int, int>(2, a.energyCost));
            }
            if (a.gemsCost.Count > 0)
            {
                foreach (var gemType in a.gemsCost)
                {
                    switch (gemType)
                    {
                        case GemType.Green:
                            list.Add(new Tuple<int, int>(3, 1));
                            break;
                        case GemType.Orange:
                            list.Add(new Tuple<int, int>(4, 1));
                            break;
                        case GemType.Blue:
                            list.Add(new Tuple<int, int>(5, 1));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return list;
        }
    }
}
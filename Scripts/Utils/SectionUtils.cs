using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;
using JamesGames.ReadmeMaker;
using JamesGames.ReadmeMaker.Sections;

namespace ReadmeMaker.Scripts.Utils
{
	public static class SectionUtils
	{
        public static List<TableColumn<CardInfo>> GetCardTableColumns()
		{
			List<TableColumn<CardInfo>> tableColumns = new List<TableColumn<CardInfo>>()
			{
				new TableColumn<CardInfo>("Name", (a) => a.displayedName),
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
				tableColumns.Insert(0, new TableColumn<CardInfo>("Mod Prefix", (a) => { return a.GetModPrefix(); }));
			}

			return tableColumns;
		}
		
		public static string GetCost(CardInfo info)
        {
            StringBuilder costBuilder = new StringBuilder();
            ReadmeDump.AppendAllCosts(info, costBuilder);
            return costBuilder.ToString();
        }
		
        public static string GetMetaCategories(CardInfo info)
        {
            string s = "";
            foreach (CardMetaCategory category in info.metaCategories)
            {
                if (string.IsNullOrEmpty(s))
                {
                    s = category.ToString();
                }
                else
                {
                    s += ", " + category;
                }
            }
            
            return s;
        }
        
        public static string GetTribes(CardInfo info)
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

        public static string GetTraits(CardInfo info)
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

        public static string GetSpecialAbilities(CardInfo info)
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

        public static string GetTail(CardInfo info)
        {
            if (info.tailParams != null && info.tailParams.tail != null)
            {
                return info.tailParams.tail.displayedName;
            }

            return "";
        }

        public static string GetFrozenAway(CardInfo info)
        {
            if (info.iceCubeParams != null && info.iceCubeParams.creatureWithin != null)
            {
                return info.iceCubeParams.creatureWithin.displayedName;
            }

            return "";
        }

        public static string GetEvolutionName(CardInfo info)
        {
            if (info.evolveParams != null && info.evolveParams.evolution != null)
            {
                return info.evolveParams.evolution.displayedName;
            }

            return "";
        }

        public static string GetSigils(CardInfo info)
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
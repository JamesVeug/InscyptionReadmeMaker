using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;

namespace JamesGames.ReadmeMaker
{
    public class ReadmeHelpers
    {
        public static List<StatIconManager.FullStatIcon> GetAllNewStatInfoIcons()
        {
            return new List<StatIconManager.FullStatIcon>(Helpers.GetStaticPrivateField <ObservableCollection<StatIconManager.FullStatIcon>>(typeof(StatIconManager), "NewStatIcons"));
        }

        public static List<SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility> GetAllNewSpecialAbilities()
        {
            var newSpecialAbilities = SpecialTriggeredAbilityManager.NewSpecialTriggers;
            var specialAbilities = new List<SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility>(newSpecialAbilities);
            return specialAbilities;
        }
        
        public static string GetSpecialAbilityName(SpecialTriggeredAbility ability)
		{
			if (ability <= SpecialTriggeredAbility.NUM_ABILITIES)
			{
				return ability.ToString();
			}

			var specialAbility = ReadmeHelpers.GetAllNewSpecialAbilities().Find((a)=>a.Id == ability);
			if (specialAbility != null)
			{
				StatIconManager.FullStatIcon icon = ReadmeHelpers.GetAllNewStatInfoIcons().Find((a)=>a.VariableStatBehavior == specialAbility.AbilityBehaviour);
				if (icon != null)
				{
					return icon.Info.rulebookName;
				}
			}

			return null;
		}
        
        public static string GetSpecialAbilityName(SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility ability)
        {
	        StatIconManager.FullStatIcon icon = ReadmeHelpers.GetAllNewStatInfoIcons().Find((a)=>a.VariableStatBehavior == ability.AbilityBehaviour);
	        if (icon != null)
	        {
		        return icon.Info.rulebookName;
	        }

	        return null;
        }
        
        public static string GetSpecialAbilityDescription(SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility ability)
        {
	        StatIconManager.FullStatIcon icon = ReadmeHelpers.GetAllNewStatInfoIcons().Find((a)=>a.VariableStatBehavior == ability.AbilityBehaviour);
	        if (icon != null)
	        {
		        return icon.Info.rulebookDescription;
	        }

	        return null;
        }
		
		public static string GetAbilityName(AbilityManager.FullAbility Ability)
		{
			return Ability.Info.rulebookName;
		}
        
		// In-game, when the rulebook description for a sigil is being displyed all instances of "[creature]" are replaced with "A card bearing this sigil".
		// We do this when generating the readme as well for the sake of consistency.
		public static string GetAbilityDescription(AbilityManager.FullAbility Ability)
		{
			// Seeing "[creature]" appear in the readme looks jarring, sigil descriptions should appear exactly as they do in the rulebook for consistency
			string description = Ability.Info.rulebookDescription;
			return description.Replace("[creature]", "A card bearing this sigil");
		}

		public static string GetTraitName(Trait trait)
		{
			if (ReadmeDump.TraitToName.TryGetValue(trait, out string name))
			{
				return name;
			}
			
			return trait.ToString();
		}

		public static string GetTribeName(Tribe tribe)
		{
			if (ReadmeDump.TribeToName.TryGetValue(tribe, out string name))
			{
				return name;
			}

			string tribeName = Helpers.GetName(((int)tribe).ToString());
			if (!string.IsNullOrEmpty(tribeName))
			{
				return tribeName;
			}
			
			return tribe.ToString();
		}

		public static string GetTribeGUID(Tribe tribe)
		{
			return Helpers.GetGUID(((int)tribe).ToString());
		}

		public static string GetPower(CardInfo info)
		{
			string power = "";
			foreach (KeyValuePair<SpecialTriggeredAbility,string> pair in ReadmeDump.PowerModifyingSpecials)
			{
				if(info.SpecialAbilities.Contains(pair.Key))
				{
					if (!string.IsNullOrEmpty(power))
					{
						power += ", ";
					}

					if (string.IsNullOrEmpty(pair.Value))
					{
						power += GetSpecialAbilityName(pair.Key);
					}
					else
					{
						power += pair.Value;
					}
				}
			}

			if (string.IsNullOrEmpty(power))
			{
				return info.baseAttack.ToString();
			}
			else if (info.baseAttack > 0)
			{
				return power + " + " + info.baseAttack;
			}
			else
			{
				return power;
			}
		}

		public static string GetHealth(CardInfo info)
		{
			string health = "";
			foreach (KeyValuePair<SpecialTriggeredAbility,string> pair in ReadmeDump.HealthModifyingSpecials)
			{
				if(info.SpecialAbilities.Contains(pair.Key))
				{
					if (!string.IsNullOrEmpty(health))
					{
						health += ", ";
					}

					if (string.IsNullOrEmpty(pair.Value))
					{
						health += GetSpecialAbilityName(pair.Key);
					}
					else
					{
						health += pair.Value;
					}
				}
			}

			if (string.IsNullOrEmpty(health))
			{
				return info.baseHealth.ToString();
			}
			else if (info.baseHealth > 0)
			{
				return health + " + " + info.baseHealth;
			}
			else
			{
				return health;
			}
		}

		public static AbilityInfo GetAbilityInfo(Ability ability)
		{
			AbilityInfo abilityInfo = AbilitiesUtil.GetInfo(ability);
			if (abilityInfo != null)
			{
				return abilityInfo;
			}
			
			var abilities = new List<AbilityManager.FullAbility>(AbilityManager.NewAbilities);
			for (int i = 0; i < abilities.Count; i++)
			{
				AbilityManager.FullAbility fullAbility = abilities[i];
				if (fullAbility.Id == ability)
				{
					return fullAbility.Info;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the GUID of the mod that called this method.
		/// Requires passing in the callingAssembly otherwise it can't find the mod.
		/// </summary>
		public static string GetModIdFromCallstack(Assembly callingAssembly)
		{
			string cacheVal = TypeManager.GetModIdFromAssembly(callingAssembly);
			if (!string.IsNullOrEmpty(cacheVal))
			{
				return cacheVal;
			}

			StackTrace trace = new StackTrace();
			StackFrame[] frames = trace.GetFrames();
			foreach (StackFrame frame in frames)
			{
				string newVal = TypeManager.GetModIdFromAssembly(frame.GetMethod().DeclaringType.Assembly);
				if (!string.IsNullOrEmpty(newVal) && newVal != Plugin.PluginGuid)
				{
					return newVal;
				}
			}

			return default(string);
		}


		public static int CompareByDisplayName(CardInfo a, CardInfo b)
        {
            if (a.displayedName == null)
            {
                if (b.displayedName != null)
                {
                    return -1;
                }
                return 0;
            }
            else if (b.displayedName == null)
            {
                return 1;
            }
            
            return String.Compare(a.displayedName.ToLower(), b.displayedName.ToLower(), StringComparison.Ordinal);
        }

        public static int CompareByCost(CardInfo a, CardInfo b)
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
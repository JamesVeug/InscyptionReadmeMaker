using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Card;

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
	        return icon?.Info.rulebookName;
        }
        
        public static string GetSpecialAbilityDescription(SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility ability)
        {
	        StatIconManager.FullStatIcon icon = ReadmeHelpers.GetAllNewStatInfoIcons().Find((a)=>a.VariableStatBehavior == ability.AbilityBehaviour);
	        return icon?.Info.rulebookDescription;
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
			return ReadmeDump.TraitToName.TryGetValue(trait, out string name) ? name : trait.ToString();
		}

		public static string GetTribeName(Tribe tribe)
		{
			return ReadmeDump.TribeToName.TryGetValue(tribe, out string name) ? name : tribe.ToString();
		}

		public static string GetPower(CardInfo info)
		{
			string power = "";
			foreach (var pair in ReadmeDump.PowerModifyingSpecials.Where(pair => info.SpecialAbilities.Contains(pair.Key)))
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

			if (string.IsNullOrEmpty(power))
			{
				return info.baseAttack.ToString();
			}

			if (info.baseAttack > 0)
			{
				return power + " + " + info.baseAttack;
			}

			return power;
		}

		public static string GetHealth(CardInfo info)
		{
			string health = "";
			foreach (var pair in ReadmeDump.HealthModifyingSpecials.Where(pair => info.SpecialAbilities.Contains(pair.Key)))
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

			if (string.IsNullOrEmpty(health))
			{
				return info.baseHealth.ToString();
			}

			if (info.baseHealth > 0)
			{
				return health + " + " + info.baseHealth;
			}

			return health;
		}

		public static AbilityInfo GetAbilityInfo(Ability ability)
		{
			AbilityInfo abilityInfo = AbilitiesUtil.GetInfo(ability);
			if (abilityInfo != null)
			{
				return abilityInfo;
			}
			
			var abilities = AbilityManager.NewAbilities;
			return (from fullAbility in abilities where fullAbility.Id == ability select fullAbility.Info).FirstOrDefault();
		}
    }
}
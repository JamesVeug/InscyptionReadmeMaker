using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using APIPlugin;
using DiskCardGame;

namespace ReadmeMaker
{
    public class ReadmeDump
    {
        public static void Dump()
        {
	        Plugin.Log.LogInfo("Generating Readme...");
	        string text = GetDumpString();

	        string path = Path.Combine(Plugin.Directory, "GENERATED_README.md");
	        File.WriteAllText(path, text);
	        
	        Plugin.Log.LogInfo("Readme Dumped to " + path);
        }

        private static string GetDumpString()
        {
	        //
	        // Initialize everything for the Summary
	        //
	        List<CardInfo> allCards = NewCard.cards.FindAll((a) => a.metaCategories.Count > 0);
	        
	        List<CardInfo> cards = allCards.FindAll((a) => !a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        cards.Sort((a, b)=>String.Compare(a.displayedName, b.displayedName, StringComparison.Ordinal));
	        
	        List<CardInfo> rareCards = allCards.FindAll((a) => a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        rareCards.Sort((a, b)=>String.Compare(a.displayedName, b.displayedName, StringComparison.Ordinal));

	        List<NewAbility> abilities = NewAbility.abilities.FindAll((a)=>a.id.GetPrivateFieldValue<string>("guid") == Plugin.PluginGuid);
	        abilities.Sort((a, b)=>String.Compare(a.info.rulebookName, b.info.rulebookName, StringComparison.Ordinal));
	        
	        List<NewSpecialAbility> specialAbilities = NewSpecialAbility.specialAbilities.FindAll((a)=>a.id.GetPrivateFieldValue<string>("guid") == Plugin.PluginGuid);
	        specialAbilities.Sort((a, b)=>String.Compare(a.statIconInfo.rulebookName, b.statIconInfo.rulebookName, StringComparison.Ordinal));

	        //
	        // Build string
	        //
	        
	        // Summary
	        StringBuilder stringBuilder = new StringBuilder();
	        stringBuilder.Append("### Includes:\n");
	        stringBuilder.Append($"- {allCards.Count} New Cards:\n");
	        stringBuilder.Append($"- {abilities.Count} New Sigils:\n");
	        stringBuilder.Append($"- {specialAbilities.Count} New Special Abilities:\n");

	        // Cards
	        stringBuilder.Append("\n### Cards:\n");
	        for (int i = 0; i < cards.Count; i++)
	        {
		        stringBuilder.Append(GetCardInfo(cards[i]) + "\n");
	        }

	        // Rare Cards
	        stringBuilder.Append("\n### Rare Cards:\n");
	        for (int i = 0; i < rareCards.Count; i++)
	        {
		        stringBuilder.Append(GetCardInfo(rareCards[i]) + "\n");
	        }
	        
	        // Sigils
	        stringBuilder.Append("\n### Sigils:\n");
	        for (int i = 0; i < abilities.Count; i++)
	        {
		        stringBuilder.Append(GetAbilityInfo(abilities[i]) + "\n");
	        }
	        
	        // Special Abilities
	        stringBuilder.Append("\n### Special Abilities:\n");
	        for (int i = 0; i < specialAbilities.Count; i++)
	        {
		        stringBuilder.Append(GetSpecialAbilityInfo(specialAbilities[i]) + "\n");
	        }

	        return stringBuilder.ToString();
        }

        private static string GetAbilityInfo(NewAbility newAbility)
        {
	        return $" - **{newAbility.info.rulebookName}** - {newAbility.info.rulebookDescription}";
        }

        private static string GetSpecialAbilityInfo(NewSpecialAbility newAbility)
        {
	        return $" - **{newAbility.statIconInfo.rulebookName}** - {newAbility.statIconInfo.rulebookDescription}";
        }

        private static string GetCardInfo(CardInfo info)
        {
	        string bloodIcon = "![Blood Cost](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_blood.png)";
	        string boneIcon = "![Bone Cost](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)";
	        
	        // - **Drone** - 1,1 with Bone digger and Evolve. Evolves into Crawler Forest
	        StringBuilder builder = new StringBuilder();
	        builder.Append($" - **{info.displayedName}** - ");
	        builder.Append($"{info.baseAttack},{info.baseHealth} -");

	        // Cost
	        if (info.BloodCost > 0)
	        {
		        for (int i = 0; i < info.BloodCost; i++)
		        {
			        builder.Append($" {bloodIcon}");
		        }
	        }
	        
	        if (info.bonesCost > 0)
	        {
		        for (int i = 0; i < info.bonesCost; i++)
		        {
			        builder.Append($" {boneIcon}");
		        }
	        }

	        // Abilities
	        for (int i = 0; i < info.abilities.Count; i++)
	        {
		        if (i == 0)
		        {
			        builder.Append($" Sigils:");
		        }
		        else
		        {
			        builder.Append($",");
		        }

		        string abilityName = AbilitiesUtil.GetInfo(info.abilities[i]).rulebookName;
		        builder.Append($" {abilityName}");

		        if (i == info.abilities.Count - 1)
		        {
			        builder.Append($".");
		        }
	        }
	        
	        // Evolution
	        if (info.evolveParams != null && info.evolveParams.evolution != null)
	        {
		        builder.Append($" Evolves into {info.evolveParams.evolution.displayedName}");
	        }
	        
	        // Specials
	        for (int i = 0; i < info.specialAbilities.Count; i++)
	        {
		        if (i == 0)
		        {
			        builder.Append($" Specials:");
		        }
		        else
		        {
			        builder.Append($",");
		        }

		        // TODO: Do this by getting the info from the rulebook?
		        string abilityName = GetSpecialAbilityName(info.specialAbilities[i]);
		        if (abilityName != null)
		        {
			        builder.Append($" {abilityName}");
		        }
		        
		        if (i == info.abilities.Count - 1)
		        {
			        builder.Append($".");
		        }
	        }

	        return builder.ToString();
        }

        private static string GetSpecialAbilityName(SpecialTriggeredAbility ability)
        {
	        if (ability <= SpecialTriggeredAbility.NUM_ABILITIES)
	        {
		        return ability.ToString();
	        }

	        for (int i = 0; i < NewSpecialAbility.specialAbilities.Count; i++)
	        {
		        if (NewSpecialAbility.specialAbilities[i].specialTriggeredAbility == ability)
		        {
			        return NewSpecialAbility.specialAbilities[i].statIconInfo.rulebookName;
		        }
	        }

	        return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using APIPlugin;
using DiskCardGame;

namespace ReadmeMaker
{
    public static class ReadmeDump
    {
	    const string bloodIcon = "![Blood Cost](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_blood.png)";
	    const string boneIcon = "![Bone Cost](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_bone.png)";
	    const string energyIconManta = "![Energy Cost](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_energy_manta.png)";
	    const string moxIconB = "![Energy Cost](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_mox_b.png)";
	    const string moxIconG = "![Energy Cost](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_mox_g.png)";
	    const string moxIconO = "![Energy Cost](https://raw.githubusercontent.com/JamesVeug/InscyptionReadmeMaker/main/Artwork/Git/cost_mox_o.png)";
	    
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
	        cards.Sort(SortCards);
	        
	        List<CardInfo> rareCards = allCards.FindAll((a) => a.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground));
	        rareCards.Sort(SortCards);

	        List<NewAbility> abilities = NewAbility.abilities;
	        abilities.Sort((a, b)=>String.Compare(a.info.rulebookName, b.info.rulebookName, StringComparison.Ordinal));
	        
	        List<NewSpecialAbility> specialAbilities = NewSpecialAbility.specialAbilities;
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

        private static int SortCards(CardInfo a, CardInfo b)
        {
	        int sorted = 0;
	        switch (Plugin.ReadmeConfig.CardSortBy)
	        {
		        case ReadmeConfig.SortByType.Cost:
			        sorted = GetCostValue(a, b); 
			        break;
		        case ReadmeConfig.SortByType.Name:
			        sorted = String.Compare(a.displayedName, b.displayedName, StringComparison.Ordinal); 
			        break;
	        }

	        if (!Plugin.ReadmeConfig.CardSortAscending)
	        {
		        return sorted * -1;
	        }
	        
	        return sorted;
        }

        private static int GetCostValue(CardInfo a, CardInfo b)
        {
	        List<Tuple<int, int>> aCosts = GetCostType(a);
	        List<Tuple<int, int>> bCosts = GetCostType(b);
	        
	        bool sameCostTypes = true;
	        foreach (Tuple<int, int> aCost in aCosts)
	        {
		        Tuple<int, int> bCost = bCosts.Find((z) => z.Item1 == aCost.Item1);
		        if (bCost != null)
		        {
			        sameCostTypes = false;
			        break;
		        }
	        }

	        int sortedValue = 0;
	        if (sameCostTypes)
	        {
		        // Compare by amount of each cost
		        foreach (Tuple<int,int> aCost in aCosts)
		        {
			        Tuple<int, int> bCost = bCosts.Find((z) => z.Item1 == aCost.Item1);
			        if (aCost.Item2 != bCost.Item2)
			        {
				        sortedValue = aCost.Item2 - bCost.Item2;
				        break;
			        }
		        }
	        }
	        else
	        {
		        // Compare by who has the minimum cost type (Not perfect)
		        Tuple<int, int> aMin = aCosts.Min();
		        Tuple<int, int> bMin = bCosts.Min();
		        sortedValue = aMin.Item1 - bMin.Item1;
	        }
	        
	        ListPool.Push(aCosts);
	        ListPool.Push(bCosts);

	        return sortedValue;
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
		        list.Add(new Tuple<int, int>(3, a.gemsCost.Count));
	        }

	        return list;
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
	        StringBuilder builder = new StringBuilder();
	        builder.Append($" - **{info.displayedName}** - ");
	        builder.Append($"{info.baseAttack},{info.baseHealth} -");

	        // Cost
	        AppendCost(info.BloodCost, bloodIcon, builder);
	        AppendCost(info.bonesCost, boneIcon, builder);
	        AppendCost(info.energyCost, energyIconManta, builder);
	        AppendCost(info.gemsCost.Contains(GemType.Blue) ? 1 : 0, moxIconB, builder);
	        AppendCost(info.gemsCost.Contains(GemType.Green) ? 1 : 0, moxIconG, builder);
	        AppendCost(info.gemsCost.Contains(GemType.Orange) ? 1 : 0, moxIconO, builder);

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
	        
	        // Traits
	        for (int i = 0; i < info.traits.Count; i++)
	        {
		        if (i == 0)
		        {
			        builder.Append($" Traits:");
		        }
		        else
		        {
			        builder.Append($",");
		        }

		        builder.Append($" {info.traits[i]}");
		        if (i == info.abilities.Count - 1)
		        {
			        builder.Append($".");
		        }
	        }

	        return builder.ToString();
        }

        private static void AppendCost(int cost, string icon, StringBuilder builder)
        {
	        if (cost <= 0)
		        return;
	        
	        if (cost <= 4)
	        {
		        // Bone Bone Bone Bone
		        for (int i = 0; i < cost; i++)
		        {
			        builder.Append($" {icon}");
		        }
	        }
	        else
	        {
		        // 4Bone
		        builder.Append($" {cost}{icon}");
	        }
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
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Card;
using SpecialAbility = InscryptionAPI.Card.SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewSpecialAbilitiesSection : ASection<SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility>
    {
        public override string SectionName => "New Special Abilities";
        public override bool Enabled => ReadmeConfig.Instance.SpecialAbilitiesShow;
        
        public override void Initialize()
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(ReadmeHelpers.GetAllNewSpecialAbilities());
	        
            // Remove special abilities that have no rulebook entry
            var icons = ReadmeHelpers.GetAllNewStatInfoIcons();
            for (int i = 0; i < rawData.Count; i++)
            {
                SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility specialAbility = rawData[i];
                StatIconManager.FullStatIcon fullStatIcon = icons.Find((b) => b.VariableStatBehavior == specialAbility.AbilityBehaviour);
                if (fullStatIcon == null || fullStatIcon.Info == null || string.IsNullOrEmpty(fullStatIcon.Info.rulebookName))
                {
                    rawData.RemoveAt(i--);
                }
            }
	        
            rawData.Sort(SortNewSpecialAbilities);
        }
        
        private static int SortNewSpecialAbilities(SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility a, SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility b)
        {
            var icons = ReadmeHelpers.GetAllNewStatInfoIcons();
            StatIconManager.FullStatIcon aStatIcon = icons.Find((icon) => icon.VariableStatBehavior == a.AbilityBehaviour);
            StatIconManager.FullStatIcon bStatIcon = icons.Find((icon) => icon.VariableStatBehavior == b.AbilityBehaviour);
            return String.Compare(aStatIcon.Info.rulebookName, bStatIcon.Info.rulebookName, StringComparison.Ordinal);
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<SpecialAbility>("Name", ReadmeHelpers.GetSpecialAbilityName),
                new TableColumn<SpecialAbility>("Description", ReadmeHelpers.GetSpecialAbilityDescription)
            });
        }

        public override string GetGUID(SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility o)
        {
            return Helpers.GetGUID(((int)o.Id).ToString());
        }
    }
}
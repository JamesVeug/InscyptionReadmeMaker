using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using InscryptionAPI.Card;
using ReadmeMaker.Configs;
using UnityEngine;
using SpecialAbility = InscryptionAPI.Card.SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility;

namespace ReadmeMaker.Sections
{
    public class NewConfigsSection : ASection
    {
        private List<ConfigData> configs = null;
        
        public override void Initialize()
        {
             configs = new List<ConfigData>();
            if (!ReadmeConfig.Instance.ConfigSectionEnabled)
            {
                return;
            }

            List<string> validModGUIDS = null;
            if (!string.IsNullOrEmpty(ReadmeConfig.Instance.ConfigOnlyShowModGUID))
            {
                string[] guids = ReadmeConfig.Instance.ConfigOnlyShowModGUID.Split(',');
                validModGUIDS = new List<string>(guids);
            }
            
            foreach (BaseUnityPlugin plugin in GameObject.FindObjectsOfType<BaseUnityPlugin>())
            {
                if (plugin.Config == null)
                {
            	    continue;
                }

                string guid = plugin.Info.Instance.Info.Metadata.GUID;
                if (validModGUIDS != null && !validModGUIDS.Contains(guid))
                {
            	    continue;
                }
                
                ConfigEntryBase[] entries = plugin.Config.GetConfigEntries();
                foreach (ConfigEntryBase definition in entries)
                {
                    configs.Add(new ConfigData()
            	    {
            		    PluginGUID = guid,
            		    Entry = definition,
            	    });
                }
            }

            // Sort by
            // GUID
            // Section
            // Key
            configs.Sort((a, b) =>
            {
                int guid = String.Compare(a.PluginGUID, b.PluginGUID, StringComparison.Ordinal);
                if (guid != 0)
                {
            	    return guid;
                }
                
                int section = String.Compare(a.Entry.Definition.Section, b.Entry.Definition.Section, StringComparison.Ordinal);
                if (section != 0)
                {
            	    return section;
                }
                
                int key = String.Compare(a.Entry.Definition.Key, b.Entry.Definition.Key, StringComparison.Ordinal);
                if (key != 0)
                {
            	    return key;
                }

                return 0;
            });
        }
        
        private static int SortNewSpecialAbilities(SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility a, SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility b)
        {
            var icons = ReadmeHelpers.GetAllNewStatInfoIcons();
            StatIconManager.FullStatIcon aStatIcon = icons.Find((icon) => icon.VariableStatBehavior == a.AbilityBehaviour);
            StatIconManager.FullStatIcon bStatIcon = icons.Find((icon) => icon.VariableStatBehavior == b.AbilityBehaviour);
            return String.Compare(aStatIcon.Info.rulebookName, bStatIcon.Info.rulebookName, StringComparison.Ordinal);
        }
        
        public override string GetSectionName()
        {
            return "New Configs";
        }

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (configs.Count > 0)
            {
                stringBuilder.Append($"- {configs.Count} {GetSectionName()}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(configs, out headers, new TableColumn<ConfigData>[]
            {
                new TableColumn<ConfigData>("GUID", (a)=>a.PluginGUID),
                new TableColumn<ConfigData>("Section", (a)=>a.Entry.Definition.Section),
                new TableColumn<ConfigData>("Key", (a)=>a.Entry.Definition.Key),
                new TableColumn<ConfigData>("Description", (a)=>a.Entry.Description.Description)
            });
        }
    }
}
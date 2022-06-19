using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using JamesGames.ReadmeMaker.Configs;
using UnityEngine;
using SpecialAbility = InscryptionAPI.Card.SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewConfigsSection : ASection
    {
        public override string SectionName => "New Configs";
        public override bool Enabled => ReadmeConfig.Instance.ConfigSectionEnabled;
        
        private List<ConfigData> configs = new List<ConfigData>();
        
        public override void Initialize()
        {
            configs.Clear(); // Clear so when we re-dump everything we don't double up
            
            foreach (BaseUnityPlugin plugin in GameObject.FindObjectsOfType<BaseUnityPlugin>())
            {
                if (plugin.Config == null)
                {
            	    continue;
                }

                string guid = plugin.Info.Instance.Info.Metadata.GUID;
                
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
                int guid = string.Compare(a.PluginGUID, b.PluginGUID, StringComparison.Ordinal);
                if (guid != 0)
                {
            	    return guid;
                }
                
                int section = string.Compare(a.Entry.Definition.Section, b.Entry.Definition.Section, StringComparison.Ordinal);
                if (section != 0)
                {
            	    return section;
                }
                
                int key = string.Compare(a.Entry.Definition.Key, b.Entry.Definition.Key, StringComparison.Ordinal);
                return key != 0 ? key : 0;
            });
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(configs, out headers, new TableColumn<ConfigData>[]
            {
                new TableColumn<ConfigData>("Section", (a)=>a.Entry.Definition.Section),
                new TableColumn<ConfigData>("Key", (a)=>a.Entry.Definition.Key),
                new TableColumn<ConfigData>("Description", (a)=>a.Entry.Description.Description)
            });
        }

        public override string GetGUID(object o)
        {
            ConfigData casted = (ConfigData)o;
            return casted.PluginGUID;
        }
    }
}
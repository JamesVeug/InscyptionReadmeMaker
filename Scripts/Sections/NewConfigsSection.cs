using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using JamesGames.ReadmeMaker.Configs;
using UnityEngine;
using SpecialAbility = InscryptionAPI.Card.SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewConfigsSection : ASection<ConfigData>
    {
        public override string SectionName => "New Configs";
        public override bool Enabled => ReadmeConfig.Instance.ConfigSectionShow;
        
        public override void Initialize()
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            
            foreach (BaseUnityPlugin plugin in GameObject.FindObjectsOfType<BaseUnityPlugin>())
            {
                if (plugin.Config == null)
                {
            	    continue;
                }

                string guid = plugin.Info.Instance.Info.Metadata.GUID;
                
#pragma warning disable CS0618
                ConfigEntryBase[] entries = plugin.Config.GetConfigEntries();
#pragma warning restore CS0618
                foreach (ConfigEntryBase definition in entries)
                {
                    rawData.Add(new ConfigData()
            	    {
            		    PluginGUID = guid,
            		    Entry = definition,
            	    });
                }
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<ConfigData>("Section", (a)=>a.Entry.Definition.Section),
                new TableColumn<ConfigData>("Key", (a)=>a.Entry.Definition.Key),
                new TableColumn<ConfigData>("Description", (a)=>a.Entry.Description.Description)
            });
        }

        public override string GetGUID(ConfigData o)
        {
            return o.PluginGUID;
        }

        protected override int Sort(ConfigData a, ConfigData b)
        {
            // Sort by
            // GUID
            // Section
            // Key
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
        }
    }
}
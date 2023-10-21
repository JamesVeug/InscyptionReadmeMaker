using System.Collections.Generic;
using BepInEx;
using DiskCardGame;

namespace JamesGames.ReadmeMaker
{
    public class RegisteredMod
    {
        public string PluginName;
        public string PluginGUID;
        
        public List<string> PluginCardModPrefixes = new List<string>();
        public Dictionary<CardInfo, CardChangeList> CardModifications = new Dictionary<CardInfo, CardChangeList>();
        public List<string> CardFieldModifications = new List<string>();

        private bool initializedWithPlugin = false;
        
        public RegisteredMod(ReadmeHelpers.ModManifest manifest)
        {
            PluginName = manifest.name;
            
            // Temporary.
            // Mods with a GUID can have multiple GUIDs but mods without them need a name
            PluginGUID = manifest.name;
        }

        public void Initialize(BaseUnityPlugin plugin)
        {
            if (initializedWithPlugin)
            {
                return;
            }

            initializedWithPlugin = true;
            PluginGUID = plugin.Info.Metadata.GUID;
            if (string.IsNullOrEmpty(PluginGUID))
            {
                Plugin.Log.LogError("Couldn't find GUID for plugin: " + PluginName);
            }
        }
        
        public void AddCardModPrefix(string modPrefix)
        {
            if (!PluginCardModPrefixes.Contains(modPrefix))
            {
                PluginCardModPrefixes.Add(modPrefix);
            }
        }

        public override string ToString()
        {
            return $"{PluginName}: ({PluginGUID})";
        }

        public bool IsModJSONLoader()
        {
            return PluginGUID == "MADH.inscryption.JSONLoader";
        }
    }
}
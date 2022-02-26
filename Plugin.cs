using System.Collections;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ReadmeMaker
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
	    public const string PluginGuid = "jamesgames.inscryption.readmemaker";
	    public const string PluginName = "Readme Maker";
	    public const string PluginVersion = "0.5.0.0";

        public static string Directory;
        public static ManualLogSource Log;
        public static Plugin Instance;
        public static ReadmeConfig ReadmeConfig;

        private void Awake()
        {
	        Log = Logger;
	        Instance = this;
	        ReadmeConfig = new ReadmeConfig();
	        if (!ReadmeConfig.ReadmeMakerEnabled)
	        {
		        Logger.LogInfo($"ReadmeMaker disabled in the Config so it will not generate a Readme.");
		        return;
	        }
	        
            Directory = this.Info.Location.Replace("ReadmeMaker.dll", "");
            
            
            Harmony harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
            Logger.LogInfo($"Loaded {PluginName}. Waiting for game to start before generating the readme...");
        }
    }
}

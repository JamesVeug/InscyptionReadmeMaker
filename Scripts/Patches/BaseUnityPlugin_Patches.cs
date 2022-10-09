using System;
using BepInEx;
using HarmonyLib;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(BaseUnityPlugin), MethodType.Constructor, new Type[] { })]
    public class BaseUnityPlugin_Constructor
    {
        public static void Postfix(BaseUnityPlugin __instance)
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }

            BepInPlugin metadata = MetadataHelper.GetMetadata((object) __instance);
            if (__instance.Info == null)
            {
                Plugin.Log.LogError("Ignoring mod: " + __instance.name + " Due to not having any info!");
                return;
            }
            
            PluginManager.Instance.RegisterPlugin(__instance);
        }
    }
}
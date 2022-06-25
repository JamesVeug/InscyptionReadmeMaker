using System;
using BepInEx;
using HarmonyLib;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(BaseUnityPlugin), MethodType.Constructor, new Type[] { })]
    public class ReadmeDump_Dump
    {
        public static void Prefix(BaseUnityPlugin __instance)
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }
            
            Plugin.Log.LogInfo("BaseUnityPlugin " + __instance.GetType().FullName);
            PluginManager.Instance.RegisterPlugin(__instance.GetType());
        }
    }
}
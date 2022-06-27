using System;
using BepInEx;
using HarmonyLib;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(BaseUnityPlugin), MethodType.Constructor, new Type[] { })]
    public class BaseUnityPlugin_Constructor
    {
        public static void Prefix(BaseUnityPlugin __instance)
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }
            
            PluginManager.Instance.RegisterPlugin(__instance.GetType());
        }
    }
}
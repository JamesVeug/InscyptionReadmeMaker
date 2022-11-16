using DiskCardGame;
using HarmonyLib;
using JamesGames.ReadmeMaker;

namespace API.Patches
{
    [HarmonyPatch(typeof(LoadingScreenManager), "LoadGameData")]
    public class LoadingScreenManager_LoadGameData
    {
        public static void Postfix()
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }
            
            ReadmeDump.Dump();
        }
    }
}
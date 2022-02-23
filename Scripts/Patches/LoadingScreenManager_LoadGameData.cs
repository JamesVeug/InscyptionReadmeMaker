using DiskCardGame;
using HarmonyLib;
using ReadmeMaker;

namespace API.Patches
{
    [HarmonyPatch(typeof(LoadingScreenManager), "LoadGameData")]
    public class LoadingScreenManager_LoadGameData
    {
        public static void Postfix()
        {
            ReadmeDump.Dump();
        }
    }
}
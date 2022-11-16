using DiskCardGame;
using HarmonyLib;
using InscryptionAPI;
using JamesGames.ReadmeMaker;

namespace API.Patches
{
    [HarmonyPatch(typeof(InscryptionAPIPlugin), "ResyncAll")]
    public class InscryptionAPIPlugin_ResyncAll
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
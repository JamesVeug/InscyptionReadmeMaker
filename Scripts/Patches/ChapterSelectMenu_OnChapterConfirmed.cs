using DiskCardGame;
using HarmonyLib;
using JamesGames.ReadmeMaker;

namespace API.Patches
{
    [HarmonyPatch(typeof(ChapterSelectMenu), "OnChapterConfirmed")]
    public class ChapterSelectMenu_OnChapterConfirmed
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
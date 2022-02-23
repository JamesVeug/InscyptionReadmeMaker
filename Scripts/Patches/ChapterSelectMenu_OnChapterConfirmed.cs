using DiskCardGame;
using HarmonyLib;
using ReadmeMaker;

namespace API.Patches
{
    [HarmonyPatch(typeof(ChapterSelectMenu), "OnChapterConfirmed")]
    public class ChapterSelectMenu_OnChapterConfirmed
    {
        public static void Postfix()
        {
            ReadmeDump.Dump();
        }
    }
}
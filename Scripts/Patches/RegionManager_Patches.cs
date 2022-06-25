using System.Collections.Generic;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Regions;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(RegionManager), "Add")]
    public class RegionManager_Add
    {
        public static Dictionary<Part1RegionData, string> RegionToGUIDLookup = new Dictionary<Part1RegionData, string>();

        public static void Postfix(RegionData newRegion, int tier)
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }
            
            Part1RegionData lastRegion = RegionManager.NewRegions[RegionManager.NewRegions.Count - 1];

            Assembly callingAssembly = Assembly.GetCallingAssembly();
            lastRegion.SetModTag(ReadmeHelpers.GetModIdFromCallstack(callingAssembly));
        }
    }

    public static class RegionManager_Extensions
    {
        public static void SetModTag(this Part1RegionData info, string modGuid)
        {
            RegionManager_Add.RegionToGUIDLookup[info] = modGuid;
        }

        public static string GetModTag(this Part1RegionData info)
        {
            return RegionManager_Add.RegionToGUIDLookup[info];
        }
    }
}
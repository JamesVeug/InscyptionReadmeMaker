using System.Collections.Generic;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Encounters;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(EncounterManager), "Add")]
    public class EncounterManager_Add
    {
        public static Dictionary<EncounterBlueprintData, string> EncounterToGUIDLookup = new Dictionary<EncounterBlueprintData, string>();

        public static void Postfix(EncounterBlueprintData newEncounter)
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }
            
            EncounterBlueprintData lastRegion = EncounterManager.NewEncounters[EncounterManager.NewEncounters.Count - 1];
                
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            lastRegion.SetModTag(ReadmeHelpers.GetModIdFromCallstack(callingAssembly));
        }
    }
    

    public static class EncounterManager_Extensions
    {
        public static void SetModTag(this EncounterBlueprintData info, string modGuid)
        {
            EncounterManager_Add.EncounterToGUIDLookup[info] = modGuid;
        }

        public static string GetModTag(this EncounterBlueprintData info)
        {
            return EncounterManager_Add.EncounterToGUIDLookup[info];
        }
    }
}
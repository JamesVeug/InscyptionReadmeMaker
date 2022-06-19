using System.Collections.Generic;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Guid;
using InscryptionAPI.Regions;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(EncounterManager), "Add")]
    public class EncounterManager_Add
    {
        public static Dictionary<EncounterBlueprintData, string> EncounterToGUIDLookup = new Dictionary<EncounterBlueprintData, string>();

        public static void Postfix(EncounterBlueprintData newEncounter)
        {
            EncounterBlueprintData lastRegion = EncounterManager.NewEncounters[EncounterManager.NewEncounters.Count - 1];
            
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            lastRegion.SetModTag(TypeManager.GetModIdFromCallstack(callingAssembly));
        }
    }

    public static class EncounterManager_Extensions
    {
        public static void SetModTag(this EncounterBlueprintData info, string modGuid)
        {
            Plugin.Log.LogInfo("EncounterBlueprintData GUID = " + modGuid);
            EncounterManager_Add.EncounterToGUIDLookup[info] = modGuid;
        }

        public static string GetModTag(this EncounterBlueprintData info)
        {
            return EncounterManager_Add.EncounterToGUIDLookup[info];
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Guid;
using JamesGames.ReadmeMaker;
using UnityEngine;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(TribeManager), "Add", new Type[] {typeof(string), typeof(string), typeof(Texture2D), typeof(bool), typeof(Texture2D)})]
    public class TribeManager_Add
    {
        public static Dictionary<Tribe, string> TribeToGUIDLookup = new Dictionary<Tribe, string>();
        public static Dictionary<Tribe, string> TribeToNameLookup = new Dictionary<Tribe, string>();

        public static void Postfix(string guid, string name, Texture2D tribeIcon, bool appearInTribeChoices, Texture2D choiceCardbackTexture)
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }
            
            TribeManager.TribeInfo lastTribe = TribeManager.tribes[TribeManager.tribes.Count - 1];
            lastTribe.tribe.SetModTag(guid);
            lastTribe.tribe.SetName(name);
        }
    }
    

    public static class TribeInfo_Extensions
    {
        public static void SetModTag(this Tribe info, string modGuid)
        {
            TribeManager_Add.TribeToGUIDLookup[info] = modGuid;
        }

        public static string GetModTag(this Tribe info)
        {
            if (TribeManager_Add.TribeToGUIDLookup.TryGetValue(info, out string g))
            {
                return g;
            }

            return null;
        }
        
        public static void SetName(this Tribe info, string modGuid)
        {
            TribeManager_Add.TribeToNameLookup[info] = modGuid;
        }

        public static string GetName(this Tribe info)
        {
            if (TribeManager_Add.TribeToNameLookup.TryGetValue(info, out string n))
            {
                return n;
            }

            return null;
        }
    }
}
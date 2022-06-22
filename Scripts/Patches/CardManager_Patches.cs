using System;
using System.Collections.Generic;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(CardManager), "Add", new Type[] { typeof(CardInfo)})]
    public class CardManager_Add
    {
        public static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic;
        public static readonly Dictionary<CardInfo, Dictionary<string, object>> ModifiedCardLookup = new Dictionary<CardInfo, Dictionary<string, object>>();

        public static void Postfix(CardInfo newCard)
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }
            // TODO: Cache new cards when added to CardManager
            // TODO: Cache modifications if the card was not added during the current Plugin
            // TODO: Store all base cards when the readme maker begins 
            
            Dictionary<string, object> stats = new Dictionary<string, object>();
            //Plugin.Log.LogInfo("Card added: " + newCard.displayedName);
            foreach (FieldInfo fieldInfo in newCard.GetType().GetFields(BindingFlags))
            {
                object value = fieldInfo.GetValue(newCard);
                //Plugin.Log.LogInfo("\t" + fieldInfo.Name + " = " + value);
                stats[fieldInfo.Name] = value;
            }

            ModifiedCardLookup[newCard] = stats;
        }
    }
}
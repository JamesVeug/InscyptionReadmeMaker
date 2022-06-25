using System;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(CardManager), "Add", new Type[] { typeof(CardInfo)})]
    public class CardManager_Add
    {
        public static void Postfix(CardInfo newCard)
        {
            if (!ReadmeConfig.Instance.ReadmeMakerEnabled)
            {
                return;
            }
            PluginManager.Instance.AddNewCard(newCard);
        }
    }
}
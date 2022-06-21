using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using InscryptionAPI.Encounters;
using JamesGames.ReadmeMaker;

namespace ReadmeMaker.Patches
{
    [HarmonyPatch(typeof(NodeManager.NodeInfo), MethodType.Constructor, new Type[] { })]
    public class NodeManager_Add
    {
        public static Dictionary<NodeManager.NodeInfo, string> NodeToGUIDLookup = new Dictionary<NodeManager.NodeInfo, string>();

        public static void Postfix(NodeManager.NodeInfo __instance)
        {
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            __instance.SetModTag(ReadmeHelpers.GetModIdFromCallstack(callingAssembly));
        }
    }

    public static class NodeManager_Extensions
    {
        public static void SetModTag(this NodeManager.NodeInfo info, string modGuid)
        {
            NodeManager_Add.NodeToGUIDLookup[info] = modGuid;
        }

        public static string GetModTag(this NodeManager.NodeInfo info)
        {
            return NodeManager_Add.NodeToGUIDLookup[info];
        }
    }
}
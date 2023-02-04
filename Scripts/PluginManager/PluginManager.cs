using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using DiskCardGame;
using InscryptionAPI.Card;
using JamesGames.ReadmeMaker.Sections;
using ReadmeMaker.Scripts.Utils;

namespace JamesGames.ReadmeMaker
{
    public class Modification
    {
        public string DisplayString => OldData + "<br>=><br>" + NewData;
        
        public string OldData;
        public string NewData;
    }

    public class CardChangeDetails
    {
        // Card changed
        public CardInfo CardInfo;
        
        // Mod that made these changes
        public string ModGUID;
        
        // How many times this card has been changed
        public int ChangeIndex;
        
        // Header Name to data
        public Dictionary<string, Modification> Modifications = new Dictionary<string, Modification>();
    }

    public class CardChangeList : List<CardChangeDetails>{}
    
    public class PluginManager
    {
        public static PluginManager Instance
        {
            get{
                if (m_instance == null)
                {
                    m_instance = new PluginManager();
                }

                return m_instance;
            }
        }
        private static PluginManager m_instance = null;
        private static List<CardInfoGetterInfo> m_cardFields = GetCardModificationGetters();

        public List<RegisteredMod> RegisteredMods => registeredMods;
        public RegisteredMod ModBeingProcess => modBeingProcess;
        
        // A mod has added a new card to the game
        private List<CardInfo> cardsLoadedDuringPlugin = new List<CardInfo>();
        private List<RegisteredMod> registeredMods = new List<RegisteredMod>();
        private Dictionary<string, RegisteredMod> registeredModLookup = new Dictionary<string, RegisteredMod>();

        // A mod has finished loading and another has started.
        // This is the list of all cards processed for a previous mod
        private Dictionary<CardInfo, Dictionary<string, object>> cardDefaultValues = new Dictionary<CardInfo, Dictionary<string, object>>();

        private bool flushProcessing = false;
        private RegisteredMod modBeingProcess = null;

        public PluginManager()
        {
            cardsLoadedDuringPlugin.AddRange(CardManager.BaseGameCards);
            Plugin.Log.LogInfo("[PluginManager] Added base cards: " + cardsLoadedDuringPlugin.Count);
            Flush(true);
            
            RegisterPlugin(Plugin.Instance);
        }
        
        public void RegisterPlugin(BaseUnityPlugin instance)
        {
            string infoLocation = instance.Info.Location;
            ReadmeHelpers.ModManifest manifest = ReadmeHelpers.GetManifestFromPath(infoLocation);
            
            if (!registeredModLookup.TryGetValue(manifest.name, out RegisteredMod registeredMod))
            {
                registeredMod = new RegisteredMod(manifest);
                registeredMods.Add(registeredMod);
                registeredModLookup[manifest.name] = registeredMod;
            }
            registeredMod.Initialize(instance);
            
            
            
            Plugin.Log.LogInfo($"[PluginManager] Registering newly processed mod: {registeredMod.PluginName}({registeredMod.PluginGUID})");
            Flush();
            
            modBeingProcess = registeredMod;
        }

        public void AddNewCard(CardInfo info)
        {
            cardsLoadedDuringPlugin.Add(info);

            // Register JSONLoaded mod
            string JSONFilePath = info.GetExtendedProperty("JSONFilePath");
            if (!string.IsNullOrEmpty(JSONFilePath))
            {
                ReadmeHelpers.ModManifest manifest = ReadmeHelpers.GetManifestFromPath(JSONFilePath);

                if (!registeredModLookup.TryGetValue(manifest.name, out RegisteredMod registeredMod))
                {
                    registeredMod = new RegisteredMod(manifest);
                    registeredMods.Add(registeredMod);
                    registeredModLookup[manifest.name] = registeredMod;
                }
                
                string modPrefix = info.GetModPrefix();
                if (!string.IsNullOrEmpty(modPrefix))
                {
                    registeredMod.AddCardModPrefix(modPrefix);
                }
                else
                {
                    Plugin.Log.LogError($"[PluginManager] Detected JSONLoaded card without modPrefix! " + info.displayedName);
                }
            }
        }

        public void Flush(bool forceFlush = false)
        {
            if (!forceFlush && (modBeingProcess == null || flushProcessing))
            {
                // No mod data to flush
                return;
            }

            flushProcessing = true;
            FlushCardModifications();
            FlushNewlyAddedCards();
            modBeingProcess = null;
            flushProcessing = false;
        }

        private void FlushCardModifications()
        {
            foreach (KeyValuePair<CardInfo,Dictionary<string,object>> pair in cardDefaultValues)
            {
                CardInfo cardInfo = pair.Key;
                CardChangeDetails cardChangeDetails = null;
                Dictionary<string,object> originalData = pair.Value;
                RegisteredMod cardMod = null;

                foreach (KeyValuePair<string,object> dataPair in originalData)
                {
                    string fieldName = dataPair.Key;
                    object fieldData = dataPair.Value;
                    object newData = m_cardFields.Find((a)=>a.HeaderName == fieldName).Getter(cardInfo);
                    if (!AreEquals(fieldData, newData))
                    {
                        if (cardMod == null)
                        {
                            cardMod = modBeingProcess.IsModJSONLoader() ? GetRegisteredModFromCard(cardInfo) : modBeingProcess;
                            if (cardMod == null)
                            {
                                string JSONFilePath = cardInfo.GetExtendedProperty("JSONFilePath");
                                Plugin.Log.LogInfo(
                                    $"Cannot flush modifications for card {cardInfo.displayedName}. prefix: '{cardInfo.GetModPrefix()}' JSONFilePath: '{JSONFilePath}'");
                                break;
                            }
                        }

                        //Plugin.Log.LogInfo($"{cardInfo.displayedName} - {fieldName} modified from '{fieldData}' to '{newData}'");
                        if (cardChangeDetails == null)
                        {
                            if(!cardMod.CardModifications.TryGetValue(cardInfo, out var m))
                            {
                                m = new CardChangeList();
                                cardMod.CardModifications[cardInfo] = m;
                            }
                            
                            cardChangeDetails = new CardChangeDetails()
                            {
                                CardInfo = cardInfo,
                                ChangeIndex = m.Count + 1,
                                ModGUID = cardMod.PluginGUID
                            };
                            m.Add(cardChangeDetails);
                        }
                        
                        cardChangeDetails.Modifications[fieldName] = new Modification()
                        {
                            OldData = ReadmeHelpers.ConvertToString(fieldData),
                            NewData = ReadmeHelpers.ConvertToString(newData)
                        };

                        if (!cardMod.CardFieldModifications.Contains(fieldName))
                        {
                            cardMod.CardFieldModifications.Add(fieldName);
                        }
                        //Plugin.Log.LogInfo($"Flushed modifications for card {cardInfo.displayedName} to mod '" + cardMod.PluginName + "'");
                    }
                    else
                    {
                        //Plugin.Log.LogInfo($"{cardInfo.displayedName} - {fieldName} not modified from '{ConvertToString(fieldData)}' to '{ConvertToString(newData)}'");
                    }
                }

                CacheCardDefaultValues(cardInfo);
            }
        }

        private RegisteredMod GetRegisteredModFromCard(CardInfo cardInfo)
        {
            Plugin.Log.LogInfo($"[PluginManager] GetRegisteredModFromCard {cardInfo.displayedName}");
            string modPrefix = cardInfo.GetModPrefix();
            if (!string.IsNullOrEmpty(modPrefix))
            {
                // Try using mod prefix
                foreach (RegisteredMod mod in registeredMods)
                {
                    if (mod.PluginCardModPrefixes.Contains(modPrefix))
                    {
                        return mod;
                    }
                }
            }

            // Lookup by JSON path manifest
            string JSONFilePath = cardInfo.GetExtendedProperty("JSONFilePath");
            if (!string.IsNullOrEmpty(JSONFilePath))
            {
                ReadmeHelpers.ModManifest manifest = ReadmeHelpers.GetManifestFromPath(JSONFilePath);
                if (registeredModLookup.TryGetValue(manifest.name, out RegisteredMod registeredMod))
                {
                    Plugin.Log.LogInfo($"[PluginManager] GetRegisteredModFromCard Got manifest {manifest.name}");
                    return registeredMod;
                }
                else
                {
                    Plugin.Log.LogInfo($"[PluginManager] Could not get manifest from path {JSONFilePath}");
                }
            }
            else
            {
                Plugin.Log.LogInfo($"[PluginManager] Card does not have a JSONFilePath!");
            }

            // Base card?
            return null;
        }

        private void FlushNewlyAddedCards()
        {
            if (cardsLoadedDuringPlugin.Count == 0)
            {
                return;
            }

            Plugin.Log.LogInfo($"[PluginManager] Flushing {cardsLoadedDuringPlugin.Count} new cards from " + modBeingProcess);
            foreach (CardInfo newCard in cardsLoadedDuringPlugin)
            {
                string modPrefix = newCard.GetModPrefix();
                if (!string.IsNullOrEmpty(modPrefix))
                {
                    modBeingProcess.AddCardModPrefix(modPrefix);
                }

                CacheCardDefaultValues(newCard);
            }
            cardsLoadedDuringPlugin.Clear();
        }

        private void CacheCardDefaultValues(CardInfo newCard)
        {
            if (!cardDefaultValues.TryGetValue(newCard, out Dictionary<string, object> stats))
            {
                stats = new Dictionary<string, object>();
                cardDefaultValues[newCard] = stats;
                //Plugin.Log.LogInfo("Caching defaults for " + newCard.displayedName);
            }
            
            //Plugin.Log.LogInfo("Card added: " + newCard.displayedName);
            foreach (CardInfoGetterInfo fieldInfo in m_cardFields)
            {
                object value = fieldInfo.Getter(newCard);
                if (ReadmeHelpers.IsList(value))
                {
                    IList valueList = (IList)value;
                    IList newValue = CloneList(valueList);
                    value = newValue;
                }
                
                //Plugin.Log.LogInfo("\t" + fieldInfo.Name + " = " + value);
                stats[fieldInfo.HeaderName] = value;
            }
        }

        private static IList CloneList(IList list)
        {
            Type itemType = list.GetType().GetGenericArguments()[0];
            Type genericListType = typeof(List<>).MakeGenericType(itemType);
            IList instance = (IList)Activator.CreateInstance(genericListType);
            for (int i = 0; i < list.Count; i++)
            {
                instance.Add(list[i]);
            }
            return instance;
        }
        
        private bool AreEquals(object a, object b)
        {
            if (a == null)
            {
                return b == null;
            }
            if(b == null)
            {
                return false;
            }

            if (ReadmeHelpers.IsList(a) || ReadmeHelpers.IsList(b))
            {
                IList aList = (IList)a;
                IList bList = (IList)b;
                if (aList.Count != bList.Count)
                {
                    return false;
                }

                for (int i = 0; i < aList.Count; i++)
                {
                    if (!AreEquals(aList[i], bList[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return a.Equals(b);
        }

        private class CardInfoGetterInfo
        {
            public string HeaderName;
            public Func<CardInfo, object> Getter;
        }
        
        private static List<CardInfoGetterInfo> GetCardModificationGetters()
        {
            List<CardInfoGetterInfo> data = new List<CardInfoGetterInfo>();
            data.Add(new CardInfoGetterInfo()
            {
                HeaderName = "Name",
                Getter = (a)=>a.name
            });
            
            foreach (TableColumn<CardInfo> columns in SectionUtils.GetCardTableColumns())
            {
                string headerName = columns.HeaderName;
                if (headerName == "Name")
                {
                    headerName = "Display Name";
                }
                
                data.Add(new CardInfoGetterInfo()
                {
                    HeaderName = headerName,
                    Getter = columns.Getter
                });
            }

            return data;
        }
    }
}
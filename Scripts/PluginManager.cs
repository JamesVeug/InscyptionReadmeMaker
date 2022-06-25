using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using DiskCardGame;
using InscryptionAPI.Card;

namespace JamesGames.ReadmeMaker
{
    public class Modification
    {
        public string DisplayString => OldData + " -> " + NewData;
        
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
        
        // FieldName to data
        public Dictionary<string, Modification> Modifications = new Dictionary<string, Modification>();
    }
    
    public class CardChangeList : List<CardChangeDetails>{}
    
    public class PluginManager
    {
        public static readonly BindingFlags ModificationBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default | BindingFlags.NonPublic;
        
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
        private static FieldInfo[] m_cardFields = typeof(CardInfo).GetFields(ModificationBindingFlags);

        // A mod has added a new card to the game
        private List<CardInfo> cardsLoadedDuringPlugin = new List<CardInfo>();
        
        // A mod has finished loading and another has started.
        // This is the list of all cards processed for a previous mod
        // TODO: Refresh default values after processing a mod so the next mod doesn't show the same modifications
        private Dictionary<CardInfo, Dictionary<string, object>> cardDefaultValues = new Dictionary<CardInfo, Dictionary<string, object>>();
        private Dictionary<CardInfo, CardChangeList> cardModifications = new Dictionary<CardInfo, CardChangeList>();
        private List<string> cardFieldModifications = new List<string>();

        private bool flushProcessing = false;
        private string modBeingProcess = null;

        public PluginManager()
        {
            cardsLoadedDuringPlugin.AddRange(CardManager.BaseGameCards);
            Plugin.Log.LogInfo("[PluginManager] Added base cards: " + cardsLoadedDuringPlugin.Count);
            Flush(true);
        }
        
        public void RegisterPlugin(Type type)
        {
            Plugin.Log.LogInfo("[PluginManager] Registering newly processed mod: " + type.FullName);
            Flush();
            modBeingProcess = GetPluginGUID(type);
        }
        
        public static string GetPluginGUID(Type type)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(type);
  
            foreach (Attribute attr in attrs)  
            {  
                if (attr is BepInPlugin plugin)  
                {
                    //Plugin.Log.LogInfo("\t2 Found GUID " + plugin.GUID + "!");
                    return plugin.GUID;
                }  
            }  

            Plugin.Log.LogInfo("Couldn't find GUID!");
            return null;
        }

        public void AddNewCard(CardInfo info)
        {
            cardsLoadedDuringPlugin.Add(info);
        }

        public Dictionary<CardInfo, Dictionary<string, object>> GetCardDefaultValues()
        {
            Plugin.Log.LogInfo("[PluginManager] Getting default values: " + cardDefaultValues.Count);
            return cardDefaultValues;
        }

        public Dictionary<CardInfo, CardChangeList> GetModifications()
        {
            Plugin.Log.LogInfo("[PluginManager] Getting card modifications: " + cardModifications.Count);
            return cardModifications;
        }

        public List<string> GetCardModifiedFieldNames()
        {
            Plugin.Log.LogInfo("[PluginManager] Getting card modified field names: " + cardFieldModifications.Count);
            return cardFieldModifications;
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
            Type type = typeof(CardInfo);
            foreach (KeyValuePair<CardInfo,Dictionary<string,object>> pair in cardDefaultValues)
            {
                CardInfo cardInfo = pair.Key;
                CardChangeDetails cardChangeDetails = null;
                Dictionary<string,object> originalData = pair.Value;

                foreach (KeyValuePair<string,object> dataPair in originalData)
                {
                    string fieldName = dataPair.Key;
                    object fieldData = dataPair.Value;
                    object newData = type.GetField(fieldName, ModificationBindingFlags)?.GetValue(cardInfo);
                    if (!AreEquals(fieldData, newData))
                    {
                        Plugin.Log.LogInfo($"{cardInfo.displayedName} - {fieldName} modified from '{fieldData}' to '{newData}'");

                        if (cardChangeDetails == null)
                        {
                            if(!cardModifications.TryGetValue(cardInfo, out var m))
                            {
                                m = new CardChangeList();
                                cardModifications[cardInfo] = m;
                            }
                            
                            cardChangeDetails = new CardChangeDetails()
                            {
                                CardInfo = cardInfo,
                                ChangeIndex = m.Count + 1,
                                ModGUID = modBeingProcess
                            };
                            m.Add(cardChangeDetails);
                        }
                        
                        cardChangeDetails.Modifications[fieldName] = new Modification()
                        {
                            OldData = ReadmeHelpers.ConvertToString(fieldData),
                            NewData = ReadmeHelpers.ConvertToString(newData)
                        };

                        if (!cardFieldModifications.Contains(fieldName))
                        {
                            cardFieldModifications.Add(fieldName);
                        }
                    }
                    else
                    {
                        //Plugin.Log.LogInfo($"{cardInfo.displayedName} - {fieldName} not modified from '{ConvertToString(fieldData)}' to '{ConvertToString(newData)}'");
                    }
                }

                CacheCardDefaultValues(cardInfo);
            }
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
            foreach (FieldInfo fieldInfo in m_cardFields)
            {
                object value = fieldInfo.GetValue(newCard);
                if (ReadmeHelpers.IsList(value))
                {
                    IList valueList = (IList)value;
                    IList newValue = CloneList(valueList);
                    value = newValue;
                }
                
                //Plugin.Log.LogInfo("\t" + fieldInfo.Name + " = " + value);
                stats[fieldInfo.Name] = value;
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
    }
}
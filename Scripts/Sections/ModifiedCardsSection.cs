using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using EasyFeedback.APIs;
using InscryptionAPI.Card;
using ReadmeMaker.Patches;

namespace JamesGames.ReadmeMaker.Sections
{
    public class Modification
    {
        public string DisplayString => OldData + " -> " + NewData;
        
        public string OldData;
        public string NewData;
    }

    public class ModifiedCard
    {
        public CardInfo CardInfo;
        public Dictionary<string, Modification> Modifications = new Dictionary<string, Modification>();
    }
    
    public class ModifiedCardsSection : ASection<ModifiedCard>
    {
        public override string SectionName => "Modified Cards";
        public override bool Enabled => ReadmeConfig.Instance.ModifiedCardsShow;

        private List<string> Modifications = new List<string>();
        
        public override void Initialize()
        {
            rawData.Clear();
            Modifications.Clear();

            
            Type type = typeof(CardInfo);
            foreach (KeyValuePair<CardInfo,Dictionary<string,object>> pair in CardManager_Add.ModifiedCardLookup)
            {
                CardInfo cardInfo = pair.Key;
                ModifiedCard modifiedCard = null;
                Dictionary<string,object> originalData = pair.Value;

                foreach (KeyValuePair<string,object> dataPair in originalData)
                {
                    string fieldName = dataPair.Key;
                    object fieldData = dataPair.Value;
                    object newData = type.GetField(fieldName, CardManager_Add.BindingFlags)?.GetValue(cardInfo);
                    if (!AreEquals(fieldData, newData))
                    {
                        Plugin.Log.LogInfo($"{cardInfo.displayedName} - {fieldName} modified from '{fieldData}' to '{newData}'");

                        if (modifiedCard == null)
                        {
                            modifiedCard = new ModifiedCard(){CardInfo = cardInfo};
                            rawData.Add(modifiedCard);
                        }
                        
                        modifiedCard.Modifications[fieldName] = new Modification()
                        {
                            OldData = ConvertToString(fieldData),
                            NewData = ConvertToString(newData)
                        };

                        if (!Modifications.Contains(fieldName))
                        {
                            Modifications.Add(fieldName);
                        }
                    }
                }
            }
            Modifications.Sort();
        }

        private string ConvertToString(object o)
        {
            if (o == null)
            {
                return "null";
            }

            if (IsList(o))
            {
                IList l = (IList)o;
                string s = "";
                for (int i = 0; i < l.Count; i++)
                {
                    object element = l[i];
                    if (i > 0)
                    {
                        s += ",";
                    }

                    s += ConvertToString(element);
                }

                return s;
            }

            if (o.GetType() == typeof(Ability))
            {
                AbilityInfo abilityInfo = ReadmeHelpers.GetAbilityInfo((Ability)o);
                return abilityInfo.rulebookName;
            }

            if (o.GetType() == typeof(Tribe))
            {
                return ReadmeHelpers.GetTribeName((Tribe)o);
            }

            if (o.GetType() == typeof(Trait))
            {
                return ReadmeHelpers.GetTraitName((Trait)o);
            }

            return o.ToString();
        }
        
        public bool IsList(object o)
        {
            if(o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public bool IsDictionary(object o)
        {
            if(o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }

        public override void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows)
        {
            List<TableColumn<ModifiedCard>> columns = new List<TableColumn<ModifiedCard>>();
            columns.Add(new TableColumn<ModifiedCard>("Name", (a) => a.CardInfo.displayedName));

            foreach (string modification in Modifications)
            {
                columns.Add(new TableColumn<ModifiedCard>(modification,
                    delegate(ModifiedCard a)
                    {
                        if (a.Modifications.TryGetValue(modification, out Modification subModification))
                        {
                            return subModification.DisplayString;
                        }

                        return "";
                    }));
            }
            
            
            rows = BreakdownForTable(out tableHeaders, columns.ToArray());
        }

        protected override int Sort(ModifiedCard a, ModifiedCard b)
        {
            switch (ReadmeConfig.Instance.CardSortBy)
            {
                case ReadmeConfig.CardSortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.CardSortByType.Name:
                    return ReadmeHelpers.CompareByDisplayName(a.CardInfo, b.CardInfo);
                case ReadmeConfig.CardSortByType.Cost:
                    return ReadmeHelpers.CompareByCost(a.CardInfo, b.CardInfo);
                case ReadmeConfig.CardSortByType.Power:
                    return a.CardInfo.Attack - b.CardInfo.Attack;
                case ReadmeConfig.CardSortByType.Health:
                    return a.CardInfo.Health - b.CardInfo.Health;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string GetGUID(ModifiedCard o)
        {
            return o.CardInfo.GetModTag();
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

            if (IsList(a) || IsList(b))
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
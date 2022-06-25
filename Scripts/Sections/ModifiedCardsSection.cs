using System.Collections.Generic;
using DiskCardGame;

namespace JamesGames.ReadmeMaker.Sections
{
    public class ModifiedCardsSection : ASection<CardChangeDetails>
    {
        public override string SectionName => "Modified Cards";
        public override bool Enabled => ReadmeConfig.Instance.ModifiedCardsShow;

        private List<string> cardModifiedFieldNames = new List<string>();
        
        public override void Initialize()
        {
            rawData.Clear();
            cardModifiedFieldNames.Clear();

            PluginManager.Instance.Flush();
            cardModifiedFieldNames.AddRange(PluginManager.Instance.GetCardModifiedFieldNames());
            cardModifiedFieldNames.Sort();
            
            foreach (KeyValuePair<CardInfo, CardChangeList> cardChanges in PluginManager.Instance.GetModifications())
            {
                rawData.AddRange(cardChanges.Value);
            }
        }

        public override void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows)
        {
            List<TableColumn<CardChangeDetails>> columns = new List<TableColumn<CardChangeDetails>>();
            columns.Add(new TableColumn<CardChangeDetails>("Name", (a) =>
            {
                if (a.Modifications.TryGetValue("name", out Modification data))
                {
                    return data.OldData;
                }

                return a.CardInfo.name;
            }));

            foreach (string fieldName in cardModifiedFieldNames)
            {
                columns.Add(new TableColumn<CardChangeDetails>(fieldName,
                    delegate(CardChangeDetails a)
                    {
                        if (a.Modifications.TryGetValue(fieldName, out Modification subModification))
                        {
                            return subModification.DisplayString;
                        }

                        return "";
                    }));
            }
            
            
            rows = BreakdownForTable(out tableHeaders, columns.ToArray());
        }

        protected override int Sort(CardChangeDetails a, CardChangeDetails b)
        {
            int nameCompare = ReadmeHelpers.CompareByDisplayName(a.CardInfo, b.CardInfo);
            if (nameCompare != 0)
            {
                return nameCompare;
            }
            
            return a.ChangeIndex - b.ChangeIndex;
        }

        public override string GetGUID(CardChangeDetails o)
        {
            return o.ModGUID;
        }
    }
}
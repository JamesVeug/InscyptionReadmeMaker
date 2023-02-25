using System;
using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Card;
using ReadmeMaker.Scripts.Utils;

namespace JamesGames.ReadmeMaker.Sections
{
    public class ModifiedCardsSection : ASection<CardChangeDetails>
    {
        public override string SectionName => "Modified Cards";
        public override bool Enabled => ReadmeConfig.Instance.ModifiedCardsShow;

        private List<string> cardModifiedFieldNames = new List<string>();
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear();
            cardModifiedFieldNames.Clear();

            PluginManager.Instance.Flush();
            cardModifiedFieldNames.AddRange(mod.CardFieldModifications);
            cardModifiedFieldNames.Sort();
            
            foreach (KeyValuePair<CardInfo, CardChangeList> cardChanges in mod.CardModifications)
            {
                rawData.AddRange(cardChanges.Value);
            }
        }

        public override void GetTableDump(out List<TableHeader> tableHeaders, out List<Dictionary<string, string>> rows)
        {
            List<TableColumn<CardChangeDetails>> columns = new List<TableColumn<CardChangeDetails>>();
            List<TableColumn<CardInfo>> tableColumns = SectionUtils.GetCardTableColumns();

            foreach (TableColumn<CardInfo> column in tableColumns)
            {
                string columnHeaderName = column.HeaderName;
                if (columnHeaderName != "Name" && columnHeaderName != "ModPrefix" && 
                    !cardModifiedFieldNames.Contains(columnHeaderName) && ReadmeConfig.Instance.ModifiedCardsOnlyShowChanges)
                {
                    // This field was never changed and we don't want to see any
                    continue;
                }

                if (columnHeaderName == "Name")
                {
                    columnHeaderName = "Display Name";
                }
                
                TableColumn<CardChangeDetails> c = new TableColumn<CardChangeDetails>(columnHeaderName, (a)=>
                {
                    if (a.Modifications.TryGetValue(columnHeaderName, out Modification subModification))
                    {
                        return subModification.DisplayString;
                    }

                    return column.Getter(a.CardInfo);
                }, column.Enabled, column.Alignment);
                columns.Add(c);
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
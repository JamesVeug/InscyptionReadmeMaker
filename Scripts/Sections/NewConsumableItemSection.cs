using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using FullAbility = InscryptionAPI.Card.AbilityManager.FullAbility;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewConsumableItemSection : ASection<ConsumableItemData>
    {
        public override string SectionName => "New Consumable Items";
        public override bool Enabled => ReadmeConfig.Instance.ConsumableItemsShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(ConsumableItemManager.NewConsumableItemDatas.Where(x=>GetGUID(x) == mod.PluginGUID));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<ConsumableItemData>("Name", (x)=>x.rulebookName),
                new TableColumn<ConsumableItemData>("Description", (x)=>RuleBookPage.ParseCardDefinition(x.rulebookDescription)),
                new TableColumn<ConsumableItemData>("Randomly Given", (x)=>(!x.notRandomlyGiven) ? "Yes" : ""),
                new TableColumn<ConsumableItemData>("Region Specific", (x)=>(x.regionSpecific) ? "Yes" : ""),
                new TableColumn<ConsumableItemData>("Power Level", (x)=>x.powerLevel.ToString()),
            });
        }

        public override string GetGUID(ConsumableItemData o)
        {
            return o.GetModPrefix();
        }

        protected override int Sort(ConsumableItemData a, ConsumableItemData b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.rulebookName, b.rulebookName, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
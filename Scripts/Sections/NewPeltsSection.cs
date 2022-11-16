using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Pelts;
using FullAbility = InscryptionAPI.Card.AbilityManager.FullAbility;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewPeltsSection : ASection<ICustomPeltData>
    {
        public override string SectionName => "New Pelts";
        public override bool Enabled => ReadmeConfig.Instance.PeltsShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(PeltManager.AllNewPelts.Where(x=>GetGUID(x) == mod.PluginGUID));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<ICustomPeltData>("Name", GetNameOfCard),
                new TableColumn<ICustomPeltData>("Cost", (a)=>a.Cost().ToString()),
                new TableColumn<ICustomPeltData>("Total Sigils", (a)=>a.AbilityCount.ToString()),
                new TableColumn<ICustomPeltData>("Available At Trader", (a)=>a.AvailableAtTrader.ToString()),
                new TableColumn<ICustomPeltData>("Cards To Choose", (a)=>a.MaxChoices.ToString()),
                new TableColumn<ICustomPeltData>("Cards In Pool", (a)=>a.GetChoices().Count.ToString()),
            });
        }

        private string GetNameOfCard(ICustomPeltData arg)
        {
            CardInfo cardByName = CardLoader.GetCardByName(arg.CardNameOfPelt);
            return cardByName.displayedName;
        }

        public override string GetGUID(ICustomPeltData o)
        {
            return o.PluginGUID;
        }

        protected override int Sort(ICustomPeltData a, ICustomPeltData b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.CardNameOfPelt, b.CardNameOfPelt, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
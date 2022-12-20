using System;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Pelts;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewPeltsSection : ASection<PeltManager.CustomPeltData>
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
                new TableColumn<PeltManager.CustomPeltData>("Name", GetNameOfCard),
                new TableColumn<PeltManager.CustomPeltData>("Cost", (a)=>a.Cost().ToString()),
                new TableColumn<PeltManager.CustomPeltData>("Total Sigils", (a)=>a.AbilityCount.ToString()),
                new TableColumn<PeltManager.CustomPeltData>("Available At Trader", (a)=>a.AvailableAtTrader.ToString()),
                new TableColumn<PeltManager.CustomPeltData>("Cards To Choose", (a)=>a.MaxChoices.ToString()),
                new TableColumn<PeltManager.CustomPeltData>("Cards In Pool", (a)=>a.GetChoices().Count.ToString()),
            });
        }

        private string GetNameOfCard(PeltManager.CustomPeltData arg)
        {
            if (arg.CardNameOfPelt == null)
            {
                return "null";
            }
            else if (arg.CardNameOfPelt == "")
            {
                return arg.CardNameOfPelt;
            }
            
            CardInfo cardByName = CardManager.AllCardsCopy.Find((a)=>a.name == arg.CardNameOfPelt);
            if (cardByName == null)
            {
                return arg.CardNameOfPelt;
            }
            
            return cardByName.displayedName;
        }

        public override string GetGUID(PeltManager.CustomPeltData o)
        {
            return o.PluginGUID;
        }

        protected override int Sort(PeltManager.CustomPeltData a, PeltManager.CustomPeltData b)
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
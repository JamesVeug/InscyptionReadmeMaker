using System;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Pelts;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewPeltsSection : ASection<PeltManager.PeltData>
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
                new TableColumn<PeltManager.PeltData>("Name", GetNameOfCard),
                new TableColumn<PeltManager.PeltData>("Base Cost", (a)=>a.baseBuyPrice.ToString()),
                new TableColumn<PeltManager.PeltData>("Extra Sigils", (a)=>a.extraAbilitiesToAdd.ToString()),
                new TableColumn<PeltManager.PeltData>("Available At Trader", (a)=>a.isSoldByTrapper.ToString()),
                new TableColumn<PeltManager.PeltData>("Cards To Choose", (a)=>a.choicesOfferedByTrader.ToString()),
            });
        }

        private string GetNameOfCard(PeltManager.PeltData arg)
        {
            if (arg.peltCardName == null)
            {
                return "null";
            }
            else if (arg.peltCardName == "")
            {
                return arg.peltCardName;
            }
            
            CardInfo cardByName = CardManager.AllCardsCopy.Find((a)=>a.name == arg.peltCardName);
            if (cardByName == null)
            {
                return arg.peltCardName;
            }
            
            return cardByName.displayedName;
        }

        public override string GetGUID(PeltManager.PeltData o)
        {
            return o.pluginGuid;
        }

        protected override int Sort(PeltManager.PeltData a, PeltManager.PeltData b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.peltCardName, b.peltCardName, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
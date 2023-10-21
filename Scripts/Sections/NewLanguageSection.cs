using System;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Masks;
using InscryptionAPI.Pelts;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewMasksSection : ASection<CustomMask>
    {
        public override string SectionName => "New Masks";
        public override bool Enabled => ReadmeConfig.Instance.MasksShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(MaskManager.CustomMasks.Where(x=>GetGUID(x) == mod.PluginGUID));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<CustomMask>("Name", GetNameOfMask),
            });
        }

        private string GetNameOfMask(CustomMask o)
        {
            return o.Name;
        }

        public override string GetGUID(CustomMask o)
        {
            return o.GUID;
        }

        protected override int Sort(CustomMask a, CustomMask b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.Name, b.Name, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
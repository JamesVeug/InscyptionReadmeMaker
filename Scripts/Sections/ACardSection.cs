using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;
using ReadmeMaker.Scripts.Utils;

namespace JamesGames.ReadmeMaker.Sections
{
    public abstract class ACardsSection : ASection<CardInfo>
    {
        public override void Initialize(RegisteredMod mod)
        {
            rawData = GetCards(mod);
        }

        protected abstract List<CardInfo> GetCards(RegisteredMod mod);

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            List<TableColumn<CardInfo>> tableColumns = SectionUtils.GetCardTableColumns();

            splitCards = BreakdownForTable(out headers, tableColumns.ToArray());
        }

        protected override bool Filter(CardInfo o)
        {
            if (!string.IsNullOrEmpty(ReadmeConfig.Instance.FilterByJSONLoaderModPrefix))
            {
                // Show everything
                string modPrefix = o.GetModPrefix();
                if (!string.IsNullOrEmpty(modPrefix))
                {
                    if (modPrefix.Trim() == ReadmeConfig.Instance.FilterByJSONLoaderModPrefix.Trim())
                    {
                        return true;
                    }
                }
            }
            
            return base.Filter(o);
        }

        protected override int Sort(CardInfo a, CardInfo b)
        {
            int compare = 0;
            switch (ReadmeConfig.Instance.CardSortBy)
            {
                case ReadmeConfig.CardSortByType.GUID:
                    compare = string.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                    if (compare == 0)
                    {
                        compare = ReadmeHelpers.CompareByDisplayName(a, b);
                    }
                    break;
                case ReadmeConfig.CardSortByType.Name:
                    compare = ReadmeHelpers.CompareByDisplayName(a, b);
                    break;
                case ReadmeConfig.CardSortByType.Cost:
                    compare = ReadmeHelpers.CompareByCost(a, b);
                    break;
                case ReadmeConfig.CardSortByType.Power:
                    compare = a.Attack - b.Attack;
                    break;
                case ReadmeConfig.CardSortByType.Health:
                    compare = a.Health - b.Health;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!ReadmeConfig.Instance.CardSortAscending)
            {
                compare *= -1;
            }

            return compare;
        }

        public override string GetGUID(CardInfo o)
        {
            return o.GetModTag();
        }

        
    }
}
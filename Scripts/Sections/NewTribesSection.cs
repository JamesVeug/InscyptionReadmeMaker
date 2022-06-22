using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;
using TribeInfo = InscryptionAPI.Card.TribeManager.TribeInfo;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewTribesSection : ASection<TribeInfo>
    {
        public override string SectionName => "New Tribes";
        public override bool Enabled => ReadmeConfig.Instance.TribesShow;
        
        public override void Initialize()
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(TribeManager.tribes);
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<TribeInfo>("Name", GetTribeName),
                new TableColumn<TribeInfo>("Cards", GetCardCount)
            });
        }

        public override string GetGUID(TribeInfo o)
        {
            return ReadmeHelpers.GetTribeGUID(o.tribe);
        }

        public static string GetTribeName(TribeInfo tribe)
        {
            return ReadmeHelpers.GetTribeName(tribe.tribe);
        }

        public static string GetCardCount(TribeInfo tribe)
        {
            int totalCards = 0;
            foreach (CardInfo info in CardManager.AllCardsCopy)
            {
                if (info.IsOfTribe(tribe.tribe))
                {
                    totalCards++;
                }
            }
            
            return totalCards.ToString();
        }
    }
}
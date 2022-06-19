using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;
using TribeInfo = InscryptionAPI.Card.TribeManager.TribeInfo;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewTribesSection : ASection
    {
        public override string SectionName => "New Tribes";
        public override bool Enabled => ReadmeConfig.Instance.SigilsShow;

        private List<TribeInfo> newTribes = new List<TribeInfo>();
        
        public override void Initialize()
        {
            newTribes.Clear(); // Clear so when we re-dump everything we don't double up
            newTribes.AddRange(TribeManager.tribes);
        }

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (newTribes.Count > 0)
            {
                stringBuilder.Append($"\n{newTribes.Count} {SectionName}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(newTribes, out headers, new TableColumn<TribeInfo>[]
            {
                new TableColumn<TribeInfo>("GUID", GetTribeGUID),
                new TableColumn<TribeInfo>("Name", GetTribeName),
                new TableColumn<TribeInfo>("Cards", GetCardCount)
            });
        }

        public static string GetTribeGUID(TribeInfo tribe)
        {
            return ReadmeHelpers.GetTribeGUID(tribe.tribe);
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
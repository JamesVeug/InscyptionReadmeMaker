using System.Collections.Generic;
using System.Text;
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
                new TableColumn<TribeInfo>("Name", GetTribeName)
            });
        }

        public static string GetTribeGUID(TribeInfo tribe)
        {
            return Helpers.GetGUID(tribe.tribe);
        }

        public static string GetTribeName(TribeInfo tribe)
        {
            return Helpers.GetName(tribe.tribe);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using InscryptionAPI.Ascension;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewAscensionChallengesSection : ASection<ChallengeManager.FullChallenge>
    {
        public override string SectionName => "New Ascension Challenges";
        public override bool Enabled => ReadmeConfig.Instance.AscensionChallengesShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(ChallengeManager.NewInfos.Where(x=>GetGUID(x) == mod.PluginGUID));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new []
            {
                new TableColumn<ChallengeManager.FullChallenge>("Name", (a)=>a.Challenge.title),
                new TableColumn<ChallengeManager.FullChallenge>("Points", (a)=>a.Challenge.pointValue.ToString()),
                new TableColumn<ChallengeManager.FullChallenge>("Description", (a)=>a.Challenge.description)
            });
        }

        public override string GetGUID(ChallengeManager.FullChallenge o)
        {
            return Helpers.GetGUID(((int)o.Challenge.challengeType).ToString());
        }

        protected override int Sort(ChallengeManager.FullChallenge a, ChallengeManager.FullChallenge b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.Challenge.title, b.Challenge.title, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
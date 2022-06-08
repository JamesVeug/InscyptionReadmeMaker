using System;
using System.Collections.Generic;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Ascension;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewAscensionChallengesSection : ASection
    {
        public override string SectionName => "New Ascension Challenges";
        public override bool Enabled => ReadmeConfig.Instance.AscensionChallengesShow;

        private List<ChallengeManager.FullChallenge> challenges = new List<ChallengeManager.FullChallenge>();
        
        public override void Initialize()
        {
            challenges.AddRange(ChallengeManager.NewInfos);
            challenges.Sort(SortAscensionChallenges);
        }

        private static int SortAscensionChallenges(ChallengeManager.FullChallenge a, ChallengeManager.FullChallenge b)
        {
            return String.Compare(a.Challenge.title, b.Challenge.title, StringComparison.Ordinal);
        }

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (challenges.Count > 0)
            {
                stringBuilder.Append($"\n{challenges.Count} {SectionName}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(challenges, out headers, new []
            {
                new TableColumn<ChallengeManager.FullChallenge>("Name", (a)=>a.Challenge.title),
                new TableColumn<ChallengeManager.FullChallenge>("Points", (a)=>a.Challenge.pointValue.ToString()),
                new TableColumn<ChallengeManager.FullChallenge>("Description", (a)=>a.Challenge.description)
            });
        }
    }
}
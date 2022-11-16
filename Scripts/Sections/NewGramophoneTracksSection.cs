using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Sound;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewGramophoneTracksSection : ASection<GramophoneManager.TrackInfo>
    {
        public override string SectionName => "New Gramophone Tracks";
        public override bool Enabled => ReadmeConfig.Instance.GramophoneTracksShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            rawData.AddRange(GramophoneManager.TracksToAdd.Where(x=>GetGUID(x) == mod.PluginGUID));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> splitCards)
        {
            splitCards = BreakdownForTable(out headers, new[]
            {
                new TableColumn<GramophoneManager.TrackInfo>("Name", (a)=>a.TrackName),
                new TableColumn<GramophoneManager.TrackInfo>("Track Index", GetTrackIndex),
            });
        }

        private string GetTrackIndex(GramophoneManager.TrackInfo arg)
        {
            string trackName = $"{GetGUID(arg)}_{Path.GetFileName(arg.FilePath)}";
            Plugin.Log.LogInfo("[GetTrackIndex] " + trackName);

            for (int i = 0; i < GramophoneInteractable.TRACK_IDS.Count; i++)
            {
                var VARIABLE = GramophoneInteractable.TRACK_IDS[i];
                Plugin.Log.LogInfo("\t" + i + " " + VARIABLE);
            }

            int indexOf = GramophoneInteractable.TRACK_IDS.IndexOf(trackName);
            return indexOf.ToString();
        }

        public override string GetGUID(GramophoneManager.TrackInfo o)
        {
            return o.Guid;
        }

        protected override int Sort(GramophoneManager.TrackInfo a, GramophoneManager.TrackInfo b)
        {
            switch (ReadmeConfig.Instance.GramophoneTracksSortBy)
            {
                case ReadmeConfig.GramophoneSortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.GramophoneSortByType.Name:
                    return String.Compare(a.TrackName, b.TrackName, StringComparison.Ordinal);
                case ReadmeConfig.GramophoneSortByType.PlayOrder:
                    int aOrder = GramophoneInteractable.TRACK_IDS.IndexOf(a.AudioClipName);
                    int bOrder = GramophoneInteractable.TRACK_IDS.IndexOf(b.AudioClipName);
                    return aOrder - bOrder;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
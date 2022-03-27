using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Encounters;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewMapNodesSection : ASection
    {
        public override string SectionName => "New Map Nodes";
        public override bool Enabled => ReadmeConfig.Instance.NodesShow;

        private List<NodeManager.NodeInfo> allNodes = null;
        
        public override void Initialize()
        {
            if (!ReadmeConfig.Instance.NodesShow)
            {
                allNodes = new List<NodeManager.NodeInfo>();
                return;
            }

            allNodes = new List<NodeManager.NodeInfo>(NodeManager.AllNodes);
            allNodes.Sort((a, b) => String.Compare(a.guid, b.guid, StringComparison.Ordinal));
        }
        
        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (allNodes.Count > 0)
            {
                stringBuilder.Append($"\n{allNodes.Count} {SectionName}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            // Nothing to show
            headers = new List<TableHeader>();
            rows = new List<Dictionary<string, string>>();
        }
    }
}
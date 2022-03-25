using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Encounters;

namespace ReadmeMaker.Sections
{
    public class NewMapNodesSection : ASection
    {
        private List<NodeManager.NodeInfo> allNodes = null;
        
        public override void Initialize()
        {
            if (!ReadmeConfig.Instance.NodesShow)
            {
                allNodes = new List<NodeManager.NodeInfo>();
            }

            allNodes = new List<NodeManager.NodeInfo>(NodeManager.AllNodes);
            allNodes.Sort((a, b) => String.Compare(a.guid, b.guid, StringComparison.Ordinal));
        }
        
        public override string GetSectionName()
        {
            return "New Map Nodes";
        }

        public override void DumpSummary(StringBuilder stringBuilder)
        {
            if (allNodes.Count > 0)
            {
                stringBuilder.Append($"- {allNodes.Count} {GetSectionName()}\n");
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            throw new NotImplementedException();
        }
    }
}
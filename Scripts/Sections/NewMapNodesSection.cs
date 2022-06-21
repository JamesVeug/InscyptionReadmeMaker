using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Encounters;
using InscryptionAPI.Nodes;
using ReadmeMaker.Patches;

namespace JamesGames.ReadmeMaker.Sections
{
    public class NewMapNodesSection : ASection
    {
        /// <summary>
        /// Wrapper to support the old NewMapnode system in the API and the new one. 
        /// </summary>
        private class NodeWrapper
        {
            public NewNodeManager.FullNode NewMapNodes;
            public NodeManager.NodeInfo OldMapNodes;

            public string GUID => NewMapNodes != null ? NewMapNodes.guid : OldMapNodes.GetModTag();
            public string Name => NewMapNodes != null ? NewMapNodes.name : OldMapNodes.guid; 
        }
        
        public override string SectionName => "New Map Nodes";
        public override bool Enabled => ReadmeConfig.Instance.NodesShow;

        private List<NodeWrapper> allNodes = new List<NodeWrapper>();
        
        public override void Initialize()
        {
            allNodes.Clear(); // Clear so when we re-dump everything we don't double up
            foreach (NodeManager.NodeInfo info in NodeManager.AllNodes)
            {
                allNodes.Add(new NodeWrapper()
                {
                    OldMapNodes = info
                });
            }
            
            foreach (NewNodeManager.FullNode info in NewNodeManager.NewNodes)
            {
                allNodes.Add(new NodeWrapper()
                {
                    NewMapNodes = info
                });
            }
            
            allNodes.Sort((a, b) => string.Compare(a.GUID, b.GUID, StringComparison.Ordinal));
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            rows = BreakdownForTable(allNodes, out headers, new TableColumn<NodeWrapper>[]
            {
                new TableColumn<NodeWrapper>("Name", (a)=>a.Name),
            });
        }

        public override string GetGUID(object o)
        {
            NodeWrapper casted = (NodeWrapper)o;
            return casted.GUID;
        }
    }
}
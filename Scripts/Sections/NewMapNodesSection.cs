using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Encounters;
using InscryptionAPI.Nodes;
using ReadmeMaker.Patches;

namespace JamesGames.ReadmeMaker.Sections
{
#pragma warning disable CS0618
    /// <summary>
    /// Wrapper to support the old NewMapnode system in the API and the new one. 
    /// </summary>
    public class NodeWrapper
    {
        public NewNodeManager.FullNode NewMapNodes;
        public NodeManager.NodeInfo OldMapNodes;

        public string GUID => NewMapNodes != null ? NewMapNodes.guid : OldMapNodes.GetModTag();
        public string Name => NewMapNodes != null ? NewMapNodes.name : OldMapNodes.guid; 
    }
    
    public class NewMapNodesSection : ASection<NodeWrapper>
    {
        
        public override string SectionName => "New Map Nodes";
        public override bool Enabled => ReadmeConfig.Instance.MapNodesShow;
        
        public override void Initialize(RegisteredMod mod)
        {
            rawData.Clear(); // Clear so when we re-dump everything we don't double up
            foreach (NodeManager.NodeInfo info in NodeManager.AllNodes)
            {
                if (info.guid != mod.PluginGUID)
                {
                    continue;
                }

                rawData.Add(new NodeWrapper()
                {
                    OldMapNodes = info
                });
            }
             
            foreach (NewNodeManager.FullNode info in NewNodeManager.NewNodes)
            {
                if (info.guid != mod.PluginGUID)
                {
                    continue;
                }
                
                rawData.Add(new NodeWrapper()
                {
                    NewMapNodes = info
                });
            }
        }

        public override void GetTableDump(out List<TableHeader> headers, out List<Dictionary<string, string>> rows)
        {
            rows = BreakdownForTable(out headers, new[]
            {
                new TableColumn<NodeWrapper>("Name", (a)=>a.Name),
            });
        }

        public override string GetGUID(NodeWrapper o)
        {
            return o.GUID;
        }

        protected override int Sort(NodeWrapper a, NodeWrapper b)
        {
            switch (ReadmeConfig.Instance.GeneralSortBy)
            {
                case ReadmeConfig.SortByType.GUID:
                    return String.Compare(GetGUID(a), GetGUID(b), StringComparison.Ordinal);
                case ReadmeConfig.SortByType.Name:
                    return String.Compare(a.Name, b.Name, StringComparison.Ordinal);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
#pragma warning restore CS0618
}
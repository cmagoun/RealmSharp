using EMK.Cartography;
using NUnit.Framework;
using RealmSharp.GameObjects;
using System;
using System.Linq;


namespace RealmSharpTests
{
    public class Tests
    {
        [Test]
        public void CanCreateGraph()
        {
            var hm = new HexMap();
            hm.Initialize(TileDefs.AllGreen);
            hm.PlaceHex("BL");
            hm.PlaceHex(TileDefs.AwfulValley, 0, -1, 1 );

            var g = Pathfinder.CreateGraph(hm);
            var nc = g.NodeToClearing;

            foreach (Arc arc in g.Graph.Arcs)
            {
                Console.WriteLine($"{nc[arc.StartNode.ToString()].Key} --> {nc[arc.EndNode.ToString()].Key}");
            }
        }

        [Test]
        public void CanFindPath()
        {
            var hm = new HexMap();
            hm.Initialize(TileDefs.AllGreen);
            hm.PlaceHex("BL");
            hm.PlaceHex(TileDefs.AwfulValley, 0, -1, 1);

            var g = Pathfinder.CreateGraph(hm);

            var path = Pathfinder.FindPath("BL1", "VA4", g);
            Assert.IsNotNull(path);

            var noPath = Pathfinder.FindPath("BL1", "VA5", g);
            Assert.IsNull(noPath);
        }

        [Test]
        public void CanFindPathWithProposedHex()
        {
            var hm = new HexMap();
            hm.Initialize(TileDefs.AllGreen);
            hm.PlaceHex("BL");

            var g = Pathfinder.CreateGraph(hm, new HexPosition(TileDefs.AwfulValley, 0, -1, 1));

            var nc = g.NodeToClearing;

            foreach (Arc arc in g.Graph.Arcs)
            {
                Console.WriteLine($"{nc[arc.StartNode.ToString()].Key} --> {nc[arc.EndNode.ToString()].Key}");
            }

            var path = Pathfinder.FindPath("BL1", "VA4", g);
            Assert.IsNotNull(path);
            
            var noPath = Pathfinder.FindPath("BL1", "VA5", g);
            Assert.IsNull(noPath);
            
            //Check that the original map is unchanged
            Assert.AreEqual(1, hm.Placed.Count);
        }

        [Test]
        public void AreTilesConnectedInternally()
        {
            //Is this sufficient proof that tiles are wired properly???.
            Assert.IsTrue(TileDefs.AllGreen.All(TestConnectedToCheckClearing));
            Assert.IsTrue(TileDefs.AllGreen.All(TestIncomingOutgoingPaths));
        }

        private bool TestIncomingOutgoingPaths(Hex hex)
        {
            var hm = new HexMap();
            hm.AddHex(hex, 0, 0, 0);
            var g = Pathfinder.CreateGraph(hm);

            var successCount = hex.Clearings.Select(c => DoesClearingHaveEqualInOutPaths(c, g)).Count(a => a);
            return successCount == hex.Clearings.Count;
        }

        private bool TestConnectedToCheckClearing(Hex hex)
        {
            var hm = new HexMap();
            hm.AddHex(hex, 0, 0, 0);
            var g = Pathfinder.CreateGraph(hm);

            var successCount = hex.Clearings.Select(c => DoesClearingConnectToCheckClearing(c, hex, g)).Count(a => a);

            return successCount == hex.Clearings.Count;
        }

        private bool DoesClearingConnectToCheckClearing(Clearing c, Hex hex, PathfinderGraph g)
        {
            foreach (var chk in hex.Checks)
            {
                if (chk == c.Number) return true;

                var path = Pathfinder.FindPath(c.Key, hex.Key + chk, g);
                if (path != null) return true;
            }

            Console.WriteLine($"{hex.Name} has connectivity issue");
            return false;
        }

        private bool DoesClearingHaveEqualInOutPaths(Clearing c, PathfinderGraph g)
        {
            var node = g.ClearingToNode[c.Key];

            if(node.OutgoingArcs.Count != node.IncomingArcs.Count) Console.WriteLine($"{c.Key} has wonky paths");

            return node.OutgoingArcs.Count == node.IncomingArcs.Count;
        }


        [Test]
        //I noticed a problem placing the Cliff tile, so I am just checking it under
        //more controlled circumstances. Don't need this any more? -- use AreTilesConnectedInternally instead?
        public void IsThereAProblemWithCliff()
        {
            var hm = new HexMap();
            hm.Initialize(TileDefs.AllGreen);
            hm.PlaceHex("BL");

            var g = Pathfinder.CreateGraph(hm, new HexPosition(TileDefs.Cliff, 1, 0, 1));

            var nc = g.NodeToClearing;

            foreach (Arc arc in g.Graph.Arcs)
            {
                Console.WriteLine($"{nc[arc.StartNode.ToString()].Key} --> {nc[arc.EndNode.ToString()].Key}");
            }

            var path = Pathfinder.FindPath("CF1", "BL4", g);

            Assert.IsNotNull(path);
        }

        [Test]
        //I noticed a problem placing the DeepWoods tile, so I am just checking it under
        //more controlled circumstances. Don't need this any more? -- use AreTilesConnectedInternally instead?
        public void IsThereAProblemWithDeepWoods()
        {
            var hm = new HexMap();
            hm.Initialize(TileDefs.AllGreen);
            hm.PlaceHex("BL");

            var g = Pathfinder.CreateGraph(hm, new HexPosition(TileDefs.DeepWoods, -1, -1, 1));

            var nc = g.NodeToClearing;

            foreach (Arc arc in g.Graph.Arcs)
            {
                Console.WriteLine($"{nc[arc.StartNode.ToString()].Key} --> {nc[arc.EndNode.ToString()].Key}");
            }

            var path = Pathfinder.FindPath("DW1", "BL4", g);

            Assert.IsNotNull(path);
        }
    }
}
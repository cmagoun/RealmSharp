using EMK.Cartography;
using System.Collections.Generic;
using System.Linq;

namespace RealmSharp.GameObjects
{
    public class Pathfinder
    {
        public static PathfinderGraph CreateGraph(HexMap map, HexPosition proposedHex = null)
        {
            var dc = new Dictionary<string, Node>(); //ClearingKey => Node
            var nc = new Dictionary<string, Clearing>(); //nodekey => Clearing
            var g = new Graph();
            var i = 0;

            var hexes = map.Placed.ToArray().ToList();
            if (proposedHex != null) hexes.Add(proposedHex);

            //Create all clearings first, so we have references with which to
            //connect them.
            hexes.ForEach(hex =>
            {
                hex.Hex.Clearings.ForEach(c =>
                {
                    //The library I used keys off of the node's x,y,z coordinate.
                    //We don't have a coordinate, so our key will just be our index in x,y and z.
                    var node = g.AddNode(i, i, i);
                    dc.Add(c.Key, node);
                    nc.Add(node.ToString(), c);
                    i++;
                });
            });

            //A little arrow of death-like
            hexes.ForEach(hex =>
            {
                hex.Hex.Clearings.ForEach(c =>
                {
                    c.Connections.ForEach(cnx =>
                    {
                        if (cnx.ConnectTo != null)
                        {
                            g.AddArc(dc[c.Key], dc[cnx.ConnectTo.Key], 1);
                        }
                        else
                        {
                            //we come to an exit
                            var ourSide = HexMap.RotateSide(cnx.ExitSide.Value, hex.Orientation);
                            var adjSpace = Hex.Adjacent(hex.X, hex.Y)[ourSide];
                            var adjHex = HexMap.HexAt(hexes, adjSpace.X, adjSpace.Y);

                            if (adjHex != null)
                            {
                                var adjExit =
                                    adjHex.Hex.Exits[
                                        HexMap.RotateExits(HexMap.FacingSides[ourSide], adjHex.Orientation)];
                                var adjKey = adjHex.Hex.Key + adjExit;
                                g.AddArc(dc[c.Key], dc[adjKey], 1);
                            }
                        }
                    });
                });
            });

            return new PathfinderGraph
            {
                Graph = g,
                ClearingToNode = dc,
                NodeToClearing = nc
            };
        }

        public static List<string> FindPath(Clearing start, Clearing end, HexMap map, HexPosition proposedHex = null)
        {
            var graph = CreateGraph(map, proposedHex);
            return FindPath(start.Key, end.Key, graph);
        }

        public static List<string> FindPath(string startKey, string endKey, HexMap map, HexPosition proposedHex = null)
        {
            var graph = CreateGraph(map, proposedHex);
            return FindPath(startKey, endKey, graph);
        }

        public static List<string> FindPath(string startKey, string endKey, PathfinderGraph pfg)
        {
            if (!pfg.ClearingToNode.ContainsKey(startKey)) return null;
            if (!pfg.ClearingToNode.ContainsKey(endKey)) return null;

            //More efficient if you have already created/stored a graph
            var aStar = new AStar(pfg.Graph);
            var found = aStar.SearchPath(pfg.ClearingToNode[startKey], pfg.ClearingToNode[endKey]);
            if (!found) return null;

            var path = aStar.PathByNodes;
            return path.Select(p => pfg.NodeToClearing[p.ToString()].Key).ToList();
        }

        public static List<string> FindPath(Clearing start, Clearing end, PathfinderGraph pfg)
        {
            return FindPath(start.Key, end.Key, pfg);
        }
    }

    public class PathfinderGraph
    {
        public Graph Graph { get; set; }
        public Dictionary<string, Node> ClearingToNode { get; set; }
        public Dictionary<string, Clearing>NodeToClearing { get; set; }

    }
}

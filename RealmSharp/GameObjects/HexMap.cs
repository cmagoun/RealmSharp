using System;
using GameUtils;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EMK.Cartography;

namespace RealmSharp.GameObjects
{
    public class HexMap
    {
        //If I am exiting side 0 -> which side on the adjacent hex do I check
        public static int[] FacingSides = new[] {3, 4, 5, 0, 1, 2};
        public List<HexPosition> Placed { get; private set; }
        public List<Hex> NotPlaced { get; private set; }

        //Maybe?? Not sure how I feel about this. How will we know that this should be used?
        public PathfinderGraph Graph { get; private set; }
    

        //Do I need this?
        //public Dictionary<string, HexPosition> PositionIndex { get; private set; }

        public HexMap()
        {
            NotPlaced = new List<Hex>();
            Placed = new List<HexPosition>();
            Graph = new PathfinderGraph();
            //PositionIndex = new Dictionary<string, HexPosition>();
        }

        public void Initialize(IEnumerable<Hex> initialList)
        {
            NotPlaced = initialList.ToArray().ToList();
        }

        public void AddHex(Hex hex, int x, int y, int orientation)
        {
            var pos = new HexPosition {Hex = hex, Orientation = orientation, X = x, Y = y};
            Placed.Add(pos);
            Graph = Pathfinder.CreateGraph(this);
        }

        public bool CheckPlacement(Hex hex, int x, int y, int orientation)
        {
            var adjacent = AllAdjacentTo(x, y);

            return IsBesideEnoughHexes(adjacent) &&
                   AllRoadsLineUp(hex, orientation, adjacent) &&
                   CanTraceBackToBorderland(hex, x, y, orientation);

        }

        public HexPosition HexAt(int x, int y)
        {
            return HexAt(Placed, x, y);
        }

        public List<HexPosition> AllAdjacentTo(int x, int y)
        {
            //Returns hexes and empty spaces (as nulls) adjacent to x,y
            return Hex.Adjacent(x, y)
                .Select(sp => HexAt(sp.X, sp.Y))
                .ToList();
        }

        public List<HexPosition> HexesAdjacentTo(int x, int y)
        {
            return Hex.Adjacent(x, y)
                .Select(sp => HexAt(sp.X, sp.Y))
                .Where(x => x != null)
                .ToList();
        }

        public List<Space> EmptySpacesAdjacentTo(int x, int y)
        {
            return Hex.Adjacent(x, y)
                .Where(sp => HexAt(sp.X, sp.Y) == null)
                .ToList();
        }

        public void PlaceHex(string key, int x = 0, int y = 0, int orientation = 0)
        {
            var hex = NotPlaced.Single(x => x.Key == key);
            PlaceHex(hex, x, y, orientation);
        }

        public void PlaceHex(Hex hex, int x, int y, int orientation)
        {
            AddHex(hex, x, y, orientation);

            //Remove from not placed if necessary
            var toRemove = NotPlaced.SingleOrDefault(np => np.Key == hex.Key);
            NotPlaced.Remove(toRemove);
        }

        public Clearing DwellingClearing(Hex hex)
        {
            var path5 = Pathfinder.FindPath(hex.Key + "5", "BL4", Graph);

            var dwellingClearing = path5 == null
                ? hex.Clearings.Single(c => c.Number == 4)
                : hex.Clearings.Single(c => c.Number == 5);

            return dwellingClearing;
        }

        public List<Clearing> ClearingsAtDistance(string key, int distance)
        {
            if (distance == 0) return Placed.SelectMany(p => p.Hex.Clearings).Where(c => c.Key == key).ToList();

            var startNode = Graph.ClearingToNode[key];
            var dist = 0;
            var visited = new List<Node> { startNode };
            var currentNodes = new List<Node>{startNode};

            while (dist < distance)
            {
                currentNodes = currentNodes.SelectMany(n => n.AccessibleNodes.Where(an => !an.Equals(n))).ToList();
                visited.AddRange(currentNodes);
                dist++;
            }

            return currentNodes
                .Where(cn => !cn.Equals(startNode))
                .Distinct()
                .Select(cn => Graph.NodeToClearing[cn.ToString()])
                .ToList();
        }

        public List<Clearing> ClearingsAtDistance(Clearing start, int distance)
        {
            return ClearingsAtDistance(start.Key, distance);
        }


        private bool IsBesideEnoughHexes(List<HexPosition> adjacent)
        {
            var count = adjacent.Count(a => a != null);

            if (Placed.Count == 1) return count == 1;
            return count > 1;
        }

        private bool AllRoadsLineUp(Hex hex, int orientation, List<HexPosition> adjacent)
        {
            //So... how do we do this
            //For our side 0, we check to our north (0) and check their 3
            for (var i = 0; i < 6; i++)
            {
                var adj = adjacent[i];
                if(adj == null) continue;

                var adjExit = adj.Hex.Exits[RotateExits(FacingSides[i], adj.Orientation)];
                var ourExit = hex.Exits[RotateExits(i, orientation)];

                if (adjExit == 0 && ourExit != 0) return false;
                if (adjExit != 0 && ourExit == 0) return false;
            }

            return true;
        }

        private bool CanTraceBackToBorderland(Hex hex, int x, int y, int orientation)
        {
            var proposed = new HexPosition(hex, x, y, orientation);
            
            //Not accounting for BL being enchanted. Do we need to?
            //We may need to later if there is some way to place tiles in the middle of the game.
            //There currently isn't so...
            var paths = hex.Checks.Select(chk =>
                {
                    var key = hex.Key + chk;
                    return Pathfinder.FindPath(key, "BL4", this, proposed);
                })
                .Where(p => p != null)
                .ToList();

            return hex.Clearings.Count == 6
                ? paths.Count == hex.Checks.Length //all clearings go back
                : paths.Any(); //any clearing goes back
        }

        public static int RotateExits(int baseSide, int orientation)
        {
            //If I am coming from your {baseSide} side, which of your exits faces me?
            //Orientation 0 means the 0th exit is on the north face (the 0th face).
            //Orientation 1 means the 5th exit is on the north face (the 0th face).
            return orientation > baseSide
                ? 6 + (baseSide - orientation)
                : baseSide - orientation;
        }

        public static int RotateSide(int baseSide, int orientation)
        {
            //I come to an exit that exits side {baseSide}, but we are rotated,
            //so which side do I actually come out?
            //Orientation 0 means that when I hit an exit on side 0, I leave side 0.
            //Orientation 1 means that when I hit an exit on side 0, I leave side 1.
            var result = baseSide + orientation;
            if (result > 5) result -= 6;

            return result;
        }

        public static HexPosition HexAt(List<HexPosition> hexes, int x, int y)
        {
            return hexes.SingleOrDefault(hex => hex.X == x && hex.Y == y);
        }
    }

    public class HexPosition
    {
        public Hex Hex { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Orientation { get; set; }

        public HexPosition(){}

        public HexPosition(Hex hex, int x, int y, int orientation)
        {
            Hex = hex;
            X = x;
            Y = y;
            Orientation = orientation;
        }
    }
}

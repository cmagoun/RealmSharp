using GameUtils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealmSharp.GameObjects
{
    //I am going to start with the assumption that this represents ONLY
    //a single side of a hex tile, and that the enchanted side will have
    //a separate hex representation.
    public class Hex
    {
        public static int COL_WIDTH = 372;

        public static int HEX_HEIGHT = 430;
        public static int HEX_WIDTH = 497;

        public static int VERT_OFFSET = 215;

        public static int RECT_WIDTH = 248;
        public static int TRI_WIDTH = 124;

        //Setup time
        public string Key { get; set; } //BL, CV, CA
        public string Name { get; set; } //Borderland, Caves, Cavern
        public string ImageKey { get; set; }

        public string HexType { get; set; } //W, V, C, M

        public int[] Exits { get; set; } //side i connects to which clearing; 0 == no exit on this side
        public int[] Checks { get; set; }
        public Vector2?[] Anchors { get; set; } //if orientation == 0, what positions are the clearings

        public List<Clearing> Clearings { get; set; }

        public Hex(string key, string name, string type, string imageKey, int[] exits, int[] checks, Vector2?[] anchors, string green)
        {
            Key = key;
            Name = name;
            HexType = type;
            Exits = exits;
            ImageKey = imageKey;
            Anchors = anchors;
            Checks = checks;


            //sides are in the form clearing;clearing;clearing;
            //clearings are in the form number/type/color/connections
            Clearings = CreateClearings(green);
        }

        private List<Clearing> CreateClearings(string side)
        {
            var sClearings = side.Replace("\r\n", "").Split(";");
            var aClearings = sClearings.Select(s => new Clearing(this, s)).ToList();

            foreach (var aClearing in aClearings)
            {
                aClearing.ConnectInternal(aClearings, Exits);
            }

            return aClearings;
        }

        public static List<Space> Adjacent(int x, int y)
        {
                 //1,-1
         //0,0           //2,0
                 //1,0
         //0,1           //2,1
                 //1,1


            if (x % 2 == 0)
            {
                return new List<Space>
                {
                    new Space(x, y-1),
                    new Space(x+1, y-1),
                    new Space(x+1, y),
                    new Space(x, y+1),
                    new Space(x-1, y),
                    new Space(x-1, y-1)
                };
            }
            else
            {
                return new List<Space>
                {
                    new Space(x, y-1),
                    new Space(x+1, y),
                    new Space(x+1, y+1),
                    new Space(x, y+1),
                    new Space(x-1, y+1),
                    new Space(x-1, y)
                };
            }
        }

        public static Vector2 Center(int x, int y)
        {
            var baseX = x * COL_WIDTH;
            var baseY = (y * HEX_HEIGHT) + (Math.Abs(x % 2) * VERT_OFFSET);
            return new Vector2(baseX, baseY);
        }

        public static Vector2 TopLeft(int x, int y)
        {
            return Center(x, y) - new Vector2(HEX_WIDTH / 2, HEX_HEIGHT / 2);
        }

        public static Vector2?[] GetAnchors(Hex hex, int orientation)
        {
            if (orientation == 0) return hex.Anchors;

            var rad =(float)(orientation * (Math.PI / 3));
            var sin = (float)Math.Sin(rad);
            var cos = (float)Math.Cos(rad);

            //rotate w. respect to hex center
            var results = hex.Anchors.Select(a =>
            {
                if (a == null) return null;

                var tx = a.Value.X - (HEX_WIDTH / 2);
                var ty = a.Value.Y - (HEX_HEIGHT / 2);

                return (Vector2?)new Vector2(
                    (cos*tx) - (sin*ty) + (HEX_WIDTH/2),
                    (sin*tx) + (cos*ty) + (HEX_HEIGHT/2));
            });

            return results.ToArray();
        }

        public static Vector2? GetAnchor(Hex hex, int clearing, int orientation)
        {
            if (orientation == 0) return hex.Anchors[clearing - 1];
            return GetAnchors(hex, orientation)[clearing - 1];
        }

    }

    public class Clearing
    {
        public Hex Parent { get; set; }
        public string Key { get; set; } //BL1, BL2
        public int Number { get; set; } //1, 2, 3
        public string ClearingType { get; set; } //-, C, M, X?
        public string ColorMagic { get; set; } //-, Gold, greY, Black, White, Purple
        public List<Path> Connections { get; set; }

        private readonly string _connectString;

        public Clearing(Hex parent, string info)
        {
            //number / type / color / connections (* = passage, @ = path)
            Parent = parent;
            
            var aVars = info.Split("/");
            Key = $"{parent.Key}{aVars[0].Trim()}";

            Number = int.Parse(aVars[0].Trim());
            ClearingType = aVars[1].Trim().ToUpper();
            ColorMagic = aVars[2].Trim().ToUpper();
            _connectString = aVars[3].Trim();
        }

        public void ConnectInternal(List<Clearing> clearings, int[] exits)
        {
            Connections = new List<Path>();

            var aConnect = _connectString.Split(",");
            foreach (var connect in aConnect)
            {
                if(connect == "-") continue;
                
                var clearing = clearings.Single(c => c.Key == $"{Parent.Key}{connect[0]}");

                if (connect.Length == 1)
                {
                    Connections.Add(new Path(clearing));
                }
                else
                {
                    var sPassage = connect[1] == '*';
                    var sPath = connect[1] == '@';
                    Connections.Add(new Path(clearing, sPath, sPassage));
                }
            }

            exits.ForEach((x, i) =>
            {
                if(x == Number) Connections.Add(Path.Exit(i));
            });
        }
    }

    public class Path
    {
        public Clearing ConnectTo { get; set; }
        public int? ExitSide { get; set; }
        public bool IsSecretPath { get; set; }
        public bool IsSecretPassage { get; set; }

        public Path(Clearing clearing, bool sPath = false, bool sPassage = false)
        {
            ConnectTo = clearing;
            IsSecretPassage = sPassage;
            IsSecretPath = sPath;
            ExitSide = null;
        }

        public static Path SecretPath(Clearing clearing) => new Path(clearing, true);

        public static Path SecretPassage(Clearing clearing) => new Path(clearing, false, true);

        public static Path Exit(int exitSide) => new Path(null){ExitSide = exitSide};
    }
}

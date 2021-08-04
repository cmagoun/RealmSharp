using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameUtils
{
    public static class Vector
    {
        //Some of these aren't going to work for a hex map
        //I will worry about that in a moment
        public static Vector2 Up => new Vector2(0, -1);
        public static Vector2 Down => new Vector2(0, 1);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 North => new Vector2(0, -1);
        public static Vector2 South => new Vector2(0, 1);
        public static Vector2 East => new Vector2(1, 0);
        public static Vector2 West => new Vector2(-1, 0);

        public static Vector2 NorthEast => North + East;
        public static Vector2 NorthWest => North + West;
        public static Vector2 SouthEast => South + East;
        public static Vector2 SouthWest => South + West;

        public static float DSqr(Vector2 v1, Vector2 v2)
        {
            var sub = v1 - v2;
            return (sub.X * sub.X) + (sub.Y * sub.Y);
        }
    }

    public class Space
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Key => $"{X},{Y}";
        public Space(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class SpaceComparer : IEqualityComparer<Space>
    {
        public bool Equals(Space x, Space y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(Space obj)
        {
            return HashCode.Combine(obj.X, obj.Y);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameUtils
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var x in list)
            {
                action(x);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T, int> action)
        {
            var i = 0;
            foreach (var x in list)
            {
                action(x, i);
                i++;
            }
        }

        public static T PickRandom<T>(this IEnumerable<T> list)
        {
            var roll = Roller.Next(list.Count());
            return list.Skip(roll).First();
        }

        public static T Pop<T>(this List<T> list)
        {
            var result = list.First();
            list.RemoveAt(0);
            return result;
        }
    }
}

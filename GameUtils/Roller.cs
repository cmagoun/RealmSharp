using System;
using System.Collections.Generic;
using System.Linq;

namespace GameUtils
{
    public class Roller
    {
        private static Roller _instance = null;
        private static Random _rnd;
        private static List<int> _testRolls;
        private static int _seed;

        private Roller() { }

        public static Roller Create(int? seed = null)
        {
            _seed = seed ?? Environment.TickCount;
            _rnd = new Random(_seed);

            _testRolls = new List<int>();
            _instance ??= new Roller();

            return _instance;
        }

        public static Roller Create(List<int>rolls)
        {
            Create();
            SetRolls(rolls);
            return _instance;
        }

        public static Roller Create(int? seed, params int[] rolls)
        {
            Create(seed);
            SetRolls(rolls.ToList());
            return _instance;
        }

        public static int NextD3 => Next(1, 4);
        public static int NextD4 => Next(1, 5);
        public static int NextD6 => Next(1, 7);
        public static int NextD8 => Next(1, 9);
        public static int NextD10 => Next(1, 11);
        public static int NextD12 => Next(1, 13);
        public static int NextD16 => Next(1, 17);
        public static int NextD20 => Next(1, 21);
        public static int NextD100 => Next(1, 101);
        public static MRRoll NextMR => NextMagicRealmRoll();


        private static int? GetNextTestRoll()
        {
            if (!_testRolls.Any()) return null;
            return _testRolls.Pop();
        }

        public static int GetSeedForTesting()
        {
            return _seed;
        }

        public static int Next(int max)
        {
            return GetNextTestRoll() ?? _rnd.Next(max);
        }

        public static int Next(int min, int max)
        {
            return GetNextTestRoll() ?? _rnd.Next(min, max);
        }

        public static int Next(string diceString)
        {
            var testResult = GetNextTestRoll();
            if (testResult != null) return testResult.Value;

            //take 3d6+7&1d4+1
            //roll each and add them together
            var result = 0;

            var aDiceExpressions = diceString.Split('&');

            foreach (var expression in aDiceExpressions)
            {
                //now I have 3d6+7;
                result += RollExpression(expression);
            }

            return result;
        }

        public static MRRoll NextMagicRealmRoll()
        {
            var red = NextD6;
            var white = NextD6;

            return new MRRoll(red, white);
        }

        private static int RollExpression(string diceString)
        {
            //now I have 3d6+7 or 2d4-1;
            var op = diceString.Contains('+')
                ? '+'
                : diceString.Contains('-')
                    ? '-'
                    : 'x';
        
            var aExpr = diceString.Split(op);

            var result = aExpr.Length > 1
                ? int.Parse(aExpr[1]) * (op == '-' ? -1 : 1)
                : 0;

            var aParsed = aExpr[0].Split('d');

            var numDice = int.Parse(aParsed[0]);
            var faces = int.Parse(aParsed[1]);

            for (var index = 0; index < numDice; index++)
            {
                result += Next(1, faces + 1);
            }

            return result;
        }

        public static void SetRolls(List<int> rolls)
        {
            _testRolls = rolls;
        }

        public static void SetRolls(params int[] rolls)
        {
            _testRolls = rolls.ToList();
        }
    }

    public class MRRoll
    {
        public int Red { get; set; }
        public int White { get; set; }
        public int Result => Math.Max(Red, White);

        public MRRoll(int r, int w)
        {
            Red = r;
            White = w;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace RealmSharp.GameObjects
{
    public static class TileDefs
    {
        public static string[] TileNames = new[]
        {
            "awfulvalley", 
            "badvalley",
            "curstvalley",
            "darkvalley",
            "evilvalley",

            "borderland", 
            "cavern", 
            "caves",
            "highpass",
            "ruins",
            
            "cliff",
            "crag",
            "deepwoods",
            "ledges",
            "mountain",

            "lindenwoods", 
            "maplewoods", 
            "nutwoods", 
            "oakwoods", 
            "pinewoods"
        };

        #region caves

        public static Hex Borderlands =
            new Hex(
                "BL", "Borderland", "C", "borderland",
                new[] { 1, 2, 4, 2, 1, 5 },
                new []{4},
                new Vector2?[]
                {
                    new Vector2(170, 78),
                    new Vector2(384, 205),
                    new Vector2(311, 85), 
                    new Vector2(167, 365),
                    new Vector2(187, 269),
                    new Vector2(226, 185) 
                },
                @"1/-/-/6;
                   2/-/-/3;
                   3/-/-/2,5,6;
                   4/C/-/5*,6;
                   5/C/-/3,4*;
                   6/C/-/1,3,4");

        public static Hex Cavern =
            new Hex(
                "CN", "Cavern", "C", "cavern",
                new[] { 2, 1, 0, 0, 0, 5 },
                new []{1},
                new Vector2?[]
                {
                    new Vector2(369, 145),
                    new Vector2(247, 79),
                    new Vector2(245, 182),
                    new Vector2(255, 363),
                    new Vector2(136, 151),
                    new Vector2(296, 272)
                }, 
                @"1/C/-/3,4*;
                   2/C/-/3;
                   3/C/-/1,2,5*,6;
                   4/C/-/1*,5,6;
                   5/C/-/3*,4;
                   6/C/-/3,4");

        public static Hex Caves =
            new Hex(
                "CV", "Caves", "C", "caves",
                new[] { 0, 0, 1, 0, 2, 5 },
                new []{1},
                new Vector2?[]
                {
                    new Vector2(363, 285),
                    new Vector2(134, 283),
                    new Vector2(227, 218),
                    new Vector2(273, 349),
                    new Vector2(132, 155),
                    new Vector2(312, 89)
                },
                @"1/C/-/6;
                   2/C/-/3*,4;
                   3/C/-/2*,5;
                   4/C/-/2,6;
                   5/C/-/3;
                   6/C/-/1,4");

        public static Hex HighPass =
            new Hex(
                "HP", "High Pass", "C", "highpass",
                new[] { 2, 3, 0, 5, 0, 6 },
                new []{2,3},
                new Vector2?[]
                {
                    new Vector2(337, 259),
                    new Vector2(245, 86),
                    new Vector2(350, 153),
                    new Vector2(216, 234),
                    new Vector2(248, 348),
                    new Vector2(131, 147)
                },
                @"1/M/-/4,5;
                   2/M/-/4;
                   3/C/-/6;
                   4/M/-/1,2;
                   5/M/-/1;
                   6/C/-/3");

        public static Hex Ruins =
            new Hex(
                "RU", "Ruins", "C", "ruins",
                new[] { 2, 2, 3, 0, 5, 1 },
                new []{1},
                new Vector2?[]
                {
                    new Vector2(159, 150),
                    new Vector2(324, 89),
                    new Vector2(371, 288),
                    new Vector2(235, 263),
                    new Vector2(146, 330),
                    new Vector2(328, 184)
                },
                @"1/-/-/2,4,5@;
                   2/-/-/1;
                   3/-/-/5,6;
                   4/-/-/1,6;
                   5/-/-/1@,3;
                   6/-/-/3,4");

        #endregion

        #region mountain
        public static Hex Cliff =
            new Hex(
                "CF", "Cliff", "M", "cliff",
                new[] { 0, 4, 5, 0, 2, 1 },
                new []{1},
                new Vector2?[]
                {
                    new Vector2(129, 150),
                    new Vector2(130, 287),
                    new Vector2(245, 220),
                    new Vector2(362, 150),
                    new Vector2(362, 283),
                    new Vector2(247, 86)
                },
                @"1/M/-/6;
                  2/-/-/3,5@;
                  3/-/-/2,5,6*;
                  4/M/-/6;
                  5/-/-/2@,3;
                  6/M/-/1,3*,4");

        public static Hex Crag =
            new Hex(
                "CG", "Crag", "M", "crag",
                new[] { 2, 0, 0, 0, 0, 0 },
                new []{2},
                new Vector2?[]
                {
                    new Vector2(247, 84),
                    new Vector2(230, 366),
                    new Vector2(169, 175),
                    new Vector2(320, 288),
                    new Vector2(318, 176),
                    new Vector2(161, 281)
                },
                @"1/M/-/4,6*;
                   2/M/-/3@,5;
                   3/M/-/2@,5,6;
                   4/M/-/1,6;
                   5/M/-/2,3;
                   6/M/-/1*,3,4");

        public static Hex DeepWoods =
            new Hex(
                "DW", "Deep Woods", "M", "deepwoods",
                new[] { 1, 2, 2, 0, 5, 1 },
                new []{1},
                new Vector2?[]
                {
                    new Vector2(163, 104),
                    new Vector2(379, 216),
                    new Vector2(298, 346),
                    new Vector2(84, 215),
                    new Vector2(144, 319),
                    new Vector2(238, 238)
                },
                @"1/-/-/4@,6;
                   2/-/-/3;
                   3/-/-/2,5,6@;
                   4/-/-/1@,5,6;
                   5/-/-/3,4;
                   6/-/-/1,3@,4");

        public static Hex Ledges =
            new Hex(
                "LG", "Ledges", "M", "ledges",
                new[] { 4, 2, 3, 0, 0, 5 },
                new [] {2,4},
                new Vector2?[]
                {
                    new Vector2(266, 311),
                    new Vector2(360, 154),
                    new Vector2(373, 269),
                    new Vector2(246, 207),
                    new Vector2(153, 124),
                    new Vector2(168, 347)
                },
                @"1/-/-/3@,4,6;
                   2/M/-/5;
                   3/-/-/1@,6;
                   4/-/-/1,6@;
                   5/M/-/2;
                   6/-/-/1,3,4@");

        public static Hex Mountain =
            new Hex(
                "MT", "Mountain", "M", "mountain",
                new[] { 4, 0, 5, 0, 2, 0 },
                new []{4},
                new Vector2?[]
                {
                    new Vector2(168, 173),
                    new Vector2(117, 289),
                    new Vector2(237, 282),
                    new Vector2(246, 66),
                    new Vector2(369, 286),
                    new Vector2(341, 159)
                },
                @"1/M/-/3;
                   2/-/-/4,5;
                   3/M/-/1,6;
                   4/-/-/2,6@;
                   5/M/-/2,6;
                   6/M/-/3,4@,5");

        #endregion

        #region woods

        private static string Woods1 =
            @"2/-/-/4;
              4/-/-/2;
              5/-/-/-";

        private static int[] WoodsChecks = new[] {4, 5};

        public static Hex LindenWoods =
            new Hex(
                "LW", "Linden Woods", "W", "lindenwoods",
                new[] { 5, 5, 2, 0, 2, 4 },
                WoodsChecks,
                new Vector2?[]
                {
                    null,
                    new Vector2(244, 272),
                    null,
                    new Vector2(126, 149),
                    new Vector2(293, 120),
                    null
                },
                Woods1);

        public static Hex MapleWoods =
            new Hex(
                "MW", "Maple Woods", "W", "maplewoods",
                new[] { 2, 2, 4, 0, 5, 5 },
                WoodsChecks,
                new Vector2?[]
                {
                    null,
                    new Vector2(312, 109),
                    null,
                    new Vector2(362, 271),
                    new Vector2(133, 212),
                    null
                },
                Woods1);

        public static Hex NutWoods =
            new Hex(
                "NW", "Nut Woods", "W", "nutwoods",
                new[] { 2, 4, 5, 0, 5, 2 },
                WoodsChecks,
                new Vector2?[]
                {
                    null,
                    new Vector2(185, 106),
                    null,
                    new Vector2(347, 129),
                    new Vector2(241, 280),
                    null
                },
                Woods1);

        public static Hex OakWoods =
            new Hex(
                "OW", "Oak Woods", "W", "oakwoods",
                new[] { 5, 2, 2, 0, 4, 5 },
                WoodsChecks,
                new Vector2?[]
                {
                    null,
                    new Vector2(357, 202),
                    null,
                    new Vector2(131, 279),
                    new Vector2(188, 114),
                    null
                },
                Woods1);

        public static Hex PineWoods =
            new Hex(
                "PW", "Pine Woods", "W", "pinewoods",
                new[] { 4, 5, 5, 0, 2, 2 },
                WoodsChecks,
                new Vector2?[]
                {
                    null,
                    new Vector2(113, 187),
                    null,
                    new Vector2(227, 79),
                    new Vector2(354, 207),
                    null
                },
                Woods1);

        #endregion

        #region valleys
        private static string StdValley =
            @"1/-/-/4;
              2/-/-/5;
              4/-/-/1;
              5/-/-/2";

        private static int[] ValleyChecks = new[] {4, 5};

        public static Hex AwfulValley =
            new Hex(
                "VA", "Awful Valley", "V", "awfulvalley",
                new[] { 5, 4, 4, 0, 2, 1 },
                ValleyChecks,
                new Vector2?[]
                {
                    new Vector2(130, 153),
                    new Vector2(128, 277),
                    null,
                    new Vector2(307, 255),
                    new Vector2(243, 88),
                    null
                }, 
                StdValley);

        public static Hex BadValley =
            new Hex(
                "VB", "Bad Valley", "V", "badvalley",
                new[]{4, 5, 1, 0, 2, 4},
                ValleyChecks,
                new Vector2?[]
                {
                    new Vector2(364, 282),
                    new Vector2(135, 284),
                    null,
                    new Vector2(182, 103),
                    new Vector2(358, 151),
                    null
                },
                StdValley);

        public static Hex CurstValley =
            new Hex(
                "VC", "Curst Valley", "V", "curstvalley",
                new[] {1, 5, 4, 0, 4, 2},
                ValleyChecks,
                new Vector2?[]
                {
                    new Vector2(247, 85),
                    new Vector2(131, 150),
                    null,
                    new Vector2(245, 286),
                    new Vector2(358, 152),
                    null
                },
                StdValley);

        public static Hex DarkValley =
            new Hex(
                "VD", "Dark Valley", "V", "darkvalley",
                new []{2, 1, 5, 0, 4, 4},
                ValleyChecks,
                new Vector2?[]
                {
                    new Vector2(365, 153),
                    new Vector2(253, 85),
                    null,
                    new Vector2(185, 254),
                    new Vector2(361, 282),
                    null
                },
                StdValley);

        public static Hex EvilValley =
            new Hex(
                "VE", "Evil Valley", "V", "evilvalley",
                new []{4, 4, 5, 0, 1, 2},
                ValleyChecks,
                new Vector2?[]
                {
                    new Vector2(131, 282),
                    new Vector2(132, 152),
                    null,
                    new Vector2(315, 103),
                    new Vector2(360, 281),
                    null
                },
                StdValley);

        #endregion

        public static List<Hex> AllGreen = new List<Hex>
        {
            Borderlands,
            Cavern,
            Caves,
            HighPass,
            Ruins,
            
            Cliff,
            Crag,
            DeepWoods,
            Ledges,
            Mountain,
            
            LindenWoods,
            NutWoods,
            PineWoods,
            MapleWoods,
            OakWoods,
        
            EvilValley,
            DarkValley,
            AwfulValley,
            BadValley,
            CurstValley
        };

    }
}

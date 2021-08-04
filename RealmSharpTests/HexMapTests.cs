using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RealmSharp.GameObjects;

namespace RealmSharpTests
{
    public class HexMapTests
    {
        [Test]
        public void CanFindClearingsAtDistance()
        {
            var hm = new HexMap();
            hm.Initialize(TileDefs.AllGreen);

            hm.PlaceHex("BL");
            hm.PlaceHex(TileDefs.AwfulValley, 0, -1, 1);
            hm.PlaceHex(TileDefs.Ledges, 1, -1, 4);

            var clearings = hm.ClearingsAtDistance("BL6", 1);
            Assert.AreEqual(3, clearings.Count);

            var c2 = hm.ClearingsAtDistance("VA4", 2);
            Assert.AreEqual(2, c2.Count);
            Assert.IsTrue(c2.Select(x => x.Key).Contains("BL6"));
            Assert.IsTrue(c2.Select(x => x.Key).Contains("LG5"));


            var c4 = hm.ClearingsAtDistance(TileDefs.Ledges.Key + "1", 2);
            Assert.AreEqual(4, c4.Count);
        }
    }
}

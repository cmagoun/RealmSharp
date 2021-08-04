using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using RealmSharp.GameObjects;

namespace RealmSharpTests
{
    public class MapMakerTests
    {
        [Test]
        public void CanScoreMap()
        {
            var hm = new HexMap();
            hm.Initialize(TileDefs.AllGreen);
            hm.PlaceHex("BL");
            hm.PlaceHex(TileDefs.AwfulValley, 0, -1, 1);

            Assert.AreEqual(1, MapMaker.ScoreMap(hm));

            hm.PlaceHex(TileDefs.Ledges, 1, -1, 4);

            //+1 for BL, -1 for Mt. clr
            Assert.AreEqual(0, MapMaker.ScoreMap(hm));

            hm.PlaceHex(TileDefs.Cavern, 1, -2, 4);
            hm.PlaceHex(TileDefs.Cliff, 0, -2, 1);

            //dwelling is now in 5, -2 for cavern
            Assert.AreEqual(-2, MapMaker.ScoreMap(hm));

            //non-mt treasure clearing off 2, scores +1
            hm.PlaceHex(TileDefs.Mountain, -1, -2, 4);
            Assert.AreEqual(-1, MapMaker.ScoreMap(hm));
        }

        
    }
}

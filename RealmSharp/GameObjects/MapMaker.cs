using GameUtils;
using System.Linq;

namespace RealmSharp.GameObjects
{
    public class MapMaker
    {
        public static HexMap CreateRandomMap(int minScore = 0)
        {
            HexMap hm = null;

            while (ScoreMap(hm) < minScore)
            {
                hm = TryToCreateMap();
            }

            return hm;
        }

        private static HexMap TryToCreateMap()
        {
            var tries = 0;
            var hm = new HexMap();
            hm.Initialize(TileDefs.AllGreen);
            hm.PlaceHex("BL");

            while (hm.NotPlaced.Any() && tries < 1000)
            {
                //This does not even check yet...
                var hex = ChooseRandomHex(hm);
                var pos = ChooseRandomPosition(hm);
                var orient = ChooseRandomOrientation();

                if (hm.CheckPlacement(hex, pos.X, pos.Y, orient))
                {
                    hm.PlaceHex(hex, pos.X, pos.Y, orient);
                }

                tries++;
            }

            //if we are over 1000 tries, we are stuck
            return tries < 1000 ? hm : null;
        }

        private static Space ChooseRandomPosition(HexMap hm)
        {
            var emptySpaces = hm.Placed
                .SelectMany(h => hm.EmptySpacesAdjacentTo(h.X, h.Y))
                .Distinct(new SpaceComparer())
                .ToList();

            var validSpaces = hm.Placed.Count > 1
                ? emptySpaces
                    .Where(es => hm.HexesAdjacentTo(es.X, es.Y).Count > 1)
                    .ToList()
                : emptySpaces;

            return validSpaces.PickRandom();
        }

        private static Hex ChooseRandomHex(HexMap hm)
        {
            return hm.NotPlaced.PickRandom();
        }

        private static int ChooseRandomOrientation()
        {
            return Roller.NextD6 - 1;
        }

        public static int ScoreMap(HexMap hm)
        {
            if (hm == null) return -99999;
            //We are using specific knowledge of how MR does valleys.
            //If this changes, or if there are different types of dwellings, we
            //need to change this scoring system a little.
            var score = 0;
            var graph = hm.Graph;

            var valleys = hm.Placed.Where(h => h.Hex.HexType == "V");

            //Each valley has 5/2 clearings, or 4/1 clearings leading from the dwelling.
            //Let's check what is in the hexes 
            valleys.ForEach(v =>
            {
                var dwellingClearing = hm.DwellingClearing(v.Hex);

                var keysToCheck = dwellingClearing.Number == 5
                    ? new[] {dwellingClearing.Key, v.Hex.Key + "2"}
                    : new[] {dwellingClearing.Key, v.Hex.Key + "1"};

                var nearby = keysToCheck
                    .SelectMany(key => hm.ClearingsAtDistance(key, 1))
                    .ToList();

                score += nearby.Aggregate(0, (x, n) => x += ScoreNearby(n, v.Hex.Key));
            });

            return score;
        }


        private static int ScoreNearby(Clearing nearby, string startKey)
        {
            if (nearby == null || nearby.Parent.Key == startKey) return 0;

            if (nearby.Parent.Key == TileDefs.Caves.Key) return -2;
            if (nearby.Parent.Key == TileDefs.Cavern.Key) return -2;
            if (nearby.Parent.Key == TileDefs.HighPass.Key) return -2;
            if (nearby.ClearingType == "M") return -1;
            if (nearby.ClearingType == "C") return -1;
            if (nearby.Parent.HexType == "V") return 1;
            if (nearby.Parent.Clearings.Count == 6) return 1;

            return 0;
        }
    }

    //Possible scoring system:
        //Check every valley for dwelling clearing
        //From dwelling clearing
        //  Check all exits (from whole hex, so 2 exits from 5, or 3 from 4)
        //      Score big minus if it runs into Caves, Cavern, High Pass or Cave clearing
        //      Score little minus if it runs into Mountain clearing
        //      Score positive if it runs into treasure hex and not Cave/Mountain clearing
    
}

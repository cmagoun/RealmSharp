using EMK.Cartography;
using RealmSharp.GameObjects;

namespace RealmSharp
{
    public class MRData 
    {
        public HexMap Map { get; set; }

        public MRData()
        {
            Map = new HexMap();
        }
    }
}

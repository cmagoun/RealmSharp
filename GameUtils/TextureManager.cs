using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace GameUtils
{
    public class TextureManager
    {
        private readonly Dictionary<string, Texture2D> _dc;

        public TextureManager()
        {
            _dc = new Dictionary<string, Texture2D>();
        }

        public Texture2D Add(string key, Texture2D tex)
        {
            if(!_dc.ContainsKey(key)) _dc.Add(key, tex);
            return tex;
        }

        public Texture2D Get(string key)
        {
            return _dc.GetValueOrDefault(key, null);
        }
    }
}

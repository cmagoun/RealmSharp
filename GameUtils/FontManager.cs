using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameUtils
{
    public class FontManager
    {
        private readonly Dictionary<string, SpriteFont> _fonts;

        public FontManager()
        {
            _fonts = new Dictionary<string, SpriteFont>();
        }

        public void AddFont(string key, SpriteFont font)
        {
            _fonts.Add(key, font);
        }

        public SpriteFont GetFont(string key)
        {
            return _fonts[key];
        }
    }
}

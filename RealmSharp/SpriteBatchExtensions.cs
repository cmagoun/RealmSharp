using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RealmSharp.GameObjects;
using System;

namespace RealmSharp
{
    public static class SpriteBatchExtensions
    {
        public static void DrawHex(this SpriteBatch sb, Texture2D tex, int x, int y, int orientation, Color? tint = null)
        {
            var baseX = x * Hex.COL_WIDTH;
            var baseY = (y * Hex.HEX_HEIGHT) + (Math.Abs(x % 2) * Hex.VERT_OFFSET);
            var rot = (float)(orientation * (Math.PI / 3f)); //pi/3 == 60d
            var color = tint ?? Color.White;

            sb.Draw(
                tex,
                new Vector2(baseX, baseY),
                null,
                color,
                rot,
                new Vector2(248, 215),
                Vector2.One,
                SpriteEffects.None,
                1.0f);
        }

    }
}

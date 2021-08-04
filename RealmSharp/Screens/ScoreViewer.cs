using System;
using System.Collections.Generic;
using System.Text;
using GameUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RealmSharp.GameObjects;

namespace RealmSharp.Screens
{
    public class ScoreViewer:Screen<MRData>
    {
        private FontManager _fontMgr;
        private SpriteFont _font;

        public override string Key => "ScoreViewer";
        public override bool IsAffectedByCamera => false;

        public override void Initialize(MRData gameData, GameServiceContainer services)
        {
            _fontMgr = services.GetService<FontManager>();
            _font = _fontMgr.GetFont("Arial");

            base.Initialize(gameData, services);
        }

        public override void Draw(MRData gameData, SpriteBatch sb, GameTime gameTime)
        {
            var score = MapMaker.ScoreMap(gameData.Map);
            sb.DrawString(_font, $"SCORE: {score}", Vector2.One*10, Color.Black);
        }
    }
}

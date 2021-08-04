//using GameUtils;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace RealmSharp.Sample
//{
//    public class Screen2:Screen<GameData>
//    {
//        private FontManager _fontMgr;
//        private SpriteFont _font;
//        private KeyboardManager _keyMgr;

//        public override string Key => "Screen2";
//        public override bool IsAffectedByCamera => true;

//        public override void Initialize(GameData gameData, GameServiceContainer services)
//        {
//            _fontMgr = services.GetService<FontManager>();
//            _font = _fontMgr.GetFont("Arial");
            
//            base.Initialize(gameData, services);
//        }
        

//        public override void Draw(GameData gameData, SpriteBatch sb, GameTime gameTime)
//        {
//            sb.DrawString(_font, "THIS IS AFFECTED", new Vector2(500, 500), Color.Black);
//        }
        
//    }
//}

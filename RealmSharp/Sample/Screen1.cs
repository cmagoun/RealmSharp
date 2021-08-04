//using GameUtils;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace RealmSharp.Sample
//{
//    public class Screen1:Screen<GameData>
//    {
//        private FontManager _fontMgr;
//        private SpriteFont _font;
//        private KeyboardManager _keyMgr;
//        private CameraManager _camera;
//        private MouseManager _mouse;
//        private MouseStateArgs _mouseArgs;

//        public override string Key => "Screen1";

//        public override void Initialize(GameData gameData, GameServiceContainer services)
//        {
//            _fontMgr = services.GetService<FontManager>();
//            _font = _fontMgr.GetFont("Arial");

//            _keyMgr = services.GetService<KeyboardManager>();
//            _keyMgr.KeyDown += KeyPressed;

//            _mouse = services.GetService<MouseManager>();
//            _mouse.MouseClick += MouseClick;
//            _mouse.MouseScroll += MouseScroll;

//            _camera = services.GetService<CameraManager>();
            
//            base.Initialize(gameData, services);
//        }

//        private void MouseScroll(object sender, int delta)
//        {
//            _camera.ChangeZoom(delta / 1200f);
//        }

//        private void MouseClick(object sender, MouseStateArgs args)
//        {
//            _mouseArgs = args;
//        }

//        private void KeyPressed(object sender, KeyStateArgs args)
//        {
//            if (!IsTopScreen) return;

//            var key = args.Key;
//            if (key == Keys.A) _camera.ChangePosition(Vector.Left*10);
//            if (key == Keys.D) _camera.ChangePosition(Vector.Right*10);
//            if (key == Keys.W) _camera.ChangePosition(Vector.Up*10);
//            if (key == Keys.S) _camera.ChangePosition(Vector.Down*10);
//        }
        
//        public override void Draw(GameData gameData, SpriteBatch sb, GameTime gameTime)
//        {
//            sb.DrawString(_font, "THIS IS NOT AFFECTED", new Vector2(500, 500), Color.Black);
            
//            if(_mouseArgs != null) sb.DrawString(_font, $"{_mouseArgs.X}, {_mouseArgs.Y} // {_mouseArgs.WorldX}, {_mouseArgs.WorldY}", new Vector2(0, 1000), Color.Black);
//        }
//    }
//}

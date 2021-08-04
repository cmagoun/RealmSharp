using GameUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RealmSharp.GameObjects;

namespace RealmSharp.Screens
{
    public class MapView : Screen<MRData>
    {
        private const float CAM_SPEED = 15;

        private KeyboardManager _keyMgr;
        private MouseManager _mouseMgr;
        private TextureManager _tex;
        private Camera _camera;
        private MRData _data;
        private bool _score;
        private ScreenManager<MRData> _screenMgr;

        public override string Key => "HexScreen";
        public override bool IsAffectedByCamera => true;

        public override void Initialize(MRData gameData, GameServiceContainer services)
        {
            _keyMgr = services.GetService<KeyboardManager>();
            _mouseMgr = services.GetService<MouseManager>();
            _tex = services.GetService<TextureManager>();
            _camera = services.GetService<Camera>();
            _screenMgr = services.GetService<ScreenManager<MRData>>();

            _data = gameData;

            _mouseMgr.MouseScroll += MouseScroll;
            _keyMgr.KeyDown += KeyDown;
            _keyMgr.KeyPressed += KeyPressed;

            base.Initialize(gameData, services);
        }

        private void KeyPressed(object sender, KeyStateArgs args)
        {
            if (args.Key == Keys.OemTilde && args.CtlPressed)
            {
                _data.Map = MapMaker.CreateRandomMap();
            }
        }

        private void KeyDown(object sender, KeyStateArgs args)
        {
            var key = args.Key;

            if(key == Keys.W) _camera.MoveCamera(Vector.Up * CAM_SPEED);
            if (key == Keys.D) _camera.MoveCamera(Vector.Right * CAM_SPEED);
            if (key == Keys.S) _camera.MoveCamera(Vector.Down * CAM_SPEED);
            if (key == Keys.A) _camera.MoveCamera(Vector.Left * CAM_SPEED);
        }

        private void MouseScroll(object sender, int delta)
        {
            _camera.AdjustZoom(delta / 1200f);
        }

        public override void Update(MRData gameData, GameTime gameTime)
        {
            var mpos = _mouseMgr.GetPosition();
            if(mpos.X < 20) _camera.MoveCamera(Vector.Left * CAM_SPEED);
            if(mpos.Y < 20) _camera.MoveCamera(Vector.Up * CAM_SPEED);
            if(mpos.X > _camera.ViewportWidth - 20) _camera.MoveCamera(Vector.Right * CAM_SPEED);
            if(mpos.Y > _camera.ViewportHeight - 20) _camera.MoveCamera(Vector.Down * CAM_SPEED);

            base.Update(gameData, gameTime);
        }

        public override void Draw(MRData gameData, SpriteBatch sb, GameTime gameTime)
        {
            //Draw the Placed hexes in the map
            var toDraw = gameData.Map.Placed;

            foreach (var hexPos in toDraw)
            {
                var tex = _tex.Get(hexPos.Hex.ImageKey + "-g");
                sb.DrawHex(tex, hexPos.X, hexPos.Y, hexPos.Orientation);
            }
        }
    }
}

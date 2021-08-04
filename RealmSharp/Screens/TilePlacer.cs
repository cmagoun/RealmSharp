using GameUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RealmSharp.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealmSharp.Screens
{
    public class TilePlacer:Screen<MRData>
    {
        private MouseManager _mouseMgr;
        private FontManager _fontMgr;
        private TextureManager _texMgr;
        public override string Key => "TilePlacer";
        public override bool IsAffectedByCamera => true;

        private int HEX_RADIUS = Hex.RECT_WIDTH + Hex.TRI_WIDTH;
        private int ROTATE_CLOCK = 1;
        private int ROTATE_COUNTER = -1;

        private KeyboardManager _keyMgr;

        private Hex _currentHex;
        private int _orientation;
        private HexMap _map;
        private int _x;
        private int _y;

        private bool _setError;
        private bool _isError;
        private int _timeError;
        private ScreenManager<MRData> _screenMgr;

        public override void Initialize(MRData gameData, GameServiceContainer services)
        {
            _fontMgr = services.GetService<FontManager>();
            _mouseMgr = services.GetService<MouseManager>();
            _texMgr = services.GetService<TextureManager>();
            _keyMgr = services.GetService<KeyboardManager>();
            _screenMgr = services.GetService<ScreenManager<MRData>>();

            _keyMgr.KeyPressed += KeyPressed;
            _mouseMgr.MouseClick += MouseClick;

            _map = gameData.Map;
            SelectNewHex();

            base.Initialize(gameData, services);
        }

        private void MouseClick(object sender, MouseStateArgs args)
        {
            PlaceHex();
        }

        private void KeyPressed(object sender, KeyStateArgs args)
        {
            var key = args.Key;
            if (key == Keys.Q) ChangeOrientation(ROTATE_COUNTER);
            if (key == Keys.E) ChangeOrientation(ROTATE_CLOCK);
            if (key == Keys.Space) PlaceHex();
            if (key == Keys.X) SelectNewHex();
        }

        private void ChangeOrientation(in int rotate)
        {
            _orientation += rotate;
            if (_orientation > 5) _orientation = 0;
            if (_orientation < 0) _orientation = 5;
        }

        private void PlaceHex()
        {
            if (_map.CheckPlacement(_currentHex, _x, _y, _orientation))
            {
                _map.PlaceHex(_currentHex, _x, _y, _orientation);
                SelectNewHex();
            }
            else
            {
                _setError = true;
            }
        }

        private void SelectNewHex()
        {
            if (!_map.NotPlaced.Any())
            {
                _screenMgr.Remove(Key);
                return;
            }

            _currentHex = _map.NotPlaced.PickRandom();
            _orientation = 0;
        }

        public override void Update(MRData gameData, GameTime gameTime)
        {
            HandleError(gameTime);

            var cursorPos = _mouseMgr.GetWorldPosition();

            //Effectively breaking the map into rectangles which mostly cover a single hex,
            //but also have the "triangles" from both hexes to the right.
            var tPos = cursorPos + new Vector2(Hex.TRI_WIDTH, Hex.HEX_HEIGHT / 2);

            var approxCol = (int)Math.Floor((tPos.X / (Hex.RECT_WIDTH + Hex.TRI_WIDTH)));

            var approxRow = approxCol % 2 == 0
                ? (int)Math.Floor(tPos.Y / Hex.HEX_HEIGHT)
                : (int)Math.Floor((tPos.Y - Hex.HEX_HEIGHT / 2) / Hex.HEX_HEIGHT);

            var candidates = CreateCandidates(approxCol, approxRow);

            //Once we have our 3 candidates, whichever hex center is closest is the one
            var dSqr = candidates.Select(c => Vector.DSqr(cursorPos, Hex.Center(c.X, c.Y))).ToArray();
            var index = Array.IndexOf(dSqr, dSqr.Min());

            _x = candidates[index].X;
            _y = candidates[index].Y;

            base.Update(gameData, gameTime);
        }

        private void HandleError(GameTime gameTime)
        {
            if (_setError)
            {
                _timeError = 0;
                _isError = true;
                _setError = false;
            }
            else
            {
                _timeError += gameTime.ElapsedGameTime.Milliseconds;
                if (_timeError > 500)
                {
                    _isError = false;
                }
            }
        }

        public override void Draw(MRData gameData, SpriteBatch sb, GameTime gameTime)
        {
            var font = _fontMgr.GetFont("Arial");
            var cursor = _texMgr.Get($"{_currentHex.ImageKey}-g");
            var skull = _texMgr.Get("skull");
            //sb.DrawString(font, $"Cursor: {cursorPos.X}, {cursorPos.Y}", new Vector2(350, -200), Color.White);

            sb.DrawHex(cursor, _x, _y, _orientation, _isError ? Color.Red : Color.Gray);

            //This is just an example of how to draw something w. relation to the clearings
            //Might need a Clearing.DrawPosition kind of idea to make this easier.
            // var origin = Hex.TopLeft(_x, _y);
            // var anchors = Hex.GetAnchors(_currentHex, _orientation).Where(a => a!=null);
            // anchors.ForEach(a => sb.Draw(skull, origin + a.Value, Color.White));


            // sb.DrawString(font, $"HexA: {c[0].X}, {c[0].Y} / {Hex.Center(c[0].X, c[0].Y)} / {DistSquared(cursorPos, Hex.Center(c[0].X, c[0].Y))}", new Vector2(350, -180), Color.Black);
            // sb.DrawString(font, $"HexB: {c[1].X}, {c[1].Y} / {Hex.Center(c[1].X, c[1].Y)} / {DistSquared(cursorPos, Hex.Center(c[1].X, c[1].Y))}", new Vector2(350, -160), Color.Black);
            // sb.DrawString(font, $"HexC: {c[2].X}, {c[2].Y} / {Hex.Center(c[2].X, c[2].Y)} / {DistSquared(cursorPos, Hex.Center(c[2].X, c[2].Y))}", new Vector2(350, -140), Color.Black);
        }

        private List<Space> CreateCandidates(int approxCol, int approxRow)
        {
            var result = new List<Space>();

            result.Add(new Space(approxCol, approxRow));

            if (approxCol % 2 == 0)
            {
                result.Add(new Space(approxCol+1, approxRow-1));
                result.Add(new Space(approxCol+1, approxRow));
            }
            else
            {
                result.Add(new Space(approxCol+1, approxRow));
                result.Add(new Space(approxCol+1, approxRow+1));
            }

            return result;
        }
    }
}

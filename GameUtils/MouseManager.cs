using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameUtils
{
    public enum MouseButton {Left, Right, Center}
    public delegate void MouseHandler(object sender, MouseStateArgs args);
    public delegate void ScrollHandler(object sender, int delta);
    
    public class MouseManager
    {
        private MouseState _oldState;
        private MouseState _currentState;
        private Camera _camera;

        public MouseHandler MouseDown;
        public MouseHandler MouseUp;
        public MouseHandler MouseClick;
        public MouseHandler MouseMove;
        public ScrollHandler MouseScroll;

        public MouseHandler MouseRightClick;
        public MouseHandler MouseCenterClick;

        public MouseHandler MouseDrag;
        public MouseHandler MouseDragEnd;

        private int _oldWorldX;
        private int _oldWorldY;

        private bool _dragging;

        public void Update(GameTime gameTime)
        {
            _currentState = Mouse.GetState();

            var pos = GetPosition();
            var wpos = GetWorldPosition();

            var x = (int) pos.X;
            var y = (int) pos.Y;
            var wx = (int) wpos.X;
            var wy = (int) wpos.Y;

            if (HasScrolled()) MouseScroll?.Invoke(this, _currentState.ScrollWheelValue - _oldState.ScrollWheelValue);

            if (_dragging)
            {
                if(HasMouseMoved()) MouseDrag?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY));
                if (!IsButtonDown(MouseButton.Left))
                {
                    _dragging = false;
                    MouseDragEnd?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY));
                }
            }

            if (HasMouseMoved())
            {
                if (IsButtonDown(MouseButton.Left))
                {
                    MouseDrag?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY ));
                    _dragging = true;
                }
                else
                {
                    MouseMove?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY));
                }
            }
            else
            {
                if (IsButtonDown(MouseButton.Left)) MouseDown?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY));
                if (IsButtonReleased(MouseButton.Left)) MouseUp?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY));
                if (IsButtonClicked(MouseButton.Left)) MouseClick?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY));
                if (IsButtonClicked(MouseButton.Right)) MouseRightClick?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY));
                if (IsButtonClicked(MouseButton.Center)) MouseCenterClick?.Invoke(this, new MouseStateArgs(x, y, wx, wy, _oldState.X, _oldState.Y, _oldWorldX, _oldWorldY));
            }

            _oldState = _currentState;
            _oldWorldX = wx;
            _oldWorldY = wy;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(_currentState.X, _currentState.Y);
        }

        public Vector2 GetWorldPosition()
        {
            if (_camera == null) return GetPosition();
            return _camera.ScreenToWorld(GetPosition());
        }

        public void SetCamera(Camera cam)
        {
            _camera = cam;
        }

        private bool HasMouseMoved()
        {
            return _currentState.X != _oldState.X || _currentState.Y != _oldState.Y;
        }

        private bool HasScrolled()
        {
            return _currentState.ScrollWheelValue != _oldState.ScrollWheelValue;
        }

        private bool IsButtonDown(MouseButton button)
        {
            var (bstate, ostate) = GetButtonStates(button);
            return bstate == ButtonState.Pressed;
        }

        private bool IsButtonReleased(MouseButton button)
        {
            var (bstate, ostate) = GetButtonStates(button);
            return bstate == ButtonState.Released && ostate == ButtonState.Pressed;
        }

        private bool IsButtonClicked(MouseButton button)
        {
            return IsButtonDown(button) && _currentState.X == _oldState.X && _currentState.Y == _oldState.Y;
        }

        private Tuple<ButtonState, ButtonState> GetButtonStates(MouseButton button)
        {
            var bstate = button == MouseButton.Left
                ? _currentState.LeftButton
                : button == MouseButton.Right
                    ? _currentState.RightButton
                    : _currentState.MiddleButton;

            var ostate = button == MouseButton.Left
                ? _oldState.LeftButton
                : button == MouseButton.Right
                    ? _oldState.RightButton
                    : _oldState.MiddleButton;

            return new Tuple<ButtonState, ButtonState>(bstate, ostate);
        }
    }
    
    public class MouseStateArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int OldX { get; set; }
        public int OldY { get; set; }
        
        public int WorldX { get; set; }
        public int WorldY { get; set; }
        public int OldWorldX { get; set; }
        public int OldWorldY { get; set; }

        // public MouseStateArgs(int x, int y, int oldX, int oldY)
        // {
        //     X = x;
        //     Y = y;
        //     OldX = oldX;
        //     OldY = oldY;
        //     WorldX = x;
        //     WorldY = y;
        //     OldWorldX = oldX;
        //     OldWorldY = oldY;
        // }

        public MouseStateArgs(int x, int y, int wx, int wy, int oldX, int oldY, int oldWx, int oldWy)
        {
            X = x;
            Y = y;
            OldX = oldX;
            OldY = oldY;
            WorldX = wx;
            WorldY = wy;
            OldWorldX = oldWx;
            OldWorldY = oldWy;
        }
    }
}
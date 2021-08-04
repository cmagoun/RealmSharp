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

        public void Update(GameTime gameTime)
        {
            _currentState = Mouse.GetState();
            var pos = GetPosition();
            var wpos = GetWorldPosition();

            var x = (int) pos.X;
            var y = (int) pos.Y;
            var wx = (int) wpos.X;
            var wy = (int) wpos.Y;
            
            //Do I need more events? What about DRAG?
            if(HasScrolled()) MouseScroll?.Invoke(this, _currentState.ScrollWheelValue - _oldState.ScrollWheelValue);
            if(HasMouseMoved()) MouseMove?.Invoke(this, new MouseStateArgs(x, y, wx, wy));
            
            if(IsButtonDown(MouseButton.Left)) MouseDown?.Invoke(this, new MouseStateArgs(x, y, wx, wy));
            if(IsButtonReleased(MouseButton.Left)) MouseUp?.Invoke(this, new MouseStateArgs(x, y, wx, wy));
            if(IsButtonClicked(MouseButton.Left)) MouseClick?.Invoke(this, new MouseStateArgs(x, y, wx, wy));
            
            if(IsButtonClicked(MouseButton.Right)) MouseRightClick?.Invoke(this, new MouseStateArgs(x, y, wx, wy));
            if(IsButtonClicked(MouseButton.Center)) MouseCenterClick?.Invoke(this, new MouseStateArgs(x, y, wx, wy));
            
            _oldState = _currentState;
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
            return bstate == ButtonState.Pressed && ostate == ButtonState.Released;
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
        
        public int WorldX { get; set; }
        public int WorldY { get; set; }

        public MouseStateArgs(int x, int y)
        {
            X = x;
            Y = y;
            WorldX = x;
            WorldY = y;
        }

        public MouseStateArgs(int x, int y, int wx, int wy)
        {
            X = x;
            Y = y;
            WorldX = wx;
            WorldY = wy;
        }
    }
}
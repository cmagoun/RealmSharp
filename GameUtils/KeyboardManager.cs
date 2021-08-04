using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace GameUtils
{
    public delegate void KeyHandler(object sender, KeyStateArgs args);


    public class KeyboardManager
    {
        private IEnumerable<Keys> _downLastCycle;

        public KeyHandler KeyDown; //fires every cycle the key is down
        public KeyHandler KeyUp; //fires the cycle the key is released
        public KeyHandler KeyPressed; //fires when a key is pressed and then released (same as keyup?)

        private KeyboardState _state;

        public KeyboardManager()
        {
            _downLastCycle = new List<Keys>();
        }

        public void Update(GameTime gameTime)
        {
            _state = Keyboard.GetState();

            var keysThisCycle = _state.GetPressedKeys();

            foreach (var key in keysThisCycle)
            {
                KeyDown?.Invoke(this, GetArgs(key));
            }

            var keysReleasedThisCycle = _downLastCycle.Where(key => !keysThisCycle.Contains(key)).ToList();

            foreach (var key in keysReleasedThisCycle)
            {
                KeyUp?.Invoke(this, GetArgs(key));
                KeyPressed?.Invoke(this, GetArgs(key));
            }

            _downLastCycle = keysThisCycle;
        }

        private KeyStateArgs GetArgs(Keys key)
        {
            return new KeyStateArgs(key, IsAltDown(), IsCtlDown(), IsShiftDown());
        }

        private bool IsAltDown()
        {
            return _state.IsKeyDown(Keys.LeftAlt) || _state.IsKeyDown(Keys.RightAlt);
        }

        private bool IsCtlDown()
        {
            return _state.IsKeyDown(Keys.LeftControl) || _state.IsKeyDown(Keys.RightControl);
        }

        private bool IsShiftDown()
        {
            return _state.IsKeyDown(Keys.LeftShift) || _state.IsKeyDown(Keys.RightShift);
        }
    }

    public class KeyStateArgs : EventArgs
    {
        public Keys Key { get; set; }
        public bool AltPressed { get; set; }
        public bool ShiftPressed { get; set; }
        public bool CtlPressed { get; set; }

        public KeyStateArgs(Keys key, bool alt, bool ctl, bool shift)
        {
            Key = key;
            AltPressed = alt;
            CtlPressed = ctl;
            ShiftPressed = shift;
        }
    }
}

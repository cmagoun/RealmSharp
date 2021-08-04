using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GameUtils
{
    public enum Screens {WithCamera, NoCamera}
    
    public class ScreenManager<TData>
    {
        private readonly List<IScreen<TData>> _screens;
        private string _existingKeys;
        private readonly Game _game;
        private readonly TData _data;

        public IScreen<TData> TopScreen => _screens.Last();
        public string TopKey => TopScreen.Key;

        public ScreenManager(Game game, TData data)
        {
            _screens = new List<IScreen<TData>>();
            _game = game;
            _data = data;
        }

        public void SetScreens(params IScreen<TData>[] screens)
        {
            //If we are setting up the same screens, do nothing
            var keys = string.Join('/', screens.Select(s => s.Key));
            if (keys == _existingKeys) return;

            foreach (var screen in _screens)
            {
                screen.TearDown(_data, _game.Services);
            }

            _screens.Clear();


            foreach (var screen in screens)
            {
                PushScreen(screen);
            }

            _existingKeys = string.Join('/', _screens.Select(s => s.Key));
        }

        //Do we need to worry about putting the same screen here twice?
        public void PushScreen(IScreen<TData> screen)
        {
            //Don't allow two of the same screen on at the same time
            if (_screens.Select(s => s.Key).Contains(screen.Key)) return;

            if (!screen.IsInitialized)
            {
                screen.SetManager(this);
                screen.Initialize(_data, _game.Services);
            }
            _screens.Add(screen);
        }

        public void Remove(string key)
        {
            var screen = _screens.SingleOrDefault(s => s.Key == key);
            if (screen == null) return;

            _screens.Remove(screen);
        }


        public void Update(TData gameData, GameTime gameTime)
        {
            foreach (var screen in _screens)
            {
                screen.Update(gameData, gameTime);
            }
        }

        public void Draw(TData gameData, SpriteBatch sb, GameTime gameTime, Screens useCamera = Screens.NoCamera)
        {
            var screens = useCamera == Screens.WithCamera
                ? _screens.Where(s => s.IsAffectedByCamera).ToList()
                : _screens.Where(s => !s.IsAffectedByCamera).ToList();
            
            var length = screens.Count;
            for(var i = length-1; i > -1; i--)
            {
                screens[i].Draw(gameData, sb, gameTime);
            }
        }
    }
}

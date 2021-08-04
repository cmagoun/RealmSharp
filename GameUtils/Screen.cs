using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameUtils
{
    public interface IScreen<TData>
    {
        public string Key { get; }
        public bool IsAffectedByCamera { get; }
        public bool IsInitialized { get; }
        public void Initialize(TData gameData, GameServiceContainer services);
        public void TearDown(TData gameData, GameServiceContainer services);
        public void Update(TData gameData, GameTime gameTime);
        public void Draw(TData gameData, SpriteBatch sb, GameTime gameTime);
        public void SetManager(ScreenManager<TData> mgr);
    }

    public abstract class Screen<TData>:IScreen<TData>
    {
        protected ScreenManager<TData> Manager;

        protected bool IsTopScreen => Manager.TopKey == Key;

        public abstract string Key { get; }
        public virtual bool IsAffectedByCamera => false;
        public bool IsInitialized { get; private set; }

        public virtual void Initialize(TData gameData, GameServiceContainer services)
        {
            IsInitialized = true;
        }

        public virtual void TearDown(TData gameData, GameServiceContainer services) {}
        public virtual void Update(TData gameData, GameTime gameTime) {}
        public abstract void Draw(TData gameData, SpriteBatch sb, GameTime gameTime);

        public void SetManager(ScreenManager<TData> mgr)
        {
            Manager = mgr;
        }
    }
}

using GameUtils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RealmSharp.GameObjects;
using RealmSharp.Screens;

namespace RealmSharp
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _sbCamera;
        private SpriteBatch _sbNoCamera;
        private MRData _data;
        private ScreenManager<MRData> _screenMgr;
        private FontManager _fontMgr;
        private KeyboardManager _keyboard;
        private readonly MouseManager _mouse;
        private Camera _camera;
        private TextureManager _tex;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _data = new MRData();
            _screenMgr = new ScreenManager<MRData>(this, _data);
            _fontMgr = new FontManager();
            _keyboard = new KeyboardManager();
            _mouse = new MouseManager();
            _tex = new TextureManager();

            Roller.Create();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = _graphics.GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = _graphics.GraphicsDevice.DisplayMode.Height;
            _graphics.ApplyChanges();

            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, Vector2.Zero);

            _mouse.SetCamera(_camera);

            Services.AddService(typeof(ScreenManager<MRData>), _screenMgr);
            Services.AddService(typeof(FontManager), _fontMgr);
            Services.AddService(typeof(KeyboardManager), _keyboard);
            Services.AddService(typeof(MouseManager), _mouse);
            Services.AddService(typeof(Camera), _camera);
            Services.AddService(typeof(TextureManager), _tex);

            SetupFakeMap();

            //_screenMgr.SetScreens(new MapView(), new TilePlacer());
            _screenMgr.SetScreens(new MapView());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _sbCamera = new SpriteBatch(GraphicsDevice);
            _sbNoCamera = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _fontMgr.AddFont("Arial", Content.Load<SpriteFont>("Arial12"));

            LoadTileTextures();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _keyboard.Update(gameTime);
            _mouse.Update(gameTime);
            _screenMgr.Update(_data, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //I think anything that is not affected by the camera is prob going to be
            //UI stuff and should go on top
            _sbCamera.Begin(transformMatrix: _camera.TranslationMatrix);
            _screenMgr.Draw(_data, _sbCamera, gameTime, GameUtils.Screens.WithCamera );
            _sbCamera.End();

            _sbNoCamera.Begin();
            _screenMgr.Draw(_data, _sbNoCamera, gameTime);
            _sbNoCamera.End();

            base.Draw(gameTime);
        }

        private void LoadTileTextures()
        {
            TileDefs.TileNames.ForEach(
                name => _tex.Add($"{name}-g", Content.Load<Texture2D>($"tiles/{name}-g")));

            TileDefs.TileNames.ForEach(
                name => _tex.Add($"{name}-e", Content.Load<Texture2D>($"tiles/{name}-e")));

            _tex.Add("cursor", Content.Load<Texture2D>("tiles/cursor"));
            _tex.Add("skull", Content.Load<Texture2D>("misc/rageskull"));
        }

        private void SetupFakeMap()
        {
            //_data.Map.Initialize(TileDefs.AllGreen);
            //_data.Map.PlaceHex("BL");

            _data.Map = MapMaker.CreateRandomMap(-3);
        }
    }
}

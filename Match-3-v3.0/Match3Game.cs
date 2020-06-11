using Match_3_v3._0.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SceneSystem;

namespace Match_3_v3._0
{
    public class Match3Game : Game
    {
        GraphicsDeviceManager _deviceManager;

        public Match3Game()
        {
            IsFixedTimeStep = true;
            IsMouseVisible = true;
            _deviceManager = new GraphicsDeviceManager(this);
            _deviceManager.GraphicsProfile = GraphicsProfile.HiDef;
            _deviceManager.IsFullScreen = false;
            _deviceManager.PreferredBackBufferWidth = 708;
            _deviceManager.PreferredBackBufferHeight = 750;
            _deviceManager.ApplyChanges();
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            PlayerPrefs.Set("CellSize", 81);
            PlayerPrefs.Set("Speed", 300);
            PlayerPrefs.Set("Width", 8);
            PlayerPrefs.Set("Height", 8);
            SceneManager.Instance.Initialize(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SceneManager.Instance.LoadScene<MainMenuScene>();
            SceneManager.Instance.SetActiveScene<MainMenuScene>();
        }

        protected override void Update(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SceneManager.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override void Dispose(bool disposing)
        {
            SceneManager.Instance.Clear();
            _deviceManager.Dispose();
            base.Dispose(disposing);
        }
    }
}

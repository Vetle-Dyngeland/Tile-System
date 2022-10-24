using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileSystem2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        public Game1()
        {
            graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }

    public static class ScreenManager
    {
        private static GraphicsDeviceManager graphics;

        public static Vector2Int fullScreenSize, prefferedScreenSize = new(1600, 900);
        public static bool isFullScreen = false;

        public static Vector2Int ScreenSize {
            get => isFullScreen ? fullScreenSize : prefferedScreenSize;
            set => prefferedScreenSize = isFullScreen ? prefferedScreenSize : value;
        }

        public static void Setup(GraphicsDeviceManager graphics_)
        {
            graphics = graphics_;

            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            fullScreenSize = new(w, h);

            UpdateScreen();
        }

        public static void UpdateScreen()
        {
            graphics.PreferredBackBufferWidth = ScreenSize.X;
            graphics.PreferredBackBufferHeight = ScreenSize.Y;
            graphics.IsFullScreen = isFullScreen;
            graphics.ApplyChanges();
        }
    }
}
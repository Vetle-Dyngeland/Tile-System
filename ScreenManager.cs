using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileSystem2
{
    public static class ScreenManager
    {
        private static GraphicsDeviceManager graphics;

        public static Vector2Int FullScreenSize { get; private set; }
        public static Vector2Int PrefferedScreenSize { get; private set; } = new (1600, 900);
        public static bool IsFullScreen { get; private set; }

        public static Vector2Int ScreenSize {
            get => IsFullScreen ? FullScreenSize : PrefferedScreenSize;
            set => PrefferedScreenSize = IsFullScreen ? PrefferedScreenSize : value;
        }

        public static void Setup(GraphicsDeviceManager _graphics)
        {
            graphics = _graphics;

            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            FullScreenSize = new(w, h);

            UpdateScreen();
        }

        public static void UpdateScreen()
        {
            graphics.PreferredBackBufferWidth = ScreenSize.X;
            graphics.PreferredBackBufferHeight = ScreenSize.Y;
            graphics.IsFullScreen = IsFullScreen;
            graphics.ApplyChanges();
        }

        public static void SetFullScreen(bool? value = null, bool updateScreen = true)
        {
            IsFullScreen = value == null ? !IsFullScreen : value.Value;
            if(updateScreen) UpdateScreen();
        }
    }
}
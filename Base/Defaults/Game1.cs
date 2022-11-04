using Apos.Input;
using Microsoft.Xna.Framework;
using TileSystem2.Managers;
using TileSystem2.Managers.Statics;

namespace TileSystem2.Base.Defaults
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private ManagerManager managerManager;

        public Game1()
        {
            graphics = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ScreenManager.Setup(graphics);
            DebugManager.ResetWindow();
            managerManager = new(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            InputHelper.Setup(this);
            ContentLoader.LoadContent(Content);
            managerManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputHelper.UpdateSetup();
            DebugManager.UpdateGameTime(gameTime);
            managerManager.Update(gameTime);
            base.Update(gameTime);
            
            InputHelper.UpdateCleanup();
        }

        protected override void Draw(GameTime gameTime)
        {
            DebugManager.UpdateGameTime(gameTime);
            DrawManager.Draw();
            base.Draw(gameTime);
        }
    }
}
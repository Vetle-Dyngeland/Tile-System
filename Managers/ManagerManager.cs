using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using TileSystem2.Managers.Level;
using TileSystem2.Managers.Statics;

namespace TileSystem2.Managers
{
    public class ManagerManager
    {
        public readonly CameraManager cameraManager;
        public readonly BoardManager boardManager;
        public Game game;

        private readonly ICondition exitCondition = new KeyboardCondition(Keys.Escape);

        public ManagerManager(Game game)
        {
            this.game = game;
            cameraManager = new(game.GraphicsDevice, game.Window);
            DrawManager.Setup(game.GraphicsDevice, cameraManager.camera);
            boardManager = new();
        }

        public void LoadContent()
        {
            boardManager.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            if(exitCondition.Held()) game.Exit();

            cameraManager.Update(gameTime);
            boardManager.Update();
        }
    }
}
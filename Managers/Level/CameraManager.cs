using Apos.Camera;
using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TileSystem2.Base.Sprites;
using TileSystem2.Helpers;

namespace TileSystem2.Managers.Level
{
    public class CameraManager
    {
        public Camera camera;

        public bool useDebugMovement = true;
        private const float debugMoveSpeed = 300;

        private static readonly ICondition[] moveConditions = new KeyboardCondition[4] {
            new(Keys.W), new(Keys.S), new(Keys.A), new(Keys.D)
        };

        public CollisionSprite followSprite;
        public List<Vector2> oldPositions = new();
        public int followSmoothing = 50;
        public float lookaheadTime = .125f;

        public CameraManager(GraphicsDevice graphicsDevice, GameWindow window)
            => camera = new(new DefaultViewport(graphicsDevice, window));

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (useDebugMovement) DebugMovement(deltaTime);
            else NormalMovement(deltaTime);
        }

        private void DebugMovement(float deltaTime)
        {
            Vector2 moveVector = new(Convert.ToInt16(moveConditions[2].Held()) - Convert.ToInt16(moveConditions[3].Held()),
                Convert.ToInt16(moveConditions[0].Held()) - Convert.ToInt16(moveConditions[1].Held()));

            camera.XY += -moveVector.Normalized() * debugMoveSpeed * deltaTime;
        }

        private void NormalMovement(float deltaTime)
        {
            oldPositions.Add(followSprite.position + lookaheadTime * followSprite.velocity * deltaTime);
            camera.XY += oldPositions.ToArray().Average();
        }
    }
}
using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TileSystem2.Base.Sprites;

namespace TileSystem2.Managers.Statics
{
    public static class DrawManager
    {
        public static Color BgColor { get; set; } = Color.CornflowerBlue;
        public static List<List<Sprite>> Sprites { get; private set; } = new();
        public static List<List<Sprite>> BgSprites { get; private set; } = new();
        public static List<List<Sprite>> PermaSprites { get; private set; } = new();
        public static List<List<Sprite>> PermaBgSprites { get; private set; } = new();

        internal static SpriteBatch spriteBatch;
        private static GraphicsDevice graphicsDevice;
        private static Camera camera;

        public static void Setup(GraphicsDevice graphicsDevice_, Camera camera_)
        {
            spriteBatch = new(graphicsDevice = graphicsDevice_);
            camera = camera_;
        }


        public static void Draw()
        {
            graphicsDevice.Clear(BgColor);
            DrawBackground();
            DrawForeground();
            ReAddSprites();
        }

        private static void BeginSpriteBatch(Matrix? transformMatrix = null)
            => spriteBatch.Begin(transformMatrix: transformMatrix, rasterizerState: RasterizerState.CullNone, samplerState: SamplerState.PointClamp);

        private static void DrawBackground()
        {
            BeginSpriteBatch(camera.GetView(-1));
            foreach(var layer in BgSprites)
                foreach(var sprite in layer)
                    sprite.Draw(spriteBatch);
            spriteBatch.End();
        }

        private static void DrawForeground()
        {
            BeginSpriteBatch(camera.View);
            foreach(var layer in Sprites)
                foreach(var sprite in layer)
                    sprite.Draw(spriteBatch);
            spriteBatch.End();
        }

        private static void ReAddSprites()
        {
            BgSprites.Clear();
            Sprites.Clear();

            foreach(var layer in PermaBgSprites) {
                BgSprites.Add(new());
                foreach(var sprite in layer)
                    BgSprites[^1].Add(sprite);
            }
            foreach(var layer in PermaSprites) {
                Sprites.Add(new());
                foreach(var sprite in layer)
                    Sprites[^1].Add(sprite);
            }
        }

        public static void AddSpriteAtLayer(Sprite sprite, int layer)
        {
            while(Sprites.Count <= layer) Sprites.Add(new());
            Sprites[layer].Add(sprite);
        }

        public static void AddBgSpriteAtLayer(Sprite sprite, int layer)
        {
            while(BgSprites.Count <= layer) BgSprites.Add(new());
            BgSprites[layer].Add(sprite);
        }

        public static void AddPermaSpriteAtLayer(Sprite sprite, int layer)
        {
            while(PermaSprites.Count <= layer) PermaSprites.Add(new());
            PermaSprites[layer].Add(sprite);
        }

        public static void AddPermaBgSpriteAtLayer(Sprite sprite, int layer)
        {
            while(PermaBgSprites.Count <= layer) PermaBgSprites.Add(new());
            PermaBgSprites[layer].Add(sprite);
        }
    }
}
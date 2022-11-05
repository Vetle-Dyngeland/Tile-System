using Apos.Camera;
using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TileSystem2.Base.Sprites;
using TileSystem2.Managers.Level;
using TileSystem2.Managers.Statics;

namespace TileSystem2.Base.Level
{
    public class BoardEditor
    {
        //Back-end 
        private List<List<string>> level;
        public bool editorMode = true;
        private readonly Camera camera;

        //Mouse overlay sprite
        private Sprite overlaySprite;
        private const float maxSpriteTransparency = .75f, minSpriteTransparency = .3f;
        private float spriteTransparency = .5f, spriteTransparencyAdd = .25f;

        //Input
        private readonly MouseCondition createTileKey = new(MouseButton.LeftButton);
        private readonly AllCondition eraseTileKey =
            new(new MouseCondition(MouseButton.LeftButton), new KeyboardCondition(Keys.LeftShift));

        public BoardEditor(Camera camera, List<List<string>> level)
        {
            this.camera = camera;
            this.level = level;
        }

        public void LoadContent()
        {
            overlaySprite = new(ContentLoader.Textures[1][Tile.TypeTextureIndexDict["Grass"]],
                sourceRect: Tile.TypeSourceRectDict["Grass0"], size: Vector2.One * Tile.TileSize, color: Color.White);
            DrawManager.AddPermaSpriteAtLayer(overlaySprite, 2);
        }

        public void Update(GameTime gameTime)
        {
            UpdateOverlaySprite(gameTime);
        }

        private void UpdateOverlaySprite(GameTime gameTime)
        {
            overlaySprite.position = BoardManager.mouseTilePosition.ToVector2();
            spriteTransparency += spriteTransparencyAdd * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(spriteTransparency > maxSpriteTransparency || spriteTransparency < minSpriteTransparency)
                spriteTransparencyAdd *= -1;
            overlaySprite.color = Color.White * spriteTransparency;
        }
    }
}
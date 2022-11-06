using Apos.Camera;
using Apos.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TileSystem2.Base.Sprites;
using TileSystem2.Managers.Level;
using TileSystem2.Managers.Statics;
using TileSystem2.Helpers;
using System.Threading;
using TileSystem2.Base.Structs;
using System;

namespace TileSystem2.Base.Level
{
    public class BoardEditor
    {
        //Back-end 
        public List<List<string>> level;
        public bool editorMode = true;

        private string selectedType = "Grass";
        private int selectedIndex;
        private string previousSelectedType = "Grass";
        private int previousSelectedIndex = 0;

        //Mouse overlay sprite
        private Sprite overlaySprite;
        private const float maxSpriteTransparency = .75f, minSpriteTransparency = .3f;
        private float spriteTransparency = .5f, spriteTransparencyAdd = .25f;

        //Input
        private readonly MouseCondition createTileKey = new(MouseButton.LeftButton);
        private readonly AllCondition eraseTileKey =
            new(new MouseCondition(MouseButton.LeftButton), new KeyboardCondition(Keys.LeftShift));

        private readonly AllCondition changeSelectedType = new(new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.T));

        public BoardEditor(List<List<string>> level) => this.level = level;

        public void Update(GameTime gameTime)
        {
            if(Tile.TypeTextureIndexDict != null) {
                UpdateOverlaySprite(gameTime);
                UpdateSelected();
            }
            EditBoard();
            RemoveEmptyLines();
        }

        private void UpdateOverlaySprite(GameTime gameTime)
        {
            if(overlaySprite == null) {
                overlaySprite = new(null, size: Vector2.One * Tile.TileSize);
                DrawManager.AddPermaSpriteAtLayer(overlaySprite, 2);
            }

            overlaySprite.shouldDraw = editorMode;
            if(!editorMode) return;

            overlaySprite.position = BoardManager.MouseTilePosition.ToVector2() * Tile.TileSize;
            spriteTransparency += spriteTransparencyAdd * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(spriteTransparency > maxSpriteTransparency || spriteTransparency < minSpriteTransparency)
                spriteTransparencyAdd *= -1;

            overlaySprite.color = Color.White * spriteTransparency;
            overlaySprite.texture = ContentLoader.Textures[1][Tile.TypeTextureIndexDict[selectedType]];
            overlaySprite.sourceRect = Tile.TypeSourceRectDict[selectedType + selectedIndex];
        }

        private void UpdateSelected()
        {
            int oldScroll = InputHelper.OldMouse.ScrollWheelValue;
            int newScroll = InputHelper.NewMouse.ScrollWheelValue;

            selectedIndex += (newScroll - oldScroll).GetValue();

            if(selectedIndex < 0) selectedIndex = Tile.TypeMaxIndexDict[selectedType];
            if(selectedIndex > Tile.TypeMaxIndexDict[selectedType]) selectedIndex = 0;
        }

        private void EditBoard()
        {
            Vector2Int mouseTilePos = BoardManager.MouseTilePosition;
            if(mouseTilePos.X < 0 || mouseTilePos.Y < 0) return; //Debug

            if(eraseTileKey.Held()) ErasePrep();
            else DrawPrep();

            if(!createTileKey.Held() && !eraseTileKey.Held()) return;
            if(mouseTilePos.X >= level.Count || mouseTilePos.Y >= (level.Count > 0 ? level[0].Count : 0))
                ExtendLevel(mouseTilePos);

            if(level[mouseTilePos.X][mouseTilePos.Y] != selectedType + selectedIndex)
                level[mouseTilePos.X][mouseTilePos.Y] = selectedType + selectedIndex;
        }

        private void ErasePrep()
        {
            if(selectedType == "Air") return; //If aleready erasing

            previousSelectedType = selectedType;
            previousSelectedIndex = selectedIndex;
            selectedType = "Air";
            selectedIndex = 0;
        }

        private void DrawPrep()
        {
            if(selectedType != "Air") return; //If aleready drawing

            selectedType = previousSelectedType;
            selectedIndex = previousSelectedIndex;
        }

        private void ExtendLevel(Vector2Int maxPoint)
        {
            while(level.Count <= maxPoint.X) {
                level.Add(new());
                for(int i = 0; i < (level.Count > 1 ? level[0].Count : 0); i++)
                    level[^1].Add("Air0");
            }
                
            while(level[0].Count <= maxPoint.Y)
                foreach(var layer in level)
                    layer.Add("Air0");
        }

        private void RemoveEmptyLines()
        {
            //X
            for(int x = level.Count - 1; x >= 0; x--) {
                int count = 0;
                for(int y = 0; y < (level.Count > 0 ? level[0].Count : 0); y++)
                    if(level[x][y] != "Air0") count++;
                if(count == 0) RemoveXLine(x);
                else break;
            }
            
            //Y
            for(int y = (level.Count > 0 ? level[0].Count : 0) - 1; y >= 0; y--) {
                int count = 0;
                for(int x = 0; x < level.Count; x++)
                    if(level[x][y] != "Air0") count++;
                if(count == 0) RemoveYLine(y);
                else break;
            }
        }

        private void RemoveXLine(int index)
            => level.RemoveAt(index);

        private void RemoveYLine(int index)
        {
            foreach(var list in level)
                list.RemoveAt(index);
        }
    }
}
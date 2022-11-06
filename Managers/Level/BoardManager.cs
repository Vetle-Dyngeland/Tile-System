using Apos.Camera;
using Apos.Input;
using IronXL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TileSystem2.Base.Level;
using TileSystem2.Base.Sprites;
using TileSystem2.Base.Structs;
using TileSystem2.Helpers;
using TileSystem2.Managers.Statics;

namespace TileSystem2.Managers.Level
{
    public class BoardManager
    {
        //Back-end
        public List<List<string>> level;
        private List<List<string>> oldLevel;
        public readonly Board board = new();
        private int currentLevelIndex = 0;
        private readonly Camera camera;
        public static Vector2Int MouseTilePosition { get; private set; }
        private readonly BoardEditor boardEditor;

        public static string LevelFilesLocation { get; } = @"D:\Development\Projects\Visual Studio Projects\Monogame\TileSystem2\External storage files\Levels\";

        //Input
        private readonly ICondition loadNextLevelCondition = new AllCondition(
            new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.N));
        private readonly ICondition loadPreviousLevelCondition = new AllCondition(
            new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.P));
        private readonly AllCondition drawEmpty = new(new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.D));

        public BoardManager(Camera camera)
        {
            LoadLevel(currentLevelIndex);

            this.camera = camera;
            boardEditor = new(level);
        }

        public void LoadLevel(int index)
        {
            currentLevelIndex = index;
            DataTable csv = GetDataTable(index);
            level = new();
            for(int x = 0; x < csv.Columns.Count; x++) {
                level.Add(new());
                for(int y = 0; y < csv.Rows.Count; y++) 
                    level[x].Add(csv.Rows[y][x].ToString());
            }
        }

        private static DataTable GetDataTable(int index)
        {
            string fullFileLocation = $"{LevelFilesLocation}";
            if(fullFileLocation[^1] != '\\') fullFileLocation += '\\';
            fullFileLocation += $"Level{index}.csv";

            return ReadCSV(fullFileLocation);
        }

        private static DataTable ReadCSV(string fileLocation)
        {
            WorkBook wb = WorkBook.Load(fileLocation);
            return wb.DefaultWorkSheet.ToDataTable(false);
        }

        public void Update(GameTime gameTime)
        {
            int oldLevelIndex = currentLevelIndex;
            //Debug
            if(loadNextLevelCondition.Pressed()) LoadLevel(++currentLevelIndex);
            if(loadPreviousLevelCondition.Pressed()) LoadLevel(--currentLevelIndex);
            if(drawEmpty.Pressed()) Tile.SetDrawEmpty(true);

            if(oldLevelIndex == currentLevelIndex)
                EditBoard(gameTime);

            if(LevelChanged() || Tile.TypeTextureIndexDict == null)
                board.Generate(level.To2DArray());
            foreach(Sprite sprite in board.tiles)
                DrawManager.AddSpriteAtLayer(sprite, 1);
        }

        private void EditBoard(GameTime gameTime)
        {
            oldLevel = new();
            foreach(var layer in level) {
                oldLevel.Add(new());
                foreach(var tile in layer)
                    oldLevel[^1].Add(tile);
            }

            UpdateMouseTilePos();
            boardEditor.Update(gameTime);

            if(LevelChanged()) UpdateCSV();
        }

        private bool LevelChanged()
        {
            if(oldLevel.Count != level.Count || 
                (oldLevel.Count > 0 ? oldLevel[0].Count : 0) != (level.Count > 0 ? level[0].Count : 0)) return true;

            for(int i = 0; i < level.Count; i++)
                for(int j = 0; j < level[i].Count; j++)
                    if(oldLevel[i][j] != level[i][j])
                        return true;
            return false;
        }

        private void UpdateCSV()
        {
            string fullFileLocation = $"{LevelFilesLocation}";
            if(fullFileLocation[^1] != '\\') fullFileLocation += '\\';
            fullFileLocation += $"Level{currentLevelIndex}.csv";

            File.WriteAllText(fullFileLocation, GetLevelInCSVFormat());
        }

        private string GetLevelInCSVFormat()
        {
            string levelString = string.Empty;
            for(int y = 0; y < (level.Count > 0 ? level[0].Count : 0); y++) {
                for(int x = 0; x < level.Count; x++)
                    levelString += $"{level[x][y]},";
                levelString = $"{levelString.TrimEnd(',')}\n";
            }
            return levelString.TrimEnd('\n');
        }

        private void UpdateMouseTilePos()
            => MouseTilePosition = new(Vector2.Floor(camera.ScreenToWorld(InputHelper.NewMouse.Position.ToVector2()) / Tile.TileSize));
    }
}
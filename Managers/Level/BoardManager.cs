using Apos.Camera;
using Apos.Input;
using IronXL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Data;
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
        public readonly Board board = new();
        private int currentLevelIndex = 0;
        private readonly Camera camera;
        public static Vector2Int mouseTilePosition;
        private readonly BoardEditor boardEditor;

        public static string LevelFilesLocation { get; } = @"D:\Development\Projects\Visual Studio Projects\Monogame\TileSystem2\External storage files\Levels\";

        //Input
        private readonly ICondition loadNextLevelCondition = new AllCondition(
            new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.N));
        private readonly ICondition loadPreviousLevelCondition = new AllCondition(
            new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.P));

        public BoardManager(Camera camera)
        {
            this.camera = camera;
            boardEditor = new(camera, level);
        }

        public void LoadContent()
        {
            LoadLevel(currentLevelIndex);
            boardEditor.LoadContent();
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
            board.Generate(level.To2DArray());
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
            //Debug
            if(loadNextLevelCondition.Pressed())
                LoadLevel(++currentLevelIndex);
            if(loadPreviousLevelCondition.Pressed())
                LoadLevel(--currentLevelIndex);

            UpdateMouseTilePos();
            boardEditor.Update(gameTime);

            board.Generate(level.To2DArray());

            foreach(Sprite sprite in board.tiles)
                DrawManager.AddSpriteAtLayer(sprite, 1);
        }

        private void UpdateMouseTilePos()
        {
            mouseTilePosition = new(Vector2.Floor(camera.ScreenToWorld(InputHelper.NewMouse.Position.ToVector2()) / Tile.TileSize) * Tile.TileSize);
        }
    }
}
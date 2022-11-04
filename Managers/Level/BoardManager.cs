using Apos.Input;
using IronXL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Data;
using TileSystem2.Base.Sprites;
using TileSystem2.Base.Structs;
using TileSystem2.Helpers;
using TileSystem2.Managers.Statics;

namespace TileSystem2.Managers.Level
{
    public class BoardManager
    {
        public List<List<string>> level;
        public readonly Board board = new();
        private int currentLevelIndex = 0;

        public string LevelFilesLocation { get; } = @"D:\Development\Projects\Visual Studio Projects\Monogame\TileSystem2\External storage files\Levels\";

        private readonly ICondition loadNextLevelCondition = new AllCondition(
            new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.N));
        private readonly ICondition loadPreviousLevelCondition = new AllCondition(
            new KeyboardCondition(Keys.LeftControl), new KeyboardCondition(Keys.P));

        public void LoadContent()
        {
            LoadLevel(currentLevelIndex);
        }

        public void LoadLevel(int index)
        {
            DataTable csv = GetDataTable(index);
            level = new();
            for(int x = 0; x < csv.Columns.Count; x++) {
                level.Add(new());
                for(int y = 0; y < csv.Rows.Count; y++) {
                    level[x].Add(csv.Rows[y][x].ToString());
                }
            }
        }

        private DataTable GetDataTable(int index)
        {
            currentLevelIndex = index;
            string fullFileLocation = $"{LevelFilesLocation}";
            if(fullFileLocation[^1] != '\\') fullFileLocation += '\\';
            fullFileLocation += $"Level{index}.csv";

            return ReadCSV(fullFileLocation);
        }

        private DataTable ReadCSV(string fileLocation)
        {
            WorkBook wb = WorkBook.Load(fileLocation);
            return wb.DefaultWorkSheet.ToDataTable(false);
        }

        public void Update()
        {
            //Debug
            if(loadNextLevelCondition.Pressed())
                LoadLevel(++currentLevelIndex);
            if(loadPreviousLevelCondition.Pressed())
                LoadLevel(--currentLevelIndex);

            board.Generate(level.To2DArray());

            foreach(Sprite sprite in board.tiles)
                DrawManager.AddSpriteAtLayer(sprite, 1);
        }
    }

    public class Board
    {
        public Tile[,] tiles;
        public int width, height;

        public void Generate(string[,] level)
        {
            width = level.GetLength(0);
            height = level.GetLength(1);
            tiles = new Tile[width, height];

            for(int x = 0; x < width; x++)
                for(int y = 0; y < height; y++) {
                    GetTileInfo(level, new(x, y), out string type, out int index);
                    tiles[x, y] = new(type, index, new Vector2(x, y) * Tile.TileSize);
                }
        }

        private static void GetTileInfo(string[,] level, Vector2Int pos, out string type, out int index)
        {
            int checkIndex = 0;

            while(checkIndex < level[pos.X, pos.Y].Length && !int.TryParse(level[pos.X, pos.Y][checkIndex].ToString(), out _)) 
                checkIndex++;

            type = level[pos.X, pos.Y].GetInt(out index, checkIndex, true);
        }
    }
}
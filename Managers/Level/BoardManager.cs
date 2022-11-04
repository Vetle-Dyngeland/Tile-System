using Microsoft.Xna.Framework;
using System.Collections.Generic;
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

        public BoardManager()
        {
            
        }

        public void LoadContent()
        {
            //Load level from json file
            level = new() {
                new() { "Grass0", "Grass1", "Grass1", "Grass1", "Grass2" },
                new() { "Grass3", "Grass4", "Grass4", "Grass4", "Grass5" },
                new() { "Grass3", "Grass4", "Grass4", "Grass4", "Grass5" },
                new() { "Grass3", "Grass4", "Grass4", "Grass4", "Grass5" },
                new() { "Grass6", "Grass7", "Grass7", "Grass7", "Grass8" },
            };
        }

        public void Update()
        {
            //if any information has changed, change the json
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
using Microsoft.Xna.Framework;
using TileSystem2.Base.Sprites;
using TileSystem2.Base.Structs;
using TileSystem2.Helpers;

namespace TileSystem2.Base.Level
{
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
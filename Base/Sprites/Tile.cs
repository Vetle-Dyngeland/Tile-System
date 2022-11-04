using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TileSystem2.Base.Structs;
using TileSystem2.Managers.Statics;

namespace TileSystem2.Base.Sprites
{
    public class Tile : CollisionSprite
    {
        private static Dictionary<string, int> typeTextureIndexDict;
        private static Dictionary<string, Rectangle> typeSourceRectDict;
        private static Dictionary<string, float> typeFrictionDict, typeBouncinessDict;

        public string type;
        public int index;

        public static int TileSize { get; set; } = 32;
        private const int sourceImageTileSize = 16;

        public Tile(string type, int index, Vector2 position = default) : base(null)
        {
            this.type = type;
            this.index = index;
            this.position = position;

            if(typeTextureIndexDict == null) InitializeDictionaries();

            CreateTile();
        }

        private static void InitializeDictionaries()
        {
            typeTextureIndexDict = new() {
                { "Grass", 0 }
            };

            typeFrictionDict = new() {
                { "Grass", 0.93f }
            };

            typeBouncinessDict = new() {
                { "Grass", 0.085f }
            };

            InitializeSourceRectDictionary();
        }

        //Hardcode rectangles into dictionary, revamp for automatic rectangles when creating new textures
        private static void InitializeSourceRectDictionary() 
        {
            typeSourceRectDict = new();

            //Grass
            int grassIndex = 0;
            grassIndex = AddRectangleToSourceRectDict("Grass", grassIndex, new(0, 0), new(2, 2));
            grassIndex = AddRectangleToSourceRectDict("Grass", grassIndex, new(3, 0), new(5, 2), true);
            AddRectangleToSourceRectDict("Grass", grassIndex, new(6, 3), new(8, 3));
        }

        private static int AddRectangleToSourceRectDict(string key, int index, Vector2Int p1, Vector2Int p2, bool hollow = false)
        {
            Vector2Int max = p2 + Vector2Int.One - p1;
            for(int x = 0; x < max.X; x++)
                for(int y = 0; y < max.Y; y++) {
                    if(hollow && x == (int)(max.X * .5f) && y == (int)(max.Y * .5f)) continue;
                    typeSourceRectDict.Add(key + index++, new((new Vector2Int(p1.X + x, p1.Y + y) * sourceImageTileSize).ToVector2().ToPoint(), new(sourceImageTileSize)));
                }
            return index;
        }

        private void CreateTile()
        {
            texture = ContentLoader.Textures[1][typeTextureIndexDict[type]];
            friction = typeFrictionDict[type];
            bounciness = typeBouncinessDict[type];
            string s = string.Empty;
            for(int i = 0; i < typeSourceRectDict.Count; i++) {
                s += $"Grass{i}";
            }

            sourceRect = typeSourceRectDict[type + index];

            size = Vector2.One * TileSize;
            hitbox = DrawRect;

            useGravity = false;
            isStatic = true;
        }
    }
}
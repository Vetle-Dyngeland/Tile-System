using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TileSystem2.Base.Structs;
using TileSystem2.Managers.Statics;

namespace TileSystem2.Base.Sprites
{
    public class Tile : CollisionSprite
    {
        public static Dictionary<string, int> TypeTextureIndexDict { get; private set; }
        public static Dictionary<string, Rectangle> TypeSourceRectDict { get; private set; }
        public static Dictionary<string, float> TypeFrictionDict { get; private set; }
        public static Dictionary<string, float> TypeBouncinessDict { get; private set; }
        public static Dictionary<string, int> TypeMaxIndexDict { get; private set; }

        public string type;
        public int index;

        public static int TileSize { get; set; } = 32;
        private const int sourceImageTileSize = 16;
        private static bool drawEmpty = false;

        public Tile(string type, int index, Vector2 position = default) : base(null)
        {
            this.type = type;
            this.index = index;
            this.position = position;

            if(TypeTextureIndexDict == null) InitializeDictionaries();

            CreateTile();
        }

        private static void InitializeDictionaries()
        {
            TypeTextureIndexDict = new() {
                { "Grass", 0 },
                { "Air", 1 }
            };

            TypeFrictionDict = new() {
                { "Grass", 0.93f }
            };

            TypeBouncinessDict = new() {
                { "Grass", 0.085f }
            };

            InitializeSourceRectDictionary();
        }

        //Hardcode rectangles into dictionary, revamp for automatic rectangles when recreating textures
        private static void InitializeSourceRectDictionary() 
        {
            TypeMaxIndexDict = new();
            TypeSourceRectDict = new();

            //Air
            int index = 0;
            index = AddRectangleToSourceRectDict("Air", index, new(0, 0), new(0, 0));
            TypeMaxIndexDict.Add("Air", index - 1);

            //Grass
            index = 0;
            index = AddRectangleToSourceRectDict("Grass", index, new(0, 0), new(2, 2));
            index = AddRectangleToSourceRectDict("Grass", index, new(3, 0), new(4, 1));
            index = AddRectangleToSourceRectDict("Grass", index, new(3, 2), new(5, 2));
            TypeMaxIndexDict.Add("Grass", index - 1);

        }

        private static int AddRectangleToSourceRectDict(string key, int index, Vector2Int p1, Vector2Int p2, bool hollow = false)
        {
            Vector2Int max = p2 + Vector2Int.One - p1;
            for(int x = 0; x < max.X; x++)
                for(int y = 0; y < max.Y; y++) {
                    if(hollow && x == (int)(max.X * .5f) && y == (int)(max.Y * .5f)) continue;
                    TypeSourceRectDict.Add(key + index++, new((new Vector2Int(p1.X + x, p1.Y + y) * sourceImageTileSize).ToVector2().ToPoint(), new(sourceImageTileSize)));
                }
            return index;
        }

        private void CreateTile()
        {
            if(type == "Air") {
                useGravity = false;
                isStatic = true;
                shouldCollide = false;
                shouldDraw = drawEmpty;

                sourceRect = TypeSourceRectDict[type + index];
                texture = ContentLoader.Textures[1][TypeTextureIndexDict[type]];
                size = Vector2.One * TileSize;
                hitbox = DrawRect;
                return;
            }
            texture = ContentLoader.Textures[1][TypeTextureIndexDict[type]];
            friction = TypeFrictionDict[type];
            bounciness = TypeBouncinessDict[type];

            sourceRect = TypeSourceRectDict[type + index];

            size = Vector2.One * TileSize;
            hitbox = DrawRect;

            useGravity = false;
            isStatic = true;
        }

        public static void SetDrawEmpty(bool value = true)
        {
            drawEmpty = value;
        }
    }
}
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TileSystem2.Managers.Statics
{
    public static class ContentLoader
    {
        private static ContentManager content;

        private static Dictionary<string, int> textureListFolderDict;

        public static bool ContentLoaded { get; internal set; }
        public static List<List<Texture2D>> Textures { get; private set; }
        public static List<List<SpriteFont>> Fonts { get; private set; }

        public static void LoadContent(ContentManager Content)
        {
            content = Content;
            InitializeDictionaries();

            Textures = new();

            LoadTexture("whitePixel", "Test");
            LoadTexture("circle", "Test");
            LoadTexture("tileset", "Tiles");
            LoadTexture("Tile0", "Tiles");

            ContentLoaded = true;
        }

        private static void InitializeDictionaries()
        {
            textureListFolderDict = new() {
                { "Test", 0 },
                { "test", 0 },
                { "Tiles", 1 },
                { "tiles", 1 },
            };
        }

        public static void LoadTexture(string textureName, string folderName, string[] subFolderNames = null)
        {
            string name = $"Sprites/{folderName}/";
            for (int i = 0; i < (subFolderNames != null ? subFolderNames.Length : 0); i++)
                name += $"{subFolderNames[i]}/";

            while (Textures.Count <= textureListFolderDict[folderName]) Textures.Add(new());
            Textures[textureListFolderDict[folderName]].Add(content.Load<Texture2D>($"{name}{textureName}"));
        }
    }
}
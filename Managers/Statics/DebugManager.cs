using Microsoft.Xna.Framework;
using System.IO;
using TileSystem2.Helpers;

namespace TileSystem2.Managers.Statics
{
    public static class DebugManager
    {
        public static string FileLocation { get; private set; } = @"D:\Development\Projects\Visual Studio Projects\Monogame\TileSystem2\External storage files\Debug\OutputLog.txt";
        private static GameTime gameTime;

        public static void ResetWindow()
            => File.WriteAllText(FileLocation, string.Empty);

        public static void Log(string message)
        {
            using StreamWriter file = new(FileLocation, append: true);
            if(File.ReadAllText(FileLocation).Length == 0)
                file.WriteLine($"\"{message}\", ({GetGameTimeString()})");
            else file.WriteLine($"\n\"{message}\", ({GetGameTimeString()})");
        }

        private static string GetGameTimeString()
        {
            if(gameTime == default || gameTime.TotalGameTime.TotalSeconds.ToString() == "0") return ", (00:00:00)";

            string gameTimeString = gameTime.TotalGameTime.ToString();
            string ret = string.Empty;
            for(int i = 0; i < gameTimeString.Length - 4; i++)
                ret += gameTimeString[i];
            while(ret[^1] == '0' && (ret[^2] != '.' && ret[^2] != ':'))
                ret = ret.RemoveChars(1, ret.Length - 1);
            return ret;
        }

        public static void Log(object message) => Log(message.ToString());

        public static void UpdateGameTime(GameTime gameTime_) => gameTime = gameTime_;  
    }
}
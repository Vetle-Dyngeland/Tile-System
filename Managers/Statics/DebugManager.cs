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
            string full = $"\"{message}\"";
            if(gameTime == default || gameTime.TotalGameTime.TotalSeconds.ToString() == "0") full += ", (00:00:00)";
            else {
                full += ", (";
                string gameTimeString = gameTime.TotalGameTime.ToString();
                for(int i = 0; i < gameTimeString.Length - 4; i++)
                    full += gameTimeString[i];
                while(full[^1] == '0' && (full[^2] != '.' && full[^2] != ':'))
                    full = full.RemoveChars(1, full.Length - 1);
                full += ")";
            }

            using StreamWriter file = new(FileLocation, append: true);
            file.WriteLine(full);
        }

        public static void Log(object message) => Log(message.ToString());

        public static void UpdateGameTime(GameTime gameTime_) => gameTime = gameTime_;
    }
}
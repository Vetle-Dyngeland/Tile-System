using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace TileSystem2.Helpers
{
    public static class VectorHelper
    {
        public static Vector2 Normalized(this Vector2 v)
            => new(v.X == 0 ? 0 : v.X / v.Length(), v.Y == 0 ? 0 : v.Y / v.Length());

        public static Vector2 Average(this Vector2[] array)
        {
            Vector2 sum = default;
            foreach(var item in array)
                sum += item;
            return sum / array.Length;
        }
    }
}
using System;

namespace TileSystem2.Helpers
{
    public static class NumberHelper
    {
        public static int GetValue(this float f)
            => f == 0 ? 0 : (int)(MathF.Abs(f) / f);
        public static int GetValue(this int i) => GetValue((float)i);
        public static int GetValue(this short s) => GetValue((float)s);
        public static int GetValue(this long l) => GetValue((float)l);
        public static int GetValue(this double d) => GetValue((float)d);

        public static float Average(this float[] arr)
        {
            float sum = 0;
            foreach(var item in arr)
                sum += item;
            return sum / arr.Length;
        }

        public static int Average(this int[] arr)
        {
            int sum = 0;
            foreach(var item in arr)
                sum += item;
            return sum / arr.Length;
        }
    }
}

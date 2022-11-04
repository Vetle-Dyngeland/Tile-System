using System;
using System.Collections.Generic;
using System.Linq;

namespace TileSystem2.Helpers
{
    public static class ArrayHelper
    {
        #region Conversion
        public static T[][] ToJaggedArray<T>(this List<List<T>> list)
        {
            List<T>[] listArray = list.ToArray();
            T[][] jagged = new T[listArray.Length][];
            for(int i = 0; i < listArray.Length; i++)
                jagged[i] = listArray[i].ToArray();
            return jagged;
        }

        public static T[][] ToJaggedArray<T>(this T[,] multiDim) 
        {
            T[][] jagged = new T[multiDim.GetLength(0)][];
            for(int i = 0; i < multiDim.GetLength(0); i++) {
                jagged[i] = new T[multiDim.GetLength(1)];
                for(int j = 0; j < multiDim.GetLength(1); j++)
                    jagged[i][j] = multiDim[i, j];
            }
            return jagged;
        }

        public static T[,] To2DArray<T>(this List<List<T>> list)
            => list.ToJaggedArray().To2D();

        public static T[,] To2D<T>(this T[][] jagged)
        {
            try {
                int d1 = jagged.Length;
                int d2 = jagged.GroupBy(row => row.Length).Single().Key;

                T[,] multiDim = new T[d1, d2];
                for(int i = 0; i < d1; ++i)
                    for(int j = 0; j < d2; ++j)
                        multiDim[i, j] = jagged[i][j];

                return multiDim;
            }
            catch(InvalidOperationException) {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }
        #endregion Conversion
    }
}
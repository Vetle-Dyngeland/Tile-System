namespace TileSystem2.Helpers
{
    public static class StringHelper
    {
        public static string GetInt(this string str, out int result, int startIndex = 0, bool shouldRemove = false)
        {
            int checkIndex = startIndex;
            string resultString = string.Empty;
            while(checkIndex < str.Length && int.TryParse(str[checkIndex].ToString(), out int res)){
                resultString += res.ToString();

                if(shouldRemove) str = RemoveChars(str, startIndex: startIndex);
                else checkIndex++;
            }
            result = int.Parse(resultString);
            return str;
        }

        public static string RemoveChars(this string str, int amount = 1, int startIndex = 0)
        {
            string ret = string.Empty;
            for(int i = 0; i < startIndex; i++) ret += str[i];
            for(int i = startIndex + amount; i < str.Length; i++) ret += str[i];
            return ret;

        }
    }
}
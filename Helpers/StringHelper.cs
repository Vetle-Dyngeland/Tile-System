namespace TileSystem2.Helpers
{
    public static class StringHelper
    {
        public static string GetInt(this string s, out int result, int startIndex = 0, bool shouldRemove = false)
        {
            int checkIndex = startIndex;
            string resultString = string.Empty;
            while(checkIndex < s.Length && int.TryParse(s[checkIndex].ToString(), out int res)){
                resultString += res.ToString();

                if(shouldRemove) s = RemoveChars(s, startIndex: startIndex);
                else checkIndex++;
            }
            result = int.Parse(resultString);
            return s;
        }

        public static string RemoveChars(this string s, int amount = 1, int startIndex = 0)
        {
            string ret = string.Empty;
            for(int i = 0; i < startIndex; i++) ret += s[i];
            for(int i = startIndex + amount; i < s.Length; i++) ret += s[i];
            return ret;

        }
    }
}
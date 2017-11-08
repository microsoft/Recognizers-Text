namespace Microsoft.Recognizers.Text.Utilities
{
    public static class FormatUtility
    {
        public static string Preprocess(string query, bool toLower = true)
        {
            if (toLower)
            {
                query = query.ToLowerInvariant();
            }
            query = query.Replace("０", "0");
            query = query.Replace("１", "1");
            query = query.Replace("２", "2");
            query = query.Replace("３", "3");
            query = query.Replace("４", "4");
            query = query.Replace("５", "5");
            query = query.Replace("６", "6");
            query = query.Replace("７", "7");
            query = query.Replace("８", "8");
            query = query.Replace("９", "9");
            query = query.Replace("：", ":");
            query = query.Replace("－", "-");
            query = query.Replace("，", ",");
            query = query.Replace("／", "/");
            query = query.Replace("Ｇ", "G");
            query = query.Replace("Ｍ", "M");
            query = query.Replace("Ｔ", "T");
            query = query.Replace("Ｋ", "K");
            query = query.Replace("ｋ", "k");
            query = query.Replace("．", ".");
            query = query.Replace("（", "(");
            query = query.Replace("）", ")");

            return query;
        }
    }
}

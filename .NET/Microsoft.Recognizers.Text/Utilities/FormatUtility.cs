using System.Globalization;
using System.Linq;
using System.Text;

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

        public static string RemoveDiacritics(string query)
        {
            if (query == null)
            {
                return null;
            }

            // FormD indicates that a Unicode string is normalized using full canonical decomposition.
            var chars =
                from c in query.Normalize(NormalizationForm.FormD).ToCharArray()
                let uc = CharUnicodeInfo.GetUnicodeCategory(c)
                where uc != UnicodeCategory.NonSpacingMark
                select c;

            // FormC indicates that a Unicode string is normalized using full canonical decomposition, 
            // followed by the replacement of sequences with their primary composites, if possible.
            var normalizedQuery = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);

            return normalizedQuery;
        }
    }
}

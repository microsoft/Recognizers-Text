using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Utilities
{
    public static class QueryProcessor
    {
        private const string Expression = @"(?<=(\s|\d))(kB|K[Bb]|K|M[Bb]|M|G[Bb]|G|B)\b";

        private static readonly Regex SpecialTokensRegex = new Regex(Expression, RegexOptions.Compiled);

        public static string Preprocess(string query, bool caseSensitive = false, bool recode = true)
        {
            if (recode)
            {
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
                query = query.Replace("％", "%");
                query = query.Replace("、", ",");
            }

            query = caseSensitive ?
                ToLowerTermSensitive(query) :
                query.ToLowerInvariant();

            return query;
        }

        public static string ToLowerTermSensitive(string input)
        {
            var lowercase = input.ToLowerInvariant();
            var buffer = new StringBuilder(lowercase);

            var replaced = false;

            var matches = SpecialTokensRegex.Matches(input);
            foreach (Match m in matches)
            {
                ReApplyValue(m.Index, ref buffer, m.Value);
                replaced = true;
            }

            return replaced ? buffer.ToString() : lowercase;
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

        private static void ReApplyValue(int idx, ref StringBuilder outString, string value)
        {
            for (int i = 0; i < value.Length; ++i)
            {
                outString[idx + i] = value[i];
            }
        }
    }
}

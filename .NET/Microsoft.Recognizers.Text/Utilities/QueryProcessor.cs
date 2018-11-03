using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Recognizers.Text.Utilities
{
    public static class QueryProcessor
    {
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

        static List<string> list = new List<string> { "M", "MB", "Mb", "KB", "kB", "K", "G", "GB", "Gb", "B" };

        private static void ApplyReverse(int idx, char[] str, string value)
        {
            for (int i = 0; i < value.Length; ++i)
            {
                str[idx + i] = value[i];
            }
        }

        public static string ToLowerTermSensitive(string toConvert)
        {
            var res = toConvert.ToLowerInvariant().ToCharArray();

            foreach (var value in list)
            {
                
                for (int idx = 0; ; idx += value.Length)
                {
                    idx = toConvert.IndexOf(value, idx, StringComparison.Ordinal);
                    if (idx == -1)
                        break;
                    char leftChar = idx <= 0 ? ' ' : toConvert[idx - 1];
                    char rightChar = idx + value.Length >= toConvert.Length ? ' ' : toConvert[idx + value.Length];
                    if (leftChar == ' ' && rightChar == ' ' || char.IsNumber(leftChar))
                    {
                        ApplyReverse(idx, res, value);
                    }
                }
            }

            return new string(res);
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

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Utilities
{
    public class DictionaryUtils
    {
        public static void BindDictionary(IDictionary<string, string> dictionary,
            IDictionary<string, string> sourceDictionary)
        {
            if (dictionary == null) return;

            foreach (var pair in dictionary)
            {
                if (string.IsNullOrEmpty(pair.Key))
                {
                    continue;
                }

                BindUnitsString(sourceDictionary, pair.Key, pair.Value);
            }
        }

        public static void BindUnitsString(IDictionary<string, string> sourceDictionary, string key, string source)
        {
            var values = source.Trim().Split('|');

            foreach (var token in values)
            {
                if (string.IsNullOrWhiteSpace(token) || sourceDictionary.ContainsKey(token))
                {
                    continue;
                }

                sourceDictionary.Add(token, key);
            }
        }
    }
}
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Utilities
{
    public static class DictionaryUtils
    {
        // Safely bind dictionary which contains several key-value pairs to the destination dictionary.
        // This function is used to bind all the prefix and suffix for units.
        public static void BindDictionary(
            IDictionary<string, string> dictionary,
            IDictionary<string, string> sourceDictionary)
        {
            if (dictionary == null)
            {
                return;
            }

            foreach (var pair in dictionary)
            {
                if (string.IsNullOrEmpty(pair.Key))
                {
                    continue;
                }

                BindUnitsString(sourceDictionary, pair.Key, pair.Value);
            }
        }

        // Bind keys in a string which contains words separated by '|'.
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
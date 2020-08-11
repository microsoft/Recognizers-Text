using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public static class StringExtension
    {
        public static string Normalized(this string text, Dictionary<char, char> dic)
        {
            foreach (var keyPair in dic)
            {
                text = text.Replace(keyPair.Key, keyPair.Value);
            }

            return text;
        }
    }
}

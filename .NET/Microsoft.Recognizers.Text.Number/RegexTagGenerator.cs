using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Recognizers.Text.Number
{
    public class RegexTagGenerator
    {
        public static string GenerateRegexTag(string extractorType, string suffix)
        {
            return $"{extractorType}{suffix}";
        }
    }
}

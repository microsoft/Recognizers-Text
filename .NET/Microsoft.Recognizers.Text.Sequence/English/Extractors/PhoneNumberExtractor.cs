using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class PhoneNumberExtractor : BasePhoneNumberExtractor
    {
        public override List<ExtractResult> Extract(string text)
        {
            var result = base.Extract(text);
            var maskMatchCollection = Regex.Matches(text, BasePhoneNumbers.PhoneNumberMaskRegex);

            for (var index = result.Count - 1; index >= 0; --index)
            {
                foreach (Match m in maskMatchCollection)
                {
                    if (result[index].Start >= m.Index &&
                        result[index].Start + result[index].Length <= m.Index + m.Length)
                    {
                        result.RemoveAt(index);
                        break;
                    }
                }
            }

            return result;
        }
    }
}

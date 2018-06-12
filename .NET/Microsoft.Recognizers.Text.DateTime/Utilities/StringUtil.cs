using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    internal class StringUtil
    {
        public static string RemoveDiacritics(string str)
        {
            if (str == null)
            {
                return null;
            }

            // FormD indicates that a Unicode string is normalized using full canonical decomposition.
            var chars =
                from c in str.Normalize(NormalizationForm.FormD).ToCharArray()
                let uc = CharUnicodeInfo.GetUnicodeCategory(c)
                where uc != UnicodeCategory.NonSpacingMark
                select c;

            // FormC indicates that a Unicode string is normalized using full canonical decomposition, 
            // followed by the replacement of sequences with their primary composites, if possible.
            var cleanStr = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);

            return cleanStr;
        }
    }
}

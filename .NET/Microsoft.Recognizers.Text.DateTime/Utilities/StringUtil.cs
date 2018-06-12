using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    class StringUtil
    {
        public static string RemoveDiacritics(string str)
        {
            if (str == null) return null;
            var chars =
                from c in str.Normalize(NormalizationForm.FormD).ToCharArray()
                let uc = CharUnicodeInfo.GetUnicodeCategory(c)
                where uc != UnicodeCategory.NonSpacingMark
                select c;

            var cleanStr = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);

            return cleanStr;
        }
    }
}

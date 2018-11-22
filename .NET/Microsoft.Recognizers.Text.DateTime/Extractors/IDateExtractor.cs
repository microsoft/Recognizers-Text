// Enable GetYearFromText method

using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateExtractor : IDateTimeExtractor
    {
        int GetYearFromText(Match match);
    }
}

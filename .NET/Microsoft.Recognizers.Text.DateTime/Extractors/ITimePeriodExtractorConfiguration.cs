using Microsoft.Recognizers.Text.Number;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimePeriodExtractorConfiguration : IOptionsConfiguration
    {
        string TokenBeforeDate { get; }

        IExtractor IntegerExtractor { get; }

        IEnumerable<Regex> SimpleCasesRegex { get; }

        Regex TillRegex { get; }

        Regex TimeOfDayRegex { get; }

        Regex GeneralEndingRegex { get; }

        IDateTimeExtractor SingleTimeExtractor { get; }
        
        bool GetFromTokenIndex(string text, out int index);

        bool HasConnectorToken(string text);

        bool GetBetweenTokenIndex(string text, out int index);
    }
}
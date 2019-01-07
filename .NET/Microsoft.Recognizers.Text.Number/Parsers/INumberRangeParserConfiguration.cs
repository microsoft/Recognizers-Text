using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public interface INumberRangeParserConfiguration
    {
        CultureInfo CultureInfo { get; }

        IExtractor NumberExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IParser NumberParser { get; }

        Regex MoreOrEqual { get; }

        Regex LessOrEqual { get; }

        Regex MoreOrEqualSuffix { get; }

        Regex LessOrEqualSuffix { get; }

        Regex MoreOrEqualSeparate { get; }

        Regex LessOrEqualSeparate { get; }
    }
}

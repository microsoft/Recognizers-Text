using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public interface IIndianNumberParserConfiguration : INumberParserConfiguration
    {
        // Map used for decimal values that are Hindi specific such as डेढ़, सवा and ढाई etc which
        // loosely transalte to "one and a half" "one and a quarter" "two and a half".
        ImmutableDictionary<string, double> DecimalUnitsMap { get; }

        ImmutableDictionary<char, long> ZeroToNineMap { get; }

        Regex AdditionTermsRegex { get;  }

        Regex FractionPrepositionInverseRegex { get; }

        // Used to parse regional Hindi cases like डेढ/सवा/ढाई  which roughly translates to one and a half, one quarters, etc.
        // these are Indian Language specific cases and hold various meaning when prefixed with Number units.
        double ResolveUnitCompositeNumber(string numberStr);

    }
}
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public interface ITimeParserConfiguration
    {
        string TimeTokenPrefix { get; }

        Regex AtRegex { get; }
        IEnumerable<Regex> TimeRegexes { get; }

        IImmutableDictionary<string, int> Numbers { get; }

        void AdjustByPrefix(string prefix, ref int hour, ref int min, ref bool hasMin);
        void AdjustBySuffix(string suffix, ref int hour, ref int min, ref bool hasMin, ref bool hasAm, ref bool hasPm);
    }
}

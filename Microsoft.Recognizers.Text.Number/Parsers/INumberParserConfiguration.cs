using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Parsers
{
    public interface INumberParserConfiguration
    {
        #region language dictionaries
        ImmutableDictionary<string, long> CardinalNumberMap { get; }

        ImmutableDictionary<string, long> OrdinalNumberMap { get; }

        ImmutableDictionary<string, long> RoundNumberMap { get; }
        #endregion

        #region language settings
        CultureInfo CultureInfo { get; }

        Regex DigitalNumberRegex { get; }

        string FractionMarkerToken { get; }

        Regex HalfADozenRegex { get; }

        string HalfADozenText { get; }

        string LangMarker { get; }

        char NonDecimalSeparatorChar { get; }

        char DecimalSeparatorChar { get; }

        string WordSeparatorToken { get; }

        IEnumerable<string> WrittenDecimalSeparatorTexts { get; }

        IEnumerable<string> WrittenGroupSeparatorTexts { get; }

        IEnumerable<string> WrittenIntegerSeparatorTexts { get; }

        IEnumerable<string> WrittenFractionSeparatorTexts { get; }
        #endregion

        /// <summary>
        /// Used when requiring to normalize a token to a valid expression supported by the ImmutableDictionaries (language dictionaries)
        /// </summary>
        /// <param name="tokens">list of tokens to normalize</param>
        /// <param name="context">context of the call</param>
        /// <returns>list of normalized tokens</returns>
        IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, object context);

        /// <summary>
        /// Used when requiring to convert a string to a valid number supported by the language
        /// </summary>
        /// <param name="numberStr">composite number</param>
        /// <returns>value of the string</returns>
        long ResolveCompositeNumber(string numberStr);
    }
}
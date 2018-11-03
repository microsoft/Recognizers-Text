using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class BaseNumberExtractor : IExtractor
    {
        internal abstract ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected virtual ImmutableDictionary<Regex, Regex> AmbiguityFiltersDict { get; } = null;

        protected virtual string ExtractType { get; } = "";

        protected virtual NumberOptions Options { get; } = NumberOptions.None;

        protected virtual Regex NegativeNumberTermsRegex { get; } = null;

        protected virtual Regex AmbiguousFractionConnectorsRegex { get; } = null;

        public static readonly Regex CurrencyRegex = 
            new Regex(BaseNumbers.CurrencyRegex, RegexOptions.Singleline);

        public virtual List<ExtractResult> Extract(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new List<ExtractResult>();
            }

            var result = new List<ExtractResult>();
            var matchSource = new Dictionary<Match, TypeTag>();
            var matched = new bool[source.Length];

            var collections = Regexes.ToDictionary(o => o.Key.Matches(source), p => p.Value);
            foreach (var collection in collections)
            {
                foreach (Match m in collection.Key)
                {
                    // In ExperimentalMode, AmbigiuousFraction like "30000 in 2009" needs to be skipped
                    if ((Options & NumberOptions.ExperimentalMode) != 0 && AmbiguousFractionConnectorsRegex.Match(m.Value).Success)
                    {
                        continue;
                    }

                    for (var j = 0; j < m.Length; j++)
                    {
                        matched[m.Index + j] = true;
                    }

                    // Keep Source Data for extra information
                    matchSource.Add(m, collection.Value);
                }
            }

            var last = -1;
            for (var i = 0; i < source.Length; i++)
            {
                if (matched[i])
                {
                    if (i + 1 == source.Length || !matched[i + 1])
                    {
                        var start = last + 1;
                        var length = i - last;
                        var substr = source.Substring(start, length);

                        if (matchSource.Keys.Any(o => o.Index == start && o.Length == length))
                        {
                            var type = matchSource.Where(p => p.Key.Index == start && p.Key.Length == length)
                                .Select(p => (p.Value.Priority, p.Value.Name)).Min().Item2;

                            // Extract negative numbers
                            if (NegativeNumberTermsRegex != null)
                            {
                                var match = NegativeNumberTermsRegex.Match(source.Substring(0, start));
                                if (match.Success)
                                {
                                    start = match.Index;
                                    length = length + match.Length;
                                    substr = match.Value + substr;
                                }
                            }

                            var er = new ExtractResult
                            {
                                Start = start,
                                Length = length,
                                Text = substr,
                                Type = ExtractType,
                                Data = type
                            };

                            result.Add(er);
                        }
                    }
                }
                else
                {
                    last = i;
                }
            }

            result = FilterAmbiguity(result, source);

            return result;
        }

        private List<ExtractResult> FilterAmbiguity(List<ExtractResult> ers, string text)
        {

            if (AmbiguityFiltersDict != null)
            {
                foreach (var regex in AmbiguityFiltersDict)
                {
                    if (regex.Key.IsMatch(text))
                    {
                        var matches = regex.Value.Matches(text).Cast<Match>();
                        ers = ers.Where(er => !matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start)).ToList();
                    }
                }
            }

            return ers;
        }

        protected Regex GenerateLongFormatNumberRegexes(LongFormatType type, string placeholder = BaseNumbers.PlaceHolderDefault)
        {
            var thousandsMark = Regex.Escape(type.ThousandsMark.ToString());
            var decimalsMark = Regex.Escape(type.DecimalsMark.ToString());

            var regexDefinition = type.DecimalsMark.Equals('\0') ?
                BaseNumbers.IntegerRegexDefinition(placeholder, thousandsMark) :
                BaseNumbers.DoubleRegexDefinition(placeholder, thousandsMark, decimalsMark);

            return new Regex(regexDefinition, RegexOptions.Singleline);
        }
    }

    public enum NumberMode
    {
        //Default is for unit and datetime
        Default,
        //Add 67.5 billion & million support.
        Currency,
        //Don't extract number from cases like 16ml
        PureNumber
    }

    public class LongFormatType
    {
        // Reference Value : 1234567.89

        // 1,234,567
        public static LongFormatType IntegerNumComma = new LongFormatType(',', '\0');

        // 1.234.567
        public static LongFormatType IntegerNumDot = new LongFormatType('.', '\0');

        // 1 234 567
        public static LongFormatType IntegerNumBlank = new LongFormatType(' ', '\0');

        // 1 234 567
        public static LongFormatType IntegerNumNoBreakSpace = new LongFormatType(Constants.NO_BREAK_SPACE, '\0');

        // 1'234'567
        public static LongFormatType IntegerNumQuote = new LongFormatType('\'', '\0');

        // 1,234,567.89
        public static LongFormatType DoubleNumCommaDot = new LongFormatType(',', '.');

        // 1,234,567·89
        public static LongFormatType DoubleNumCommaCdot = new LongFormatType(',', '·');

        // 1 234 567,89
        public static LongFormatType DoubleNumBlankComma = new LongFormatType(' ', ',');

        // 1 234 567,89
        public static LongFormatType DoubleNumNoBreakSpaceComma = new LongFormatType(Constants.NO_BREAK_SPACE, ',');

        // 1 234 567.89
        public static LongFormatType DoubleNumBlankDot = new LongFormatType(' ', '.');

        // 1 234 567.89
        public static LongFormatType DoubleNumNoBreakSpaceDot = new LongFormatType(Constants.NO_BREAK_SPACE, '.');

        // 1.234.567,89
        public static LongFormatType DoubleNumDotComma = new LongFormatType('.', ',');

        // 1'234'567,89
        public static LongFormatType DoubleNumQuoteComma = new LongFormatType('\'', ',');

        private LongFormatType(char thousandsMark, char decimalsMark)
        {
            ThousandsMark = thousandsMark;
            DecimalsMark = decimalsMark;
        }

        public char DecimalsMark { get; }

        public char ThousandsMark { get; }
    }
}
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class BaseNumberExtractor : IExtractor
    {
        public static readonly Regex CurrencyRegex =
            new Regex(BaseNumbers.CurrencyRegex, RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        protected BaseNumberExtractor(NumberOptions options = NumberOptions.None)
        {
            Options = options;
        }

        internal abstract ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected virtual ImmutableDictionary<Regex, Regex> AmbiguityFiltersDict { get; } = null;

        protected virtual string ExtractType { get; } = string.Empty;

        protected virtual NumberOptions Options { get; } = NumberOptions.None;

        protected virtual Regex NegativeNumberTermsRegex { get; } = null;

        protected virtual Regex AmbiguousFractionConnectorsRegex { get; } = null;

        protected virtual Regex RelativeReferenceRegex { get; } = null;

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
                    // In ExperimentalMode, AmbiguousFraction like "30000 in 2009" needs to be skipped
                    if ((Options & NumberOptions.ExperimentalMode) != 0 && AmbiguousFractionConnectorsRegex.Match(m.Value).Success)
                    {
                        continue;
                    }

                    // If SuppressExtendedTypes is on, cases like "last", "next" should be skipped
                    if ((Options & NumberOptions.SuppressExtendedTypes) != 0 && m.Groups[Constants.RelativeOrdinalGroupName].Success)
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
                            var (_, type, originalMatch) = matchSource.Where(p => p.Key.Index == start && p.Key.Length == length)
                                .Select(p => (p.Value.Priority, p.Value.Name, p.Key)).Min();

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
                                Data = type,
                            };

                            // Add Metadata information for Ordinal
                            if (ExtractType.Contains(Constants.MODEL_ORDINAL))
                            {
                                er.Metadata = new Metadata();
                                if ((Options & NumberOptions.SuppressExtendedTypes) == 0 &&
                                    originalMatch.Groups[Constants.RelativeOrdinalGroupName].Success)
                                {
                                    er.Metadata.IsOrdinalRelative = true;
                                }
                            }

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

        protected static Regex GenerateLongFormatNumberRegexes(LongFormatType type, string placeholder = BaseNumbers.PlaceHolderDefault,
                                                               RegexOptions flags = RegexOptions.Singleline)
        {
            var thousandsMark = Regex.Escape(type.ThousandsMark.ToString(CultureInfo.InvariantCulture));
            var decimalsMark = Regex.Escape(type.DecimalsMark.ToString(CultureInfo.InvariantCulture));

            var regexDefinition = type.DecimalsMark.Equals('\0') ?
                BaseNumbers.IntegerRegexDefinition(placeholder, thousandsMark) :
                BaseNumbers.DoubleRegexDefinition(placeholder, thousandsMark, decimalsMark);

            return new Regex(regexDefinition, flags);
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
    }
}

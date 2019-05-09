using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class BaseNumberExtractor : IExtractor
    {
        public static readonly Regex CurrencyRegex =
            new Regex(BaseNumbers.CurrencyRegex, RegexOptions.Singleline);

        public BaseNumberExtractor(NumberOptions options = NumberOptions.None)
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

        protected virtual Regex RelativeOrdinalFilterRegex { get; } = null;

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

                    // In EnablePreview, cases like "last", "next" should not be skipped
                    if ((Options & NumberOptions.EnablePreview) == 0 && IsRelativeOrdinal(m.Value))
                    {
                        continue;
                    }

                    for (var j = 0; j < m.Length; j++)
                    {
                        matched[m.Index + j] = true;
                    }

                    // Fliter out cases like "first two", "last one"
                    // only support in English now
                    if (ExtractType.Contains(Constants.MODEL_ORDINAL) && RelativeOrdinalFilterRegex != null && RelativeOrdinalFilterRegex.IsMatch(source))
                    {
                        continue;
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
                                Data = type,
                            };

                            // Add Metadata information for Ordinal
                            if (ExtractType.Contains(Constants.MODEL_ORDINAL))
                            {
                                er.Metadata = new Metadata();
                                if (IsRelativeOrdinal(substr))
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

        protected static Regex GenerateLongFormatNumberRegexes(LongFormatType type, string placeholder = BaseNumbers.PlaceHolderDefault)
        {
            var thousandsMark = Regex.Escape(type.ThousandsMark.ToString());
            var decimalsMark = Regex.Escape(type.DecimalsMark.ToString());

            var regexDefinition = type.DecimalsMark.Equals('\0') ?
                BaseNumbers.IntegerRegexDefinition(placeholder, thousandsMark) :
                BaseNumbers.DoubleRegexDefinition(placeholder, thousandsMark, decimalsMark);

            return new Regex(regexDefinition, RegexOptions.Singleline);
        }

        private bool IsRelativeOrdinal(string matchValue)
        {
            if (RelativeReferenceRegex == null)
            {
                return false;
            }

            return RelativeReferenceRegex.Match(matchValue).Success;
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
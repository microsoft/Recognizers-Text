using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Resources;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class BaseNumberExtractor : IExtractor
    {
        internal abstract ImmutableDictionary<Regex, string> Regexes { get; }

        protected virtual string ExtractType { get; } = "";

        public virtual List<ExtractResult> Extract(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new List<ExtractResult>();
            }

            var result = new List<ExtractResult>();
            var matchSource = new Dictionary<Match, string>();
            var matched = new bool[source.Length];
            for (var i = 0; i < source.Length; i++)
            {
                matched[i] = false;
            }

            var collections = Regexes.ToDictionary(o => o.Key.Matches(source), p => p.Value);
            foreach (var collection in collections)
            {
                foreach (Match m in collection.Key)
                {
                    for (var j = 0; j < m.Length; j++)
                    {
                        matched[m.Index + j] = true;
                    }

                    //Keep Source Data for extra information
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
                            var srcMatch = matchSource.Keys.First(o => o.Index == start && o.Length == length);
                            var er = new ExtractResult
                            {
                                Start = start,
                                Length = length,
                                Text = substr,
                                Type = ExtractType,
                                Data = matchSource.ContainsKey(srcMatch) ? matchSource[srcMatch] : null
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

            return result;
        }

        protected Regex GenerateLongFormatNumberRegexes(LongFormatType type, string placeholder = CommonNumeric.PlaceHolderDefault)
        {
            Regex addedRegex = null;
            string integerTemplate = CommonNumeric.IntegerTemplateRegex + $@"(?={placeholder})";
            string doubleTemplate = CommonNumeric.DoubleTemplateRegex + $@"(?={placeholder})";
            switch (type)
            {
                case LongFormatType.IntegerNumComma:
                    addedRegex = new Regex(string.Format(integerTemplate, ","), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.IntegerNumDot:
                    addedRegex = new Regex(string.Format(integerTemplate, Regex.Escape(".")), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.IntegerNumBlank:
                    addedRegex = new Regex(string.Format(integerTemplate, " "), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.IntegerNumQuote:
                    addedRegex = new Regex(string.Format(integerTemplate, Regex.Escape("'")), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.DoubleNumCommaDot:
                    addedRegex = new Regex(string.Format(doubleTemplate, ",", Regex.Escape(".")), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.DoubleNumDotComma:
                    addedRegex = new Regex(string.Format(doubleTemplate, Regex.Escape("."), ","), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.DoubleNumBlankComma:
                    addedRegex = new Regex(string.Format(doubleTemplate, " ", ","), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.DoubleNumBlankDot:
                    addedRegex = new Regex(string.Format(doubleTemplate, " ", Regex.Escape(".")), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.DoubleNumCommaCdot:
                    addedRegex = new Regex(string.Format(doubleTemplate, ",", "·"), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
                case LongFormatType.DoubleNumQuoteComma:
                    addedRegex = new Regex(string.Format(doubleTemplate, Regex.Escape("'"), ","), RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    break;
            }
            return addedRegex;
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

    public enum LongFormatType
    {
        // Reference : https://www.wikiwand.com/en/Decimal_mark

        // Value : 1234567.89
        // 1,234,567
        IntegerNumComma,
        // 1.234.567
        IntegerNumDot,
        // 1 234 567
        IntegerNumBlank,
        // 1'234'567
        IntegerNumQuote,
        // 1,234,567.89
        DoubleNumCommaDot,
        // 1,234,567·89
        DoubleNumCommaCdot,
        // 1 234 567,89
        DoubleNumBlankComma,
        // 1 234 567.89
        DoubleNumBlankDot,
        // 1.234.567,89
        DoubleNumDotComma,
        // 1'234'567,89
        DoubleNumQuoteComma
    }
}
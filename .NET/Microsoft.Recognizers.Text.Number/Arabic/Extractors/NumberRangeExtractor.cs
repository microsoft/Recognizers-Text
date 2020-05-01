using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Arabic;

namespace Microsoft.Recognizers.Text.Number.Arabic
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.RightToLeft;

        private readonly BaseNumberExtractor numberExtractor;

        private readonly BaseNumberExtractor ordinalExtractor;

        private readonly BaseNumberParser numberParser;

        public NumberRangeExtractor(INumberOptionsConfiguration config)
            : base(
                   NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   new BaseNumberParser(new ArabicNumberParserConfiguration(config)),
                   config)
        {

            this.numberExtractor = NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options));
            this.ordinalExtractor = OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options));
            this.numberParser = new BaseNumberParser(new ArabicNumberParserConfiguration(config));

            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // between...and...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // more than ... less than ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // less than ... more than ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // from ... to/~/- ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // more/greater/higher than ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // 30 and/or greater/higher
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // less/smaller/lower than ...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // 30 and/or less/smaller/lower
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // equal to ...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags),
                    NumberRangeConstants.EQUAL
                },
                {
                    // equal to 30 or more than, larger than 30 or equal to ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // equal to 30 or less, smaller than 30 or equal ...
                    new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexFlags),
                    NumberRangeConstants.LESS
                },
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex =
                new Regex(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexFlags);
        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;

        public override List<ExtractResult> Extract(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new List<ExtractResult>();
            }

            var results = new List<ExtractResult>();
            var matchSource = new Dictionary<Tuple<int, int>, string>();
            var matched = new bool[source.Length];

            var collections = Regexes.ToDictionary(o => o.Key.Matches(source), p => p.Value);
            foreach (var collection in collections)
            {
                foreach (Match m in collection.Key)
                {
                    GetMatchedStartAndLengthForArabic(m, collection.Value, source, out int start, out int length);

                    if (start >= 0 && length > 0)
                    {
                        // Keep Source Data for extra information
                        matchSource.Add(new Tuple<int, int>(start, length), collection.Value);
                    }
                }
            }

            foreach (var match in matchSource)
            {
                var start = match.Key.Item1;
                var length = match.Key.Item2;

                // Filter wrong two number ranges such as "more than 20 and less than 10" and "大于20小于10".
                if (match.Value.Equals(NumberRangeConstants.TWONUM))
                {
                    int moreIndex = 0, lessIndex = 0;

                    var text = source.Substring(match.Key.Item1, match.Key.Item2);

                    var er = numberExtractor.Extract(text);

                    if (er.Count != 2)
                    {
                        er = ordinalExtractor.Extract(text);

                        if (er.Count != 2)
                        {
                            continue;
                        }
                    }

                    var nums = er.Select(r => (double)(numberParser.Parse(r).Value ?? 0)).ToList();

                    moreIndex = matchSource.First(r => r.Value.Equals(NumberRangeConstants.MORE) && r.Key.Item1 >= start &&
                                                       r.Key.Item1 + r.Key.Item2 <= start + length).Key.Item1;
                    lessIndex = matchSource.First(r => r.Value.Equals(NumberRangeConstants.LESS) && r.Key.Item1 >= start &&
                                                       r.Key.Item1 + r.Key.Item2 <= start + length).Key.Item1;

                    if (!((nums[0] < nums[1] && moreIndex <= lessIndex) || (nums[0] > nums[1] && moreIndex >= lessIndex)))
                    {
                        continue;
                    }
                }

                // The entity is longer than 1, so don't mark the last char to represent the end.
                // To avoid no connector cases like "大于20小于10" being marked as a whole entity.
                for (var j = 0; j < length - 1; j++)
                {
                    matched[start + j] = true;
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
                        var length = i - last + 1;
                        var substr = source.Substring(start, length);

                        if (matchSource.Keys.Any(o => o.Item1 == start && o.Item2 == length))
                        {
                            var srcMatch = matchSource.Keys.First(o => o.Item1 == start && o.Item2 == length);
                            var er = new ExtractResult
                            {
                                Start = start,
                                Length = length,
                                Text = substr,
                                Type = ExtractType,
                                Data = matchSource.ContainsKey(srcMatch) ? matchSource[srcMatch] : null,
                            };

                            results.Add(er);
                        }
                    }
                }
                else
                {
                    last = i;
                }
            }

            // In ExperimentalMode, cases like "from 3 to 5" and "between 10 and 15" are set to closed at both start and end
            if ((Config.Options & NumberOptions.ExperimentalMode) != 0)
            {
                foreach (var result in results)
                {
                    var data = result.Data.ToString();
                    if (data == NumberRangeConstants.TWONUMBETWEEN ||
                        data == NumberRangeConstants.TWONUMTILL)
                    {
                        result.Data = NumberRangeConstants.TWONUMCLOSED;
                    }
                }
            }

            return results;
        }

        private void GetMatchedStartAndLengthForArabic(Match match, string type, string source, out int start, out int length)
        {
            start = NumberRangeConstants.INVALID_NUM;
            length = NumberRangeConstants.INVALID_NUM;

            // There is space coming for grouped data before text, similar issue did not observe in English.
            // So trimming the data from start
            var numberStr1 = match.Groups["number1"].Value.TrimStart();
            var numberStr2 = match.Groups["number2"].Value.TrimStart();

            if (type.Contains(NumberRangeConstants.TWONUM))
            {
                var extractNumList1 = ExtractNumberAndOrdinalFromStr(numberStr1);
                var extractNumList2 = ExtractNumberAndOrdinalFromStr(numberStr2);

                if (extractNumList1 != null && extractNumList2 != null)
                {
                    if (type.Contains(NumberRangeConstants.TWONUMTILL))
                    {
                        // num1 must have same type with num2
                        if (extractNumList1[0].Type != extractNumList2[0].Type)
                        {
                            return;
                        }

                        // num1 must less than num2
                        var num1 = (double)(numberParser.Parse(extractNumList1[0]).Value ?? 0);
                        var num2 = (double)(numberParser.Parse(extractNumList2[0]).Value ?? 0);

                        if (num1 > num2)
                        {
                            return;
                        }

                        extractNumList1.RemoveRange(1, extractNumList1.Count - 1);
                        extractNumList2.RemoveRange(1, extractNumList2.Count - 1);
                    }

                    bool validNum1, validNum2;
                    start = match.Index;
                    length = match.Length;

                    validNum1 = ValidateMatchAndGetStartAndLength(extractNumList1, numberStr1, match, source, ref start, ref length);
                    validNum2 = ValidateMatchAndGetStartAndLength(extractNumList2, numberStr2, match, source, ref start, ref length);

                    if (!validNum1 || !validNum2)
                    {
                        start = NumberRangeConstants.INVALID_NUM;
                        length = NumberRangeConstants.INVALID_NUM;
                    }
                }
            }
            else
            {
                var numberStr = string.IsNullOrEmpty(numberStr1) ? numberStr2 : numberStr1;

                var isAmbiguousRangeOrFraction = IsAmbiguousRangeOrFraction(match, type, numberStr);
                var extractNumList = ExtractNumberAndOrdinalFromStr(numberStr, isAmbiguousRangeOrFraction);

                if (extractNumList != null)
                {
                    start = match.Index;
                    length = match.Length;

                    if (!ValidateMatchAndGetStartAndLength(extractNumList, numberStr, match, source, ref start, ref length))
                    {
                        start = NumberRangeConstants.INVALID_NUM;
                        length = NumberRangeConstants.INVALID_NUM;
                    }
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseNumberRangeParser : IParser
    {
        public BaseNumberRangeParser(INumberRangeParserConfiguration config)
        {
            this.Config = config;
        }

        protected INumberRangeParserConfiguration Config { get; private set; }

        public virtual ParseResult Parse(ExtractResult extResult)
        {
            ParseResult ret = null;

            var type = extResult.Data as string;
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Contains(NumberRangeConstants.TWONUM))
                {
                    ret = ParseNumberRangeWhichHasTwoNum(extResult);
                }
                else
                {
                    ret = ParseNumberRangeWhichHasOneNum(extResult);
                }
            }

            return ret;
        }

        private ParseResult ParseNumberRangeWhichHasTwoNum(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var er = Config.NumberExtractor.Extract(extResult.Text);

            // Valid extracted results for this type should have two numbers
            if (er.Count != 2)
            {
                er = Config.OrdinalExtractor.Extract(extResult.Text);

                if (er.Count != 2)
                {
                    return result;
                }
            }

            var nums = er.Select(r => (double)(Config.NumberParser.Parse(r).Value ?? 0)).ToList();

            double startValue, endValue;
            if (nums[0] < nums[1])
            {
                startValue = nums[0];
                endValue = nums[1];
            }
            else
            {
                startValue = nums[1];
                endValue = nums[0];
            }

            var startValueStr = startValue.ToString(CultureInfo.InvariantCulture);
            var endValueStr = endValue.ToString(CultureInfo.InvariantCulture);

            char leftBracket, rightBracket;
            var type = extResult.Data as string;

            if (type.Contains(NumberRangeConstants.TWONUMCLOSED))
            {
                leftBracket = NumberRangeConstants.LEFT_CLOSED;
                rightBracket = NumberRangeConstants.RIGHT_CLOSED;
            }
            else if (type.Contains(NumberRangeConstants.TWONUMTILL) || type.Contains(NumberRangeConstants.TWONUMBETWEEN))
            {
                // 20~30: [20,30)
                // between 20 and 30: [20,30)
                leftBracket = NumberRangeConstants.LEFT_CLOSED;
                rightBracket = NumberRangeConstants.RIGHT_OPEN;
            }
            else
            {
                // check whether it contains string like "more or equal", "less or equal", "at least", etc.
                var match = Config.MoreOrEqual.Match(extResult.Text);

                if (!match.Success)
                {
                    match = Config.MoreOrEqualSuffix.Match(extResult.Text);
                }

                if (match.Success)
                {
                    leftBracket = NumberRangeConstants.LEFT_CLOSED;
                }
                else
                {
                    leftBracket = NumberRangeConstants.LEFT_OPEN;
                }

                match = Config.LessOrEqual.Match(extResult.Text);

                if (!match.Success)
                {
                    match = Config.LessOrEqualSuffix.Match(extResult.Text);
                }

                if (match.Success)
                {
                    rightBracket = NumberRangeConstants.RIGHT_CLOSED;
                }
                else
                {
                    rightBracket = NumberRangeConstants.RIGHT_OPEN;
                }
            }

            result.Value = new Dictionary<string, double>()
            {
                { "StartValue", startValue },
                { "EndValue", endValue },
            };

            result.ResolutionStr = string.Concat(leftBracket, startValueStr, NumberRangeConstants.INTERVAL_SEPARATOR, endValueStr, rightBracket);

            return result;
        }

        private ParseResult ParseNumberRangeWhichHasOneNum(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type,
            };

            var er = Config.NumberExtractor.Extract(extResult.Text);

            // Valid extracted results for this type should have one number
            if (er.Count != 1)
            {
                er = Config.OrdinalExtractor.Extract(extResult.Text);

                if (er.Count != 1)
                {
                    return result;
                }
            }

            var num = er.Select(r => (double)(Config.NumberParser.Parse(r).Value ?? 0)).ToList();

            char leftBracket, rightBracket;
            string startValueStr = string.Empty, endValueStr = string.Empty;
            var type = extResult.Data as string;
            if (type.Contains(NumberRangeConstants.MORE))
            {
                rightBracket = NumberRangeConstants.RIGHT_OPEN;

                var match = Config.MoreOrEqual.Match(extResult.Text);

                if (!match.Success)
                {
                    match = Config.MoreOrEqualSuffix.Match(extResult.Text);
                }

                if (!match.Success)
                {
                    match = Config.MoreOrEqualSeparate.Match(extResult.Text);
                }

                if (match.Success)
                {
                    leftBracket = NumberRangeConstants.LEFT_CLOSED;
                }
                else
                {
                    leftBracket = NumberRangeConstants.LEFT_OPEN;
                }

                startValueStr = num[0].ToString(CultureInfo.InvariantCulture);

                result.Value = new Dictionary<string, double>()
                {
                    { "StartValue", num[0] },
                };
            }
            else if (type.Contains(NumberRangeConstants.LESS))
            {
                leftBracket = NumberRangeConstants.LEFT_OPEN;

                var match = Config.LessOrEqual.Match(extResult.Text);

                if (!match.Success)
                {
                    match = Config.LessOrEqualSuffix.Match(extResult.Text);
                }

                if (!match.Success)
                {
                    match = Config.LessOrEqualSeparate.Match(extResult.Text);
                }

                if (match.Success)
                {
                    rightBracket = NumberRangeConstants.RIGHT_CLOSED;
                }
                else
                {
                    rightBracket = NumberRangeConstants.RIGHT_OPEN;
                }

                endValueStr = num[0].ToString(CultureInfo.InvariantCulture);

                result.Value = new Dictionary<string, double>()
                {
                    { "EndValue", num[0] },
                };
            }
            else
            {
                leftBracket = NumberRangeConstants.LEFT_CLOSED;
                rightBracket = NumberRangeConstants.RIGHT_CLOSED;

                startValueStr = num[0].ToString(CultureInfo.InvariantCulture);
                endValueStr = startValueStr;

                result.Value = new Dictionary<string, double>()
                {
                    { "StartValue", num[0] },
                    { "EndValue", num[0] },
                };
            }

            result.ResolutionStr = string.Concat(leftBracket, startValueStr, NumberRangeConstants.INTERVAL_SEPARATOR, endValueStr, rightBracket);

            return result;
        }
    }
}

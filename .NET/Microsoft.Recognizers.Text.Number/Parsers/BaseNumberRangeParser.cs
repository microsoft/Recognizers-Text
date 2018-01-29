using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseNumberRangeParser : IParser
    {
        protected readonly INumberRangeParserConfiguration Config;

        public BaseNumberRangeParser(INumberRangeParserConfiguration config)
        {
            this.Config = config;
        }

        public virtual ParseResult Parse(ExtractResult extResult)
        {
            ParseResult ret = null;

            var type = extResult.Data as string;
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Contains(Constants.TWONUM))
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
                Type = extResult.Type
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
            var startValueStr = Config.CultureInfo != null ? startValue.ToString(Config.CultureInfo) : startValue.ToString();
            var endValueStr = Config.CultureInfo != null ? endValue.ToString(Config.CultureInfo) : endValue.ToString();

            char leftBracket, rightBracket;
            var type = extResult.Data as string;
            if (type.Contains(Constants.TWONUMBETWEEN))
            {
                // between 20 and 30: (20,30)
                leftBracket = Constants.LEFT_OPEN;
                rightBracket = Constants.RIGHT_OPEN;
            }
            else if (type.Contains(Constants.TWONUMTILL))
            {
                // 20~30: [20,30)
                leftBracket = Constants.LEFT_CLOSED;
                rightBracket = Constants.RIGHT_OPEN;
            }
            else
            {
                // check whether it contains string like "more or equal", "less or equal", "at least", etc.
                var match = Config.MoreOrEqual.Match(extResult.Text);
                if (match.Success)
                {
                    leftBracket = Constants.LEFT_CLOSED;
                }
                else
                {
                    leftBracket = Constants.LEFT_OPEN;
                }

                match = Config.LessOrEqual.Match(extResult.Text);
                if (match.Success)
                {
                    rightBracket = Constants.RIGHT_CLOSED;
                }
                else
                {
                    rightBracket = Constants.RIGHT_OPEN;
                }
            }

            result.Value = new Dictionary<string, double>()
                {
                    { "StartValue", startValue },
                    { "EndValue", endValue }
                };
            result.ResolutionStr = string.Concat(leftBracket, startValueStr, Constants.COMMA, endValueStr, rightBracket);

            return result;
        }

        private ParseResult ParseNumberRangeWhichHasOneNum(ExtractResult extResult)
        {
            var result = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
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
            if (type.Contains(Constants.MORE))
            {
                rightBracket = Constants.RIGHT_OPEN;

                var match = Config.MoreOrEqual.Match(extResult.Text);
                if (match.Success)
                {
                    leftBracket = Constants.LEFT_CLOSED;
                }
                else
                {
                    leftBracket = Constants.LEFT_OPEN;
                }

                startValueStr = Config.CultureInfo != null ? num[0].ToString(Config.CultureInfo) : num[0].ToString();

                result.Value = new Dictionary<string, double>()
                {
                    { "StartValue", num[0] }
                };
            }
            else if (type.Contains(Constants.LESS))
            {
                leftBracket = Constants.LEFT_OPEN;

                var match = Config.LessOrEqual.Match(extResult.Text);
                if (match.Success)
                {
                    rightBracket = Constants.RIGHT_CLOSED;
                }
                else
                {
                    rightBracket = Constants.RIGHT_OPEN;
                }

                endValueStr = Config.CultureInfo != null ? num[0].ToString(Config.CultureInfo) : num[0].ToString();

                result.Value = new Dictionary<string, double>()
                {
                    { "EndValue", num[0] }
                };
            }
            else
            {
                leftBracket = Constants.LEFT_CLOSED;
                rightBracket = Constants.RIGHT_CLOSED;

                startValueStr = Config.CultureInfo != null ? num[0].ToString(Config.CultureInfo) : num[0].ToString();
                endValueStr = startValueStr;

                result.Value = new Dictionary<string, double>()
                {
                    { "StartValue", num[0] },
                    { "EndValue", num[0] }
                };
            }

            result.ResolutionStr = string.Concat(leftBracket, startValueStr, Constants.COMMA, endValueStr, rightBracket);

            return result;
        }
    }
}

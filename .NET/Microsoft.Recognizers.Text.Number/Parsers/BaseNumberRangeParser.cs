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
                if (type.Contains("TwoNum"))
                {
                    ret = ParseNumberRangeWhichHasTwoNum(extResult);
                }
                else if (type.Contains("OneNum"))
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

            result.Value = new Dictionary<string, double>()
            {
                { "StartValue", startValue },
                { "EndValue", endValue }
            };

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

            if (er.Count != 1)
            {
                er = Config.OrdinalExtractor.Extract(extResult.Text);

                if (er.Count != 1)
                {
                    return result;
                }
            }

            var num = er.Select(r => (double)(Config.NumberParser.Parse(r).Value ?? 0)).ToList();

            var type = extResult.Data as string;
            if (type.Contains("More"))
            {
                result.Value = new Dictionary<string, double>()
                {
                    { "StartValue", num[0] }
                };
            }
            else if (type.Contains("Less"))
            {
                result.Value = new Dictionary<string, double>()
                {
                    { "EndValue", num[0] }
                };
            }
            else if (type.Contains("Equal"))
            {
                result.Value = new Dictionary<string, double>()
                {
                    { "StartValue", num[0] },
                    { "EndValue", num[0] }
                };
            }

            return result;
        }
    }
}

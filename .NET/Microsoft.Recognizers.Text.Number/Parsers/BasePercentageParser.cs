using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Number
{
    public class BasePercentageParser : BaseNumberParser
    {
        public BasePercentageParser(INumberParserConfiguration config) : base(config)
        {
        }

        public override ParseResult Parse(ExtractResult extResult)
        {
            string originText = extResult.Text;
            ParseResult ret = null;

            // do replace text & data from extended info
            if (extResult.Data is List<KeyValuePair<string, ExtractResult>>)
            {
                var extendedData = (List<KeyValuePair<string, ExtractResult>>) extResult.Data;
                if (extendedData.Count == 2)
                {
                    extResult.Text = $"{extendedData[0].Key} {Config.FractionMarkerToken} {extendedData[1].Key}";
                    extResult.Data = $"Frac{Config.LangMarker}";

                    ret = base.Parse(extResult);
                    ret.Value = (double)ret.Value * 100;
                    ret.ResolutionStr = Config.CultureInfo != null
                        ? ((double)ret.Value).ToString(Config.CultureInfo) + "%"
                        : ret.Value + "%";
                }
                else if (extendedData.Count == 1)
                {
                    extResult.Text = extendedData[0].Key;
                    extResult.Data = extendedData[0].Value.Data;

                    ret = base.Parse(extResult);

                    if (!string.IsNullOrWhiteSpace(ret.ResolutionStr))
                    {
                        if (!ret.ResolutionStr.Trim().EndsWith("%"))
                        {
                            ret.ResolutionStr = ret.ResolutionStr.Trim() + "%";
                        }
                    }
                }
            }

            ret.Data = extResult.Text;
            ret.Text = originText;

            return ret;
        }
    }
}
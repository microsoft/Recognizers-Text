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

            // replace text & data from extended info
            if (extResult.Data is List<(string, ExtractResult)> extendedData1)
            {
                if (extendedData1.Count == 2)
                {
                    // for case like "2 out of 5".
                    extResult.Text = $"{extendedData1[0].Item1} {Config.FractionMarkerToken} {extendedData1[1].Item1}";
                    extResult.Data = $"Frac{Config.LangMarker}";

                    ret = base.Parse(extResult);
                    ret.Value = (double) ret.Value * 100;
                }
                else if (extendedData1.Count == 1)
                {
                    // for case like "one third of".
                    extResult.Text = extendedData1[0].Item1;
                    extResult.Data = extendedData1[0].Item2.Data;

                    ret = base.Parse(extResult);

                    if (extResult.Data.ToString().StartsWith("Frac"))
                    {
                        ret.Value = (double) ret.Value * 100;
                    }
                }
                ret.ResolutionStr = Config.CultureInfo != null
                    ? ((double) ret.Value).ToString(Config.CultureInfo) + "%"
                    : ret.Value + "%";
            }
            else 
            {
                // for case like "one percent" or "1%".
                var extendedData = ((string, ExtractResult)) extResult.Data;
                extResult.Text = extendedData.Item1;
                extResult.Data = extendedData.Item2.Data;
                ret = base.Parse(extResult);

                if (!string.IsNullOrWhiteSpace(ret.ResolutionStr))
                {
                    if (!ret.ResolutionStr.Trim().EndsWith("%"))
                    {
                        ret.ResolutionStr = ret.ResolutionStr.Trim() + "%";
                    }
                }
            }

            ret.Data = extResult.Text;
            ret.Text = originText;
            ret.Type = string.Empty;

            return ret;
        }
    }
}
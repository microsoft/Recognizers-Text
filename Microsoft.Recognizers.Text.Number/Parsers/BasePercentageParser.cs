using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Number.Parsers
{
    public class BasePercentageParser : BaseNumberParser
    {
        public BasePercentageParser(INumberParserConfiguration config) : base(config) { }

        public override ParseResult Parse(ExtractResult extResult)
        {
            string originText = extResult.Text;

            // do replace text & data from extended info
            if (extResult.Data is KeyValuePair<string, ExtractResult>)
            {
                var extendedData = (KeyValuePair<string, ExtractResult>)extResult.Data;
                extResult.Text = extendedData.Key;
                extResult.Data = extendedData.Value.Data;
            }

            var ret = base.Parse(extResult);

            if (!string.IsNullOrWhiteSpace(ret.ResolutionStr))
            {
                if (!ret.ResolutionStr.Trim().EndsWith("%"))
                {
                    ret.ResolutionStr = ret.ResolutionStr.Trim() + "%";
                }
            }

            ret.Data = extResult.Text;
            ret.Text = originText;

            return ret;
        }
    }
}

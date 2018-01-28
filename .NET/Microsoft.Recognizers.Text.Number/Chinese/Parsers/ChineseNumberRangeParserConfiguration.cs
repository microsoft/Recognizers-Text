using Microsoft.Recognizers.Definitions.Chinese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class ChineseNumberRangeParserConfiguration :INumberRangeParserConfiguration
    {
        public IExtractor NumberExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IParser NumberParser { get; }

        public ChineseNumberRangeParserConfiguration()
        {
            NumberExtractor = new NumberExtractor();
            OrdinalExtractor = new OrdinalExtractor();
            NumberParser =  new ChineseNumberParser(new ChineseNumberParserConfiguration());
        }
    }
}

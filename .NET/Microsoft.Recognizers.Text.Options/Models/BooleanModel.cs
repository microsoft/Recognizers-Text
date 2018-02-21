using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.Options.Models
{
    public class BooleanModel : OptionsModel
    {
        public BooleanModel(IParser parser, IExtractor extractor) : base(parser,extractor)
        {

        }

        public override string ModelTypeName => Constants.MODEL_BOOLEAN;

        protected override SortedDictionary<string, object> GetResolution(ParseResult parseResult)
        {
            var results = new SortedDictionary<string, object>()
            {
                { "value", parseResult.Value }
            };
            

            return results;
        }
    }
}

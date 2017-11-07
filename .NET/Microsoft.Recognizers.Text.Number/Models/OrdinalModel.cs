using Microsoft.Recognizers.Text.Number.Extractors;
using Microsoft.Recognizers.Text.Number.Parsers;

namespace Microsoft.Recognizers.Text.Number.Models
{
    public class OrdinalModel : AbstractNumberModel
    {
        public OrdinalModel(IParser parser, IExtractor extractor) : base(parser, extractor)
        {
        }

        public override string ModelTypeName => "ordinal";
    }
}
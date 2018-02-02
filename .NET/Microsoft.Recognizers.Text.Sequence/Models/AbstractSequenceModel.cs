using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Sequence
{
    public abstract class AbstractSequenceModel : IModel
    {
        protected AbstractSequenceModel(IParser parser, IExtractor extractor)
        {
            this.Parser = parser;
            this.Extractor = extractor;
        }

        public abstract string ModelTypeName { get; }

        protected IExtractor Extractor { get; private set; }

        protected IParser Parser { get; private set; }

        public List<ModelResult> Parse(string query)
        {
            throw new NotImplementedException();

        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Number.Models
{
    public abstract class AbstractNumberModel : IModel
    {
        public AbstractNumberModel(IParser parser, IExtractor extractor)
        {
            this.Parser = parser;
            this.Extractor = extractor;
        }

        public abstract string ModelTypeName { get; }

        protected IExtractor Extractor { get; private set; }
        protected IParser Parser { get; private set; }

        public List<ModelResult> Parse(string query)
        {
            var extractResults = Extractor.Extract(query);
            var parseNums = new List<ParseResult>();
            foreach (var result in extractResults)
            {
                parseNums.Add(Parser.Parse(result));
            }
            return parseNums.Select(o => new ModelResult
            {
                Start = o.Start.Value,
                End = o.Start.Value + o.Length.Value - 1,
                Resolution = new SortedDictionary<string, object> { { "value", o.ResolutionStr } },
                Text = o.Text,
                TypeName = ModelTypeName
            }).ToList();
        }
    }
}
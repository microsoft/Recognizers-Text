using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public abstract class AbstractNumberWithUnitModel : IModel
    {
        private IExtractor extractor;
        private IParser parser;

        public AbstractNumberWithUnitModel(IParser parser, IExtractor extractor)
        {
            this.parser = parser;
            this.extractor = extractor;
        }

        public abstract string ModelTypeName { get; }

        protected IExtractor Extractor => extractor;
        protected IParser Parser => parser;

        public List<ModelResult> Parse(string query)
        {
            // preprocess the query
            query = FormatUtility.Preprocess(query, false);
            var extractResults = Extractor.Extract(query);
            var parseResults = new List<ParseResult>();
            foreach (var result in extractResults)
            {
                parseResults.Add(Parser.Parse(result));
            }
            return parseResults.Select(o => new ModelResult
            {
                Start = o.Start.Value,
                End = o.Start.Value + o.Length.Value - 1,
                Resolution = new SortedDictionary<string, object>
                {
                    {"value", ((UnitValue) o.Value).Number},
                    {"unit", ((UnitValue) o.Value).Unit}
                },
                Text = o.Text,
                TypeName = ModelTypeName
            }).ToList();
        }
    }
}
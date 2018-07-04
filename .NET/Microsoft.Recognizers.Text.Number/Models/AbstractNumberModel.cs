using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class AbstractNumberModel : IModel
    {
        protected AbstractNumberModel(IParser parser, IExtractor extractor)
        {
            this.Parser = parser;
            this.Extractor = extractor;
        }

        public abstract string ModelTypeName { get; }

        protected IExtractor Extractor { get; private set; }

        protected IParser Parser { get; private set; }

        public List<ModelResult> Parse(string query)
        {

            var parsedNumbers = new List<ParseResult>();

            try
            {
                var extractResults = Extractor.Extract(query);

                foreach (var result in extractResults)
                {
                    var parseResult = Parser.Parse(result);
                    if (parseResult.Data is List<ParseResult> parseResults)
                    {
                        parsedNumbers.AddRange(parseResults);
                    }
                    else
                    {
                        parsedNumbers.Add(parseResult);
                    }
                }
            }
            catch (Exception)
            {
                // Nothing to do. Exceptions in parse should not break users of recognizers.
                // No result.
            }

            return parsedNumbers.Select(o =>
            {
                var end = o.Start.Value + o.Length.Value - 1;
                return new ModelResult
                {
                    Start = o.Start.Value,
                    End = end,
                    Resolution = new SortedDictionary<string, object> {{ResolutionKey.Value, o.ResolutionStr}},
                    Text = o.Text,
                    TypeName = ModelTypeName
                };
            }).ToList();
        }
    }
}
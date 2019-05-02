using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class AbstractNumberModel : IModel
    {
        // Languages supporting subtypes in the resolution to be added here
        private static readonly List<string> ExtractorsSupportingSubtype = new List<string> { Constants.ENGLISH, Constants.SWEDISH };

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

            // Preprocess the query
            query = QueryProcessor.Preprocess(query, caseSensitive: true);

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
                var resolution = new SortedDictionary<string, object>();
                if (o.Value != null)
                {
                    resolution.Add(ResolutionKey.Value, o.ResolutionStr);
                }

                var extractorSupportsSubtype = ExtractorsSupportingSubtype.Exists(e => Extractor.GetType().ToString().Contains(e));

                // Check if current extractor supports the Subtype field in the resolution
                // As some languages like German, we miss handling some subtypes between "decimal" and "integer"
                if (!string.IsNullOrEmpty(o.Type) &&
                    Constants.ValidSubTypes.Contains(o.Type) && extractorSupportsSubtype)
                {
                    resolution.Add(ResolutionKey.SubType, o.Type);
                }

                var type = string.Empty;

                // for ordinal and ordinal.relative
                // Only support "ordinal.relative" for English for now
                if (ModelTypeName.Equals(Constants.MODEL_ORDINAL))
                {
                    if (o.Metadata != null && o.Metadata.IsOrdinalRelative)
                    {
                        type = $"{ModelTypeName}.{Constants.RELATIVE}";
                    }
                    else
                    {
                        type = ModelTypeName;
                    }

                    resolution.Add(ResolutionKey.Offset, o.Metadata.Offset);
                    resolution.Add(ResolutionKey.RelativeTo, o.Metadata.RelativeTo);
                }
                else
                {
                    type = ModelTypeName;
                }

                return new ModelResult
                {
                    Start = o.Start.Value,
                    End = end,
                    Resolution = resolution,
                    Text = o.Text,
                    TypeName = type,
                };
            }).ToList();
        }
    }
}
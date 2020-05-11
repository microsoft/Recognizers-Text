using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class AbstractNumberModel : IModel
    {
        // Languages supporting subtypes in the resolution to be added here
        private static readonly List<string> ExtractorsSupportingSubtype = new List<string>
        {
                Constants.ENGLISH,
                Constants.SWEDISH,
                Constants.ARABIC,

                // TODO: Temporarily disabled as existing TestSpec not supporting
                // Constants.JAPANESE_SUBS
        };

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

                var modelResults = parsedNumbers.Select(BuildModelResult).Where(r => r != null).ToList();

                return modelResults;
            }
            catch (Exception)
            {
                // Nothing to do. Exceptions in parse should not break users of recognizers.
                // No result.
            }

            return new List<ModelResult>();
        }

        private ModelResult BuildModelResult(ParseResult pn)
        {

            try
            {
                var end = pn.Start.Value + pn.Length.Value - 1;
                var resolution = new SortedDictionary<string, object>();
                if (pn.Value != null)
                {
                    resolution.Add(ResolutionKey.Value, pn.ResolutionStr);
                }

                var extractorSupportsSubtype = ExtractorsSupportingSubtype.Exists(e => Extractor.GetType().ToString().Contains(e));

                // Check if current extractor supports the Subtype field in the resolution
                // As some languages like German, we miss handling some subtypes between "decimal" and "integer"
                if (!string.IsNullOrEmpty(pn.Type) &&
                    Constants.ValidSubTypes.Contains(pn.Type) && extractorSupportsSubtype)
                {
                    resolution.Add(ResolutionKey.SubType, pn.Type);
                }

                string specificNumberType;

                // For ordinal and ordinal.relative - "ordinal.relative" only available in English for now
                if (ModelTypeName.Equals(Constants.MODEL_ORDINAL, StringComparison.Ordinal))
                {
                    specificNumberType = pn.Type;
                    resolution.Add(ResolutionKey.Offset, pn.Metadata.Offset);
                    resolution.Add(ResolutionKey.RelativeTo, pn.Metadata.RelativeTo);
                }
                else
                {
                    specificNumberType = ModelTypeName;
                }

                return new ModelResult
                {
                    Start = pn.Start.Value,
                    End = end,
                    Resolution = resolution,
                    Text = pn.Text,
                    TypeName = specificNumberType,
                };
            }
            catch (Exception)
            {
                // Nothing to do. Exceptions in result process should not affect other extracted entities.
                // No result.
            }

            return null; // Only in failure cases. These will be filtered out before final output.
        }

    }
}
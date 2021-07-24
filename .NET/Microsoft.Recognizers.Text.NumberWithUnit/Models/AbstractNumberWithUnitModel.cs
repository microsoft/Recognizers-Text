﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public abstract class AbstractNumberWithUnitModel : IModel
    {
        private string culture;

        private string requestedCulture;

        protected AbstractNumberWithUnitModel(Dictionary<IExtractor, IParser> extractorParserDic)
        {
            this.ExtractorParserDic = extractorParserDic;
        }

        public abstract string ModelTypeName { get; }

        public string Culture => this.culture;

        public string RequestedCulture => this.requestedCulture;

        protected Dictionary<IExtractor, IParser> ExtractorParserDic { get; }

        public List<ModelResult> Parse(string query)
        {
            // Preprocess the query
            query = QueryProcessor.Preprocess(query, caseSensitive: true);

            var extractionResults = new List<ModelResult>();

            try
            {
                foreach (var p in ExtractorParserDic)
                {
                    var extractor = p.Key;
                    var parser = p.Value;

                    var extractedResults = extractor.Extract(query);

                    var parsedResults = new List<ParseResult>();

                    foreach (var result in extractedResults)
                    {
                        var parseResult = parser.Parse(result);
                        if (parseResult.Value is List<ParseResult>)
                        {
                            parsedResults.AddRange((List<ParseResult>)parseResult.Value);
                        }
                        else
                        {
                            parsedResults.Add(parseResult);
                        }
                    }

                    var modelResults = parsedResults.Select(o => new ModelResult
                    {
                        Start = o.Start.Value,
                        End = o.Start.Value + o.Length.Value - 1,
                        Resolution = (o.Value is CurrencyUnitValue) ? new SortedDictionary<string, object>
                            {
                                { ResolutionKey.Value, ((CurrencyUnitValue)o.Value).Number },
                                { ResolutionKey.Unit, ((CurrencyUnitValue)o.Value).Unit },
                                { ResolutionKey.IsoCurrency, ((CurrencyUnitValue)o.Value).IsoCurrency },
                            }
                            : (o.Value is UnitValue && !string.IsNullOrEmpty(o.Type) && Constants.ValidSubTypes.Contains(o.Type)) ? new SortedDictionary<string, object>
                            {
                                { ResolutionKey.Value, ((UnitValue)o.Value).Number },
                                { ResolutionKey.Unit, ((UnitValue)o.Value).Unit },
                                { ResolutionKey.SubType, o.Type },
                            }
                            : (o.Value is UnitValue) ? new SortedDictionary<string, object>
                            {
                                { ResolutionKey.Value, ((UnitValue)o.Value).Number },
                                { ResolutionKey.Unit, ((UnitValue)o.Value).Unit },
                            }
                            : new SortedDictionary<string, object>
                            {
                                { ResolutionKey.Value, (string)o.Value },
                            },
                        Text = o.Text,
                        TypeName = ModelTypeName,
                    }).ToList();

                    foreach (var result in modelResults)
                    {
                        bool shouldAdd = true;

                        foreach (var extractionResult in extractionResults)
                        {
                            if (result.Start <= extractionResult.Start && result.End >= extractionResult.End)
                            {
                                shouldAdd = false;
                            }
                        }

                        if (shouldAdd)
                        {
                            extractionResults.Add(result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Nothing to do. Exceptions in parse should not break users of recognizers.
                // No result.
            }

            return extractionResults;
        }

        public void SetCultureInfo(string culture, string requestedCulture = null)
        {
            this.culture = culture;
            this.requestedCulture = requestedCulture;
        }
    }
}
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Sequence
{
    public abstract class AbstractSequenceModel : IModel
    {

        private string culture;

        private string requestedCulture;

        protected AbstractSequenceModel(IParser parser, IExtractor extractor)
        {
            this.Parser = parser;
            this.Extractor = extractor;
        }

        public abstract string ModelTypeName { get; }

        public string Culture => this.culture;

        public string RequestedCulture => this.requestedCulture;

        protected IExtractor Extractor { get; private set; }

        protected IParser Parser { get; private set; }

        public virtual List<ModelResult> Parse(string query)
        {
            var parsedSequences = new List<ParseResult>();

            try
            {
                var extractResults = Extractor.Extract(query);

                foreach (var result in extractResults)
                {
                    parsedSequences.Add(Parser.Parse(result));
                }
            }
            catch (Exception)
            {
                // Nothing to do. Exceptions in parse should not break users of recognizers.
                // No result.
            }

            return parsedSequences.Select(o => new ModelResult
            {
                Start = o.Start.Value,
                End = o.Start.Value + o.Length.Value - 1,
                Resolution = new SortedDictionary<string, object> { { ResolutionKey.Value, o.ResolutionStr } },
                Text = o.Text,
                TypeName = ModelTypeName,
            }).ToList();
        }

        public void SetCultureInfo(string culture, string requestedCulture = null)
        {
            this.culture = culture;
            this.requestedCulture = requestedCulture;
        }
    }
}
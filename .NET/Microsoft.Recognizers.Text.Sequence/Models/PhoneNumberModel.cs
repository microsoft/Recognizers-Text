﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class PhoneNumberModel : AbstractSequenceModel
    {
        public PhoneNumberModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_PHONE_NUMBER;

        public override List<ModelResult> Parse(string query)
        {
            var parsedSequences = new List<ParseResult>();

            // Preprocess the query
            query = QueryProcessor.Preprocess(query);

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
                Resolution = new SortedDictionary<string, object>
                {
                    {
                        ResolutionKey.Value, o.ResolutionStr
                    },
                    {
                        ResolutionKey.Score, o.Value.ToString()
                    },
                },
                Text = o.Text,
                TypeName = ModelTypeName,
            }).ToList();
        }
    }
}
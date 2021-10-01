﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Choice
{
    public class BooleanModel : ChoiceModel
    {
        public BooleanModel(IParser parser, IExtractor extractor)
               : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_BOOLEAN;

        protected override SortedDictionary<string, object> GetResolution(ParseResult parseResult)
        {
            var data = parseResult.Data as ChoiceParseDataResult;
            var results = new SortedDictionary<string, object>()
            {
                { "value", parseResult.Value },
                { "score", data.Score },
                {
                    "otherResults", data.OtherMatches.Select(l => new
                    {
                        Text = l.Text,
                        Value = l.Value,
                        Score = l.Score,
                    })
                },
            };

            return results;
        }
    }
}

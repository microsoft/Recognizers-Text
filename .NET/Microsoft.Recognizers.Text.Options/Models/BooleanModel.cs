﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Recognizers.Text.Options.Parsers;

namespace Microsoft.Recognizers.Text.Options.Models
{
    public class BooleanModel : OptionsModel
    {
        public BooleanModel(IParser parser, IExtractor extractor) : base(parser,extractor)
        {

        }

        public override string ModelTypeName => Constants.MODEL_BOOLEAN;

        protected override SortedDictionary<string, object> GetResolution(ParseResult parseResult)
        {
            var data = parseResult.Data as OptionsParseDataResult;
            var results = new SortedDictionary<string, object>()
            {
                { "value", parseResult.Value },
                { "score", data.Score },
                { "otherResults", data.OtherMatches.Select(l => new
                    {
                        Text = l.Text,
                        Value = l.Value,
                        Score = l.Score
                    } 
                )},

            };
            

            return results;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class IpAddressModel : AbstractSequenceModel
    {
        public IpAddressModel(IParser parser, IExtractor extractor)
            : base(parser, extractor)
        {
        }

        public override string ModelTypeName => Constants.MODEL_IP;

        public override List<ModelResult> Parse(string query)
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

            return parsedSequences.Where(o => o.Data != null).Select(o => new ModelResult
            {
                Start = o.Start.Value,
                End = o.Start.Value + o.Length.Value - 1,
                Resolution = new SortedDictionary<string, object>
                {
                    { ResolutionKey.Value, o.ResolutionStr },
                    { ResolutionKey.Type, o.Data },
                },
                Text = o.Text,
                TypeName = ModelTypeName,
            }).ToList();
        }
    }
}
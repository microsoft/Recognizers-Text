using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeModel : IModel
    {
        public string ModelTypeName => "datetime";
        
        protected IDateTimeExtractor Extractor { get; private set; }

        protected IDateTimeParser Parser { get; private set; }

        public DateTimeModel(IDateTimeParser parser, IDateTimeExtractor extractor)
        {
            this.Parser = parser;
            this.Extractor = extractor;
        }

        public List<ModelResult> Parse(string query)
        {
            return this.Parse(query, System.DateTime.Now);
        }

        public List<ModelResult> Parse(string query, System.DateTime refTime)
        {
            // preprocess the query
            query = FormatUtility.Preprocess(query);
            var extractResults = Extractor.Extract(query, refTime);

            var parseDateTimes = new List<DateTimeParseResult>();
            foreach (var result in extractResults)
            {
                var parseResult = Parser.Parse(result, refTime);
                if (parseResult.Value is List<DateTimeParseResult>)
                {
                    parseDateTimes.AddRange((List<DateTimeParseResult>)parseResult.Value);
                }
                else
                {
                    parseDateTimes.Add(parseResult);
                }
            }

            return parseDateTimes.Select(o => new ModelResult
            {
                Start = o.Start.Value,
                End = o.Start.Value + o.Length.Value - 1,
                TypeName = o.Type,
                Resolution = o.Value as SortedDictionary<string, object>,
                Text = o.Text
            }).ToList();
        }
    }
}
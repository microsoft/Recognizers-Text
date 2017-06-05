using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeModel : IModel
    {
        public string ModelTypeName => "datetime";
        
        protected IExtractor Extractor { get; private set; }
        protected IDateTimeParser Parser { get; private set; }

        public DateTimeModel(IDateTimeParser parser, IExtractor extractor)
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
            var extractResults = Extractor.Extract(query);
            var parseDateTimes = new List<DateTimeParseResult>();
            foreach (var result in extractResults)
            {
                parseDateTimes.Add(Parser.Parse(result, refTime));
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
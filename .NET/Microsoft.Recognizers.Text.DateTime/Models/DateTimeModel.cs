using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<object> Parse(string query)
        {
            return this.Parse(query, System.DateTime.Now);
        }

        public List<object> Parse(string query, System.DateTime refTime)
        {
            // Preprocess the query
            query = FormatUtility.Preprocess(query);

            var parsedDateTimes = new List<DateTimeParseResult>();

            try {

                var extractResults = Extractor.Extract(query, refTime);

                foreach (var result in extractResults)
                {
                    var parseResult = Parser.Parse(result, refTime);
                    if (parseResult.Value is List<DateTimeParseResult>)
                    {
                        parsedDateTimes.AddRange((List<DateTimeParseResult>)parseResult.Value);
                    }
                    else
                    {
                        parsedDateTimes.Add(parseResult);
                    }
                }
            }
            catch (Exception)
            { 
                // Nothing to do. Exceptions in parse should not break users of recognizers.
                // No result.
            }

            return parsedDateTimes.Select(o => GetModelResult(o)).ToList();
        }

        private object GetModelResult(DateTimeParseResult parsedDateTime)
        {
            var type = parsedDateTime.Type.Split('.').Last();
            if (type.Equals(Constants.SYS_DATETIME_DATETIMEALT))
            {
                return new DateTimeAltModelResult
                {
                    Start = parsedDateTime.Start.Value,
                    End = parsedDateTime.Start.Value + parsedDateTime.Length.Value - 1,
                    TypeName = parsedDateTime.Type,
                    Resolution = parsedDateTime.Value as SortedDictionary<string, object>,
                    Text = parsedDateTime.Text,
                    ParentText = ((Dictionary<string, object>)(parsedDateTime.Data))[Constants.ParentText].ToString()
                };
            }
            else
            {
                return new ModelResult
                {
                    Start = parsedDateTime.Start.Value,
                    End = parsedDateTime.Start.Value + parsedDateTime.Length.Value - 1,
                    TypeName = parsedDateTime.Type,
                    Resolution = parsedDateTime.Value as SortedDictionary<string, object>,
                    Text = parsedDateTime.Text
                };
            }
        }

    }

    public class DateTimeAltModelResult : ModelResult
    {
        public string ParentText { get; set; }
    }
}
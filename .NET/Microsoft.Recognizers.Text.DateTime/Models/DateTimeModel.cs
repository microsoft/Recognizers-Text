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

        public List<ModelResult> Parse(string query)
        {
            return this.Parse(query, System.DateTime.Now);
        }

        public List<ModelResult> Parse(string query, System.DateTime refTime)
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

        private ModelResult GetModelResult(DateTimeParseResult parsedDateTime)
        {
            ModelResult ret;
            var modelResult = new ModelResult
            {
                Start = parsedDateTime.Start.Value,
                End = parsedDateTime.Start.Value + parsedDateTime.Length.Value - 1,
                TypeName = parsedDateTime.Type,
                Resolution = parsedDateTime.Value as SortedDictionary<string, object>,
                Text = parsedDateTime.Text
            };

            var type = parsedDateTime.Type.Split('.').Last();
            if (type.Equals(Constants.SYS_DATETIME_DATETIMEALT))
            {
                ret = new ExtendedModelResult(modelResult)
                {
                    ParentText = GetParentText(parsedDateTime)
                };
            }
            else
            {
                ret = modelResult;
            }

            return ret;
        }

        private string GetParentText(DateTimeParseResult parsedDateTime)
        {
            return ((Dictionary<string, object>)(parsedDateTime.Data))[Constants.ParentText].ToString();
        }

    }
}
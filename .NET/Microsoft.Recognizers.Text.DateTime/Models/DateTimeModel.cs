using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeModel : IModel
    {
        private string culture;

        private string requestedCulture;

        public DateTimeModel(IDateTimeParser parser, IDateTimeExtractor extractor)
        {
            this.Parser = parser;
            this.Extractor = extractor;
        }

        public string ModelTypeName => Constants.MODEL_DATETIME;

        public string Culture => this.culture;

        public string RequestedCulture => this.requestedCulture;

        protected IDateTimeExtractor Extractor { get; }

        protected IDateTimeParser Parser { get; }

        public List<ModelResult> Parse(string query)
        {
            return this.Parse(query, DateObject.Now);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design")]
        public List<ModelResult> Parse(string query, DateObject refTime)
        {
            var parsedDateTimes = new List<DateTimeParseResult>();

            // Preprocess the query
            query = QueryProcessor.Preprocess(query);

            try
            {
                var extractResults = Extractor.Extract(query, refTime);

                foreach (var result in extractResults)
                {
                    var parseResults = Parser.Parse(result, refTime);

                    if (parseResults.Value is List<DateTimeParseResult> list)
                    {
                        parsedDateTimes.AddRange(list);
                    }
                    else
                    {
                        parsedDateTimes.Add(parseResults);
                    }
                }

                // Filter out ambiguous cases. Naïve approach.
                parsedDateTimes = Parser.FilterResults(query, parsedDateTimes);
            }
            catch (Exception)
            {
                // Nothing to do. Exceptions in parse should not break users of recognizers.
                // No result.
            }

            return parsedDateTimes.Select(o => GetModelResult(o)).ToList();
        }

        public void SetCultureInfo(string culture, string requestedCulture = null)
        {
            this.culture = culture;
            this.requestedCulture = requestedCulture;
        }

        private static string GetParentText(DateTimeParseResult parsedDateTime)
        {
            return ((Dictionary<string, object>)parsedDateTime.Data)[ExtendedModelResult.ParentTextKey].ToString();
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
                Text = parsedDateTime.Text,
            };

            var type = parsedDateTime.Type.Split('.').Last();
            if (type.Equals(Constants.SYS_DATETIME_DATETIMEALT, StringComparison.Ordinal))
            {
                ret = new ExtendedModelResult(modelResult)
                {
                    ParentText = GetParentText(parsedDateTime),
                };
            }
            else
            {
                ret = modelResult;
            }

            return ret;
        }

    }
}
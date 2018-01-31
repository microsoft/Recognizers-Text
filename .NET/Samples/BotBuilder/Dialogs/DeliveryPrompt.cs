namespace BotBuilderRecognizerSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Connector;
    using Microsoft.Recognizers.Text.DateTime;

    [Serializable]
    public class DeliveryPrompt : Prompt<IEnumerable<DateTime>, DateTime>
    {
        public const string DeliveryPromptMessage =
            "When do you want to receive the delivery?\n\n" +
            "Some valid options are:\n\n" +
            " - Tomorrow morning\n\n" +
            " - 12/30/2017\n\n" +
            " - 9PM Tomorrow\n\n" +
            " - Five hours from now\n\n" +
            " - From 9AM to 10AM tomorrow";

        public const string PastValueErrorMessage =
            "I'm sorry, but I need at least an hour to deliver.\n\n $moment$ is no good for me.\n\nWhat other moment suits you best?";

        private readonly string culture;

        public DeliveryPrompt(string culture) : base(new PromptOptions<DateTime>(DeliveryPromptMessage, attempts: 5))
        {
            this.culture = culture;
        }

        protected override bool TryParse(IMessageActivity message, out IEnumerable<DateTime> result)
        {
            var extraction = ValidateAndExtract(message.Text, this.culture);
            if (!extraction.IsValid)
            {
                this.promptOptions.DefaultRetry = extraction.ErrorMessage;
            }

            result = extraction.Values;
            return extraction.IsValid;
        }

        public static Extraction ValidateAndExtract(string input, string culture)
        {
            // Get DateTime model for the specified culture
            var model = DateTimeRecognizer.GetSingleCultureInstance(culture).GetDateTimeModel();
            var results = model.Parse(input);

            // Check there are valid results
            if (results.Count > 0 && results.First().TypeName.StartsWith("datetimeV2"))
            {
                // The DateTime model can return several resolution types (https://github.com/Microsoft/Recognizers-Text/blob/master/.NET/Microsoft.Recognizers.Text.DateTime/Constants.cs#L7-L14)
                // We only care for those with a date, date and time, or date time period:
                // date, daterange, datetime, datetimerange

                var first = results.First();
                var resolutionValues = (IList<Dictionary<string, string>>)first.Resolution["values"];

                var subType = first.TypeName.Split('.').Last();
                if (subType.Contains("date") && !subType.Contains("range"))
                {
                    // a date (or date & time) or multiple
                    var moment = resolutionValues.Select(v => DateTime.Parse(v["value"])).FirstOrDefault();
                    if (IsFuture(moment))
                    {
                        // a future moment, valid!
                        return new Extraction
                        {
                            IsValid = true,
                            Values = new[] { moment }
                        };
                    }

                    // a past moment
                    return new Extraction
                    {
                        IsValid = false,
                        Values = new[] { moment },
                        ErrorMessage = PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(moment))
                    };
                }
                else if (subType.Contains("date") && subType.Contains("range"))
                {
                    // range
                    var from = DateTime.Parse(resolutionValues.First()["start"]);
                    var to = DateTime.Parse(resolutionValues.First()["end"]);
                    if (IsFuture(from) && IsFuture(to))
                    {
                        // future
                        return new Extraction
                        {
                            IsValid = true,
                            Values = new[] { from, to }
                        };
                    }

                    var values = new[] { from, to };
                    return new Extraction
                    {
                        IsValid = false,
                        Values = values,
                        ErrorMessage = PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(values))
                    };
                }
            }

            return new Extraction
            {
                IsValid = false,
                Values = Enumerable.Empty<DateTime>(),
                ErrorMessage = "I'm sorry, that doesn't seem to be a valid delivery date and time"
            };
        }

        public static bool IsFuture(DateTime date)
        {
            // at least one hour
            return date > DateTime.Now.AddHours(1);
        }

        public static string MomentOrRangeToString(IEnumerable<DateTime> moments, string momentPrefix = "on ")
        {
            if (moments.Count() == 1)
            {
                return MomentOrRangeToString(moments.First(), momentPrefix);
            }

            return "from " + string.Join(" to ", moments.Select(m => MomentOrRangeToString(m, "")));
        }

        public static string MomentOrRangeToString(DateTime moment, string momentPrefix = "on ")
        {
            return momentPrefix + moment.ToString();
        }

        public class Extraction
        {
            public bool IsValid { get; set; }

            public IEnumerable<DateTime> Values { get; set; }

            public string ErrorMessage { get; set; }
        }
    }
}

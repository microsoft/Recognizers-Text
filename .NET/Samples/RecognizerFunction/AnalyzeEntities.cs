using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using Newtonsoft.Json;

using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.Recognizers.Text.Sequence;
using System.Net.Http.Formatting;

namespace RecognizerFunction
{
    public static class AnalyzeEntities
    {

        private const string DefaultCulture = Culture.English;

        [FunctionName("entities")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# AnalyzeEntities function processed a request.");

            // Parse query parameter
            string text = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "text", true) == 0).Value;
            string cultureCode = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "culture", true) == 0).Value;

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // Set name to query string or body data
            text = text ?? data?.text;
            cultureCode = cultureCode ?? data?.cultureCode;

            if (text == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a text on the query string or in the request body", JsonMediaTypeFormatter.DefaultMediaType);
            }

            string culture = ResolveCulture(cultureCode);

            // Retrieve all the parsers and call 'Parse' to recognize all the values from the user input
            var results = ParseAll(text, culture);

            // Log output
            log.Info(results.Any() ? string.Format("I found {0:d} entities in culture {1}:", results.Count(), culture) : string.Format("I found no entities in culture {0}.", culture));
            results.ToList().ForEach(result => log.Info(JsonConvert.SerializeObject(result, Formatting.Indented)));

            // Return array
            return req.CreateResponse(HttpStatusCode.OK, results, JsonMediaTypeFormatter.DefaultMediaType);
        }

        /// <summary>
        /// Parse query with all recognizers
        /// </summary>
        private static IEnumerable<ModelResult> ParseAll(string query, string culture)
        {
            return MergeResults(
                // Number recognizer will find any number from the input
                // E.g "I have two apples" will return "2".
                NumberRecognizer.RecognizeNumber(query, culture),

                // Ordinal number recognizer will find any ordinal number
                // E.g "eleventh" will return "11".
                NumberRecognizer.RecognizeOrdinal(query, culture),

                // Percentage recognizer will find any number presented as percentage
                // E.g "one hundred percents" will return "100%"
                NumberRecognizer.RecognizePercentage(query, culture),

                // Number Range recognizer will find any cardinal or ordinal number range
                // E.g. "between 2 and 5" will return "(2,5)"
                NumberRecognizer.RecognizeNumberRange(query, culture),

                // Age recognizer will find any age number presented
                // E.g "After ninety five years of age, perspectives change" will return "95 Year"
                NumberWithUnitRecognizer.RecognizeAge(query, culture),

                // Currency recognizer will find any currency presented
                // E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
                NumberWithUnitRecognizer.RecognizeCurrency(query, culture),

                // Dimension recognizer will find any dimension presented
                // E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
                NumberWithUnitRecognizer.RecognizeDimension(query, culture),

                // Temperature recognizer will find any temperature presented
                // E.g "Set the temperature to 30 degrees celsius" will return "30 C"
                NumberWithUnitRecognizer.RecognizeTemperature(query, culture),

                // Datetime recognizer This model will find any Date even if its write in coloquial language 
                // E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
                DateTimeRecognizer.RecognizeDateTime(query, culture),

                // PhoneNumber recognizer will find any phone number presented
                // E.g "My phone number is ( 19 ) 38294427."
                SequenceRecognizer.RecognizePhoneNumber(query, culture),

                // Add IP recognizer - This recognizer will find any Ipv4/Ipv6 presented
                // E.g "My Ip is 8.8.8.8"
                SequenceRecognizer.RecognizeIpAddress(query, culture),

                // Mention recognizer will find all the mention usages
                // E.g "@Cicero"
                SequenceRecognizer.RecognizeMention(query, culture),

                // Hashtag recognizer will find all the hash tag usages
                // E.g "task #123"
                SequenceRecognizer.RecognizeHashtag(query, culture),

                // Email recognizer will find all the emails
                // E.g "a@b.com"
                SequenceRecognizer.RecognizeEmail(query, culture),

                // URL recognizer will find all the urls
                // E.g "bing.com"
                SequenceRecognizer.RecognizeURL(query, culture),

                // Add Boolean recognizer - This model will find yes/no like responses, including emoji -
                // E.g "yup, I need that" will return "True"
                ChoiceRecognizer.RecognizeBoolean(query, culture)
            );
        }

        private static IEnumerable<ModelResult> MergeResults(params List<ModelResult>[] results)
        {
            return results.SelectMany(o => o);
        }

        private static string ResolveCulture(string cultureCode)
        {
            string[] supportedCultures = Culture.GetSupportedCultureCodes();

            if (cultureCode == null)
            {
                return DefaultCulture;
            }

            if (supportedCultures.Contains(cultureCode))
            {
                return cultureCode;
            } 
            else
            {
                return Culture.MapToNearestLanguage(cultureCode);
            }

        }

    }

}
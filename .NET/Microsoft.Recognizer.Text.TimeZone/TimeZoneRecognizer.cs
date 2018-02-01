using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Recognizers.Text.TimeZone.English;

namespace Microsoft.Recognizers.Text.TimeZone
{
    public class TimeZoneRecognizer : Recognizer
    {
        public static readonly TimeZoneRecognizer Instance = new TimeZoneRecognizer(TimeZoneOptions.None);
        private TimeZoneRecognizer(TimeZoneOptions options)
        {
            RegisterModel(Culture.English, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(TimeZoneModel)] = new TimeZoneModel(
                            new TimeZoneParser(),
                            English.TimeZoneExtractor.GetInstance())
            });
        }
        public IModel GetTimeZoneModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<TimeZoneModel>(culture, fallbackToDefaultCulture, TimeZoneOptions.None.ToString());
        }
    }
}

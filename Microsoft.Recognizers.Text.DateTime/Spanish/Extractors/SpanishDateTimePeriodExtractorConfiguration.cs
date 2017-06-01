using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Extractors;
using Microsoft.Recognizers.Text.Number.Spanish.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Extractors
{
    public class SpanishDateTimePeriodExtractorConfiguration : IDateTimePeriodExtractorConfiguration
    {
        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\.\d*)?)\s*{SpanishTimePeriodExtractorConfiguration.UnitRegex}\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex fromRegex = new Regex(@"((desde|de)(\s*la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex andRegex = new Regex(@"(y\s*(la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex beforeRegex = new Regex(@"(entre\s*(la(s)?)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishDateTimePeriodExtractorConfiguration()
        {
            CardinalExtractor = new CardinalExtractor();
            SingleDateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            SingleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
        }

        public IExtractor CardinalExtractor { get; }

        public IExtractor SingleDateExtractor { get; }

        public IExtractor SingleTimeExtractor { get; }

        public IExtractor SingleDateTimeExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[] 
            {
                SpanishTimePeriodExtractorConfiguration.PureNumFromTo,
                SpanishTimePeriodExtractorConfiguration.PureNumBetweenAnd
            };

        public Regex PrepositionRegex => SpanishDateTimeExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => SpanishDatePeriodExtractorConfiguration.TillRegex;

        public Regex SpecificNightRegex => SpanishDateTimeExtractorConfiguration.SpecificNightRegex;

        public Regex NightRegex => SpanishDateTimeExtractorConfiguration.NightRegex;

        public Regex FollowedUnit => SpanishTimePeriodExtractorConfiguration.FollowedUnit;

        public Regex UnitRegex => SpanishTimePeriodExtractorConfiguration.UnitRegex;

        public Regex PastRegex => SpanishDatePeriodExtractorConfiguration.PastRegex;

        public Regex FutureRegex => SpanishDatePeriodExtractorConfiguration.FutureRegex;

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            var fromMatch = fromRegex.Match(text);
            if (fromMatch.Success)
            {
                index = fromMatch.Index;
            }
            return fromMatch.Success;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var beforeMatch = beforeRegex.Match(text);
            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            return beforeMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return andRegex.IsMatch(text);
        }
    }
}

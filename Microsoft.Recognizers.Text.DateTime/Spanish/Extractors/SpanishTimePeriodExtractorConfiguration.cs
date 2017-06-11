using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimePeriodExtractorConfiguration : ITimePeriodExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; //"TimePeriod";

        public static readonly Regex HourNumRegex =
            new Regex(
                @"(?<hour>veintiuno|veintidos|veintitres|veinticuatro|cero|uno|dos|tres|cuatro|cinco|seis|siete|ocho|nueve|diez|once|doce|trece|catorce|quince|diecis([eé])is|diecisiete|dieciocho|diecinueve|veinte)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumFromTo =
            new Regex(
                $@"((desde|de)\s+(la(s)?\s+)?)?({BaseTimeExtractor.HourRegex}|{HourNumRegex})(\s*{SpanishTimeExtractorConfiguration.DescRegex})?\s*{SpanishDatePeriodExtractorConfiguration.TillRegex}\s*({BaseTimeExtractor.HourRegex}|{HourNumRegex})\s*({SpanishTimeExtractorConfiguration.PmRegex}|{SpanishTimeExtractorConfiguration.AmRegex}|{SpanishTimeExtractorConfiguration.DescRegex})?",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumBetweenAnd =
            new Regex(
                $@"(entre\s+(la(s)?\s+)?)({BaseTimeExtractor.HourRegex}|{HourNumRegex})(\s*{SpanishTimeExtractorConfiguration.DescRegex})?\s*y\s*(la(s)?\s+)?({BaseTimeExtractor.HourRegex}|{HourNumRegex})\s*({SpanishTimeExtractorConfiguration.PmRegex}|{SpanishTimeExtractorConfiguration.AmRegex}|{SpanishTimeExtractorConfiguration.DescRegex})?",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex =
            new Regex(@"(?<unit>horas|hora|h|minutos|minuto|mins|min|segundos|segundo|secs|sec)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit = 
            new Regex($@"\b(?<num>\d+(\,\d*)?)\s*{UnitRegex}\b", 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex fromRegex = new Regex(@"((desde|de)(\s*la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex andRegex = new Regex(@"(y\s*(la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex beforeRegex = new Regex(@"(entre\s*(la(s)?)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishTimePeriodExtractorConfiguration()
        {
            SingleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
        }


        public IExtractor SingleTimeExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd };

        Regex ITimePeriodExtractorConfiguration.TillRegex => SpanishDatePeriodExtractorConfiguration.TillRegex;

        Regex ITimePeriodExtractorConfiguration.NightRegex => SpanishDateTimeExtractorConfiguration.NightRegex;

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

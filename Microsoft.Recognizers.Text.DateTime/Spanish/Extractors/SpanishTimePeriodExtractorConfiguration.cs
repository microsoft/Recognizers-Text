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
                $@"((desde|de)\s+(la(s)?\s+)?)?({BaseTimeExtractor.HourRegex}|{HourNumRegex})(\s*(?<left_desc>{SpanishTimeExtractorConfiguration.DescRegex}))?\s*{SpanishDatePeriodExtractorConfiguration.TillRegex}\s*({BaseTimeExtractor.HourRegex}|{HourNumRegex})\s*(?<right_desc>{SpanishTimeExtractorConfiguration.PmRegex}|{SpanishTimeExtractorConfiguration.AmRegex}|{SpanishTimeExtractorConfiguration.DescRegex})?",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumBetweenAnd =
            new Regex(
                $@"(entre\s+(la(s)?\s+)?)({BaseTimeExtractor.HourRegex}|{HourNumRegex})(\s*(?<left_desc>{SpanishTimeExtractorConfiguration.DescRegex}))?\s*y\s*(la(s)?\s+)?({BaseTimeExtractor.HourRegex}|{HourNumRegex})\s*(?<right_desc>{SpanishTimeExtractorConfiguration.PmRegex}|{SpanishTimeExtractorConfiguration.AmRegex}|{SpanishTimeExtractorConfiguration.DescRegex})?",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex =
            new Regex(@"(?<unit>horas|hora|h|minutos|minuto|mins|min|segundos|segundo|secs|sec)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit = 
            new Regex($@"\b(?<num>\d+(\,\d*)?)\s*{UnitRegex}", 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex FromRegex = new Regex(@"((desde|de)(\s*la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(@"(y\s*(la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex BeforeRegex = new Regex(@"(entre\s*(la(s)?)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

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
            var fromMatch = FromRegex.Match(text);
            if (fromMatch.Success)
            {
                index = fromMatch.Index;
            }
            return fromMatch.Success;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var beforeMatch = BeforeRegex.Match(text);
            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            return beforeMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}

using System;

using Microsoft.Recognizers.Resources.English;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishTimePeriodExtractorConfiguration : ITimePeriodExtractorConfiguration
    {
        public static readonly Regex TillRegex = new Regex(DateTimeDefinition.TillRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourRegex =
            new Regex(
                DateTimeDefinition.HourRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PeriodHourNumRegex =
            new Regex(
                DateTimeDefinition.PeriodHourNumRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PeriodDescRegex = new Regex(DateTimeDefinition.PeriodDescRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinition.PmRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmRegex = new Regex(DateTimeDefinition.AmRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumFromTo =
            new Regex(
                DateTimeDefinition.PureNumFromTo,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumBetweenAnd =
            new Regex(
                DateTimeDefinition.PureNumBetweenAnd, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinition.PrepositionRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NightRegex =
            new Regex(DateTimeDefinition.NightRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecificNightRegex =
            new Regex(DateTimeDefinition.SpecificNightRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinition.TimeUnitRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeFollowedUnit = new Regex(DateTimeDefinition.TimeFollowedUnit,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeNumberCombinedWithUnit =
            new Regex(DateTimeDefinition.TimeNumberCombinedWithUnit, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinition.PastRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinition.FutureRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishTimePeriodExtractorConfiguration()
        {
            SingleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        }

        public IExtractor SingleTimeExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[]{ PureNumFromTo, PureNumBetweenAnd };

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.NightRegex => NightRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("from"))
            {
                index = text.LastIndexOf("from", StringComparison.Ordinal);
                return true;
            }
            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("between"))
            {
                index = text.LastIndexOf("between", StringComparison.Ordinal);
                return true;
            }
            return false;
        }

        public bool HasConnectorToken(string text)
        {
            return text.Equals("and");
        }
    }
}
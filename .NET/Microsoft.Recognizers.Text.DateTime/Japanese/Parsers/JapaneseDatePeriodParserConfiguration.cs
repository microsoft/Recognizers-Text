using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Japanese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDatePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDatePeriodParserConfiguration
    {

        public static readonly Regex WoMLastRegex = new Regex(DateTimeDefinitions.WoMLastRegex, RegexFlags);
        public static readonly Regex WoMPreviousRegex = new Regex(DateTimeDefinitions.WoMPreviousRegex, RegexFlags);
        public static readonly Regex WoMNextRegex = new Regex(DateTimeDefinitions.WoMNextRegex, RegexFlags);

        public static readonly ImmutableDictionary<string, int> MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseDatePeriodParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            NumberParser = config.NumberParser;
            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DateParser = config.DateParser;

            DynastyYearRegex = JapaneseDateExtractorConfiguration.DynastyYearRegex;
            DynastyStartYear = JapaneseDateExtractorConfiguration.DynastyStartYear;
            DynastyYearMap = JapaneseDateExtractorConfiguration.DynastyYearMap;
            SimpleCasesRegex = JapaneseDatePeriodExtractorConfiguration.SimpleCasesRegex;
            ThisRegex = JapaneseDatePeriodExtractorConfiguration.ThisRegex;
            NextRegex = JapaneseDatePeriodExtractorConfiguration.NextRegex;
            LastRegex = JapaneseDatePeriodExtractorConfiguration.LastRegex;
            YearToYear = JapaneseDatePeriodExtractorConfiguration.YearToYear;
            YearToYearSuffixRequired = JapaneseDatePeriodExtractorConfiguration.YearToYearSuffixRequired;
            YearRegex = JapaneseDatePeriodExtractorConfiguration.YearRegex;
            YearInCJKRegex = JapaneseDatePeriodExtractorConfiguration.YearInCJKRegex;
            MonthToMonth = JapaneseDatePeriodExtractorConfiguration.MonthToMonth;
            MonthToMonthSuffixRequired = JapaneseDatePeriodExtractorConfiguration.MonthToMonthSuffixRequired;
            DayToDay = JapaneseDatePeriodExtractorConfiguration.DayToDay;
            DayRegexForPeriod = JapaneseDatePeriodExtractorConfiguration.DayRegexForPeriod;
            MonthRegex = JapaneseDatePeriodExtractorConfiguration.MonthRegex;
            SpecialMonthRegex = JapaneseDatePeriodExtractorConfiguration.SpecialMonthRegex;
            SpecialYearRegex = JapaneseDatePeriodExtractorConfiguration.SpecialYearRegex;
            YearAndMonth = JapaneseDatePeriodExtractorConfiguration.YearAndMonth;
            PureNumYearAndMonth = JapaneseDatePeriodExtractorConfiguration.PureNumYearAndMonth;
            SimpleYearAndMonth = JapaneseDatePeriodExtractorConfiguration.SimpleYearAndMonth;
            OneWordPeriodRegex = JapaneseDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            NumberCombinedWithUnit = JapaneseDatePeriodExtractorConfiguration.NumberCombinedWithUnit;
            PastRegex = JapaneseDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = JapaneseDatePeriodExtractorConfiguration.FutureRegex;
            UnitRegex = JapaneseDatePeriodExtractorConfiguration.UnitRegex;
            WeekOfMonthRegex = JapaneseDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            SeasonWithYear = JapaneseDatePeriodExtractorConfiguration.SeasonWithYear;
            QuarterRegex = JapaneseDatePeriodExtractorConfiguration.QuarterRegex;
            DecadeRegex = JapaneseDatePeriodExtractorConfiguration.DecadeRegex;
            RelativeRegex = JapaneseDateExtractorConfiguration.RelativeRegex;
            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.ParserConfigurationCardinalMap.ToImmutableDictionary();
            DayOfMonth = DateTimeDefinitions.ParserConfigurationDayOfMonth.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.ParserConfigurationSeasonMap.ToImmutableDictionary();

        }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IExtractor IntegerExtractor { get; }

        public IParser NumberParser { get; }

        public ImmutableDictionary<string, int> DynastyYearMap { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        IImmutableDictionary<string, int> ICJKDatePeriodParserConfiguration.MonthOfYear => MonthOfYear;

        public IImmutableDictionary<string, string> SeasonMap { get; }

        public string DynastyStartYear { get; }

        public string TokenBeforeDate => string.Empty;

        public Regex DynastyYearRegex { get; }

        public Regex SimpleCasesRegex { get; }

        public Regex ThisRegex { get; }

        public Regex NextRegex { get; }

        public Regex LastRegex { get; }

        public Regex YearToYear { get; }

        public Regex YearToYearSuffixRequired { get; }

        public Regex YearRegex { get; }

        public Regex RelativeRegex { get; }

        public Regex YearInCJKRegex { get; }

        public Regex MonthToMonth { get; }

        public Regex MonthToMonthSuffixRequired { get; }

        public Regex MonthRegex { get; }

        public Regex YearAndMonth { get; }

        public Regex PureNumYearAndMonth { get; }

        public Regex OneWordPeriodRegex { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex PastRegex { get; }

        public Regex FutureRegex { get; }

        public Regex UnitRegex { get; }

        public Regex WeekOfMonthRegex { get; }

        public Regex SeasonWithYear { get; }

        public Regex QuarterRegex { get; }

        public Regex DecadeRegex { get; }

        public Regex DayToDay { get; }

        public Regex DayRegexForPeriod { get; }

        public Regex SimpleYearAndMonth { get; }

        public Regex SpecialMonthRegex { get; }

        public Regex SpecialYearRegex { get; }

        Regex ICJKDatePeriodParserConfiguration.WoMLastRegex => WoMLastRegex;

        Regex ICJKDatePeriodParserConfiguration.WoMPreviousRegex => WoMPreviousRegex;

        Regex ICJKDatePeriodParserConfiguration.WoMNextRegex => WoMNextRegex;

        public int TwoNumYear => int.Parse(DateTimeDefinitions.TwoNumYear, CultureInfo.InvariantCulture);

        public int ToMonthNumber(string monthStr)
        {
            return MonthOfYear[monthStr] > 12 ? MonthOfYear[monthStr] % 12 : MonthOfYear[monthStr];
        }

        public bool IsMonthOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.MonthTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsWeekend(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekendTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsWeekOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.WeekTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsYearOnly(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearTerms.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal));
        }

        public bool IsThisYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.ThisYearTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsLastYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.LastYearTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsNextYear(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.NextYearTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsYearAfterNext(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearAfterNextTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public bool IsYearBeforeLast(string text)
        {
            var trimmedText = text.Trim();
            return DateTimeDefinitions.YearBeforeLastTerms.Any(o => trimmedText.Equals(o, StringComparison.Ordinal));
        }

        public int GetSwiftMonth(string text)
        {
            // Current month: 今月
            var value = 0;

            // @TODO move hardcoded values to resources file

            if (text.Equals("来月", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.Equals("前月", StringComparison.Ordinal) ||
                     text.Equals("先月", StringComparison.Ordinal) ||
                     text.Equals("昨月", StringComparison.Ordinal) ||
                     text.Equals("先々月", StringComparison.Ordinal))
            {
                value = -1;
            }
            else if (text.Equals("再来月", StringComparison.Ordinal))
            {
                value = 2;
            }

            return value;
        }

        public int GetSwiftYear(string text)
        {
            // Current year: 今年
            var value = 0;

            // @TODO move hardcoded values to resources file

            if (text.Equals("来年", StringComparison.Ordinal) ||
                text.Equals("らいねん", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.Equals("昨年", StringComparison.Ordinal) ||
                     text.Equals("前年", StringComparison.Ordinal))
            {
                value = -1;
            }

            return value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDatePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDatePeriodParserConfiguration
    {

        public static readonly ImmutableDictionary<string, int> MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();

        public KoreanDatePeriodParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            NumberParser = config.NumberParser;
            DateExtractor = config.DateExtractor;
            DurationExtractor = config.DurationExtractor;
            DateParser = config.DateParser;

            DynastyYearRegex = KoreanDateExtractorConfiguration.DynastyYearRegex;
            DynastyStartYear = KoreanDateExtractorConfiguration.DynastyStartYear;
            DynastyYearMap = KoreanDateExtractorConfiguration.DynastyYearMap;
            SimpleCasesRegex = KoreanDatePeriodExtractorConfiguration.SimpleCasesRegex;
            ThisRegex = KoreanDatePeriodExtractorConfiguration.ThisRegex;
            NextRegex = KoreanDatePeriodExtractorConfiguration.NextRegex;
            LastRegex = KoreanDatePeriodExtractorConfiguration.LastRegex;
            YearToYear = KoreanDatePeriodExtractorConfiguration.YearToYear;
            YearToYearSuffixRequired = KoreanDatePeriodExtractorConfiguration.YearToYearSuffixRequired;
            YearRegex = KoreanDatePeriodExtractorConfiguration.YearRegex;
            YearInCJKRegex = KoreanDatePeriodExtractorConfiguration.YearInCJKRegex;
            MonthToMonth = KoreanDatePeriodExtractorConfiguration.MonthToMonth;
            MonthToMonthSuffixRequired = KoreanDatePeriodExtractorConfiguration.MonthToMonthSuffixRequired;
            MonthRegex = KoreanDatePeriodExtractorConfiguration.MonthRegex;
            YearAndMonth = KoreanDatePeriodExtractorConfiguration.YearAndMonth;
            PureNumYearAndMonth = KoreanDatePeriodExtractorConfiguration.PureNumYearAndMonth;
            OneWordPeriodRegex = KoreanDatePeriodExtractorConfiguration.OneWordPeriodRegex;
            NumberCombinedWithUnit = KoreanDatePeriodExtractorConfiguration.NumberCombinedWithUnit;
            PastRegex = KoreanDatePeriodExtractorConfiguration.PastRegex;
            FutureRegex = KoreanDatePeriodExtractorConfiguration.FutureRegex;
            UnitRegex = KoreanDatePeriodExtractorConfiguration.UnitRegex;
            WeekOfMonthRegex = KoreanDatePeriodExtractorConfiguration.WeekOfMonthRegex;
            SeasonWithYear = KoreanDatePeriodExtractorConfiguration.SeasonWithYear;
            QuarterRegex = KoreanDatePeriodExtractorConfiguration.QuarterRegex;
            DecadeRegex = KoreanDatePeriodExtractorConfiguration.DecadeRegex;
            RelativeRegex = KoreanDateExtractorConfiguration.RelativeRegex;
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
    }
}
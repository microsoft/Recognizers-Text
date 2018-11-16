using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
  public class JapaneseDateTimeParserConfiguration : BaseOptionsConfiguration, IFullDateTimeParserConfiguration
  {
    public JapaneseDateTimeParserConfiguration(DateTimeOptions options = DateTimeOptions.None) : base(options)
    {
      DateParser = new DateParser(this);
      TimeParser = new TimeParser(this);
      DateTimeParser = new DateTimeParser(this);
      DatePeriodParser = new DatePeriodParser(this);
      TimePeriodParser = new TimePeriodParser(this);
      DateTimePeriodParser = new DateTimePeriodParser(this);
      DurationParser = new DurationParser(this);
      GetParser = new SetParser(this);
      HolidayParser = new HolidayParser(this);
      UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
      UnitValueMap = DateTimeDefinitions.ParserConfigurationUnitValueMap.ToImmutableDictionary();
      SeasonMap = DateTimeDefinitions.ParserConfigurationSeasonMap.ToImmutableDictionary();
      SeasonValueMap = DateTimeDefinitions.ParserConfigurationSeasonValueMap.ToImmutableDictionary();
      CardinalMap = DateTimeDefinitions.ParserConfigurationCardinalMap.ToImmutableDictionary();
      DayOfMonth = DateTimeDefinitions.ParserConfigurationDayOfMonth.ToImmutableDictionary();
      DayOfWeek = DateTimeDefinitions.ParserConfigurationDayOfWeek.ToImmutableDictionary();
      MonthOfYear = DateTimeDefinitions.ParserConfigurationMonthOfYear.ToImmutableDictionary();
      Numbers = InitNumbers();
      DateRegexList = DateExtractor.DateRegexList;
      NextRegex = DateExtractor.NextRegex;
      ThisRegex = DateExtractor.ThisRegex;
      LastRegex = DateExtractor.LastRegex;
      StrictWeekDayRegex = DateExtractor.WeekDayRegex;
      WeekDayOfMonthRegex = DateExtractor.WeekDayOfMonthRegex;
      BeforeRegex = JapaneseMergedExtractor.BeforeRegex;
      AfterRegex = JapaneseMergedExtractor.AfterRegex;
      UntilRegex = JapaneseMergedExtractor.UntilRegex;
      SincePrefixRegex = JapaneseMergedExtractor.SincePrefixRegex;
      SinceSuffixRegex = JapaneseMergedExtractor.SinceSuffixRegex;
    }

    public int TwoNumYear => int.Parse(DateTimeDefinitions.TwoNumYear);

    public string LastWeekDayToken => DateTimeDefinitions.ParserConfigurationLastWeekDayToken;

    public string NextMonthToken => DateTimeDefinitions.ParserConfigurationNextMonthToken;

    public string LastMonthToken => DateTimeDefinitions.ParserConfigurationLastMonthToken;

    public string DatePrefix => DateTimeDefinitions.ParserConfigurationDatePrefix;

    #region internalParsers

    public IDateTimeParser DateParser { get; }

    public IDateTimeParser TimeParser { get; }

    public IDateTimeParser DateTimeParser { get; }

    public IDateTimeParser DatePeriodParser { get; }

    public IDateTimeParser TimePeriodParser { get; }

    public IDateTimeParser DateTimePeriodParser { get; }

    public IDateTimeParser DurationParser { get; }

    public IDateTimeParser GetParser { get; }

    public IDateTimeParser HolidayParser { get; }

    #endregion

    #region Dictionaries

    public ImmutableDictionary<string, string> UnitMap { get; }

    public ImmutableDictionary<string, long> UnitValueMap { get; }

    public ImmutableDictionary<string, string> SeasonMap { get; }

    public ImmutableDictionary<string, int> SeasonValueMap { get; }

    public ImmutableDictionary<string, int> CardinalMap { get; }

    public ImmutableDictionary<string, int> DayOfMonth { get; }

    public ImmutableDictionary<string, int> DayOfWeek { get; }

    public ImmutableDictionary<string, int> MonthOfYear { get; }

    public ImmutableDictionary<string, int> Numbers { get; }

    #endregion

    #region Regexes

    public IEnumerable<Regex> DateRegexList { get; }

    public Regex NextRegex { get; }

    public Regex ThisRegex { get; }

    public Regex LastRegex { get; }

    public Regex StrictWeekDayRegex { get; }

    public Regex WeekDayOfMonthRegex { get; }

    public Regex BeforeRegex { get; }

    public Regex AfterRegex { get; }

    public Regex UntilRegex { get; }

    public Regex SincePrefixRegex { get; }

    public Regex SinceSuffixRegex { get; }

    #endregion

    private static ImmutableDictionary<string, int> InitNumbers()
    {
      return new Dictionary<string, int>
      {
      }.ToImmutableDictionary();
    }

    public int GetSwiftDay(string text)
    {
      //Today: 今天, 今日, 最近, きょう, この日
      var value = 0;

      if (text.StartsWith("来") || text.Equals("あす") || text.Equals("あした") || text.Equals("明日"))
      {
        value = 1;
      }
      else if (text.StartsWith("昨") || text.Equals("きのう") || text.Equals("前日"))
      {
        value = -1;
      }
      else if (text.Equals("大后天") || text.Equals("大後天"))
      {
        value = 3;
      }
      else if (text.Equals("大前天"))
      {
        value = -3;
      }
      else if (text.Equals("后天") || text.Equals("後天") || text.Equals("明後日") || text.Equals("あさって"))
      {
        value = 2;
      }
      else if (text.Equals("前天") || text.Equals("一昨日") || text.Equals("二日前") || text.Equals("おととい"))
      {
        value = -2;
      }

      return value;
    }

    public int GetSwiftMonth(string text)
    {
      //Current month: 今月
      var value = 0;

      if (text.Equals("来月"))
      {
        value = 1;
      }
      else if (text.Equals("前月") || text.Equals("先月") || text.Equals("昨月") || text.Equals("先々月"))
      {
        value = -1;
      }
      else if (text.Equals("再来月"))
      {
        value = 2;
      }
      
      return value;
    }

    public int GetSwiftYear(string text)
    {
      //Current year: 今年
      var value = 0;

      if (text.Equals("来年") || text.Equals("らいねん"))
      {
        value = 1;
      }
      else if (text.Equals("昨年") || text.Equals("前年"))
      {
        value = -1;
      }

      return value;
    }
  }
}

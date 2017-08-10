namespace Microsoft.Recognizers.Resources.English
{
	using System;
	using System.Collections.Generic;

	public static class DateTimeDefinition
	{
		public const string TillRegex = @"(?<till>to|till|until|thru|through|--|-|—|——)";
		public const string AndRegex = @"(?<and>and|--|-|—|——)";
		public const string DayRegex = @"(?<day>01|02|03|04|05|06|07|08|09|10th|10|11th|11st|11|12nd|12th|12|13rd|13th|13|14th|14|15th|15|16th|16|17th|17|18th|18|19th|19|1st|1|20th|20|21st|21|22nd|22|23rd|23|24th|24|25th|25|26th|26|27th|27|28th|28|29th|29|2nd|2|30th|30|31st|31|3rd|3|4th|4|5th|5|6th|6|7th|7|8th|8|9th|9)";
		public const string MonthNumRegex = @"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)";
		public const string PeriodYearRegex = @"\b(?<year>19\d{2}|20\d{2})\b";
		public const string WeekDayRegex = @"(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tue|Wedn|Weds|Wed|Thurs|Thur|Thu|Fri|Sat|Sun)";
		public const string RelativeMonthRegex = @"(?<relmonth>(this|next|last)\s+month)";
		public const string EngMonthRegex = @"(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sept|Sep)";
		public static readonly string MonthSuffixRegex = $@"(?<msuf>(in\s+|of\s+|on\s+)?({RelativeMonthRegex}|{EngMonthRegex}))";
		public const string UnitRegex = @"(?<unit>years|year|months|month|weeks|week|days|day)\b";
		public const string PastRegex = @"(?<past>\b(past|last|previous)\b)";
		public const string FutureRegex = @"(?<past>\b(next|in)\b)";
		public static readonly string SimpleCasesRegex = $@"\b(from\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){PeriodYearRegex})?\b";
		public static readonly string MonthFrontSimpleCasesRegex = $@"\b{MonthSuffixRegex}\s+(from\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})((\s+|\s*,\s*){PeriodYearRegex})?\b";
		public static readonly string MonthFrontBetweenRegex = $@"\b{MonthSuffixRegex}\s+(between\s+)({DayRegex})\s*{AndRegex}\s*({DayRegex})((\s+|\s*,\s*){PeriodYearRegex})?\b";
		public static readonly string BetweenRegex = $@"\b(between\s+)({DayRegex})\s*{AndRegex}\s*({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){PeriodYearRegex})?\b";
		public static readonly string MonthWithYear = $@"\b((?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sep|Sept),?(\s+of)?\s+({PeriodYearRegex}|(?<order>next|last|this)\s+year))";
		public const string OneWordPeriodRegex = @"\b(((next|this|last)\s+)?(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sep|Sept)|(next|last|this)\s+(weekend|week|month|year)|weekend|(month|year) to date)\b";
		public static readonly string MonthNumWithYear = $@"({PeriodYearRegex}[/\-\.]{MonthNumRegex})|({MonthNumRegex}[/\-]{PeriodYearRegex})";
		public static readonly string WeekOfMonthRegex = $@"(?<wom>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+week\s+{MonthSuffixRegex})";
		public static readonly string WeekOfYearRegex = $@"(?<woy>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+week(\s+of)?\s+({PeriodYearRegex}|(?<order>next|last|this)\s+year))";
		public static readonly string FollowedUnit = $@"^\s*{UnitRegex}";
		public static readonly string NumberCombinedWithUnit = $@"\b(?<num>\d+(\.\d*)?){UnitRegex}";
		public static readonly string QuarterRegex = $@"(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th)\s+quarter(\s+of|\s*,\s*)?\s+({PeriodYearRegex}|(?<order>next|last|this)\s+year)";
		public static readonly string QuarterRegexYearFront = $@"({PeriodYearRegex}|(?<order>next|last|this)\s+year)\s+(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th)\s+quarter";
		public static readonly string SeasonRegex = $@"\b(?<season>((last|this|the|next)\s+)?(?<seas>spring|summer|fall|autumn|winter)((\s+of|\s*,\s*)?\s+({PeriodYearRegex}|(?<order>next|last|this)\s+year))?)\b";
		public const string WhichWeekRegex = @"(week)(\s*)(?<number>\d\d|\d|0\d)";
		public const string WeekOfRegex = @"(week)(\s*)(of)";
		public const string MonthOfRegex = @"(month)(\s*)(of)";
		public const string MonthRegex = @"(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sept|Sep)";
		public const string DateYearRegex = @"(?<year>19\d{2}|20\d{2}|9\d|0\d|1\d|2\d)";
		public static readonly string OnRegex = $@"(?<=\bon\s+)({DayRegex}s?)\b";
		public const string RelaxedOnRegex = @"(?<=\b(on|at|in)\s+)((?<day>10th|11th|11st|12nd|12th|13rd|13th|14th|15th|16th|17th|18th|19th|1st|20th|21st|22nd|23rd|24th|25th|26th|27th|28th|29th|2nd|30th|31st|3rd|4th|5th|6th|7th|8th|9th)s?)\b";
		public static readonly string ThisRegex = $@"\b((this(\s*week)?\s+){WeekDayRegex})|({WeekDayRegex}(\s+this\s*week))\b";
		public static readonly string LastRegex = $@"\b(last(\s*week)?\s+{WeekDayRegex})|({WeekDayRegex}(\s+last\s*week))\b";
		public static readonly string NextRegex = $@"\b(next(\s*week)?\s+{WeekDayRegex})|({WeekDayRegex}(\s+next\s*week))\b";
		public const string SpecialDayRegex = @"\b((the\s+)?day before yesterday|(the\s+)?day after (tomorrow|tmr)|(the\s)?next day|(the\s+)?last day|the day|yesterday|tomorrow|tmr|today)\b";
		public const string StrictWeekDay = @"\b(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tue|Wedn|Weds|Wed|Thurs|Thur|Fri|Sat)s?\b";
		public static readonly string WeekDayOfMonthRegex = $@"(?<wom>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+{WeekDayRegex}\s+{MonthSuffixRegex})";
		public static readonly string SpecialDate = $@"(?<=\b(on|at)\s+the\s+){DayRegex}\b";
		public static readonly string DateExtractor1 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{MonthRegex}\s*[/\\\.\-]?\s*{DayRegex}\b";
		public static readonly string DateExtractor2 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{MonthRegex}\s*[\.\-]?\s*{DayRegex}(\s+|\s*,\s*|\s+of\s+){DateYearRegex}\b";
		public static readonly string DateExtractor3 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}(\s+|\s*,\s*|\s+of\s+|\s*-\s*){MonthRegex}((\s+|\s*,\s*){DateYearRegex})?\b";
		public static readonly string DateExtractor4 = $@"\b{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}\s*[/\\\-]\s*{DateYearRegex}";
		public static readonly string DateExtractor5 = $@"\b{DayRegex}\s*[/\\\-]\s*{MonthNumRegex}\s*[/\\\-]\s*{DateYearRegex}";
		public static readonly string DateExtractor6 = $@"(?<=\b(on|in|at)\s+){MonthNumRegex}[\-\.]{DayRegex}\b";
		public static readonly string DateExtractor7 = $@"\b{MonthNumRegex}\s*/\s*{DayRegex}((\s+|\s*,\s*|\s+of\s+){DateYearRegex})?\b";
		public static readonly string DateExtractor8 = $@"(?<=\b(on|in|at)\s+){DayRegex}[\\\-]{MonthNumRegex}\b";
		public static readonly string DateExtractor9 = $@"\b{DayRegex}\s*/\s*{MonthNumRegex}((\s+|\s*,\s*|\s+of\s+){DateYearRegex})?\b";
		public static readonly string DateExtractorA = $@"\b{DateYearRegex}\s*[/\\\-]\s*{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}";
		public static readonly string OfMonth = $@"^\s*of\s*{MonthRegex}";
		public static readonly string MonthEnd = $@"{MonthRegex}\s*(the)?\s*$";
		public const string NonDateUnitRegex = @"(?<unit>hours|hour|hrs|seconds|second|secs|sec|minutes|minute|mins)\b'";
		public const string DescRegex = @"(?<desc>pm\b|am\b|p\.m\.|a\.m\.|p\b|a\b)";
		public const string HourNumRegex = @"(?<hournum>zero|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve)";
		public const string MinuteNumRegex = @"(?<minnum>one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|thirty|forty|fifty)";
		public const string DeltaMinuteNumRegex = @"(?<deltaminnum>one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|thirty|forty|fifty)";
		public const string OclockRegex = @"(?<oclock>o’clock|o'clock|oclock)";
		public const string PmRegex = @"(?<pm>(in the\s+)?afternoon|(in the\s+)?evening|(in the\s+)?night)";
		public const string AmRegex = @"(?<am>(in the\s+)?morning)";
		public static readonly string LessThanOneHour = $@"(?<lth>(a\s+)?quarter|three quarter(s)?|half( an hour)?|{CommonDateTime.DeltaMinuteRegex}(\s+(minute|minutes|min|mins))|{DeltaMinuteNumRegex}(\s+(minute|minutes|min|mins)))";
		public static readonly string EngTimeRegex = $@"(?<engtime>{HourNumRegex}\s+({MinuteNumRegex}|(?<tens>twenty|thirty|forty|fourty|fifty)\s+{MinuteNumRegex}))";
		public static readonly string TimePrefix = $@"(?<prefix>({LessThanOneHour} past|{LessThanOneHour} to))";
		public static readonly string TimeSuffix = $@"(?<suffix>{AmRegex}|{PmRegex}|{OclockRegex})";
		public static readonly string BasicTime = $@"(?<basictime>{EngTimeRegex}|{HourNumRegex}|{CommonDateTime.HourRegex}:{CommonDateTime.MinuteRegex}(:{CommonDateTime.SecondRegex})?|{CommonDateTime.HourRegex})";
		public static readonly string AtRegex = $@"\b(?<=\bat\s+)({EngTimeRegex}|{HourNumRegex}|{CommonDateTime.HourRegex})\b";
		public static readonly string IshRegex = $@"{CommonDateTime.HourRegex}(-|——)?ish|noonish|noon";
		public const string TimeUnitRegex = @"(?<unit>hours|hour|hrs|hr|h|minutes|minute|mins|min|seconds|second|secs|sec)\b";
		public static readonly string ConnectNumRegex = $@"{CommonDateTime.HourRegex}(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\s*{DescRegex}";
		public static readonly string TimeRegex1 = $@"(\b{TimePrefix}\s+)?({EngTimeRegex}|{HourNumRegex}|{CommonDateTime.HourRegex})\s*{DescRegex}";
		public static readonly string TimeRegex2 = $@"(\b{TimePrefix}\s+)?(T)?{CommonDateTime.HourRegex}(\s*)?:(\s*)?{CommonDateTime.MinuteRegex}((\s*)?:(\s*)?{CommonDateTime.SecondRegex})?((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex3 = $@"(\b{TimePrefix}\s+)?{CommonDateTime.HourRegex}\.{CommonDateTime.MinuteRegex}(\s*{DescRegex})";
		public static readonly string TimeRegex4 = $@"\b{TimePrefix}\s+{BasicTime}(\s*{DescRegex})?\s+{TimeSuffix}\b";
		public static readonly string TimeRegex5 = $@"\b{TimePrefix}\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex6 = $@"{BasicTime}(\s*{DescRegex})?\s+{TimeSuffix}\b";
		public static readonly string TimeRegex7 = $@"\b{TimeSuffix}\s+at\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex8 = $@"\b{TimeSuffix}\s+{BasicTime}((\s*{DescRegex})|\b)";
		public const string HourRegex = @"(?<hour>00|01|02|03|04|05|06|07|08|09|0|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|1|2|3|4|5|6|7|8|9)";
		public const string PeriodHourNumRegex = @"(?<hour>twenty one|twenty two|twenty three|twenty four|zero|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nighteen|twenty)";
		public const string PeriodDescRegex = @"(?<desc>pm|am|p\.m\.|a\.m\.|p|a)";
		public const string PeriodPmRegex = @"(?<pm>afternoon|evening|in the afternoon|in the evening|in the night)s?";
		public const string PeriodAmRegex = @"(?<am>morning|in the morning)s?";
		public static readonly string PureNumFromTo = $@"(from\s+)?({HourRegex}|{PeriodHourNumRegex})(\s*{PeriodDescRegex})?\s*{TillRegex}\s*({HourRegex}|{PeriodHourNumRegex})\s*({PmRegex}|{AmRegex}|{PeriodDescRegex})?";
		public static readonly string PureNumBetweenAnd = $@"(between\s+)({HourRegex}|{PeriodHourNumRegex})(\s*{PeriodDescRegex})?\s*and\s*({HourRegex}|{PeriodHourNumRegex})\s*({PmRegex}|{AmRegex}|{PeriodDescRegex})?";
		public const string PrepositionRegex = @"(?<prep>^(at|on|of)(\s+the)?$)";
		public const string NightRegex = @"\b(?<night>daytime|morning|afternoon|(late\s+)?night|evening)s?\b";
		public static readonly string SpecificNightRegex = $@"\b(((this|next|last)\s+{NightRegex})\b|\btonight)s?\b";
		public static readonly string TimeFollowedUnit = $@"^\s*{TimeUnitRegex}";
		public static readonly string TimeNumberCombinedWithUnit = $@"\b(?<num>\d+(\.\d*)?){TimeUnitRegex}";
		public const string NowRegex = @"\b(?<now>(right\s+)?now|as soon as possible|asap|recently|previously)\b";
		public const string SuffixRegex = @"^\s*(in the\s+)?(morning|afternoon|evening|night)\b";
		public const string DateTimeNightRegex = @"\b(?<night>morning|afternoon|night|evening)\b";
		public static readonly string DateTimeSpecificNightRegex = $@"\b(((this|next|last)\s+{DateTimeNightRegex})\b|\btonight)\b";
		public static readonly string TimeOfTodayAfterRegex = $@"^\s*(,\s*)?(in\s+)?{DateTimeSpecificNightRegex}";
		public static readonly string TimeOfTodayBeforeRegex = $@"{DateTimeSpecificNightRegex}(\s*,)?(\s+(at|for))?\s*$";
		public static readonly string SimpleTimeOfTodayAfterRegex = $@"({HourNumRegex}|{CommonDateTime.HourRegex})\s*(,\s*)?(in\s+)?{DateTimeSpecificNightRegex}";
		public static readonly string SimpleTimeOfTodayBeforeRegex = $@"{DateTimeSpecificNightRegex}(\s*,)?(\s+(at|for))?\s*({HourNumRegex}|{CommonDateTime.HourRegex})";
		public const string TheEndOfRegex = @"(the\s+)?end of(\s+the)?\s*$";
		public const string PeriodNightRegex = @"\b(?<night>morning|afternoon|(late\s+)?night|evening)\b";
		public static readonly string PeriodSpecificNightRegex = $@"\b(((this|next|last)\s+{PeriodNightRegex})\b|\btonight)\b";
		public const string DurationUnitRegex = @"(?<unit>years|year|months|month|weeks|week|days|day|hours|hour|hrs|hr|h|minutes|minute|mins|min|seconds|second|secs|sec)\b";
		public const string SuffixAndRegex = @"(?<suffix>\s*(and)\s+((an|a)\s+)?(?<suffix_num>half|quarter))";
		public const string PeriodicRegex = @"\b(?<periodic>daily|monthly|weekly|biweekly|yearly|annually|annual)\b";
		public static readonly string EachUnitRegex = $@"(?<each>(each|every)\s*{DurationUnitRegex})";
		public const string EachPrefixRegex = @"(?<each>(each|every)\s*$)";
		public const string SetLastRegex = @"(?<last>last|this|next)";
		public const string EachDayRegex = @"^\s*(each|every)\s*day\b";
		public static readonly string DurationFollowedUnit = $@"^\s*{SuffixAndRegex}?(\s+|-)?{DurationUnitRegex}";
		public static readonly string NumberCombinedWithDurationUnit = $@"\b(?<num>\d+(\.\d*)?)(-)?{DurationUnitRegex}";
		public static readonly string AnUnitRegex = $@"(((?<half>half\s+)*(an|a))|(an|a))\s+{DurationUnitRegex}";
		public const string AllRegex = @"\b(?<all>all\s+(?<unit>year|month|week|day))\b";
		public const string HalfRegex = @"(((a|an)\s*)|\b)(?<half>half\s+(?<unit>year|month|week|day|hour))\b";
		public const string YearRegex = @"\b(?<year>19\d{2}|20\d{2})\b";
		public static readonly string HolidayRegex1 = $@"\b(?<holiday>clean monday|good friday|ash wednesday|mardi gras|washington's birthday|mao's birthday|chinese new Year|new years|mayday|yuan dan|april fools|christmas|christmas eve|xmas|thanksgiving|halloween|yuandan)(\s+(of\s+)?({YearRegex}|(?<order>next|last|this)\s+year))?\b";
		public static readonly string HolidayRegex2 = $@"\b(?<holiday>martin luther king|martin luther king jr|all saint's|tree planting day|white lover|st patrick|st george|cinco de mayo|independence|us independence|all hallow|all souls|guy fawkes)(\s+(of\s+)?({YearRegex}|(?<order>next|last|this)\s+year))?\b";
		public static readonly string HolidayRegex3 = $@"(?<holiday>(canberra|easter|columbus|thanks\s*giving|labour|mother's|mother|mothers|father's|father|fathers|female|single|teacher's|youth|children|arbor|girls|chsmilbuild|lover|labor|inauguration|groundhog|valentine's|baptiste|bastille|halloween|veterans|memorial|mid(-| )autumn|moon|new year|new years|new year eve|new year's eve|spring|lantern|qingming|dragon boat)\s+(day))(\s+(of\s+)?({YearRegex}|(?<order>next|last|this)\s+year))?";
		public const string DateTokenPrefix = "on ";
		public const string TimeTokenPrefix = "at ";
		public const string TokenBeforeDate = "on ";
		public const string TokenBeforeTime = "at ";
		public const string AMTimeRegex = @"(?<am>morning)";
		public const string PMTimeRegex = @"(?<pm>afternoon|evening|night)";
		public const string BeforeRegex = @"^\s*before\s+";
		public const string AfterRegex = @"^\s*after\s+";
		public static readonly Dictionary<string, string> UnitMap = new Dictionary<string, string>
		{
			{ "years", "Y" },
			{ "year", "Y" },
			{ "months", "MON" },
			{ "month", "MON" },
			{ "weeks", "W" },
			{ "week", "W" },
			{ "days", "D" },
			{ "day", "D" },
			{ "hours", "H" },
			{ "hour", "H" },
			{ "hrs", "H" },
			{ "hr", "H" },
			{ "h", "H" },
			{ "minutes", "M" },
			{ "minute", "M" },
			{ "mins", "M" },
			{ "min", "M" },
			{ "seconds", "S" },
			{ "second", "S" },
			{ "secs", "S" },
			{ "sec", "S" }
		};
		public static readonly Dictionary<string, long> UnitValueMap = new Dictionary<string, long>
		{
			{ "years", 31536000 },
			{ "year", 31536000 },
			{ "months", 2592000 },
			{ "month", 2592000 },
			{ "weeks", 604800 },
			{ "week", 604800 },
			{ "days", 86400 },
			{ "day", 86400 },
			{ "hours", 3600 },
			{ "hour", 3600 },
			{ "hrs", 3600 },
			{ "hr", 3600 },
			{ "h", 3600 },
			{ "minutes", 60 },
			{ "minute", 60 },
			{ "mins", 60 },
			{ "min", 60 },
			{ "seconds", 1 },
			{ "second", 1 },
			{ "secs", 1 },
			{ "sec", 1 }
		};
		public static readonly Dictionary<string, string> SeasonMap = new Dictionary<string, string>
		{
			{ "spring", "SP" },
			{ "summer", "SU" },
			{ "fall", "FA" },
			{ "autumn", "FA" },
			{ "winter", "WI" }
		};
		public static readonly Dictionary<string, int> SeasonValueMap = new Dictionary<string, int>
		{
			{ "SP", 3 },
			{ "SU", 6 },
			{ "FA", 9 },
			{ "WI", 12 }
		};
		public static readonly Dictionary<string, int> CardinalMap = new Dictionary<string, int>
		{
			{ "first", 1 },
			{ "1st", 1 },
			{ "second", 2 },
			{ "2nd", 2 },
			{ "third", 3 },
			{ "3rd", 3 },
			{ "fourth", 4 },
			{ "4th", 4 },
			{ "fifth", 5 },
			{ "5th", 5 }
		};
		public static readonly Dictionary<string, int> DayOfWeek = new Dictionary<string, int>
		{
			{ "monday", 1 },
			{ "tuesday", 2 },
			{ "wednesday", 3 },
			{ "thursday", 4 },
			{ "friday", 5 },
			{ "saturday", 6 },
			{ "sunday", 0 },
			{ "mon", 1 },
			{ "tue", 2 },
			{ "wed", 3 },
			{ "wedn", 3 },
			{ "weds", 3 },
			{ "thu", 4 },
			{ "thur", 4 },
			{ "thurs", 4 },
			{ "fri", 5 },
			{ "sat", 6 },
			{ "sun", 0 }
		};
		public static readonly Dictionary<string, int> MonthOfYear = new Dictionary<string, int>
		{
			{ "january", 1 },
			{ "february", 2 },
			{ "march", 3 },
			{ "april", 4 },
			{ "may", 5 },
			{ "june", 6 },
			{ "july", 7 },
			{ "august", 8 },
			{ "september", 9 },
			{ "october", 10 },
			{ "november", 11 },
			{ "december", 12 },
			{ "jan", 1 },
			{ "feb", 2 },
			{ "mar", 3 },
			{ "apr", 4 },
			{ "jun", 6 },
			{ "jul", 7 },
			{ "aug", 8 },
			{ "sep", 9 },
			{ "sept", 9 },
			{ "oct", 10 },
			{ "nov", 11 },
			{ "dec", 12 },
			{ "1", 1 },
			{ "2", 2 },
			{ "3", 3 },
			{ "4", 4 },
			{ "5", 5 },
			{ "6", 6 },
			{ "7", 7 },
			{ "8", 8 },
			{ "9", 9 },
			{ "10", 10 },
			{ "11", 11 },
			{ "12", 12 },
			{ "01", 1 },
			{ "02", 2 },
			{ "03", 3 },
			{ "04", 4 },
			{ "05", 5 },
			{ "06", 6 },
			{ "07", 7 },
			{ "08", 8 },
			{ "09", 9 }
		};
		public static readonly Dictionary<string, int> Numbers = new Dictionary<string, int>
		{
			{ "zero", 0 },
			{ "one", 1 },
			{ "a", 1 },
			{ "an", 1 },
			{ "two", 2 },
			{ "three", 3 },
			{ "four", 4 },
			{ "five", 5 },
			{ "six", 6 },
			{ "seven", 7 },
			{ "eight", 8 },
			{ "nine", 9 },
			{ "ten", 10 },
			{ "eleven", 11 },
			{ "twelve", 12 },
			{ "thirteen", 13 },
			{ "fourteen", 14 },
			{ "fifteen", 15 },
			{ "sixteen", 16 },
			{ "seventeen", 17 },
			{ "eighteen", 18 },
			{ "nineteen", 19 },
			{ "twenty", 20 },
			{ "twenty one", 21 },
			{ "twenty two", 22 },
			{ "twenty three", 23 },
			{ "twenty four", 24 },
			{ "twenty five", 25 },
			{ "twenty six", 26 },
			{ "twenty seven", 27 },
			{ "twenty eight", 28 },
			{ "twenty nine", 29 },
			{ "thirty", 30 },
			{ "thirty one", 31 },
			{ "thirty two", 32 },
			{ "thirty three", 33 },
			{ "thirty four", 34 },
			{ "thirty five", 35 },
			{ "thirty six", 36 },
			{ "thirty seven", 37 },
			{ "thirty eight", 38 },
			{ "thirty nine", 39 },
			{ "forty", 40 },
			{ "forty one", 41 },
			{ "forty two", 42 },
			{ "forty three", 43 },
			{ "forty four", 44 },
			{ "forty five", 45 },
			{ "forty six", 46 },
			{ "forty seven", 47 },
			{ "forty eight", 48 },
			{ "forty nine", 49 },
			{ "fifty", 50 },
			{ "fifty one", 51 },
			{ "fifty two", 52 },
			{ "fifty three", 53 },
			{ "fifty four", 54 },
			{ "fifty five", 55 },
			{ "fifty six", 56 },
			{ "fifty seven", 57 },
			{ "fifty eight", 58 },
			{ "fifty nine", 59 },
			{ "sixty", 60 },
			{ "sixty one", 61 },
			{ "sixty two", 62 },
			{ "sixty three", 63 },
			{ "sixty four", 64 },
			{ "sixty five", 65 },
			{ "sixty six", 66 },
			{ "sixty seven", 67 },
			{ "sixty eight", 68 },
			{ "sixty nine", 69 },
			{ "seventy", 70 },
			{ "seventy one", 71 },
			{ "seventy two", 72 },
			{ "seventy three", 73 },
			{ "seventy four", 74 },
			{ "seventy five", 75 },
			{ "seventy six", 76 },
			{ "seventy seven", 77 },
			{ "seventy eight", 78 },
			{ "seventy nine", 79 },
			{ "eighty", 80 },
			{ "eighty one", 81 },
			{ "eighty two", 82 },
			{ "eighty three", 83 },
			{ "eighty four", 84 },
			{ "eighty five", 85 },
			{ "eighty six", 86 },
			{ "eighty seven", 87 },
			{ "eighty eight", 88 },
			{ "eighty nine", 89 },
			{ "ninety", 90 },
			{ "ninety one", 91 },
			{ "ninety two", 92 },
			{ "ninety three", 93 },
			{ "ninety four", 94 },
			{ "ninety five", 95 },
			{ "ninety six", 96 },
			{ "ninety seven", 97 },
			{ "ninety eight", 98 },
			{ "ninety nine", 99 },
			{ "one hundred", 100 }
		};
		public static readonly Dictionary<string, int> DayOfMonth = new Dictionary<string, int>
		{
			{ "1st", 1 },
			{ "2nd", 2 },
			{ "3rd", 3 },
			{ "4th", 4 },
			{ "5th", 5 },
			{ "6th", 6 },
			{ "7th", 7 },
			{ "8th", 8 },
			{ "9th", 9 },
			{ "10th", 10 },
			{ "11th", 11 },
			{ "11st", 11 },
			{ "12th", 12 },
			{ "12nd", 12 },
			{ "13th", 13 },
			{ "13rd", 13 },
			{ "14th", 14 },
			{ "15th", 15 },
			{ "16th", 16 },
			{ "17th", 17 },
			{ "18th", 18 },
			{ "19th", 19 },
			{ "20th", 20 },
			{ "21st", 21 },
			{ "22nd", 22 },
			{ "23rd", 23 },
			{ "24th", 24 },
			{ "25th", 25 },
			{ "26th", 26 },
			{ "27th", 27 },
			{ "28th", 28 },
			{ "29th", 29 },
			{ "30th", 30 },
			{ "31st", 31 }
		};
		public static readonly Dictionary<string, double> DoubleNumbers = new Dictionary<string, double>
		{
			{ "half", 0.5 },
			{ "quarter", 0.25 }
		};
	}
}
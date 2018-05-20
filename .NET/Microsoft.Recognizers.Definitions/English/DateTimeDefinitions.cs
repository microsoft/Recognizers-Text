﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//     
//     Generation parameters:
//     - DataFilename: Patterns\English\English-DateTime.yaml
//     - Language: English
//     - ClassName: DateTimeDefinitions
// </auto-generated>
//------------------------------------------------------------------------------
namespace Microsoft.Recognizers.Definitions.English
{
	using System;
	using System.Collections.Generic;

	public static class DateTimeDefinitions
	{
		public const string TillRegex = @"(?<till>to|till|til|until|thru|through|--|-|—|——|~)";
		public const string RangeConnectorRegex = @"(?<and>and|through|to|--|-|—|——)";
		public const string RelativeRegex = @"(?<order>next|coming|upcoming|this|last|past|previous|current|the)";
		public const string StrictRelativeRegex = @"(?<order>next|coming|upcoming|this|last|past|previous|current)";
		public const string NextPrefixRegex = @"(next|coming|upcoming)\b";
		public const string AfterNextSuffixRegex = @"\b(after\s+(the\s+)?next)\b";
		public const string PastPrefixRegex = @"(last|past|previous)\b";
		public const string ThisPrefixRegex = @"(this|current)\b";
		public const string ReferencePrefixRegex = @"(that|same)\b";
		public const string FutureSuffixRegex = @"\b(in\s+the\s+)?(future|hence)\b";
		public const string DayRegex = @"(the\s*)?(?<day>01|02|03|04|05|06|07|08|09|10th|10|11th|11st|11|12nd|12th|12|13rd|13th|13|14th|14|15th|15|16th|16|17th|17|18th|18|19th|19|1st|1|20th|20|21st|21th|21|22nd|22th|22|23rd|23th|23|24th|24|25th|25|26th|26|27th|27|28th|28|29th|29|2nd|2|30th|30|31st|31|3rd|3|4th|4|5th|5|6th|6|7th|7|8th|8|9th|9)(?=\b|t)";
		public const string MonthNumRegex = @"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)\b";
		public const string CenturyRegex = @"\b(?<century>((one|two)\s+thousand(\s+and)?(\s+(one|two|three|four|five|six|seven|eight|nine)\s+hundred(\s+and)?)?)|((twenty one|twenty two|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty)(\s+hundred)?(\s+and)?))\b";
		public const string WrittenNumRegex = @"(one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|thirty|forty|fourty|fifty|sixty|seventy|eighty|ninety)";
		public static readonly string FullTextYearRegex = $@"\b((?<firsttwoyearnum>{CenturyRegex})\s+(?<lasttwoyearnum>((zero|twenty|thirty|forty|fourty|fifty|sixty|seventy|eighty|ninety)\s+{WrittenNumRegex})|{WrittenNumRegex}))\b|\b(?<firsttwoyearnum>{CenturyRegex})\b";
		public const string YearNumRegex = @"\b(?<year>((1[5-9]|20)\d{2})|2100)\b";
		public static readonly string YearRegex = $@"({YearNumRegex}|{FullTextYearRegex})";
		public const string WeekDayRegex = @"\b(in\s+the\s+day\s+)?(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tues|Tue|Wedn|Weds|Wed|Thurs|Thur|Thu|Fri|Sat|Sun)s?\b";
		public const string SingleWeekDayRegex = @"\b(in\s+the\s+day\s+)?(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tue|Tues|Wedn|Weds|Wed|Thurs|Thur|Thu|Fri|((?<=on)\s+(Sat|Sun)))\b";
		public static readonly string RelativeMonthRegex = $@"(?<relmonth>(of\s+)?{RelativeRegex}\s+month)\b";
		public const string WrittenMonthRegex = @"(((the\s+)?month of\s+)?(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sept|Sep))";
		public static readonly string MonthSuffixRegex = $@"(?<msuf>(in\s+|of\s+|on\s+)?({RelativeMonthRegex}|{WrittenMonthRegex}))";
		public const string DateUnitRegex = @"(?<unit>years|year|months|month|weeks|week|days|day)\b";
		public static readonly string SimpleCasesRegex = $@"\b((from|between)\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex}\s+{MonthSuffixRegex}|{MonthSuffixRegex}\s+{DayRegex})((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string MonthFrontSimpleCasesRegex = $@"\b((from|between)\s+)?{MonthSuffixRegex}\s+((from)\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string MonthFrontBetweenRegex = $@"\b{MonthSuffixRegex}\s+(between\s+)({DayRegex})\s*{RangeConnectorRegex}\s*({DayRegex})((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string BetweenRegex = $@"\b(between\s+)({DayRegex})\s*{RangeConnectorRegex}\s*({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string MonthWithYear = $@"\b(({WrittenMonthRegex}(\s*),?(\s+of)?(\s*)({YearRegex}|(?<order>next|last|this)\s+year))|(({YearRegex}|(?<order>next|last|this)\s+year)(\s*),?(\s*){WrittenMonthRegex}))\b";
		public static readonly string OneWordPeriodRegex = $@"\b((((the\s+)?month of\s+)?({RelativeRegex}\s+)?(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sep|Sept))|(month|year) to date|({RelativeRegex}\s+)?(my\s+)?(weekend|week|month|year)(\s+{AfterNextSuffixRegex})?)\b";
		public static readonly string MonthNumWithYear = $@"({YearNumRegex}(\s*)[/\-\.](\s*){MonthNumRegex})|({MonthNumRegex}(\s*)[/\-](\s*){YearNumRegex})";
		public static readonly string WeekOfMonthRegex = $@"(?<wom>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+week\s+{MonthSuffixRegex})";
		public static readonly string WeekOfYearRegex = $@"(?<woy>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+week(\s+of)?\s+({YearRegex}|{RelativeRegex}\s+year))";
		public static readonly string FollowedDateUnit = $@"^\s*{DateUnitRegex}";
		public static readonly string NumberCombinedWithDateUnit = $@"\b(?<num>\d+(\.\d*)?){DateUnitRegex}";
		public const string QuarterTermRegex = @"(((?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th)\s+quarter)|(Q(?<number>[1-4])))";
		public static readonly string QuarterRegex = $@"(the\s+)?{QuarterTermRegex}((\s+of|\s*,\s*)?\s+({YearRegex}|{RelativeRegex}\s+year))?";
		public static readonly string QuarterRegexYearFront = $@"({YearRegex}|{RelativeRegex}\s+year)\s+(the\s+)?{QuarterTermRegex}";
		public const string HalfYearTermRegex = @"(?<cardinal>first|1st|second|2nd)\s+half";
		public static readonly string HalfYearFrontRegex = $@"(?<year>((1[5-9]|20)\d{{2}})|2100)\s*(the\s+)?H(?<number>[1-2])";
		public static readonly string HalfYearBackRegex = $@"(the\s+)?(H(?<number>[1-2])|({HalfYearTermRegex}))(\s+of|\s*,\s*)?\s+({YearRegex})";
		public static readonly string HalfYearRelativeRegex = $@"(the\s+)?{HalfYearTermRegex}(\s+of|\s*,\s*)?\s+({RelativeRegex}\s+year)";
		public static readonly string AllHalfYearRegex = $@"({HalfYearFrontRegex})|({HalfYearBackRegex})|({HalfYearRelativeRegex})";
		public const string EarlyPrefixRegex = @"(?<EarlyPrefix>early|beginning of|start of)";
		public const string MidPrefixRegex = @"(?<MidPrefix>mid|middle of)";
		public const string LaterPrefixRegex = @"(?<LatePrefix>late|later(\s+in)?|end of)";
		public static readonly string PrefixPeriodRegex = $@"({EarlyPrefixRegex}|{MidPrefixRegex}|{LaterPrefixRegex})";
		public const string SeasonDescRegex = @"(?<seas>spring|summer|fall|autumn|winter)";
		public static readonly string SeasonRegex = $@"\b(?<season>({PrefixPeriodRegex}\s+)?({RelativeRegex}\s+)?{SeasonDescRegex}((\s+of|\s*,\s*)?\s+({YearRegex}|{RelativeRegex}\s+year))?)\b";
		public const string WhichWeekRegex = @"(week)(\s*)(?<number>\d\d|\d|0\d)";
		public const string WeekOfRegex = @"(the\s+)?(week)(\s+of)";
		public const string MonthOfRegex = @"(month)(\s*)(of)";
		public const string MonthRegex = @"(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sept|Sep)";
		public const string AmbiguousMonthP0Regex = @"\b((^may i)|(i|you|he|she|we|they)\s+may|(may\s+((((also|not|(also not)|well)\s+)?(be|contain|constitute|email|e-mail|take|have|result|involve|get|work|reply))|(or may not))))\b";
		public const string AmDescRegex = @"(am\b|a\.m\.|a m\b|a\. m\.|a\.m\b|a\. m\b|a m\b)";
		public const string PmDescRegex = @"(pm\b|p\.m\.|p\b|p m\b|p\. m\.|p\.m\b|p\. m\b|p m\b)";
		public static readonly string DateYearRegex = $@"(?<year>((1\d|20)\d{{2}})|2100|(([0-27-9]\d)\b(?!(\s*((\:)|{AmDescRegex}|{PmDescRegex})))))";
		public static readonly string YearSuffix = $@"(,?\s*({DateYearRegex}|{FullTextYearRegex}))";
		public static readonly string OnRegex = $@"(?<=\bon\s+)({DayRegex}s?)\b";
		public const string RelaxedOnRegex = @"(?<=\b(on|at|in)\s+)((?<day>10th|11th|11st|12nd|12th|13rd|13th|14th|15th|16th|17th|18th|19th|1st|20th|21st|21th|22nd|22th|23rd|23th|24th|25th|26th|27th|28th|29th|2nd|30th|31st|3rd|4th|5th|6th|7th|8th|9th)s?)\b";
		public static readonly string ThisRegex = $@"\b((this(\s*week)?(\s*on)?\s+){WeekDayRegex})|({WeekDayRegex}((\s+of)?\s+this\s*week))\b";
		public static readonly string LastDateRegex = $@"\b({PastPrefixRegex}(\s*week)?\s+{WeekDayRegex})|({WeekDayRegex}(\s+last\s*week))\b";
		public static readonly string NextDateRegex = $@"\b({NextPrefixRegex}(\s*week(\s*on)?)?\s+{WeekDayRegex})|((on\s+)?{WeekDayRegex}((\s+of)?\s+next\s*week))\b";
		public static readonly string SpecialDayRegex = $@"\b((the\s+)?day before yesterday|(the\s+)?day after (tomorrow|tmr)|((the\s+)?({RelativeRegex}|my)\s+day)|yesterday|tomorrow|tmr|today)\b";
		public static readonly string SpecialDayWithNumRegex = $@"\b((?<number>{WrittenNumRegex})\s+days?\s+from\s+(?<day>yesterday|tomorrow|tmr|today))\b";
		public static readonly string RelativeDayRegex = $@"\b(((the\s+)?{RelativeRegex}\s+day))\b";
		public const string SetWeekDayRegex = @"\b(?<prefix>on\s+)?(?<weekday>morning|afternoon|evening|night|Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday)s\b";
		public static readonly string WeekDayOfMonthRegex = $@"(?<wom>(the\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\s+{WeekDayRegex}\s+{MonthSuffixRegex})";
		public static readonly string RelativeWeekDayRegex = $@"\b({WrittenNumRegex}\s+{WeekDayRegex}\s+(from\s+now|later))\b";
		public static readonly string SpecialDate = $@"(?=\b(on|at)\s+the\s+){DayRegex}\b";
		public static readonly string DateExtractor1 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{MonthRegex}(\.)?\s*[/\\\.\-]?\s*{DayRegex}(\.)?\b";
		public static readonly string DateExtractor2 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{MonthRegex}(\.)?\s*[\.\-]?\s*{DayRegex}(\.)?(\s+|\s*,\s*|\s+of\s+){DateYearRegex}\b";
		public static readonly string DateExtractor3 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}(\.)?(\s+|\s*,\s*|\s+of\s+|\s*-\s*){MonthRegex}(\.)?((\s+|\s*,\s*){DateYearRegex})?\b";
		public static readonly string DateExtractor4 = $@"\b{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}(\.)?\s*[/\\\-]\s*{DateYearRegex}";
		public static readonly string DateExtractor5 = $@"\b{DayRegex}\s*[/\\\-\.]\s*{MonthNumRegex}\s*[/\\\-\.]\s*{DateYearRegex}";
		public static readonly string DateExtractor6 = $@"(?<=\b(on|in|at)\s+){MonthNumRegex}[\-\.]{DayRegex}\b";
		public static readonly string DateExtractor7 = $@"\b{MonthNumRegex}\s*/\s*{DayRegex}((\s+|\s*,\s*|\s+of\s+){DateYearRegex})?\b";
		public static readonly string DateExtractor8 = $@"(?<=\b(on|in|at)\s+){DayRegex}[\\\-]{MonthNumRegex}\b";
		public static readonly string DateExtractor9 = $@"\b{DayRegex}\s*/\s*{MonthNumRegex}((\s+|\s*,\s*|\s+of\s+){DateYearRegex})?\b";
		public static readonly string DateExtractorA = $@"\b{DateYearRegex}\s*[/\\\-\.]\s*{MonthNumRegex}\s*[/\\\-\.]\s*{DayRegex}";
		public static readonly string OfMonth = $@"^\s*of\s*{MonthRegex}";
		public static readonly string MonthEnd = $@"{MonthRegex}\s*(the)?\s*$";
		public static readonly string WeekDayEnd = $@"{WeekDayRegex}\s*,?\s*$";
		public const string RangeUnitRegex = @"\b(?<unit>years|year|months|month|weeks|week)\b";
		public const string OclockRegex = @"(?<oclock>o\s*’\s*clock|o\s*‘\s*clock|o\s*'\s*clock|o\s*clock)";
		public static readonly string DescRegex = $@"((({OclockRegex}\s+)?(?<desc>ampm|am\b|a\.m\.|a m\b|a\. m\.|a\.m\b|a\. m\b|a m\b|pm\b|p\.m\.|p m\b|p\. m\.|p\.m\b|p\. m\b|p\b|p m\b))|{OclockRegex})";
		public const string HourNumRegex = @"\b(?<hournum>zero|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve)\b";
		public const string MinuteNumRegex = @"(?<minnum>one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|thirty|forty|fifty)";
		public const string DeltaMinuteNumRegex = @"(?<deltaminnum>one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|thirty|forty|fifty)";
		public const string PmRegex = @"(?<pm>(((at|in|around|on|for)\s+(the\s+)?)?(afternoon|evening|midnight|lunchtime))|((at|in|around|on|for)\s+(the\s+)?night))";
		public const string PmRegexFull = @"(?<pm>((at|in|around|on|for)\s+(the\s+)?)?(afternoon|evening|midnight|night|lunchtime))";
		public const string AmRegex = @"(?<am>((at|in|around|on|for)\s+(the\s+)?)?(morning))";
		public const string LunchRegex = @"\b(lunchtime)\b";
		public const string NightRegex = @"\b(midnight|night)\b";
		public const string CommonDatePrefixRegex = @"^[\.]";
		public static readonly string LessThanOneHour = $@"(?<lth>(a\s+)?quarter|three quarter(s)?|half( an hour)?|{BaseDateTime.DeltaMinuteRegex}(\s+(minute|minutes|min|mins))|{DeltaMinuteNumRegex}(\s+(minute|minutes|min|mins)))";
		public static readonly string WrittenTimeRegex = $@"(?<writtentime>{HourNumRegex}\s+({MinuteNumRegex}|(?<tens>twenty|thirty|forty|fourty|fifty)\s+{MinuteNumRegex}))";
		public static readonly string TimePrefix = $@"(?<prefix>({LessThanOneHour} past|{LessThanOneHour} to))";
		public static readonly string TimeSuffix = $@"(?<suffix>{AmRegex}|{PmRegex}|{OclockRegex})";
		public static readonly string TimeSuffixFull = $@"(?<suffix>{AmRegex}|{PmRegexFull}|{OclockRegex})";
		public static readonly string BasicTime = $@"\b(?<basictime>{WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}:{BaseDateTime.MinuteRegex}(:{BaseDateTime.SecondRegex})?|{BaseDateTime.HourRegex})";
		public const string MidnightRegex = @"(?<midnight>midnight|mid-night|mid night)";
		public const string MidmorningRegex = @"(?<midmorning>midmorning|mid-morning|mid morning)";
		public const string MidafternoonRegex = @"(?<midafternoon>midafternoon|mid-afternoon|mid afternoon)";
		public const string MiddayRegex = @"(?<midday>midday|mid-day|mid day|((12\s)?noon))";
		public static readonly string MidTimeRegex = $@"(?<mid>({MidnightRegex}|{MidmorningRegex}|{MidafternoonRegex}|{MiddayRegex}))";
		public static readonly string AtRegex = $@"\b(((?<=\bat\s+)({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}|{MidTimeRegex}))|{MidTimeRegex})\b";
		public static readonly string IshRegex = $@"\b({BaseDateTime.HourRegex}(-|——)?ish|noonish|noon)\b";
		public const string TimeUnitRegex = @"([^A-Za-z]{1,}|\b)(?<unit>hours|hour|hrs|hr|h|minutes|minute|mins|min|seconds|second|secs|sec)\b";
		public const string RestrictedTimeUnitRegex = @"(?<unit>hour|minute)\b";
		public const string FivesRegex = @"(?<tens>(fifteen|twenty(\s*five)?|thirty(\s*five)?|forty(\s*five)?|fourty(\s*five)?|fifty(\s*five)?|ten|five))\b";
		public static readonly string HourRegex = $@"\b{BaseDateTime.HourRegex}";
		public const string PeriodHourNumRegex = @"\b(?<hour>twenty one|twenty two|twenty three|twenty four|zero|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty)\b";
		public static readonly string ConnectNumRegex = $@"{BaseDateTime.HourRegex}(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\s*{DescRegex}";
		public static readonly string TimeRegex1 = $@"\b({TimePrefix}\s+)?({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})\s*{DescRegex}";
		public static readonly string TimeRegex2 = $@"(\b{TimePrefix}\s+)?(T)?{BaseDateTime.HourRegex}(\s*)?:(\s*)?{BaseDateTime.MinuteRegex}((\s*)?:(\s*)?{BaseDateTime.SecondRegex})?((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex3 = $@"(\b{TimePrefix}\s+)?{BaseDateTime.HourRegex}\.{BaseDateTime.MinuteRegex}(\s*{DescRegex})";
		public static readonly string TimeRegex4 = $@"\b{TimePrefix}\s+{BasicTime}(\s*{DescRegex})?\s+{TimeSuffix}\b";
		public static readonly string TimeRegex5 = $@"\b{TimePrefix}\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex6 = $@"{BasicTime}(\s*{DescRegex})?\s+{TimeSuffix}\b";
		public static readonly string TimeRegex7 = $@"\b{TimeSuffixFull}\s+at\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex8 = $@"\b{TimeSuffixFull}\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex9 = $@"\b{PeriodHourNumRegex}\s+{FivesRegex}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex10 = $@"(\b{TimePrefix}\s+)?{BaseDateTime.HourRegex}(\s*h\s*){BaseDateTime.MinuteRegex}(\s*{DescRegex})?";
		public static readonly string PureNumFromTo = $@"((from|between)\s+)?({HourRegex}|{PeriodHourNumRegex})(\s*(?<leftDesc>{DescRegex}))?\s*{TillRegex}\s*({HourRegex}|{PeriodHourNumRegex})\s*(?<rightDesc>{PmRegex}|{AmRegex}|{DescRegex})?";
		public static readonly string PureNumBetweenAnd = $@"(between\s+)({HourRegex}|{PeriodHourNumRegex})(\s*(?<leftDesc>{DescRegex}))?\s*{RangeConnectorRegex}\s*({HourRegex}|{PeriodHourNumRegex})\s*(?<rightDesc>{PmRegex}|{AmRegex}|{DescRegex})?";
		public const string PrepositionRegex = @"(?<prep>^(at|on|of)(\s+the)?$)";
		public const string TimeOfDayRegex = @"\b(?<timeOfDay>((((in\s+(the)?\s+)?((?<early>early(\s+|-))|(?<late>late(\s+|-)))?(morning|afternoon|night|evening)))|(((in\s+(the)?\s+)?)(daytime)))s?)\b";
		public static readonly string SpecificTimeOfDayRegex = $@"\b(({StrictRelativeRegex}\s+{TimeOfDayRegex})\b|\btonight)s?\b";
		public static readonly string TimeFollowedUnit = $@"^\s*{TimeUnitRegex}";
		public static readonly string TimeNumberCombinedWithUnit = $@"\b(?<num>\d+(\.\d*)?){TimeUnitRegex}";
		public const string NowRegex = @"\b(?<now>(right\s+)?now|as soon as possible|asap|recently|previously)\b";
		public const string SuffixRegex = @"^\s*(in the\s+)?(morning|afternoon|evening|night)\b";
		public const string DateTimeTimeOfDayRegex = @"\b(?<timeOfDay>morning|afternoon|night|evening)\b";
		public static readonly string DateTimeSpecificTimeOfDayRegex = $@"\b(({RelativeRegex}\s+{DateTimeTimeOfDayRegex})\b|\btonight)\b";
		public static readonly string TimeOfTodayAfterRegex = $@"^\s*(,\s*)?(in\s+)?{DateTimeSpecificTimeOfDayRegex}";
		public static readonly string TimeOfTodayBeforeRegex = $@"{DateTimeSpecificTimeOfDayRegex}(\s*,)?(\s+(at|around|in|on))?\s*$";
		public static readonly string SimpleTimeOfTodayAfterRegex = $@"({HourNumRegex}|{BaseDateTime.HourRegex})\s*(,\s*)?(in\s+)?{DateTimeSpecificTimeOfDayRegex}";
		public static readonly string SimpleTimeOfTodayBeforeRegex = $@"{DateTimeSpecificTimeOfDayRegex}(\s*,)?(\s+(at|around))?\s*({HourNumRegex}|{BaseDateTime.HourRegex})";
		public const string TheEndOfRegex = @"(the\s+)?end of(\s+the)?\s*$";
		public const string PeriodTimeOfDayRegex = @"\b((in\s+(the)?\s+)?((?<early>early(\s+|-))|(?<late>late(\s+|-)))?(?<timeOfDay>morning|afternoon|night|evening))\b";
		public static readonly string PeriodSpecificTimeOfDayRegex = $@"\b(({StrictRelativeRegex}\s+{PeriodTimeOfDayRegex})\b|\btonight)\b";
		public static readonly string PeriodTimeOfDayWithDateRegex = $@"\b(({TimeOfDayRegex}(\s+(on|of))?))\b";
		public const string LessThanRegex = @"\b(less\s+than)\s+$";
		public const string MoreThanRegex = @"\b(more\s+than)\s+$";
		public const string DurationUnitRegex = @"(?<unit>years|year|months|month|weeks|week|days|day|hours|hour|hrs|hr|h|minutes|minute|mins|min|seconds|second|secs|sec)\b";
		public const string SuffixAndRegex = @"(?<suffix>\s*(and)\s+((an|a)\s+)?(?<suffix_num>half|quarter))";
		public const string PeriodicRegex = @"\b(?<periodic>daily|monthly|weekly|biweekly|yearly|annually|annual)\b";
		public static readonly string EachUnitRegex = $@"(?<each>(each|every)(?<other>\s+other)?\s*{DurationUnitRegex})";
		public const string EachPrefixRegex = @"\b(?<each>(each|(every))\s*$)";
		public const string SetEachRegex = @"\b(?<each>(each|(every))\s*)";
		public const string SetLastRegex = @"(?<last>next|upcoming|this|last|past|previous|current)";
		public const string EachDayRegex = @"^\s*(each|every)\s*day\b";
		public static readonly string DurationFollowedUnit = $@"^\s*{SuffixAndRegex}?(\s+|-)?{DurationUnitRegex}";
		public static readonly string NumberCombinedWithDurationUnit = $@"\b(?<num>\d+(\.\d*)?)(-)?{DurationUnitRegex}";
		public static readonly string AnUnitRegex = $@"\b((?<half>half\s+)?(an|a)|another)\s+{DurationUnitRegex}";
		public const string AllRegex = @"\b(?<all>(all|full|whole)(\s+|-)(?<unit>year|month|week|day))\b";
		public const string HalfRegex = @"(((a|an)\s*)|\b)(?<half>half\s+(?<unit>year|month|week|day|hour))\b";
		public const string ConjunctionRegex = @"\b((and(\s+for)?)|with)\b";
		public static readonly string HolidayRegex1 = $@"\b(?<holiday>clean monday|good friday|ash wednesday|mardi gras|washington's birthday|mao's birthday|chinese new Year|new years' eve|new year's eve|new year 's eve|new years eve|new year eve|new years'|new year's|new year 's|new years|new year|mayday|yuan dan|april fools|christmas eve|christmas|xmas|thanksgiving|halloween|yuandan|easter)(\s+(of\s+)?({YearRegex}|{RelativeRegex}\s+year))?\b";
		public static readonly string HolidayRegex2 = $@"\b(?<holiday>martin luther king|martin luther king jr|all saint's|tree planting day|white lover|st patrick|st george|cinco de mayo|independence|us independence|all hallow|all souls|guy fawkes)(\s+(of\s+)?({YearRegex}|{RelativeRegex}\s+year))?\b";
		public static readonly string HolidayRegex3 = $@"(?<holiday>(canberra|easter|columbus|thanks\s*giving|christmas|xmas|labour|mother's|mother|mothers|father's|father|fathers|female|single|teacher's|youth|children|arbor|girls|chsmilbuild|lover|labor|inauguration|groundhog|valentine's|baptiste|bastille|halloween|veterans|memorial|mid(-| )autumn|moon|spring|lantern|qingming|dragon boat|new years'|new year's|new year 's|new years|new year)\s+(day))(\s+(of\s+)?({YearRegex}|{RelativeRegex}\s+year))?";
		public const string DateTokenPrefix = "on ";
		public const string TimeTokenPrefix = "at ";
		public const string TokenBeforeDate = "on ";
		public const string TokenBeforeTime = "at ";
		public const string AMTimeRegex = @"(?<am>morning)";
		public const string PMTimeRegex = @"\b(?<pm>afternoon|evening|night)\b";
		public const string InclusiveModPrepositions = @"(?<include>((on|in|at)\s+or\s+)|(\s+or\s+(on|in|at)))";
		public static readonly string BeforeRegex = $@"\b{InclusiveModPrepositions}?(before|in advance of|prior to|(no later|earlier|sooner) than|ending (with|on)|by|till|til|until){InclusiveModPrepositions}?\b\s*";
		public static readonly string AfterRegex = $@"\b{InclusiveModPrepositions}?(after(?!\s+or equal to)|(?<!no\s+)later than){InclusiveModPrepositions}?\b\s*";
		public const string SinceRegex = @"\b(since|after or equal to|starting (from|on|with))\b\s*";
		public const string AgoRegex = @"\b(ago)\b";
		public const string LaterRegex = @"\b(later|from now)\b";
		public const string InConnectorRegex = @"\b(in)\b";
		public static readonly string WithinNextPrefixRegex = $@"\b(within(\s+the)?(\s+{NextPrefixRegex})?)\b";
		public const string AmPmDescRegex = @"(ampm)";
		public static readonly string MorningStartEndRegex = $@"(^(morning|{AmDescRegex}))|((morning|{AmDescRegex})$)";
		public static readonly string AfternoonStartEndRegex = $@"(^(afternoon|{PmDescRegex}))|((afternoon|{PmDescRegex})$)";
		public const string EveningStartEndRegex = @"(^(evening))|((evening)$)";
		public const string NightStartEndRegex = @"(^(overnight|tonight|night))|((overnight|tonight|night)$)";
		public const string InexactNumberRegex = @"\b(a few|few|some|several|(?<NumTwoTerm>(a\s+)?couple of))\b";
		public static readonly string InexactNumberUnitRegex = $@"({InexactNumberRegex})\s+({DurationUnitRegex})";
		public static readonly string RelativeTimeUnitRegex = $@"((({NextPrefixRegex}|{PastPrefixRegex}|{ThisPrefixRegex})\s+({TimeUnitRegex}))|((the|my))\s+({RestrictedTimeUnitRegex}))";
		public static readonly string RelativeDurationUnitRegex = $@"(((?<=({NextPrefixRegex}|{PastPrefixRegex}|{ThisPrefixRegex})\s+)({DurationUnitRegex}))|((the|my))\s+({RestrictedTimeUnitRegex}))";
		public static readonly string ReferenceDatePeriodRegex = $@"\b{ReferencePrefixRegex}\s+(?<duration>week|month|year|weekend)\b";
		public const string ConnectorRegex = @"^(-|,|for|t|around)$";
		public const string FromToRegex = @"\b(from).+(to)\b.+";
		public const string SingleAmbiguousMonthRegex = @"^(the\s+)?(may|march)$";
		public const string PrepositionSuffixRegex = @"\b(on|in|at|around|from|to)$";
		public const string FlexibleDayRegex = @"(?<DayOfMonth>([A-Za-z]+\s)?[A-Za-z\d]+)";
		public static readonly string ForTheRegex = $@"\b(((for the {FlexibleDayRegex})|(on (the\s+)?{FlexibleDayRegex}(?<=(st|nd|rd|th))))(?<end>\s*(,|\.|!|\?|$)))";
		public static readonly string WeekDayAndDayOfMonthRegex = $@"\b{WeekDayRegex}\s+(the\s+{FlexibleDayRegex})\b";
		public const string RestOfDateRegex = @"\bRest\s+(of\s+)?((the|my|this|current)\s+)?(?<duration>week|month|year)\b";
		public const string RestOfDateTimeRegex = @"\bRest\s+(of\s+)?((the|my|this|current)\s+)?(?<unit>day)\b";
		public const string MealTimeRegex = @"\b(at\s+)?(?<mealTime>lunchtime)\b";
		public static readonly string NumberEndingPattern = $@"^(\s+(?<meeting>meeting|appointment|conference|call|skype call)\s+to\s+(?<newTime>{PeriodHourNumRegex}|{HourRegex})((\.)?$|(\.,|,|!|\?)))";
		public const string OneOnOneRegex = @"\b(1\s*:\s*1)|(one (on )?one|one\s*-\s*one|one\s*:\s*one)\b";
		public static readonly string LaterEarlyPeriodRegex = $@"\b({PrefixPeriodRegex})\s+(?<suffix>{OneWordPeriodRegex})\b";
		public static readonly string WeekWithWeekDayRangeRegex = $@"\b((?<week>({NextPrefixRegex}|{PastPrefixRegex}|this)\s+week)((\s+between\s+{WeekDayRegex}\s+and\s+{WeekDayRegex})|(\s+from\s+{WeekDayRegex}\s+to\s+{WeekDayRegex})))\b";
		public const string GeneralEndingRegex = @"^\s*((\.,)|\.|,|!|\?)?\s*$";
		public const string MiddlePauseRegex = @"\s*(,)\s*";
		public const string DurationConnectorRegex = @"^\s*(?<connector>\s+|and|,)\s*$";
		public const string PrefixArticleRegex = @"\bthe\s+";
		public const string OrRegex = @"\s*((\b|,\s*)(or|and)\b|,)\s*";
		public static readonly string YearPlusNumberRegex = $@"\b(Year\s+((?<year>(\d{{3,4}}))|{FullTextYearRegex}))\b";
		public static readonly string NumberAsTimeRegex = $@"\b({WrittenTimeRegex}|{PeriodHourNumRegex}|{BaseDateTime.HourRegex})\b";
		public static readonly string TimeBeforeAfterRegex = $@"\b(((?<=\b(before|no later than|by|after)\s+)({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}|{MidTimeRegex}))|{MidTimeRegex})\b";
		public const string DateNumberConnectorRegex = @"^\s*(?<connector>\s+at)\s*$";
		public const string DecadeRegex = @"(?<decade>noughties|twenties|thirties|forties|fifties|sixties|seventies|eighties|nineties|two thousands)";
		public static readonly string DecadeWithCenturyRegex = $@"(the\s+)?(((?<century>\d|1\d|2\d)?(')?(?<decade>\d0)(')?s)|(({CenturyRegex}(\s+|-)(and\s+)?)?{DecadeRegex})|({CenturyRegex}(\s+|-)(and\s+)?(?<decade>tens|hundreds)))";
		public static readonly string RelativeDecadeRegex = $@"\b((the\s+)?{RelativeRegex}\s+((?<number>[\w,]+)\s+)?decades?)\b";
		public const string YearAfterRegex = @"\b(or\s+(above|after))\b";
		public static readonly string YearPeriodRegex = $@"(((from|during|in|between)\s+)?{YearRegex}\s*({TillRegex}|{RangeConnectorRegex})\s*{YearRegex})";
		public static readonly string ComplexDatePeriodRegex = $@"((from|during|in|between)\s+)?(?<start>.+)\s*({TillRegex}|{RangeConnectorRegex})\s*(?<end>.+)";
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
			{ "tues", 2 },
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
			{ "21th", 21 },
			{ "22nd", 22 },
			{ "22th", 22 },
			{ "23rd", 23 },
			{ "23th", 23 },
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
		public static readonly Dictionary<string, IEnumerable<string>> HolidayNames = new Dictionary<string, IEnumerable<string>>
		{
			{ "easterday", new string[] { "easterday", "easter" } },
			{ "fathers", new string[] { "fatherday", "fathersday" } },
			{ "mothers", new string[] { "motherday", "mothersday" } },
			{ "thanksgiving", new string[] { "thanksgivingday", "thanksgiving" } },
			{ "martinlutherking", new string[] { "martinlutherkingday", "martinlutherkingjrday" } },
			{ "washingtonsbirthday", new string[] { "washingtonsbirthday", "washingtonbirthday" } },
			{ "canberra", new string[] { "canberraday" } },
			{ "labour", new string[] { "labourday", "laborday" } },
			{ "columbus", new string[] { "columbusday" } },
			{ "memorial", new string[] { "memorialday" } },
			{ "yuandan", new string[] { "yuandan" } },
			{ "maosbirthday", new string[] { "maosbirthday" } },
			{ "teachersday", new string[] { "teachersday", "teacherday" } },
			{ "singleday", new string[] { "singleday" } },
			{ "allsaintsday", new string[] { "allsaintsday" } },
			{ "youthday", new string[] { "youthday" } },
			{ "childrenday", new string[] { "childrenday", "childday" } },
			{ "femaleday", new string[] { "femaleday" } },
			{ "treeplantingday", new string[] { "treeplantingday" } },
			{ "arborday", new string[] { "arborday" } },
			{ "girlsday", new string[] { "girlsday" } },
			{ "whiteloverday", new string[] { "whiteloverday" } },
			{ "loverday", new string[] { "loverday" } },
			{ "christmas", new string[] { "christmasday", "christmas" } },
			{ "xmas", new string[] { "xmasday", "xmas" } },
			{ "newyear", new string[] { "newyear" } },
			{ "newyearday", new string[] { "newyearday" } },
			{ "newyearsday", new string[] { "newyearsday" } },
			{ "inaugurationday", new string[] { "inaugurationday" } },
			{ "groundhougday", new string[] { "groundhougday" } },
			{ "valentinesday", new string[] { "valentinesday" } },
			{ "stpatrickday", new string[] { "stpatrickday" } },
			{ "aprilfools", new string[] { "aprilfools" } },
			{ "stgeorgeday", new string[] { "stgeorgeday" } },
			{ "mayday", new string[] { "mayday" } },
			{ "cincodemayoday", new string[] { "cincodemayoday" } },
			{ "baptisteday", new string[] { "baptisteday" } },
			{ "usindependenceday", new string[] { "usindependenceday" } },
			{ "independenceday", new string[] { "independenceday" } },
			{ "bastilleday", new string[] { "bastilleday" } },
			{ "halloweenday", new string[] { "halloweenday" } },
			{ "allhallowday", new string[] { "allhallowday" } },
			{ "allsoulsday", new string[] { "allsoulsday" } },
			{ "guyfawkesday", new string[] { "guyfawkesday" } },
			{ "veteransday", new string[] { "veteransday" } },
			{ "christmaseve", new string[] { "christmaseve" } },
			{ "newyeareve", new string[] { "newyearseve", "newyeareve" } }
		};
		public static readonly Dictionary<string, int> WrittenDecades = new Dictionary<string, int>
		{
			{ "hundreds", 0 },
			{ "tens", 10 },
			{ "twenties", 20 },
			{ "thirties", 30 },
			{ "forties", 40 },
			{ "fifties", 50 },
			{ "sixties", 60 },
			{ "seventies", 70 },
			{ "eighties", 80 },
			{ "nineties", 90 }
		};
		public static readonly Dictionary<string, int> SpecialDecadeCases = new Dictionary<string, int>
		{
			{ "noughties", 2000 },
			{ "two thousands", 2000 }
		};
		public const string DefaultLanguageFallback = "MDY";
	}
}
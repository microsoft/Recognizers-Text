// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

import { BaseDateTime } from "./baseDateTime";
export namespace EnglishDateTime {
	export const TillRegex = `(?<till>to|till|til|until|thru|through|--|-|—|——)`;
	export const RangeConnectorRegex = `(?<and>and|through|to|--|-|—|——)`;
	export const RelativeRegex = `(?<order>next|upcoming|this|last|past|previous|current|the|my)`;
	export const NextPrefixRegex = `(next|upcoming)\\b`;
	export const PastPrefixRegex = `(last|past|previous)\\b`;
	export const ThisPrefixRegex = `(this|current|over the)\\b`;
	export const DayRegex = `(the\\s*)?(?<day>01|02|03|04|05|06|07|08|09|10th|10|11th|11st|11|12nd|12th|12|13rd|13th|13|14th|14|15th|15|16th|16|17th|17|18th|18|19th|19|1st|1|20th|20|21st|21|22nd|22|23rd|23|24th|24|25th|25|26th|26|27th|27|28th|28|29th|29|2nd|2|30th|30|31st|31|3rd|3|4th|4|5th|5|6th|6|7th|7|8th|8|9th|9)(?=\\b|t)`;
	export const MonthNumRegex = `(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)\\b`;
	export const PeriodYearRegex = `\\b(?<year>19\\d{2}|20\\d{2})\\b`;
	export const WeekDayRegex = `\\b(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tue|Wedn|Weds|Wed|Thurs|Thur|Thu|Fri|Sat|Sun)\\b`;
	export const SingleWeekDayRegex = `\\b(?<weekday>Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Mon|Tue|Wedn|Weds|Wed|Thurs|Thur|Thu|Fri)\\b`;
	export const RelativeMonthRegex = `(?<relmonth>${RelativeRegex}\\s+month)\\b`;
	export const EngMonthRegex = `(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sept|Sep)`;
	export const MonthSuffixRegex = `(?<msuf>(in\\s+|of\\s+|on\\s+)?(${RelativeMonthRegex}|${EngMonthRegex}))`;
	export const DateUnitRegex = `(?<unit>years|year|months|month|weeks|week|days|day)\\b`;
	export const SimpleCasesRegex = `\\b((from|between)\\s+)?(${DayRegex})\\s*${TillRegex}\\s*(${DayRegex})\\s+${MonthSuffixRegex}((\\s+|\\s*,\\s*)${PeriodYearRegex})?\\b`;
	export const MonthFrontSimpleCasesRegex = `\\b((from|between)\\s+)?${MonthSuffixRegex}\\s+((from|between)\\s+)?(${DayRegex})\\s*${TillRegex}\\s*(${DayRegex})((\\s+|\\s*,\\s*)${PeriodYearRegex})?\\b`;
	export const MonthFrontBetweenRegex = `\\b${MonthSuffixRegex}\\s+(between\\s+)(${DayRegex})\\s*${RangeConnectorRegex}\\s*(${DayRegex})((\\s+|\\s*,\\s*)${PeriodYearRegex})?\\b`;
	export const BetweenRegex = `\\b(between\\s+)(${DayRegex})\\s*${RangeConnectorRegex}\\s*(${DayRegex})\\s+${MonthSuffixRegex}((\\s+|\\s*,\\s*)${PeriodYearRegex})?\\b`;
	export const MonthWithYear = `\\b((?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sep|Sept),?(\\s+of)?\\s+(${PeriodYearRegex}|(?<order>next|last|this)\\s+year))`;
	export const OneWordPeriodRegex = `\\b((${RelativeRegex}\\s+)?(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sep|Sept)|(${RelativeRegex}\\s+)?(weekend|week|month|year)|weekend|(month|year) to date)\\b`;
	export const MonthNumWithYear = `(${PeriodYearRegex}[/\\-\\.]${MonthNumRegex})|(${MonthNumRegex}[/\\-]${PeriodYearRegex})`;
	export const WeekOfMonthRegex = `(?<wom>(the\\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\\s+week\\s+${MonthSuffixRegex})`;
	export const WeekOfYearRegex = `(?<woy>(the\\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\\s+week(\\s+of)?\\s+(${PeriodYearRegex}|${RelativeRegex}\\s+year))`;
	export const FollowedDateUnit = `^\\s*${DateUnitRegex}`;
	export const NumberCombinedWithDateUnit = `\\b(?<num>\\d+(\\.\\d*)?)${DateUnitRegex}`;
	export const QuarterRegex = `(the\\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th)\\s+quarter(\\s+of|\\s*,\\s*)?\\s+(${PeriodYearRegex}|${RelativeRegex}\\s+year)`;
	export const QuarterRegexYearFront = `(${PeriodYearRegex}|${RelativeRegex}\\s+year)\\s+(the\\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th)\\s+quarter`;
	export const SeasonRegex = `\\b(?<season>(${RelativeRegex}\\s+)?(?<seas>spring|summer|fall|autumn|winter)((\\s+of|\\s*,\\s*)?\\s+(${PeriodYearRegex}|${RelativeRegex}\\s+year))?)\\b`;
	export const WhichWeekRegex = `(week)(\\s*)(?<number>\\d\\d|\\d|0\\d)`;
	export const WeekOfRegex = `(week)(\\s*)(of)`;
	export const MonthOfRegex = `(month)(\\s*)(of)`;
	export const MonthRegex = `(?<month>April|Apr|August|Aug|December|Dec|February|Feb|January|Jan|July|Jul|June|Jun|March|Mar|May|November|Nov|October|Oct|September|Sept|Sep)`;
	export const DateYearRegex = `(?<year>19\\d{2}|20\\d{2}|((9\\d|0\\d|1\\d|2\\d)(?!\\s*\\:)))`;
	export const OnRegex = `(?<=\\bon\\s+)(${DayRegex}s?)\\b`;
	export const RelaxedOnRegex = `(?<=\\b(on|at|in)\\s+)((?<day>10th|11th|11st|12nd|12th|13rd|13th|14th|15th|16th|17th|18th|19th|1st|20th|21st|22nd|23rd|24th|25th|26th|27th|28th|29th|2nd|30th|31st|3rd|4th|5th|6th|7th|8th|9th)s?)\\b`;
	export const ThisRegex = `\\b((this(\\s*week)?\\s+)${WeekDayRegex})|(${WeekDayRegex}(\\s+this\\s*week))\\b`;
	export const LastDateRegex = `\\b(${PastPrefixRegex}(\\s*week)?\\s+${WeekDayRegex})|(${WeekDayRegex}(\\s+last\\s*week))\\b`;
	export const NextDateRegex = `\\b(${NextPrefixRegex}(\\s*week(\\s*on)?)?\\s+${WeekDayRegex})|((on\\s+)?${WeekDayRegex}(\\s+next\\s*week))\\b`;
	export const SpecialDayRegex = `\\b((the\\s+)?day before yesterday|(the\\s+)?day after (tomorrow|tmr)|((the\\s+)?${RelativeRegex}\\s+day)|yesterday|tomorrow|tmr|today)\\b`;
	export const RelativeDayRegex = `\\b(((the\\s+)?${RelativeRegex}\\s+day))\\b`;
	export const SetWeekDayRegex = `\\b(?<prefix>on\\s+)?(?<weekday>morning|afternoon|evening|night|Sunday|Monday|Tuesday|Wednesday|Thursday|Friday|Saturday)s\\b`;
	export const WeekDayOfMonthRegex = `(?<wom>(the\\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\\s+${WeekDayRegex}\\s+${MonthSuffixRegex})`;
	export const SpecialDate = `(?=\\b(on|at)\\s+the\\s+)${DayRegex}\\b`;
	export const DateExtractor1 = `\\b(${WeekDayRegex}(\\s+|\\s*,\\s*))?${MonthRegex}\\s*[/\\\\\\.\\-]?\\s*${DayRegex}\\b`;
	export const DateExtractor2 = `\\b(${WeekDayRegex}(\\s+|\\s*,\\s*))?${MonthRegex}\\s*[\\.\\-]?\\s*${DayRegex}(\\s+|\\s*,\\s*|\\s+of\\s+)${DateYearRegex}\\b`;
	export const DateExtractor3 = `\\b(${WeekDayRegex}(\\s+|\\s*,\\s*))?${DayRegex}(\\s+|\\s*,\\s*|\\s+of\\s+|\\s*-\\s*)${MonthRegex}((\\s+|\\s*,\\s*)${DateYearRegex})?\\b`;
	export const DateExtractor4 = `\\b${MonthNumRegex}\\s*[/\\\\\\-]\\s*${DayRegex}\\s*[/\\\\\\-]\\s*${DateYearRegex}`;
	export const DateExtractor5 = `\\b${DayRegex}\\s*[/\\\\\\-]\\s*${MonthNumRegex}\\s*[/\\\\\\-]\\s*${DateYearRegex}`;
	export const DateExtractor6 = `(?<=\\b(on|in|at)\\s+)${MonthNumRegex}[\\-\\.]${DayRegex}\\b`;
	export const DateExtractor7 = `\\b${MonthNumRegex}\\s*/\\s*${DayRegex}((\\s+|\\s*,\\s*|\\s+of\\s+)${DateYearRegex})?\\b`;
	export const DateExtractor8 = `(?<=\\b(on|in|at)\\s+)${DayRegex}[\\\\\\-]${MonthNumRegex}\\b`;
	export const DateExtractor9 = `\\b${DayRegex}\\s*/\\s*${MonthNumRegex}((\\s+|\\s*,\\s*|\\s+of\\s+)${DateYearRegex})?\\b`;
	export const DateExtractorA = `\\b${DateYearRegex}\\s*[/\\\\\\-]\\s*${MonthNumRegex}\\s*[/\\\\\\-]\\s*${DayRegex}`;
	export const OfMonth = `^\\s*of\\s*${MonthRegex}`;
	export const MonthEnd = `${MonthRegex}\\s*(the)?\\s*$`;
	export const RangeUnitRegex = `\\b(?<unit>years|year|months|month|weeks|week)\\b`;
	export const DescRegex = `(?<desc>ampm|am\\b|a\\.m\\.|a m\\b|a\\. m\\.|a\\.m\\b|a\\. m\\b|a m\\b|pm\\b|p\\.m\\.|p m\\b|p\\. m\\.|p\\.m\\b|p\\. m\\b|p\\b|p m\\b)`;
	export const HourNumRegex = `\\b(?<hournum>zero|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve)\\b`;
	export const MinuteNumRegex = `(?<minnum>one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|thirty|forty|fifty)`;
	export const DeltaMinuteNumRegex = `(?<deltaminnum>one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|thirty|forty|fifty)`;
	export const OclockRegex = `(?<oclock>o\\s*’\\s*clock|o\\s*‘\\s*clock|o\\s*'\\s*clock|o\\s*clock)`;
	export const PmRegex = `(?<pm>(in the\\s+)?afternoon|(in the\\s+)?evening|(in the\\s+)?night)`;
	export const AmRegex = `(?<am>(in the\\s+)?morning)`;
	export const LessThanOneHour = `(?<lth>(a\\s+)?quarter|three quarter(s)?|half( an hour)?|${BaseDateTime.DeltaMinuteRegex}(\\s+(minute|minutes|min|mins))|${DeltaMinuteNumRegex}(\\s+(minute|minutes|min|mins)))`;
	export const EngTimeRegex = `(?<engtime>${HourNumRegex}\\s+(${MinuteNumRegex}|(?<tens>twenty|thirty|forty|fourty|fifty)\\s+${MinuteNumRegex}))`;
	export const TimePrefix = `(?<prefix>(${LessThanOneHour} past|${LessThanOneHour} to))`;
	export const TimeSuffix = `(?<suffix>${AmRegex}|${PmRegex}|${OclockRegex})`;
	export const BasicTime = `(?<basictime>${EngTimeRegex}|${HourNumRegex}|${BaseDateTime.HourRegex}:${BaseDateTime.MinuteRegex}(:${BaseDateTime.SecondRegex})?|${BaseDateTime.HourRegex})`;
	export const MidnightRegex = `(?<midnight>midnight|mid-night|mid night)`;
	export const MidmorningRegex = `(?<midmorning>midmorning|mid-morning|mid morning)`;
	export const MidafternoonRegex = `(?<midafternoon>midafternoon|mid-afternoon|mid afternoon)`;
	export const MiddayRegex = `(?<midday>midday|mid-day|mid day|((12\\s)?noon))`;
	export const MidTimeRegex = `(?<mid>(${MidnightRegex}|${MidmorningRegex}|${MidafternoonRegex}|${MiddayRegex}))`;
	export const AtRegex = `\\b(((?<=\\bat\\s+)(${EngTimeRegex}|${HourNumRegex}|${BaseDateTime.HourRegex}|${MidTimeRegex}))|${MidTimeRegex})\\b`;
	export const IshRegex = `\\b(${BaseDateTime.HourRegex}(-|——)?ish|noonish|noon)\\b`;
	export const TimeUnitRegex = `(?<unit>hours|hour|hrs|hr|h|minutes|minute|mins|min|seconds|second|secs|sec)\\b`;
	export const FivesRegex = `(?<tens>(fifteen|twenty(\\s*five)?|thirty(\\s*five)?|forty(\\s*five)?|fourty(\\s*five)?|fifty(\\s*five)?|ten|five))\\b`;
	export const HourRegex = `(?<hour>00|01|02|03|04|05|06|07|08|09|0|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|1|2|3|4|5|6|7|8|9)`;
	export const PeriodHourNumRegex = `(?<hour>twenty one|twenty two|twenty three|twenty four|zero|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nighteen|twenty)`;
	export const PeriodDescRegex = `(?<desc>pm|am|p\\.m\\.|a\\.m\\.|p|a m\\b|p m\\b)`;
	export const PeriodPmRegex = `(?<pm>afternoon|evening|in the afternoon|in the evening|in the night)s?`;
	export const PeriodAmRegex = `(?<am>morning|in the morning)s?`;
	export const ConnectNumRegex = `${BaseDateTime.HourRegex}(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\\s*${DescRegex}`;
	export const TimeRegex1 = `\\b(${TimePrefix}\\s+)?(${EngTimeRegex}|${HourNumRegex}|${BaseDateTime.HourRegex})\\s*${DescRegex}`;
	export const TimeRegex2 = `(\\b${TimePrefix}\\s+)?(T)?${BaseDateTime.HourRegex}(\\s*)?:(\\s*)?${BaseDateTime.MinuteRegex}((\\s*)?:(\\s*)?${BaseDateTime.SecondRegex})?((\\s*${DescRegex})|\\b)`;
	export const TimeRegex3 = `(\\b${TimePrefix}\\s+)?${BaseDateTime.HourRegex}\\.${BaseDateTime.MinuteRegex}(\\s*${DescRegex})`;
	export const TimeRegex4 = `\\b${TimePrefix}\\s+${BasicTime}(\\s*${DescRegex})?\\s+${TimeSuffix}\\b`;
	export const TimeRegex5 = `\\b${TimePrefix}\\s+${BasicTime}((\\s*${DescRegex})|\\b)`;
	export const TimeRegex6 = `${BasicTime}(\\s*${DescRegex})?\\s+${TimeSuffix}\\b`;
	export const TimeRegex7 = `\\b${TimeSuffix}\\s+at\\s+${BasicTime}((\\s*${DescRegex})|\\b)`;
	export const TimeRegex8 = `\\b${TimeSuffix}\\s+${BasicTime}((\\s*${DescRegex})|\\b)`;
	export const TimeRegex9 = `\\b${PeriodHourNumRegex}\\s+${FivesRegex}((\\s*${DescRegex})|\\b)`;
	export const PureNumFromTo = `((from|between)\\s+)?(${HourRegex}|${PeriodHourNumRegex})(\\s*(?<leftDesc>${PeriodDescRegex}))?\\s*${TillRegex}\\s*(${HourRegex}|${PeriodHourNumRegex})\\s*(?<rightDesc>${PmRegex}|${AmRegex}|${PeriodDescRegex})?`;
	export const PureNumBetweenAnd = `(between\\s+)(${HourRegex}|${PeriodHourNumRegex})(\\s*(?<leftDesc>${PeriodDescRegex}))?\\s*${RangeConnectorRegex}\\s*(${HourRegex}|${PeriodHourNumRegex})\\s*(?<rightDesc>${PmRegex}|${AmRegex}|${PeriodDescRegex})?`;
	export const PrepositionRegex = `(?<prep>^(at|on|of)(\\s+the)?$)`;
	export const TimeOfDayRegex = `\\b(?<timeOfDay>((((in\\s+(the)?\\s+)?((?<early>early(\\s+|-))|(?<late>late(\\s+|-)))?(morning|afternoon|night|evening)))|(((in\\s+(the)?\\s+)?)(daytime)))s?)\\b`;
	export const SpecificTimeOfDayRegex = `\\b((${RelativeRegex}\\s+${TimeOfDayRegex})\\b|\\btonight)s?\\b`;
	export const TimeFollowedUnit = `^\\s*${TimeUnitRegex}`;
	export const TimeNumberCombinedWithUnit = `\\b(?<num>\\d+(\\.\\d*)?)${TimeUnitRegex}`;
	export const NowRegex = `\\b(?<now>(right\\s+)?now|as soon as possible|asap|recently|previously)\\b`;
	export const SuffixRegex = `^\\s*(in the\\s+)?(morning|afternoon|evening|night)\\b`;
	export const DateTimeTimeOfDayRegex = `\\b(?<timeOfDay>morning|afternoon|night|evening)\\b`;
	export const DateTimeSpecificTimeOfDayRegex = `\\b((${RelativeRegex}\\s+${DateTimeTimeOfDayRegex})\\b|\\btonight)\\b`;
	export const TimeOfTodayAfterRegex = `^\\s*(,\\s*)?(in\\s+)?${DateTimeSpecificTimeOfDayRegex}`;
	export const TimeOfTodayBeforeRegex = `${DateTimeSpecificTimeOfDayRegex}(\\s*,)?(\\s+(at|for))?\\s*$`;
	export const SimpleTimeOfTodayAfterRegex = `(${HourNumRegex}|${BaseDateTime.HourRegex})\\s*(,\\s*)?(in\\s+)?${DateTimeSpecificTimeOfDayRegex}`;
	export const SimpleTimeOfTodayBeforeRegex = `${DateTimeSpecificTimeOfDayRegex}(\\s*,)?(\\s+(at|around))?\\s*(${HourNumRegex}|${BaseDateTime.HourRegex})`;
	export const TheEndOfRegex = `(the\\s+)?end of(\\s+the)?\\s*$`;
	export const PeriodTimeOfDayRegex = `\\b((in\\s+(the)?\\s+)?((?<early>early(\\s+|-))|(?<late>late(\\s+|-)))?(?<timeOfDay>morning|afternoon|night|evening))\\b`;
	export const PeriodSpecificTimeOfDayRegex = `\\b((${RelativeRegex}\\s+${PeriodTimeOfDayRegex})\\b|\\btonight)\\b`;
	export const PeriodTimeOfDayWithDateRegex = `\\b((${TimeOfDayRegex}(\\s+on)?))\\b`;
	export const DurationUnitRegex = `(?<unit>years|year|months|month|weeks|week|days|day|hours|hour|hrs|hr|h|minutes|minute|mins|min|seconds|second|secs|sec)\\b`;
	export const SuffixAndRegex = `(?<suffix>\\s*(and)\\s+((an|a)\\s+)?(?<suffix_num>half|quarter))`;
	export const PeriodicRegex = `\\b(?<periodic>daily|monthly|weekly|biweekly|yearly|annually|annual)\\b`;
	export const EachUnitRegex = `(?<each>(each|every)(?<other>\\s+other)?\\s*${DurationUnitRegex})`;
	export const EachPrefixRegex = `\\b(?<each>(each|(every))\\s*$)`;
	export const SetEachRegex = `\\b(?<each>(each|(every))\\s*)`;
	export const SetLastRegex = `(?<last>next|upcoming|this|last|past|previous|current)`;
	export const EachDayRegex = `^\\s*(each|every)\\s*day\\b`;
	export const DurationFollowedUnit = `^\\s*${SuffixAndRegex}?(\\s+|-)?${DurationUnitRegex}`;
	export const NumberCombinedWithDurationUnit = `\\b(?<num>\\d+(\\.\\d*)?)(-)?${DurationUnitRegex}`;
	export const AnUnitRegex = `\\b(((?<half>half\\s+)?(an|a))|(an|a))\\s+${DurationUnitRegex}`;
	export const AllRegex = `\\b(?<all>all\\s+(?<unit>year|month|week|day))\\b`;
	export const HalfRegex = `(((a|an)\\s*)|\\b)(?<half>half\\s+(?<unit>year|month|week|day|hour))\\b`;
	export const ConjunctionRegex = `\\b((and(\\s+for)?)|with)\\b`;
	export const YearRegex = `\\b(?<year>19\\d{2}|20\\d{2})\\b`;
	export const HolidayRegex1 = `\\b(?<holiday>clean monday|good friday|ash wednesday|mardi gras|washington's birthday|mao's birthday|chinese new Year|new years' eve|new year's eve|new year 's eve|new years eve|new year eve|new years'|new year's|new year 's|new years|new year|mayday|yuan dan|april fools|christmas eve|christmas|xmas|thanksgiving|halloween|yuandan)(\\s+(of\\s+)?(${YearRegex}|${RelativeRegex}\\s+year))?\\b`;
	export const HolidayRegex2 = `\\b(?<holiday>martin luther king|martin luther king jr|all saint's|tree planting day|white lover|st patrick|st george|cinco de mayo|independence|us independence|all hallow|all souls|guy fawkes)(\\s+(of\\s+)?(${YearRegex}|${RelativeRegex}\\s+year))?\\b`;
	export const HolidayRegex3 = `(?<holiday>(canberra|easter|columbus|thanks\\s*giving|labour|mother's|mother|mothers|father's|father|fathers|female|single|teacher's|youth|children|arbor|girls|chsmilbuild|lover|labor|inauguration|groundhog|valentine's|baptiste|bastille|halloween|veterans|memorial|mid(-| )autumn|moon|spring|lantern|qingming|dragon boat|new years'|new year's|new year 's|new years|new year)\\s+(day))(\\s+(of\\s+)?(${YearRegex}|${RelativeRegex}\\s+year))?`;
	export const DateTokenPrefix = 'on ';
	export const TimeTokenPrefix = 'at ';
	export const TokenBeforeDate = 'on ';
	export const TokenBeforeTime = 'at ';
	export const AMTimeRegex = `(?<am>morning)`;
	export const PMTimeRegex = `\\b(?<pm>afternoon|evening|night)\\b`;
	export const BeforeRegex = `\\b(before)\\b`;
	export const AfterRegex = `\\b(after|since)\\b`;
	export const AgoRegex = `\\b(ago)\\b`;
	export const LaterRegex = `\\b(later|from now)\\b`;
	export const InConnectorRegex = `\\b(in)\\b`;
	export const AmDescRegex = `(am\\b|a\\.m\\.|a m\\b|a\\. m\\.|a\\.m\\b|a\\. m\\b|a m\\b)`;
	export const PmDescRegex = `(pm\\b|p\\.m\\.|p\\b|p m\\b|p\\. m\\.|p\\.m\\b|p\\. m\\b|p m\\b)`;
	export const AmPmDescRegex = `(ampm)`;
	export const MorningStartEndRegex = `(^(morning))|((morning)$)`;
	export const AfternoonStartEndRegex = `(^(afternoon))|((afternoon)$)`;
	export const EveningStartEndRegex = `(^(evening))|((evening)$)`;
	export const NightStartEndRegex = `(^(overnight|tonight|night))|((overnight|tonight|night)$)`;
	export const InExactNumberRegex = `\\b(a few|few|some|several)\\b`;
	export const InExactNumberUnitRegex = `(${InExactNumberRegex})\\s+(${DurationUnitRegex})`;
	export const RelativeTimeUnitRegex = `(${RelativeRegex})\\s+(${TimeUnitRegex})`;
	export const ConnectorRegex = `^(,|for|t|around)$`;
	export const FromToRegex = `\\b(from).+(to)\\b.+`;
	export const SingleAmbiguousMonthRegex = `^(the\\s+)?(may|march)$`;
	export const PrepositionSuffixRegex = `\\b(on|in|at|around|from|to)$`;
	export const ForTheRegex = `(for the (?<day>([A-Za-z]+\\s)?[A-Za-z\\d]+)(?<end>\\s*(,|\\.|!|\\?|$)))`;
	export const WeekDayAndDayOfMothRegex = `\\b${WeekDayRegex}(\\s+the\\s+)(?<DayOfMonth>([A-Za-z]+\\s)?[A-Za-z\\d]+)\\b`;
	export const UnitMap: ReadonlyMap<string, string> = new Map<string, string>([["years", "Y"],["year", "Y"],["months", "MON"],["month", "MON"],["weeks", "W"],["week", "W"],["days", "D"],["day", "D"],["hours", "H"],["hour", "H"],["hrs", "H"],["hr", "H"],["h", "H"],["minutes", "M"],["minute", "M"],["mins", "M"],["min", "M"],["seconds", "S"],["second", "S"],["secs", "S"],["sec", "S"]]);
	export const UnitValueMap: ReadonlyMap<string, number> = new Map<string, number>([["years", 31536000],["year", 31536000],["months", 2592000],["month", 2592000],["weeks", 604800],["week", 604800],["days", 86400],["day", 86400],["hours", 3600],["hour", 3600],["hrs", 3600],["hr", 3600],["h", 3600],["minutes", 60],["minute", 60],["mins", 60],["min", 60],["seconds", 1],["second", 1],["secs", 1],["sec", 1]]);
	export const SeasonMap: ReadonlyMap<string, string> = new Map<string, string>([["spring", "SP"],["summer", "SU"],["fall", "FA"],["autumn", "FA"],["winter", "WI"]]);
	export const SeasonValueMap: ReadonlyMap<string, number> = new Map<string, number>([["SP", 3],["SU", 6],["FA", 9],["WI", 12]]);
	export const CardinalMap: ReadonlyMap<string, number> = new Map<string, number>([["first", 1],["1st", 1],["second", 2],["2nd", 2],["third", 3],["3rd", 3],["fourth", 4],["4th", 4],["fifth", 5],["5th", 5]]);
	export const DayOfWeek: ReadonlyMap<string, number> = new Map<string, number>([["monday", 1],["tuesday", 2],["wednesday", 3],["thursday", 4],["friday", 5],["saturday", 6],["sunday", 0],["mon", 1],["tue", 2],["wed", 3],["wedn", 3],["weds", 3],["thu", 4],["thur", 4],["thurs", 4],["fri", 5],["sat", 6],["sun", 0]]);
	export const MonthOfYear: ReadonlyMap<string, number> = new Map<string, number>([["1", 1],["2", 2],["3", 3],["4", 4],["5", 5],["6", 6],["7", 7],["8", 8],["9", 9],["10", 10],["11", 11],["12", 12],["january", 1],["february", 2],["march", 3],["april", 4],["may", 5],["june", 6],["july", 7],["august", 8],["september", 9],["october", 10],["november", 11],["december", 12],["jan", 1],["feb", 2],["mar", 3],["apr", 4],["jun", 6],["jul", 7],["aug", 8],["sep", 9],["sept", 9],["oct", 10],["nov", 11],["dec", 12],["01", 1],["02", 2],["03", 3],["04", 4],["05", 5],["06", 6],["07", 7],["08", 8],["09", 9]]);
	export const Numbers: ReadonlyMap<string, number> = new Map<string, number>([["zero", 0],["one", 1],["a", 1],["an", 1],["two", 2],["three", 3],["four", 4],["five", 5],["six", 6],["seven", 7],["eight", 8],["nine", 9],["ten", 10],["eleven", 11],["twelve", 12],["thirteen", 13],["fourteen", 14],["fifteen", 15],["sixteen", 16],["seventeen", 17],["eighteen", 18],["nineteen", 19],["twenty", 20],["twenty one", 21],["twenty two", 22],["twenty three", 23],["twenty four", 24],["twenty five", 25],["twenty six", 26],["twenty seven", 27],["twenty eight", 28],["twenty nine", 29],["thirty", 30],["thirty one", 31],["thirty two", 32],["thirty three", 33],["thirty four", 34],["thirty five", 35],["thirty six", 36],["thirty seven", 37],["thirty eight", 38],["thirty nine", 39],["forty", 40],["forty one", 41],["forty two", 42],["forty three", 43],["forty four", 44],["forty five", 45],["forty six", 46],["forty seven", 47],["forty eight", 48],["forty nine", 49],["fifty", 50],["fifty one", 51],["fifty two", 52],["fifty three", 53],["fifty four", 54],["fifty five", 55],["fifty six", 56],["fifty seven", 57],["fifty eight", 58],["fifty nine", 59],["sixty", 60],["sixty one", 61],["sixty two", 62],["sixty three", 63],["sixty four", 64],["sixty five", 65],["sixty six", 66],["sixty seven", 67],["sixty eight", 68],["sixty nine", 69],["seventy", 70],["seventy one", 71],["seventy two", 72],["seventy three", 73],["seventy four", 74],["seventy five", 75],["seventy six", 76],["seventy seven", 77],["seventy eight", 78],["seventy nine", 79],["eighty", 80],["eighty one", 81],["eighty two", 82],["eighty three", 83],["eighty four", 84],["eighty five", 85],["eighty six", 86],["eighty seven", 87],["eighty eight", 88],["eighty nine", 89],["ninety", 90],["ninety one", 91],["ninety two", 92],["ninety three", 93],["ninety four", 94],["ninety five", 95],["ninety six", 96],["ninety seven", 97],["ninety eight", 98],["ninety nine", 99],["one hundred", 100]]);
	export const DayOfMonth: ReadonlyMap<string, number> = new Map<string, number>([["1st", 1],["2nd", 2],["3rd", 3],["4th", 4],["5th", 5],["6th", 6],["7th", 7],["8th", 8],["9th", 9],["10th", 10],["11th", 11],["11st", 11],["12th", 12],["12nd", 12],["13th", 13],["13rd", 13],["14th", 14],["15th", 15],["16th", 16],["17th", 17],["18th", 18],["19th", 19],["20th", 20],["21st", 21],["22nd", 22],["23rd", 23],["24th", 24],["25th", 25],["26th", 26],["27th", 27],["28th", 28],["29th", 29],["30th", 30],["31st", 31]]);
	export const DoubleNumbers: ReadonlyMap<string, number> = new Map<string, number>([["half", 0.5],["quarter", 0.25]]);
	export const HolidayNames: ReadonlyMap<string, string[]> = new Map<string, string[]>([["fathers", ["fatherday","fathersday"]],["mothers", ["motherday","mothersday"]],["thanksgiving", ["thanksgivingday","thanksgiving"]],["martinlutherking", ["martinlutherkingday","martinlutherkingjrday"]],["washingtonsbirthday", ["washingtonsbirthday","washingtonbirthday"]],["canberra", ["canberraday"]],["labour", ["labourday","laborday"]],["columbus", ["columbusday"]],["memorial", ["memorialday"]],["yuandan", ["yuandan"]],["maosbirthday", ["maosbirthday"]],["teachersday", ["teachersday","teacherday"]],["singleday", ["singleday"]],["allsaintsday", ["allsaintsday"]],["youthday", ["youthday"]],["childrenday", ["childrenday","childday"]],["femaleday", ["femaleday"]],["treeplantingday", ["treeplantingday"]],["arborday", ["arborday"]],["girlsday", ["girlsday"]],["whiteloverday", ["whiteloverday"]],["loverday", ["loverday"]],["christmas", ["christmas"]],["xmas", ["xmas"]],["newyear", ["newyear"]],["newyearday", ["newyearday"]],["newyearsday", ["newyearsday"]],["inaugurationday", ["inaugurationday"]],["groundhougday", ["groundhougday"]],["valentinesday", ["valentinesday"]],["stpatrickday", ["stpatrickday"]],["aprilfools", ["aprilfools"]],["stgeorgeday", ["stgeorgeday"]],["mayday", ["mayday"]],["cincodemayoday", ["cincodemayoday"]],["baptisteday", ["baptisteday"]],["usindependenceday", ["usindependenceday"]],["independenceday", ["independenceday"]],["bastilleday", ["bastilleday"]],["halloweenday", ["halloweenday"]],["allhallowday", ["allhallowday"]],["allsoulsday", ["allsoulsday"]],["guyfawkesday", ["guyfawkesday"]],["veteransday", ["veteransday"]],["christmaseve", ["christmaseve"]],["newyeareve", ["newyearseve","newyeareve"]]]);
}

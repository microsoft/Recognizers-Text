﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//     
//     Generation parameters:
//     - DataFilename: Patterns\Portuguese\Portuguese-DateTime.yaml
//     - Language: Portuguese
//     - ClassName: DateTimeDefinitions
// </auto-generated>
//------------------------------------------------------------------------------
namespace Microsoft.Recognizers.Definitions.Portuguese
{
	using System;
	using System.Collections.Generic;

	public static class DateTimeDefinitions
	{
		public const string TillRegex = @"(?<till>ate|as|às|até|ateh|a|ao|--|-|—|——)(\s+(o|[aà](s)?))?";
		public const string AndRegex = @"(?<and>e|e\s*o|--|-|—|——)";
		public const string DayRegex = @"(?<day>01|02|03|04|05|06|07|08|09|1|10|11|12|13|14|15|16|17|18|19|2|20|21|22|23|24|25|26|27|28|29|3|30|31|4|5|6|7|8|9)(?=\b|t)";
		public const string MonthNumRegex = @"(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)\b";
		public const string IllegalYearRegex = @"([-])(?<year>((1[5-9]|20)\d{2})|2100)([-])";
		public const string DescRegex = @"(?<desc>pm\b|am\b|p\.m\.|a\.m\.)";
		public const string AmDescRegex = @"(am\b|a\.m\.|a m\b|a\. m\.\b|a\.m\b|a\. m\b)";
		public const string PmDescRegex = @"(pm\b|p\.m\.|p\b|p m\b|p\. m\.\b|p\.m\b|p\. m\b)";
		public const string AmPmDescRegex = @"(ampm)";
		public const string FourDigitYearRegex = @"\b(?<year>(1[5-9]|20)\d{2})(?!\.0\b)\b";
		public static readonly string TwoDigitYearRegex = $@"\b(?<![$])(?<year>([0-27-9]\d))(?!(\s*((\:)|{AmDescRegex}|{PmDescRegex}|\.\d)))\b";
		public const string FullTextYearRegex = @"^[\*]";
		public static readonly string YearRegex = $@"({FourDigitYearRegex}|{FullTextYearRegex})";
		public const string RelativeMonthRegex = @"(?<relmonth>([nd]?es[st]e|pr[óo]ximo|passsado|[uú]ltimo)\s+m[eê]s)\b";
		public const string MonthRegex = @"(?<month>Abril|Abr|Agosto|Ago|Dezembro|Dez|Fevereiro|Fev|Janeiro|Jan|Julho|Jul|Junho|Jun|Mar[çc]o|Mar|Maio|Mai|Novembro|Nov|Outubro|Out|Septembro|Setembro|Sept|Set)";
		public static readonly string MonthSuffixRegex = $@"(?<msuf>((em|no)\s+|d[eo]\s+)?({RelativeMonthRegex}|{MonthRegex}))";
		public const string DateUnitRegex = @"(?<unit>anos|ano|meses|m[êe]s|semanas|semana|dia(s)?)\b";
		public const string PastRegex = @"(?<past>\b(passad(a|o)(s)?|[uú]ltim[oa](s)?|anterior(es)?|h[aá]|pr[ée]vi[oa](s)?)\b)";
		public const string FutureRegex = @"(?<past>\b(seguinte(s)?|pr[oó]xim[oa](s)?|dentro\s+de|em|daqui\s+a)\b)";
		public static readonly string SimpleCasesRegex = $@"\b((desde\s+[oa]|desde|d[oa])\s+)?(dia\s+)?({DayRegex})\s*{TillRegex}\s*(o dia\s+)?({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string MonthFrontSimpleCasesRegex = $@"\b{MonthSuffixRegex}\s+((desde\s+[oa]|desde|d[oa])\s+)?(dia\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string MonthFrontBetweenRegex = $@"\b{MonthSuffixRegex}\s+((entre|entre\s+[oa]s?)\s+)(dias?\s+)?({DayRegex})\s*{AndRegex}\s*({DayRegex})((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string DayBetweenRegex = $@"\b((entre|entre\s+[oa]s?)\s+)(dia\s+)?({DayRegex})\s*{AndRegex}\s*({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){YearRegex})?\b";
		public const string OneWordPeriodRegex = @"\b(((pr[oó]xim[oa]?|[nd]?es[st]e|aquel[ea]|[uú]ltim[oa]?|em)\s+)?(?<month>Abril|Abr|Agosto|Ago|Dezembro|Dez|Fevereiro|Fev|Janeiro|Jan|Julho|Jul|Junho|Jun|Mar[çc]o|Mar|Maio|Mai|Novembro|Nov|Outubro|Out|Septembro|Setembro|Sept|Set)|(?<=\b(de|do|da|o|a)\s+)?(pr[oó]xim[oa](s)?|[uú]ltim[oa]s?|est(e|a))\s+(fim de semana|fins de semana|semana|m[êe]s|ano)|fim de semana|fins de semana|(m[êe]s|anos)? [àa] data)\b";
		public static readonly string MonthWithYearRegex = $@"\b(((pr[oó]xim[oa](s)?|[nd]?es[st]e|aquele|[uú]ltim[oa]?|em)\s+)?(?<month><month>Abril|Abr|Agosto|Ago|Dezembro|Dez|Fevereiro|Fev|Janeiro|Jan|Julho|Jul|Junho|Jun|Mar[çc]o|Mar|Maio|Mai|Novembro|Nov|Outubro|Out|Septembro|Setembro|Sept|Set)\s+((de|do|da|o|a)\s+)?({YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|[nd]?es[st]e)\s+ano))\b";
		public static readonly string MonthNumWithYearRegex = $@"({YearRegex}(\s*?)[/\-\.](\s*?){MonthNumRegex})|({MonthNumRegex}(\s*?)[/\-](\s*?){YearRegex})";
		public static readonly string WeekOfMonthRegex = $@"(?<wom>(a|na\s+)?(?<cardinal>primeira?|1a|segunda|2a|terceira|3a|[qc]uarta|4a|quinta|5a|[uú]ltima)\s+semana\s+{MonthSuffixRegex})";
		public static readonly string WeekOfYearRegex = $@"(?<woy>(a|na\s+)?(?<cardinal>primeira?|1a|segunda|2a|terceira|3a|[qc]uarta|4a|quinta|5a|[uú]ltima?)\s+semana(\s+d[oe]?)?\s+({YearRegex}|(?<order>pr[oó]ximo|[uú]ltimo|[nd]?es[st]e)\s+ano))";
		public static readonly string FollowedDateUnit = $@"^\s*{DateUnitRegex}";
		public static readonly string NumberCombinedWithDateUnit = $@"\b(?<num>\d+(\.\d*)?){DateUnitRegex}";
		public static readonly string QuarterRegex = $@"(n?o\s+)?(?<cardinal>primeiro|1[oº]|segundo|2[oº]|terceiro|3[oº]|[qc]uarto|4[oº])\s+trimestre(\s+d[oe]|\s*,\s*)?\s+({YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|[nd]?es[st]e)\s+ano)";
		public static readonly string QuarterRegexYearFront = $@"({YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|[nd]?es[st]e)\s+ano)\s+(n?o\s+)?(?<cardinal>(primeiro)|1[oº]|segundo|2[oº]|terceiro|3[oº]|[qc]uarto|4[oº])\s+trimestre";
		public const string AllHalfYearRegex = @"^[.]";
		public const string PrefixDayRegex = @"^[.]";
		public static readonly string SeasonRegex = $@"\b(?<season>(([uú]ltim[oa]|[nd]?es[st][ea]|n?[oa]|(pr[oó]xim[oa]s?|seguinte))\s+)?(?<seas>primavera|ver[ãa]o|outono|inverno)((\s+)?(seguinte|((de\s+|,)?\s*{YearRegex})|((do\s+)?(?<order>pr[oó]ximo|[uú]ltimo|[nd]?es[st]e)\s+ano)))?)\b";
		public const string WhichWeekRegex = @"(semana)(\s*)(?<number>\d\d|\d|0\d)";
		public const string WeekOfRegex = @"(semana)(\s*)((do|da|de))";
		public const string MonthOfRegex = @"(mes)(\s*)((do|da|de))";
		public const string RangeUnitRegex = @"\b(?<unit>anos|ano|meses|m[êe]s|semanas|semana)\b";
		public const string InConnectorRegex = @"\b(em)\b";
		public const string WithinNextPrefixRegex = @"^[.]";
		public const string CenturySuffixRegex = @"^[.]";
		public const string FromRegex = @"((desde|de)(\s*a(s)?)?)$";
		public const string ConnectorAndRegex = @"(e\s*([àa](s)?)?)$";
		public const string BetweenRegex = @"(entre\s*([oa](s)?)?)";
		public const string WeekDayRegex = @"\b(?<weekday>Domingos?|Segundas?-feiras?|Segundas?\s+feiras?|Segundas?|2a|Ter[çca]s?-feiras?|Ter[çc]as?\s+feiras?|Ter[cça]s?|3a|Quartas?-feiras?|Quartas?\s+feiras?|Quartas?|4a|Quintas?-feiras?|Quintas?\s+feiras?|Quintas?|5a|Sextas?-feiras?|Sextas?\s+feiras?|Sextas?|6a|S[aá]bados?|2ª|3ª|4ª|5ª|6ª|Dom|Seg|Ter|Qua|Qui|Sex|Sab|Seg\.|Ter[cç]\.|Qua\.|Qui\.|Sex\.|S[aá]b\.|Dom\.)\b";
		public static readonly string OnRegex = $@"(?<=\b(em|no)\s+)({DayRegex}s?)\b";
		public const string RelaxedOnRegex = @"(?<=\b(em|n[oa]|d[oa])\s+)(dia\s+)?((?<day>10|11|12|13|14|15|16|17|18|19|1|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)s?)\b";
		public static readonly string ThisRegex = $@"\b(([nd]?es[st][ea]\s*){WeekDayRegex})|({WeekDayRegex}\s*([nd]?es[st]a\s+semana))\b";
		public static readonly string LastDateRegex = $@"\b(([uú]ltim[ao])\s*{WeekDayRegex})|({WeekDayRegex}(\s+(([nd]?es[st]a|na|da)\s+([uú]ltima\s+)?semana)))\b";
		public static readonly string NextDateRegex = $@"\b(((pr[oó]xim[oa]|seguinte)\s*){WeekDayRegex})|({WeekDayRegex}((\s+(pr[oó]xim[oa]|seguinte))|(\s+(da\s+)?(semana\s+seguinte|pr[oó]xima\s+semana))))\b";
		public const string SpecialDayRegex = @"\b((d?o\s+)?(dia\s+antes\s+de\s+ontem|antes\s+de\s+ontem|anteontem)|((d?o\s+)?(dia\s+|depois\s+|dia\s+depois\s+)?de\s+amanh[aã])|(o\s)?dia\s+seguinte|(o\s)?pr[oó]ximo\s+dia|(o\s+)?[uú]ltimo\s+dia|ontem|amanh[ãa]|hoje)|(do\s+dia$)\b";
		public const string SpecialDayWithNumRegex = @"^[.]";
		public const string ForTheRegex = @".^";
		public const string WeekDayAndDayOfMonthRegex = @".^";
		public static readonly string WeekDayOfMonthRegex = $@"(?<wom>(n?[ao]\s+)?(?<cardinal>primeir[ao]|1[ao]|segund[ao]|2[ao]|terceir[ao]|3[ao]|[qc]uart[ao]|4[ao]|quint[ao]|5[ao]|[uú]ltim[ao])\s+{WeekDayRegex}\s+{MonthSuffixRegex})";
		public const string RelativeWeekDayRegex = @"^[.]";
		public const string NumberEndingPattern = @"^[.]";
		public static readonly string SpecialDateRegex = $@"(?<=\bno\s+){DayRegex}\b";
		public static readonly string OfMonthRegex = $@"^\s*de\s*{MonthSuffixRegex}";
		public static readonly string MonthEndRegex = $@"({MonthRegex}\s*(o)?\s*$)";
		public static readonly string WeekDayEnd = $@"{WeekDayRegex}\s*,?\s*$";
		public static readonly string DateYearRegex = $@"(?<year>{YearRegex}|{TwoDigitYearRegex})";
		public static readonly string DateExtractor1 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}?((\s*(de)|[/\\\.\-])\s*)?{MonthRegex}\b";
		public static readonly string DateExtractor2 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}\s*([\.\-]|de)?\s*{MonthRegex}?(\s*(,|de)\s*){YearRegex}\b";
		public static readonly string DateExtractor3 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}(\s+|\s*,\s*|\s+de\s+|\s*-\s*){MonthRegex}((\s+|\s*(,|de)\s*){YearRegex})?\b";
		public static readonly string DateExtractor4 = $@"\b{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}\s*[/\\\-]\s*{DateYearRegex}";
		public static readonly string DateExtractor5 = $@"\b{DayRegex}\s*[/\\\-\.]\s*{MonthNumRegex}\s*[/\\\-\.]\s*{DateYearRegex}";
		public static readonly string DateExtractor6 = $@"(?<=\b(em|no|o)\s+){MonthNumRegex}[\-\.]{DayRegex}\b";
		public static readonly string DateExtractor7 = $@"\b{MonthNumRegex}\s*/\s*{DayRegex}((\s+|\s*(,|de)\s*){DateYearRegex})?\b";
		public static readonly string DateExtractor8 = $@"(?<=\b(em|no|o)\s+){DayRegex}[\\\-]{MonthNumRegex}\b";
		public static readonly string DateExtractor9 = $@"\b{DayRegex}\s*/\s*{MonthNumRegex}((\s+|\s*(,|de)\s*){DateYearRegex})?\b";
		public static readonly string DateExtractor10 = $@"\b{YearRegex}\s*[/\\\-\.]\s*{MonthNumRegex}\s*[/\\\-\.]\s*{DayRegex}";
		public static readonly string DateExtractor11 = $@"(?<=\b(dia)\s+){DayRegex}";
		public const string HourNumRegex = @"\b(?<hournum>zero|uma|duas|tr[êe]s|[qc]uatro|cinco|seis|sete|oito|nove|dez|onze|doze)\b";
		public const string MinuteNumRegex = @"(?<minnum>um|dois|tr[êe]s|[qc]uatro|cinco|seis|sete|oito|nove|dez|onze|doze|treze|catorze|quatorze|quinze|dez[ea]sseis|dez[ea]sete|dezoito|dez[ea]nove|vinte|trinta|[qc]uarenta|cin[qc]uenta)";
		public const string DeltaMinuteNumRegex = @"(?<deltaminnum>um|dois|tr[êe]s|[qc]uatro|cinco|seis|sete|oito|nove|dez|onze|doze|treze|catorze|quatorze|quinze|dez[ea]sseis|dez[ea]sete|dezoito|dez[ea]nove|vinte|trinta|[qc]uarenta|cin[qc]uenta)";
		public const string OclockRegex = @"(?<oclock>em\s+ponto)";
		public const string PmRegex = @"(?<pm>((pela|de|da|\b[àa]\b|na)\s+(tarde|noite)))|((depois\s+do|ap[óo]s\s+o)\s+(almo[çc]o|meio dia|meio-dia))";
		public const string AmRegex = @"(?<am>(pela|de|da|na)\s+(manh[ãa]|madrugada))";
		public const string AmTimeRegex = @"(?<am>([dn]?es[st]a|(pela|de|da|na))\s+(manh[ãa]|madrugada))";
		public const string PmTimeRegex = @"(?<pm>(([dn]?es[st]a|\b[àa]\b|(pela|de|da|na))\s+(tarde|noite)))|((depois\s+do|ap[óo]s\s+o)\s+(almo[çc]o|meio dia|meio-dia))";
		public static readonly string LessThanOneHour = $@"(?<lth>((\s+e\s+)?(quinze|(um\s+|dois\s+|tr[êes]\s+)?quartos?)|quinze|(\s*)(um\s+|dois\s+|tr[êes]\s+)?quartos?|(\s+e\s+)(meia|trinta)|{BaseDateTime.DeltaMinuteRegex}(\s+(minuto|minutos|min|mins))|{DeltaMinuteNumRegex}(\s+(minuto|minutos|min|mins))))";
		public const string TensTimeRegex = @"(?<tens>dez|vinte|trinta|[qc]uarenta|cin[qc]uenta)";
		public static readonly string WrittenTimeRegex = $@"(?<writtentime>({HourNumRegex}\s*((e|menos)\s+)?({MinuteNumRegex}|({TensTimeRegex}((\s*e\s+)?{MinuteNumRegex})?)))|(({MinuteNumRegex}|({TensTimeRegex}((\s*e\s+)?{MinuteNumRegex})?))\s*((para as|pras|antes da|antes das)\s+)?({HourNumRegex}|{BaseDateTime.HourRegex})))";
		public static readonly string TimePrefix = $@"(?<prefix>{LessThanOneHour}(\s+(passad[ao]s)\s+(as)?|\s+depois\s+(das?|do)|\s+pras?|\s+(para|antes)?\s+([àa]s?))?)";
		public static readonly string TimeSuffix = $@"(?<suffix>({LessThanOneHour}\s+)?({AmRegex}|{PmRegex}|{OclockRegex}))";
		public static readonly string BasicTime = $@"(?<basictime>{WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}:{BaseDateTime.MinuteRegex}(:{BaseDateTime.SecondRegex})?|{BaseDateTime.HourRegex})";
		public static readonly string AtRegex = $@"\b(?<=\b([aà]s?)\s+)({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})\b";
		public static readonly string ConnectNumRegex = $@"({BaseDateTime.HourRegex}(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\s*{DescRegex})";
		public static readonly string TimeRegex1 = $@"(\b{TimePrefix}\s+)?({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})\s*({DescRegex})";
		public static readonly string TimeRegex2 = $@"(\b{TimePrefix}\s+)?(T)?{BaseDateTime.HourRegex}(\s*)?:(\s*)?{BaseDateTime.MinuteRegex}((\s*)?:(\s*)?{BaseDateTime.SecondRegex})?((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex3 = $@"(\b{TimePrefix}\s+)?{BaseDateTime.HourRegex}\.{BaseDateTime.MinuteRegex}(\s*{DescRegex})";
		public static readonly string TimeRegex4 = $@"\b(({DescRegex}?)|({BasicTime}?)({DescRegex}?))({TimePrefix}\s*)({HourNumRegex}|{BaseDateTime.HourRegex})?(\s+{TensTimeRegex}(\s+e\s+)?{MinuteNumRegex}?)?({OclockRegex})?\b";
		public static readonly string TimeRegex5 = $@"\b({TimePrefix}|{BasicTime}{TimePrefix})\s+(\s*{DescRegex})?{BasicTime}?\s*{TimeSuffix}\b";
		public static readonly string TimeRegex6 = $@"({BasicTime}(\s*{DescRegex})?\s+{TimeSuffix}\b)";
		public static readonly string TimeRegex7 = $@"\b{TimeSuffix}\s+[àa]s?\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex8 = $@"\b{TimeSuffix}\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex9 = $@"\b(?<writtentime>{HourNumRegex}\s+({TensTimeRegex}\s*)?(e\s+)?{MinuteNumRegex}?)\b";
		public const string TimeRegex10 = @"(\b([àa]|ao?)|na|de|da|pela)\s+(madrugada|manh[ãa]|meio\s*dia|meia\s*noite|tarde|noite)";
		public static readonly string TimeRegex11 = $@"\b({WrittenTimeRegex})({DescRegex}?)\b";
		public static readonly string TimeRegex12 = $@"(\b{TimePrefix}\s+)?{BaseDateTime.HourRegex}(\s*h\s*){BaseDateTime.MinuteRegex}(\s*{DescRegex})?";
		public const string PrepositionRegex = @"(?<prep>([àa]s?|em|por|pelo|pela|no|na|de|d[oa]?)?$)";
		public const string NowRegex = @"\b(?<now>((logo|exatamente)\s+)?agora(\s+mesmo)?|neste\s+momento|(assim\s+que|t[ãa]o\s+cedo\s+quanto)\s+(poss[ií]vel|possas?|possamos)|o\s+mais\s+(cedo|r[aá]pido)\s+poss[íi]vel|recentemente|previamente)\b";
		public const string SuffixRegex = @"^\s*((e|a|em|por|pelo|pela|no|na|de)\s+)?(manh[ãa]|madrugada|meio\s*dia|tarde|noite)\b";
		public const string TimeOfDayRegex = @"\b(?<timeOfDay>manh[ãa]|madrugada|tarde|noite|((depois\s+do|ap[óo]s\s+o)\s+(almo[çc]o|meio dia|meio-dia)))\b";
		public static readonly string SpecificTimeOfDayRegex = $@"\b(((((a)?\s+|[nd]?es[st]a|seguinte|pr[oó]xim[oa]|[uú]ltim[oa])\s+)?{TimeOfDayRegex}))\b";
		public static readonly string TimeOfTodayAfterRegex = $@"^\s*(,\s*)?([àa]|em|por|pelo|pela|de|no|na?\s+)?{SpecificTimeOfDayRegex}";
		public static readonly string TimeOfTodayBeforeRegex = $@"({SpecificTimeOfDayRegex}(\s*,)?(\s+(a\s+la(s)?|para))?\s*)";
		public static readonly string SimpleTimeOfTodayAfterRegex = $@"({HourNumRegex}|{BaseDateTime.HourRegex})\s*(,\s*)?((en|de(l)?)?\s+)?{SpecificTimeOfDayRegex}";
		public static readonly string SimpleTimeOfTodayBeforeRegex = $@"({SpecificTimeOfDayRegex}(\s*,)?(\s+(a\s+la|para))?\s*({HourNumRegex}|{BaseDateTime.HourRegex}))";
		public const string TheEndOfRegex = @"((no|ao)\s+)?(fi(m|nal)|t[ée]rmin(o|ar))(\s+d?o(\s+dia)?(\s+de)?)?\s*$";
		public const string UnitRegex = @"(?<unit>anos|ano|meses|m[êe]s|semanas|semana|dias|dia|horas|hora|h|hr|hrs|hs|minutos|minuto|mins|min|segundos|segundo|segs|seg)\b";
		public const string ConnectorRegex = @"^(,|t|para [ao]|para as|pras|cerca de|cerca das|perto de|perto das|quase)$";
		public const string TimeHourNumRegex = @"(?<hour>vinte e um|vinte e dois|vinte e tr[êe]s|vinte e quatro|zero|um|uma|dois|duas|tr[êe]s|quatro|cinco|seis|sete|oito|nove|dez|onze|doze|treze|quatorze|catorze|quinze|dez[ea]sseis|dez[ea]ssete|dezoito|dez[ea]nove|vinte)";
		public static readonly string PureNumFromTo = $@"((desde|de|da|das)\s+(a(s)?\s+)?)?({BaseDateTime.HourRegex}|{TimeHourNumRegex})(\s*(?<leftDesc>{DescRegex}))?\s*{TillRegex}\s*({BaseDateTime.HourRegex}|{TimeHourNumRegex})\s*(?<rightDesc>{PmRegex}|{AmRegex}|{DescRegex})?";
		public static readonly string PureNumBetweenAnd = $@"(entre\s+((a|as)?\s+)?)({BaseDateTime.HourRegex}|{TimeHourNumRegex})(\s*(?<leftDesc>{DescRegex}))?\s*e\s*(a(s)?\s+)?({BaseDateTime.HourRegex}|{TimeHourNumRegex})\s*(?<rightDesc>{PmRegex}|{AmRegex}|{DescRegex})?";
		public const string SpecificTimeFromTo = @"^[.]";
		public const string SpecificTimeBetweenAnd = @"^[.]";
		public const string TimeUnitRegex = @"(?<unit>horas|hora|h|minutos|minuto|mins|min|segundos|segundo|secs|sec)\b";
		public static readonly string TimeFollowedUnit = $@"^\s*{TimeUnitRegex}";
		public static readonly string TimeNumberCombinedWithUnit = $@"\b(?<num>\d+(\,\d*)?)\s*{TimeUnitRegex}";
		public static readonly string DateTimePeriodNumberCombinedWithUnit = $@"\b(?<num>\d+(\.\d*)?)\s*{TimeUnitRegex}";
		public const string PeriodTimeOfDayWithDateRegex = @"\b((e|[àa]|em|na|no|ao|pel[ao]|de)\s+)?(?<timeOfDay>manh[ãa]|madrugada|(passado\s+(o\s+)?)?meio\s+dia|tarde|noite)\b";
		public static readonly string RelativeTimeUnitRegex = $@"({PastRegex}|{FutureRegex})\s+{UnitRegex}|{UnitRegex}\s+({PastRegex}|{FutureRegex})";
		public const string SuffixAndRegex = @"(?<suffix>\s*(e)\s+(?<suffix_num>meia|(um\s+)?quarto))";
		public static readonly string FollowedUnit = $@"^\s*{UnitRegex}";
		public const string LessThanRegex = @"^[.]";
		public const string MoreThanRegex = @"^[.]";
		public static readonly string DurationNumberCombinedWithUnit = $@"\b(?<num>\d+(\,\d*)?){UnitRegex}";
		public static readonly string AnUnitRegex = $@"\b(um(a)?)\s+{UnitRegex}";
		public const string DuringRegex = @"^[.]";
		public const string AllRegex = @"\b(?<all>tod[oa]?\s+(o|a)\s+(?<unit>ano|m[êe]s|semana|dia))\b";
		public const string HalfRegex = @"\b(?<half>mei[oa]\s+(?<unit>ano|m[êe]s|semana|dia|hora))\b";
		public const string ConjunctionRegex = @"^[.]";
		public const string InexactNumberRegex = @"\b(poucos|pouco|algum|alguns|v[áa]rios)\b";
		public static readonly string InexactNumberUnitRegex = $@"\b(poucos|pouco|algum|alguns|v[áa]rios)\s+{UnitRegex}";
		public static readonly string HolidayRegex1 = $@"\b(?<holiday>sexta-feira santa|sexta-feira da paix[ãa]o|quarta-feira de cinzas|carnaval|dia (de|de los) presidentes?|ano novo chin[eê]s|ano novo|v[ée]spera de ano novo|natal|v[ée]spera de natal|dia de a[cç][ãa]o de gra[çc]as|a[cç][ãa]o de gra[çc]as|yuandan|halloween|dia das bruxas|p[áa]scoa)(\s+(d[eo]?\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|[nd]?es[st][ea]|[uú]ltim[oa]?|em))\s+ano))?\b";
		public static readonly string HolidayRegex2 = $@"\b(?<holiday>(dia( d[eoa]s?)? )?(martin luther king|todos os santos|trabalho|s[ãa]o (patr[íi]cio|francisco|jorge|jo[ãa]o)|independ[êe]ncia|trabalhador|trabalho))(\s+(d[eo]?\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|[nd]?es[st][ea]|[uú]ltim[oa]?|em))\s+ano))?\b";
		public static readonly string HolidayRegex3 = $@"\b(?<holiday>(dia( d[eoa]s?)? )(trabalhador|trabalhadores|trabalho|m[ãa]es?|pais?|mulher(es)?|crian[çc]as?|marmota|professor|professores))(\s+(d[eo]?\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|[nd]?es[st][ea]|[uú]ltim[oa]?|em))\s+ano))?\b";
		public const string BeforeRegex = @"(antes(\s+(de|dos?|das?)?)?)";
		public const string AfterRegex = @"((depois|ap[óo]s)(\s*(de|d?os?|d?as?)?)?)";
		public const string SinceRegex = @"(desde(\s+(as?|o))?)";
		public const string AroundRegex = @"^[.]";
		public const string PeriodicRegex = @"\b(?<periodic>di[áa]ri[ao]|diariamente|mensalmente|semanalmente|quinzenalmente|anualmente)\b";
		public const string EachExpression = @"cada|tod[oa]s?\s*([oa]s)?";
		public static readonly string EachUnitRegex = $@"(?<each>({EachExpression})\s*{UnitRegex})";
		public static readonly string EachPrefixRegex = $@"(?<each>({EachExpression})\s*$)";
		public static readonly string EachDayRegex = $@"\s*({EachExpression})\s*dias\s*\b";
		public static readonly string BeforeEachDayRegex = $@"({EachExpression})\s*dias(\s+(as|ao))?\s*\b";
		public static readonly string SetEachRegex = $@"(?<each>({EachExpression})\s*)";
		public const string LaterEarlyPeriodRegex = @"^[.]";
		public const string WeekWithWeekDayRangeRegex = @"^[.]";
		public const string GeneralEndingRegex = @"^[.]";
		public const string MiddlePauseRegex = @"^[.]";
		public const string PrefixArticleRegex = @"^[\.]";
		public const string OrRegex = @"^[.]";
		public const string YearPlusNumberRegex = @"^[.]";
		public const string NumberAsTimeRegex = @"^[.]";
		public const string TimeBeforeAfterRegex = @"^[.]";
		public const string DateNumberConnectorRegex = @"^[.]";
		public const string ComplexDatePeriodRegex = @"^[.]";
		public const string AgoRegex = @"\b(antes|atr[áa]s|no passado)\b";
		public const string LaterRegex = @"\b(depois d[eoa]s?|ap[óo]s (as)?|desde (as|o)|desde|no futuro|mais tarde)\b";
		public const string Tomorrow = "amanh[ãa]";
		public static readonly Dictionary<string, string> UnitMap = new Dictionary<string, string>
		{
			{ "anos", "Y" },
			{ "ano", "Y" },
			{ "meses", "MON" },
			{ "mes", "MON" },
			{ "mês", "MON" },
			{ "semanas", "W" },
			{ "semana", "W" },
			{ "dias", "D" },
			{ "dia", "D" },
			{ "horas", "H" },
			{ "hora", "H" },
			{ "hrs", "H" },
			{ "hr", "H" },
			{ "h", "H" },
			{ "minutos", "M" },
			{ "minuto", "M" },
			{ "mins", "M" },
			{ "min", "M" },
			{ "segundos", "S" },
			{ "segundo", "S" },
			{ "segs", "S" },
			{ "seg", "S" }
		};
		public static readonly Dictionary<string, long> UnitValueMap = new Dictionary<string, long>
		{
			{ "anos", 31536000 },
			{ "ano", 31536000 },
			{ "meses", 2592000 },
			{ "mes", 2592000 },
			{ "mês", 2592000 },
			{ "semanas", 604800 },
			{ "semana", 604800 },
			{ "dias", 86400 },
			{ "dia", 86400 },
			{ "horas", 3600 },
			{ "hora", 3600 },
			{ "hrs", 3600 },
			{ "hr", 3600 },
			{ "h", 3600 },
			{ "minutos", 60 },
			{ "minuto", 60 },
			{ "mins", 60 },
			{ "min", 60 },
			{ "segundos", 1 },
			{ "segundo", 1 },
			{ "segs", 1 },
			{ "seg", 1 }
		};
		public static readonly Dictionary<string, string> SeasonMap = new Dictionary<string, string>
		{
			{ "primavera", "SP" },
			{ "verao", "SU" },
			{ "verão", "SU" },
			{ "outono", "FA" },
			{ "inverno", "WI" }
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
			{ "primeiro", 1 },
			{ "primeira", 1 },
			{ "1o", 1 },
			{ "1a", 1 },
			{ "segundo", 2 },
			{ "segunda", 2 },
			{ "2o", 2 },
			{ "2a", 2 },
			{ "terceiro", 3 },
			{ "terceira", 3 },
			{ "3o", 3 },
			{ "3a", 3 },
			{ "cuarto", 4 },
			{ "quarto", 4 },
			{ "cuarta", 4 },
			{ "quarta", 4 },
			{ "4o", 4 },
			{ "4a", 4 },
			{ "quinto", 5 },
			{ "quinta", 5 },
			{ "5o", 5 },
			{ "5a", 5 }
		};
		public static readonly Dictionary<string, int> DayOfWeek = new Dictionary<string, int>
		{
			{ "segunda-feira", 1 },
			{ "segundas-feiras", 1 },
			{ "segunda", 1 },
			{ "segundas", 1 },
			{ "terça-feira", 2 },
			{ "terças-feiras", 2 },
			{ "terça", 2 },
			{ "terças", 2 },
			{ "terca-feira", 2 },
			{ "tercas-feiras", 2 },
			{ "terca", 2 },
			{ "tercas", 2 },
			{ "quarta-feira", 3 },
			{ "quartas-feiras", 3 },
			{ "quarta", 3 },
			{ "quartas", 3 },
			{ "quinta-feira", 4 },
			{ "quintas-feiras", 4 },
			{ "quinta", 4 },
			{ "quintas", 4 },
			{ "sexta-feira", 5 },
			{ "sextas-feiras", 5 },
			{ "sexta", 5 },
			{ "sextas", 5 },
			{ "sabado", 6 },
			{ "sabados", 6 },
			{ "sábado", 6 },
			{ "sábados", 6 },
			{ "domingo", 0 },
			{ "domingos", 0 },
			{ "seg", 1 },
			{ "2a", 1 },
			{ "ter", 2 },
			{ "3a", 2 },
			{ "qua", 3 },
			{ "4a", 3 },
			{ "qui", 4 },
			{ "5a", 4 },
			{ "sex", 5 },
			{ "6a", 5 },
			{ "sab", 6 },
			{ "dom", 0 }
		};
		public static readonly Dictionary<string, int> MonthOfYear = new Dictionary<string, int>
		{
			{ "janeiro", 1 },
			{ "fevereiro", 2 },
			{ "março", 3 },
			{ "marco", 3 },
			{ "abril", 4 },
			{ "maio", 5 },
			{ "junho", 6 },
			{ "julho", 7 },
			{ "agosto", 8 },
			{ "septembro", 9 },
			{ "setembro", 9 },
			{ "outubro", 10 },
			{ "novembro", 11 },
			{ "dezembro", 12 },
			{ "jan", 1 },
			{ "fev", 2 },
			{ "mar", 3 },
			{ "abr", 4 },
			{ "mai", 5 },
			{ "jun", 6 },
			{ "jul", 7 },
			{ "ago", 8 },
			{ "sept", 9 },
			{ "set", 9 },
			{ "out", 10 },
			{ "nov", 11 },
			{ "dez", 12 },
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
			{ "um", 1 },
			{ "uma", 1 },
			{ "dois", 2 },
			{ "tres", 3 },
			{ "três", 3 },
			{ "quatro", 4 },
			{ "cinco", 5 },
			{ "seis", 6 },
			{ "sete", 7 },
			{ "oito", 8 },
			{ "nove", 9 },
			{ "dez", 10 },
			{ "onze", 11 },
			{ "doze", 12 },
			{ "dezena", 12 },
			{ "dezenas", 12 },
			{ "treze", 13 },
			{ "catorze", 14 },
			{ "quatorze", 14 },
			{ "quinze", 15 },
			{ "dezesseis", 16 },
			{ "dezasseis", 16 },
			{ "dezessete", 17 },
			{ "dezassete", 17 },
			{ "dezoito", 18 },
			{ "dezenove", 19 },
			{ "dezanove", 19 },
			{ "vinte", 20 },
			{ "vinte e um", 21 },
			{ "vinte e uma", 21 },
			{ "vinte e dois", 22 },
			{ "vinte e duas", 22 },
			{ "vinte e tres", 23 },
			{ "vinte e três", 23 },
			{ "vinte e quatro", 24 },
			{ "vinte e cinco", 25 },
			{ "vinte e seis", 26 },
			{ "vinte e sete", 27 },
			{ "vinte e oito", 28 },
			{ "vinte e nove", 29 },
			{ "trinta", 30 }
		};
		public static readonly Dictionary<string, IEnumerable<string>> HolidayNames = new Dictionary<string, IEnumerable<string>>
		{
			{ "pai", new string[] { "diadopai", "diadospais" } },
			{ "mae", new string[] { "diadamae", "diadasmaes" } },
			{ "acaodegracas", new string[] { "diadegracas", "diadeacaodegracas", "acaodegracas" } },
			{ "trabalho", new string[] { "diadotrabalho", "diadotrabalhador", "diadostrabalhadores" } },
			{ "pascoa", new string[] { "diadepascoa", "pascoa" } },
			{ "natal", new string[] { "natal", "diadenatal" } },
			{ "vesperadenatal", new string[] { "vesperadenatal" } },
			{ "anonovo", new string[] { "anonovo", "diadeanonovo", "diadoanonovo" } },
			{ "vesperadeanonovo", new string[] { "vesperadeanonovo", "vesperadoanonovo" } },
			{ "yuandan", new string[] { "yuandan" } },
			{ "todosossantos", new string[] { "todosossantos" } },
			{ "professor", new string[] { "diadoprofessor", "diadosprofessores" } },
			{ "crianca", new string[] { "diadacrianca", "diadascriancas" } },
			{ "mulher", new string[] { "diadamulher" } }
		};
		public static readonly Dictionary<string, string> VariableHolidaysTimexDictionary = new Dictionary<string, string>
		{
			{ "pai", "-06-WXX-7-3" },
			{ "mae", "-05-WXX-7-2" },
			{ "acaodegracas", "-11-WXX-4-4" },
			{ "trabalho", "-05-WXX-1-1" },
			{ "memoria", "-03-WXX-2-4" }
		};
		public static readonly Dictionary<string, double> DoubleNumbers = new Dictionary<string, double>
		{
			{ "metade", 0.5 },
			{ "quarto", 0.25 }
		};
		public const string DateTokenPrefix = "em ";
		public const string TimeTokenPrefix = "as ";
		public const string TokenBeforeDate = "o ";
		public const string TokenBeforeTime = "as ";
		public const string NextPrefixRegex = @"(pr[oó]xim[oa]|seguinte)\b";
		public const string PastPrefixRegex = @"([uú]ltim[oa])\b";
		public const string ThisPrefixRegex = @"([nd]?es[st][ea])\b";
		public const string RelativeDayRegex = @"^[\.]";
		public const string RestOfDateRegex = @"^[\.]";
		public const string RelativeDurationUnitRegex = @"^[\.]";
		public const string ReferenceDatePeriodRegex = @"^[.]";
		public const string FromToRegex = @"\b(from).+(to)\b.+";
		public const string SingleAmbiguousMonthRegex = @"^(the\s+)?(may|march)$";
		public const string UnspecificDatePeriodRegex = @"^[.]";
		public const string PrepositionSuffixRegex = @"\b(on|in|at|around|from|to)$";
		public const string RestOfDateTimeRegex = @"^[\.]";
		public const string SetWeekDayRegex = @"^[\.]";
		public const string NightRegex = @"\b(meia noite|noite|de noite)\b";
		public const string CommonDatePrefixRegex = @"\b(dia)\s+$";
		public const string DurationUnitRegex = @"^[\.]";
		public const string DurationConnectorRegex = @"^[.]";
		public const string CenturyRegex = @"^[.]";
		public const string DecadeRegex = @"^[.]";
		public const string DecadeWithCenturyRegex = @"^[.]";
		public const string RelativeDecadeRegex = @"^[.]";
		public static readonly string YearSuffix = $@"(,?\s*({YearRegex}|{FullTextYearRegex}))";
		public const string YearAfterRegex = @"^[.]";
		public const string YearPeriodRegex = @"^[.]";
		public const string FutureSuffixRegex = @"^[.]";
		public static readonly Dictionary<string, int> WrittenDecades = new Dictionary<string, int>
		{
			{ "", 0 }
		};
		public static readonly Dictionary<string, int> SpecialDecadeCases = new Dictionary<string, int>
		{
			{ "", 0 }
		};
		public const string DefaultLanguageFallback = "DMY";
		public static readonly string[] DurationDateRestrictions = {  };
	}
}
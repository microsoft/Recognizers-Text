﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//     
//     Generation parameters:
//     - DataFilename: Patterns\Spanish\Spanish-DateTime.yaml
//     - Language: Spanish
//     - ClassName: DateTimeDefinitions
// </auto-generated>
//------------------------------------------------------------------------------
namespace Microsoft.Recognizers.Definitions.Spanish
{
	using System;
	using System.Collections.Generic;

	public static class DateTimeDefinitions
	{
		public const string TillRegex = @"(?<till>hasta|al|a|--|-|—|——)(\s+(el|la(s)?))?";
		public const string AndRegex = @"(?<and>y|y\s*el|--|-|—|——)";
		public const string DayRegex = @"(?<day>01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|1|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)(?=\b|t)";
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
		public const string RelativeMonthRegex = @"(?<relmonth>(este|pr[oó]ximo|[uú]ltimo)\s+mes)\b";
		public const string MonthRegex = @"(?<month>Abril|Abr|Agosto|Ago|Diciembre|Dic|Febrero|Feb|Enero|Ene|Julio|Jul|Junio|Jun|Marzo|Mar|Mayo|May|Noviembre|Nov|Octubre|Oct|Septiembre|Setiembre|Sept|Set)";
		public static readonly string MonthSuffixRegex = $@"(?<msuf>(en\s+|del\s+|de\s+)?({RelativeMonthRegex}|{MonthRegex}))";
		public const string DateUnitRegex = @"(?<unit>años|año|meses|mes|semanas|semana|d[ií]a(s)?)\b";
		public const string PastRegex = @"(?<past>\b(pasad(a|o)(s)?|[uú]ltim[oa](s)?|anterior(es)?|previo(s)?)\b)";
		public const string FutureRegex = @"(?<past>\b(siguiente(s)?|pr[oó]xim[oa](s)?|dentro\s+de|en)\b)";
		public static readonly string SimpleCasesRegex = $@"\b((desde\s+el|desde|del)\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string MonthFrontSimpleCasesRegex = $@"\b{MonthSuffixRegex}\s+((desde\s+el|desde|del)\s+)?({DayRegex})\s*{TillRegex}\s*({DayRegex})((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string MonthFrontBetweenRegex = $@"\b{MonthSuffixRegex}\s+((entre|entre\s+el)\s+)({DayRegex})\s*{AndRegex}\s*({DayRegex})((\s+|\s*,\s*){YearRegex})?\b";
		public static readonly string DayBetweenRegex = $@"\b((entre|entre\s+el)\s+)({DayRegex})\s*{AndRegex}\s*({DayRegex})\s+{MonthSuffixRegex}((\s+|\s*,\s*){YearRegex})?\b";
		public const string OneWordPeriodRegex = @"\b(((pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en)\s+)?(?<month>Abril|Abr|Agosto|Ago|Diciembre|Dic|Enero|Ene|Febrero|Feb|Julio|Jul|Junio|Jun|Marzo|Mar|Mayo|May|Noviembre|Nov|Octubre|Oct|Septiembre|Setiembre|Sept|Set)|(?<=\b(del|de la|el|la)\s+)?(pr[oó]xim[oa](s)?|[uú]ltim[oa]?|est(e|a))\s+(fin de semana|semana|mes|año)|fin de semana|(mes|años)? a la fecha)\b";
		public static readonly string MonthWithYearRegex = $@"\b(((pr[oó]xim[oa](s)?|este|esta|[uú]ltim[oa]?|en)\s+)?(?<month>Abril|Abr|Agosto|Ago|Diciembre|Dic|Enero|Ene|Febrero|Feb|Julio|Jul|Junio|Jun|Marzo|Mar|Mayo|May|Noviembre|Nov|Octubre|Oct|Septiembre|Setiembre|Sept|Set)\s+((de|del|de la)\s+)?({YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|este)\s+año))\b";
		public static readonly string MonthNumWithYearRegex = $@"({YearRegex}(\s*?)[/\-\.](\s*?){MonthNumRegex})|({MonthNumRegex}(\s*?)[/\-](\s*?){YearRegex})";
		public static readonly string WeekOfMonthRegex = $@"(?<wom>(la\s+)?(?<cardinal>primera?|1ra|segunda|2da|tercera?|3ra|cuarta|4ta|quinta|5ta|[uú]ltima)\s+semana\s+{MonthSuffixRegex})";
		public static readonly string WeekOfYearRegex = $@"(?<woy>(la\s+)?(?<cardinal>primera?|1ra|segunda|2da|tercera?|3ra|cuarta|4ta|quinta|5ta|[uú]ltima?)\s+semana(\s+del?)?\s+({YearRegex}|(?<order>pr[oó]ximo|[uú]ltimo|este)\s+año))";
		public static readonly string FollowedDateUnit = $@"^\s*{DateUnitRegex}";
		public static readonly string NumberCombinedWithDateUnit = $@"\b(?<num>\d+(\.\d*)?){DateUnitRegex}";
		public static readonly string QuarterRegex = $@"(el\s+)?(?<cardinal>primer|1er|segundo|2do|tercer|3ro|cuarto|4to)\s+cuatrimestre(\s+de|\s*,\s*)?\s+({YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|este)\s+año)";
		public static readonly string QuarterRegexYearFront = $@"({YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|este)\s+año)\s+(el\s+)?(?<cardinal>(primer|primero)|1er|segundo|2do|(tercer|terceo)|3ro|cuarto|4to)\s+cuatrimestre";
		public const string AllHalfYearRegex = @"^[.]";
		public const string PrefixDayRegex = @"^[.]";
		public const string CenturySuffixRegex = @"^[.]";
		public static readonly string SeasonRegex = $@"\b(?<season>(([uú]ltim[oa]|est[ea]|el|la|(pr[oó]xim[oa]s?|siguiente))\s+)?(?<seas>primavera|verano|otoño|invierno)((\s+del?|\s*,\s*)?\s+({YearRegex}|(?<order>pr[oó]ximo|[uú]ltimo|este)\s+año))?)\b";
		public const string WhichWeekRegex = @"(semana)(\s*)(?<number>\d\d|\d|0\d)";
		public const string WeekOfRegex = @"(semana)(\s*)((do|da|de))";
		public const string MonthOfRegex = @"(mes)(\s*)((do|da|de))";
		public const string RangeUnitRegex = @"\b(?<unit>años|año|meses|mes|semanas|semana)\b";
		public const string InConnectorRegex = @"\b(in)\b";
		public const string WithinNextPrefixRegex = @"^[.]";
		public const string FromRegex = @"((desde|de)(\s*la(s)?)?)$";
		public const string ConnectorAndRegex = @"(y\s*(la(s)?)?)$";
		public const string BetweenRegex = @"(entre\s*(la(s)?)?)";
		public const string WeekDayRegex = @"\b(?<weekday>Domingos?|Lunes|Martes|Mi[eé]rcoles|Jueves|Viernes|S[aá]bados?|Lu|Ma|Mi|Ju|Vi|Sa|Do)\b";
		public static readonly string OnRegex = $@"(?<=\ben\s+)({DayRegex}s?)\b";
		public const string RelaxedOnRegex = @"(?<=\b(en|el|del)\s+)((?<day>10|11|12|13|14|15|16|17|18|19|1st|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)s?)\b";
		public static readonly string ThisRegex = $@"\b((este\s*){WeekDayRegex})|({WeekDayRegex}\s*((de\s+)?esta\s+semana))\b";
		public static readonly string LastDateRegex = $@"\b(([uú]ltimo)\s*{WeekDayRegex})|({WeekDayRegex}(\s+((de\s+)?(esta|la)\s+([uú]ltima\s+)?semana)))\b";
		public static readonly string NextDateRegex = $@"\b(((pr[oó]ximo|siguiente)\s*){WeekDayRegex})|({WeekDayRegex}(\s+(de\s+)?(la\s+)?(pr[oó]xima|siguiente)(\s*semana)))\b";
		public const string SpecialDayRegex = @"\b((el\s+)?(d[ií]a\s+antes\s+de\s+ayer|anteayer)|((el\s+)?d[ií]a\s+(despu[eé]s\s+)?de\s+mañana|pasado\s+mañana)|(el\s)?d[ií]a siguiente|(el\s)?pr[oó]ximo\s+d[ií]a|(el\s+)?[uú]ltimo d[ií]a|(d)?el d[ií]a|ayer|mañana|hoy)\b";
		public const string SpecialDayWithNumRegex = @"^[.]";
		public const string ForTheRegex = @"^[.]";
		public const string WeekDayAndDayOfMonthRegex = @"^[.]";
		public static readonly string WeekDayOfMonthRegex = $@"(?<wom>(el\s+)?(?<cardinal>primer|1er|segundo|2do|tercer|3er|cuarto|4to|quinto|5to|[uú]ltimo)\s+{WeekDayRegex}\s+{MonthSuffixRegex})";
		public const string RelativeWeekDayRegex = @"^[.]";
		public const string NumberEndingPattern = @"^[.]";
		public static readonly string SpecialDateRegex = $@"(?<=\b(en)\s+el\s+){DayRegex}\b";
		public static readonly string OfMonthRegex = $@"^\s*de\s*{MonthSuffixRegex}";
		public static readonly string MonthEndRegex = $@"({MonthRegex}\s*(el)?\s*$)";
		public static readonly string WeekDayEnd = $@"{WeekDayRegex}\s*,?\s*$";
		public static readonly string DateYearRegex = $@"(?<year>{YearRegex}|{TwoDigitYearRegex})";
		public static readonly string DateExtractor1 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}?((\s*(de)|[/\\\.\-])\s*)?{MonthRegex}\b";
		public static readonly string DateExtractor2 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}\s*([\.\-]|de)\s*{MonthRegex}(\s*,\s*|\s*(del?)\s*){DateYearRegex}\b";
		public static readonly string DateExtractor3 = $@"\b({WeekDayRegex}(\s+|\s*,\s*))?{DayRegex}(\s+|\s*,\s*|\s+de\s+|\s*-\s*){MonthRegex}((\s+|\s*,\s*){DateYearRegex})?\b";
		public static readonly string DateExtractor4 = $@"\b{MonthNumRegex}\s*[/\\\-]\s*{DayRegex}\s*[/\\\-]\s*{DateYearRegex}";
		public static readonly string DateExtractor5 = $@"\b{DayRegex}\s*[/\\\-\.]\s*{MonthNumRegex}\s*[/\\\-\.]\s*{DateYearRegex}";
		public static readonly string DateExtractor6 = $@"(?<=\b(en|el)\s+){MonthNumRegex}[\-\.]{DayRegex}\b";
		public static readonly string DateExtractor7 = $@"\b{MonthNumRegex}\s*/\s*{DayRegex}((\s+|\s*,\s*|\s+de\s+){DateYearRegex})?\b";
		public static readonly string DateExtractor8 = $@"(?<=\b(en|el)\s+){DayRegex}[\\\-]{MonthNumRegex}\b";
		public static readonly string DateExtractor9 = $@"\b{DayRegex}\s*/\s*{MonthNumRegex}((\s+|\s*,\s*|\s+de\s+){DateYearRegex})?\b";
		public static readonly string DateExtractor10 = $@"\b{YearRegex}\s*[/\\\-\.]\s*{MonthNumRegex}\s*[/\\\-\.]\s*{DayRegex}";
		public const string HourNumRegex = @"\b(?<hournum>cero|una|dos|tres|cuatro|cinco|seis|siete|ocho|nueve|diez|once|doce)\b";
		public const string MinuteNumRegex = @"(?<minnum>un|dos|tres|cuatro|cinco|seis|siete|ocho|nueve|diez|once|doce|trece|catorce|quince|dieciseis|diecisiete|dieciocho|diecinueve|veinte|treinta|cuarenta|cincuenta)";
		public const string DeltaMinuteNumRegex = @"(?<deltaminnum>un|dos|tres|cuatro|cinco|seis|siete|ocho|nueve|diez|once|doce|trece|catorce|quince|dieciseis|diecisiete|dieciocho|diecinueve|veinte|treinta|cuarenta|cincuenta)";
		public const string OclockRegex = @"(?<oclock>en\s+punto)";
		public const string PmRegex = @"(?<pm>((por|de|a|en)\s+la)\s+(tarde|noche))";
		public const string AmRegex = @"(?<am>((por|de|a|en)\s+la)\s+(mañana|madrugada))";
		public const string AmTimeRegex = @"(?<am>(esta|(por|de|a|en)\s+la)\s+(mañana|madrugada))";
		public const string PmTimeRegex = @"(?<pm>(esta|(por|de|a|en)\s+la)\s+(tarde|noche))";
		public static readonly string LessThanOneHour = $@"(?<lth>((\s+y\s+)?cuarto|(\s*)menos cuarto|(\s+y\s+)media|{BaseDateTime.DeltaMinuteRegex}(\s+(minuto|minutos|min|mins))|{DeltaMinuteNumRegex}(\s+(minuto|minutos|min|mins))))";
		public const string TensTimeRegex = @"(?<tens>diez|veint(i|e)|treinta|cuarenta|cincuenta)";
		public static readonly string WrittenTimeRegex = $@"(?<writtentime>{HourNumRegex}\s*((y|menos)\s+)?({MinuteNumRegex}|({TensTimeRegex}((\s*y\s+)?{MinuteNumRegex})?)))";
		public static readonly string TimePrefix = $@"(?<prefix>{LessThanOneHour}(\s+(pasad[ao]s)\s+(de\s+las|las)?|\s+(para|antes\s+de)?\s+(las?))?)";
		public static readonly string TimeSuffix = $@"(?<suffix>({LessThanOneHour}\s+)?({AmRegex}|{PmRegex}|{OclockRegex}))";
		public static readonly string BasicTime = $@"(?<basictime>{WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}:{BaseDateTime.MinuteRegex}(:{BaseDateTime.SecondRegex})?|{BaseDateTime.HourRegex})";
		public static readonly string AtRegex = $@"\b(?<=\b(a las?)\s+)({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})\b";
		public static readonly string ConnectNumRegex = $@"({BaseDateTime.HourRegex}(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\s*{DescRegex})";
		public static readonly string TimeRegex1 = $@"(\b{TimePrefix}\s+)?({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})\s*({DescRegex})";
		public static readonly string TimeRegex2 = $@"(\b{TimePrefix}\s+)?(T)?{BaseDateTime.HourRegex}(\s*)?:(\s*)?{BaseDateTime.MinuteRegex}((\s*)?:(\s*)?{BaseDateTime.SecondRegex})?((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex3 = $@"(\b{TimePrefix}\s+)?{BaseDateTime.HourRegex}\.{BaseDateTime.MinuteRegex}(\s*{DescRegex})";
		public static readonly string TimeRegex4 = $@"\b(({DescRegex}?)|({BasicTime}?)({DescRegex}?))({TimePrefix}\s*)({HourNumRegex}|{BaseDateTime.HourRegex})?(\s+{TensTimeRegex}(\s+y\s+)?{MinuteNumRegex}?)?({OclockRegex})?\b";
		public static readonly string TimeRegex5 = $@"\b({TimePrefix}|{BasicTime}{TimePrefix})\s+(\s*{DescRegex})?{BasicTime}?\s*{TimeSuffix}\b";
		public static readonly string TimeRegex6 = $@"({BasicTime}(\s*{DescRegex})?\s+{TimeSuffix}\b)";
		public static readonly string TimeRegex7 = $@"\b{TimeSuffix}\s+a\s+las\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex8 = $@"\b{TimeSuffix}\s+{BasicTime}((\s*{DescRegex})|\b)";
		public static readonly string TimeRegex9 = $@"\b(?<writtentime>{HourNumRegex}\s+({TensTimeRegex}\s*)?(y\s+)?{MinuteNumRegex}?)\b";
		public const string TimeRegex10 = @"(a\s+la|al)\s+(madrugada|mañana|medio\s*d[ií]a|tarde|noche)";
		public static readonly string TimeRegex11 = $@"\b({WrittenTimeRegex})({DescRegex}?)\b";
		public static readonly string TimeRegex12 = $@"(\b{TimePrefix}\s+)?{BaseDateTime.HourRegex}(\s*h\s*){BaseDateTime.MinuteRegex}(\s*{DescRegex})?";
		public const string PrepositionRegex = @"(?<prep>(a(l)?|en|de(l)?)?(\s*(la(s)?|el|los))?$)";
		public const string NowRegex = @"\b(?<now>(justo\s+)?ahora(\s+mismo)?|en\s+este\s+momento|tan\s+pronto\s+como\s+sea\s+posible|tan\s+pronto\s+como\s+(pueda|puedas|podamos|puedan)|lo\s+m[aá]s\s+pronto\s+posible|recientemente|previamente)\b";
		public const string SuffixRegex = @"^\s*(((y|a|en|por)\s+la|al)\s+)?(mañana|madrugada|medio\s*d[ií]a|tarde|noche)\b";
		public const string TimeOfDayRegex = @"\b(?<timeOfDay>mañana|madrugada|(pasado\s+(el\s+)?)?medio\s?d[ií]a|tarde|noche|anoche)\b";
		public static readonly string SpecificTimeOfDayRegex = $@"\b(((((a)?\s+la|esta|siguiente|pr[oó]xim[oa]|[uú]ltim[oa])\s+)?{TimeOfDayRegex}))\b";
		public static readonly string TimeOfTodayAfterRegex = $@"^\s*(,\s*)?(en|de(l)?\s+)?{SpecificTimeOfDayRegex}";
		public static readonly string TimeOfTodayBeforeRegex = $@"({SpecificTimeOfDayRegex}(\s*,)?(\s+(a\s+la(s)?|para))?\s*)";
		public static readonly string SimpleTimeOfTodayAfterRegex = $@"({HourNumRegex}|{BaseDateTime.HourRegex})\s*(,\s*)?((en|de(l)?)?\s+)?{SpecificTimeOfDayRegex}";
		public static readonly string SimpleTimeOfTodayBeforeRegex = $@"({SpecificTimeOfDayRegex}(\s*,)?(\s+(a\s+la|para))?\s*({HourNumRegex}|{BaseDateTime.HourRegex}))";
		public const string TheEndOfRegex = @"((a|e)l\s+)?fin(alizar|al)?(\s+(el|de(l)?)(\s+d[ií]a)?(\s+de)?)?\s*$";
		public const string UnitRegex = @"(?<unit>años|año|meses|mes|semanas|semana|d[ií]as|d[ií]a|horas|hora|h|hr|hrs|hs|minutos|minuto|mins|min|segundos|segundo|segs|seg)\b";
		public const string ConnectorRegex = @"^(,|t|para la|para las|cerca de la|cerca de las)$";
		public const string TimeHourNumRegex = @"(?<hour>veintiuno|veintidos|veintitres|veinticuatro|cero|uno|dos|tres|cuatro|cinco|seis|siete|ocho|nueve|diez|once|doce|trece|catorce|quince|diecis([eé])is|diecisiete|dieciocho|diecinueve|veinte)";
		public static readonly string PureNumFromTo = $@"((desde|de)\s+(la(s)?\s+)?)?({BaseDateTime.HourRegex}|{TimeHourNumRegex})(\s*(?<leftDesc>{DescRegex}))?\s*{TillRegex}\s*({BaseDateTime.HourRegex}|{TimeHourNumRegex})\s*(?<rightDesc>{PmRegex}|{AmRegex}|{DescRegex})?";
		public static readonly string PureNumBetweenAnd = $@"(entre\s+(la(s)?\s+)?)({BaseDateTime.HourRegex}|{TimeHourNumRegex})(\s*(?<leftDesc>{DescRegex}))?\s*y\s*(la(s)?\s+)?({BaseDateTime.HourRegex}|{TimeHourNumRegex})\s*(?<rightDesc>{PmRegex}|{AmRegex}|{DescRegex})?";
		public const string SpecificTimeFromTo = @"^[.]";
		public const string SpecificTimeBetweenAnd = @"^[.]";
		public const string TimeUnitRegex = @"(?<unit>horas|hora|h|minutos|minuto|mins|min|segundos|segundo|secs|sec)\b";
		public static readonly string TimeFollowedUnit = $@"^\s*{TimeUnitRegex}";
		public static readonly string TimeNumberCombinedWithUnit = $@"\b(?<num>\d+(\,\d*)?)\s*{TimeUnitRegex}";
		public static readonly string DateTimePeriodNumberCombinedWithUnit = $@"\b(?<num>\d+(\.\d*)?)\s*{TimeUnitRegex}";
		public const string PeriodTimeOfDayWithDateRegex = @"\b(((y|a|en|por)\s+la|al)\s+)?(?<timeOfDay>mañana|madrugada|(pasado\s+(el\s+)?)?medio\s?d[ií]a|tarde|noche|anoche)\b";
		public static readonly string RelativeTimeUnitRegex = $@"({PastRegex}|{FutureRegex})\s+{UnitRegex}";
		public const string LessThanRegex = @"^[.]";
		public const string MoreThanRegex = @"^[.]";
		public const string SuffixAndRegex = @"(?<suffix>\s*(y)\s+((un|uno|una)\s+)?(?<suffix_num>media|cuarto))";
		public static readonly string FollowedUnit = $@"^\s*{UnitRegex}";
		public static readonly string DurationNumberCombinedWithUnit = $@"\b(?<num>\d+(\,\d*)?){UnitRegex}";
		public static readonly string AnUnitRegex = $@"\b(un(a)?)\s+{UnitRegex}";
		public const string DuringRegex = @"^[.]";
		public const string AllRegex = @"\b(?<all>tod[oa]?\s+(el|la)\s+(?<unit>año|mes|semana|d[ií]a))\b";
		public const string HalfRegex = @"\b(?<half>medi[oa]\s+(?<unit>ano|mes|semana|d[íi]a|hora))\b";
		public const string ConjunctionRegex = @"^[.]";
		public const string InexactNumberRegex = @"\b(pocos|poco|algo|varios)\b";
		public static readonly string InexactNumberUnitRegex = $@"\b(pocos|poco|algo|varios)\s+{UnitRegex}";
		public static readonly string HolidayRegex1 = $@"\b(?<holiday>viernes santo|mi[eé]rcoles de ceniza|martes de carnaval|d[ií]a (de|de los) presidentes?|clebraci[oó]n de mao|año nuevo chino|año nuevo|noche vieja|(festividad de )?los mayos|d[ií]a de los inocentes|navidad|noche buena|d[ií]a de acci[oó]n de gracias|acci[oó]n de gracias|yuandan|halloween|noches de brujas|pascuas)(\s+(del?\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b";
		public static readonly string HolidayRegex2 = $@"\b(?<holiday>(d[ií]a( del?( la)?)? )?(martin luther king|todos los santos|blanco|san patricio|san valent[ií]n|san jorge|cinco de mayo|independencia|raza|trabajador))(\s+(del?\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b";
		public static readonly string HolidayRegex3 = $@"\b(?<holiday>(d[ií]a( del?( las?)?)? )(trabajador|madres?|padres?|[aá]rbol|mujer(es)?|solteros?|niños?|marmota|san valent[ií]n|maestro))(\s+(del?\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|est[ea]|[uú]ltim[oa]?|en))\s+año))?\b";
		public const string BeforeRegex = @"(antes(\s+del?(\s+las?)?)?)";
		public const string AfterRegex = @"(despues(\s*del?(\s+las?)?)?)";
		public const string SinceRegex = @"(desde(\s+(las?|el))?)";
		public const string AroundRegex = @"^[.]";
		public const string PeriodicRegex = @"\b(?<periodic>a\s*diario|diariamente|mensualmente|semanalmente|quincenalmente|anualmente)\b";
		public const string EachExpression = @"cada|tod[oa]s\s*(l[oa]s)?";
		public static readonly string EachUnitRegex = $@"(?<each>({EachExpression})\s*{UnitRegex})";
		public static readonly string EachPrefixRegex = $@"(?<each>({EachExpression})\s*$)";
		public static readonly string EachDayRegex = $@"\s*({EachExpression})\s*d[ií]as\s*\b";
		public static readonly string BeforeEachDayRegex = $@"({EachExpression})\s*d[ií]as(\s+a\s+las?)?\s*\b";
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
		public const string CenturyRegex = @"^[.]";
		public const string DecadeRegex = @"^[.]";
		public const string DecadeWithCenturyRegex = @"^[.]";
		public const string RelativeDecadeRegex = @"^[.]";
		public const string ComplexDatePeriodRegex = @"^[.]";
		public static readonly string YearSuffix = $@"(,?\s*({YearRegex}|{FullTextYearRegex}))";
		public const string AgoRegex = @"\b(antes)\b";
		public const string LaterRegex = @"\b(despu[eé]s|desde ahora)\b";
		public const string Tomorrow = "mañana";
		public static readonly Dictionary<string, string> UnitMap = new Dictionary<string, string>
		{
			{ "años", "Y" },
			{ "año", "Y" },
			{ "meses", "MON" },
			{ "mes", "MON" },
			{ "semanas", "W" },
			{ "semana", "W" },
			{ "dias", "D" },
			{ "dia", "D" },
			{ "días", "D" },
			{ "día", "D" },
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
			{ "años", 31536000 },
			{ "año", 31536000 },
			{ "meses", 2592000 },
			{ "mes", 2592000 },
			{ "semanas", 604800 },
			{ "semana", 604800 },
			{ "dias", 86400 },
			{ "dia", 86400 },
			{ "días", 86400 },
			{ "día", 86400 },
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
			{ "verano", "SU" },
			{ "otoño", "FA" },
			{ "invierno", "WI" }
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
			{ "primer", 1 },
			{ "primero", 1 },
			{ "primera", 1 },
			{ "1er", 1 },
			{ "1ro", 1 },
			{ "1ra", 1 },
			{ "segundo", 2 },
			{ "segunda", 2 },
			{ "2do", 2 },
			{ "2da", 2 },
			{ "tercer", 3 },
			{ "tercero", 3 },
			{ "tercera", 3 },
			{ "3er", 3 },
			{ "3ro", 3 },
			{ "3ra", 3 },
			{ "cuarto", 4 },
			{ "cuarta", 4 },
			{ "4to", 4 },
			{ "4ta", 4 },
			{ "quinto", 5 },
			{ "quinta", 5 },
			{ "5to", 5 },
			{ "5ta", 5 }
		};
		public static readonly Dictionary<string, int> DayOfWeek = new Dictionary<string, int>
		{
			{ "lunes", 1 },
			{ "martes", 2 },
			{ "miercoles", 3 },
			{ "miércoles", 3 },
			{ "jueves", 4 },
			{ "viernes", 5 },
			{ "sabado", 6 },
			{ "domingo", 0 },
			{ "lu", 1 },
			{ "ma", 2 },
			{ "mi", 3 },
			{ "ju", 4 },
			{ "vi", 5 },
			{ "sa", 6 },
			{ "do", 0 }
		};
		public static readonly Dictionary<string, int> MonthOfYear = new Dictionary<string, int>
		{
			{ "enero", 1 },
			{ "febrero", 2 },
			{ "marzo", 3 },
			{ "abril", 4 },
			{ "mayo", 5 },
			{ "junio", 6 },
			{ "julio", 7 },
			{ "agosto", 8 },
			{ "septiembre", 9 },
			{ "setiembre", 9 },
			{ "octubre", 10 },
			{ "noviembre", 11 },
			{ "diciembre", 12 },
			{ "ene", 1 },
			{ "feb", 2 },
			{ "mar", 3 },
			{ "abr", 4 },
			{ "may", 5 },
			{ "jun", 6 },
			{ "jul", 7 },
			{ "ago", 8 },
			{ "sept", 9 },
			{ "set", 9 },
			{ "oct", 10 },
			{ "nov", 11 },
			{ "dic", 12 },
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
			{ "cero", 0 },
			{ "un", 1 },
			{ "una", 1 },
			{ "uno", 1 },
			{ "dos", 2 },
			{ "tres", 3 },
			{ "cuatro", 4 },
			{ "cinco", 5 },
			{ "seis", 6 },
			{ "siete", 7 },
			{ "ocho", 8 },
			{ "nueve", 9 },
			{ "diez", 10 },
			{ "once", 11 },
			{ "doce", 12 },
			{ "docena", 12 },
			{ "docenas", 12 },
			{ "trece", 13 },
			{ "catorce", 14 },
			{ "quince", 15 },
			{ "dieciseis", 16 },
			{ "dieciséis", 16 },
			{ "diecisiete", 17 },
			{ "dieciocho", 18 },
			{ "diecinueve", 19 },
			{ "veinte", 20 },
			{ "ventiuna", 21 },
			{ "ventiuno", 21 },
			{ "veintiun", 21 },
			{ "veintiún", 21 },
			{ "veintiuno", 21 },
			{ "veintiuna", 21 },
			{ "veintidos", 22 },
			{ "veintidós", 22 },
			{ "veintitres", 23 },
			{ "veintitrés", 23 },
			{ "veinticuatro", 24 },
			{ "veinticinco", 25 },
			{ "veintiseis", 26 },
			{ "veintiséis", 26 },
			{ "veintisiete", 27 },
			{ "veintiocho", 28 },
			{ "veintinueve", 29 },
			{ "treinta", 30 }
		};
		public static readonly Dictionary<string, IEnumerable<string>> HolidayNames = new Dictionary<string, IEnumerable<string>>
		{
			{ "padres", new string[] { "diadelpadre" } },
			{ "madres", new string[] { "diadelamadre" } },
			{ "acciondegracias", new string[] { "diadegracias", "diadeacciondegracias", "acciondegracias" } },
			{ "trabajador", new string[] { "diadeltrabajador" } },
			{ "delaraza", new string[] { "diadelaraza", "diadeladiversidadcultural" } },
			{ "memoria", new string[] { "diadelamemoria" } },
			{ "pascuas", new string[] { "diadepascuas", "pascuas" } },
			{ "navidad", new string[] { "navidad", "diadenavidad" } },
			{ "nochebuena", new string[] { "diadenochebuena", "nochebuena" } },
			{ "añonuevo", new string[] { "añonuevo", "diadeañonuevo" } },
			{ "nochevieja", new string[] { "nochevieja", "diadenochevieja" } },
			{ "yuandan", new string[] { "yuandan" } },
			{ "maestro", new string[] { "diadelmaestro" } },
			{ "todoslossantos", new string[] { "todoslossantos" } },
			{ "niño", new string[] { "diadelniño" } },
			{ "mujer", new string[] { "diadelamujer" } }
		};
		public static readonly Dictionary<string, string> VariableHolidaysTimexDictionary = new Dictionary<string, string>
		{
			{ "padres", "-06-WXX-7-3" },
			{ "madres", "-05-WXX-7-2" },
			{ "acciondegracias", "-11-WXX-4-4" },
			{ "trabajador", "-05-WXX-1-1" },
			{ "delaraza", "-10-WXX-1-2" },
			{ "memoria", "-03-WXX-2-4" }
		};
		public static readonly Dictionary<string, double> DoubleNumbers = new Dictionary<string, double>
		{
			{ "mitad", 0.5 },
			{ "cuarto", 0.25 }
		};
		public const string DateTokenPrefix = "en ";
		public const string TimeTokenPrefix = "a las ";
		public const string TokenBeforeDate = "el ";
		public const string TokenBeforeTime = "la ";
		public const string NextPrefixRegex = @"(pr[oó]xim[oa]|siguiente)\b";
		public const string PastPrefixRegex = @"([uú]ltim[oa])\b";
		public const string ThisPrefixRegex = @"(est[ea])\b";
		public const string RelativeDayRegex = @"^[\.]";
		public const string RestOfDateRegex = @"^[\.]";
		public const string RelativeDurationUnitRegex = @"^[\.]";
		public const string ReferenceDatePeriodRegex = @"^[.]";
		public const string FromToRegex = @"\b(from).+(to)\b.+";
		public const string SingleAmbiguousMonthRegex = @"^(the\s+)?(may|march)$";
		public const string UnspecificDatePeriodRegex = @"^[.]";
		public const string PrepositionSuffixRegex = @"\b(on|in|at|around|for|during|since|from|to)$";
		public const string RestOfDateTimeRegex = @"^[\.]";
		public const string SetWeekDayRegex = @"^[\.]";
		public const string NightRegex = @"\b(medionoche|noche)\b";
		public const string CommonDatePrefixRegex = @"^[\.]";
		public const string DurationUnitRegex = @"^[\.]";
		public const string DurationConnectorRegex = @"^[.]";
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
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

import { BaseDateTime } from "./baseDateTime";
export namespace FrenchDateTime {
	export const TillRegex = `(?<till>au|[aà]|et|jusqu'[aà]|avant|--|-|—|——)`;
	export const RangeConnectorRegex = `(?<and>et|de la|au|[aà]|et\\s*la|--|-|—|——)`;
	export const RelativeRegex = `(?<order>prochain|prochaine|de|du|ce|cette|l[ae]|derni[eè]re|pr[eé]c[eé]dente|au\\s+cours+(de|du\\s*))`;
	export const NextSuffixRegex = `(?<order>prochain|prochaine|prochaines|suivante)\\b`;
	export const PastSuffixRegex = `(?<order>dernier|derni[eè]re|pr[eé]c[eé]dente)\\b`;
	export const ThisPrefixRegex = `(?<order>ce|cette|au\\s+cours+(du|de))\\b`;
	export const DayRegex = `(?<day>01|02|03|04|05|06|07|08|09|10|11|11e|12|12e|13|13e|14|14e|15|15e|16|16e|17|17e|18|18e|19|19e|1er|1|21|21e|20|20e|22|22e|23|23e|24|24e|25|25e|26|26e|27|27e|28|28e|29|29e|2|2e|30|30e|31|31e|3|3e|4|4e|5|5e|6|6e|7|7e|8|8e|9|9e)(?=\\b|t)`;
	export const MonthNumRegex = `(?<month>01|02|03|04|05|06|07|08|09|10|11|12|1|2|3|4|5|6|7|8|9)\\b`;
	export const PeriodYearRegex = `\\b(?<year>19\\d{2}|20\\d{2})\\b`;
	export const WeekDayRegex = `(?<weekday>Dimanche|Lundi|Mardi|Mecredi|Jeudi|Vendredi|Samedi|Lun|Mar|Mer|Jeu|Ven|Sam|Dim)\\b`;
	export const RelativeMonthRegex = `(?<relmonth>(${ThisPrefixRegex}\\s+mois)|(mois\\s+${PastSuffixRegex})|(mois\\s+${NextSuffixRegex}))\\b`;
	export const EngMonthRegex = `(?<month>Avril|Avr\\.|Avr|Août|D[eé]cembre|D[eé]c|D[eé]c\\.|F[eé]vrier|F[eé]v|F[eé]vr\\.|F[eé]vr|Javier|Jan|Janv\\.|Janv|Juillet|Jul|Juil|Juil\\.|Juin|Jun|Mars|Mar|Mai|Novembre|Nov|Nov\\.|Octobre|Oct|Oct\\.|Septembre|Sep|Sept|Sept\\.)`;
	export const MonthSuffixRegex = `(?<msuf>(en\\s*|le\\s*|de\\s*|dans\\s*)?(${RelativeMonthRegex}|${EngMonthRegex}))`;
	export const DateUnitRegex = `(?<unit>l'ann[eé]e|ann[eé]es|an|mois|semaines|semaine|jours|jour|journ[eé]e|journ[eé]es)\\b`;
	export const SimpleCasesRegex = `\\b((d[ue])|entre\\s+)?(${DayRegex})\\s*${TillRegex}\\s*(${DayRegex})\\s+${MonthSuffixRegex}((\\s+|\\s*,\\s*)${PeriodYearRegex})?\\b`;
	export const MonthFrontSimpleCasesRegex = `\\b((d[ue]|entre)\\s+)?${MonthSuffixRegex}\\s+((d[ue]|entre)\\s+)?(${DayRegex})\\s*${TillRegex}\\s*(${DayRegex})((\\s+|\\s*,\\s*)${PeriodYearRegex})?\\b`;
	export const MonthFrontBetweenRegex = `\\b${MonthSuffixRegex}\\s+(entre|d[ue]\\s+)(${DayRegex})\\s*${RangeConnectorRegex}\\s*(${DayRegex})((\\s+|\\s*,\\s*)${PeriodYearRegex})?\\b`;
	export const BetweenRegex = `\\b(entre\\s+)(${DayRegex})\\s*${RangeConnectorRegex}\\s*(${DayRegex})\\s+${MonthSuffixRegex}((\\s+|\\s*,\\s*)${PeriodYearRegex})?\\b`;
	export const YearWordRegex = `\\b(?<year>l'ann[ée]e)\\b`;
	export const MonthWithYear = `\\b((?<month>Avril|Avr\\.|Avr|Août|Aout|Decembre|D[eé]c|Dec\\.|F[eé]v|F[eé]vr|Fev|F[eé]vrier|F[eé]v\\.|Janvier|Jan|Janv|Janv\\.|Jan\\.|Jul|Juillet|Juil\\.|Jun|Juin|Mar|Mars|Mai|Novembre|Nov|Nov\\.|Octobre|Oct|Oct\\.|Septembre|Sep|Sept|Sept\\.),?(\\s+de)?\\s+(${PeriodYearRegex}|(?<order>cette)\\s*${YearWordRegex})|${YearWordRegex}\\s*(${PastSuffixRegex}|${NextSuffixRegex}))`;
	export const OneWordPeriodRegex = `\\b((${RelativeRegex}\\s+)?(?<month>Avril|Avr\\.|Avr|Août|Aout|D[eé]cembre|Dec|D[eé]c\\.|F[eé]vrier|Fev|F[eé]v\\.|F[eé]vr|Janvier|Janv\\.|Janv|Jan|Jan\\.|Jul|Juillet|Juil\\.|Jun|Juin|Mar|Mars|Mai|Nov|Novembre|Nov\\.|Oct|Octobre|Oct\\.|Sep|Septembre|Sept\\.)|${RelativeRegex}\\s+(weekend|fin de semaine|week-end|semaine|mois|ans|l'année)|weekend|week-end|(mois|l'année))\\b`;
	export const MonthNumWithYear = `(${PeriodYearRegex}[/\\-\\.]${MonthNumRegex})|(${MonthNumRegex}[/\\-]${PeriodYearRegex})`;
	export const WeekOfMonthRegex = `(?<wom>(le\\s+)?(?<cardinal>premier|1er|duexi[èe]me|2|troisi[èe]me|3|quatri[èe]me|4|cinqi[èe]me|5)\\s+semaine\\s+${MonthSuffixRegex})`;
	export const WeekOfYearRegex = `(?<woy>(le\\s+)?(?<cardinal>premier|1er|duexi[èe]me|2|troisi[èe]me|3|quatri[èe]me|4|cinqi[èe]me|5)\\s+semaine(\\s+de)?\\s+(${PeriodYearRegex}|${RelativeRegex}\\s+ann[ée]e))`;
	export const FollowedDateUnit = `^\\s*${DateUnitRegex}`;
	export const NumberCombinedWithDateUnit = `\\b(?<num>\\d+(\\.\\d*)?)${DateUnitRegex}`;
	export const QuarterRegex = `(le\\s+)?(?<cardinal>premier|1er|duexi[èe]me|2|troisi[èe]me|3|quatri[èe]me|4)\\s+quart(\\s+de|\\s*,\\s*)?\\s+(${PeriodYearRegex}|${RelativeRegex}\\s+l'ann[eé]e)`;
	export const QuarterRegexYearFront = `(${PeriodYearRegex}|l'année\\s+({PastSuffixRegex}|{NextSuffixRegex})|${RelativeRegex}\\s+ann[eé]e)\\s+(le\\s+)?(?<cardinal>premier|1er|duexi[èe]me|2|troisi[èe]me|3|quatri[èe]me|4)\\s+quarts`;
	export const SeasonRegex = `\\b((<seas>printemps|été|automne|hiver)+\\s*(${NextSuffixRegex}|${PastSuffixRegex}))|(?<season>(${RelativeRegex}\\s+)?(?<seas>printemps|[ée]t[ée]|automne|hiver)((\\s+de|\\s*,\\s*)?\\s+(${PeriodYearRegex}|${RelativeRegex}\\s+l'ann[eé]e))?)\\b`;
	export const WhichWeekRegex = `(semaine)(\\s*)(?<number>\\d\\d|\\d|0\\d)`;
	export const WeekOfRegex = `(semaine)(\\s*)(de)`;
	export const MonthOfRegex = `(mois)(\\s*)(de)`;
	export const MonthRegex = `(?<month>Avril|Avr|Avr\\.|Août|Aout|Decembre|D[eé]c|Dec\\.|F[eé]vrier|F[eé]vr|Fev|F[eé]v|F[eé]v\\.|Janvier|Janv\\.|Janv|Jan|Jan\\.|Juillet|Juil|Juil\\.|Juin|Mars|Mai|Novembre|Nov|Nov\\.|Octobre|Oct|Oct\\.|Septembre|Sep|Sept|Sept\\.)`;
	export const DateYearRegex = `(?<year>19\\d{2}|20\\d{2}|((9\\d|0\\d|1\\d|2\\d)(?!\\s*\\:)))`;
	export const OnRegex = `(?<=\\b(en|sur\\s*l[ea]|sur)\\s+)(${DayRegex}s?)\\b`;
	export const RelaxedOnRegex = `(?<=\\b(en|le|dans|sur\\s*l[ea]|du|sur)\\s+)((?<day>10e|11e|12e|13e|14e|15e|16e|17e|18e|19e|1er|20e|21e|22e|23e|24e|25e|26e|27e|28e|29e|2e|30e|31e|3e|4e|5e|6e|7e|8e|9e)s?)\\b`;
	export const ThisRegex = `\\b((cette(\\s*semaine)?\\s+)${WeekDayRegex})|(${WeekDayRegex}(\\s+cette\\s*semaine))\\b`;
	export const LastDateRegex = `\\b((${WeekDayRegex}(\\s*(de)?\\s*la\\s*semaine\\s+${PastSuffixRegex}))|(${WeekDayRegex}(\\s+${PastSuffixRegex})))\\b`;
	export const NextDateRegex = `\\b((${WeekDayRegex}(\\s+${NextSuffixRegex}))|(${WeekDayRegex}(\\s*(de)?\\s*la\\s*semaine\\s+${NextSuffixRegex})))\\b`;
	export const SpecialDayRegex = `\\b(avant[\\s|-]hier|apr[eè]s(-demain|\\s*demain)|(le\\s)?jour suivant|(le\\s+)?dernier jour|hier|lendemain|demain|de la journ[ée]e|aujourd'hui)\\b`;
	export const StrictWeekDay = `\\b(?<weekday>Dimanche|Lundi|Mardi|Mecredi|Jeudi|Vendredi|Samedi|Lun|Mar|Mer|Jeu|Ven|Sam|Dim)s?\\b`;
	export const SetWeekDayRegex = `\\b(?<prefix>le\\s+)?(?<weekday>matin|matin[ée]e|apres-midi|soir|soir[ée]e|Dimanche|Lundi|Mardi|Mercredi|Jeudi|Vendredi|Samedi)s\\b`;
	export const WeekDayOfMonthRegex = `(?<wom>(le\\s+)?(?<cardinal>premier|1er|duexi[èe]me|2|troisi[èe]me|3|quatri[èe]me|4|cinqi[èe]me|5)\\s+${WeekDayRegex}\\s+${MonthSuffixRegex})`;
	export const SpecialDate = `(?<=\\b([àa]|au|le)\\s+)${DayRegex}\\b`;
	export const DateExtractor1 = `\\b(${WeekDayRegex}(\\s+|\\s*,\\s*))?${MonthRegex}\\s*[/\\\\\\.\\-]?\\s*${DayRegex}\\b`;
	export const DateExtractor2 = `\\b(${WeekDayRegex}(\\s+|\\s*,\\s*))?${DayRegex}(\\s+|\\s*,\\s*|\\s+)${MonthRegex}\\s*[\\.\\-]?\\s*${DateYearRegex}\\b`;
	export const DateExtractor3 = `\\b(${WeekDayRegex}(\\s+|\\s*,\\s*))?${DayRegex}(\\s+|\\s*,\\s*|\\s*-\\s*)${MonthRegex}((\\s+|\\s*,\\s*)${DateYearRegex})?\\b`;
	export const DateExtractor4 = `\\b${MonthNumRegex}\\s*[/\\\\\\-]\\s*${DayRegex}\\s*[/\\\\\\-]\\s*${DateYearRegex}`;
	export const DateExtractor5 = `\\b${DayRegex}\\s*[/\\\\\\-]\\s*${MonthNumRegex}\\s*[/\\\\\\-]\\s*${DateYearRegex}`;
	export const DateExtractor6 = `(?<=\\b(le|sur|sur l[ae])\\s+)${MonthNumRegex}[\\-\\.\\/]${DayRegex}\\b`;
	export const DateExtractor7 = `\\b${DayRegex}\\s*/\\s*${MonthNumRegex}((\\s+|\\s*,\\s*)${DateYearRegex})?\\b`;
	export const DateExtractor8 = `(?<=\\b(le)\\s+)${DayRegex}[\\\\\\-]${MonthNumRegex}\\b`;
	export const DateExtractor9 = `\\b${DayRegex}\\s*/\\s*${MonthNumRegex}((\\s+|\\s*,\\s*)${DateYearRegex})?\\b`;
	export const DateExtractorA = `\\b${DateYearRegex}\\s*[/\\\\\\-]\\s*${MonthNumRegex}\\s*[/\\\\\\-]\\s*${DayRegex}`;
	export const OfMonth = `^\\s*de\\s*${MonthRegex}`;
	export const MonthEnd = `${MonthRegex}\\s*(le)?\\s*$`;
	export const RangeUnitRegex = `\\b(?<unit>l'année|ann[eé]e(s)?|mois|semaines|semaine)\\b`;
	export const DescRegex = `(?<desc>h|ampm|am\\b|a\\.m\\.|a m\\b|a\\. m\\.|a\\.m\\b|a\\. m\\b|pm\\b|p\\.m\\.|p m\\b|p\\. m\\.|p\\.m\\b|p\\. m\\b|p\\b\\b)`;
	export const HourNumRegex = `\\b(?<hournum>zero|un|deux|trois|quatre|cinq|six|sept|huit|neuf|dix|onze|douze|treize|quatorze|quinze|dix-six|dix-sept|dix-huit|dix-neuf|vingt|vingt-et-un|vingt-deux|vingt-trois)\\b`;
	export const MinuteNumRegex = `(?<minnum>un|deux|trois|quatre|cinq|six|sept|huit|neuf|dix|onze|douze|treize|quatorze|quinze|seize|dix-sept|dix-huit|dix-neuf|vingt|trente|quarante|cinquante)`;
	export const DeltaMinuteNumRegex = `(?<deltaminnum>un|deux|trois|quatre|cinq|six|sept|huit|neuf|dix|onze|douze|treize|quatorze|quinze|seize|dix-sept|dix-huit|dix-neuf|vingt|trente|quarante|cinquante)`;
	export const OclockRegex = `(?<oclock>heure|heures|h)`;
	export const PmRegex = `(?<pm>(dans l'\\s*)?apr[eè]s(\\s*|-)midi|(du|ce|de|le)\\s*(soir[ée]e|soir)|(dans l[ea]\\s+)?(nuit|soir[eé]e))`;
	export const AmRegex = `(?<am>(du|de|ce|dans l[ea]|le)?\\s*matin|(du|de|ce|(du|de|dans)\\s*l[ea]|le)?\\s*matin[ée]e)`;
	export const LessThanOneHour = `(?<lth>(une\\s+)?quart|trois quart(s)?|demie( heure)?|${BaseDateTime.DeltaMinuteRegex}(\\s+(minute|minutes|min|mins))|${DeltaMinuteNumRegex}(\\s+(minute|minutes|min|mins)))`;
	export const EngTimeRegex = `(?<engtime>${HourNumRegex}\\s+(${MinuteNumRegex}|(?<tens>vingt|trente|quarante|cinquante)\\s+${MinuteNumRegex}))`;
	export const TimePrefix = `(?<prefix>(heures\\s*et\\s+${LessThanOneHour}|et ${LessThanOneHour}|${LessThanOneHour} [àa]))`;
	export const TimeSuffix = `(?<suffix>${AmRegex}|${PmRegex}|${OclockRegex})`;
	export const BasicTime = `(?<basictime>${EngTimeRegex}|${HourNumRegex}|${BaseDateTime.HourRegex}:${BaseDateTime.MinuteRegex}(:${BaseDateTime.SecondRegex})?|${BaseDateTime.HourRegex})`;
	export const MidnightRegex = `(?<midnight>minuit)`;
	export const MorningRegex = `(?<morning>matin|matin[ée]e)`;
	export const AfternoonRegex = `(?<afternoon>(d'|l')?apr[eè]s(-|\\s*)midi)`;
	export const MidmorningRegex = `(?<midmorning>milieu\\s*d[ue]\\s*${MorningRegex})`;
	export const MiddayRegex = `(?<midday>milieu(\\s*|-)d[eu]\\s*(jour|midi)|apr[eè]s(-|\\s*)midi)`;
	export const MidafternoonRegex = `(?<midafternoon>milieu\\s*d'+${AfternoonRegex})`;
	export const MidTimeRegex = `(?<mid>(${MidnightRegex}|${MidmorningRegex}|${MidafternoonRegex}|${MiddayRegex}))`;
	export const AtRegex = `\\b(((?<=\\b[àa]\\s+)(${EngTimeRegex}|${HourNumRegex}|${BaseDateTime.HourRegex}|${MidTimeRegex}))|${MidTimeRegex})\\b`;
	export const IshRegex = `\\b(peu\\s*pr[èe]s\\s*${BaseDateTime.HourRegex}|peu\\s*pr[èe]s\\s*${EngTimeRegex}|peu\\s*pr[èe]s\\s*[àa]\\s*${BaseDateTime.HourRegex}|peu pr[èe]s midi)\\b`;
	export const TimeUnitRegex = `(?<unit>heures|heure|hrs|hr|h|minutes|minute|mins|min|secondes|seconde|secs|sec)\\b`;
	export const RestrictedTimeUnitRegex = `(?<unit>huere|minute)\\b`;
	export const ConnectNumRegex = `${BaseDateTime.HourRegex}(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\\s*${DescRegex}`;
	export const TimeRegex1 = `\\b(${EngTimeRegex}|${HourNumRegex}|${BaseDateTime.HourRegex})\\s*${DescRegex}(\\s+${TimePrefix})?`;
	export const TimeRegex2 = `(\\b${TimePrefix}\\s+)?(T)?${BaseDateTime.HourRegex}(\\s*)?:(\\s*)?${BaseDateTime.MinuteRegex}((\\s*)?:(\\s*)?${BaseDateTime.SecondRegex})?((\\s*${DescRegex})|\\b)`;
	export const TimeRegex3 = `\\b${BaseDateTime.HourRegex}\\.${BaseDateTime.MinuteRegex}(\\s*${DescRegex})(\\s+${TimePrefix})?`;
	export const TimeRegex4 = `\\b${BasicTime}(\\s*${DescRegex})?(\\s+${TimePrefix})?\\s+${TimeSuffix}\\b`;
	export const TimeRegex5 = `\\b${BasicTime}((\\s*${DescRegex})|\\b)(\\s+${TimePrefix})?`;
	export const TimeRegex6 = `${BasicTime}(\\s*${DescRegex})?\\s+${TimeSuffix}\\b`;
	export const TimeRegex7 = `\\b${TimeSuffix}\\s+[àa]\\s+${BasicTime}((\\s*${DescRegex})|\\b)`;
	export const TimeRegex8 = `\\b${TimeSuffix}\\s+${BasicTime}((\\s*${DescRegex})|\\b)`;
	export const TimeRegex9 = `\\b${PeriodHourNumRegex}\\s+${FivesRegex}((\\s*${DescRegex})|\\b)`;
	export const FivesRegex = `(?<tens>(quinze|vingt(\\s*|-*(cinq))?|trente(\\s*|-*(cinq))?|quarante(\\s*|-*(cinq))??|cinquante(\\s*|-*(cinq))?|dix|cinq))\\b`;
	export const HourRegex = `(?<hour>00|01|02|03|04|05|06|07|08|09|0|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|1|2|3|4|5|6|7|8|9)`;
	export const PeriodHourNumRegex = `(?<hour>vingt-et-un|vingt-deux|vingt-trois|vingt-quatre|zero|une|deux|trois|quatre|cinq|six|sept|huit|neuf|dix|onze|douze|treize|quatorze|quinze|seize|dix-sept|dix-huit|dix-neuf|vingt)`;
	export const PeriodDescRegex = `(?<desc>pm|am|p\\.m\\.|a\\.m\\.|p)`;
	export const PeriodPmRegex = `(?<pm>dans l'apr[eè]s-midi|ce soir|d[eu] soir|dans l[ea] soir[eé]e|dans la nuit|d[eu] soir[ée]e)s?`;
	export const PeriodAmRegex = `(?<am>matin|d[eu] matin|matin[ée]e)s?`;
	export const PureNumFromTo = `((du|de|des|depuis)\\s+)?(${HourRegex}|${PeriodHourNumRegex})(\\s*(?<leftDesc>${PeriodDescRegex}))?\\s*${TillRegex}\\s*(${HourRegex}|${PeriodHourNumRegex})\\s*(?<rightDesc>${PmRegex}|${AmRegex}|${PeriodDescRegex})?`;
	export const PureNumBetweenAnd = `(entre\\s+)(${HourRegex}|${PeriodHourNumRegex})(\\s*(?<leftDesc>${PeriodDescRegex}))?\\s*${RangeConnectorRegex}\\s*(${HourRegex}|${PeriodHourNumRegex})\\s*(?<rightDesc>${PmRegex}|${AmRegex}|${PeriodDescRegex})?`;
	export const PrepositionRegex = `(?<prep>^([aà] la|en|sur\\s*l[ea]|sur|de)$)`;
	export const TimeOfDayRegex = `\\b(?<timeOfDay>((((dans\\s+(l[ea])?\\s+)?((?<early>d[eé]but(\\s+|-)|t[oô]t(\\s+|-)(l[ea]\\s*)?)|(?<late>fin\\s*|fin de(\\s+(la)?)|tard\\s*))?(matin[ée]e|matin|((d|l)?'?)apr[eè]s[-|\\s*]midi|nuit|soir|soir[eé]e)))|(((\\s+(l[ea])?\\s+)?)(jour|journ[eé]e)))s?)\\b`;
	export const SpecificTimeOfDayRegex = `\\b((${RelativeRegex}\\s+${TimeOfDayRegex})|(${TimeOfDayRegex}\\s*(${NextSuffixRegex}))\\b|\\bsoir|\\bdu soir)s?\\b`;
	export const TimeFollowedUnit = `^\\s*${TimeUnitRegex}`;
	export const TimeNumberCombinedWithUnit = `\\b(?<num>\\d+(\\.\\d*)?)${TimeUnitRegex}`;
	export const NowRegex = `\\b(?<now>(ce\\s+)?moment|maintenant|d[eè]s que possible|dqp|r[eé]cemment|auparavant)\\b`;
	export const SuffixRegex = `^\\s*(dans\\s+l[ea]\\s+)|(en\\s+)|(du)?(matin|matin([eé]e)?|apr[eè]s-midi|soir[eé]e|nuit)\\b`;
	export const DateTimeTimeOfDayRegex = `\\b(?<timeOfDay>matin|matin[ée]e|apr[eè]s-midi|nuit|soir)\\b`;
	export const DateTimeSpecificTimeOfDayRegex = `\\b((${RelativeRegex}\\s+${DateTimeTimeOfDayRegex})\\b|\\b(ce|cette\\s+)(soir|nuit))\\b`;
	export const TimeOfTodayAfterRegex = `^\\s*(,\\s*)?(en|dans|du\\s+)?${DateTimeSpecificTimeOfDayRegex}`;
	export const TimeOfTodayBeforeRegex = `${DateTimeSpecificTimeOfDayRegex}(\\s*,)?(\\s+([àa]|pour))?\\s*$`;
	export const SimpleTimeOfTodayAfterRegex = `(${HourNumRegex}|${BaseDateTime.HourRegex})\\s*(,\\s*)?(en|[àa]\\s+)?${DateTimeSpecificTimeOfDayRegex}`;
	export const SimpleTimeOfTodayBeforeRegex = `${DateTimeSpecificTimeOfDayRegex}(\\s*,)?(\\s+([àa]|vers))?\\s*(${HourNumRegex}|${BaseDateTime.HourRegex})`;
	export const TheEndOfRegex = `(la\\s+)?fin(\\s+de\\s*|\\s*de*l[ea])?\\s*$`;
	export const PeriodTimeOfDayRegex = `\\b((dans\\s+(le)?\\s+)?((?<early>d[eé]but(\\s+|-|d[ue]|de la)|t[oô]t)|(?<late>tard\\s*|fin(\\s+|-|d[eu])?))?(?<timeOfDay>matin|((d|l)?'?)apr[eè]s-midi|nuit|soir|soir[eé]e))\\b`;
	export const PeriodSpecificTimeOfDayRegex = `\\b((${RelativeRegex}\\s+${PeriodTimeOfDayRegex})\\b|\\b(ce|cette\\s+)(soir|nuit))\\b`;
	export const PeriodTimeOfDayWithDateRegex = `\\b((${TimeOfDayRegex}))\\b`;
	export const DurationUnitRegex = `(?<unit>ans|ann[eé]e|mois|semaines|semaine|jour|jours|heures|heure|hrs|hr|h|minutes|minute|mins|min|secondes|seconde|secs|sec|ann[eé]es|journ[eé]e)\\b`;
	export const SuffixAndRegex = `(?<suffix>\\s*(et)\\s+((un|une)\\s+)?(?<suffix_num>demi|quart))`;
	export const PeriodicRegex = `\\b(?<periodic>quotidienne|quotidien|journellement|mensuel|mensuelle|jour|jours|hebdomadaire|bihebdomadaire|annuellement|annuel)\\b`;
	export const EachUnitRegex = `(?<each>(chaque|toutes les|tous les)(?<other>\\s+autres)?\\s*${DurationUnitRegex})`;
	export const EachPrefixRegex = `\\b(?<each>(chaque|tous les|(toutes les))\\s*$)`;
	export const SetEachRegex = `\\b(?<each>(chaque|tous les|(toutes les))\\s*)`;
	export const SetLastRegex = `(?<last>prochain|dernier|derni[eè]re|pass[ée]s|pr[eé]c[eé]dent|courant|en\\s*cours)`;
	export const EachDayRegex = `^\\s*(chaque|tous les)\\s*(jour|jours)\\b`;
	export const DurationFollowedUnit = `^\\s*${SuffixAndRegex}?(\\s+|-)?${DurationUnitRegex}`;
	export const NumberCombinedWithDurationUnit = `\\b(?<num>\\d+(\\.\\d*)?)(-)?${DurationUnitRegex}`;
	export const AnUnitRegex = `\\b(((?<half>demi\\s+)?(-)\\s+${DurationUnitRegex}))`;
	export const AllRegex = `\\b(?<all>toute\\s(l['ea])\\s?(?<unit>ann[eé]e|mois|semaine|semaines|jour|jours|journ[eé]e))\\b`;
	export const HalfRegex = `(((un|une)\\s*)|\\b)(?<half>demi?(\\s*|-)+(?<unit>ann[eé]e|ans|mois|semaine|jour|heure))\\b`;
	export const ConjunctionRegex = `\\b((et(\\s+de|pour)?)|avec)\\b`;
	export const YearRegex = `\\b(?<year>19\\d{2}|20\\d{2})\\b`;
	export const HolidayRegex1 = `\\b(?<holiday>vendredi saint|mercredi des cendres|p[aâ]ques|l'action de gr[âa]ce|mardi gras|la saint-sylvestre|la saint sylvestre|la Saint-Valentin|la saint valentin|nouvel an chinois|nouvel an|r[eé]veillon de Nouvel an|jour de l'an|premier-mai|ler-mai|1-mai|poisson d'avril|r[eé]veillon de No[eë]l|veille de no[eë]l|noël|noel|thanksgiving|halloween|yuandan)(\\s+((d[ue]\\s+|d'))?(${YearRegex}|(${ThisPrefixRegex}\\s+)ann[eé]e|ann[eé]e\\s+(${PastSuffixRegex}|${NextSuffixRegex})))?\\b`;
	export const HolidayRegex2 = `\\b(?<holiday>martin luther king|martin luther king jr|toussaint|st patrick|st george|cinco de mayo|l'ind[eé]pendance|guy fawkes)(\\s+(de\\s+)?(${YearRegex}|${ThisPrefixRegex}\\s+ann[eé]e|ann[eé]e\\s+(${PastSuffixRegex}|${NextSuffixRegex})))?\\b`;
	export const HolidayRegex3 = `(?<holiday>(jour\\s*(d[eu]|des)\\s*(canberra|p[aâ]ques|colomb|bastille|la prise de la bastille|l'ind[eé]pendance|l'ind[eé]pendance am[eé]ricaine|thanks\\s*giving|bapt[êe]me|nationale|d'armistice|inaugueration|marmotte|assomption|femme|comm[ée]moratif)))(\\s+(de\\s+)?(${YearRegex}|{ThisPrefixRegex}\\s+ann[eé]e|ann[eé]e\\s+({PastSuffixRegex}|{NextSuffixRegex})))?`;
	export const HolidayRegex4 = `(?<holiday>(F[eê]te\\s*(d[eu]|des)\\s*)(travail|m[eè]re|m[eè]res|p[eè]re|p[eè]res))(\\s+(de\\s+)?(${YearRegex}|${ThisPrefixRegex}\\s+ann[eé]e|ann[eé]e\\s+(${PastSuffixRegex}|${NextSuffixRegex})))?\\b`;
	export const DateTokenPrefix = 'le ';
	export const TimeTokenPrefix = 'à ';
	export const TokenBeforeDate = 'le ';
	export const TokenBeforeTime = 'à ';
	export const AMTimeRegex = `(?<am>matin|matin[ée]e)`;
	export const PMTimeRegex = `\\b(?<pm>(d'|l')?apr[eè]s-midi|soir|nuit|\\s*ce soir|du soir)\\b`;
	export const BeforeRegex = `\\b(avant)\\b`;
	export const AfterRegex = `\\b(apres)\\b`;
	export const SinceRegex = `\\b(depuis)\\b`;
	export const AgoPrefixRegex = `\\b(y a)\\b`;
	export const LaterRegex = `\\b(plus tard)\\b`;
	export const InConnectorRegex = `\\b(dans|en|sur)\\b`;
	export const AmDescRegex = `(h|am\\b|a\\.m\\.|a m\\b|a\\. m\\.|a\\.m\\b|a\\. m\\b)`;
	export const PmDescRegex = `(h|pm\\b|p\\.m\\.|p\\b|p m\\b|p\\. m\\.|p\\.m\\b|p\\. m\\b)`;
	export const AmPmDescRegex = `(h|ampm)`;
	export const MorningStartEndRegex = `(^(matin))|((matin)$)`;
	export const AfternoonStartEndRegex = `(^((d'|l')?apr[eè]s-midi))|(((d'|l')?apr[eè]s-midi)$)`;
	export const EveningStartEndRegex = `(^(soir|soir[ée]e))|((soir|soir[ée]e)$)`;
	export const NightStartEndRegex = `(^(nuit))|((nuit)$)`;
	export const InExactNumberRegex = `\\b(quelque|quel qu[ée]s|quelqu[ée]s|plusieur|plusieurs|divers)\\b`;
	export const InExactNumberUnitRegex = `(${InExactNumberRegex})\\s+(${DurationUnitRegex})`;
	export const RelativeTimeUnitRegex = `(((${ThisPrefixRegex}?)\\s+(${TimeUnitRegex}(\\s*${NextSuffixRegex}|${PastSuffixRegex})?))|((le))\\s+(${RestrictedTimeUnitRegex}))`;
	export const RelativeDurationUnitRegex = `(((?<=({ThisPrefixRegex})\\s+)?(${DurationUnitRegex})(\\s+{NextSuffixRegex}|{PastSuffixRegex})?)|((le|my))\\s+(${RestrictedTimeUnitRegex}))`;
	export const ConnectorRegex = `^(,|pour|t|vers)$`;
	export const FromToRegex = `\\b(du|de|des|depuis).+(à|a|au)\\b.+`;
	export const SingleAmbiguousMonthRegex = `^(le\\s+)?(may|march)$`;
	export const PrepositionSuffixRegex = `\\b(du|de|[àa]|vers|dans)$`;
	export const FlexibleDayRegex = `(?<DayOfMonth>([A-Za-z]+\\s)?[A-Za-z\\d]+)`;
	export const ForTheRegex = `\\b(((pour le ${FlexibleDayRegex})|(dans (le\\s+)?${FlexibleDayRegex}(?<=(st|nd|rd|th))))(?<end>\\s*(,|\\.|!|\\?|$)))`;
	export const WeekDayAndDayOfMothRegex = `\\b${WeekDayRegex}\\s+(le\\s+${FlexibleDayRegex})\\b`;
	export const RestOfDateRegex = `\\b(Reste|fin)\\s+(d[eu]\\s+)?((le|cette|ce)\\s+)?(?<duration>semaine|mois|l'ann[ée]e)\\b`;
	export const RestOfDateTimeRegex = `\\b(Reste|fin)\\s+(d[eu]\\s+)?((le|cette|ce)\\s+)?(?<unit>jour)\\b`;
	export const UnitMap: ReadonlyMap<string, string> = new Map<string, string>([["annees", "Y"],["annee", "Y"],["ans", "Y"],["mois", "MON"],["semaines", "W"],["semaine", "W"],["journees", "D"],["journee", "D"],["jour", "D"],["jours", "D"],["heures", "H"],["heure", "H"],["hrs", "H"],["hr", "H"],["h", "H"],["minutes", "M"],["minute", "M"],["mins", "M"],["min", "M"],["secondes", "S"],["seconde", "S"],["secs", "S"],["sec", "S"]]);
	export const UnitValueMap: ReadonlyMap<string, number> = new Map<string, number>([["annees", 31536000],["annee", 31536000],["l'annees", 31536000],["l'annee", 31536000],["ans", 31536000],["mois", 2592000],["semaines", 604800],["semaine", 604800],["journees", 86400],["journee", 86400],["jour", 86400],["jours", 86400],["heures", 3600],["heure", 3600],["hrs", 3600],["hr", 3600],["h", 3600],["minutes", 60],["minute", 60],["mins", 60],["min", 60],["secondes", 1],["seconde", 1],["secs", 1],["sec", 1]]);
	export const SeasonMap: ReadonlyMap<string, string> = new Map<string, string>([["printemps", "SP"],["été", "SU"],["automne", "FA"],["hiver", "WI"]]);
	export const SeasonValueMap: ReadonlyMap<string, number> = new Map<string, number>([["SP", 3],["SU", 6],["FA", 9],["WI", 12]]);
	export const CardinalMap: ReadonlyMap<string, number> = new Map<string, number>([["premier", 1],["1er", 1],["deuxième", 2],["2e", 2],["troisième", 3],["troisieme", 3],["3e", 3],["quatrième", 4],["4e", 4],["cinqième", 5],["5e", 5]]);
	export const DayOfWeek: ReadonlyMap<string, number> = new Map<string, number>([["lundi", 1],["mardi", 2],["mecredi", 3],["jeudi", 4],["vendredi", 5],["samedi", 6],["dimanche", 0],["lun", 1],["mar", 2],["mer", 3],["jeu", 4],["ven", 5],["sam", 6],["dim", 0]]);
	export const MonthOfYear: ReadonlyMap<string, number> = new Map<string, number>([["1", 1],["2", 2],["3", 3],["4", 4],["5", 5],["6", 6],["7", 7],["8", 8],["9", 9],["10", 10],["11", 11],["12", 12],["janvier", 1],["fevrier", 2],["mars", 3],["mar", 3],["avril", 4],["avr", 4],["mai", 5],["juin", 6],["jun", 6],["juillet", 7],["aout", 8],["septembre", 9],["octobre", 10],["novembre", 11],["decembre", 12],["janv", 1],["janv.", 1],["jan", 1],["fevr", 2],["fevr.", 2],["févr.", 2],["févr", 2],["fev", 2],["juil", 7],["jul", 7],["sep", 9],["sept.", 9],["sept", 9],["oct", 10],["oct.", 10],["nov", 11],["nov.", 11],["dec", 12],["déc.", 12],["déc", 12],["01", 1],["02", 2],["03", 3],["04", 4],["05", 5],["06", 6],["07", 7],["08", 8],["09", 9]]);
	export const Numbers: ReadonlyMap<string, number> = new Map<string, number>([["zero", 0],["un", 1],["une", 1],["a", 1],["deux", 2],["trois", 3],["quatre", 4],["cinq", 5],["six", 6],["sept", 7],["huit", 8],["neuf", 9],["dix", 10],["onze", 11],["douze", 12],["treize", 13],["quatorze", 14],["quinze", 15],["seize", 16],["dix-sept", 17],["dix-huit", 18],["dix-neuf", 19],["vingt-et-un", 21],["vingt et un", 21],["vingt", 20],["vingt deux", 22],["vingt-deux", 22],["vingt trois", 23],["vingt-trois", 23],["vingt quatre", 24],["vingt-quatre", 24],["vingt cinq", 25],["vingt-cinq", 25],["vingt six", 26],["vingt-six", 26],["vingt sept", 27],["vingt-sept", 27],["vingt huit", 28],["vingt-huit", 28],["vingt neuf", 29],["vingt-neuf", 29],["trente", 30],["trente et un", 31],["trente-et-un", 31],["trente deux", 32],["trente-deux", 32],["trente trois", 33],["trente-trois", 33],["trente quatre", 34],["trente-quatre", 34],["trente cinq", 35],["trente-cinq", 35],["trente six", 36],["trente-six", 36],["trente sept", 37],["trente-sept", 37],["trente huit", 38],["trente-huit", 38],["trente neuf", 39],["trente-neuf", 39],["quarante", 40],["quarante et un", 41],["quarante-et-un", 41],["quarante deux", 42],["quarante-duex", 42],["quarante trois", 43],["quarante-trois", 43],["quarante quatre", 44],["quarante-quatre", 44],["quarante cinq", 45],["quarante-cinq", 45],["quarante six", 46],["quarante-six", 46],["quarante sept", 47],["quarante-sept", 47],["quarante huit", 48],["quarante-huit", 48],["quarante neuf", 49],["quarante-neuf", 49],["cinquante", 50],["cinquante et un", 51],["cinquante-et-un", 51],["cinquante deux", 52],["cinquante-deux", 52],["cinquante trois", 53],["cinquante-trois", 53],["cinquante quatre", 54],["cinquante-quatre", 54],["cinquante cinq", 55],["cinquante-cinq", 55],["cinquante six", 56],["cinquante-six", 56],["cinquante sept", 57],["cinquante-sept", 57],["cinquante huit", 58],["cinquante-huit", 58],["cinquante neuf", 59],["cinquante-neuf", 59],["soixante", 60],["soixante et un", 61],["soixante-et-un", 61],["soixante deux", 62],["soixante-deux", 62],["soixante trois", 63],["soixante-trois", 63],["soixante quatre", 64],["soixante-quatre", 64],["soixante cinq", 65],["soixante-cinq", 65],["soixante six", 66],["soixante-six", 66],["soixante sept", 67],["soixante-sept", 67],["soixante huit", 68],["soixante-huit", 68],["soixante neuf", 69],["soixante-neuf", 69],["soixante dix", 70],["soixante-dix", 70],["soixante et onze", 71],["soixante-et-onze", 71],["soixante douze", 72],["soixante-douze", 72],["soixante treize", 73],["soixante-treize", 73],["soixante quatorze", 74],["soixante-quatorze", 74],["soixante quinze", 75],["soixante-quinze", 75],["soixante seize", 76],["soixante-seize", 76],["soixante dix sept", 77],["soixante-dix-sept", 77],["soixante dix huit", 78],["soixante-dix-huit", 78],["soixante dix neuf", 79],["soixante-dix-neuf", 79],["quatre vingt", 80],["quatre-vingt", 80],["quatre vingt un", 81],["quatre-vingt-un", 81],["quatre vingt deux", 82],["quatre-vingt-duex", 82],["quatre vingt trois", 83],["quatre-vingt-trois", 83],["quatre vingt quatre", 84],["quatre-vingt-quatre", 84],["quatre vingt cinq", 85],["quatre-vingt-cinq", 85],["quatre vingt six", 86],["quatre-vingt-six", 86],["quatre vingt sept", 87],["quatre-vingt-sept", 87],["quatre vingt huit", 88],["quatre-vingt-huit", 88],["quatre vingt neuf", 89],["quatre-vingt-neuf", 89],["quatre vingt dix", 90],["quatre-vingt-dix", 90],["quatre vingt onze", 91],["quatre-vingt-onze", 91],["quatre vingt douze", 92],["quatre-vingt-douze", 92],["quatre vingt treize", 93],["quatre-vingt-treize", 93],["quatre vingt quatorze", 94],["quatre-vingt-quatorze", 94],["quatre vingt quinze", 95],["quatre-vingt-quinze", 95],["quatre vingt seize", 96],["quatre-vingt-seize", 96],["quatre vingt dix sept", 97],["quatre-vingt-dix-sept", 97],["quatre vingt dix huit", 98],["quatre-vingt-dix-huit", 98],["quatre vingt dix neuf", 99],["quatre-vingt-dix-neuf", 99],["cent", 100]]);
	export const DayOfMonth: ReadonlyMap<string, number> = new Map<string, number>([["1er", 1],["2e", 2],["3e", 3],["4e", 4],["5e", 5],["6e", 6],["7e", 7],["8e", 8],["9e", 9],["10e", 10],["11e", 11],["12e", 12],["13e", 13],["14e", 14],["15e", 15],["16e", 16],["17e", 17],["18e", 18],["19e", 19],["20e", 20],["21e", 21],["22e", 22],["23e", 23],["24e", 24],["25e", 25],["26e", 26],["27e", 27],["28e", 28],["29e", 29],["30e", 30],["31e", 31]]);
	export const DoubleNumbers: ReadonlyMap<string, number> = new Map<string, number>([["demi", 0.5],["quart", 0.25]]);
	export const HolidayNames: ReadonlyMap<string, string[]> = new Map<string, string[]>([["peres", ["fatherday","fathersday"]],["pères", ["fatherday","fathersday"]],["meres", ["motherday","mothersday"]],["mères", ["motherday","mothersday"]],["thanksgiving", ["thanksgivingday","thanksgiving"]],["martinlutherking", ["martinlutherkingday","martinlutherkingjrday"]],["canberra", ["canberraday"]],["travail", ["labourday","laborday"]],["colomb", ["columbusday"]],["commémoratif", ["memorialday"]],["commemoratif", ["memorialday"]],["yuandan", ["yuandan"]],["toussaint", ["allsaintsday"]],["femme", ["femaleday"]],["noel", ["christmas"]],["noël", ["christmas"]],["newyear", ["newyear"]],["newyearday", ["newyearday"]],["newyearsday", ["newyearsday"]],["inaugurationday", ["inaugurationday"]],["groundhougday", ["groundhougday"]],["valentinesday", ["valentinesday"]],["stpatrickday", ["stpatrickday"]],["aprilfools", ["aprilfools"]],["stgeorgeday", ["stgeorgeday"]],["mayday", ["mayday"]],["cincodemayoday", ["cincodemayoday"]],["baptisteday", ["baptisteday"]],["usindependenceday", ["usindependenceday"]],["independenceday", ["independenceday"]],["bastilleday", ["bastilleday"]],["halloween", ["halloweenday"]],["guyfawkesday", ["guyfawkesday"]],["veteransday", ["veteransday"]],["christmaseve", ["christmaseve"]],["newyeareve", ["newyearseve","newyeareve"]]]);
}

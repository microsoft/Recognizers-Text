# ------------------------------------------------------------------------------
# <auto-generated>
#     This code was generated by a tool.
#     Changes to this file may cause incorrect behavior and will be lost if
#     the code is regenerated.
# </auto-generated>
#
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.
# ------------------------------------------------------------------------------

from .base_date_time import BaseDateTime
# pylint: disable=line-too-long


class PortugueseDateTime:
    LangMarker = 'Por'
    CheckBothBeforeAfter = False
    TillRegex = f'(?<till>\\b(at[eé]h?|[aà]s|ao?)\\b|--|-|—|——)(\\s+\\b(o|[aà](s)?)\\b)?'
    RangeConnectorRegex = f'(?<and>(e\\s*(([àa]s?)|o)?)|{BaseDateTime.RangeConnectorSymbolRegex})'
    DayRegex = f'(?<day>(?:3[0-1]|[1-2]\\d|0?[1-9]))(?=\\b|t)'
    WrittenDayRegex = f'(?<day>(vinte\\s+e\\s+)?(um|dois|tr[eê]s|quatro|cinco|seis|sete|oito|nove)|dez|onze|doze|treze|(c|qu)atorze|quinze|dez[ae](s(seis|sete)|nove)|dezoito|vinte|trinta(\\s+e\\s+um)?)'
    MonthNumRegex = f'(?<month>1[0-2]|(0)?[1-9])\\b'
    AmDescRegex = f'({BaseDateTime.BaseAmDescRegex})'
    PmDescRegex = f'({BaseDateTime.BasePmDescRegex})'
    AmPmDescRegex = f'({BaseDateTime.BaseAmPmDescRegex})'
    OclockRegex = f'(?<oclock>em\\s+ponto)'
    DescRegex = f'((horas\\s+)?(?<desc>{AmDescRegex}|{PmDescRegex})|{OclockRegex})'
    OfPrepositionRegex = f'(\\bd(o|a|e)s?\\b)'
    AfterNextSuffixRegex = f'\\b(que\\s+vem|passad[oa])\\b'
    RangePrefixRegex = f'((de(sde)?|das?|entre)\\s+(a(s)?\\s+)?)'
    TwoDigitYearRegex = f'\\b(?<![$])(?<year>([0-9]\\d))(?!(\\s*((\\:\\d)|{AmDescRegex}|{PmDescRegex}|\\.\\d)))\\b'
    RelativeRegex = f'(?<order>((n?est[ae]s?|pr[oó]xim[oa]s?|([uú]ltim[ao]s?))(\\s+fina(l|is)\\s+d[eao])?)|(fina(l|is)\\s+d[eao]))\\b'
    StrictRelativeRegex = f'(?<order>((n?est[ae]|pr[oó]xim[oa]|([uú]ltim(o|as|os)))(\\s+fina(l|is)\\s+d[eao])?)|(fina(l|is)\\s+d[eao]))\\b'
    WrittenOneToNineRegex = f'(uma?|dois|duas|tr[eê]s|quatro|cinco|seis|sete|oito|nove)'
    WrittenOneHundredToNineHundredRegex = f'(duzent[oa]s|trezent[oa]s|[cq]uatrocent[ao]s|quinhent[ao]s|seiscent[ao]s|setecent[ao]s|oitocent[ao]s|novecent[ao]s|cem|(?<!por\\s+)(cento))'
    WrittenOneToNinetyNineRegex = f'(((vinte|trinta|[cq]uarenta|cinquenta|sessenta|setenta|oitenta|noventa)(\\s+e\\s+{WrittenOneToNineRegex})?)|d[eé]z|onze|doze|treze|(c|qu)atorze|quinze|dez[ea]sseis|dez[ea]ssete|dez[ea]nove|dezoito|uma?|d(oi|ua)s|tr[eê]s|quatro|cinco|seis|sete|oito|nove)'
    FullTextYearRegex = f'\\b(?<fullyear>((dois\\s+)?mil)((\\s+e)?\\s+{WrittenOneHundredToNineHundredRegex})?((\\s+e)?\\s+{WrittenOneToNinetyNineRegex})?)'
    YearRegex = f'({BaseDateTime.FourDigitYearRegex}|{FullTextYearRegex})'
    RelativeMonthRegex = f'(?<relmonth>([nd]?es[st]e|pr[óo]ximo|passsado|[uú]ltimo)\\s+m[eê]s)\\b'
    MonthRegex = f'(?<month>abr(il)?|ago(sto)?|dez(embro)?|fev(ereiro)?|jan(eiro)?|ju[ln](ho)?|mar([çc]o)?|maio?|nov(embro)?|out(ubro)?|sep?t(embro)?)'
    MonthSuffixRegex = f'(?<msuf>((em|no)\\s+|d[eo]\\s+)?({RelativeMonthRegex}|{MonthRegex}))'
    DateUnitRegex = f'(?<unit>(?<uoy>m[êe]s)(?<plural>es)?|(ano|(?<uoy>semana|dia))(?<plural>s)?)\\b'
    PastRegex = f'(?<past>\\b(passad[ao](s)?|[uú]ltim[oa](s)?|anterior(es)?|h[aá]|pr[ée]vi[oa](s)?)\\b)'
    FutureRegex = f'(?<past>\\b(seguinte(s)?|pr[oó]xim[oa](s)?|daqui\\s+a)\\b)'
    SimpleCasesRegex = f'\\b((desde\\s+[oa]|desde|d[oa])\\s+)?(dia\\s+)?({DayRegex})\\s*{TillRegex}\\s*(o dia\\s+)?({DayRegex})\\s+{MonthSuffixRegex}((\\s+|\\s*,\\s*){YearRegex})?\\b'
    MonthFrontSimpleCasesRegex = f'\\b{MonthSuffixRegex}\\s+((desde\\s+[oa]|desde|d[oa])\\s+)?(dia\\s+)?({DayRegex})\\s*{TillRegex}\\s*({DayRegex})((\\s+|\\s*,\\s*){YearRegex})?\\b'
    MonthFrontBetweenRegex = f'\\b{MonthSuffixRegex}\\s+((entre|entre\\s+[oa]s?)\\s+)(dias?\\s+)?({DayRegex})\\s*{RangeConnectorRegex}\\s*({DayRegex})((\\s+|\\s*,\\s*){YearRegex})?\\b'
    DayBetweenRegex = f'\\b((entre|entre\\s+[oa]s?)\\s+)(dia\\s+)?({DayRegex})\\s*{RangeConnectorRegex}\\s*({DayRegex})\\s+{MonthSuffixRegex}((\\s+|\\s*,\\s*){YearRegex})?\\b'
    SpecialYearPrefixes = f'((do\\s+)?calend[aá]rio|civil|(?<special>fiscal|escolar|letivo))'
    OneWordPeriodRegex = f'\\b(((pr[oó]xim[oa]?|[nd]?es[st]e|aquel[ea]|[uú]ltim[oa]?|em)\\s+)?(?<month>abr(il)?|ago(sto)?|dez(embro)?|fev(ereiro)?|jan(eiro)?|ju[ln](ho)?|mar([çc]o)?|maio?|nov(embro)?|out(ubro)?|sep?t(embro)?)|({RelativeRegex}\\s+)?(ano\\s+{SpecialYearPrefixes}|{SpecialYearPrefixes}\\s+ano)|(?<=\\b(de|do|da|o|a)\\s+)?(pr[oó]xim[oa](s)?|[uú]ltim[oa]s?|est(e|a))\\s+(fim de semana|fins de semana|semana|m[êe]s|ano)|fim de semana|fins de semana|(m[êe]s|anos)? [àa] data)\\b'
    MonthWithYearRegex = f'\\b((((pr[oó]xim[oa](s)?|[nd]?es[st]e|aquele|[uú]ltim[oa]?|em)\\s+)?{MonthRegex}|((n?o\\s+)?(?<cardinal>primeiro|1o|segundo|2o|terceiro|3o|[cq]uarto|4o|quinto|5o|sexto|6o|s[eé]timo|7o|oitavo|8o|nono|9o|d[eé]cimo(\\s+(primeiro|segundo))?|10o|11o|12o|[uú]ltimo)\\s+m[eê]s(?=\\s+(d[aeo]|[ao]))))\\s+((d[aeo]|[ao])\\s+)?({YearRegex}|{TwoDigitYearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|[nd]?es[st]e)\\s+ano))\\b'
    MonthNumWithYearRegex = f'({YearRegex}(\\s*?)[/\\-\\.](\\s*?){MonthNumRegex})|({MonthNumRegex}(\\s*?)[/\\-](\\s*?){YearRegex})'
    WeekOfMonthRegex = f'(?<wom>(a|na\\s+)?(?<cardinal>primeira?|1a|segunda|2a|terceira|3a|[qc]uarta|4a|quinta|5a|[uú]ltima)\\s+semana\\s+{MonthSuffixRegex})'
    WeekOfYearRegex = f'(?<woy>(a|na\\s+)?(?<cardinal>primeira?|1a|segunda|2a|terceira|3a|[qc]uarta|4a|quinta|5a|[uú]ltima?)\\s+semana(\\s+d[oe]?)?\\s+({YearRegex}|(?<order>pr[oó]ximo|[uú]ltimo|[nd]?es[st]e)\\s+ano))'
    OfYearRegex = f'\\b((d[aeo]?|[ao])\\s*({YearRegex}|{StrictRelativeRegex}\\s+ano))\\b'
    FirstLastRegex = f'\\b(n?[ao]s?\\s+)?((?<first>primeir[ao]s?)|(?<last>[uú]ltim[ao]s?))\\b'
    FollowedDateUnit = f'^\\s*{DateUnitRegex}'
    NumberCombinedWithDateUnit = f'\\b(?<num>\\d+(\\.\\d*)?){DateUnitRegex}'
    QuarterRegex = f'(n?o\\s+)?(?<cardinal>primeiro|1[oº]|segundo|2[oº]|terceiro|3[oº]|[qc]uarto|4[oº])\\s+trimestre(\\s+d[oe]|\\s*,\\s*)?\\s+({YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|[nd]?es[st]e)\\s+ano)'
    QuarterRegexYearFront = f'({YearRegex}|(?<order>pr[oó]ximo(s)?|[uú]ltimo?|[nd]?es[st]e)\\s+ano)\\s+(n?o\\s+)?(?<cardinal>(primeiro)|1[oº]|segundo|2[oº]|terceiro|3[oº]|[qc]uarto|4[oº])\\s+trimestre'
    AllHalfYearRegex = f'^[.]'
    PrefixDayRegex = f'^[.]'
    SeasonRegex = f'\\b(?<season>(([uú]ltim[oa]|[nd]?es[st][ea]|n?[oa]|(pr[oó]xim[oa]s?|seguinte))\\s+)?(?<seas>primavera|ver[ãa]o|outono|inverno)((\\s+)?(seguinte|((de\\s+|,)?\\s*{YearRegex})|((do\\s+)?(?<order>pr[oó]ximo|[uú]ltimo|[nd]?es[st]e)\\s+ano)))?)\\b'
    WhichWeekRegex = f'\\b(semana)(\\s*)(?<number>5[0-3]|[1-4]\\d|0?[1-9])(\\s+(de|do)\\s+({YearRegex}|(?<order>pr[oó]ximo|[uú]ltimo|[nd]?es[st]e)\\s+ano|ano\\s+(?<order>passado)))?\\b'
    WeekOfRegex = f'(semana)(\\s*)((do|da|de))'
    MonthOfRegex = f'(mes)(\\s*)((do|da|de))'
    RangeUnitRegex = f'\\b(?<unit>anos?|meses|m[êe]s|semanas?)\\b'
    BeforeAfterRegex = f'^[.]'
    UpcomingPrefixRegex = f'.^'
    NextPrefixRegex = f'(pr[oó]xim[oa]s?|seguinte|{UpcomingPrefixRegex})\\b'
    InConnectorRegex = f'\\b(em)\\b'
    SinceYearSuffixRegex = f'^[.]'
    WithinNextPrefixRegex = f'\\b(dentro\\s+d(e|as)(\\s+(?<next>{NextPrefixRegex}))?)\\b'
    TodayNowRegex = f'\\b(hoje|agora)\\b'
    CenturySuffixRegex = f'^[.]'
    FromRegex = f'(de(sde)?(\\s*a(s)?)?)$'
    BetweenRegex = f'(entre\\s*([oa](s)?)?)'
    WeekDayRegex = f'\\b(?<weekday>(domingos?|(segunda|ter[çc]a|quarta|quinta|sexta)s?([-\\s+]feiras?)?|s[aá]bados?|(2|3|4|5|6)[aª])\\b|(dom|seg|ter[cç]|qua|qui|sex|sab)\\b(\\.?(?=\\s|,|;|$)))'
    OnRegex = f'(?<=\\b(em|no)\\s+)({DayRegex}s?)\\b'
    RelaxedOnRegex = f'((?<=\\b(em|[nd][oa])\\s+)(dia\\s+)?({DayRegex}s?)|dia\\s+{DayRegex}s?)\\b(?!\\s*[/\\\\\\-\\.,:\\s]\\s*(\\d|(de\\s+)?{MonthRegex}))'
    ThisRegex = f'\\b(([nd]?es[st][ea]\\s*){WeekDayRegex})|({WeekDayRegex}\\s*([nd]?es[st]a\\s+semana))\\b'
    LastDateRegex = f'\\b(([uú]ltim[ao])\\s*{WeekDayRegex})|({WeekDayRegex}(\\s+(([nd]?es[st]a|[nd]a)\\s+([uú]ltima\\s+)?semana)))\\b'
    NextDateRegex = f'\\b(((pr[oó]xim[oa]|seguinte)\\s*){WeekDayRegex})|({WeekDayRegex}((\\s+(pr[oó]xim[oa]|seguinte))|(\\s+(da\\s+)?(semana\\s+seguinte|pr[oó]xima\\s+semana))))\\b'
    SpecialDayRegex = f'\\b((d?o\\s+)?(dia\\s+antes\\s+de\\s+ontem|antes\\s+de\\s+ontem|anteontem)|((d?o\\s+)?(dia\\s+|depois\\s+|dia\\s+depois\\s+)?de\\s+amanh[aã])|(o\\s)?dia\\s+seguinte|(o\\s)?pr[oó]ximo\\s+dia|(o\\s+)?[uú]ltimo\\s+dia|ontem|amanh[ãa]|hoje)|(do\\s+dia$)\\b'
    SpecialDayWithNumRegex = f'^[.]'
    ForTheRegex = f'.^'
    FlexibleDayRegex = f'(?<DayOfMonth>([a-z]+\\s)?({WrittenDayRegex}|{DayRegex}))'
    WeekDayAndDayOfMonthRegex = f'\\b{WeekDayRegex}\\s+(dia\\s+{FlexibleDayRegex})\\b'
    WeekDayAndDayRegex = f'\\b{WeekDayRegex}\\s+({DayRegex})(?!([-:/]|\\.\\d|(\\s+({AmDescRegex}|{PmDescRegex}|{OclockRegex}))))\\b'
    WeekDayOfMonthRegex = f'(?<wom>(n?[ao]\\s+)?(?<cardinal>primeir[ao]|1[ao]|segund[ao]|2[ao]|terceir[ao]|3[ao]|[qc]uart[ao]|4[ao]|quint[ao]|5[ao]|[uú]ltim[ao])\\s+{WeekDayRegex}\\s+{MonthSuffixRegex})'
    RelativeWeekDayRegex = f'^[.]'
    AmbiguousRangeModifierPrefix = f'^[.]'
    NumberEndingPattern = f'^[.]'
    SpecialDateRegex = f'(?<=\\bno\\s+){DayRegex}\\b'
    OfMonthRegex = f'^(\\s*de)?\\s*{MonthSuffixRegex}'
    MonthEndRegex = f'({MonthRegex}\\s*(o)?\\s*$)'
    WeekDayEnd = f'{WeekDayRegex}\\s*,?\\s*$'
    WeekDayStart = f'^\\b$'
    DateYearRegex = f'(?<year>{YearRegex}|{TwoDigitYearRegex})'
    DateExtractor1 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?{DayRegex}((\\s*(de)|[/\\\\\\.\\- ])\\s*)?{MonthRegex}\\b'
    DateExtractor2 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?({DayRegex}(\\s*([/\\.\\-]|de)?\\s*{MonthRegex}|\\s+de\\s+{MonthNumRegex})(\\s*([,./-]|de|\\s+)\\s*){DateYearRegex}|{BaseDateTime.FourDigitYearRegex}\\s*[/\\.\\- ]\\s*{DayRegex}\\s*[/\\.\\- ]\\s*{MonthRegex})\\b'
    DateExtractor3 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?{MonthRegex}(\\s*[/\\.\\- ]\\s*|\\s+de\\s+){DayRegex}((\\s*[/\\.\\- ]\\s*|\\s+de\\s+){DateYearRegex})?\\b'
    DateExtractor4 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?{MonthNumRegex}\\s*[/\\\\\\-]\\s*{DayRegex}\\s*[/\\\\\\-]\\s*{DateYearRegex}(?!\\s*[/\\\\\\-\\.]\\s*\\d+)'
    DateExtractor5 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?{DayRegex}\\s*[/\\\\\\-\\.]\\s*({MonthNumRegex}|{MonthRegex})\\s*[/\\\\\\-\\.]\\s*{DateYearRegex}(?!\\s*[/\\\\\\-\\.]\\s*\\d+)'
    DateExtractor6 = f'(?<=\\b(em|no|o)\\s+){MonthNumRegex}[\\-\\.]{DayRegex}{BaseDateTime.CheckDecimalRegex}\\b'
    DateExtractor7 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?{MonthNumRegex}\\s*/\\s*{DayRegex}((\\s+|\\s*(,|de)\\s*){DateYearRegex})?{BaseDateTime.CheckDecimalRegex}\\b'
    DateExtractor8 = f'(?<=\\b(em|no|o)\\s+){DayRegex}[\\\\\\-]{MonthNumRegex}{BaseDateTime.CheckDecimalRegex}\\b'
    DateExtractor9 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?{DayRegex}\\s*/\\s*{MonthNumRegex}((\\s+|\\s*(,|de)\\s*){DateYearRegex})?{BaseDateTime.CheckDecimalRegex}\\b'
    DateExtractor10 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?({YearRegex}\\s*[/\\\\\\-\\.]\\s*({MonthNumRegex}|{MonthRegex})\\s*[/\\\\\\-\\.]\\s*{DayRegex}|{MonthRegex}\\s*[/\\\\\\-\\.]\\s*{BaseDateTime.FourDigitYearRegex}\\s*[/\\\\\\-\\.]\\s*{DayRegex}|{DayRegex}\\s*[/\\\\\\-\\.]\\s*{BaseDateTime.FourDigitYearRegex}\\s*[/\\\\\\-\\.]\\s*{MonthRegex})(?!\\s*[/\\\\\\-\\.:]\\s*\\d+)'
    DateExtractor11 = f'(?<=\\b(dia)\\s+){DayRegex}'
    HourNumRegex = f'\\b(?<hournum>zero|uma|duas|tr[êe]s|[qc]uatro|cinco|seis|sete|oito|nove|dez|onze|doze)\\b'
    MinuteNumRegex = f'(?<minnum>um|dois|tr[êe]s|[qc]uatro|cinco|seis|sete|oito|nove|dez|onze|doze|treze|catorze|quatorze|quinze|dez[ea]sseis|dez[ea]sete|dezoito|dez[ea]nove|vinte|trinta|[qc]uarenta|cin[qc]uenta)'
    DeltaMinuteNumRegex = f'(?<deltaminnum>um|dois|tr[êe]s|[qc]uatro|cinco|seis|sete|oito|nove|dez|onze|doze|treze|catorze|quatorze|quinze|dez[ea]sseis|dez[ea]sete|dezoito|dez[ea]nove|vinte|trinta|[qc]uarenta|cin[qc]uenta)'
    PmRegex = f'(horas\\s+)?(?<pm>((pela|de|da|\\b[àa]\\b|na)\\s+(tarde|noite)))|((depois\\s+do|ap[óo]s\\s+o)\\s+(almo[çc]o|meio dia|meio-dia))'
    AmRegex = f'(horas\\s+)?(?<am>(pela|de|da|na)\\s+(manh[ãa]|madrugada))'
    AmTimeRegex = f'(?<am>([dn]?es[st]a|(pela|de|da|na))\\s+(manh[ãa]|madrugada))'
    PmTimeRegex = f'(?<pm>(([dn]?es[st]a|\\b[àa]\\b|(pela|de|da|na))\\s+(tarde|noite)))|((depois\\s+do|ap[óo]s\\s+o)\\s+(almo[çc]o|meio dia|meio-dia))'
    LessThanOneHour = f'(?<lth>((\\s+e\\s+)?(quinze|(um\\s+|dois\\s+|tr[êes]\\s+)?quartos?)|quinze|(\\s*)(um\\s+|dois\\s+|tr[êes]\\s+)?quartos?|(\\s+e\\s+)(meia|trinta)|({BaseDateTime.DeltaMinuteRegex}|{DeltaMinuteNumRegex})(\\s+(minuto|minutos|min|mins))?))'
    LessThanOneHourSuffix = f'(?<lth>((\\s+e\\s+)?(quinze|(um\\s+|dois\\s+|tr[êes]\\s+)?quartos?)|quinze|(\\s*)(um\\s+|dois\\s+|tr[êes]\\s+)?quartos?|(\\s+e\\s+)(meia|trinta)))'
    TensTimeRegex = f'(?<tens>dez|vinte|trinta|[qc]uarenta|cin[qc]uenta)'
    WrittenTimeRegex = f'(?<writtentime>({HourNumRegex}\\s*((e|menos)\\s+)?(({TensTimeRegex}((\\s*e\\s+)?{MinuteNumRegex}))|{MinuteNumRegex}))|(({MinuteNumRegex}|({TensTimeRegex}((\\s*e\\s+)?{MinuteNumRegex})?))\\s*((para as|pras|antes da|antes das)\\s+)?({HourNumRegex}|{BaseDateTime.HourRegex})))'
    TimePrefix = f'(?<prefix>{LessThanOneHour}(\\s+(passad[ao]s)\\s+(as)?|\\s+depois\\s+(das?|do)|\\s+pras?|\\s+(para|antes)?\\s+([àa]s?)))'
    TimeSuffix = f'(?<suffix>({LessThanOneHour}\\s+)?({AmRegex}|{PmRegex}|{OclockRegex}))'
    BasicTime = f'(?<basictime>{WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}:{BaseDateTime.MinuteRegex}(:{BaseDateTime.SecondRegex})?|{BaseDateTime.HourRegex})'
    MidnightRegex = f'(?<midnight>meia\\s*(-\\s*)?noite)'
    MidmorningRegex = f'(?<midmorning>meio\\s+da\\s+manhã)'
    MidEarlyMorning = f'(?<midearlymorning>meio\\s+da\\s+madrugada)'
    MidafternoonRegex = f'(?<midafternoon>meio\\s+da\\s+tarde)'
    MiddayRegex = f'(?<midday>meio\\s*(-\\s*)?dia)'
    MidTimeRegex = f'(?<mid>({MidnightRegex}|{MidmorningRegex}|{MidEarlyMorning}|{MidafternoonRegex}|{MiddayRegex}))'
    AtRegex = f'\\b(((?<=\\b([aà]s?)\\s+)({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}(\\s+e\\s+{BaseDateTime.MinuteRegex})?)(\\s+horas?|\\s*h\\b)?|(?<=\\b(s(er)?[aã]o|v[aã]o\\s+ser|^[eé]h?)\\s+|^\\s*)({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})(\\s+horas?|\\s*h\\b))(\\s+{OclockRegex})?|{MidTimeRegex})\\b'
    ConnectNumRegex = f'({BaseDateTime.HourRegex}(?<min>[0-5][0-9])\\s*{DescRegex})'
    TimeRegex1 = f'(\\b{TimePrefix}\\s+)?({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})\\s*({DescRegex})'
    TimeRegex2 = f'(\\b{TimePrefix}\\s+)?(t)?{BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex}((\\s*)?:(\\s*)?{BaseDateTime.SecondRegex})?((\\s*{DescRegex})|\\b)'
    TimeRegex3 = f'(\\b{TimePrefix}\\s+)?{BaseDateTime.HourRegex}\\.{BaseDateTime.MinuteRegex}(\\s*{DescRegex})'
    TimeRegex4 = f'\\b(({DescRegex}\\s*)?(({TimePrefix}\\s*)({HourNumRegex}|{BaseDateTime.HourRegex})|({HourNumRegex}|{BaseDateTime.HourRegex})(\\s+{TensTimeRegex}(\\s+e\\s+)?{MinuteNumRegex}?)|{BasicTime}(\\s*{DescRegex})?(?<prefix>{LessThanOneHourSuffix}))(\\s*({DescRegex}|{OclockRegex}))?)\\b'
    TimeRegex5 = f'\\b({TimePrefix}|{BasicTime}(?<prefix>{LessThanOneHourSuffix}))\\s+(\\s*{DescRegex})?{BasicTime}?\\s*{TimeSuffix}\\b'
    TimeRegex6 = f'({BasicTime}(\\s*{DescRegex})?\\s+{TimeSuffix}\\b)'
    TimeRegex7 = f'\\b{TimeSuffix}\\s+[àa]s?\\s+{BasicTime}((\\s*{DescRegex})|\\b)'
    TimeRegex8 = f'\\b{TimeSuffix}\\s+{BasicTime}((\\s*{DescRegex})|\\b)'
    TimeRegex9 = f'\\b(?<writtentime>{HourNumRegex}\\s+({TensTimeRegex}\\s*)(e\\s+)?{MinuteNumRegex}?)\\b'
    TimeRegex11 = f'\\b({WrittenTimeRegex})(\\s+{DescRegex})?\\b'
    TimeRegex12 = f'(\\b{TimePrefix}\\s+)?{BaseDateTime.HourRegex}(\\s*h\\s*){BaseDateTime.MinuteRegex}(\\s*{DescRegex})?'
    PrepositionRegex = f'(?<prep>([àa]s?|em|por|pel[ao]|n[ao]|de|d[ao]?)?$)'
    NowRegex = f'\\b(?<now>((logo|exatamente)\\s+)?agora(\\s+mesmo)?|neste\\s+momento|(assim\\s+que|t[ãa]o\\s+cedo\\s+quanto)\\s+(poss[ií]vel|possas?|possamos)|o\\s+mais\\s+(cedo|r[aá]pido)\\s+poss[íi]vel|recentemente|previamente)\\b'
    SuffixRegex = f'^\\s*((e|a|em|por|pel[ao]|n[ao]|de)\\s+)?(manh[ãa]|madrugada|meio\\s*dia|tarde|noite)\\b'
    TimeOfDayRegex = f'\\b(?<timeOfDay>manh[ãa]|madrugada|tarde|noite|((depois\\s+do|ap[óo]s\\s+o)\\s+(almo[çc]o|meio[ -]dia)))\\b'
    SpecificTimeOfDayRegex = f'\\b(((((a)?\\s+|[nd]?es[st]a|seguinte|pr[oó]xim[oa]|[uú]ltim[oa])\\s+)?{TimeOfDayRegex}))\\b'
    TimeOfTodayAfterRegex = f'^\\s*(,\\s*)?([àa]|em|por|pel[ao]|de|no|na?\\s+)?{SpecificTimeOfDayRegex}'
    TimeOfTodayBeforeRegex = f'({SpecificTimeOfDayRegex}(\\s*,)?(\\s+([àa]s|para))?\\s*)'
    SimpleTimeOfTodayAfterRegex = f'({HourNumRegex}|{BaseDateTime.HourRegex})\\s*(,\\s*)?{SpecificTimeOfDayRegex}'
    SimpleTimeOfTodayBeforeRegex = f'({SpecificTimeOfDayRegex}(\\s*,)?(\\s+([àa]s|((cerca|perto|ao\\s+redor|por\\s+volta)\\s+(de|das))))?\\s*({HourNumRegex}|{BaseDateTime.HourRegex}))'
    SpecificEndOfRegex = f'([na]o\\s+)?(fi(m|nal)|t[ée]rmin(o|ar))(\\s+d?o(\\s+dia)?(\\s+de)?)?\\s*$'
    UnspecificEndOfRegex = f'^[.]'
    UnspecificEndOfRangeRegex = f'^[.]'
    UnitRegex = f'(?<unit>anos?|meses|m[êe]s|semanas?|dias?|horas?|hrs?|hs?|minutos?|mins?|segundos?|segs?)\\b'
    ConnectorRegex = f'^(,|t|para [ao]|para as|pras|(cerca|perto|ao\\s+redor|por\\s+volta)\\s+(de|das)|quase)$'
    TimeHourNumRegex = f'(?<hour>vinte( e (um|dois|tr[êe]s|quatro))?|zero|uma?|dois|duas|tr[êe]s|quatro|cinco|seis|sete|oito|nove|dez|onze|doze|treze|quatorze|catorze|quinze|dez([ea]sseis|[ea]ssete|oito|[ea]nove))'
    PureNumFromTo = f'(((desde|de|da|das)\\s+(a(s)?\\s+)?)?({BaseDateTime.HourRegex}|{TimeHourNumRegex})(\\s*(?<leftDesc>{DescRegex}|horas))?\\s*{TillRegex}(?<![aà]s)|((desde|de|da|das)\\s+(a(s)?\\s+)?)({BaseDateTime.HourRegex}|{TimeHourNumRegex})(\\s*(?<leftDesc>{DescRegex}|horas))?\\s*{TillRegex})\\s*({BaseDateTime.HourRegex}|{TimeHourNumRegex})\\s*(?<rightDesc>{PmRegex}|{AmRegex}|{DescRegex}|horas)?'
    PureNumBetweenAnd = f'(entre\\s+((a|as)?\\s+)?)({BaseDateTime.HourRegex}|{TimeHourNumRegex})(\\s*(?<leftDesc>{DescRegex}|horas))?\\s*e\\s*(a(s)?\\s+)?({BaseDateTime.HourRegex}|{TimeHourNumRegex})\\s*(?<rightDesc>{PmRegex}|{AmRegex}|{DescRegex}|horas)?'
    SpecificTimeFromTo = f'^[.]'
    SpecificTimeBetweenAnd = f'^[.]'
    TimeUnitRegex = f'(?<unit>(hora|minuto|min|segundo|se[cg])(?<plural>s)?|h)\\b'
    TimeFollowedUnit = f'^\\s*{TimeUnitRegex}'
    TimeNumberCombinedWithUnit = f'\\b(?<num>\\d+(\\,\\d*)?)\\s*{TimeUnitRegex}'
    DateTimePeriodNumberCombinedWithUnit = f'\\b(?<num>\\d+(\\.\\d*)?)\\s*{TimeUnitRegex}'
    PeriodTimeOfDayWithDateRegex = f'\\b((e|[àa]|em|na|no|ao|pel[ao]|de)\\s+)?(?<timeOfDay>manh[ãa]|madrugada|(passado\\s+(o\\s+)?)?meio\\s+dia|tarde|noite)\\b'
    RelativeTimeUnitRegex = f'({PastRegex}|{FutureRegex})\\s+{UnitRegex}|{UnitRegex}\\s+({PastRegex}|{FutureRegex})'
    SuffixAndRegex = f'(?<suffix>\\s*(e)\\s+(?<suffix_num>meia|(um\\s+)?quarto))'
    FollowedUnit = f'^\\s*{UnitRegex}'
    LessThanRegex = f'^[.]'
    MoreThanRegex = f'^[.]'
    DurationNumberCombinedWithUnit = f'\\b(?<num>\\d+(\\,\\d*)?){UnitRegex}'
    AnUnitRegex = f'\\b(um(a)?)\\s+{UnitRegex}'
    DuringRegex = f'^[.]'
    AllRegex = f'\\b(?<all>tod[oa]?\\s+(o|a)\\s+(?<unit>ano|m[êe]s|semana|dia))\\b'
    HalfRegex = f'\\b(?<half>mei[oa]\\s+(?<unit>ano|m[êe]s|semana|dia|hora))\\b'
    ConjunctionRegex = f'^[.]'
    InexactNumberRegex = f'\\b(poucos|pouco|algum|alguns|v[áa]rios)\\b'
    InexactNumberUnitRegex = f'\\b(poucos|pouco|algum|alguns|v[áa]rios)\\s+{UnitRegex}'
    HolidayRegex1 = f'\\b(?<holiday>sexta-feira santa|sexta-feira da paix[ãa]o|quarta-feira de cinzas|carnaval|dia dos? presidentes?|ano novo chin[eê]s|ano novo|v[ée]spera de ano novo|natal|v[ée]spera de natal|dia de a[cç][ãa]o de gra[çc]as|a[cç][ãa]o de gra[çc]as|yuandan|halloween|dia das bruxas|p[áa]scoa)(\\s+(d[eo]?\\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|[nd]?es[st][ea]|[uú]ltim[oa]?|em))\\s+ano))?\\b'
    HolidayRegex2 = f'\\b(?<holiday>(dia\\s+(d[eoa]s?\\s+)?)?(martin luther king|todos os santos|s[ãa]o (patr[íi]cio|francisco|jorge|jo[ãa]o)|independ[êe]ncia))(\\s+(d[eo]?\\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|[nd]?es[st][ea]|[uú]ltim[oa]?|em))\\s+ano))?\\b'
    HolidayRegex3 = f'\\b(?<holiday>(dia\\s+d[eoa]s?\\s+)(trabalh(o|ador(es)?)|m[ãa]es?|pais?|mulher(es)?|crian[çc]as?|marmota|professor(es)?))(\\s+(d[eo]?\\s+)?({YearRegex}|(?<order>(pr[oó]xim[oa]?|[nd]?es[st][ea]|[uú]ltim[oa]?|em))\\s+ano))?\\b'
    BeforeRegex = f'(antes(\\s+(d[aeo]s?)?)?|at[ée]h?(\\s+[oàa]s?\\b)?)'
    AfterRegex = f'((depois|ap[óo]s|a\\s+partir)(\\s*(de|d?[oa]s?)?)?)'
    SinceRegex = f'(desde(\\s+(as?|o))?)'
    AroundRegex = f'(?:\\b(?:cerca|perto|ao\\s+redor|por\\s+volta)\\s*?\\b)(\\s+(de|das))?'
    PeriodicRegex = f'\\b(?<periodic>di[áa]ri[ao]|(diaria|mensal|semanal|quinzenal|(bi|tri|se)mestral|anual)(mente)?)\\b'
    EachExpression = f'cada|tod[oa]s?\\s*([oa]s)?'
    EachUnitRegex = f'(?<each>({EachExpression})\\s*{UnitRegex})'
    EachPrefixRegex = f'(?<each>({EachExpression})\\s*$)'
    EachDayRegex = f'\\s*({EachExpression})\\s*dias\\s*\\b'
    BeforeEachDayRegex = f'({EachExpression})\\s*dias(\\s+a[so])?\\s*\\b'
    SetEachRegex = f'(?<each>({EachExpression})\\s*)'
    LaterEarlyPeriodRegex = f'^[.]'
    WeekWithWeekDayRangeRegex = f'^[.]'
    GeneralEndingRegex = f'^[.]'
    MiddlePauseRegex = f'^[.]'
    PrefixArticleRegex = f'^[\\.]'
    OrRegex = f'^[.]'
    SpecialYearTermsRegex = f'\\b(({SpecialYearPrefixes}\\s+anos?\\s+|anos?\\s+({SpecialYearPrefixes}\\s+)?)(d[oe]\\s+)?)'
    YearPlusNumberRegex = f'\\b({SpecialYearTermsRegex}((?<year>(\\d{{2,4}}))|{FullTextYearRegex}))\\b'
    NumberAsTimeRegex = f'\\b({WrittenTimeRegex}|({TimeHourNumRegex}|{BaseDateTime.HourRegex})(?<desc>\\s*horas)?)\\b'
    TimeBeforeAfterRegex = f'^[.]'
    DateNumberConnectorRegex = f'^[.]'
    ComplexDatePeriodRegex = f'^[.]'
    AgoRegex = f'\\b(antes(\\s+d[eoa]s?\\s+(?<day>hoje|ontem|manhã))?|atr[áa]s|no passado)\\b'
    LaterRegex = f'\\b(depois(\\s+d[eoa]s?\\s+(agora|(?<day>hoje|ontem|manhã)))?|ap[óo]s (as)?|desde( (as|o))?|no futuro|mais tarde)\\b'
    Tomorrow = 'amanh[ãa]'
    UnitMap = dict([("anos", "Y"),
                    ("ano", "Y"),
                    ("meses", "MON"),
                    ("mes", "MON"),
                    ("mês", "MON"),
                    ("semanas", "W"),
                    ("semana", "W"),
                    ("dias", "D"),
                    ("dia", "D"),
                    ("horas", "H"),
                    ("hora", "H"),
                    ("hrs", "H"),
                    ("hr", "H"),
                    ("h", "H"),
                    ("minutos", "M"),
                    ("minuto", "M"),
                    ("mins", "M"),
                    ("min", "M"),
                    ("segundos", "S"),
                    ("segundo", "S"),
                    ("segs", "S"),
                    ("seg", "S")])
    UnitValueMap = dict([("anos", 31536000),
                         ("ano", 31536000),
                         ("meses", 2592000),
                         ("mes", 2592000),
                         ("mês", 2592000),
                         ("semanas", 604800),
                         ("semana", 604800),
                         ("dias", 86400),
                         ("dia", 86400),
                         ("horas", 3600),
                         ("hora", 3600),
                         ("hrs", 3600),
                         ("hr", 3600),
                         ("h", 3600),
                         ("minutos", 60),
                         ("minuto", 60),
                         ("mins", 60),
                         ("min", 60),
                         ("segundos", 1),
                         ("segundo", 1),
                         ("segs", 1),
                         ("seg", 1)])
    SpecialYearPrefixesMap = dict([("fiscal", "FY"),
                                   ("escolar", "SY"),
                                   ("letivo", "SY")])
    SeasonMap = dict([("primavera", "SP"),
                      ("verao", "SU"),
                      ("verão", "SU"),
                      ("outono", "FA"),
                      ("inverno", "WI")])
    SeasonValueMap = dict([("SP", 3),
                           ("SU", 6),
                           ("FA", 9),
                           ("WI", 12)])
    CardinalMap = dict([("primeiro", 1),
                        ("primeira", 1),
                        ("1o", 1),
                        ("1a", 1),
                        ("segundo", 2),
                        ("segunda", 2),
                        ("2o", 2),
                        ("2a", 2),
                        ("terceiro", 3),
                        ("terceira", 3),
                        ("3o", 3),
                        ("3a", 3),
                        ("cuarto", 4),
                        ("quarto", 4),
                        ("cuarta", 4),
                        ("quarta", 4),
                        ("4o", 4),
                        ("4a", 4),
                        ("quinto", 5),
                        ("quinta", 5),
                        ("5o", 5),
                        ("5a", 5),
                        ("sexto", 6),
                        ("sexta", 6),
                        ("6o", 6),
                        ("6a", 6),
                        ("setimo", 7),
                        ("sétimo", 7),
                        ("setima", 7),
                        ("sétima", 7),
                        ("7o", 7),
                        ("7a", 7),
                        ("oitavo", 8),
                        ("oitava", 8),
                        ("8o", 8),
                        ("8a", 8),
                        ("nono", 9),
                        ("nona", 9),
                        ("9o", 9),
                        ("9a", 9),
                        ("decimo", 10),
                        ("décimo", 10),
                        ("decima", 10),
                        ("décima", 10),
                        ("10o", 10),
                        ("10a", 10),
                        ("decimo primeiro", 11),
                        ("décimo primeiro", 11),
                        ("decima primeira", 11),
                        ("décima primeira", 11),
                        ("11o", 11),
                        ("11a", 11),
                        ("decimo segundo", 12),
                        ("décimo segundo", 12),
                        ("decima segunda", 12),
                        ("décima segunda", 12),
                        ("12o", 12),
                        ("12a", 12)])
    DayOfWeek = dict([("segunda-feira", 1),
                      ("segundas-feiras", 1),
                      ("segunda feira", 1),
                      ("segundas feiras", 1),
                      ("segunda", 1),
                      ("segundas", 1),
                      ("terça-feira", 2),
                      ("terças-feiras", 2),
                      ("terça feira", 2),
                      ("terças feiras", 2),
                      ("terça", 2),
                      ("terças", 2),
                      ("terca-feira", 2),
                      ("tercas-feiras", 2),
                      ("terca feira", 2),
                      ("tercas feiras", 2),
                      ("terca", 2),
                      ("tercas", 2),
                      ("quarta-feira", 3),
                      ("quartas-feiras", 3),
                      ("quarta feira", 3),
                      ("quartas feiras", 3),
                      ("quarta", 3),
                      ("quartas", 3),
                      ("quinta-feira", 4),
                      ("quintas-feiras", 4),
                      ("quinta feira", 4),
                      ("quintas feiras", 4),
                      ("quinta", 4),
                      ("quintas", 4),
                      ("sexta-feira", 5),
                      ("sextas-feiras", 5),
                      ("sexta feira", 5),
                      ("sextas feiras", 5),
                      ("sexta", 5),
                      ("sextas", 5),
                      ("sabado", 6),
                      ("sabados", 6),
                      ("sábado", 6),
                      ("sábados", 6),
                      ("domingo", 0),
                      ("domingos", 0),
                      ("seg", 1),
                      ("seg.", 1),
                      ("2a", 1),
                      ("ter", 2),
                      ("ter.", 2),
                      ("3a", 2),
                      ("qua", 3),
                      ("qua.", 3),
                      ("4a", 3),
                      ("qui", 4),
                      ("qui.", 4),
                      ("5a", 4),
                      ("sex", 5),
                      ("sex.", 5),
                      ("6a", 5),
                      ("sab", 6),
                      ("sab.", 6),
                      ("dom", 0),
                      ("dom.", 0)])
    MonthOfYear = dict([("janeiro", 1),
                        ("fevereiro", 2),
                        ("março", 3),
                        ("marco", 3),
                        ("abril", 4),
                        ("maio", 5),
                        ("junho", 6),
                        ("julho", 7),
                        ("agosto", 8),
                        ("septembro", 9),
                        ("setembro", 9),
                        ("outubro", 10),
                        ("novembro", 11),
                        ("dezembro", 12),
                        ("jan", 1),
                        ("fev", 2),
                        ("mar", 3),
                        ("abr", 4),
                        ("mai", 5),
                        ("jun", 6),
                        ("jul", 7),
                        ("ago", 8),
                        ("sept", 9),
                        ("set", 9),
                        ("out", 10),
                        ("nov", 11),
                        ("dez", 12),
                        ("1", 1),
                        ("2", 2),
                        ("3", 3),
                        ("4", 4),
                        ("5", 5),
                        ("6", 6),
                        ("7", 7),
                        ("8", 8),
                        ("9", 9),
                        ("10", 10),
                        ("11", 11),
                        ("12", 12),
                        ("01", 1),
                        ("02", 2),
                        ("03", 3),
                        ("04", 4),
                        ("05", 5),
                        ("06", 6),
                        ("07", 7),
                        ("08", 8),
                        ("09", 9)])
    Numbers = dict([("zero", 0),
                    ("um", 1),
                    ("uma", 1),
                    ("dois", 2),
                    ("tres", 3),
                    ("três", 3),
                    ("quatro", 4),
                    ("cinco", 5),
                    ("seis", 6),
                    ("sete", 7),
                    ("oito", 8),
                    ("nove", 9),
                    ("dez", 10),
                    ("onze", 11),
                    ("doze", 12),
                    ("dezena", 12),
                    ("dezenas", 12),
                    ("treze", 13),
                    ("catorze", 14),
                    ("quatorze", 14),
                    ("quinze", 15),
                    ("dezesseis", 16),
                    ("dezasseis", 16),
                    ("dezessete", 17),
                    ("dezassete", 17),
                    ("dezoito", 18),
                    ("dezenove", 19),
                    ("dezanove", 19),
                    ("vinte", 20),
                    ("vinte e um", 21),
                    ("vinte e uma", 21),
                    ("vinte e dois", 22),
                    ("vinte e duas", 22),
                    ("vinte e tres", 23),
                    ("vinte e três", 23),
                    ("vinte e quatro", 24),
                    ("vinte e cinco", 25),
                    ("vinte e seis", 26),
                    ("vinte e sete", 27),
                    ("vinte e oito", 28),
                    ("vinte e nove", 29),
                    ("trinta", 30),
                    ("trinta e um", 31),
                    ("quarenta", 40),
                    ("cinquenta", 50)])
    HolidayNames = dict([("pai", ["diadopai", "diadospais"]),
                         ("mae", ["diadamae", "diadasmaes"]),
                         ("acaodegracas", ["diadegracas", "diadeacaodegracas", "acaodegracas"]),
                         ("trabalho", ["diadotrabalho", "diadotrabalhador", "diadostrabalhadores"]),
                         ("pascoa", ["diadepascoa", "pascoa"]),
                         ("natal", ["natal", "diadenatal"]),
                         ("vesperadenatal", ["vesperadenatal"]),
                         ("anonovo", ["anonovo", "diadeanonovo", "diadoanonovo"]),
                         ("vesperadeanonovo", ["vesperadeanonovo", "vesperadoanonovo"]),
                         ("yuandan", ["yuandan"]),
                         ("todosossantos", ["todosossantos"]),
                         ("professor", ["diadoprofessor", "diadosprofessores"]),
                         ("crianca", ["diadacrianca", "diadascriancas"]),
                         ("mulher", ["diadamulher"])])
    VariableHolidaysTimexDictionary = dict([("pai", "-06-WXX-7-3"),
                                            ("mae", "-05-WXX-7-2"),
                                            ("acaodegracas", "-11-WXX-4-4"),
                                            ("memoria", "-03-WXX-2-4")])
    DoubleNumbers = dict([("metade", 0.5),
                          ("quarto", 0.25)])
    DateTokenPrefix = 'em '
    TimeTokenPrefix = 'as '
    TokenBeforeDate = 'o '
    TokenBeforeTime = 'as '
    PastPrefixRegex = f'.^'
    PreviousPrefixRegex = f'([uú]ltim[oa]s?|passad[oa]s?|{PastPrefixRegex})\\b'
    ThisPrefixRegex = f'([nd]?es[st][ea])\\b'
    RelativeDayRegex = f'^[\\.]'
    RestOfDateRegex = f'^[\\.]'
    DurationUnitRegex = f'(?<unit>{DateUnitRegex}|{TimeUnitRegex}|noites?)\\b'
    RelativeDurationUnitRegex = f'(?:(?<=({NextPrefixRegex}|{PreviousPrefixRegex}|{ThisPrefixRegex})\\s+)({DurationUnitRegex}))'
    ReferenceDatePeriodRegex = f'^[.]'
    FromToRegex = f'\\b(from).+(to)\\b.+'
    SingleAmbiguousMonthRegex = f'^(the\\s+)?(may|march)$'
    UnspecificDatePeriodRegex = f'^[.]'
    PrepositionSuffixRegex = f'\\b(on|in|at|around|from|to)$'
    RestOfDateTimeRegex = f'^[\\.]'
    SetWeekDayRegex = f'^[\\.]'
    NightRegex = f'\\b(meia noite|noite|de noite)\\b'
    CommonDatePrefixRegex = f'\\b(dia)\\s+$'
    DurationConnectorRegex = f'^[.]'
    CenturyRegex = f'^[.]'
    DecadeRegex = f'^[.]'
    DecadeWithCenturyRegex = f'^[.]'
    RelativeDecadeRegex = f'\\b((n?as?\\s+)?{RelativeRegex}\\s+((?<number>[\\w,]+)\\s+)?(d[eé]cada)s?)\\b'
    YearSuffix = f'((,|\\sde)?\\s*({YearRegex}|{FullTextYearRegex}))'
    SuffixAfterRegex = f'^\\b$'
    YearPeriodRegex = f'((((de(sde)?(\\s*a(s)?)?)\\s+)?{YearRegex}\\s*({TillRegex})\\s*{YearRegex})|(((entre\\s*([oa](s)?)?)\\s+){YearRegex}\\s*({RangeConnectorRegex})\\s*{YearRegex}))'
    FutureSuffixRegex = f'\\b(seguinte(s)?|pr[oó]xim[oa](s)?|no\\s+futuro)\\b'
    PastSuffixRegex = f'^\\b$'
    ModPrefixRegex = f'\\b({RelativeRegex}|{AroundRegex}|{BeforeRegex}|{AfterRegex}|{SinceRegex})\\b'
    ModSuffixRegex = f'\\b({AgoRegex}|{LaterRegex}|{BeforeAfterRegex}|{FutureSuffixRegex}|{PastSuffixRegex})\\b'
    WrittenDecades = dict([("", 0)])
    SpecialDecadeCases = dict([("", 0)])
    DefaultLanguageFallback = 'DMY'
    DurationDateRestrictions = []
    AmbiguityFiltersDict = dict([("^\\d{4}$", "(\\d\\.\\d{4}|\\d{4}\\.\\d)"),
                                 ("^(abr|ago|dez|fev|jan|ju[ln]|mar|maio?|nov|out|sep?t)$", "([$%£&!?@#])(abr|ago|dez|fev|jan|ju[ln]|mar|maio?|nov|out|sep?t)|(abr|ago|dez|fev|jan|ju[ln]|mar|maio?|nov|out|sep?t)([$%£&@#])")])
    AmbiguityTimeFiltersDict = dict([("horas?$", "\\b((por|duração\\s+de|durante)\\s+(\\S+\\s+){1,2}horas?|horas?\\s+(\\S+\\s+){0,2}dur(ação|ou|a(rá|va)?))\\b")])
    EarlyMorningTermList = [r'madrugada']
    MorningTermList = [r'manha', r'manhã']
    AfternoonTermList = [r'passado o meio dia', r'depois do meio dia']
    EveningTermList = [r'tarde']
    NightTermList = [r'noite']
    SameDayTerms = [r'hoje', r'este dia', r'esse dia', r'o dia']
    PlusOneDayTerms = [r'amanha', r'de amanha', r'dia seguinte', r'o dia de amanha', r'proximo dia']
    MinusOneDayTerms = [r'ontem', r'ultimo dia']
    PlusTwoDayTerms = [r'depois de amanha', r'dia depois de amanha']
    MinusTwoDayTerms = [r'anteontem', r'dia antes de ontem']
    MonthTerms = [r'mes', r'meses']
    MonthToDateTerms = [r'mes ate agora', r'mes ate hoje', r'mes ate a data']
    WeekendTerms = [r'fim de semana']
    WeekTerms = [r'semana']
    FortnightTerms = [r'quinzena']
    YearTerms = [r'ano', r'anos']
    YearToDateTerms = [r'ano ate agora', r'ano ate hoje', r'ano ate a data', r'anos ate agora', r'anos ate hoje', r'anos ate a data']
    SpecialCharactersEquivalent = dict([("á", "a"),
                                        ("é", "e"),
                                        ("í", "i"),
                                        ("ó", "o"),
                                        ("ú", "u"),
                                        ("ê", "e"),
                                        ("ô", "o"),
                                        ("ü", "u"),
                                        ("ã", "a"),
                                        ("õ", "o"),
                                        ("ç", "c")])
    DayTypeRegex = f'(diari([ao]|amente))$'
    WeekTypeRegex = f'(semanal(mente)?)$'
    BiWeekTypeRegex = f'(quinzenal(mente)?)$'
    MonthTypeRegex = f'(mensal(mente)?)$'
    BiMonthTypeRegex = f'(bimestral(mente)?)$'
    QuarterTypeRegex = f'(trimestral(mente)?)$'
    SemiAnnualTypeRegex = f'(semestral(mente)?)$'
    YearTypeRegex = f'(anual(mente)?)$'
# pylint: enable=line-too-long

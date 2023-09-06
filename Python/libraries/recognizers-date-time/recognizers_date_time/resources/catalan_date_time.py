from .base_date_time import BaseDateTime
# pylint: disable=line-too-long


class CatalanDateTime:
    LangMarker = 'Cat'
    CheckBothBeforeAfter = False
    TillRegex = f'(?<till>\\bfins\\sa|{BaseDateTime.RangeConnectorSymbolRegex})'
    RangeConnectorRegex = f'(?<and>\\b(i)\\b|{BaseDateTime.RangeConnectorSymbolRegex})'
    ThisPrefixRegex = f'(aix[òo]|aquesta)\\b'
    RangePrefixRegex = f'(des de|entre)'
    DayRegex = f'\\b(?<day>01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|1|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)(?:\\.[º°])?(?=\\b|t)'
    MonthNumRegex = f'(?<month>1[0-2]|(0)?[1-9])\\b'
    WrittenDayRegex = f'(?<day>un|dos|tres|quatre|cinc|cinc|sis|set|vuit|nou|nou|deu|onze|dotze|dotze|tretze|catorze|quinze|setze|mor disset|mor divuit|dinnou|vint|vint-i-un|vint-i-dos|vint-i-tres|vint-i-quatre|vint-i-cinc|vint-i-sis|vint-i-set|vint-i-vuit|vint-i-nou|trenta|trenta-un)'
    OclockRegex = f'(?<oclock>en\\s+punt(o)?)'
    AmDescRegex = f'({BaseDateTime.BaseAmDescRegex})'
    PmDescRegex = f'({BaseDateTime.BasePmDescRegex})'
    AmPmDescRegex = f'({BaseDateTime.BaseAmPmDescRegex})'
    DescRegex = f'(?<desc>({AmDescRegex}|{PmDescRegex}))'
    OfPrepositionRegex = f'(\\bde\\b)'
    TwoDigitYearRegex = f'\\b(?<![$])(?<year>([0-9]\\d))(?!(\\s*((\\:\\d)|{AmDescRegex}|{PmDescRegex}|\\.\\d))|\\.?[º°ª])\\b'
    WrittenOneToNineRegex = f'(?:u(n)?|dos|tres|quatre|cinc|sis|set|vuit|nou)'
    TwoToNineIntegerRegex = f'(?:tres|set|vuit|quatre|cinc|nou|dos|sis)'
    WrittenElevenToNineteenRegex = f'(?:onze|dotze|tretze|catorze|quinze|setze|disset|divuit|dinou)'
    WrittenTensRegex = f'(?:setanta|vint|trenta|vuitanta|noranta|quaranta|cinquanta|seixanta)'
    WrittenTwentiesRegex = f'(vint(\\s?-\\s?|\\s)i(\\s?-\\s?|\\s)({WrittenOneToNineRegex}))'
    WrittenOneHundredToNineHundredRegex = f'(({TwoToNineIntegerRegex}(\\s?-\\s?|\\s))?cent(s?))'
    WrittenOneToNinetyNineRegex = f'(({WrittenElevenToNineteenRegex}|{WrittenTwentiesRegex}|({WrittenTensRegex}((\\s?-\\s?|\\s)({WrittenOneToNineRegex}))?)|deu|{WrittenOneToNineRegex}))'
    FullTextYearRegex = f'\\b(?<fullyear>((dos\\s+)?mil)(\\s+{WrittenOneHundredToNineHundredRegex})?(\\s+{WrittenOneToNinetyNineRegex})?)'
    YearRegex = f'({BaseDateTime.FourDigitYearRegex}|{FullTextYearRegex})'
    MonthRegex = f'\\b(?<month>gener|febrer|mar[çc]|abril|maig|juny|juliol|agost|setembre|octubre|novembre|desembre)'
    MonthSuffixRegex = f'(?<msuf>((a|de)\\s+)?({MonthRegex}))'
    SimpleCasesRegex = f'\\b({RangePrefixRegex}\\s+)?({DayRegex})\\s*{TillRegex}\\s*({DayRegex})\\s+{MonthSuffixRegex}((\\s+|\\s*,\\s*){YearRegex})?\\b'
    MonthFrontSimpleCasesRegex = f'\\b{MonthSuffixRegex}\\s+({RangePrefixRegex}?({DayRegex})\\s*{TillRegex}\\s*({DayRegex})((\\s+|\\s*,\\s*){YearRegex})?\\b'
    MonthFrontBetweenRegex = f'\\b{MonthSuffixRegex}\\s+({RangePrefixRegex}({DayRegex})\\s*{RangeConnectorRegex}\\s*({DayRegex})((\\s+|\\s*,\\s*){YearRegex})?\\b'
    MonthNumWithYearRegex = f'\\b(({YearRegex}(\\s*?)[/\\-\\.~](\\s*?){MonthNumRegex})|({MonthNumRegex}(\\s*?)[/\\-\\.~](\\s*?){YearRegex}))\\b'
    OfYearRegex = f'\\b((de?)\\s+({YearRegex}\\s+any))\\b'
    CenturySuffixRegex = f'(^segle)\\b'
    RangeUnitRegex = f'\\b(?<unit>anys?|mesos?|setmanes?)\\b'
    BeforeAfterRegex = f'^[.]'
    InConnectorRegex = f'\\b(en)(?=\\s*$)\\b'
    TodayNowRegex = f'\\b(avui|ara)\\b'
    FromRegex = f'((\\bdes?)(\\s*del)?)$'
    BetweenRegex = f'(\\bentre\\s*)'
    WeekDayRegex = f'\\b(?<weekday>(dilluns|dimarts|dimecres|dijous|divendres|dissabte|diumenge))\\b'
    OnRegex = f'\\b(a\\s+)({DayRegex}s?)(?![.,]\\d)\\b'
    RelaxedOnRegex = f'(?<=\\b(a)\\s+)((?<day>10|11|12|13|14|15|16|17|18|19|1st|20|21|22|23|24|25|26|27|28|29|2|30|31|3|4|5|6|7|8|9)s?)(?![.,]\\d)\\b'
    SpecialDayRegex = f'\\b(avui|dem[àa]|ahir)\\b'
    SpecialDayWithNumRegex = f'^[.]'
    FlexibleDayRegex = f'(?<DayOfMonth>([a-z]+\\s)?({WrittenDayRegex}|{DayRegex}))'
    WeekDayAndDayOfMonthRegex = f'\\b{WeekDayRegex}\\s+((el\\s+(d[ií]a\\s+)?){FlexibleDayRegex})\\b'
    WeekDayAndDayRegex = f'\\b{WeekDayRegex}\\s+({DayRegex}|{WrittenDayRegex})(?!([-:/]|\\.\\d|(\\s+({AmDescRegex}|{PmDescRegex}|{OclockRegex}))))\\b'
    RelativeWeekDayRegex = f'^[.]'
    AmbiguousRangeModifierPrefix = f'^[.]'
    NumberEndingPattern = f'^[.]'
    DateTokenPrefix = 'a '
    TimeTokenPrefix = 'a les '
    TokenBeforeDate = 'el '
    TokenBeforeTime = 'a les '
    HalfTokenRegex = f'^((i\\s+)?mitja(na)?)'
    QuarterTokenRegex = f'^((i\\s+)?quart|(?<neg>menys\\s+quart))'
    PastTokenRegex = f'\\b((passades|passats)(\\s+(de\\s+)?les)?)$'
    ToTokenRegex = f'\\b((per|abans)(\\s+(de\\s+)?les?)|(?<neg>^menys))$'
    SpecialDateRegex = f'(?<=\\b(a)\\s+el\\s+){DayRegex}\\b'
    OfMonthRegex = f'^\\s*((d[ií]a\\s+)?d[e\']\\s?)?{MonthSuffixRegex}'
    MonthEndRegex = f'({MonthRegex}\\s*(el)?\\s*$)'
    WeekDayEnd = f'{WeekDayRegex}\\s*,?\\s*$'
    WeekDayStart = f'^\\b$'
    DateYearRegex = f'(?<year>{YearRegex}|(?<!,\\s?){TwoDigitYearRegex}|{TwoDigitYearRegex}(?=(\\.(?!\\d)|[?!;]|$)))'
    DateExtractor1 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?(?<!\\d[.,]){DayRegex}((\\s*(d[e\'])|[/\\\\\\.\\-])\\s*)?{MonthRegex}\\b'
    DateExtractor2 = f'\\b((el\\s+d[ií]a|{WeekDayRegex})(\\s+|\\s*,\\s*))?(?<!\\d[.,])(({DayRegex}((\\s+(d[e\']\\s?)?|\\s*[.,/-]\\s*){MonthRegex}((\\s+(del?\\s+)?|\\s*[.,/-]\\s*){DateYearRegex}\\b)?|\\s+(d[e\']\\s?){MonthNumRegex}\\s+(del?\\s+{DateYearRegex}\\b)))|{BaseDateTime.FourDigitYearRegex}\\s*[.,/-]?\\s*(el\\s+d[ií]a\\s+)?{DayRegex}(\\s+(d[e\']\\s+)?|\\s*[.,/-]\\s*){MonthRegex})'
    DateExtractor3 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*))?{MonthRegex}(\\s*[.,/-]?\\s*)(el\\s+d[ií]a\\s+)?{DayRegex}(?!\\s*\\-\\s*\\d{{2}}\\b)((\\s+(del?\\s+)?|\\s*[.,/-]\\s*){DateYearRegex})?\\b'
    DateExtractor4 = f'\\b(?<!\\d[.,]){MonthNumRegex}\\s*[/\\\\\\-]\\s*{DayRegex}\\s*[/\\\\\\-]\\s*{DateYearRegex}(?!\\s*[/\\\\\\-\\.]\\s*\\d+)'
    DateExtractor5 = f'\\b(?<!\\d[.,]){DayRegex}\\s*[/\\\\\\-\\.]\\s*({MonthNumRegex}|{MonthRegex})\\s*[/\\\\\\-\\.]\\s*{DateYearRegex}(?!\\s*[/\\\\\\.]\\s*\\d+)'
    DateExtractor6 = f'(?<=\\b(en|el)\\s+){MonthNumRegex}[\\-\\.]{DayRegex}{BaseDateTime.CheckDecimalRegex}\\b(?!\\s*[/\\\\\\.]\\s*\\d+)'
    DateExtractor7 = f'\\b(?<!\\d[.,]){MonthNumRegex}\\s*/\\s*{DayRegex}((\\s+|\\s*,\\s*|\\s+d[e\']\\s?){DateYearRegex})?\\b{BaseDateTime.CheckDecimalRegex}(?!\\s*[/\\\\\\.]\\s*\\d+)'
    DateExtractor8 = f'(?<=\\b(en|el)\\s+){DayRegex}[\\\\\\-]{MonthNumRegex}{BaseDateTime.CheckDecimalRegex}\\b(?!\\s*[/\\\\\\.]\\s*\\d+)'
    DateExtractor9 = f'\\b({WeekDayRegex}\\s+)?(?<!\\d[.,]){DayRegex}\\s*(/|\\bde(l?)\\b)\\s*{MonthNumRegex}((\\s+|\\s*,\\s*|\\s+d[e\']\\s?){DateYearRegex})?\\b{BaseDateTime.CheckDecimalRegex}(?!\\s*[/\\\\\\.]\\s*\\d+)'
    DateExtractor10 = f'\\b(?<!\\d[.,])(({YearRegex}\\s*[/\\\\\\-\\.]\\s*({MonthNumRegex}|{MonthRegex})\\s*[/\\\\\\-\\.]\\s*{DayRegex}(?!\\s*[/\\\\\\-\\.]\\s*\\d+))|({MonthRegex}\\s*[/\\\\\\-\\.]\\s*{BaseDateTime.FourDigitYearRegex}\\s*[/\\\\\\-\\.]\\s*{DayRegex})|({DayRegex}\\s*[/\\\\\\-\\.]\\s*{BaseDateTime.FourDigitYearRegex}\\s*[/\\\\\\-\\.]\\s*{MonthRegex}))'

    HourRegex = f'\\b(?<!\\d[,.])(?<hour>2[0-4]|[0-1]?\\d)'
    HourNumRegex = f'\\b(?<hournum>zero|una|dues|tres|quatre|cinc|sis|set|vuit|nou|deu|deu|onze|dotze)\\b'
    MinuteNumRegex = f'(?<minnum>u(n)?|dos|tres|quatre|cinc|sis|set|vuit|nou|deu|onze|dotze|tretze|catorze|quinze|setze|disset|divuit|dinou|vint|trenta|quaranta|cinquanta)'
    DeltaMinuteNumRegex = f'(?<deltaminnum>u(n)?|dos|tres|quatre|cinc|sis|set|vuit|nou|deu|onze|dotze|tretze|catorze|quinze|setze|disset|divuit|dinou|vint|trenta|quaranta|cinquanta)'
    PmRegex = f'(?<pm>((de|a)\\s+la)\\s+(tarda|nit))'
    AmRegex = f'(?<am>(((de|a)\\s+la)|al|del)\\s+(matí|matinada))'
    AmTimeRegex = f'(?<am>(aquesta|(de|a)\\s+la)|(aquest)\\s+(matí|matinada))'
    PmTimeRegex = f'(?<pm>(aquesta|(de|a)\\s+la)\\s+(tarda|nit))'
    NightTimeRegex = f'(nit)'
    LastNightTimeRegex = f'(ahir a la nit)'
    NowTimeRegex = f'(ara|mateix|moment)'
    RecentlyTimeRegex = f'(ment)'
    LessThanOneHour = f'(?<lth>((\\s+i\\s+)?quart|(\\s*)menys quart|(\\s+i\\s+)mitja(na)?|{BaseDateTime.DeltaMinuteRegex}(\\s+(minuts?|mins?))|{DeltaMinuteNumRegex}(\\s+(minuts?|mins?))))'
    TensTimeRegex = f'(?<tens>deu|vint|trenta|quaranta|cinquanta)'
    WrittenTimeRegex = f'(?<writtentime>{HourNumRegex}\\s*((i|(?<prefix>menys))\\s+)?(({TensTimeRegex}(\\s*i\\s+)?)?{MinuteNumRegex}))'
    TimePrefix = f'(?<prefix>{LessThanOneHour}(\\s+(passades)\\s+(de\\s+les|les)?|\\s+(per\\s+a|abans\\s+de)?\\s+(les?))?)'
    TimeSuffix = f'(?<suffix>({LessThanOneHour}\\s+)?({AmRegex}|{PmRegex}|{OclockRegex}))'
    GeneralDescRegex = f'({DescRegex}|(?<suffix>{AmRegex}|{PmRegex}))'
    BasicTime = f'(?<basictime>{WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}:{BaseDateTime.MinuteRegex}(:{BaseDateTime.SecondRegex})?|{BaseDateTime.HourRegex})'
    MidTimeRegex = f'(?<mid>((?<midnight>mitja\\s*nit)|(?<midearlymorning>mitja\\s*matinada)|(?<midmorning>mig\\s*matí)|(?<midafternoon>mitja\\s*tarda)|(?<midday>mig\\s*dia)))'
    AtRegex = f'\\b((?<=\\b((a)\\s+les?|al)\\s+)(({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})\\b?(DescRegex)?|{MidTimeRegex})|{MidTimeRegex})'
    ConnectNumRegex = f'({BaseDateTime.HourRegex}(?<min>[0-5][0-9])\\s*{DescRegex})'
    TimeRegexWithDotConnector = f'({BaseDateTime.HourRegex}\\.{BaseDateTime.MinuteRegex})'
    TimeRegex1 = f'(\\b{TimePrefix}\\s+)?({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex})\\s*({DescRegex})'
    TimeRegex2 = f'(\\b{TimePrefix}\\s+)?(t)?{BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex}((\\s*)?:(\\s*)?{BaseDateTime.SecondRegex})?(\\s*({DescRegex})|\\b)'
    TimeRegex3 = f'\\b(({TimePrefix}\\s+)?{TimeRegexWithDotConnector}(\\s*({DescRegex}|{TimeSuffix}))|((les\\s+{TimeRegexWithDotConnector})(?!\\s*(per\\s+cent?|%))(\\s*({DescRegex}|{TimeSuffix})|\\b)))'
    TimeRegex4 = f'\\b(({DescRegex}?)|({BasicTime}\\s*)?({GeneralDescRegex}?)){TimePrefix}(\\s*({HourNumRegex}|{BaseDateTime.HourRegex}))?(\\s+{TensTimeRegex}(\\s*(i\\s+)?{MinuteNumRegex})?)?(\\s*({OclockRegex}|{DescRegex})|\\b)'
    TimeRegex5 = f'\\b({TimePrefix}|{BasicTime}{TimePrefix})\\s+(\\s*{DescRegex})?{BasicTime}?\\s*{TimeSuffix}\\b'
    TimeRegex6 = f'({BasicTime}(\\s*{DescRegex})?\\s+{TimeSuffix}\\b)'
    TimeRegex7 = f'\\b{TimeSuffix}\\s+a\\s+les\\s+{BasicTime}((\\s*{DescRegex})|\\b)'
    TimeRegex8 = f'\\b{TimeSuffix}\\s+{BasicTime}((\\s*{DescRegex})|\\b)'
    TimeRegex9 = f'\\b(?<writtentime>{HourNumRegex}\\s+({TensTimeRegex}\\s*)(i\\s+)?{MinuteNumRegex}?)\\b'
    TimeRegex11 = f'\\b({WrittenTimeRegex})(\\s+{DescRegex})?\\b'
    TimeRegex12 = f'(\\b{TimePrefix}\\s+)?{BaseDateTime.HourRegex}(\\s*){BaseDateTime.MinuteRegex}(\\s*{DescRegex})?'
    DayOfWeek = dict([("dilluns", 1),
                      ("dimarts", 2),
                      ("dimecres", 3),
                      ("dijous", 4),
                      ("divendres", 5),
                      ("dissabte", 6),
                      ("diumenge", 0),
                      # ("dom", 0),
                      # ("lun", 1),
                      # ("mar", 2),
                      # ("mie", 3),
                      # ("mié", 3),
                      # ("jue", 4),
                      # ("vie", 5),
                      # ("sab", 6),
                      # ("sáb", 6),
                      # ("dom.", 0),
                      # ("lun.", 1),
                      # ("mar.", 2),x
                      # ("mie.", 3),
                      # ("mié.", 3),
                      # ("jue.", 4),
                      # ("vie.", 5),
                      # ("sab.", 6),
                      # ("sáb.", 6),
                      # ("do", 0),
                      # ("lu", 1),
                      # ("ma", 2),
                      # ("mi", 3),
                      # ("ju", 4),
                      # ("vi", 5),
                      ("sa", 6)])
    MonthOfYear = dict([("gener", 1),
                        ("febrer", 2),
                        ("març", 3),
                        ("marc", 3),
                        ("abril", 4),
                        ("maig", 5),
                        ("juny", 6),
                        ("juliol", 7),
                        ("agost", 8),
                        ("setembre", 9),
                        ("octubre", 10),
                        ("novembre", 11),
                        ("desembre", 12),
                        # ("ene", 1),
                        # ("feb", 2),
                        # ("mar", 3),
                        # ("abr", 4),
                        # ("may", 5),
                        # ("jun", 6),
                        # ("jul", 7),
                        # ("ago", 8),
                        # ("sept", 9),
                        # ("sep", 9),
                        # ("set", 9),
                        # ("oct", 10),
                        # ("nov", 11),
                        # ("dic", 12),
                        # ("ene.", 1),
                        # ("feb.", 2),
                        # ("mar.", 3),
                        # ("abr.", 4),
                        # ("may.", 5),
                        # ("jun.", 6),
                        # ("jul.", 7),
                        # ("ago.", 8),
                        # ("sept.", 9),
                        # ("sep.", 9),
                        # ("set.", 9),
                        # ("oct.", 10),
                        # ("nov.", 11),
                        # ("dic.", 12),
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
                    ("un", 1),
                    ("una", 1),
                    ("dos", 2),
                    ("tres", 3),
                    ("trés", 3),
                    ("quatre", 4),
                    ("cinc", 5),
                    ("sis", 6),
                    ("set", 7),
                    ("vuit", 8),
                    ("nou", 9),
                    ("deu", 10),
                    ("onze", 11),
                    ("dotze", 12),
                    ("docena", 12),
                    ("dotzenes", 12),
                    ("tretze", 13),
                    ("catorze", 14),
                    ("quinze", 15),
                    ("setze", 16),
                    ("disset", 17),
                    ("divuit", 18),
                    ("dinou", 19),
                    ("vint", 20),
                    ("veinti", 20),
                    ("ventiuna", 21),
                    ("ventiuno", 21),
                    ("vint-i-un", 21),
                    ("vint-i-una", 21),
                    ("vint-i-dos", 22),
                    ("vint-i-tres", 23),
                    ("vint-i-quatre", 24),
                    ("vint-i-cinc", 25),
                    ("vint-i-sis", 26),
                    ("vint-sis", 26),
                    ("vint-i-set", 27),
                    ("vint-i-vuit", 28),
                    ("vint-i-nou", 29),
                    ("treinta", 30),
                    ("quaranta", 40),
                    ("cinquanta", 50)])
    DefaultLanguageFallback = 'DMY'
# pylint: enable=line-too-long
from .base_date_time import BaseDateTime
# pylint: disable=line-too-long


class ArabicDateTime:
    LangMarker = 'Ara'
    CheckBothBeforeAfter = False
    TillRegex = f'(?<till>\\b(إلى|حتى يوم|حتى|خلال|عبر)\\b|{BaseDateTime.RangeConnectorSymbolRegex})'
    RangeConnectorRegex = f'(?<and>و|خلال|عبر|{BaseDateTime.RangeConnectorSymbolRegex})'
    LastNegPrefix = f'(?<!(w(ill|ould|on\\s*\'\\s*t)|m(ay|ight|ust)|sh(all|ould(n\\s*\'\\s*t)?)|c(an(\\s*\'\\s*t|not)?|ould(n\\s*\'\\s*t)?))(\\s+not)?\\s+)'
    RelativeRegex = f'\\b(?<order>القادم|التالي|الآتي|الحالي|الماضي|المقبل|الحاضر|السابق|الأخير)\\b'
    StrictRelativeRegex = f'\\b(?<order>القادم|التالي|الآتي|هذا|الحالي|الماضي|السابق|الأخير)\\b'
    UpcomingPrefixRegex = f'((هذه\\s+)?(المقبل(ة)?))'
    NextPrefixRegex = f'\\b(بعد|القادم(ة)?|{UpcomingPrefixRegex})\\b'
    AfterNextSuffixRegex = f'\\b(after\\s+(the\\s+)?next)\\b'
    PastPrefixRegex = f'((this\\s+)?past)\\b'
    PreviousPrefixRegex = f'(الماضي(ة)?|السابق(ة)?)\\b'
    ThisPrefixRegex = f'(هذه|الحالي(ة)?)\\b'
    RangePrefixRegex = f'(من|بين)'
    CenturySuffixRegex = f'(^century)\\b'
    ReferencePrefixRegex = f'(ذلك|نفس|هذا)\\b'
    FutureSuffixRegex = f'\\b(الحالي(ة)|القادم(ة)|في المستقبل|التالي(ة)|الآتي(ة)|المقبلين|المقبل(ة))\\b'
    PastSuffixRegex = f'^\\b$'
    DayRegex = f'(?<day>(?:3[0-1]|[1-2]\\d|0?[1-9]))'
    ImplicitDayRegex = f'(the\\s*)?(?<day>(?:3[0-1]|[0-2]?\\d)(?:th|nd|rd|st))\\b'
    MonthNumRegex = f'(?<month>1[0-2]|(0)?[1-9])\\b'
    WrittenOneToNineRegex = f'(?:واحد|اثنان|ثلاثة|أربعة|خمسة|ستة|سبعة|ثمانية|تسعة)'
    WrittenElevenToNineteenRegex = f'(إحدى عشر|إثنى عشر|ثلاثة عشر|أربعة عشر|خمسة عشر|ستة عشر|سبعة عشر|ثمانية عشر|تسعة عشر)'
    WrittenTensRegex = f'(عشر[وي]ن|ثلاث[وي]ن|أربع[وي]ن|خمس[وي]ن|ست[وي]ن|سبع[وي]ن|ثمان[وي]ن|تسع[وي]ن)'
    WrittenNumRegex = f'(?:{WrittenOneToNineRegex}|{WrittenElevenToNineteenRegex}|{WrittenTensRegex}(\\s+{WrittenOneToNineRegex})?)'
    WrittenCenturyFullYearRegex = f'(?<firsttwoyearnum>(واحد|اثنان\\s*)?((,\\s+|،\\s+)?(الألفين|ألفين|ألفان|ألف))(\\s+و)?(\\s*(ثلاث|أربع|خمس|ست|سبع|ثمان|تسع)\\s*(مائة|مئتان)(\\s+و)?)?)(?<lasttwoyearnum>({WrittenElevenToNineteenRegex})|(({WrittenOneToNineRegex})?(\\s+و\\s*)?)({WrittenTensRegex})?)?'
    WrittenCenturyOrdinalYearRegex = f'(?<fullyear>({WrittenElevenToNineteenRegex}|مائة|مائتين)\\s+((و)\\s*)?({WrittenOneToNineRegex})\\s+(و)\\s*{WrittenTensRegex})'
    CenturyRegex = f'\\b(?<century>{WrittenCenturyFullYearRegex}|{WrittenCenturyOrdinalYearRegex}(\\s*مائة)?(\\s*و)?)\\b'
    LastTwoYearNumRegex = f'(?:zero\\s+{WrittenOneToNineRegex}|{WrittenElevenToNineteenRegex}|{WrittenTensRegex}(\\s+{WrittenOneToNineRegex})?)'
    FullTextYearRegex = f'(?<firsttwoyearnum>{CenturyRegex})\\s*(?<lasttwoyearnum>{LastTwoYearNumRegex})|(?<firsttwoyearnum>{WrittenCenturyFullYearRegex})|{WrittenCenturyOrdinalYearRegex}'
    OclockRegex = f'(?<oclock>(ال)?ساعة|(ال)?ساعات)'
    SpecialDescRegex = f'((?<ipm>)p\\b)'
    AmDescRegex = f'(في\\s)?(صباح(ا)?|صباحًا|ص|الصباح|{BaseDateTime.BaseAmDescRegex})'
    PmDescRegex = f'(في\\s)?((ال)?مساء|مساءً|ليلًا|ليلا|(ال)?ليل(ة)?|بعد الظهر|الظهر|ظهرا|{BaseDateTime.BasePmDescRegex})'
    AmPmDescRegex = f'({BaseDateTime.BaseAmPmDescRegex})'
    DescRegex = f'(?:(?:({OclockRegex}\\s+)?(?<desc>({AmPmDescRegex}|{AmDescRegex}|{PmDescRegex}|{SpecialDescRegex})|{OclockRegex})))'
    OfPrepositionRegex = f'(\\bof\\b)'
    TwoDigitYearRegex = f'\\b(?<![$])(?<year>([0-9]\\d))(?!(\\s*((\\:\\d)|{AmDescRegex}|{PmDescRegex}|\\.\\d)))\\b'
    YearRegex = f'(?:{BaseDateTime.FourDigitYearRegex}|{FullTextYearRegex})'
    WeekDayRegex = f'(?<weekday>(?=يوم\\s+)?(الأحد|الإثنين|الاثنين|الثلاثاء|الأربعاء|الخميس|الجمعة|السبت|أحد|إثنين|ثلاثاء|أربعاء|خميس|جمعة|سبت))'
    SingleWeekDayRegex = f'(?<weekday>(?=يوم\\s+)?(الأحد|الإثنين|الاثنين|الثلاثاء|الأربعاء|الخميس|الجمعة|السبت|أحد|إثنين|ثلاثاء|أربعاء|خميس|جمعة|سبت))'
    NextRegex = f'(?<next>الآتي|الأخير|التالي|القادم|من الآن|الحالي|المقبل|الحاضر)'
    RelativeMonthRegex = f'(?<relmonth>(من\\s+)?(هذا\\s+)?(الشهر|شهر)(\\s+)?({NextRegex})?)'
    WrittenMonthRegex = f'(((the\\s+)?month of\\s+)?(?<month>apr(il)?|aug(ust)?|dec(ember)?|feb(ruary)?|jan(uary)?|july?|june?|mar(ch)?|may|nov(ember)?|oct(ober)?|sept(ember)?|sept?))'
    MonthSuffixRegex = f'(?<msuf>(?:(in|of|on)\\s+)?({RelativeMonthRegex}|{WrittenMonthRegex}))'
    DateUnitRegex = f'((?<unit>(((ال)?(يوم(ا)?|أسبوع(ا)?|شهر(ا)?|سنة|عام(ا)?|قرن|حقبة))|نهاية الأسبوع))|(?<plural>((ال)?(يومان|أسبوعان|شهران|سنتان|عامان|قرنان|حقبتان|يومين|أسبوعين|شهرين|سنتين|عامين|قرنين|حقبتين|يومان|أسبوعان|شهران|سنتان|عامان|قرنان|حقبتان|أيام|أسابيع|أشهر|سنوات|أعوام|حقبات|قرون|سنين|شهور)))|((?<=\\s+\\d{{1,4}})[ymwd]))\\b'
    DateTokenPrefix = 'في '
    TimeTokenPrefix = 'عند '
    TokenBeforeDate = 'في '
    TokenBeforeTime = 'عند '
    HalfTokenRegex = f'^(النصف|نصف|والنصف|ونصف)'
    QuarterTokenRegex = f'^(ربع|الربع|وربع|والربع|إلا ربع|إلا الربع)'
    ThreeQuarterTokenRegex = f'^(وثلاثة أرباع|ثلاثة أرباع|إلا الربع)'
    ToTokenRegex = f'\\b(إلا)'
    ToHalfTokenRegex = f'\\b(إلا\\s+(النصف|نصف))$'
    ForHalfTokenRegex = f'\\b(ل(s+)?(نصف))$'
    FromRegex = f'\\b(from(\\s+the)?)$'
    BetweenTokenRegex = f'\\b(between(\\s+the)?)$'
    OrdinalNumberRegex = f'((ال)?حادي عشر|ل(ال)?ثاني عشر|(ال)?ثالث عشر|(ال)?رابع عشر|(ال)?خامس عشر|(ال)?خمسة عشر|(ال)?سادس عشر|(ال)?سابع عشر|(ال)?ثامن عشر|(ال)?تاسع عشر|(ال)?عشرون|(ال)?عشرين|(ال)?حادي والعشرون|(ال)?حادية والعشرين|(ال)?حادي والعشرين|(ال)?ثاني والعشرون|(ال)?ثانية والعشرين|(ال)?ثالث والعشرون|(ال)?رابع والعشرون|(ال)?خامس والعشرون|(ال)?سادس والعشرون|(ال)?تاسع والعشرون|(ال)?سابع والعشرون|(ال)?رابع والعشرون|الثامن|الأول|الثالث|الرابع|الخامس|السادس|الثاني|العاشر|السابع)'
    SolarMonthRegex = f'(?<month>يناير|فبراير|مارس|أبريل|مايو|يونيو|يوليو|أغسطس|سبتمبر|أكتوبر|نوفمبر|ديسمبر)'
    LunarMonthRegex = f'(?<month>محرم|صفر|ربيع الأول|ربيع الثاني|جمادى الأول|جمادى الثاني|رجب|شعبان|رمضان|شوال|ذو القعدة|ذو الحجة)'
    ArabicMonthRegex = f'(?<month>كانون الثاني|شباط|آذار|نيسان|حزيران|تموز|آب|أيلول|تشرين الأول|تشرين الثاني|كانون الأول|أيار)'
    SimpleCasePreMonthRegex = f'((بين|من)\\s+)(({DayRegex}-{DayRegex})\\s+)((من|في)\\s+)?((الشهر|{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})\\s+)({RelativeRegex})?({YearRegex})?'
    SimpleCasesRegex = f'(((من)\\s+)?(({DayRegex}|{OrdinalNumberRegex})\\s+)((الشهر|{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})\\s+)?((حتى|إلى)\\s*)(({DayRegex}|{OrdinalNumberRegex})\\s+)((من هذا|من|هذا|في)\\s+)?(الشهر|{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})?(\\s+({RelativeRegex}))?(\\s+{YearRegex})?)|({SimpleCasePreMonthRegex})'
    MonthFrontSimpleCasesRegex = f'(((شهر\\s+)?{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})\\s+(بين|من)\\s+({DayRegex}|{OrdinalNumberRegex})\\s+[و]\\s*({DayRegex}|{OrdinalNumberRegex}))|({DayRegex}\\s*[-\\./]\\s*{DayRegex}\\s+{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})'
    MonthFrontBetweenRegex = f'\\b{MonthSuffixRegex}\\s+(between\\s+)({DayRegex})\\s*{RangeConnectorRegex}\\s*({DayRegex})((\\s+|\\s*,\\s*){YearRegex})?\\b'
    BetweenRegex = f'((بين|من)\\s+)(({DayRegex}|{OrdinalNumberRegex})\\s*)((الشهر|{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})\\s+)?((حتى|إلى|و|-)\\s*)(({DayRegex}|{OrdinalNumberRegex})\\s+)((من هذا|من|هذا|في)\\s+)?(الشهر|{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})?(\\s*([,،-])\\s*)?(\\s*({RelativeRegex}))?(\\s+{YearRegex})?'
    MonthWithYear = f'((هذا\\s+)?(شهر\\s+)?({SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})[\\.]?(\\s*)[/\\\\\\-\\.,]?(\\s*(من عام|من|في|عام))?(\\s*)({YearRegex}))|(({SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})\\s+(هذا\\s+)?((عام|العام)\\s+)?({RelativeRegex})?)'
    SpecialYearPrefixes = f'(التقويمي(ة)?|(?<special>المالي(ة)?|الدراسي(ة)?))'
    ArabicWeekRegex = f'(?<ArabicWeek>الأسبوعين|الاسبوعين|أسابيع|الاسبوع|الأسبوع|الإسبوع|أسبوعين|أسبوعي|اسبوعين|اسبوعي|أسبوع|الاسابيع|الأسابيع)'
    OneWordPeriodRegex = f'((بعد|في|آخر)\\s+(\\d+\\s+)?((ال)?سنوات|(ال)?أعوام|(ال)?سنين|(ال)?أسابيع|(ال)?أشهر|(ال)?أيام))(\\s+\\d+)?(\\s+{FutureSuffixRegex})?|((هذا\\s+)?(شهر\\s+)?(الشهر|{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})(\\s+{RelativeRegex})?)|((هذا\\s+)?((عطلة|خلال)\\s+)?((نهاية\\s+)?({ArabicWeekRegex}|العام)\\s*)((بعد\\s+)?{RelativeRegex})?)'
    MonthNumWithYear = f'\\b(({BaseDateTime.FourDigitYearRegex}(\\s*)[/\\-\\.](\\s*){MonthNumRegex})|({MonthNumRegex}(\\s*)[/\\-](\\s*){BaseDateTime.FourDigitYearRegex}))\\b'
    WeekOfMonthRegex = f'(\\b(?<wom>(الأسبوع)\\s+((?<cardinal>الأول|الثاني|الثالث|الرابع|الخامس|الأخير)\\s+)((من هذا|هذا|من)\\s+)?(شهر\\s+)?(الشهر|{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})[,]?(\\s+{YearRegex})?)\\b)|(((الأسبوع|أسبوع)\\s+)(في\\s+)?{DayRegex}\\s+({SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex}))'
    WeekOfYearRegex = f'(?<woy>(الأسبوع)\\s+(?<cardinal>الأول|الثاني|الثالث|الرابع|الخامس|الأخير)\\s+((من هذا|هذا|من)\\s+)?(العام|من عام|عام)\\s*({YearRegex}|{RelativeRegex})?)'
    OfYearRegex = f'\\b((of|in)\\s+({YearRegex}|{StrictRelativeRegex}\\s+year))\\b'
    FirstLastRegex = f'\\b(the\\s+)?((?<first>first)|(?<last>last))\\b'
    FollowedDateUnit = f'^\\s*{DateUnitRegex}'
    NumberCombinedWithDateUnit = f'\\b(?<num>\\d+(\\.\\d*)?)(\\s)?(-)?{DateUnitRegex}'
    QuarterTermRegex = f'(الربع[- ]+(?<cardinal>الأول|الثاني|الثالث|الرابع))'
    RelativeQuarterTermRegex = f'\\b(الربع)\\s+(?<orderQuarter>{StrictRelativeRegex})\\b'
    QuarterRegex = f'({YearRegex}\\s+)?({QuarterTermRegex})(((\\s+(من عام|من))?\\s+({YearRegex}))|(\\s+(هذا|من هذا|)\\s+العام))?'
    QuarterRegexYearFront = f'(?:{YearRegex}|{RelativeRegex}\\s+year)(\'s)?(?:\\s*-\\s*|\\s+(the\\s+)?)?{QuarterTermRegex}'
    HalfYearTermRegex = f'(?<cardinal>first|1st|second|2nd)\\s+half'
    HalfYearFrontRegex = f'(?<year>((1[5-9]|20)\\d{{2}})|2100)(\\s*-\\s*|\\s+(the\\s+)?)?h(?<number>[1-2])'
    HalfYearBackRegex = f'(the\\s+)?(h(?<number>[1-2])|({HalfYearTermRegex}))(\\s+of|\\s*,\\s*)?\\s+({YearRegex})'
    HalfYearRelativeRegex = f'(the\\s+)?{HalfYearTermRegex}(\\s+of|\\s*,\\s*)?\\s+({RelativeRegex}\\s+year)'
    AllHalfYearRegex = f'({HalfYearFrontRegex})|({HalfYearBackRegex})|({HalfYearRelativeRegex})'
    EarlyPrefixRegex = f'\\b(?<EarlyPrefix>بداية|مطلع|وقت مبكر|(?<RelEarly>قبل))\\b'
    MidPrefixRegex = f'\\b(?<MidPrefix>في منتصف|منتصف)\\b'
    LaterPrefixRegex = f'\\b(?<LatePrefix>نهاية|باقي|بقية|أواخر|(?<RelLate>في وقت لاحق|لاحقا في|بعد))\\b'
    PrefixPeriodRegex = f'({EarlyPrefixRegex}|{MidPrefixRegex}|{LaterPrefixRegex}|{RelativeRegex})'
    PrefixDayRegex = f'\\b((?<EarlyPrefix>early)|(?<MidPrefix>mid(dle)?)|(?<LatePrefix>later?))(\\s+in)?(\\s+the\\s+day)?$'
    SeasonDescRegex = f'(?<seas>(ال)?ربيع|(ال)?صيف|(ال)?خريف|(ال)?شتاء)'
    SeasonRegex = f'\\b(?<season>(هذا\\s+)?(منتصف\\s+)?(({SeasonDescRegex})(\\s+{PrefixPeriodRegex})?(\\s*عام\\s*)?(\\s*{YearRegex})?))\\b'
    WhichWeekRegex = f'\\b(week)(\\s*)(?<number>5[0-3]|[1-4]\\d|0?[1-9])\\b'
    WeekOfRegex = f'(the\\s+)?((week)(\\s+(of|(commencing|starting|beginning)(\\s+on)?))|w/c)(\\s+the)?'
    MonthOfRegex = f'(من)(\\s*)(شهر)'
    MonthRegex = f'(?<month>{SolarMonthRegex}|{LunarMonthRegex}|{ArabicMonthRegex})'
    DateYearRegex = f'(?<year>{BaseDateTime.FourDigitYearRegex}|(?<!,\\s?){TwoDigitYearRegex}|{TwoDigitYearRegex}(?=(\\.(?!\\d)|[?!;]|$)))'
    YearSuffix = f'((\\s*،\\s*|,|\\sمن)?\\s*({DateYearRegex}|{FullTextYearRegex}))'
    OnRegex = f'(?<=\\bفي\\s+){DayRegex}\\b'
    OrdinalDayOfMonthRegex = f'(?=يوم\\s+)?(الأحد|الإثنين|الاثنين|الثلاثاء|الأربعاء|الخميس|الجمعة|السبت)\\s+(في\\s+)((?:3[0-1]|[1-2]\\d|0?[1-9])|((ال)?حادي عشر|ل(ال)?ثاني عشر|(ال)?ثالث عشر|(ال)?رابع عشر|(ال)?خامس عشر|(ال)?خمسة عشر|(ال)?سادس عشر|(ال)?سابع عشر|(ال)?ثامن عشر|(ال)?تاسع عشر|(ال)?عشرون|(ال)?عشرين|(ال)?حادي والعشرون|(ال)?حادية والعشرين|(ال)?حادي والعشرين|(ال)?ثاني والعشرون|(ال)?ثانية والعشرين|(ال)?ثالث والعشرون|(ال)?رابع والعشرون|(ال)?خامس والعشرون|(ال)?سادس والعشرون|(ال)?تاسع والعشرون|(ال)?سابع والعشرون|(ال)?رابع والعشرون|الثامن|الأول|الثالث|الرابع|الخامس|السادس|الثاني|العاشر|السابع))'
    WeekDayofMonthRegex = f'(?=يوم\\s+)?(الأحد|الإثنين|الاثنين|الثلاثاء|الأربعاء|الخميس|الجمعة|السبت)\\s+(في\\s+)?((?:3[0-1]|[1-2]\\d|0?[1-9])|(الأول|الثاني|الثالث|الرابع|الخامس))'
    RelaxedOnRegex = f'({OrdinalDayOfMonthRegex}|{WeekDayofMonthRegex})'
    PrefixWeekDayRegex = f'(\\s*((,?\\s*on)|[-—–]))'
    ThisRegex = f'(?=يوم\\s+)?({WeekDayRegex})(\\s+)?(من|هذا|)(\\s+)?(هذا)?(\\s+)({ArabicWeekRegex})((\\s+)({RelativeRegex}))?'
    LastDayDateRegex = f'(?=يوم\\s+)?({WeekDayRegex})\\s+(الماضي|السابق|الأخير)'
    LastWeekDateRegex = f'({ArabicWeekRegex})\\s+(الماضي|السابق|الأخير)\\s+({WeekDayRegex})'
    LastMonthYearDateRegex = f'(قبل\\s+)(\\d+ )?((بضعة|بضع|عدة)\\s+)?(سنتين|شهرين|الشهور|أشهر|اشهر|شهر|الشهر|أيام|عامين|عام|أعوام|سنة|سنين|سنوات)'
    SpecificDayRegex = f'((قبل|بعد)\\s+)?((اليوم|يوم)\\s+)?(((?<=ب)الأمس|أمس|الأمس|البارحة)|(آخر يوم|الماضي|السابق|الأخير|يومين)|({DayRegex}\\s+{MonthRegex}))'
    LastDateRegex = f'({LastDayDateRegex}|{LastWeekDateRegex})'
    NextDayRegex = f'(هذا يوم\\s+|بعد\\s+)?(?=(ال)?يوم\\s+)?({WeekDayRegex})((\\s+)({NextRegex}))?'
    NextWeekDayRegex = f'((بعد )|(في هذا ?=)|(هذا ?=))?((ال|لل|ل)?أسبوع(ين)?|{ArabicWeekRegex}|اليوم|يومي|الغد|غداً|غد|غدا)(يوم)?({ArabicWeekRegex})?(\\s*(الآتي|الأخير|التالي|القادم|من الآن|الحالي|المقبل|الحاضر))?(\\s*{ArabicWeekRegex})?'
    NextWeekRegex = f'(?=بعد )?(هذا )?({ArabicWeekRegex})\\s*({NextRegex})?\\s+?(يوم)?(\\s+)?({WeekDayRegex})?'
    NextDateRegex = f'({NextWeekRegex}|{NextDayRegex})'
    CardinalDayOfMonthRegex = f'(((?<=في )|(إلى |لل|يوم ))((((ال)?عاشر|(ال)?حادي(ة)? والعشرين|(ال)?ثاني(ة)? والعشرين|(ال)?ثالث(ة)? والعشرين|(ال)?رابع(ة)? والعشرين|(ال)?خامس(ة)? والعشرين|(ال)?سادس(ة)? والعشرين|(ال)?سابع(ة)? والعشرين|(ال)?ثامن(ة)? والعشرين|(ال)?تاسع(ة)? والعشرين|(ال)?ثلاثين|(ال)?حادي(ة)? والثلاثين|(ال)?أول|(ال)?ثاني|(ال)?ثالث|(ال)?رابع|(ال)?خامس|(ال)?سادس|(ال)?سابع|(ال)?ثامن|(ال)?تاسع))|({DayRegex})))|((?<=يوم )({DayRegex})[\\./-]\\s+({MonthRegex}))'
    SpecialDayRegex = f'({NextWeekDayRegex}|{CardinalDayOfMonthRegex}|{SpecificDayRegex}|{LastMonthYearDateRegex})'
    SpecialDayWithNumRegex = f'\\b((?<number>{WrittenNumRegex})\\s+days?\\s+from\\s+(?<day>yesterday|tomorrow|tmr|today))\\b'
    RelativeDayRegex = f'\\b(((the\\s+)?{RelativeRegex}\\s+day))\\b'
    SetWeekDayRegex = f'\\b(?<prefix>on\\s+)?(?<weekday>morning|afternoon|evening|night|(sun|mon|tues|wednes|thurs|fri|satur)day)s\\b'
    WeekDayOfMonthRegex = f'(?<wom>(the\\s+)?(?<cardinal>first|1st|second|2nd|third|3rd|fourth|4th|fifth|5th|last)\\s+(week\\s+{MonthSuffixRegex}[\\.]?\\s+(on\\s+)?{WeekDayRegex}|{WeekDayRegex}\\s+{MonthSuffixRegex}))'
    RelativeWeekDayRegex = f'\\b({WrittenNumRegex}\\s+{WeekDayRegex}\\s+(from\\s+now|later))\\b'
    SpecialDate = f'(?=\\b(on|at)\\s+the\\s+){DayRegex}\\b'
    DatePreposition = f'\\b(في|عند|من)'
    DateExtractorYearTermRegex = f'(\\s+|\\s*,\\s*|\\s+من\\s+){DateYearRegex}'
    CardinalDayRegex = f'(?=يوم\\s+)?((ال|لل|ل)?عاشر|(ال|لل|ل)?حادي(ة)? و(ال)?عشر[يو]ن|واحد و(ال)?عشر[يو]ن|(ال|لل|ل)?ثاني(ة)? و(ال)?عشر[يو]ن|(ال|لل|ل)?ثالث(ة)? و(ال)?عشر[يو]ن|(ال|لل|ل)?رابع(ة)? و(ال)?عشر[يو]ن|(ال|لل|ل)?خامس(ة)? و(ال)?عشر[يو]ن|(ال|لل|ل)?سادس(ة)? و(ال)?عشر[يو]ن|(ال|لل|ل)?سابع(ة)? و(ال)?عشر[يو]ن|(ال|لل|ل)?ثامن(ة)? و(ال)?عشر[يو]ن|(ال|لل|ل)?تاسع(ة)? و(ال)?عشر[يو]ن|(ال|لل|ل)?ثلاثين|(ال|لل|ل)?حادي(ة)? والثلاثين|(ال|لل|ل)?أول|(ال|لل|ل)?ثاني|(ال|لل|ل)?ثالث|(ال|لل|ل)?رابع|(ال|لل|ل)?خامس|(ال|لل|ل)?سادس|(ال|لل|ل)?سابع|(ال|لل|ل)?ثامن|(ال|لل|ل)?تاسع)'
    DateExtractor1 = f'({CardinalDayRegex})(\\s+يوم\\s+)({WeekDayRegex})(\\s+)(في|من)(\\s+)(هذا|هذه)?(\\s+)?(الشهر|{MonthRegex})({DateExtractorYearTermRegex}\\b)?'
    DateExtractor3 = f'\\b({WeekDayRegex}(\\s+|\\s*,\\s*|\\s*،\\s*))?({DayRegex}|{CardinalDayRegex})[\\.]?(\\s+|\\s*,\\s*|\\s+من\\s+|\\s*-\\s*)?{MonthRegex}[\\.]?((\\s+(في|عند|عام|سنة|من عام|من سنة))?{DateExtractorYearTermRegex})?\\b'
    DateExtractor4 = f'\\b{MonthNumRegex}\\s*[/\\\\\\-]\\s*{DayRegex}[\\.]?\\s*[/\\\\\\-]\\s*{DateYearRegex}'
    DateExtractor5 = f'\\b{DayRegex}\\s*[/\\\\\\-\\.]\\s*({MonthNumRegex}|{MonthRegex})\\s*[/\\\\\\-\\.]\\s*{DateYearRegex}(?!\\s*[/\\\\\\-\\.]\\s*\\d+)'
    DateExtractor6 = f'(?<={DatePreposition}\\s+)({StrictRelativeRegex}\\s+)?({WeekDayRegex}\\s+)?{MonthNumRegex}[\\-\\.]{DayRegex}(?![%])\\b'
    DateExtractor7L = f'\\b({WeekDayRegex}\\s+)?{MonthNumRegex}\\s*/\\s*{DayRegex}{DateExtractorYearTermRegex}(?![%])\\b'
    DateExtractor7S = f'({MonthRegex}\\s*[-\\./]\\s*{DayRegex})|(\\b({WeekDayRegex}\\s+)?{MonthNumRegex}\\s*[/\\.]\\s*{DayRegex}(?![%])\\b)'
    DateExtractor8 = f'(?<={DatePreposition}\\s+)({StrictRelativeRegex}\\s+)?({WeekDayRegex}\\s+)?{DayRegex}[\\\\\\-]{MonthNumRegex}(?![%])\\b'
    DateExtractor9L = f'\\b({WeekDayRegex}\\s+)?{DayRegex}\\s*/\\s*{MonthNumRegex}{DateExtractorYearTermRegex}(?![%])\\b'
    DateExtractor9S = f'\\b({WeekDayRegex}\\s+)?{DayRegex}\\s*[/-]\\s*{MonthNumRegex}(?![%])\\b'
    DateExtractorA = f'\\b({WeekDayRegex}\\s+)?{BaseDateTime.FourDigitYearRegex}\\s*[/\\\\\\-\\.]\\s*({MonthNumRegex}|{MonthRegex})\\s*[/\\\\\\-\\.]\\s*{DayRegex}'
    OfMonth = f'^\\s*(يوم\\s+)?من\\s*{MonthRegex}'
    MonthEnd = f'{MonthRegex}\\s*(في)?\\s*$'
    WeekDayEnd = f'(هذا\\s+)?{WeekDayRegex}\\s*[,،]?\\s*$'
    WeekDayStart = f'^[\\.]'
    RangeUnitRegex = f'\\b(?<unit>years?|months?|weeks?)\\b'
    HourNumRegex = f'\\b(?<hournum>الأولى|(ال)?واحدة|(ال)?ثانية|(ال)?ثالثة|(ال)?رابعة|(ال)?خامسة|(ال)?سادسة|(ال)?سابعة|(ال)?ثامنة|(ال)?تاسعة|(ال)?عاشرة|(ال)?حادية عشر(ة)?|(ال)?ثانية عشر(ة)?|خمسة عشر|أحد عشر)\\b'
    MinuteNumRegex = f'\\b(?<minnum>أربع|خمس|ست|سبع|ثمان|تسع|عشر|عشرة|أحد عشر|إثني عشر|إثنا عشر|ثلاثة عشر|خمسة عشر|ثمانية عشر|أربعة عشر|ستة عشر|سبعة عشر|(ال)?حادية عشر(ة)?|تسعة عشر|عشرون|ثلاثون|أربعون|خمسون|عشرين|ثلاث(ين)?|أربعين|خمسين|واحد|إثنان|ثلاثة|خمسة|ثمانية)\\b'
    DeltaMinuteNumRegex = f'(?<deltaminnum>عشرة|أحد عشر|اثنا عشر|ثلاثة عشر|خمسة عشر|ثمانية عشر|أربعة|ستة|سبعة|تسعة|عشرين|أربعة عشر|ستة عشر|سبعة عشر|تسعة عشر| ثلاثون|أربعون|خمسين|أربعين|خمسون|واحد|اثنان|ثلاثة|خمسة|ثمانية|ثلاث(ين)?|أربع|خمس|ست|سبع|ثمان|تسع|(ال)?واحدة|(ال)?ثانية|(ال)?ثالثة|(ال)?رابعة|(ال)?خامسة|(ال)?سادسة|(ال)?سابعة|(ال)?ثامنة|(ال)?تاسعة|(ال)?عاشرة|(ال)?حادية عشر(ة)?|(ال)?ثانية عشر(ة)?)'
    PmRegex = f'(?<pm>(?:(في|حول)\\s|ل)?(وقت\\s)?(بعد الظهر|بعد الظهيرة|(ال)?مساء|مساءً|منتصف(\\s|-)الليل|الغداء|الليل|ليلا))'
    PmRegexFull = f'(?<pm>(?:(في|حول)\\s|ل)?(وقت\\s)?(بعد الظهر|بعد الظهيرة|(ال)?مساء|مساءً|منتصف(\\s|-)الليل|الغداء|الليل|ليلا))'
    AmRegex = f'(?<am>(?:(في|حول)\\s|ل)?(وقت\\s)?((ال)?صباح|صباحا|صباحًا))'
    LunchRegex = f'\\b(موعد الغذاء|وقت الغذاء)\\b'
    NightRegex = f'\\bمنتصف(\\s|-)الليل\\b'
    CommonDatePrefixRegex = f'^[\\.]'
    LessThanOneHour = f'(?<lth>((ال)?ربع|ثلاثة أرباع|(ال)?نصف)|({BaseDateTime.DeltaMinuteRegex}(\\s(دقيقة|دقائق))?)|({DeltaMinuteNumRegex}(\\s(دقيقة|دقائق))?))'
    WrittenTimeRegex = f'(?<writtentime>((ال)?ساعة\\s)?{HourNumRegex}\\s+(و(\\s)?)?({MinuteNumRegex}|{{LessThanOneHour}}|({MinuteNumRegex}\\s+(و(\\s)?)?(?<tens>عشرون|ثلاثون|أربعون|خمسون|عشرين|ثلاثين|أربعين|خمسين))))'
    TimePrefix = f'(?<prefix>(إلا|حتى|و|قبل)?(\\s)?{LessThanOneHour})'
    TimeSuffix = f'(?<suffix>{AmRegex}|{PmRegex}|{OclockRegex})'
    TimeSuffixFull = f'(?<suffix>{AmRegex}|{PmRegexFull}|{OclockRegex})'
    BasicTime = f'\\b(?<basictime>{WrittenTimeRegex}|{HourNumRegex}|({MinuteNumRegex}(\\s(دقيقة|دقائق))?)|{BaseDateTime.HourRegex}:{BaseDateTime.MinuteRegex}(:{BaseDateTime.SecondRegex})?|{BaseDateTime.HourRegex}(?![%\\d]))'
    MidnightRegex = f'(?<midnight>منتصف(\\s|(\\s?-\\s?))الليل)'
    MidmorningRegex = f'(?<midmorning>منتصف(\\s|(\\s?-\\s?))الصباح)'
    MidafternoonRegex = f'(?<midafternoon>منتصف(\\s|(\\s?-\\s?))بعد الظهر)'
    MiddayRegex = f'(?<midday>(وقت الغداء\\s)?(منتصف(\\s|(\\s?-\\s?)))?(النهار|(الساعة\\s)?((((12\\s)?الظهر)|(12\\s)?الظهيرة)|(12\\s)?ظهرا))(\\sوقت الغداء)?)'
    MidTimeRegex = f'(?<mid>({MidnightRegex}|{MidmorningRegex}|{MidafternoonRegex}|{MiddayRegex}))'
    AtRegex = f'\\b(?:(?:(?<=\\bفي\\s+)?(?:{WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}(?!\\.\\d)|{MidTimeRegex}))|{MidTimeRegex})\\b'
    IshRegex = f'\\b((({BaseDateTime.HourRegex}|{WrittenTimeRegex})(\\s|-))?(وقت\\s)?((الظهيرة|الظهر|ظهر(ا|اً))))\\b'
    TimeUnitRegex = f'([^A-Za-z]{{1,}}|\\b)((?<unit>((ال)?(ساعة|دقيقة|ثانية)))|(?<plural>((ال)?(ساعات|دقائق|ثوان|ساعتين|دقيقتين|ثانيتين|ساعتان|دقيقتان|ثانيتان))))\\b'
    RestrictedTimeUnitRegex = f'(?<unit>(ال)?ساعة|(ال)?دقيقة)\\b'
    FivesRegex = f'(?<tens>(?:fifteen|(?:twen|thir|fou?r|fif)ty(\\s*five)?|ten|five))\\b'
    HourRegex = f'\\b{BaseDateTime.HourRegex}'
    PeriodHourNumRegex = f'(?<hour>((واحد|اثنان|اثنين|إثنين|ثلاثة|أربعة|إثنان)?(و(\\s+)?(عشرون|عشرين)))|أحد عشر|إثني عشر|((ثلاثة|خمسة|ثمانية|أربعة|ستة|سبعة|تسعة)(عشر)?)|صفر|واحد|اثنان|إثنان|ثنان|اثنين|عشرة|الأولى|(ال)?واحدة|(ال)?ثانية|(ال)?ثالثة|(ال)?رابعة|(ال)?خامسة|(ال)?سادسة|(ال)?سابعة|(ال)?ثامنة|(ال)?تاسعة|(ال)?عاشرة|(ال)?حادية عشر(ة)?|(ال)?ثانية عشر(ة)?|خمسة عشر)'
    ConnectNumRegex = f'\\b{BaseDateTime.HourRegex}(?<min>[0-5][0-9])\\s*{DescRegex}'
    TimeRegexWithDotConnector = f'({BaseDateTime.HourRegex}(\\s*\\.\\s*){BaseDateTime.MinuteRegex})'
    TimeRegex1 = f'\\b({TimePrefix}\\s+)?({WrittenTimeRegex}(\\s{TimePrefix})?|{HourNumRegex}|{BaseDateTime.HourRegex})(\\s*|[.]){DescRegex}'
    TimeRegex2 = f'(\\b{TimePrefix}\\s+)?(t)?{BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex}((\\s*)?:(\\s*)?{BaseDateTime.SecondRegex})?(?<iam>a)?((\\s*{DescRegex})|\\b)'
    TimeRegex3 = f'(\\b{TimePrefix}\\s+)?{BaseDateTime.HourRegex}\\.{BaseDateTime.MinuteRegex}(\\s*{DescRegex})'
    TimeRegex4 = f'\\b({TimePrefix}\\s+)?{BasicTime}(\\s*{DescRegex})?(\\s+{TimeSuffix})?(\\s*{DescRegex})?\\b'
    TimeRegex5 = f'\\b({DescRegex}\\s)?{BasicTime}((\\s*{DescRegex})((\\s+{TimePrefix})?)|(\\s+{TimePrefix}(\\s+{TimePrefix})?))(\\s{DescRegex})?'
    TimeRegex6 = f'{BasicTime}(\\s*{DescRegex})?\\s+{TimeSuffix}\\b'
    TimeRegex7 = f'\\b({DescRegex}\\s)?(وقت الغداء\\s)?{TimeSuffixFull}\\s+(في\\s+)?{BasicTime}(\\s{DescRegex})?(\\sوقت الغداء)?(\\s{TimePrefix})?((\\s*{DescRegex})|\\b)?'
    TimeRegex8 = f'.^'
    TimeRegex9 = f'\\b{PeriodHourNumRegex}(\\s+|-){FivesRegex}((\\s*{DescRegex})|\\b)'
    TimeRegex10 = f'\\b({TimePrefix}\\s+)?{BaseDateTime.HourRegex}(\\s*h\\s*){BaseDateTime.MinuteRegex}(\\s*{DescRegex})?'
    TimeRegex11 = f'\\b((?:({TimeTokenPrefix})?{TimeRegexWithDotConnector}(\\s*{DescRegex}))|(?:(?:{TimeTokenPrefix}{TimeRegexWithDotConnector})(?!\\s*per\\s*cent|%)))'
    FirstTimeRegexInTimeRange = f'\\b{TimeRegexWithDotConnector}(\\s*{DescRegex})?'
    PureNumFromTo = f'({RangePrefixRegex}\\s+)?(الساعة\\s+)?(({BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex})|{HourRegex}|{PeriodHourNumRegex})(\\s*(?<leftDesc>{DescRegex}))?\\s*{TillRegex}\\s*(\\s+الساعة\\s+)?(({BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex})|{HourRegex}|{PeriodHourNumRegex})(\\s+{TimePrefix})?(?<rightDesc>\\s*({PmRegex}|{AmRegex}|{DescRegex}))?(\\s+اليوم)?'
    PureNumBetweenAnd = f'(بين\\s+)(الساعة\\s+)?(({BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex})|{HourRegex}|{PeriodHourNumRegex})(\\s*(?<leftDesc>{DescRegex}))?\\s*{RangeConnectorRegex}(\\s*)?(\\s+الساعة\\s+)?(({BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex})|{HourRegex}|{PeriodHourNumRegex})(\\s+{TimePrefix})?(?<rightDesc>\\s*({PmRegex}|{AmRegex}|{DescRegex}))?(\\s+اليوم)?'
    SpecificTimeFromTo = f'({RangePrefixRegex}\\s+)?(الساعة\\s+)?(?<time1>(({BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex})|({HourRegex}|{PeriodHourNumRegex})(\\s*(?<leftDesc>{DescRegex}))?))\\s*{TillRegex}\\s*(\\s+الساعة\\s+)?(?<time2>(({TimeRegexWithDotConnector}(?<rightDesc>\\s*{DescRegex}))|({BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex})|({HourRegex}|{PeriodHourNumRegex})(\\s+{TimePrefix})?(\\s*(?<rightDesc>{DescRegex}))?))(\\s+اليوم)?'
    SpecificTimeBetweenAnd = f'(بين\\s+)(الساعة\\s+)?(?<time1>(({BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex})|({HourRegex}|{PeriodHourNumRegex})(\\s*(?<leftDesc>{DescRegex}))?))\\s*{RangeConnectorRegex}(\\s*)?(\\s+الساعة\\s+)?(?<time2>(({TimeRegexWithDotConnector}(?<rightDesc>\\s*{DescRegex}))|(({BaseDateTime.HourRegex}(\\s*)?:(\\s*)?{BaseDateTime.MinuteRegex})|{HourRegex}|{PeriodHourNumRegex})(\\s+{TimePrefix})?(\\s*(?<rightDesc>{DescRegex}))?))(\\s+اليوم)?'
    SuffixAfterRegex = f'\\b(((at)\\s)?(or|and)\\s+(above|after|later|greater)(?!\\s+than))\\b'
    PrepositionRegex = f'(?<prep>^(at|on|of)(\\s+the)?$)'
    LaterEarlyRegex = f'((?<early>(\\s+|-)الباكر)|(?<late>وقت متأخر(\\s+|-))|أواخر(\\s+|-)|وقت مبكر(\\s+|-)|أول(\\s+|-)|آخر(\\s+|-))'
    MealTimeRegex = f'\\b((في|عند)\\s+)?(وقت\\s)?(?<mealTime>(ال)?إفطار|(ال)?فطور|(ال)?عشاء|(ال)?غذاء)\\b'
    UnspecificTimePeriodRegex = f'({MealTimeRegex})'
    TimeOfDayRegex = f'\\b(?<timeOfDay>((((في|عند)\\s+)?{LaterEarlyRegex}?(من\\s+)?(الصباح|بعد الظهر|الليل|المساء|الظهر|الأمسيات){LaterEarlyRegex}?)|{MealTimeRegex}|(((في|عند|خلال)\\s+)?(النهار|((ساعة|ساعات)(\\s)?العمل)))))\\b'
    SpecificTimeOfDayRegex = f'\\b(({StrictRelativeRegex}\\s+{TimeOfDayRegex})\\b|\\btoni(ght|te))s?\\b'
    TimeFollowedUnit = f'^\\s*{TimeUnitRegex}'
    TimeNumberCombinedWithUnit = f'\\b(?<num>\\d+(\\.\\d*)?)(\\s)?(-)?{TimeUnitRegex}'
    BusinessHourSplitStrings = [r'business', r'hour']
    NowRegex = f'\\b(?<now>(right\\s+)?now|at th(e|is) minute|as soon as possible|asap|recently|previously)\\b'
    NowParseRegex = f'\\b({NowRegex}|^(date)$)\\b'
    SuffixRegex = f'^\\s*(in the\\s+)?(morning|afternoon|evening|night)\\b'
    NonTimeContextTokens = f'(building)'
    DateTimeTimeOfDayRegex = f'\\b(?<timeOfDay>morning|(?<pm>afternoon|night|evening))\\b'
    DateTimeSpecificTimeOfDayRegex = f'\\b(({RelativeRegex}\\s+{DateTimeTimeOfDayRegex})\\b|\\btoni(ght|te))\\b'
    TimeOfTodayAfterRegex = f'^\\s*(,\\s*)?(in\\s+)?{DateTimeSpecificTimeOfDayRegex}'
    TimeOfTodayBeforeRegex = f'{DateTimeSpecificTimeOfDayRegex}(\\s*,)?(\\s+(at|around|in|on))?\\s*$'
    SimpleTimeOfTodayAfterRegex = f'(?<!{NonTimeContextTokens}\\s*)\\b({HourNumRegex}|{BaseDateTime.HourRegex})\\s*(,\\s*)?(in\\s+)?{DateTimeSpecificTimeOfDayRegex}\\b'
    SimpleTimeOfTodayBeforeRegex = f'\\b{DateTimeSpecificTimeOfDayRegex}(\\s*,)?(\\s+(at|around))?\\s*({HourNumRegex}|{BaseDateTime.HourRegex})\\b'
    SpecificEndOfRegex = f'(the\\s+)?end of(\\s+the)?\\s*$'
    UnspecificEndOfRegex = f'\\b(the\\s+)?(eod|(end\\s+of\\s+day))\\b'
    UnspecificEndOfRangeRegex = f'\\b(eoy)\\b'
    PeriodTimeOfDayRegex = f'\\b((in\\s+(the)?\\s+)?{LaterEarlyRegex}?(this\\s+)?{DateTimeTimeOfDayRegex})\\b'
    PeriodSpecificTimeOfDayRegex = f'\\b({LaterEarlyRegex}?this\\s+{DateTimeTimeOfDayRegex}|({StrictRelativeRegex}\\s+{PeriodTimeOfDayRegex})\\b|\\btoni(ght|te))\\b'
    PeriodTimeOfDayWithDateRegex = f'\\b(({PeriodTimeOfDayRegex}(\\s+(on|of))?))\\b'
    LessThanRegex = f'\\b(أقل\\s+من)\\b'
    MoreThanRegex = f'\\b(أكثر\\s+من)\\b'
    DurationUnitRegex = f'(?<unit>{DateUnitRegex}|(ال)?ساعة|(ال)?ساعات|(ال)?دقيقة|(ال)?دقائق|(ال)?ثانية|(ال)?ثوان|(ال)?ليلة|(ال)?ليال)|ساعت(ين)?(ان)?|دقيقت(ين)?(ان)?|ثانيت(ين)?(ان)?|ليلت(ين)?(ان)?\\b'
    SuffixAndRegex = f'(?<suffix>\\s*(و)\\s+(?<suffix_num>نصف|ربع))'
    PeriodicRegex = f'\\b(?<periodic>((?<multiplier>semi|bi|tri)(\\s*|-))?(daily|monthly|weekly|quarterly|yearly|annual(ly)?))\\b'
    EachUnitRegex = f'\\b(?<each>(each|every|any|once an?)(?<other>\\s+other)?\\s+({DurationUnitRegex}|(?<specialUnit>quarters?|weekends?)|{WeekDayRegex})|(?<specialUnit>weekends))'
    EachPrefixRegex = f'\\b(?<each>(each|every|once an?)\\s*$)'
    SetEachRegex = f'\\b(?<each>(each|every)(?<other>\\s+other)?\\s*)(?!the|that)\\b'
    SetLastRegex = f'(?<last>following|next|upcoming|this|{LastNegPrefix}last|past|previous|current)'
    EachDayRegex = f'^\\s*(each|every)\\s*day\\b'
    DurationFollowedUnit = f'(^\\s*{DurationUnitRegex}\\s+{SuffixAndRegex})|(^\\s*{SuffixAndRegex}?\\s+?{DurationUnitRegex})'
    NumberCombinedWithDurationUnit = f'((?<num>\\d+(\\.\\d*)?(\\s)?)?({DurationUnitRegex})(\\s{WrittenOneToNineRegex})?(\\sو)?(\\s)?(?<num>\\d+(\\.\\d*)?(\\s)?)?(({DurationUnitRegex})?(\\s{WrittenOneToNineRegex})?)(\\sو)?(\\s)?(?<num>\\d+(\\.\\d*)?(\\s)?)?({DurationUnitRegex})(\\s{WrittenOneToNineRegex})?)'
    AnUnitRegex = f'\\b((?<half>(1/2|½|نصف)))\\s+{DurationUnitRegex}(\\s(أخرى))?'
    DuringRegex = f'\\b((((خلال|على مدى|مدة)\\s)|ل)+)(?<unit>(ال)?عام(ين)?|(ال)?سنتين|(ال)?سنة|(ال)?شهر(ين)?|الأشهر|(ال)?أسبوع(ين)?|(ال)?يوم(ين)?)\\b'
    AllRegex = f'(?<all>(طوال\\s+))?(?<unit>(ال)?عام|(ال)?سنة|(ال)?شهر|(ال)?أسبوع|(ال)?أسابيع|(ال)?أيام|(ال)?يوم)(?<all>(\\s+كامل(ة)?))?'
    HalfRegex = f'\\b((نصف)\\s+)?(?<unit>(ال)?ساعة|ساعتين|دقيقة|دقيقتين|ثانية|ثانيتين|(ال)?عام(ين)?|(ال)?سنة|(ال)?شهر(ين)?|(ال)?أسبوع(ين)?|(ال)?يوم(ين)?)(?<half>(\\s+)?(و)?نصف)?\\b'
    ConjunctionRegex = f'\\b((و(\\s+ل)?)|مع)\\b'
    ArabicThisYearRegex = f'(\\s*)?(هذا|هذه)?(لعام|عام|العام|سنة)?(\\s*)?'
    ArabicEidDay = f'?(يوم |عيد |ليلة | ليل)?(?=\\s*)'
    HolidayList1 = f'((?=\\s*){ArabicEidDay}(إثنين الرماد|رأس السنة الهجرية|الحج|يواندان|الفطر|رأس السنة الجديدة|الأضحى|الأب|الشكر|عيد الميلاد|المولد النبوي|الفصح)(){ArabicThisYearRegex}(العام|السنة)?({YearRegex})?({RelativeRegex})?)'
    HolidayList2 = f'((?=\\s*){ArabicEidDay}(الشباب|الأطفال|الفتيات|العشاق|الأرض|الافتتاح|جرذ الأرض|الحب|الذكرى|الخريف|القمر|الربيع|الفانوس)(){ArabicThisYearRegex}(العام|السنة)?({YearRegex})?({RelativeRegex})?)'
    HolidayList3 = f'((?=\\s*){ArabicEidDay}(حقوق الإنسان|العالمي للأعمال الخيرية|يوم التحرير الأفريقي|حرية الصحافة العالمية|الاستقلال|الرؤساء|كل الأرواح|الشجرة|مارتن لوثر كينج| هالوين|العمال العالمي|الأم)(){ArabicThisYearRegex}(العام|السنة)?({YearRegex})?({RelativeRegex})?)'
    HolidayRegex = f'({HolidayList1})|({HolidayList2})|({HolidayList3})'
    AMTimeRegex = f'(?<am>morning)'
    PMTimeRegex = f'\\b(?<pm>afternoon|evening|night)\\b'
    NightTimeRegex = f'(night)'
    NowTimeRegex = f'(now|at th(e|is) minute)'
    RecentlyTimeRegex = f'(recently|previously)'
    AsapTimeRegex = f'(as soon as possible|asap)'
    InclusiveModPrepositions = f'(?<include>((on|in|at)\\s+or\\s+)|(\\s+or\\s+(on|in|at)))'
    AroundRegex = f'(?:\\b(?:around|circa)\\s*?\\b)(\\s+the)?'
    BeforeRegex = f'((\\b{InclusiveModPrepositions}?(?:before|in\\s+advance\\s+of|prior\\s+to|(no\\s+later|earlier|sooner)\\s+than|ending\\s+(with|on)|by|(un)?till?|(?<include>as\\s+late\\s+as)){InclusiveModPrepositions}?\\b\\s*?)|(?<!\\w|>)((?<include><\\s*=)|<))(\\s+the)?'
    AfterRegex = f'((\\b{InclusiveModPrepositions}?((after|(starting|beginning)(\\s+on)?(?!\\sfrom)|(?<!no\\s+)later than)|(year greater than))(?!\\s+or equal to){InclusiveModPrepositions}?\\b\\s*?)|(?<!\\w|<)((?<include>>\\s*=)|>))(\\s+the)?'
    SinceRegex = f'(?:(?:\\b(?:since|after\\s+or\\s+equal\\s+to|starting\\s+(?:from|on|with)|as\\s+early\\s+as|(any\\s+time\\s+)from)\\b\\s*?)|(?<!\\w|<)(>=))(\\s+the)?'
    SinceRegexExp = f'({SinceRegex}|\\bfrom(\\s+the)?\\b)'
    AgoRegex = f'\\b(ago|before\\s+(?<day>yesterday|today))\\b'
    LaterRegex = f'\\b(?:later(?!((\\s+in)?\\s*{OneWordPeriodRegex})|(\\s+{TimeOfDayRegex})|\\s+than\\b)|من الآن|من الان|(from|after)\\s+(?<day>tomorrow|tmr|today))\\b'
    BeforeAfterRegex = f'\\b((?<before>before)|(?<after>from|after))\\b'
    ModPrefixRegex = f'\\b({RelativeRegex}|{AroundRegex}|{BeforeRegex}|{AfterRegex}|{SinceRegex})\\b'
    ModSuffixRegex = f'\\b({AgoRegex}|{LaterRegex}|{BeforeAfterRegex}|{FutureSuffixRegex}|{PastSuffixRegex})\\b'
    InConnectorRegex = f'\\b(in)\\b'
    SinceYearSuffixRegex = f'(^\\s*{SinceRegex}(\\s*(the\\s+)?year\\s*)?{YearSuffix})'
    WithinNextPrefixRegex = f'\\b(within(\\s+the)?(\\s+(?<next>{NextPrefixRegex}))?)\\b'
    TodayNowRegex = f'\\b(today|now)\\b'
    MorningStartEndRegex = f'(^(morning|{AmDescRegex}))|((morning|{AmDescRegex})$)'
    AfternoonStartEndRegex = f'(^(afternoon|{PmDescRegex}))|((afternoon|{PmDescRegex})$)'
    EveningStartEndRegex = f'(^(evening))|((evening)$)'
    NightStartEndRegex = f'(^(over|to)?ni(ght|te))|((over|to)?ni(ght|te)$)'
    InexactNumberRegex = f'بضع(ة)?|عدة|(?<NumTwoTerm>((ل))?عدد(\\s+من)?)'
    InexactNumberUnitRegex = f'({InexactNumberRegex})\\s+({DurationUnitRegex})|(في\\s+)?((ال)?يومين|(ال)?أيام|(ال)?أسابيع|(ال)?أشهر|(ال)?سنوات|(ال)?أعوام|(ال)?سنين)\\s+(العديدة|القليلة|الثلاثة|الأربعة|الخمسة|الستة|السبعة|الثمانية|التسعة|العشرة)'
    RelativeTimeUnitRegex = f'(?:(?:(?:{NextPrefixRegex}|{PreviousPrefixRegex}|{ThisPrefixRegex})\\s+({TimeUnitRegex}))|((the|my))\\s+({RestrictedTimeUnitRegex}))'
    RelativeDurationUnitRegex = f'(?:(?:(?<=({NextPrefixRegex}|{PreviousPrefixRegex}|{ThisPrefixRegex})\\s+)({DurationUnitRegex}))|((the|my))\\s+({RestrictedTimeUnitRegex}))'
    ReferenceDatePeriodRegex = f'\\b{ReferencePrefixRegex}\\s+(?<duration>week|month|year|decade|weekend)\\b'
    ConnectorRegex = f'^(-|,|for|t|around|@)$'
    FromToRegex = f'(\\b(from).+(to|and|or)\\b.+)'
    SingleAmbiguousMonthRegex = f'^(the\\s+)?(may|march)$'
    SingleAmbiguousTermsRegex = f'^(the\\s+)?(day|week|month|year)$'
    UnspecificDatePeriodRegex = f'^(week|month|year)$'
    PrepositionSuffixRegex = f'\\b(on|in|at|around|from|to)$'
    FlexibleDayRegex = f'(?<DayOfMonth>([A-Za-z]+\\s)?[A-Za-z\\d]+)'
    ForTheRegex = f'\\b((((?<=for\\s+)the\\s+{FlexibleDayRegex})|((?<=on\\s+)(the\\s+)?{FlexibleDayRegex}(?<=(st|nd|rd|th))))(?<end>\\s*(,|\\.(?!\\d)|!|\\?|$)))'
    WeekDayAndDayOfMonthRegex = f'\\b{WeekDayRegex}\\s+(the\\s+{FlexibleDayRegex})\\b'
    WeekDayAndDayRegex = f'\\b{WeekDayRegex}\\s+(?!(the)){DayRegex}(?!([-:]|(\\s+({AmDescRegex}|{PmDescRegex}|{OclockRegex}))))\\b'
    RestOfDateRegex = f'\\b(باقي|بقية)\\s+(?<duration>الشهر|العام|الأسبوع|العقد)\\b'
    RestOfDateTimeRegex = f'\\b(rest|remaining)\\s+(of\\s+)?((the|my|this|current)\\s+)?(?<unit>day)\\b'
    AmbiguousRangeModifierPrefix = f'(from)'
    NumberEndingPattern = f'^(?:\\s+(?<meeting>meeting|appointment|conference|((skype|teams|zoom|facetime)\\s+)?call)\\s+to\\s+(?<newTime>{PeriodHourNumRegex}|{HourRegex})([\\.]?$|(\\.,|,|!|\\?)))'
    OneOnOneRegex = f'\\b(1\\s*:\\s*1(?!\\d))|(one (on )?one|one\\s*-\\s*one|one\\s*:\\s*one)\\b'
    LaterEarlyPeriodRegex = f'(\\b(({PrefixPeriodRegex})\\s*\\b\\s*(?<suffix>{OneWordPeriodRegex}|(?<FourDigitYear>{BaseDateTime.FourDigitYearRegex}))|({UnspecificEndOfRangeRegex}))\\b)|({PrefixPeriodRegex}\\s+(من هذا|من|هذا)\\s+(الشهر|الأسبوع|العام|الاسبوع)(\\s+{PrefixPeriodRegex})?)'
    WeekWithWeekDayRangeRegex = f'\\b(?<week>(هذا\\s+)?(الأسبوع)\\s+(({NextPrefixRegex}|{PreviousPrefixRegex})\\s+)?)(((بين)\\s+{WeekDayRegex}\\s+(و)\\s*{WeekDayRegex})|(من)\\s+{WeekDayRegex}\\s+(إلى)\\s+{WeekDayRegex})\\b'
    GeneralEndingRegex = f'^\\s*((\\.,)|\\.|,|!|\\?)?\\s*$'
    MiddlePauseRegex = f'\\s*(,)\\s*'
    DurationConnectorRegex = f'^\\s*(?<connector>\\s+|و|،|,)\\s*$'
    PrefixArticleRegex = f'\\bإلى\\s+'
    OrRegex = f'\\s*((\\b|,\\s*)(or|and)\\b|,)\\s*'
    SpecialYearTermsRegex = f'\\b(((ال)?سنة|(ال)?عام)(\\s+{SpecialYearPrefixes})?)'
    YearPlusNumberRegex = f'\\b({SpecialYearTermsRegex}\\s*((?<year>(\\d{{2,4}}))|{FullTextYearRegex}))\\b'
    NumberAsTimeRegex = f'\\b({WrittenTimeRegex}|{PeriodHourNumRegex}|{BaseDateTime.HourRegex})\\b'
    TimeBeforeAfterRegex = f'\\b(((?<=\\b(ب|((قبل|في موعد لا يتجاوز| بعد)\\s))(وقت\\s+)?)({WrittenTimeRegex}|{HourNumRegex}|{BaseDateTime.HourRegex}|{MidTimeRegex}))|{MidTimeRegex})\\b'
    DateNumberConnectorRegex = f'^\\s*(?<connector>\\s+at)\\s*$'
    DecadeRegex = f'(?<decade>(ال)?عشرات|(ال)?عشرينيات|(ال)?عشرينات|(ال)?ثلاثينات|(ال)?أربعينيات|(ال)?أربعينات|(ال)?خمسينيات|(ال)?خمسينات|(ال)?ستينات|(ال)?سبعينيات|(ال)?سبعينات|(ال)?ثمانينات|(ال)?تسعينات|الألفين|ألفين)'
    DecadeWithCenturyRegex = f'({DecadeRegex})((\\s+القرن(\\s+(الثماني عشر|التاسع عشر)))|(\\s+(و{DecadeRegex})))?'
    RelativeDecadeRegex = f'\\b(?<number>(الثلاثة|الأربعة|الخمسة|الستة|السبعة|الثمانية|التسعة|العشر|\\d+)\\s+)?((ال)?عقدين|(ال)?عقد|(ال)?عقود)\\s+(الماضيين|الماضية|الماضي|القادمين|القادمة|القادم)\\b'
    YearPeriodRegex = f'(((من|بين)\\s+)?{YearRegex}\\s*({TillRegex}|{RangeConnectorRegex})\\s*{YearRegex})'
    StrictTillRegex = f'(?<till>\\b(to|(un)?till?|thru|through)\\b|{BaseDateTime.RangeConnectorSymbolRegex}(?!\\s*(h[1-2]|q[1-4])(?!(\\s+of|\\s*,\\s*))))'
    StrictRangeConnectorRegex = f'(?<and>\\b(and|through|to)\\b|{BaseDateTime.RangeConnectorSymbolRegex}(?!\\s*(h[1-2]|q[1-4])(?!(\\s+of|\\s*,\\s*))))'
    StartMiddleEndRegex = f'\\b((?<StartOf>((the\\s+)?(start|beginning)\\s+of\\s+)?)(?<MiddleOf>((the\\s+)?middle\\s+of\\s+)?)(?<EndOf>((the\\s+)?end\\s+of\\s+)?))'
    ComplexDatePeriodRegex = f'(?:((from|during|in)\\s+)?{StartMiddleEndRegex}(?<start>.+)\\s*({StrictTillRegex})\\s*{StartMiddleEndRegex}(?<end>.+)|((between)\\s+){StartMiddleEndRegex}(?<start>.+)\\s*({StrictRangeConnectorRegex})\\s*{StartMiddleEndRegex}(?<end>.+))'
    FailFastRegex = f'{BaseDateTime.DeltaMinuteRegex}|\\b(?:{BaseDateTime.BaseAmDescRegex}|{BaseDateTime.BasePmDescRegex})|{BaseDateTime.BaseAmPmDescRegex}|\\b(?:zero|{WrittenOneToNineRegex}|{WrittenElevenToNineteenRegex}|{WrittenTensRegex}|{WrittenMonthRegex}|{SeasonDescRegex}|{DecadeRegex}|centur(y|ies)|weekends?|quarters?|hal(f|ves)|yesterday|to(morrow|day|night)|tmr|noonish|\\d(-|——)?ish|((the\\s+\\w*)|\\d)(th|rd|nd|st)|(mid\\s*(-\\s*)?)?(night|morning|afternoon|day)s?|evenings?||noon|lunch(time)?|dinner(time)?|(day|night)time|overnight|dawn|dusk|sunset|hours?|hrs?|h|minutes?|mins?|seconds?|secs?|eo[dmy]|mardi[ -]?gras|birthday|eve|christmas|xmas|thanksgiving|halloween|yuandan|easter|yuan dan|april fools|cinco de mayo|all (hallow|souls)|guy fawkes|(st )?patrick|hundreds?|noughties|aughts|thousands?)\\b|{WeekDayRegex}|{SetWeekDayRegex}|{NowRegex}|{PeriodicRegex}|\\b({DateUnitRegex}|{ImplicitDayRegex})'
    UnitMap = dict([("قرن", "10Y"),
                    ("حقبة", "10Y"),
                    ("قرون", "10Y"),
                    ("حقبات", "10Y"),
                    ("قرنين", "20Y"),
                    ("قرنان", "20Y"),
                    ("حقبتان", "20Y"),
                    ("حقبتين", "20Y"),
                    ("سنة", "Y"),
                    ("العام", "Y"),
                    ("عام", "Y"),
                    ("سنوات", "Y"),
                    ("أعوام", "Y"),
                    ("عامان", "2Y"),
                    ("سنتان", "2Y"),
                    ("سنتين", "2Y"),
                    ("عامين", "2Y"),
                    ("الشهر", "MON"),
                    ("شهر", "MON"),
                    ("أشهر", "MON"),
                    ("شهور", "MON"),
                    ("شهرا", "MON"),
                    ("شهرين", "2MON"),
                    ("شهران", "2MON"),
                    ("quarters", "3MON"),
                    ("quarter", "3MON"),
                    ("semesters", "6MON"),
                    ("semestres", "6MON"),
                    ("semester", "6MON"),
                    ("semestre", "6MON"),
                    ("أسبوع", "W"),
                    ("أسابيع", "W"),
                    ("أسبوعا", "W"),
                    ("أسبوعان", "2W"),
                    ("أسبوعين", "2W"),
                    ("نهاية الأسبوع", "WE"),
                    ("يوم", "D"),
                    ("أيام", "D"),
                    ("يوما", "D"),
                    ("يومان", "2D"),
                    ("يومين", "2D"),
                    ("ليال", "D"),
                    ("ليلة", "D"),
                    ("ساعة", "H"),
                    ("ساعات", "H"),
                    ("ساعتان", "2H"),
                    ("ساعتين", "2H"),
                    ("دقيقة", "M"),
                    ("دقائق", "M"),
                    ("دقيقتان", "2M"),
                    ("دقيقتين", "2M"),
                    ("ثانية", "S"),
                    ("ثوان", "S"),
                    ("ثانيتان", "2S"),
                    ("ثانيتين", "2S")])
    UnitValueMap = dict([("قرن", 315360000),
                         ("حقبة", 315360000),
                         ("قرون", 315360000),
                         ("حقبات", 315360000),
                         ("قرنين", 630720000),
                         ("حقبتين", 630720000),
                         ("قرنان", 630720000),
                         ("حقبتان", 630720000),
                         ("سنة", 31536000),
                         ("العام", 31536000),
                         ("عام", 31536000),
                         ("سنوات", 31536000),
                         ("أعوام", 31536000),
                         ("عامان", 63072000),
                         ("سنتان", 63072000),
                         ("سنتين", 63072000),
                         ("عامين", 63072000),
                         ("الشهر", 2592000),
                         ("شهر", 2592000),
                         ("أشهر", 2592000),
                         ("شهور", 2592000),
                         ("شهرا", 2592000),
                         ("شهرين", 5184000),
                         ("شهران", 5184000),
                         ("نهاية الأسبوع", 172800),
                         ("أسبوع", 604800),
                         ("أسابيع", 604800),
                         ("أسبوعا", 604800),
                         ("أسبوعان", 1209600),
                         ("أسبوعين", 1209600),
                         ("يوم", 86400),
                         ("أيام", 86400),
                         ("يوما", 86400),
                         ("يومان", 172800),
                         ("يومين", 172800),
                         ("ليلة", 86400),
                         ("ليال", 86400),
                         ("ساعة", 3600),
                         ("ساعات", 3600),
                         ("ساعتان", 7200),
                         ("ساعتين", 7200),
                         ("دقيقة", 60),
                         ("دقائق", 60),
                         ("دقيقتان", 120),
                         ("دقيقتين", 120),
                         ("ثانية", 1),
                         ("ثوان", 1),
                         ("ثانيتان", 2),
                         ("ثانيتين", 2)])
    SpecialYearPrefixesMap = dict([("fiscal", "FY"),
                                   ("school", "SY"),
                                   ("fy", "FY"),
                                   ("sy", "SY")])
    SeasonMap = dict([("الربيع", "SP"),
                      ("ربيع", "SP"),
                      ("الصيف", "SU"),
                      ("صيف", "SU"),
                      ("الخريف", "FA"),
                      ("خريف", "FA"),
                      ("الشتاء", "WI"),
                      ("شتاء", "WI")])
    SeasonValueMap = dict([("SP", 3),
                           ("SU", 6),
                           ("FA", 9),
                           ("WI", 12)])
    CardinalMap = dict([("الأول", 1),
                        ("الثاني", 2),
                        ("الثالث", 3),
                        ("الرابع", 4),
                        ("الخامس", 5)])
    DayOfWeek = dict([("الاثنين", 1),
                      ("الإثنين", 1),
                      ("monday", 1),
                      ("الثلاثاء", 2),
                      ("tuesday", 2),
                      ("الأربعاء", 3),
                      ("wednesday", 3),
                      ("الخميس", 4),
                      ("thursday", 4),
                      ("الجمعة", 5),
                      ("friday", 5),
                      ("السبت", 6),
                      ("saturday", 6),
                      ("الأحد", 0),
                      ("sunday", 0),
                      ("إثنين", 1),
                      ("ثلاثاء", 2),
                      ("أربعاء", 3),
                      ("خميس", 4),
                      ("جمعة", 5),
                      ("سبت", 6),
                      ("أحد", 0)])
    MonthOfYear = dict([("يناير", 1),
                        ("فبراير", 2),
                        ("مارس", 3),
                        ("أبريل", 4),
                        ("مايو", 5),
                        ("يونيو", 6),
                        ("يوليو", 7),
                        ("أغسطس", 8),
                        ("سبتمبر", 9),
                        ("أكتوبر", 10),
                        ("نوفمبر", 11),
                        ("ديسمبر", 12),
                        ("محرم", 1),
                        ("صفر", 2),
                        ("ربيع الأول", 3),
                        ("ربيع الثاني", 4),
                        ("جمادى الأول", 5),
                        ("جمادى الثاني", 6),
                        ("رجب", 7),
                        ("شعبان", 8),
                        ("رمضان", 9),
                        ("شوال", 10),
                        ("ذو القعدة", 11),
                        ("ذو الحجة", 12),
                        ("كانون الثاني", 1),
                        ("شباط", 2),
                        ("آذار", 3),
                        ("نيسان", 4),
                        ("أيار", 5),
                        ("حزيران", 6),
                        ("تموز", 7),
                        ("آب", 8),
                        ("أيلول", 9),
                        ("تشرين الأول", 10),
                        ("تشرين الثاني", 11),
                        ("كانون الأول", 12),
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
    Numbers = dict([("صفر", 0),
                    ("واحد", 1),
                    ("الواحدة", 1),
                    ("اثنان", 2),
                    ("الثانية", 2),
                    ("ثلاثة", 3),
                    ("ثلاث", 3),
                    ("الثالثة", 3),
                    ("أربعة", 4),
                    ("الرابعة", 4),
                    ("خمسة", 5),
                    ("الخامسة", 5),
                    ("ستة", 6),
                    ("السادسة", 6),
                    ("سبعة", 7),
                    ("السابعة", 7),
                    ("ثمانية", 8),
                    ("الثامنة", 8),
                    ("تسعة", 9),
                    ("التاسعة", 9),
                    ("عشرة", 10),
                    ("العاشرة", 10),
                    ("أحد عشر", 11),
                    ("الحادية عشر", 11),
                    ("اثنا عشر", 12),
                    ("ثلاثة عشر", 13),
                    ("أربعة عشر", 14),
                    ("خمسة عشر", 15),
                    ("ستة عشر", 16),
                    ("سبعة عشر", 17),
                    ("ثمانية عشر", 18),
                    ("تسعة عشر", 19),
                    ("عشرون", 20),
                    ("عشرين", 20),
                    ("واحد وعشرون", 21),
                    ("اثنان وعشرون", 22),
                    ("ثلاثة وعشرون", 23),
                    ("أربعة وعشرون", 24),
                    ("خمسة وعشرون", 25),
                    ("ستة وعشرون", 26),
                    ("سبعة وعشرون", 27),
                    ("ثمانية وعشرون", 28),
                    ("تسعة وعشرون", 29),
                    ("الثلاثين", 30),
                    ("ثلاثين", 30),
                    ("واحد وثلاثون", 31),
                    ("اثنان وثلاثون", 32),
                    ("ثلاثة وثلاثون", 33),
                    ("أربعة وثلاثون", 34),
                    ("خمسة وثلاثون", 35),
                    ("ستة وثلاثون", 36),
                    ("سبعة وثلاثون", 37),
                    ("ثمانية وثلاثون", 38),
                    ("تسعة وثلاثون", 39),
                    ("أربعون", 40),
                    ("واحد وأربعون", 41),
                    ("اثنان وأربعون", 42),
                    ("ثلاثة وأربعون", 43),
                    ("أربعة وأربعون", 44),
                    ("خمسة وأربعون", 45),
                    ("ستة وأربعون", 46),
                    ("سبعة وأربعون", 47),
                    ("ثمانية وأربعون", 48),
                    ("تسعة وأربعون", 49),
                    ("خمسون", 50),
                    ("واحد وخمسون", 51),
                    ("اثنان وخمسون", 52),
                    ("ثلاثة وخمسون", 53),
                    ("أربعة وخمسون", 54),
                    ("خمسة وخمسون", 55),
                    ("ستة وخمسون", 56),
                    ("سبعة وخمسون", 57),
                    ("ثمانية وخمسون", 58),
                    ("تسعة وخمسون", 59),
                    ("ستين", 60),
                    ("واحد وستون", 61),
                    ("اثنان وستون", 62),
                    ("ثلاثة وستون", 63),
                    ("أربعة وستون", 64),
                    ("خمسة وستون", 65),
                    ("ستة وستون", 66),
                    ("سبعة وستون", 67),
                    ("ثمانية وستون", 68),
                    ("تسعة وستون", 69),
                    ("السبعون", 70),
                    ("واحد وسبعون", 71),
                    ("اثنان وسبعون", 72),
                    ("ثلاثة وسبعون", 73),
                    ("أربعة وسبعون", 74),
                    ("خمسة وسبعون", 75),
                    ("ستة وسبعون", 76),
                    ("سبعة وسبعون", 77),
                    ("ثمانية وسبعون", 78),
                    ("تسعة وسبعون", 79),
                    ("ثمانون", 80),
                    ("واحد وثمانون", 81),
                    ("اثنان وثمانون", 82),
                    ("ثلاثة وثمانون", 83),
                    ("أربعة وثمانون", 84),
                    ("خمسة وثمانون", 85),
                    ("ستة وثمانون", 86),
                    ("سبعة وثمانون", 87),
                    ("ثمانية وثمانين", 88),
                    ("تسعة وثمانون", 89),
                    ("تسعون", 90),
                    ("واحد وتسعون", 91),
                    ("اثنان وتسعون", 92),
                    ("ثلاثة وتسعون", 93),
                    ("أربعة وتسعون", 94),
                    ("خمسة وتسعون", 95),
                    ("ستة وتسعون", 96),
                    ("سبعة وتسعون", 97),
                    ("ثمانية وتسعون", 98),
                    ("تسعة وتسعون", 99),
                    ("مائة", 100)])
    DayOfMonth = dict([("1st", 1),
                       ("1th", 1),
                       ("2nd", 2),
                       ("2th", 2),
                       ("3rd", 3),
                       ("3th", 3),
                       ("4th", 4),
                       ("5th", 5),
                       ("6th", 6),
                       ("7th", 7),
                       ("8th", 8),
                       ("9th", 9),
                       ("10th", 10),
                       ("11th", 11),
                       ("11st", 11),
                       ("12th", 12),
                       ("12nd", 12),
                       ("13th", 13),
                       ("13rd", 13),
                       ("14th", 14),
                       ("15th", 15),
                       ("16th", 16),
                       ("17th", 17),
                       ("18th", 18),
                       ("19th", 19),
                       ("20th", 20),
                       ("21st", 21),
                       ("21th", 21),
                       ("22nd", 22),
                       ("22th", 22),
                       ("23rd", 23),
                       ("23th", 23),
                       ("24th", 24),
                       ("25th", 25),
                       ("26th", 26),
                       ("27th", 27),
                       ("28th", 28),
                       ("29th", 29),
                       ("30th", 30),
                       ("31st", 31),
                       ("01st", 1),
                       ("01th", 1),
                       ("02nd", 2),
                       ("02th", 2),
                       ("03rd", 3),
                       ("03th", 3),
                       ("04th", 4),
                       ("05th", 5),
                       ("06th", 6),
                       ("07th", 7),
                       ("08th", 8),
                       ("09th", 9)])
    DoubleNumbers = dict([("half", 0.5),
                          ("quarter", 0.25)])
    HolidayNames = dict([("easterday", ["easterday", "easter", "eastersunday"]),
                         ("ashwednesday", ["ashwednesday"]),
                         ("palmsunday", ["palmsunday"]),
                         ("maundythursday", ["maundythursday"]),
                         ("goodfriday", ["goodfriday"]),
                         ("eastersaturday", ["eastersaturday"]),
                         ("eastermonday", ["eastermonday"]),
                         ("ascensionday", ["ascensionday"]),
                         ("whitesunday", ["whitesunday", "pentecost", "pentecostday"]),
                         ("whitemonday", ["whitemonday"]),
                         ("trinitysunday", ["trinitysunday"]),
                         ("corpuschristi", ["corpuschristi"]),
                         ("earthday", ["earthday"]),
                         ("fathers", ["fatherday", "fathersday"]),
                         ("mothers", ["motherday", "mothersday"]),
                         ("thanksgiving", ["thanksgivingday", "thanksgiving"]),
                         ("blackfriday", ["blackfriday"]),
                         ("cybermonday", ["cybermonday"]),
                         ("martinlutherking", ["mlkday", "martinlutherkingday", "martinlutherkingjrday"]),
                         ("washingtonsbirthday", ["washingtonsbirthday", "washingtonbirthday", "presidentsday"]),
                         ("canberra", ["canberraday"]),
                         ("labour", ["labourday", "laborday"]),
                         ("columbus", ["columbusday"]),
                         ("memorial", ["memorialday"]),
                         ("yuandan", ["yuandan"]),
                         ("maosbirthday", ["maosbirthday"]),
                         ("teachersday", ["teachersday", "teacherday"]),
                         ("singleday", ["singleday"]),
                         ("allsaintsday", ["allsaintsday"]),
                         ("youthday", ["youthday"]),
                         ("childrenday", ["childrenday", "childday"]),
                         ("femaleday", ["femaleday"]),
                         ("treeplantingday", ["treeplantingday"]),
                         ("arborday", ["arborday"]),
                         ("girlsday", ["girlsday"]),
                         ("whiteloverday", ["whiteloverday"]),
                         ("loverday", ["loverday", "loversday"]),
                         ("christmas", ["christmasday", "christmas"]),
                         ("xmas", ["xmasday", "xmas"]),
                         ("newyear", ["newyear"]),
                         ("newyearday", ["newyearday"]),
                         ("newyearsday", ["newyearsday"]),
                         ("inaugurationday", ["inaugurationday"]),
                         ("groundhougday", ["groundhougday"]),
                         ("valentinesday", ["valentinesday"]),
                         ("stpatrickday", ["stpatrickday", "stpatricksday", "stpatrick"]),
                         ("aprilfools", ["aprilfools"]),
                         ("stgeorgeday", ["stgeorgeday"]),
                         ("mayday", ["mayday", "intlworkersday", "internationalworkersday", "workersday"]),
                         ("cincodemayoday", ["cincodemayoday"]),
                         ("baptisteday", ["baptisteday"]),
                         ("usindependenceday", ["usindependenceday"]),
                         ("independenceday", ["independenceday"]),
                         ("bastilleday", ["bastilleday"]),
                         ("halloweenday", ["halloweenday", "halloween"]),
                         ("allhallowday", ["allhallowday"]),
                         ("allsoulsday", ["allsoulsday"]),
                         ("guyfawkesday", ["guyfawkesday"]),
                         ("veteransday", ["veteransday"]),
                         ("christmaseve", ["christmaseve"]),
                         ("newyeareve", ["newyearseve", "newyeareve"]),
                         ("juneteenth", ["juneteenth", "freedomday", "jubileeday"])])
    WrittenDecades = dict([("hundreds", 0),
                           ("tens", 10),
                           ("twenties", 20),
                           ("thirties", 30),
                           ("forties", 40),
                           ("fifties", 50),
                           ("sixties", 60),
                           ("seventies", 70),
                           ("eighties", 80),
                           ("nineties", 90)])
    SpecialDecadeCases = dict([("noughties", 2000),
                               ("aughts", 2000),
                               ("two thousands", 2000)])
    DefaultLanguageFallback = 'MDY'
    SuperfluousWordList = [r'preferably', r'how about', r'maybe', r'perhaps', r'say', r'like']
    DurationDateRestrictions = [r'today', r'now']
    AmbiguityFiltersDict = dict([("^(morning|afternoon|evening|night|day)\\b", "\\b(good\\s+(morning|afternoon|evening|night|day))|(nighty\\s+night)\\b"),
                                 ("\\bnow\\b", "\\b(^now,)|\\b((is|are)\\s+now\\s+for|for\\s+now)\\b"),
                                 ("\\bmay\\b", "\\b((((!|\\.|\\?|,|;|)\\s+|^)may i)|(i|you|he|she|we|they)\\s+may|(may\\s+((((also|not|(also not)|well)\\s+)?(be|ask|contain|constitute|e-?mail|take|have|result|involve|get|work|reply|differ))|(or may not))))\\b"),
                                 ("\\b(a|one) second\\b", "\\b(?<!an?\\s+)(a|one) second (round|time)\\b"),
                                 ("\\b(breakfast|brunch|lunch(time)?|dinner(time)?|supper)$", "(?<!\\b(at|before|after|around|circa)\\b\\s)(breakfast|brunch|lunch|dinner|supper)(?!\\s*time)"),
                                 ("^\\d+m$", "^\\d+m$")])
    MorningTermList = [r'morning']
    AfternoonTermList = [r'afternoon']
    EveningTermList = [r'evening']
    MealtimeBreakfastTermList = [r'breakfast']
    MealtimeBrunchTermList = [r'brunch']
    MealtimeLunchTermList = [r'lunch', r'lunchtime']
    MealtimeDinnerTermList = [r'dinner', r'dinnertime', r'supper']
    DaytimeTermList = [r'daytime']
    NightTermList = [r'night']
    SameDayTerms = [r'today', r'otd']
    PlusOneDayTerms = [r'tomorrow', r'tmr', r'day after']
    MinusOneDayTerms = [r'yesterday', r'day before']
    PlusTwoDayTerms = [r'day after tomorrow', r'day after tmr']
    MinusTwoDayTerms = [r'day before yesterday']
    FutureTerms = [r'this', r'next']
    LastCardinalTerms = [r'الأخير']
    MonthTerms = [r'month']
    MonthToDateTerms = [r'month to date']
    WeekendTerms = [r'weekend']
    WeekTerms = [r'week']
    YearTerms = [r'year']
    GenericYearTerms = [r'y']
    YearToDateTerms = [r'year to date']
    DoubleMultiplierRegex = f'^(bi)(-|\\s)?'
    HalfMultiplierRegex = f'^(semi)(-|\\s)?'
    DayTypeRegex = f'((week)?da(il)?ys?)$'
    WeekTypeRegex = f'(week(s|ly)?)$'
    WeekendTypeRegex = f'(weekends?)$'
    MonthTypeRegex = f'(month(s|ly)?)$'
    QuarterTypeRegex = f'(quarter(s|ly)?)$'
    YearTypeRegex = f'((years?|annual)(ly)?)$'
# pylint: enable=line-too-long

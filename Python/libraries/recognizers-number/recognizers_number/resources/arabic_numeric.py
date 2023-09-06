from typing import Dict

from .base_numbers import BaseNumbers
# pylint: disable=line-too-long

class ArabicNumeric:
    LangMarker = 'Ara'
    CompoundNumberLanguage = False
    MultiDecimalSeparatorCulture = True
    RoundNumberIntegerRegex = r"(?:مائتان|مائة|مائة|مائتين|ثلاثمائه|أربعة مئة|خمسمائة|ستمائة|سبعمائة|ثمان مائة|تسعمائة|تريليون|ترليون|آلاف|تريليونين|تريليونات|مليار|ملياري|مليارات|مليون|مليونان|ملايين|ملايين|ألف|مليونين|ألفين|مئة|الف|ومائتين|الفين|بألفين|مئتان|الآف)"
    ZeroToNineIntegerRegex = r"(وخمسة|و خمسة|بإثنان|وواحد|و واحد|واحد|وأربعة|و أربعة|واثنان|اثنان|إثنان|وثلاثة|و ثلاثة|ثلاثة|واربعة|أربع|أربعة|خمسة|وستة|و ستة|ستة|وسبعة|و سبعة|سبعة|وثمانية|و ثمانية|ثمانية|ثمانٍ|وتسعة|و تسعة|تسع|أحد|اثني|إثني|ثلاث|صفر|سبع|ست|اربع|أربع|السادس|الثامنة|تسعة|اثنين|واحدُ|وإثنين|وواحدُ|الواحد:?)"
    TwoToNineIntegerRegex = r"(?:ثلاث|ثلاثة|سبعة|ثمان|ثمانية|أربع|أربعة|خمسة|تسعة|اثنان|اثنتان|اثنين|اثتنين|اثنتان|إثنان|إثنتان|إثنين|إثتنين|إثنتان|ست|ستة)"
    NegativeNumberTermsRegex = r"(?<negTerm>(سالب|ناقص)(\s+)?)"
    NegativeNumberSignRegex = f"^{NegativeNumberTermsRegex}.*"
    AnIntRegex = r"(واحد|أحد)(?=\s)"
    TenToNineteenIntegerRegex = r"(?:((ثلاث|ثلاثة|سبعة|ثمان|ثمانية|أربع|أربعة|خمسة|تسعة|اثنان|اثنان|اثنين|اثتنين|اثنتان|إثنان|إثنتان|إثنين|إثتنين|إثنتان|ستة|أحد|أربعة|إثني|اثني)\s(عشر|عشرة)))"
    TensNumberIntegerRegex = r"(عشرة|عشرون|ثلاثون|أربعون|خمسون|ستون|سبعون|ثمانون|تسعين|وعشرين|و عشرين|وثلاثين|و ثلاثين|وأربعين|و أربعين|وخمسين|و خمسين|وستين|وستين|وسبعين|و سبعين|وثمانين|و ثمانين|وتسعين|وتسعين|وعشرون|ثلاثون|وأربعون|و أربعون|وخمسون|و خمسون|وستون|و ستون|وسبعون|و سبعون|وثمانون|و ثمانون|وتسعون|و تسعون|عشرين|ثلاثين|أربعين|خمسين|ستين|سبعين|ثمانين|تسعون|العشرون:?)"
    SeparaIntRegex = fr"(?:((({RoundNumberIntegerRegex}\s{RoundNumberIntegerRegex})|{TenToNineteenIntegerRegex}|({ZeroToNineIntegerRegex}(((و)?)\s+(و)?|\s*-\s*){TensNumberIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|{RoundNumberIntegerRegex})(\s+{RoundNumberIntegerRegex})*))|(((\s+{RoundNumberIntegerRegex})+))"
    AllIntRegex = fr"(?:({SeparaIntRegex})((\s+(و)?)({SeparaIntRegex})(\s+{RoundNumberIntegerRegex})?)*|((({TenToNineteenIntegerRegex}|({TensNumberIntegerRegex}(\s+(و)?|\s*-\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex})?(\s+{RoundNumberIntegerRegex})+)\s+(و)?)*{SeparaIntRegex})"
    PlaceHolderPureNumber = r"\b"
    PlaceHolderDefault = r"\D|\b"

    @staticmethod
    def NumbersWithPlaceHolder(placeholder: str) -> str:
        return fr"(((?<!\d+\s*)([-]\s*)?)|(?<=\b))\d+(?!([\.،,]\d+[\u0621-\u064A]))(?={placeholder})"

    NumbersWithSuffix = fr"(((?<!\d+\s*)([-]\s*)?)|(?<=\b))\d+\s*{BaseNumbers.NumberMultiplierRegex}(?=\b)"
    RoundNumberIntegerRegexWithLocks = fr"(?<=\b)(\d+\s*({RoundNumberIntegerRegex})(\s|و\s|\sو))?\d+(\s|و\s|\sو)+{RoundNumberIntegerRegex}((\s*و\s*)+\d+)?(?=\b)"
    NumbersWithDozenSuffix = r"(((?<!\d+\s*)([-]\s*)?)|(?<=\b))(\d+\s+)?(دستة|دستات|دست|دزينة|دزينات|دزينتين)(?=\b)"
    AllIntRegexWithLocks = fr"((?<=\b){AllIntRegex}(?=\b))"
    AllIntRegexWithDozenSuffixLocks = fr"(?<=\b)(((نصف\s+)(دزينة|دستة|دستات|دست|دزينات|دزينتين))|({AllIntRegex}(و)?\s+((و)?))(دزينة|دستة|دستات|دست|دزينات|دزينتين))(?=\b)"
    RoundNumberOrdinalRegex = fr"(?:((من|على)\s+)({RoundNumberIntegerRegex}))"
    NumberOrdinalRegex = r"(اخماس|ثلثان|واحد جزء من|أجزاء من|المئتيان|مائتي|الحاديه عشر|سابعًا|خامسا|ثانيا|أول|الأول|الثاني|الثالث|الرابع|الخامس|السابع|الثامن|التاسع|الأولى|الثانية|الثالثة|الرابعة|الخامسة|السادسة|السابعة|التاسعة|السادس عشر|السابعة عشرة|السادسة عشرة|الثالثة عشرة|الحادية عشرة|السابع عشر|سادس عشر|الخامس عشر|الحادية عَشْرةَ|الثانيَ عَشَر|الثانيةَ عَشْرةَ|الثالثَ عَشَرَ|الثالثةَ عَشْرةَ|الرابعَ عَشَرَ|الرابعةَ عَشْرةَ|الخامِسَ عَشَرَ|الخامسةَ عَشْرةَ|السادِسَ عَشَرَ|السادسةَ عَشْرةَ|السابعَ عَشَرَ|السابعةَ عَشْرةَ|الثامنَ عَشَرَ|الثامنةَ عَشْرةَ|التاسعَ عَشَرَ|التاسعةَ عَشْرةَ|الحادِيَ عَشَرَ|الحادي عشر|الثاني عشر|الثالث عشر|الرابع عشر|الثامن عشر|التاسع عشر|الثانية عشرة|الرابعة عشرة|الخامسة عشرة|الثامنة عشرة|التاسعة عشرة|العاشر|العاشرة|عشرون|العشرين|الثلاثين|الثلاثون|الرابعة والأربعون|الرابع والأربعون|خمسون|الخمسون|الستين|ستون|والستين|سبعون|السبعون|والسبعون|ثامن عشر|الثامن عشر|الرابع والأربعين|الثامنة والثمانون|الثامن|والثمانين|وثلثان|ثمن|أثمان|التاسع والتسعون|التاسعة والتسعون|اثمان|خمس|أخماس|وثلاثون|ثلثان|الأخماس|اخماس|ثلثان|واحد جزء من|العشرون|التريليون|الواحد والعشرون|العشرين|الحادي والعشرين|الثاني والعشرين|الثالث والعشرين|الرابع والعشرين|الخامس والعشرين|السادس والعشرين|السابع والعشرين|الثامن والعشرين|التاسع والعشرين|الثلاثين|الحادي والثلاثين|الخامسة والعشرون:?)"
    RelativeOrdinalRegex = r"(?<relativeOrdinal>(الواحد\s)?((السابق|السابقة|الثانية الى|((الذي)\s*(قبل|قبلا)\s*)?(الأخير)|قبل|بعد|سبق|سبقت|التالي|الحالي|الذي|اخر)(\s))?((تالي|الحالي|السابقة|سابق|قادم|التالي|((الذي)\s*(قبل|قبلا)\s*)?(الأخير)|آخر|أخير|حالي|اخر|الاخير|الأولى)(ة)?)|(الاخر|الاول|الأول|اول|الأولى|((الذي)\s*(قبل|قبلا)\s*)?(الأخير)|السابق|التالي|أخر))"
    BasicOrdinalRegex = fr"({NumberOrdinalRegex}|{RelativeOrdinalRegex})"
    SuffixBasicOrdinalRegex = fr"(?:(((({TensNumberIntegerRegex}(\s+(و)?|\s*){ZeroToNineIntegerRegex})|{TensNumberIntegerRegex}|{ZeroToNineIntegerRegex}|({RoundNumberIntegerRegex}|المئة(\s+(و)?)))((\s+{RoundNumberIntegerRegex}|المئة)+|({BasicOrdinalRegex})))\s+(و)?)*({TensNumberIntegerRegex}(\s+|\s*))?{BasicOrdinalRegex}|({TensNumberIntegerRegex}))"
    SuffixRoundNumberOrdinalRegex = fr"(?:({AllIntRegex}\s+){RoundNumberOrdinalRegex})"
    AllOrdinalRegex = fr"(?:{SuffixBasicOrdinalRegex}|{SuffixRoundNumberOrdinalRegex})"
    OrdinalNumericRegex = fr"(?<=\b)(?:\d{1,3}(\s*,\s*\d{3})*\s*th)(?=\b)"
    OrdinalRoundNumberRegex = fr"({RoundNumberOrdinalRegex})"
    OrdinalEnglishRegex = fr"(?<=\b){AllOrdinalRegex}(?=\b)"
    FractionNotationWithSpacesRegex = r"(((?<={?[\u0600-\u06ff]}|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))"
    FractionNotationWithSpacesRegex2 = r"(((?<={?[\u0600-\u06ff]}|^)-\s*)|(?<![/-])(?<=\b))\d+[/]\d+(?=(\b[^/]|$))(\s*\d+)"
    FractionNotationRegex = "(((?<={?[\u0600-\u06ff]}|^)-\s*)|(?<![/-])(?<=\b))\d+[/]\d+(?=(\b[^/]|$))"
    ArabicBuiltInFraction = r"(ثلثان|ربع|خمس|عشرونات|ثلاثون|خُمسَين:?)"
    FractionOrdinalPrefix = r"(الوزن|المحتوى:?)"
    FractionNounRegex = fr"(?<=\b){ArabicBuiltInFraction}|{AllIntRegex}\s(و\s|و){ArabicBuiltInFraction}|(({AllIntRegex}\s(و\s|و)?)?({AllIntRegex})(\s+|\s*)(({AllOrdinalRegex})|({RoundNumberOrdinalRegex})|أرباع|وربع|ارباع|واحد وربع|نصف|ربع|أنصاف|ربعين|أرباع|ارباع))(?=\b)"
    FractionNounWithArticleRegex = fr"(?<=\b)((({AllIntRegex}(\s|(\s*-\s*)|و\s+)?)(({AllOrdinalRegex})|{NumberOrdinalRegex}|نصف|وربع|ربع|ونصف))|(الربع|النصف|نصف|))(?=\b)"
    FractionPrepositionRegex = fr"(?<!{BaseNumbers.CommonCurrencySymbol}\s*)(?<=\b)(?<numerator>({AllIntRegex})|((?<![\.,])\d+))\s+(فوق|على|في|جزء|من|أجزاء من|اجزاء من|جزء من)\s+(?<denominator>({AllIntRegex})|(\d+)(?![\.,]))(?=\b)"
    FractionPrepositionWithinPercentModeRegex = "(?<!{BaseNumbers.CommonCurrencySymbol}\s*)(?<=\b)(?<numerator>({AllIntRegex})|((?<![\.,])\d+))\s+على\s+(?<denominator>({AllIntRegex})|(\d+)(?![\.,]))(?=\b)"
    FractionWithOrdinalPrefix = fr"({AllOrdinalRegex})(?=\s*({FractionOrdinalPrefix}))"
    FractionWithPartOfPrefix = fr"((جزء من)\s+)({AllIntRegexWithLocks})"
    AllPointRegex = fr"((\s+{ZeroToNineIntegerRegex})+|(\s+{SeparaIntRegex}))"
    AllFloatRegex = fr"{AllIntRegex}(\s+(نقطة|جزء|جزء من)){AllPointRegex}"
    DoubleWithMultiplierRegex = fr"(((?<!\d+\s*)([-]\s*)?)|((?<=\b)(?<!\d+[\.,])))\d+\u202A?[\.,]\u202A?\d+\s*{BaseNumbers.NumberMultiplierRegex}(?=\b)"
    DoubleExponentialNotationRegex = r"(((?<!\d+\s*)([-]\s*)?)|((?<=\b)(?<!\d+[\.,])))(\d+(\u202A?[\.,]\u202A?\d+)?)e([+-]*[\u0660-\u0669]\d*)(?=\b)"
    DoubleCaretExponentialNotationRegex = r"(((?<!\d+\s*)([-]\s*)?)|((?<=\b)(?<!\d+[\.,])))(\d+(\u202A?[\.,]\u202A?\d+)?)[+-]*\^([+-]*[\u0660-\u0669]([\.,])?\d*)(?=\b)"

    @staticmethod
    def DoubleDecimalPointRegex(placeholder) -> str:
        return fr"(((?<!\d+\s*)([-]\s*)?)|((?<=\b)(?<!\d+[\.,])))((?<!\d.)(\d+\u202A?[\.,]\u202A?\d+))(?!([\.,]\d+))(?={placeholder})"

    @staticmethod
    def DoubleWithoutIntegralRegex(placeholder) -> str:
        return fr"(?<=\s|^)(?<!(\d+))\u202A?[\.,]\u202A?\d+(?!([\.,]\d+))(?={placeholder})"

    DoubleWithRoundNumber = fr"(((?<!\d+\s*)([-]\s*)?)|((?<=\b)(?<!\d+[\.,])))\d+\u202A?[\.,]\u202A?\d+\s+{RoundNumberIntegerRegex}(?=\b)"

    @staticmethod
    def DoubleWithThousandMarkRegex(placeholder) -> str:
        return fr"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.|\d+,)))\d{{1,3}}(\u202A?[،]\u202A?\d{{3}})+\u202A?[\.,]\u202A?\d+(?={placeholder})"

    DoubleAllFloatRegex = fr"((?<=\b){AllFloatRegex}(?=\b))"
    ConnectorRegex = r"(?<spacer>و)"
    NumberWithSuffixPercentage = fr"((?<!(٪|%))({BaseNumbers.NumberReplaceToken})(\s*)((٪|%)(?!{BaseNumbers.NumberReplaceToken})|(بالمائة|في المئة|بالمئة)))"
    FractionNumberWithSuffixPercentage = fr"(({BaseNumbers.FractionNumberReplaceToken})\s+(من|في المئة))"
    NumberWithPrefixPercentage = fr"(نسبة|بالمائة)(\s*)({BaseNumbers.NumberReplaceToken})"
    NumberWithPrepositionPercentage = fr"({BaseNumbers.NumberReplaceToken})\s*(في|خارج\s+من)\s*({BaseNumbers.NumberReplaceToken})"
    TillRegex = r"(الى|إلى|خلال|--|-|—|——|~|–)"
    MoreRegex = r"(?:(اكثر|فوق|أكبر|أعظم|أطول|يتجاوز|تفوق|أعلى|أكثر)|(?<!<|=)>)"
    LessRegex = r"(?:(أقل|اقل|اصغر|أصغر|أخفض|ادنى)(\s*من)?|تحت|(?<!>|=)<)"
    EqualRegex = r"(يساوي|(?<!<|>)=)"
    MoreOrEqualPrefix = fr"(((ليس|لا)\s+{LessRegex})|(على\s+الأقل))"
    MoreOrEqual = fr"(?:(({MoreRegex}(\s+من)?)\s+(أو|او)?\s+{EqualRegex})|(({MoreOrEqualPrefix}|(تفوق))(\s+(أو|او)?\s+{EqualRegex})?)|(({EqualRegex}\s+(أو|او)?\s+)?({MoreOrEqualPrefix}|تفوق))|>\s*=)"
    MoreOrEqualSuffix = r"((أو|او)\s+(((أكبر|أعظم|أطول|فوق|اكثر|اكثر|اكبر|أكثر)((?!\s+من)|(\s+من(?!(\s*\d+)))))|((فوق|أكبر|أطول|اكثر)(?!\s+من))))"
    LessOrEqualPrefix = fr"((ليس\s+{MoreRegex})|(at\s+most)|(بحد أقصى)|(يصل الى))"
    LessOrEqual = fr"(((لا\s*)?{LessRegex}\s+(أو|او)?\s+{EqualRegex})|({EqualRegex}\s+(أو|او)?\s+(((أقل|اقل|أدنى|اصغر|أصغر|ادنى)(\s+من))|تحت|(?<!>|=)<))|({LessOrEqualPrefix}(\s+(أو|او)?\s+{EqualRegex})?)|(({EqualRegex}\s+(أو|او)?\s+)?{LessOrEqualPrefix})|<\s*=)"
    LessOrEqualSuffix = r"((أ|ا)?و\s+(أقل)((?!\s+من)|(\s+من(?!(\s*\d+)))))"
    NumberSplitMark = r"(?![.،](?!\d+))"
    MoreRegexNoNumberSucceed = r"((أكبر|أعظم|أطول|فوق|اكثر)((?!\s+من)|\s+(من(?!(\s*\d+))))|(فوق|أكبر|أعظم)(?!(\s*\d+)))"
    LessRegexNoNumberSucceed = r"((أقل|أصغر)((?!\s+من)|\s+(من(?!(\s*\d+))))|(تحت|اقل|أقل|أصغر)(?!((\s*\d+)|\s*من)))"
    EqualRegexNoNumberSucceed = r"((يساوي)(?!(\s*\d+)))"
    OneNumberRangeMoreRegex1 = r"({MoreOrEqual})\s*(ال)?(?<number1>({NumberSplitMark}.)+)|({EqualRegex}\s*(أو|او)?\s+({MoreRegex}))(\s+(من))\s*(?<number1>({NumberSplitMark}.)+)|({EqualRegex}\s+(أو|او)?\s+({MoreRegex}))\s*(?<number1>({NumberSplitMark}.)+)|({MoreRegex})(\s+(من))\s*(?<number1>({NumberSplitMark}.)+)|({MoreRegex})\s*(?<number1>({NumberSplitMark}.)+)"
    OneNumberRangeMoreRegex3 = fr"(?<number1>({NumberSplitMark}.)+)\s*(و|أو)\s*({MoreRegex})"
    OneNumberRangeMoreRegex2 = fr"(?<number1>({NumberSplitMark}.)+)\s*{MoreOrEqualSuffix}"
    OneNumberRangeMoreSeparateRegex = fr"({MoreRegex}\s*(من)\s+(?<number1>({NumberSplitMark}.)+)\s+(أو|او)\s+{EqualRegexNoNumberSucceed})|({EqualRegex}\s+(?<number1>({NumberSplitMark}.)+)(\s+(أو|او)\s+){MoreRegexNoNumberSucceed})|({MoreRegex}\s+(?<number1>({NumberSplitMark}.)+)(\s+(أو|او)\s+){EqualRegexNoNumberSucceed})"
    OneNumberRangeLessRegex1 = fr"(({LessOrEqual})\s*(ال)?(?<number2>({NumberSplitMark}.)+))|(لا\s*)?((((أقل|اقل|أدنى|اصغر|أصغر|ادنى)(\s+من))|تحت|(?<!>|=)<))\s*(ال)?(?<number2>({NumberSplitMark}.)+)|(لا\s*)?(({LessRegex})\s*(ال)?(?<number2>({NumberSplitMark}.)+))"
    OneNumberRangeLessRegex2 = fr"(?<number2>({NumberSplitMark}.)+)\s*{LessOrEqualSuffix}"
    OneNumberRangeLessSeparateRegex = fr"({EqualRegex}\s+(?<number1>({NumberSplitMark}.)+)\s*(أو|او)\s+{LessRegexNoNumberSucceed})|(((((أقل|اقل|أدنى|اصغر|أصغر|ادنى)(\s+من))|تحت|(?<!>|=)<))\s+(?<number1>({NumberSplitMark}.)+)(\s+(أو|او)\s+){EqualRegexNoNumberSucceed})"
    OneNumberRangeEqualRegex = fr"{EqualRegex}\s*(ال)?(?<number1>({NumberSplitMark}.)+)"
    TwoNumberRangeRegex1 = fr"بين\s*(ال)?(?<number1>({NumberSplitMark}.)+)\s*و\s*(ال)?(?<number2>({NumberSplitMark}.)+)"
    TwoNumberRangeRegex2 = fr"({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})\s*(،)?\s*((أ|ا)?و|لكن|,)\s*({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})"
    TwoNumberRangeRegex3 = fr"({OneNumberRangeLessRegex1}|{OneNumberRangeLessRegex2})\s*(،)?\s*((أ|ا)?و|لكن|,)\s*({OneNumberRangeMoreRegex1}|{OneNumberRangeMoreRegex2})"
    TwoNumberRangeRegex4 = fr"((من\s)(?<number1>({NumberSplitMark}(?!\bمن\b).)+)\s*{TillRegex}\s*(ال\s+)?(?<number2>({NumberSplitMark}.)+))|((من\s)?(?<number1>({NumberSplitMark}(?!\bمن\b).)+)\s*{TillRegex}\s*(ال\s+)?(?<number2>({NumberSplitMark}.)+))"
    AmbiguousFractionConnectorsRegex = r"(\bمن|بين|من|بين\b)"
    DecimalSeparatorChar = ','
    FractionMarkerToken = r"أكثر"
    NonDecimalSeparatorChar = '،'
    HalfADozenText = r"ستة"
    WordSeparatorToken = r"و"
    WrittenDecimalSeparatorTexts = [r"نقطة | فاصلة"]
    WrittenGroupSeparatorTexts = [r"punto"]
    WrittenIntegerSeparatorTexts = [r"و"]
    WrittenFractionSeparatorTexts = [r"و"]
    HalfADozenRegex = r"نصف?\sدستة"
    DigitalNumberRegex = fr"((?<=\b)(مائة|مائتان|دست|دستات|ألف|ألفين|مائتين|ألفين|ثلاثمائة|أربعمائة|خمسمائة|ستمائة|سبعمائة|تسعمائة|ثمانمائة|مليون|مليار|ترليون)(?=\b))|((?<=(\d|\b)){BaseNumbers.MultiplierLookupRegex}(?=\b))"
    CardinalNumberMap: Dict[str, int] = {
        r"واحد": 1,
        r"صفر": 0,
        r"اثنان": 2,
        r"اثنين": 2,
        r"ثلاث": 3,
        r"ثلاثة": 3,
        r"أربعة": 4,
        r"خمسة": 5,
        r"ستة": 6,
        r"سبعة": 7,
        r"ثمانية": 8,
        r"تسعة": 9,
        r"عشرة": 10,
        r"إحدى عشر": 11,
        r"اثنى عشر": 12,
        r"دستة": 12,
        r"دستات": 12,
        r"ثلاثة عشر": 13,
        r"أربعة عشر": 14,
        r"خمسة عشر": 15,
        r"ستة عشر": 16,
        r"سبعة عشر": 17,
        r"ثمانية عشر": 18,
        r"تسعة عشر": 19,
        r"عشرون": 20,
        r"وعشرون": 20,
        r"ثلاثون": 30,
        r"وثلاثون": 30,
        r"أربعون": 40,
        r"وأربعون": 40,
        r"خمسون": 50,
        r"وخمسون": 50,
        r"ستون": 60,
        r"وستون": 60,
        r"سبعون": 70,
        r"وسبعون": 70,
        r"ثمانون": 80,
        r"وثمانون": 80,
        r"تسعون": 90,
        r"وتسعون": 90,
        r"مائة": 100,
        r"ومائة": 100,
        r"مائتان": 200,
        r"ومائتان": 200,
        r"مائتين": 200,
        r"ومائتين": 200,
        r"ثلاثمائة": 300,
        r"وثلاثمائة": 300,
        r"أربعمائة": 400,
        r"وأربعمائة": 400,
        r"خمسمائة": 500,
        r"وخمسمائة": 500,
        r"ستمائة": 600,
        r"وستمائة": 600,
        r"سبعمائة": 700,
        r"وسبعمائة": 700,
        r"ثمانمائة": 800,
        r"وثمانمائة": 800,
        r"تسعمائة": 900,
        r"وتسعمائة": 900,
        r"ألف": 1000,
        r"آلاف": 1000,
        r"ألفين": 2000,
        r"ألفان": 2000,
        r"المليون": 1000000,
        r"مليون": 1000000,
        r"مليار": 1000000000,
        r"المليار": 1000000000,
        r"تريليون": 1000000000000,
        r"التريليون": 1000000000000,
        r"الواحد": 1,
        r"الصفر": 0,
        r"الاثنان": 2,
        r"الاثنين": 2,
        r"الثلاثة": 3,
        r"الأربعة": 4,
        r"الخمسة": 5,
        r"الستة": 6,
        r"السبعة": 7,
        r"الثمانية": 8,
        r"التسعة": 9,
        r"العشرة": 10,
        r"الإحدى عشر": 11,
        r"الاثنى عشر": 12,
        r"الدستة": 12,
        r"الدستات": 12,
        r"الثلاثة عشر": 13,
        r"الأربعة عشر": 14,
        r"الخمسة عشر": 15,
        r"الستة عشر": 16,
        r"السبعة عشر": 17,
        r"الثمانية عشر": 18,
        r"التسعة عشر": 19,
        r"العشرون": 20,
        r"الثلاثون": 30,
        r"الأربعون": 40,
        r"الخمسون": 50,
        r"الستون": 60,
        r"السبعون": 70,
        r"الثمانون": 80,
        r"التسعون": 90,
        r"المائة": 100,
        r"المائتين": 200,
        r"المائتان": 200,
        r"الثلاثمائة": 300,
        r"الأربعمائة": 400,
        r"الخمسمائة": 500,
        r"الستمائة": 600,
        r"السبعمائة": 700,
        r"الثمانمائة": 800,
        r"التسعمائة": 900,
        r"الألف": 1000,
        r"الآلاف": 1000,
        r"الألفين": 2000
    }

    OrdinalNumberMap: Dict[str, int] = {
        r"أول": 1,
        r"أولى": 1,
        r"الأول": 1,
        r"الأولى": 1,
        r"ثاني": 2,
        r"ثانية": 2,
        r"الثاني": 2,
        r"الثانية": 2,
        r"ثان": 2,
        r"النصف": 2,
        r"نصف": 2,
        r"ثلث": 3,
        r"الثالث": 3,
        r"الثالثة": 3,
        r"ثالث": 3,
        r"ثالثة": 3,
        r"الربع": 4,
        r"ربع": 4,
        r"الرابع": 4,
        r"الرابعة": 4,
        r"رابع": 4,
        r"رابعة": 4,
        r"خمس": 5,
        r"الخامس": 5,
        r"الخامسة": 5,
        r"خامس": 5,
        r"خامسة": 5,
        r"سدس": 6,
        r"السادس": 6,
        r"السادسة": 6,
        r"سادس": 6,
        r"سادسة": 6,
        r"سبع": 7,
        r"السابع": 7,
        r"السابعة": 7,
        r"سابع": 7,
        r"سابعة": 7,
        r"ثمن": 8,
        r"الثامن": 8,
        r"الثامنة": 8,
        r"ثامن": 8,
        r"ثامنة": 8,
        r"تسع": 9,
        r"التاسع": 9,
        r"التاسعة": 10,
        r"تاسع": 9,
        r"تاسعة": 9,
        r"واحد من عشرة": 10,
        r"العاشر": 10,
        r"واحد من إحدى عشر": 11,
        r"الحادية عشرة": 11,
        r"الحادي عشر": 11,
        r"واحد من إثنى عشر": 12,
        r"الثانية عشرة": 12,
        r"الثاني عشر": 12,
        r"واحد من ثلاثة عشر": 13,
        r"الثالثة عشرة": 13,
        r"الثالث عشر": 13,
        r"واحد من أربعة عشر": 14,
        r"الرابعة عشرة": 14,
        r"الرابع عشر": 14,
        r"واحد من خمسة عشر": 15,
        r"الخامسة عشرة": 15,
        r"الخامس عشر": 15,
        r"واحد من ستة عشر": 16,
        r"السادسة عشرة": 16,
        r"السادس عشر": 16,
        r"واحد من سبعة عشر": 17,
        r"السابعة عشرة": 17,
        r"السابع عشر": 17,
        r"واحد من ثمانية عشر": 18,
        r"الثامنة عشرة": 18,
        r"الثامن عشر": 18,
        r"واحد من تسعة عشر": 19,
        r"التاسع عشر": 19,
        r"واحد من عشرين": 20,
        r"العشرون": 20,
        r"العشرين": 20,
        r"الحادي والعشرين": 21,
        r"الثاني والعشرين": 22,
        r"الثالث والعشرين": 23,
        r"الرابع والعشرين": 24,
        r"الخامس والعشرين": 25,
        r"السادس والعشرين": 26,
        r"السابع والعشرين": 27,
        r"الثامن والعشرين": 28,
        r"التاسع والعشرين": 29,
        r"واحد من ثلاثين": 30,
        r"الثلاثون": 30,
        r"الثلاثين": 30,
        r"الحادي والثلاثين": 31,
        r"واحد من أربعين": 40,
        r"الأربعون": 40,
        r"الأربعين": 40,
        r"واحد من خمسين": 50,
        r"الخمسون": 50,
        r"الخمسين": 50,
        r"واحد من ستين": 60,
        r"الستون": 60,
        r"الستين": 60,
        r"واحد من سبعين": 70,
        r"السبعون": 70,
        r"السبعين": 70,
        r"واحد من ثمانين": 80,
        r"الثمانون": 80,
        r"الثمانين": 80,
        r"واحد من تسعين": 90,
        r"التسعون": 90,
        r"التسعين": 90,
        r"واحد من مائة": 100,
        r"المائة": 100,
        r"المائتان": 200,
        r"المائتين": 200,
        r"الثلاثمائة": 300,
        r"الأربعمائة": 400,
        r"الخمسمائة": 500,
        r"الستمائة": 600,
        r"السبعمائة": 700,
        r"الثمانمائة": 800,
        r"التسعمائة": 100,
        r"الألف": 1000,
        r"واحد من ألف": 1000,
        r"واحد من مليون": 1000000,
        r"المليون": 1000000,
        r"واحد من مليار": 1000000000,
        r"المليار": 1000000000,
        r"واحد من تريليون": 1000000000000,
        r"التريليون": 1000000000000,
        r"أوائل": 1,
        r"أنصاف": 2,
        r"أثلاث": 3,
        r"أرباع": 4,
        r"أخماس": 5,
        r"أسداس": 6,
        r"أسباع": 7,
        r"أثمان": 8,
        r"أتساع": 9,
        r"أعشار": 10,
        r"عشرينات": 20,
        r"ثلاثينات": 30,
        r"أربعينات": 40,
        r"خمسينات": 50,
        r"ستينات": 60,
        r"سبعينات": 70,
        r"ثمانينات": 80,
        r"تسعينات": 90,
        r"مئات": 100,
        r"ألوف": 1000,
        r"ملايين": 1000000,
        r"مليارات": 1000000000,
        r"تريليون": 1000000000000
    }

    RoundNumberMap: Dict[str, int] = {
        r"ترليون": 1000000000000,
        r"مائة": 100,
        r"ألف": 1000,
        r"مليون": 1000000,
        r"مليار": 1000000000,
        r"تريليون": 1000000000000,
        r"مائتين": 200,
        r"مائتان": 200,
        r"ثلاثمائة": 300,
        r"أربعمائة": 400,
        r"خمسمائة": 500,
        r"ستمائة": 600,
        r"سبعمائة": 700,
        r"ثمانمائة": 800,
        r"تسعمائة": 900,
        r"ألفين": 2000,
        r"دستة": 12,
        r"دستات": 12,
        r"المائة": 100,
        r"الألف": 1000,
        r"المليون": 1000000,
        r"المليار": 1000000000,
        r"التريليون": 1000000000000,
        r"المائتين": 200,
        r"المائتان": 200,
        r"الثلاثمائة": 300,
        r"الأربعمائة": 400,
        r"الخمسمائة": 500,
        r"الستمائة": 600,
        r"السبعمائة": 700,
        r"الثمانمائة": 800,
        r"التسعمائة": 900,
        r"الألفين": 2000,
        r"الدستة": 12,
        r"الدستات": 12
    }

    AmbiguityFiltersDict: Dict[str, str] = {
        r"\bواحد\b": r"\b(الذي|هذا|ذلك|ذاك|أي)\s+(واحد)\b"
    }

    RelativeReferenceOffsetMap: Dict[str, str] = {
        r"الاخر": "0",
        r"آخر": "0",
        r"اخر": "0",
        r"الأخيرة": "0",
        r"الأخير": "0",
        r"سبقت الأخيرة": "-1",
        r"سبقت الأخير": "-1",
        r"قبل الأخير": "-1",
        r"قبل الأخيرة": "-1",
        r"القبل الأخير": "-1",
        r"قبلا الأخي": "-1",
        r"التالي": "1",
        r"بعد التالي": "2",
        r"قادم": "1",
        r"قادمة": "1",
        r"القادم": "1",
        r"القادمة": "1",
        r"السابقة": "-1",
        r"الحالي": "0",
        r"الحالية": "0",
        r"قبل الاخير": "-1",
        r"الواحد قبل الاخير": "-1",
        r"الثانية الى الاخير": "-1",
        r"الذي قبلا الأخير": "-1",
        r"الذي قبل الأخير": "-1",
        r"الذي قبلا الأخي": "-1",
        r"السابق": "-1",
        r"أخر": "0",
        r"الاخير": "0",
        r"اول": "1",
        r"الاول": "1",
        r"التالية": "-1"
    }

    RelativeReferenceRelativeToMap: Dict[str, str] = {
        r"اول": "current",
        r"التالية": "current",
        r"الاول": "current",
        r"الاخر": "end",
        r"الاخير": "end",
        r"أخر": "end",
        r"آخر": "end",
        r"اخر": "end",
        r"الأخيرة": "end",
        r"الأخير": "end",
        r"سبقت الأخيرة": "current",
        r"سبقت الأخير": "current",
        r"قبل الأخير": "end",
        r"قبل الأخيرة": "current",
        r"القبل الأخير": "current",
        r"الذي قبلا الأخي": "end",
        r"التالي": "current",
        r"بعد التالي": "current",
        r"قادم": "current",
        r"قادمة": "current",
        r"القادم": "current",
        r"القادمة": "current",
        r"السابقة": "current",
        r"الحالي": "current",
        r"قبلا الأخي": "current",
        r"الحالية": "end",
        r"قبل الاخير": "end",
        r"الواحد قبل الاخير": "end",
        r"الذي قبل الأخير": "end",
        r"الذي قبلا الأخير": "end",
        r"الثانية الى الاخير": "end",
        r"السابق": "current"
    }

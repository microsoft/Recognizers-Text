import { CommonNumeric } from "./commonNumeric";

export class EnglishNumericResources {
    // Integer Regex
    static readonly LangMarker = String.raw`Eng`;
    static readonly RoundNumberIntegerRegex = String.raw`(hundred|thousand|million|billion|trillion)`;
    static readonly ZeroToNineIntegerRegex = String.raw`(three|seven|eight|four|five|zero|nine|one|two|six)`;
    static readonly AnIntRegex = String.raw`(an|a)(?=\s)`;
    static readonly TenToNineteenIntegerRegex = String.raw`(seventeen|thirteen|fourteen|eighteen|nineteen|fifteen|sixteen|eleven|twelve|ten)`;
    static readonly TensNumberIntegerRegex = String.raw`(seventy|twenty|thirty|eighty|ninety|forty|fifty|sixty)`;
    static readonly SeparaIntRegex = String.raw`(((${EnglishNumericResources.TenToNineteenIntegerRegex}|(${EnglishNumericResources.TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*)${EnglishNumericResources.ZeroToNineIntegerRegex})|${EnglishNumericResources.TensNumberIntegerRegex}|${EnglishNumericResources.ZeroToNineIntegerRegex})(\s+${EnglishNumericResources.RoundNumberIntegerRegex})*))|((${EnglishNumericResources.AnIntRegex}(\s+${EnglishNumericResources.RoundNumberIntegerRegex})+))`;
    static readonly AllIntRegex = String.raw`((((${EnglishNumericResources.TenToNineteenIntegerRegex}|(${EnglishNumericResources.TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*)${EnglishNumericResources.ZeroToNineIntegerRegex})|${EnglishNumericResources.TensNumberIntegerRegex}|${EnglishNumericResources.ZeroToNineIntegerRegex}|${EnglishNumericResources.AnIntRegex})(\s+${EnglishNumericResources.RoundNumberIntegerRegex})+)\s+(and\s+)?)*${EnglishNumericResources.SeparaIntRegex})`;
    static readonly PlaceHolderPureNumber = String.raw`(?=\b)`;
    static readonly PlaceHolderDefault = String.raw`\D|\b`;

    // static readonly NumbersWithPlaceHolder = (placeholder: string): string => String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d+(?!(\.\d+[a-zA-Z]))(?=${placeholder})`;
    static readonly NumbersWithPlaceHolder = (placeholder: string): string => String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d+(?!(\.\d+[a-zA-Z]))(?=${placeholder})`;
    // static readonly DottedNumbersWithPlaceHolder = (placeholder: string): string => String.raw`(((?<!\d+\s*)-\s*)|(?<=\b))\d{1,3}(,\d{3})+(?=${placeholder})`;
    static readonly DottedNumbersWithPlaceHolder = (placeholder: string): string => String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d{1,3}($0$\d{3})+(?=${placeholder})`;
    // static readonly NumbersWithSufix = String.raw`(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s*(K|k|M|T|G)(?=\b)`;
    static readonly NumbersWithSufix = String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d+\s*(K|k|M|T|G)(?=\b)`;
    // static readonly RoundNumberIntegerRegexWithLocks = String.raw`(?<=\b)\d+\s+${EnglishNumericResources.RoundNumberIntegerRegex}(?=\b)`;
    static readonly RoundNumberIntegerRegexWithLocks = String.raw`(?=\b)\d+\s+${EnglishNumericResources.RoundNumberIntegerRegex}(?=\b)`;
    // static readonly NumbersWithDozenSufix = String.raw`(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s+dozen(s)?(?=\b)`;
    static readonly NumbersWithDozenSufix = String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d+\s+dozen(s)?(?=\b)`;
    // static readonly AllIntRegexWithLocks = String.raw`((?<=\b)${EnglishNumericResources.AllIntRegex}(?=\b))`;
    static readonly AllIntRegexWithLocks = String.raw`((?=\b)${EnglishNumericResources.AllIntRegex}(?=\b))`;
    // static readonly AllIntRegexWithDozenSufixLocks = String.raw`(?<=\b)(((half\s+)?a\s+dozen)|(${EnglishNumericResources.AllIntRegex}\s+dozen(s)?))(?=\b)`;
    static readonly AllIntRegexWithDozenSufixLocks = String.raw`(?=\b)(((half\s+)?a\s+dozen)|(${EnglishNumericResources.AllIntRegex}\s+dozen(s)?))(?=\b)`;

    // Ordinal Regex
    static readonly RoundNumberOrdinalRegex = String.raw`(hundredth|thousandth|millionth|billionth|trillionth)`;
    static readonly BasicOrdinalRegex = String.raw`(first|second|third|fourth|fifth|sixth|seventh|eighth|ninth|tenth|eleventh|twelfth|thirteenth|fourteenth|fifteenth|sixteenth|seventeenth|eighteenth|nineteenth|twentieth|thirtieth|fortieth|fiftieth|sixtieth|seventieth|eightieth|ninetieth)`;
    static readonly SuffixBasicOrdinalRegex
    = String.raw`(((((${EnglishNumericResources.TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*)${
        EnglishNumericResources.ZeroToNineIntegerRegex})|${EnglishNumericResources.TensNumberIntegerRegex}|${EnglishNumericResources.ZeroToNineIntegerRegex}|${EnglishNumericResources.AnIntRegex
        })(\s+${EnglishNumericResources.RoundNumberIntegerRegex})+)\s+(and\s+)?)*(${EnglishNumericResources.TensNumberIntegerRegex
        }(\s+|\s*-\s*))?${EnglishNumericResources.BasicOrdinalRegex})`;
    static readonly SuffixRoundNumberOrdinalRegex = String.raw`((${EnglishNumericResources.AllIntRegex}\s+)${EnglishNumericResources.RoundNumberOrdinalRegex})`;
    static readonly AllOrdinalRegex = String.raw`(${EnglishNumericResources.SuffixBasicOrdinalRegex}|${EnglishNumericResources.SuffixRoundNumberOrdinalRegex})`;
    // static readonly OrdinalSuffixRegex = String.raw`(?<=\b)((\d*(1st|2nd|3rd|4th|5th|6th|7th|8th|9th|0th))|(11th|12th))(?=\b)`;
    static readonly OrdinalSuffixRegex = String.raw`(?=\b)((\d*(1st|2nd|3rd|4th|5th|6th|7th|8th|9th|0th))|(11th|12th))(?=\b)`;
    // static readonly OrdinalNumericRegex = String.raw`(?<=\b)(\d{1,3}(\s*,\s*\d{3})*\s*th)(?=\b)`;
    static readonly OrdinalNumericRegex = String.raw`(?=\b)(\d{1,3}(\s*,\s*\d{3})*\s*th)(?=\b)`;
    // static readonly OrdinalRoundNumberRegex = String.raw`(?<!(a|an)\s+)${EnglishNumericResources.RoundNumberOrdinalRegex}`;
    static readonly OrdinalRoundNumberRegex = String.raw`(?!(a|an)\s+)${EnglishNumericResources.RoundNumberOrdinalRegex}`;
    // static readonly OrdinalEnglishRegex = String.raw`(?<=\b)${EnglishNumericResources.AllOrdinalRegex}(?=\b)`;
    static readonly OrdinalEnglishRegex = String.raw`(?=\b)${EnglishNumericResources.AllOrdinalRegex}(?=\b)`;

    // Fraction Regex
    // static readonly FractionNotationWithSpacesRegex = String.raw`(((?<=\W|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))`;
    static readonly FractionNotationWithSpacesRegex = String.raw`(((?=\W|^)-\s*)|(?=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))`;
    // static readonly FractionNotationRegex = String.raw`(((?<=\W|^)-\s*)|(?<=\b))\d+[/]\d+(?=(\b[^/]|$))`;
    static readonly FractionNotationRegex = String.raw`(((?=\W|^)-\s*)|(?=\b))\d+[/]\d+(?=(\b[^/]|$))`;
    // static readonly FractionNounRegex = String.raw`(?<=\b)(${EnglishNumericResources.AllIntRegex}\s+(and\s+)?)?(${EnglishNumericResources.AllIntRegex
    static readonly FractionNounRegex = String.raw`(?=\b)(${EnglishNumericResources.AllIntRegex}\s+(and\s+)?)?(${EnglishNumericResources.AllIntRegex})(\s+|\s*-\s*)(((${EnglishNumericResources.AllOrdinalRegex})|(${EnglishNumericResources.RoundNumberOrdinalRegex}))s|halves|quarters)(?=\b)`;
    // static readonly FractionNounWithArticleRegex = String.raw`(?<=\b)(${EnglishNumericResources.AllIntRegex}\s+(and\s+)?)?(a|an|one)(\s+|\s*-\s*)((${EnglishNumericResources.AllOrdinalRegex})|(${EnglishNumericResources.RoundNumberOrdinalRegex})|half|quarter)(?=\b)`;
    static readonly FractionNounWithArticleRegex = String.raw`(?=\b)(${EnglishNumericResources.AllIntRegex}\s+(and\s+)?)?(a|an|one)(\s+|\s*-\s*)((${EnglishNumericResources.AllOrdinalRegex})|(${EnglishNumericResources.RoundNumberOrdinalRegex})|half|quarter)(?=\b)`;
    // static readonly FractionPrepositionRegex = String.raw`(?<=\b)((${EnglishNumericResources.AllIntRegex})|((?<!\.)\d+))\s+over\s+((${EnglishNumericResources.AllIntRegex})|(\d+)(?!\.))(?=\b)`;
    static readonly FractionPrepositionRegex = String.raw`(?=\b)((${EnglishNumericResources.AllIntRegex})|((?!\.)\d+))\s+over\s+((${EnglishNumericResources.AllIntRegex})|(\d+)(?!\.))(?=\b)`;

    // DoubleRegex
    static readonly AllPointRegex = String.raw`((\s+${EnglishNumericResources.ZeroToNineIntegerRegex})+|(\s+${EnglishNumericResources.SeparaIntRegex}))`;
    static readonly AllFloatRegex = String.raw`${EnglishNumericResources.AllIntRegex}(\s+point)${EnglishNumericResources.AllPointRegex}`;
    // static readonly DoubleWithMultiplierRegex = String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+\s*(K|k|M|G|T|B|b)(?=\b)`;
    static readonly DoubleWithMultiplierRegex = String.raw`(((?!\d+\s*)-\s*)|((\s|^)(?=\b)))\d+\.\d+\s*(K|k|M|G|T|B|b)(?=\b)`;
    // static readonly DoubleExponentialNotationRegex = String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))(\d+(\.\d+)?)e([+-]*[1-9]\d*)(?=\b)`;
    static readonly DoubleExponentialNotationRegex = String.raw`(((?!\d+\s*)-\s*)|((?=\b)(((\d+\.\d)|\d+)|(?!\d+\.))))(\d+(\.\d+)?)e([+-]*[1-9]\d*)(?=\b)`;
    // static readonly DoubleCaretExponentialNotationRegex = String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))(\d+(\.\d+)?)\^([+-]*[1-9]\d*)(?=\b)`;
    static readonly DoubleCaretExponentialNotationRegex = String.raw`(((?!\d+\s*)-\s*)|((\s|^)(?=\b)))((\d+\.\d)|\d+)+(?!(\.\d+))(?=(?=\b))\^([+-]*[1-9]\d*)(?=\b)`;
    // static readonly DoubleDecimalPointRegex = (placeholder: string): string => String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+(?!(\.\d+))(?=${placeholder})`;
    static readonly DoubleDecimalPointRegex =  (placeholder: string): string => String.raw`(((?!\d+\s*)-\s*)|((?=\b)))\d+\.\d+(?!\.)(?=${placeholder})`;

    // static readonly DoubleWithoutIntegralRegex = (placeholder: string): string => String.raw`(?<=\s|^)(?<!(\d+))\.\d+(?!(\.\d+))(?=${placeholder})`;
    static readonly DoubleWithoutIntegralRegex = (placeholder: string): string => String.raw`(?=\s|^)(?!(\d+))\.\d+(?!(\.\d+))(?=${placeholder})`;
    // static readonly DoubleWithThousandsRegex = (placeholder: string): string => String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d{1,3}(,\d{3})+\.\d+(?=${placeholder})`;
    static readonly DoubleWithThousandsRegex = (placeholder: string): string => String.raw`(((?!\d+\s*)-\s*)|((?=\b)(?!\d+\.)))\d{1,3}($0$\d{3})+$1$\d+(?=${placeholder})`;
    // static readonly DoubleWithRoundNumber = String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+\s+${EnglishNumericResources.RoundNumberIntegerRegex}(?=\b)`;
    static readonly DoubleWithRoundNumber = String.raw`(((?!\d+\s*)-\s*)|((?!=\b)(?!!\d+\.)))\d+\.\d+\s+${EnglishNumericResources.RoundNumberIntegerRegex}(?=\b)`;
    // static readonly DoubleAllFloatRegex = String.raw`((?<=\b)${EnglishNumericResources.AllFloatRegex}(?=\b))`;
    static readonly DoubleAllFloatRegex = String.raw`((?=\b)${EnglishNumericResources.AllFloatRegex}(?=\b))`;

    // Number Regex
    static readonly CurrencyRegex = String.raw`(((?<=\W|^)-\s*)|(?<=\b))\d+\s*(B|b|m|t|g)(?=\b)`;

    // Percentage Regex
    static readonly NumberWithSuffixPercentage = String.raw`(${CommonNumeric.NumberReplaceToken})(\s*)(%|per cents|per cent|cents|cent|percentage|percents|percent)`;
    static readonly NumberWithPrefixPercentage = String.raw`(per cent of|percent of|percents of)(\s*)(${CommonNumeric.NumberReplaceToken})`;

    // Parser
    static readonly DecimalSeparatorChar = '.';
    static readonly FractionMarkerToken = String.raw`over`;
    static readonly NonDecimalSeparatorChar = ',';
    static readonly HalfADozenText = String.raw`six`;
    static readonly WordSeparatorToken = String.raw`and`;
    static readonly WrittenDecimalSeparatorTexts = ["point"];
    static readonly WrittenGroupSeparatorTexts = ["punto"];
    static readonly WrittenIntegerSeparatorTexts = ["and"];
    static readonly WrittenFractionSeparatorTexts = ["and"];

    static readonly HalfADozenRegex = String.raw`half\s+a\s+dozen`;

    // HACK: Next regex is buggy, may include a digit before the unit
    // TODO: support look behind:
    // ((?<=\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\b))|((?<=(\d|\b))(k|t|m|g|b)(?=\b))
    static readonly DigitalNumberRegex = String.raw`((?=\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\b))|(?:(\d{1}|\b))(k|t|m|g|b)(?=\b)`;

    static readonly CardinalNumberMap: ReadonlyMap<string, number> = new Map([
        ["a", 1],
        ["zero", 0],
        ["an", 1],
        ["one", 1],
        ["two", 2],
        ["three", 3],
        ["four", 4],
        ["five", 5],
        ["six", 6],
        ["seven", 7],
        ["eight", 8],
        ["nine", 9],
        ["ten", 10],
        ["eleven", 11],
        ["twelve", 12],
        ["dozen", 12],
        ["dozens", 12],
        ["thirteen", 13],
        ["fourteen", 14],
        ["fifteen", 15],
        ["sixteen", 16],
        ["seventeen", 17],
        ["eighteen", 18],
        ["nineteen", 19],
        ["twenty", 20],
        ["thirty", 30],
        ["forty", 40],
        ["fifty", 50],
        ["sixty", 60],
        ["seventy", 70],
        ["eighty", 80],
        ["ninety", 90],
        ["hundred", 100],
        ["thousand", 1000],
        ["million", 1000000],
        ["billion", 1000000000],
        ["trillion", 1000000000000]
    ]);

    static readonly OrdinalNumberMap: ReadonlyMap<string, number> = new Map([
        ["first", 1],
        ["second", 2],
        ["secondary", 2],
        ["half", 2],
        ["third", 3],
        ["fourth", 4],
        ["quarter", 4],
        ["fifth", 5],
        ["sixth", 6],
        ["seventh", 7],
        ["eighth", 8],
        ["ninth", 9],
        ["tenth", 10],
        ["eleventh", 11],
        ["twelfth", 12],
        ["thirteenth", 13],
        ["fourteenth", 14],
        ["fifteenth", 15],
        ["sixteenth", 16],
        ["seventeenth", 17],
        ["eighteenth", 18],
        ["nineteenth", 19],
        ["twentieth", 20],
        ["thirtieth", 30],
        ["fortieth", 40],
        ["fiftieth", 50],
        ["sixtieth", 60],
        ["seventieth", 70],
        ["eightieth", 80],
        ["ninetieth", 90],
        ["hundredth", 100],
        ["thousandth", 1000],
        ["millionth", 1000000],
        ["billionth", 1000000000],
        ["trillionth", 1000000000000],
        ["firsts", 1],
        ["halves", 2],
        ["thirds", 3],
        ["fourths", 4],
        ["quarters", 4],
        ["fifths", 5],
        ["sixths", 6],
        ["sevenths", 7],
        ["eighths", 8],
        ["ninths", 9],
        ["tenths", 10],
        ["elevenths", 11],
        ["twelfths", 12],
        ["thirteenths", 13],
        ["fourteenths", 14],
        ["fifteenths", 15],
        ["sixteenths", 16],
        ["seventeenths", 17],
        ["eighteenths", 18],
        ["nineteenths", 19],
        ["twentieths", 20],
        ["thirtieths", 30],
        ["fortieths", 40],
        ["fiftieths", 50],
        ["sixtieths", 60],
        ["seventieths", 70],
        ["eightieths", 80],
        ["ninetieths", 90],
        ["hundredths", 100],
        ["thousandths", 1000],
        ["millionths", 1000000],
        ["billionths", 1000000000],
        ["trillionths", 1000000000000]
    ]);

    static readonly RoundNumberMap: ReadonlyMap<string, number> = new Map([
        ["hundred", 100],
        ["thousand", 1000],
        ["million", 1000000],
        ["billion", 1000000000],
        ["trillion", 1000000000000],
        ["hundredth", 100],
        ["thousandth", 1000],
        ["millionth", 1000000],
        ["billionth", 1000000000],
        ["trillionth", 1000000000000],
        ["hundredths", 100],
        ["thousandths", 1000],
        ["millionths", 1000000],
        ["billionths", 1000000000],
        ["trillionths", 1000000000000],
        ["dozen", 12],
        ["dozens", 12],
        ["k", 1000],
        ["m", 1000000],
        ["g", 1000000000],
        ["b", 1000000000],
        ["t", 1000000000000]
    ]);
}
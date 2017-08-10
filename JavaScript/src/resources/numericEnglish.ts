import { CommonNumeric } from "./numericCommon";

export namespace EnglishNumeric {
    // Integer Regex
    export const LangMarker = String.raw`Eng`;
    export const RoundNumberIntegerRegex = String.raw`(hundred|thousand|million|billion|trillion)`;
    export const ZeroToNineIntegerRegex = String.raw`(three|seven|eight|four|five|zero|nine|one|two|six)`;
    export const AnIntRegex = String.raw`(an|a)(?=\s)`;
    export const TenToNineteenIntegerRegex = String.raw`(seventeen|thirteen|fourteen|eighteen|nineteen|fifteen|sixteen|eleven|twelve|ten)`;
    export const TensNumberIntegerRegex = String.raw`(seventy|twenty|thirty|eighty|ninety|forty|fifty|sixty)`;
    export const SeparaIntRegex = String.raw`(((${EnglishNumeric.TenToNineteenIntegerRegex}|(${EnglishNumeric.TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*)${EnglishNumeric.ZeroToNineIntegerRegex})|${EnglishNumeric.TensNumberIntegerRegex}|${EnglishNumeric.ZeroToNineIntegerRegex})(\s+${EnglishNumeric.RoundNumberIntegerRegex})*))|((${EnglishNumeric.AnIntRegex}(\s+${EnglishNumeric.RoundNumberIntegerRegex})+))`;
    export const AllIntRegex = String.raw`((((${EnglishNumeric.TenToNineteenIntegerRegex}|(${EnglishNumeric.TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*)${EnglishNumeric.ZeroToNineIntegerRegex})|${EnglishNumeric.TensNumberIntegerRegex}|${EnglishNumeric.ZeroToNineIntegerRegex}|${EnglishNumeric.AnIntRegex})(\s+${EnglishNumeric.RoundNumberIntegerRegex})+)\s+(and\s+)?)*${EnglishNumeric.SeparaIntRegex})`;
    export const PlaceHolderPureNumber = String.raw`(?=\b)`;
    export const PlaceHolderDefault = String.raw`\D|\b`;

    // export const NumbersWithPlaceHolder = (placeholder: string): string => String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d+(?!(\.\d+[a-zA-Z]))(?=${placeholder})`;
    export const NumbersWithPlaceHolder = (placeholder: string): string => String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d+(?!(\.\d+[a-zA-Z]))(?=${placeholder})`;
    // export const NumbersWithSuffix = String.raw`(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s*(K|k|M|T|G)(?=\b)`;
    export const NumbersWithSuffix = String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d+\s*(K|k|M|T|G)(?=\b)`;
    // export const RoundNumberIntegerRegexWithLocks = String.raw`(?<=\b)\d+\s+${EnglishNumeric.RoundNumberIntegerRegex}(?=\b)`;
    export const RoundNumberIntegerRegexWithLocks = String.raw`(?=\b)\d+\s+${EnglishNumeric.RoundNumberIntegerRegex}(?=\b)`;
    // export const NumbersWithDozenSuffix = String.raw`(((?<!\d+\s*)-\s*)|(?<=\b))\d+\s+dozen(s)?(?=\b)`;
    export const NumbersWithDozenSuffix = String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d+\s+dozen(s)?(?=\b)`;
    // export const AllIntRegexWithLocks = String.raw`((?<=\b)${EnglishNumeric.AllIntRegex}(?=\b))`;
    export const AllIntRegexWithLocks = String.raw`((?=\b)${EnglishNumeric.AllIntRegex}(?=\b))`;
    // export const AllIntRegexWithDozenSuffixLocks = String.raw`(?<=\b)(((half\s+)?a\s+dozen)|(${EnglishNumeric.AllIntRegex}\s+dozen(s)?))(?=\b)`;
    export const AllIntRegexWithDozenSuffixLocks = String.raw`(?=\b)(((half\s+)?a\s+dozen)|(${EnglishNumeric.AllIntRegex}\s+dozen(s)?))(?=\b)`;

    // Ordinal Regex
    export const RoundNumberOrdinalRegex = String.raw`(hundredth|thousandth|millionth|billionth|trillionth)`;
    export const BasicOrdinalRegex = String.raw`(first|second|third|fourth|fifth|sixth|seventh|eighth|ninth|tenth|eleventh|twelfth|thirteenth|fourteenth|fifteenth|sixteenth|seventeenth|eighteenth|nineteenth|twentieth|thirtieth|fortieth|fiftieth|sixtieth|seventieth|eightieth|ninetieth)`;
    export const SuffixBasicOrdinalRegex
    = String.raw`(((((${EnglishNumeric.TensNumberIntegerRegex}(\s+(and\s+)?|\s*-\s*)${
        EnglishNumeric.ZeroToNineIntegerRegex})|${EnglishNumeric.TensNumberIntegerRegex}|${EnglishNumeric.ZeroToNineIntegerRegex}|${EnglishNumeric.AnIntRegex
        })(\s+${EnglishNumeric.RoundNumberIntegerRegex})+)\s+(and\s+)?)*(${EnglishNumeric.TensNumberIntegerRegex
        }(\s+|\s*-\s*))?${EnglishNumeric.BasicOrdinalRegex})`;
    export const SuffixRoundNumberOrdinalRegex = String.raw`((${EnglishNumeric.AllIntRegex}\s+)${EnglishNumeric.RoundNumberOrdinalRegex})`;
    export const AllOrdinalRegex = String.raw`(${EnglishNumeric.SuffixBasicOrdinalRegex}|${EnglishNumeric.SuffixRoundNumberOrdinalRegex})`;
    // export const OrdinalSuffixRegex = String.raw`(?<=\b)((\d*(1st|2nd|3rd|4th|5th|6th|7th|8th|9th|0th))|(11th|12th))(?=\b)`;
    export const OrdinalSuffixRegex = String.raw`(?=\b)((\d*(1st|2nd|3rd|4th|5th|6th|7th|8th|9th|0th))|(11th|12th))(?=\b)`;
    // export const OrdinalNumericRegex = String.raw`(?<=\b)(\d{1,3}(\s*,\s*\d{3})*\s*th)(?=\b)`;
    export const OrdinalNumericRegex = String.raw`(?=\b)(\d{1,3}(\s*,\s*\d{3})*\s*th)(?=\b)`;
    // export const OrdinalRoundNumberRegex = String.raw`(?<!(a|an)\s+)${EnglishNumeric.RoundNumberOrdinalRegex}`;
    export const OrdinalRoundNumberRegex = String.raw`(?!(a|an)\s+)${EnglishNumeric.RoundNumberOrdinalRegex}`;
    // export const OrdinalEnglishRegex = String.raw`(?<=\b)${EnglishNumeric.AllOrdinalRegex}(?=\b)`;
    export const OrdinalEnglishRegex = String.raw`(?=\b)${EnglishNumeric.AllOrdinalRegex}(?=\b)`;

    // Fraction Regex
    // export const FractionNotationWithSpacesRegex = String.raw`(((?<=\W|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))`;
    export const FractionNotationWithSpacesRegex = String.raw`(((?=\W|^)-\s*)|(?=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))`;
    // export const FractionNotationRegex = String.raw`(((?<=\W|^)-\s*)|(?<=\b))\d+[/]\d+(?=(\b[^/]|$))`;
    export const FractionNotationRegex = String.raw`(((?=\W|^)-\s*)|(?=\b))\d+[/]\d+(?=(\b[^/]|$))`;
    // export const FractionNounRegex = String.raw`(?<=\b)(${EnglishNumeric.AllIntRegex}\s+(and\s+)?)?(${EnglishNumeric.AllIntRegex
    export const FractionNounRegex = String.raw`(?=\b)(${EnglishNumeric.AllIntRegex}\s+(and\s+)?)?(${EnglishNumeric.AllIntRegex})(\s+|\s*-\s*)(((${EnglishNumeric.AllOrdinalRegex})|(${EnglishNumeric.RoundNumberOrdinalRegex}))s|halves|quarters)(?=\b)`;
    // export const FractionNounWithArticleRegex = String.raw`(?<=\b)(${EnglishNumeric.AllIntRegex}\s+(and\s+)?)?(a|an|one)(\s+|\s*-\s*)((${EnglishNumeric.AllOrdinalRegex})|(${EnglishNumeric.RoundNumberOrdinalRegex})|half|quarter)(?=\b)`;
    export const FractionNounWithArticleRegex = String.raw`(?=\b)(${EnglishNumeric.AllIntRegex}\s+(and\s+)?)?(a|an|one)(\s+|\s*-\s*)((${EnglishNumeric.AllOrdinalRegex})|(${EnglishNumeric.RoundNumberOrdinalRegex})|half|quarter)(?=\b)`;
    // export const FractionPrepositionRegex = String.raw`(?<=\b)((${EnglishNumeric.AllIntRegex})|((?<!\.)\d+))\s+over\s+((${EnglishNumeric.AllIntRegex})|(\d+)(?!\.))(?=\b)`;
    export const FractionPrepositionRegex = String.raw`(?=\b)((${EnglishNumeric.AllIntRegex})|((?!\.)\d+))\s+over\s+((${EnglishNumeric.AllIntRegex})|(\d+)(?!\.))(?=\b)`;

    // DoubleRegex
    export const AllPointRegex = String.raw`((\s+${EnglishNumeric.ZeroToNineIntegerRegex})+|(\s+${EnglishNumeric.SeparaIntRegex}))`;
    export const AllFloatRegex = String.raw`${EnglishNumeric.AllIntRegex}(\s+point)${EnglishNumeric.AllPointRegex}`;
    // export const DoubleWithMultiplierRegex = String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+\s*(K|k|M|G|T|B|b)(?=\b)`;
    export const DoubleWithMultiplierRegex = String.raw`(((?!\d+\s*)-\s*)|((\s|^)(?=\b)))\d+\.\d+\s*(K|k|M|G|T|B|b)(?=\b)`;
    // export const DoubleExponentialNotationRegex = String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))(\d+(\.\d+)?)e([+-]*[1-9]\d*)(?=\b)`;
    export const DoubleExponentialNotationRegex = String.raw`(((?!\d+\s*)-\s*)|((?=\b)(((\d+\.\d)|\d+)|(?!\d+\.))))(\d+(\.\d+)?)e([+-]*[1-9]\d*)(?=\b)`;
    // export const DoubleCaretExponentialNotationRegex = String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))(\d+(\.\d+)?)\^([+-]*[1-9]\d*)(?=\b)`;
    export const DoubleCaretExponentialNotationRegex = String.raw`(((?!\d+\s*)-\s*)|((\s|^)(?=\b)))((\d+\.\d)|\d+)+(?!(\.\d+))(?=(?=\b))\^([+-]*[1-9]\d*)(?=\b)`;
    // export const DoubleDecimalPointRegex = (placeholder: string): string => String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+(?!(\.\d+))(?=${placeholder})`;
    export const DoubleDecimalPointRegex =  (placeholder: string): string => String.raw`(((?!\d+\s*)-\s*)|((?=\b)))\d+\.\d+(?!\.)(?=${placeholder})`;

    // export const DoubleWithoutIntegralRegex = (placeholder: string): string => String.raw`(?<=\s|^)(?<!(\d+))\.\d+(?!(\.\d+))(?=${placeholder})`;
    export const DoubleWithoutIntegralRegex = (placeholder: string): string => String.raw`(?=\s|^)(?!(\d+))\.\d+(?!(\.\d+))(?=${placeholder})`;
    // export const DoubleWithRoundNumber = String.raw`(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.)))\d+\.\d+\s+${EnglishNumeric.RoundNumberIntegerRegex}(?=\b)`;
    export const DoubleWithRoundNumber = String.raw`(((?!\d+\s*)-\s*)|((?!=\b)(?!!\d+\.)))\d+\.\d+\s+${EnglishNumeric.RoundNumberIntegerRegex}(?=\b)`;
    // export const DoubleAllFloatRegex = String.raw`((?<=\b)${EnglishNumeric.AllFloatRegex}(?=\b))`;
    export const DoubleAllFloatRegex = String.raw`((?=\b)${EnglishNumeric.AllFloatRegex}(?=\b))`;

    // Number Regex
    export const CurrencyRegex = String.raw`(((?<=\W|^)-\s*)|(?<=\b))\d+\s*(B|b|m|t|g)(?=\b)`;

    // Percentage Regex
    export const NumberWithSuffixPercentage = String.raw`(${CommonNumeric.NumberReplaceToken})(\s*)(%|per cents|per cent|cents|cent|percentage|percents|percent)`;
    export const NumberWithPrefixPercentage = String.raw`(per cent of|percent of|percents of)(\s*)(${CommonNumeric.NumberReplaceToken})`;

    // Parser
    export const DecimalSeparatorChar = '.';
    export const FractionMarkerToken = String.raw`over`;
    export const NonDecimalSeparatorChar = ',';
    export const HalfADozenText = String.raw`six`;
    export const WordSeparatorToken = String.raw`and`;
    export const WrittenDecimalSeparatorTexts = ["point"];
    export const WrittenGroupSeparatorTexts = ["punto"];
    export const WrittenIntegerSeparatorTexts = ["and"];
    export const WrittenFractionSeparatorTexts = ["and"];

    export const HalfADozenRegex = String.raw`half\s+a\s+dozen`;

    // HACK: Next regex is buggy, may include a digit before the unit
    // TODO: support look behind:
    // ((?<=\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\b))|((?<=(\d|\b))(k|t|m|g|b)(?=\b))
    export const DigitalNumberRegex = String.raw`((?=\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\b))|(?:(\d{1}|\b))(k|t|m|g|b)(?=\b)`;

    export const CardinalNumberMap: ReadonlyMap<string, number> = new Map([
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

    export const OrdinalNumberMap: ReadonlyMap<string, number> = new Map([
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

    export const RoundNumberMap: ReadonlyMap<string, number> = new Map([
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
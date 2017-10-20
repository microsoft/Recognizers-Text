import { IExtractor, ExtractResult, BaseNumberParser, BaseNumberExtractor, RegExpUtility } from "recognizers-text-number"
import { IDateExtractorConfiguration, IDateParserConfiguration, BaseDateExtractor } from "../baseDate";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { ChineseDurationExtractor } from "./durationConfiguration";
import { Token, IDateTimeUtilityConfiguration } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";

class ChineseDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];
    readonly implicitDateList: RegExp[];
    readonly monthEnd: RegExp;
    readonly ofMonth: RegExp;
    readonly dateUnitRegex: RegExp;
    readonly forTheRegex: RegExp;
    readonly weekDayAndDayOfMothRegex: RegExp;
    readonly relativeMonthRegex: RegExp;
    readonly weekDayRegex: RegExp;
    readonly dayOfWeek: ReadonlyMap<string, number>;
    readonly ordinalExtractor: BaseNumberExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: BaseDurationExtractor;
    readonly utilityConfiguration: IDateTimeUtilityConfiguration;

    constructor() {
        this.dateRegexList = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList1),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList2),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList3),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList4),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList5),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList6),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList7),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateRegexList8)
        ];
        this.implicitDateList = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.LunarRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDayRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateThisRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateLastRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DateNextRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.WeekDayOfMonthRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.SpecialDate)
        ];
    }
}

export class ChineseDateExtractor extends BaseDateExtractor {
    private readonly durationExtractor: ChineseDurationExtractor;

    constructor() {
        super(new ChineseDateExtractorConfiguration());
        this.durationExtractor = new ChineseDurationExtractor();
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(super.basicRegexMatch(source))
            .concat(super.implicitDate(source))
            .concat(this.durationWithBeforeAndAfter(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected durationWithBeforeAndAfter(source: string): Array<Token> {
        let ret = [];
        let durEx = this.durationExtractor.extract(source);
        durEx.forEach(er => {
            let pos = er.start + er.length;
            if (pos < source.length) {
                let nextChar = source.substr(pos, 1);
                if (nextChar === '前' || nextChar === '后') {
                    ret.push(new Token(er.start, pos + 1));
                }
            }
        });
        return ret;
    }
}
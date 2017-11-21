import { IHolidayExtractorConfiguration, BaseHolidayParserConfiguration, BaseHolidayParser } from "../baseHoliday"
import { RegExpUtility, ExtractResult, Match, IExtractor, IParser, StringUtility, ChineseIntegerExtractor, AgnosticNumberParserFactory, AgnosticNumberParserType, ChineseNumberParserConfiguration } from "recognizers-text-number";
import { Constants as NumberConstants } from "recognizers-text-number"
import { DateUtils, FormatUtil, DateTimeResolutionResult, StringMap } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";
import { Constants, TimeTypeConstants } from "../constants";

export class ChineseHolidayExtractorConfiguration implements IHolidayExtractorConfiguration {
    readonly holidayRegexes: RegExp[]

    constructor() {
        this.holidayRegexes = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.HolidayRegexList1),
            RegExpUtility.getSafeRegExp(ChineseDateTime.HolidayRegexList2),
            RegExpUtility.getSafeRegExp(ChineseDateTime.LunarHolidayRegex)
        ];
    }
}

class ChineseHolidayParserConfiguration extends BaseHolidayParserConfiguration {
    constructor() {
        super();
        this.holidayRegexList = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.HolidayRegexList1),
            RegExpUtility.getSafeRegExp(ChineseDateTime.HolidayRegexList1)
        ];
        this.holidayFuncDictionary = this.initHolidayFuncs();
        this.variableHolidaysTimexDictionary = ChineseDateTime.HolidayNoFixedTimex;
    }

    getSwiftYear(source: string): number {
        if (source.endsWith('年')) return 0;
        if (source.endsWith('去年')) return -1;
        if (source.endsWith('明年')) return 1;
        return null;
    }

    sanitizeHolidayToken(holiday: string): string {
        return holiday;
    }

    protected initHolidayFuncs(): ReadonlyMap<string, (year: number) => Date> {
        return new Map<string, (year: number) => Date>([
            ...super.initHolidayFuncs(),
            ['父亲节', BaseHolidayParserConfiguration.FathersDay],
            ['母亲节', BaseHolidayParserConfiguration.MothersDay],
            ['感恩节', BaseHolidayParserConfiguration.ThanksgivingDay]
        ]);
    }
}

const yearNow = (new Date()).getFullYear();
const yuandan = new Date(yearNow, 1 - 1, 1);
const chsnationalday = new Date(yearNow, 10 - 1, 1);
const laborday = new Date(yearNow, 5 - 1, 1);
const christmasday = new Date(yearNow, 12 - 1, 25);
const loverday = new Date(yearNow, 2 - 1, 14);
const chsmilbuildday = new Date(yearNow, 8 - 1, 1);
const foolday = new Date(yearNow, 4 - 1, 1);
const girlsday = new Date(yearNow, 3 - 1, 7);
const treeplantday = new Date(yearNow, 3 - 1, 12);
const femaleday = new Date(yearNow, 3 - 1, 8);
const childrenday = new Date(yearNow, 6 - 1, 1);
const youthday = new Date(yearNow, 5 - 1, 4);
const teacherday = new Date(yearNow, 9 - 1, 10);
const singlesday = new Date(yearNow, 11 - 1, 11);
const halloweenday = new Date(yearNow, 10 - 1, 31);
const midautumnday = new Date(yearNow, 8 - 1, 15);
const springday = new Date(yearNow, 1 - 1, 1);
const chuxiday = DateUtils.addDays(new Date(yearNow, 1 - 1, 1), -1);
const lanternday = new Date(yearNow, 1 - 1, 15);
const qingmingday = new Date(yearNow, 4 - 1, 4);
const dragonboatday = new Date(yearNow, 5 - 1, 5);
const chongyangday = new Date(yearNow, 9 - 1, 9);

export class ChineseHolidayParser extends BaseHolidayParser {
    private readonly lunarHolidayRegex; RegExp;
    private readonly integerExtractor: IExtractor;
    private readonly numberParser: IParser;
    private readonly fixedHolidayDictionary: Map<string, Date>;

    constructor() {
        let config = new ChineseHolidayParserConfiguration();
        super(config);
        this.lunarHolidayRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.LunarHolidayRegex);
        this.integerExtractor = new ChineseIntegerExtractor();
        this.numberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Integer, new ChineseNumberParserConfiguration());
        this.fixedHolidayDictionary = new Map<string, Date>([
            ['元旦', yuandan],
            ['元旦节', yuandan],
            ['教师节', teacherday],
            ['青年节', youthday],
            ['儿童节', childrenday],
            ['妇女节', femaleday],
            ['植树节', treeplantday],
            ['情人节', loverday],
            ['圣诞节', christmasday],
            ['新年', yuandan],
            ['愚人节', foolday],
            ['五一', laborday],
            ['劳动节', laborday],
            ['万圣节', halloweenday],
            ['中秋节', midautumnday],
            ['中秋', midautumnday],
            ['春节', springday],
            ['除夕', chuxiday],
            ['元宵节', lanternday],
            ['清明节', qingmingday],
            ['清明', qingmingday],
            ['端午节', dragonboatday],
            ['端午', dragonboatday],
            ['国庆节', chsnationalday],
            ['建军节', chsmilbuildday],
            ['女生节', girlsday],
            ['光棍节', singlesday],
            ['双十一', singlesday],
            ['重阳节', chongyangday]
        ]);
    }

    parse(er: ExtractResult, referenceDate?: Date): DateTimeParseResult {
        if (!referenceDate) referenceDate = new Date();
        let value = null;

        if (er.type === BaseHolidayParser.ParserName) {
            let innerResult = this.parseHolidayRegexMatch(er.text, referenceDate);

            if (innerResult.success) {
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.DATE] = FormatUtil.formatDate(innerResult.futureValue);
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.DATE] = FormatUtil.formatDate(innerResult.pastValue);
                innerResult.isLunar = this.isLunar(er.text);
                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        ret.value = value;
        ret.timexStr = value === null ? "" : value.timex;
        ret.resolutionStr = "";

        return ret;
    }

    private isLunar(source: string): boolean {
        return RegExpUtility.isMatch(this.lunarHolidayRegex, source);
    }

    protected match2Date(match: Match, referenceDate: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();

        let holidayStr = this.config.sanitizeHolidayToken(match.groups("holiday").value.toLowerCase());
        if (StringUtility.isNullOrEmpty(holidayStr)) return ret;

        // get year (if exist)
        let year = referenceDate.getFullYear();
        let yearNum = match.groups('year').value;
        let yearChinese = match.groups('yearchs').value;
        let yearRelative = match.groups('yearrel').value;
        let hasYear = false;

        if (!StringUtility.isNullOrEmpty(yearNum)) {
            hasYear = true;
            if (this.config.getSwiftYear(yearNum) === 0) {
                yearNum = yearNum.substr(0, yearNum.length - 1);
            }
            year = this.convertYear(yearNum, false);
        } else if (!StringUtility.isNullOrEmpty(yearChinese)) {
            hasYear = true;
            if (this.config.getSwiftYear(yearChinese) === 0) {
                yearChinese = yearChinese.substr(0, yearChinese.length - 1);
            }
            year = this.convertYear(yearChinese, true);
        } else if (!StringUtility.isNullOrEmpty(yearRelative)) {
            hasYear = true;
            year += this.config.getSwiftYear(yearRelative);
        }

        if (year < 100 && year >= 90) {
            year += 1900;
        } else if (year < 100 && year < 20) {
            year += 2000;
        }

        let timex = '';
        let date = new Date(referenceDate);
        if (this.fixedHolidayDictionary.has(holidayStr)) {
            date = this.fixedHolidayDictionary.get(holidayStr);
            timex = `-${FormatUtil.toString(date.getMonth() + 1, 2)}-${FormatUtil.toString(date.getDate(), 2)}`;
        } else if (this.config.holidayFuncDictionary.has(holidayStr)) {
            date = this.config.holidayFuncDictionary.get(holidayStr)(year);
            timex = this.config.variableHolidaysTimexDictionary.get(holidayStr);
        } else {
            return ret;
        }

        if (hasYear) {
            ret.timex = FormatUtil.toString(year, 4) + timex;
            ret.futureValue = new Date(year, date.getMonth(), date.getDate());
            ret.pastValue = new Date(year, date.getMonth(), date.getDate());
        } else {
            ret.timex = "XXXX" + timex;
            ret.futureValue = this.getDateValue(date, referenceDate, holidayStr, 1, (d, r) => d.getTime() < r.getTime());
            ret.pastValue = this.getDateValue(date, referenceDate, holidayStr, -1, (d, r) => d.getTime() >= r.getTime());
        }

        ret.success = true;

        return ret;
    }

    private convertYear(yearStr: string, isChinese: boolean): number {
        let year = -1;
        let er: ExtractResult;
        if (isChinese) {
            let yearNum = 0;
            er = this.integerExtractor.extract(yearStr).pop();
            if (er && er.type === NumberConstants.SYS_NUM_INTEGER) {
                yearNum = Number.parseInt(this.numberParser.parse(er).value);
            }

            if (yearNum < 10) {
                yearNum = 0;
                for (let index = 0; index < yearStr.length; index++) {
                    let char = yearStr.charAt[index];
                    yearNum *= 10;
                    er = this.integerExtractor.extract(char).pop();
                    if (er && er.type === NumberConstants.SYS_NUM_INTEGER) {
                        yearNum += Number.parseInt(this.numberParser.parse(er).value);
                    }
                }
            } else {
                year = yearNum;
            }
        } else {
            year = Number.parseInt(yearStr, 10);
        }

        return year === 0 ? -1 : year;
    }

    private getDateValue(date: Date, referenceDate: Date, holiday: string, swift: number, comparer: (date: Date, referenceDate: Date) => boolean): Date {
        let result = new Date(date);
        if (comparer(date, referenceDate)) {
            if (this.fixedHolidayDictionary.has(holiday)) {
                return DateUtils.addYears(date, swift);
            }
            if (this.config.holidayFuncDictionary.has(holiday)) {
                result = this.config.holidayFuncDictionary.get(holiday)(referenceDate.getFullYear() + swift);
            }
        }

        return result;
    }
}
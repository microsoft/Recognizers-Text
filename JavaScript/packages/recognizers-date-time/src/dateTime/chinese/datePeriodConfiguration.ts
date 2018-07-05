import { IExtractor, IParser, ExtractResult, RegExpUtility, StringUtility, Match } from "@microsoft/recognizers-text";
import { AgnosticNumberParserFactory, AgnosticNumberParserType, ChineseNumberParserConfiguration, BaseNumberExtractor, ChineseIntegerExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number"
import { Constants as NumberConstants } from "@microsoft/recognizers-text-number"
import { IDatePeriodExtractorConfiguration, BaseDatePeriodExtractor, IDatePeriodParserConfiguration, BaseDatePeriodParser } from "../baseDatePeriod";
import { ChineseDateExtractor, ChineseDateParser } from "./dateConfiguration";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { ChineseDurationExtractor } from "./durationConfiguration";
import { Token, IDateTimeUtilityConfiguration, DateTimeResolutionResult, DateUtils, FormatUtil, StringMap } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";
import { Constants, TimeTypeConstants } from "../constants";

class ChineseDatePeriodExtractorConfiguration implements IDatePeriodExtractorConfiguration {
    readonly simpleCasesRegexes: RegExp[]
    readonly YearRegex: RegExp
    readonly tillRegex: RegExp
    readonly followedUnit: RegExp
    readonly numberCombinedWithUnit: RegExp
    readonly pastRegex: RegExp
    readonly futureRegex: RegExp
    readonly weekOfRegex: RegExp
    readonly monthOfRegex: RegExp
    readonly dateUnitRegex: RegExp
    readonly inConnectorRegex: RegExp
    readonly rangeUnitRegex: RegExp
    readonly datePointExtractor: ChineseDateExtractor
    readonly integerExtractor: BaseNumberExtractor
    readonly numberParser: BaseNumberParser
    readonly durationExtractor: BaseDurationExtractor
    readonly rangeConnectorRegex: RegExp

    constructor() {
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.SimpleCasesRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.OneWordPeriodRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.StrictYearRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.YearToYear),
            RegExpUtility.getSafeRegExp(ChineseDateTime.YearAndMonth),
            RegExpUtility.getSafeRegExp(ChineseDateTime.PureNumYearAndMonth),
            RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodYearInChineseRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.WeekOfMonthRegex),
            RegExpUtility.getSafeRegExp(ChineseDateTime.SeasonWithYear),
            RegExpUtility.getSafeRegExp(ChineseDateTime.QuarterRegex),
        ];
        this.datePointExtractor = new ChineseDateExtractor();
        this.integerExtractor = new ChineseIntegerExtractor();
        this.numberParser = new BaseNumberParser(new ChineseNumberParserConfiguration());
        this.tillRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodTillRegex)
        this.followedUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.FollowedUnit);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(ChineseDateTime.NumberCombinedWithUnit);
        this.pastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PastRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FutureRegex);
    }

    getFromTokenIndex(source: string) {
        let result = { matched: false, index: -1 };
        if (source.endsWith("从")) {
            result.index = source.lastIndexOf("从");
            result.matched = true;
        }
        return result;
    };

    getBetweenTokenIndex(source: string) {
        return { matched: false, index: -1 };
    };

    hasConnectorToken(source: string) {
        return false;
    }
}

export class ChineseDatePeriodExtractor extends BaseDatePeriodExtractor {
    constructor() {
        super(new ChineseDatePeriodExtractorConfiguration());
    }

    extract(source: string, refDate: Date): Array<ExtractResult> {
        if (!refDate) refDate = new Date();
        let referenceDate = refDate;
        
        let tokens: Array<Token> = new Array<Token>()
            .concat(super.matchSimpleCases(source))
            .concat(super.mergeTwoTimePoints(source, refDate))
            .concat(this.matchNumberWithUnit(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchNumberWithUnit(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let durations = new Array<Token>();
        this.config.integerExtractor.extract(source).forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            let followedUnitMatch = RegExpUtility.getMatches(this.config.followedUnit, afterStr).pop();
            if (followedUnitMatch && followedUnitMatch.index === 0) {
                durations.push(new Token(er.start, er.start + er.length + followedUnitMatch.length));
            }
        });

        RegExpUtility.getMatches(this.config.numberCombinedWithUnit, source).forEach(match => {
            durations.push(new Token(match.index, match.index + match.length));
        });

        durations.forEach(duration => {
            let beforeStr = source.substr(0, duration.start).toLowerCase();
            if (StringUtility.isNullOrWhitespace(beforeStr)) {
                return;
            }

            let match = RegExpUtility.getMatches(this.config.pastRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end));
                return;
            }

            match = RegExpUtility.getMatches(this.config.futureRegex, beforeStr).pop();
            if (match && StringUtility.isNullOrWhitespace(beforeStr.substr(match.index + match.length))) {
                tokens.push(new Token(match.index, duration.end));
                return;
            }
        });

        return tokens;
    }
}

class ChineseDatePeriodParserConfiguration implements IDatePeriodParserConfiguration {
    readonly dateExtractor: BaseDateExtractor
    readonly dateParser: BaseDateParser
    readonly durationExtractor: ChineseDurationExtractor
    readonly durationParser: BaseDurationParser
    readonly monthFrontBetweenRegex: RegExp
    readonly betweenRegex: RegExp
    readonly monthFrontSimpleCasesRegex: RegExp
    readonly simpleCasesRegex: RegExp
    readonly oneWordPeriodRegex: RegExp
    readonly monthWithYear: RegExp
    readonly monthNumWithYear: RegExp
    readonly yearRegex: RegExp
    readonly pastRegex: RegExp
    readonly futureRegex: RegExp
    readonly inConnectorRegex: RegExp
    readonly weekOfMonthRegex: RegExp
    readonly weekOfYearRegex: RegExp
    readonly quarterRegex: RegExp
    readonly quarterRegexYearFront: RegExp
    readonly allHalfYearRegex: RegExp
    readonly seasonRegex: RegExp
    readonly weekOfRegex: RegExp
    readonly monthOfRegex: RegExp
    readonly whichWeekRegex: RegExp
    readonly nextPrefixRegex: RegExp
    readonly pastPrefixRegex: RegExp
    readonly thisPrefixRegex: RegExp
    readonly restOfDateRegex: RegExp
    readonly laterEarlyPeriodRegex: RegExp
    readonly weekWithWeekDayRangeRegex: RegExp
    readonly tokenBeforeDate: string
    readonly dayOfMonth: ReadonlyMap<string, number>
    readonly monthOfYear: ReadonlyMap<string, number>
    readonly cardinalMap: ReadonlyMap<string, number>
    readonly seasonMap: ReadonlyMap<string, string>
    readonly unitMap: ReadonlyMap<string, string>

    constructor() {
        this.simpleCasesRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SimpleCasesRegex);
        this.yearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodYearRegex);
        this.seasonRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SeasonRegex);
        this.seasonMap = ChineseDateTime.ParserConfigurationSeasonMap;
        this.quarterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.QuarterRegex);
        this.cardinalMap = ChineseDateTime.ParserConfigurationCardinalMap;
        this.unitMap = ChineseDateTime.ParserConfigurationUnitMap;
        this.durationExtractor = new ChineseDurationExtractor();
        this.pastRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PastRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.FutureRegex);
        this.monthOfYear = ChineseDateTime.ParserConfigurationMonthOfYear;
        this.dayOfMonth = ChineseDateTime.ParserConfigurationDayOfMonth;
        this.monthOfYear = ChineseDateTime.ParserConfigurationMonthOfYear;
        this.oneWordPeriodRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.OneWordPeriodRegex);
        this.dateExtractor = new ChineseDateExtractor();
        this.dateParser = new ChineseDateParser();
        this.tokenBeforeDate = 'on ';
        this.weekOfMonthRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.WeekOfMonthRegex);
        this.thisPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodThisRegex);
        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodNextRegex);
        this.pastPrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodLastRegex);

    }

    getSwiftDayOrMonth(source: string): number {
        let trimmedSource = source.trim().toLowerCase();
        if (trimmedSource.endsWith('去年')) {
            return -1;
        }
        if (trimmedSource.endsWith('明年')) {
            return 1;
        }
        if (trimmedSource.endsWith('前年')) {
            return -2;
        }
        if (trimmedSource. endsWith('后年')) {
            return 2;
        }
        if (trimmedSource.startsWith('下个')) {
            return 1;
        }
        if (trimmedSource.startsWith('上个')) {
            return -1;
        }
        if (RegExpUtility.isMatch(this.thisPrefixRegex, trimmedSource)) {
            return 0;
        }
        if (RegExpUtility.isMatch(this.nextPrefixRegex, trimmedSource)) {
            return 1;
        }
        if (RegExpUtility.isMatch(this.pastPrefixRegex, trimmedSource)) {
            return -1;
        }
        return 0;
    }

    getSwiftYear(source: string): number {
        let trimmedSource = source.trim().toLowerCase();
        let swift = -10;
        if (trimmedSource.startsWith('明年')) {
            swift = 1;
        } else if (trimmedSource.startsWith('去年')) {
            swift = -1;
        } else if (trimmedSource.startsWith('今年')) {
            swift = 0;
        }
        return swift;
    }

    isFuture(source: string): boolean {
        return (RegExpUtility.isMatch(this.thisPrefixRegex, source)
            || RegExpUtility.isMatch(this.nextPrefixRegex, source));
    }

    isYearToDate(source: string): boolean {
        let trimmedSource = source.trim().toLowerCase();
        return trimmedSource === '今年';
    }

    isMonthToDate(source: string): boolean {
        return false;
    }

    isWeekOnly(source: string): boolean {
        let trimmedSource = source.trim().toLowerCase();
        return trimmedSource.endsWith('周') || trimmedSource.endsWith('星期');
    }

    isWeekend(source: string): boolean {
        let trimmedSource = source.trim().toLowerCase();
        return trimmedSource.endsWith('周末');
    }

    isMonthOnly(source: string): boolean {
        let trimmedSource = source.trim().toLowerCase();
        return trimmedSource.endsWith('月');
    }

    isYearOnly(source: string): boolean {
        let trimmedSource = source.trim().toLowerCase();
        return trimmedSource.endsWith('年');
    }

    isLastCardinal(source: string): boolean {
        return source === '最后一';
    }
}

export class ChineseDatePeriodParser extends BaseDatePeriodParser {
    private readonly integerExtractor: IExtractor;
    private readonly numberParser: IParser;
    private readonly yearInChineseRegex: RegExp;
    private readonly numberCombinedWithUnitRegex: RegExp;
    private readonly unitRegex: RegExp;
    private readonly yearAndMonthRegex: RegExp;
    private readonly pureNumberYearAndMonthRegex: RegExp;
    private readonly yearToYearRegex: RegExp;
    private readonly chineseYearRegex: RegExp;
    private readonly seasonWithYearRegex: RegExp;

    constructor() {
        let config = new ChineseDatePeriodParserConfiguration();
        super(config, false);
        this.integerExtractor = new ChineseIntegerExtractor();
        this.numberParser = AgnosticNumberParserFactory.getParser(AgnosticNumberParserType.Integer, new ChineseNumberParserConfiguration());
        this.yearInChineseRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodYearInChineseRegex);
        this.numberCombinedWithUnitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.NumberCombinedWithUnit);
        this.unitRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.UnitRegex);
        this.yearAndMonthRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.YearAndMonth);
        this.pureNumberYearAndMonthRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.PureNumYearAndMonth);
        this.yearToYearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.YearToYear);
        this.chineseYearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.DatePeriodYearInChineseRegex);
        this.seasonWithYearRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.SeasonWithYear);
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) referenceDate = new Date();
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.trim().toLowerCase();
            let innerResult = this.parseSimpleCases(source, referenceDate);
            if (!innerResult.success) {
                innerResult = this.parseOneWordPeriod(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.mergeTwoTimePoints(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseNumberWithUnit(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseDuration(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseYearAndMonth(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseYearToYear(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseYear(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseWeekOfMonth(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseSeason(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseQuarter(source, referenceDate);
            }

            if (innerResult.success) {
                if (innerResult.futureValue && innerResult.pastValue) {
                    innerResult.futureResolution = {};
                    innerResult.futureResolution[TimeTypeConstants.START_DATE] = FormatUtil.formatDate(innerResult.futureValue[0]);
                    innerResult.futureResolution[TimeTypeConstants.END_DATE] = FormatUtil.formatDate(innerResult.futureValue[1]);
                    innerResult.pastResolution = {};
                    innerResult.pastResolution[TimeTypeConstants.START_DATE] = FormatUtil.formatDate(innerResult.pastValue[0]);
                    innerResult.pastResolution[TimeTypeConstants.END_DATE] = FormatUtil.formatDate(innerResult.pastValue[1]);
                } else {
                    innerResult.futureResolution = {};
                    innerResult.pastResolution = {};
                }
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }

    protected getMatchSimpleCase(source: string): Match {
        return RegExpUtility.getMatches(this.config.simpleCasesRegex, source).pop();
    }

    protected parseSimpleCases(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let year = referenceDate.getFullYear();
        let month = referenceDate.getMonth();
        let noYear = false;
        let inputYear = false;

        let match = this.getMatchSimpleCase(source);

        if (!match || match.index !== 0 || match.length !== source.length) return result;
        let days = match.groups('day');
        let beginDay = this.config.dayOfMonth.get(days.captures[0]);
        let endDay = this.config.dayOfMonth.get(days.captures[1]);
        let monthStr = match.groups('month').value;
        if (!StringUtility.isNullOrEmpty(monthStr)) {
            month = this.config.monthOfYear.get(monthStr) - 1;
        } else {
            monthStr = match.groups('relmonth').value;
            month += this.config.getSwiftDayOrMonth(monthStr);
            if (month < 0) {
                month = 0;
                year--;
            } else if (month > 11) {
                month = 11;
                year++;
            }
        }

        let yearStr = match.groups('year').value;
        if (!StringUtility.isNullOrEmpty(yearStr)) {
            year = Number.parseInt(yearStr, 10);
            inputYear = true;
        } else {
            noYear = true;
        }

        let beginDateLuis = FormatUtil.luisDate(inputYear || this.config.isFuture(monthStr) ? year : -1, month, beginDay);
        let endDateLuis = FormatUtil.luisDate(inputYear || this.config.isFuture(monthStr) ? year : -1, month, endDay);

        let futureYear = year;
        let pastYear = year;
        let startDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, beginDay);
        if (noYear && startDate < referenceDate) futureYear++;
        if (noYear && startDate >= referenceDate) pastYear--;

        result.timex = `(${beginDateLuis},${endDateLuis},P${endDay - beginDay}D)`;
        result.futureValue = [
            DateUtils.safeCreateFromValue(DateUtils.minValue(), futureYear, month, beginDay),
            DateUtils.safeCreateFromValue(DateUtils.minValue(), futureYear, month, endDay),
        ];
        result.pastValue = [
            DateUtils.safeCreateFromValue(DateUtils.minValue(), pastYear, month, beginDay),
            DateUtils.safeCreateFromValue(DateUtils.minValue(), pastYear, month, endDay),
        ];
        result.success = true;
        return result;
    }

    protected parseYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let isChinese = false;
        let match = RegExpUtility.getMatches(this.config.yearRegex, trimmedSource).pop();
        if (!match || match.length !== trimmedSource.length) {
            match = RegExpUtility.getMatches(this.yearInChineseRegex, trimmedSource).pop();
            isChinese =  (match && match.length === trimmedSource.length);
        }

        if (!match || match.length !== trimmedSource.length) {
            return result;
        }

        let yearStr = match.value;
        if (this.config.isYearOnly(yearStr)) {
            yearStr = yearStr.substr(0, yearStr.length - 1).trim();
        }

        let year = this.convertYear(yearStr, isChinese);
        if (yearStr.length === 2) {
            if (year < 100 && year >= 30) {
                year += 1900;
            } else if (year < 30) {
                year += 2000;
            }
        }
        let beginDay = DateUtils.safeCreateFromMinValue(year, 0, 1);
        let endDay = DateUtils.safeCreateFromMinValue(year + 1, 0, 1);

        result.timex = FormatUtil.toString(year, 4);
        result.futureValue = [beginDay, endDay];
        result.pastValue = [beginDay, endDay];
        result.success = true;
        return result;
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

    protected getWeekOfMonth(cardinal: number, month: number, year: number, referenceDate: Date, noYear: boolean): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let seedDate = this.computeDate(cardinal, 1, month, year);

        let futureDate = new Date(seedDate);
        let pastDate = new Date(seedDate);
        if (noYear && futureDate < referenceDate) {
            futureDate = this.computeDate(cardinal, 1, month, year + 1);
            if (futureDate.getMonth() !== month) {
                futureDate.setDate(futureDate.getDate() - 7);
            }
        }
        if (noYear && pastDate >= referenceDate) {
            pastDate = this.computeDate(cardinal, 1, month, year - 1);
            if (pastDate.getMonth() !== month) {
                pastDate.setDate(pastDate.getDate() - 7);
            }
        }
        result.timex = noYear ?
        `XXXX-${FormatUtil.toString(month + 1, 2)}-W${FormatUtil.toString(cardinal, 2)}` :
        `${FormatUtil.toString(year, 4)}-${FormatUtil.toString(month + 1, 2)}-W${FormatUtil.toString(cardinal, 2)}`;
        result.futureValue = [futureDate, DateUtils.addDays(futureDate, this.inclusiveEndPeriod ? 6 : 7)];
        result.pastValue = [pastDate, DateUtils.addDays(pastDate, this.inclusiveEndPeriod ? 6 : 7)];
        result.success = true;
        return result;
    }

    protected computeDate(cardinal: number, weekday: number, month: number, year: number) {
        let firstDay = new Date(year, month, 1);
        let firstWeekday = DateUtils.this(firstDay, weekday);
        if (weekday === 0) weekday = 7;
        if (weekday < firstDay.getDay()) firstWeekday = DateUtils.next(firstDay, weekday);
        firstWeekday.setDate(firstWeekday.getDate() + (7 * (cardinal - 1)));
        return firstWeekday;
    }

    protected parseSeason(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.seasonWithYearRegex, source).pop();
        if (!match || match.length !== source.length) return result;

        let year = referenceDate.getFullYear();
        let yearNum = match.groups('year').value;
        let yearChinese = match.groups('yearchs').value;
        let yearRelative = match.groups('yearrel').value;
        let hasYear = false;

        if (!StringUtility.isNullOrEmpty(yearNum)) {
            hasYear = true;
            if (this.config.isYearOnly(yearNum)) {
                yearNum = yearNum.substr(0, yearNum.length - 1);
            }
            year = this.convertYear(yearNum, false);
        } else if (!StringUtility.isNullOrEmpty(yearChinese)) {
            hasYear = true;
            if (this.config.isYearOnly(yearChinese)) {
                yearChinese = yearChinese.substr(0, yearChinese.length - 1);
            }
            year = this.convertYear(yearChinese, true);
        } else if (!StringUtility.isNullOrEmpty(yearRelative)) {
            hasYear = true;
            year += this.config.getSwiftDayOrMonth(yearRelative);
        }

        if (year < 100 && year >= 90) {
            year += 1900;
        } else if (year < 100 && year < 20) {
            year += 2000;
        }

        let seasonStr = match.groups('season').value;
        let season = this.config.seasonMap.get(seasonStr);

        if (hasYear) {
            result.timex = `${FormatUtil.toString(year, 4)}-${season}`;
        }

        result.success = true;
        return result;
    }

    protected parseQuarter(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.quarterRegex, source).pop();
        if (!match || match.length !== source.length) return result;

        let year = referenceDate.getFullYear();
        let yearNum = match.groups('year').value;
        let yearChinese = match.groups('yearchs').value;
        let yearRelative = match.groups('yearrel').value;

        if (!StringUtility.isNullOrEmpty(yearNum)) {
            if (this.config.isYearOnly(yearNum)) {
                yearNum = yearNum.substr(0, yearNum.length - 1);
            }
            year = this.convertYear(yearNum, false);
        } else if (!StringUtility.isNullOrEmpty(yearChinese)) {
            if (this.config.isYearOnly(yearChinese)) {
                yearChinese = yearChinese.substr(0, yearChinese.length - 1);
            }
            year = this.convertYear(yearChinese, true);
        } else if (!StringUtility.isNullOrEmpty(yearRelative)) {
            year += this.config.getSwiftDayOrMonth(yearRelative);
        }

        if (year < 100 && year >= 90) {
            year += 1900;
        } else if (year < 100 && year < 20) {
            year += 2000;
        }

        let cardinalStr = match.groups('cardinal').value;
        let quarterNum = this.config.cardinalMap.get(cardinalStr);
        let beginDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, quarterNum * 3 - 3, 1);
        let endDate = DateUtils.safeCreateFromValue(DateUtils.minValue(), year, quarterNum * 3, 1);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.timex = `(${FormatUtil.luisDateFromDate(beginDate)},${FormatUtil.luisDateFromDate(endDate)},P3M)`;
        result.success = true;
        return result;
    }

    protected parseNumberWithUnit(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();

        // if there are NO spaces between number and unit
        let match = RegExpUtility.getMatches(this.numberCombinedWithUnitRegex, source).pop();
        if (!match) return result;

        let sourceUnit = match.groups('unit').value.trim().toLowerCase();
        if (!this.config.unitMap.has(sourceUnit)) return result;

        let numStr = match.groups('num').value;
        let beforeStr = source.substr(0, match.index).trim().toLowerCase();

        return this.parseCommonDurationWithUnit(beforeStr, sourceUnit, numStr, referenceDate);
    }

    protected parseDuration(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        // for case "前两年" "后三年"
        let durationResult = this.config.durationExtractor.extract(source, referenceDate).pop();
        if (!durationResult) return result;

        let match = RegExpUtility.getMatches(this.unitRegex, durationResult.text).pop();
        if (!match) return result;

        let sourceUnit = match.groups('unit').value.trim().toLowerCase();
        if (!this.config.unitMap.has(sourceUnit)) return result;

        let beforeStr = source.substr(0, durationResult.start).trim().toLowerCase();
        let numberStr = durationResult.text.substr(0, match.index).trim().toLowerCase();
        let numberValue = this.convertChineseToNumber(numberStr);
        let numStr = numberValue.toString();

        return this.parseCommonDurationWithUnit(beforeStr, sourceUnit, numStr, referenceDate);
    }

    private parseCommonDurationWithUnit(beforeStr: string, sourceUnit: string, numStr: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();

        let unitStr = this.config.unitMap.get(sourceUnit);
        let pastMatch = RegExpUtility.getMatches(this.config.pastRegex, beforeStr).pop();
        let hasPast = pastMatch && pastMatch.length === beforeStr.length;

        let futureMatch = RegExpUtility.getMatches(this.config.futureRegex, beforeStr).pop();
        let hasFuture = futureMatch && futureMatch.length === beforeStr.length;

        if (!hasFuture && !hasPast) {
            return result;
        }

        let beginDate = new Date(referenceDate);
        let endDate = new Date(referenceDate);
        let difference = Number.parseFloat(numStr);
        switch(unitStr) {
            case 'D':
                beginDate = hasPast ? DateUtils.addDays(referenceDate, -difference) : beginDate;
                endDate = hasFuture ? DateUtils.addDays(referenceDate, difference) : endDate;
            break;
            case 'W':
                beginDate = hasPast ? DateUtils.addDays(referenceDate, -7 * difference) : beginDate;
                endDate = hasFuture ? DateUtils.addDays(referenceDate, 7 * difference) : endDate;
            break;
            case 'MON':
                beginDate = hasPast ? DateUtils.addMonths(referenceDate, -Math.round(difference)) : beginDate;
                endDate = hasFuture ? DateUtils.addMonths(referenceDate, Math.round(difference)) : endDate;
            break;
            case 'Y':
                beginDate = hasPast ? DateUtils.addYears(referenceDate, -Math.round(difference)) : beginDate;
                endDate = hasFuture ? DateUtils.addYears(referenceDate, Math.round(difference)) : endDate;
            break;
            default: return result;
        }
        if (hasFuture) {
            beginDate = DateUtils.addDays(beginDate, 1);
            endDate = DateUtils.addDays(endDate, 1);
        }

        let beginTimex = FormatUtil.luisDateFromDate(beginDate);
        let endTimex = FormatUtil.luisDateFromDate(endDate);
        result.timex = `(${beginTimex},${endTimex},P${numStr}${unitStr.charAt(0)})`;
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.success = true;
        return result;
    }

    private convertChineseToNumber(source: string): number {
        let num = -1;
        let er = this.integerExtractor.extract(source).pop();
        if (er && er.type === NumberConstants.SYS_NUM_INTEGER) {
            num = Number.parseInt(this.numberParser.parse(er).value);
        }
        return num;
    }

    private parseYearAndMonth(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.yearAndMonthRegex, source).pop();
        if (!match || match.length !== source.length) {
            match = RegExpUtility.getMatches(this.pureNumberYearAndMonthRegex, source).pop();
        }
        if (!match || match.length !== source.length) {
            return result;
        }

        // parse year
        let year = referenceDate.getFullYear();
        let yearNum = match.groups('year').value;
        let yearChinese = match.groups('yearchs').value;
        let yearRelative = match.groups('yearrel').value;

        if (!StringUtility.isNullOrEmpty(yearNum)) {
            if (this.config.isYearOnly(yearNum)) {
                yearNum = yearNum.substr(0, yearNum.length - 1);
            }
            year = this.convertYear(yearNum, false);
        } else if (!StringUtility.isNullOrEmpty(yearChinese)) {
            if (this.config.isYearOnly(yearChinese)) {
                yearChinese = yearChinese.substr(0, yearChinese.length - 1);
            }
            year = this.convertYear(yearChinese, true);
        } else if (!StringUtility.isNullOrEmpty(yearRelative)) {
            year += this.config.getSwiftDayOrMonth(yearRelative);
        }

        if (year < 100 && year >= 90) {
            year += 1900;
        } else if (year < 100 && year < 20) {
            year += 2000;
        }

        let monthStr = match.groups('month').value.toLowerCase();
        let month = (this.config.monthOfYear.get(monthStr) % 12) - 1;

        let beginDate = DateUtils.safeCreateFromMinValue(year, month, 1);
        let endDate = month === 11
            ? DateUtils.safeCreateFromMinValue(year + 1, 0, 1)
            : DateUtils.safeCreateFromMinValue(year, month + 1, 1);

        result.timex = FormatUtil.toString(year, 4) + '-' + FormatUtil.toString(month, 2);
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.success = true;
        return result;
    }

    private parseYearToYear(source: string, referenceDate: Date): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();

        let match = RegExpUtility.getMatches(this.yearToYearRegex, source).pop();
        if (!match) {
            return result;
        }

        let yearMatches = RegExpUtility.getMatches(this.config.yearRegex, source);
        let chineseYearMatches = RegExpUtility.getMatches(this.chineseYearRegex, source);

        let beginYear = 0;
        let endYear = 0;

        if (yearMatches.length === 2) {
            beginYear = this.convertChineseToNumber(yearMatches[0].groups('year').value);
            endYear = this.convertChineseToNumber(yearMatches[1].groups('year').value);
        } else if (chineseYearMatches.length === 2) {
            beginYear = this.convertChineseToNumber(chineseYearMatches[0].groups('yearchs').value);
            endYear = this.convertChineseToNumber(chineseYearMatches[1].groups('yearchs').value);
        } else if (yearMatches.length === 1 && chineseYearMatches.length === 1) {
            if (yearMatches[0].index < chineseYearMatches[0].index) {
                beginYear = this.convertChineseToNumber(yearMatches[0].groups('year').value);
                endYear = this.convertChineseToNumber(chineseYearMatches[0].groups('yearchs').value);
            } else {
                beginYear = this.convertChineseToNumber(chineseYearMatches[0].groups('yearchs').value);
                endYear = this.convertChineseToNumber(yearMatches[0].groups('year').value);
            }
        }

        beginYear = this.sanitizeYear(beginYear);
        endYear = this.sanitizeYear(endYear);

        let beginDate = DateUtils.safeCreateFromMinValue(beginYear, 0, 1);
        let endDate = DateUtils.safeCreateFromMinValue(endYear, 0, 1);
        let beginTimex = FormatUtil.luisDateFromDate(beginDate);
        let endTimex = FormatUtil.luisDateFromDate(endDate);

        result.timex = `(${beginTimex},${endTimex},P${endYear - beginYear}Y)`;
        result.futureValue = [beginDate, endDate];
        result.pastValue = [beginDate, endDate];
        result.success = true;

        return result;
    }

    private sanitizeYear(year: number): number {
        let result = year;
        if (year < 100 && year >= 90) {
            result += 1900;
        } else if (year < 100 && year < 20) {
            result += 2000;
        }
        return result;
    }
}
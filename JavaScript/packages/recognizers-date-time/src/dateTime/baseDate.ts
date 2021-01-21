import { ExtractResult, RegExpUtility, Match, StringUtility } from "@microsoft/recognizers-text";
import { Constants, TimeTypeConstants } from "./constants";
import { Constants as NumberConstants } from "@microsoft/recognizers-text-number";
import { BaseNumberExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number";
import { Token, DateTimeFormatUtil, DateTimeResolutionResult, IDateTimeUtilityConfiguration, AgoLaterUtil, AgoLaterMode, DateUtils, DayOfWeek } from "./utilities";
import { IDateTimeExtractor } from "./baseDateTime";
import { BaseDurationExtractor, BaseDurationParser } from "./baseDuration";
import { IDateTimeParser, DateTimeParseResult } from "./parsers";
import toNumber = require("lodash.tonumber");

export interface IDateExtractorConfiguration {
    dateRegexList: RegExp[],
    implicitDateList: RegExp[],
    monthEnd: RegExp,
    ofMonth: RegExp,
    dateUnitRegex: RegExp,
    forTheRegex: RegExp,
    weekDayAndDayOfMonthRegex: RegExp,
    relativeMonthRegex: RegExp,
    strictRelativeRegex: RegExp,
    weekDayRegex: RegExp,
    dayOfWeek: ReadonlyMap<string, number>;
    ordinalExtractor: BaseNumberExtractor,
    integerExtractor: BaseNumberExtractor,
    numberParser: BaseNumberParser,
    durationExtractor: IDateTimeExtractor,
    utilityConfiguration: IDateTimeUtilityConfiguration,
}

export class BaseDateExtractor implements IDateTimeExtractor {
    protected readonly extractorName = Constants.SYS_DATETIME_DATE;
    protected readonly config: IDateExtractorConfiguration;

    constructor(config: IDateExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string, refDate: Date): ExtractResult[] {
        if (!refDate) {
            refDate = new Date();
        }
        let referenceDate = refDate;

        let tokens: Token[] = new Array<Token>();
        tokens = tokens.concat(this.basicRegexMatch(source));
        tokens = tokens.concat(this.implicitDate(source));
        tokens = tokens.concat(this.numberWithMonth(source, referenceDate));
        tokens = tokens.concat(this.durationWithBeforeAndAfter(source, referenceDate));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected basicRegexMatch(source: string): Token[] {
        let ret = [];
        this.config.dateRegexList.forEach(regexp => {
            let matches = RegExpUtility.getMatches(regexp, source);
            matches.forEach(match => {
                // @TODO Implement validateMatch as in .NET
                let preText = source.substring(0, match.index);
                let relativeRegex = RegExpUtility.getMatchEnd(this.config.strictRelativeRegex, preText, true);
                if (relativeRegex.success) {
                    ret.push(new Token(relativeRegex.match.index, match.index + match.length));
                }
                else {
                    ret.push(new Token(match.index, match.index + match.length));
                }

            });
        });
        return ret;
    }

    protected implicitDate(source: string): Token[] {
        let ret = [];
        this.config.implicitDateList.forEach(regexp => {
            let matches = RegExpUtility.getMatches(regexp, source);
            matches.forEach(match => {
                ret.push(new Token(match.index, match.index + match.length));
            });
        });
        return ret;
    }

    private numberWithMonth(source: string, refDate: Date): Token[] {
        let ret = [];
        let er = this.config.ordinalExtractor.extract(source).concat(this.config.integerExtractor.extract(source));
        er.forEach(result => {
            let num = toNumber(this.config.numberParser.parse(result).value);
            if (num < 1 || num > 31) {
                return;
            }
            if (result.start >= 0) {
                let frontString = source.substring(0, result.start | 0);
                let match = RegExpUtility.getMatches(this.config.monthEnd, frontString)[0];
                if (match && match.length) {
                    ret.push(new Token(match.index, match.index + match.length + result.length));
                    return;
                }

                // handling cases like 'for the 25th'
                let matches = RegExpUtility.getMatches(this.config.forTheRegex, source);
                let isFound = false;
                matches.forEach(matchCase => {
                    if (matchCase) {
                        let ordinalNum = matchCase.groups('DayOfMonth').value;
                        if (ordinalNum === result.text) {
                            let length = matchCase.groups('end').value.length;
                            ret.push(new Token(matchCase.index, matchCase.index + matchCase.length - length));
                            isFound = true;
                        }
                    }
                });

                if (isFound) {
                    return;
                }

                // handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
                matches = RegExpUtility.getMatches(this.config.weekDayAndDayOfMonthRegex, source);
                matches.forEach(matchCase => {
                    if (matchCase) {
                        let ordinalNum = matchCase.groups('DayOfMonth').value;
                        if (ordinalNum === result.text) {
                            let month = refDate.getMonth();
                            let year = refDate.getFullYear();

                            // get week of day for the ordinal number which is regarded as a date of reference month
                            let date = DateUtils.safeCreateFromMinValue(year, month, num);
                            let numWeekDayStr = DayOfWeek[date.getDay()].toString().toLowerCase();

                            // get week day from text directly, compare it with the weekday generated above
                            // to see whether they refer to a same week day
                            let extractedWeekDayStr = matchCase.groups("weekday").value.toString().toLowerCase();
                            if (date !== DateUtils.minValue() &&
                                this.config.dayOfWeek.get(numWeekDayStr) === this.config.dayOfWeek.get(extractedWeekDayStr)) {
                                ret.push(new Token(matchCase.index, result.start + result.length));
                                isFound = true;
                            }
                        }
                    }
                });

                if (isFound) {
                    return;
                }

                // handling cases like '20th of next month'
                let suffixStr = source.substr(result.start + result.length).toLowerCase();
                match = RegExpUtility.getMatches(this.config.relativeMonthRegex, suffixStr.trim()).pop();
                if (match && match.index === 0) {
                    let spaceLen = suffixStr.length - suffixStr.trim().length;
                    ret.push(new Token(result.start, result.start + result.length + spaceLen + match.length));
                }

                // handling cases like 'second Sunday'
                match = RegExpUtility.getMatches(this.config.weekDayRegex, suffixStr.trim()).pop();
                if (match && match.index === 0 && num >= 1 && num <= 5
                    && result.type === NumberConstants.SYS_NUM_ORDINAL) {
                    let weekDayStr = match.groups('weekday').value;
                    if (this.config.dayOfWeek.has(weekDayStr)) {
                        let spaceLen = suffixStr.length - suffixStr.trim().length;
                        ret.push(new Token(result.start, result.start + result.length + spaceLen + match.length));
                    }
                }
            }
            if (result.start + result.length < source.length) {
                let afterString = source.substring(result.start + result.length);
                let match = RegExpUtility.getMatches(this.config.ofMonth, afterString)[0];
                if (match && match.length) {
                    ret.push(new Token(result.start, result.start + result.length + match.length));
                    return;
                }
            }
        });
        return ret;
    }

    protected durationWithBeforeAndAfter(source: string, refDate: Date): Token[] {
        let ret = [];
        let durEx = this.config.durationExtractor.extract(source, refDate);
        durEx.forEach(er => {
            let match = RegExpUtility.getMatches(this.config.dateUnitRegex, er.text).pop();
            if (!match) {
                return;
            }
            ret = AgoLaterUtil.extractorDurationWithBeforeAndAfter(source, er, ret, this.config.utilityConfiguration);
        });
        return ret;
    }
}

export interface IDateParserConfiguration {
    ordinalExtractor: BaseNumberExtractor
    integerExtractor: BaseNumberExtractor
    cardinalExtractor: BaseNumberExtractor
    durationExtractor: IDateTimeExtractor
    durationParser: IDateTimeParser
    numberParser: BaseNumberParser
    monthOfYear: ReadonlyMap<string, number>
    dayOfMonth: ReadonlyMap<string, number>
    dayOfWeek: ReadonlyMap<string, number>
    unitMap: ReadonlyMap<string, string>
    cardinalMap: ReadonlyMap<string, number>
    dateRegex: RegExp[]
    onRegex: RegExp
    specialDayRegex: RegExp
    specialDayWithNumRegex: RegExp
    nextRegex: RegExp
    unitRegex: RegExp
    monthRegex: RegExp
    weekDayRegex: RegExp
    lastRegex: RegExp
    thisRegex: RegExp
    weekDayOfMonthRegex: RegExp
    forTheRegex: RegExp
    weekDayAndDayOfMonthRegex: RegExp
    relativeMonthRegex: RegExp
    strictRelativeRegex: RegExp
    relativeWeekDayRegex: RegExp
    utilityConfiguration: IDateTimeUtilityConfiguration
    dateTokenPrefix: string
    getSwiftDay(source: string): number
    getSwiftMonthOrYear(source: string): number
    isCardinalLast(source: string): boolean
}

export class BaseDateParser implements IDateTimeParser {
    protected readonly parserName = Constants.SYS_DATETIME_DATE;
    protected readonly config: IDateParserConfiguration;

    constructor(config: IDateParserConfiguration) {
        this.config = config;
    }

    parse(extractorResult: ExtractResult, referenceDate?: Date): DateTimeParseResult | null {
        if (!referenceDate) {
            referenceDate = new Date();
        }
        let resultValue;
        if (extractorResult.type === this.parserName) {
            let source = extractorResult.text.toLowerCase();
            let innerResult = this.parseBasicRegexMatch(source, referenceDate);
            if (!innerResult.success) {
                innerResult = this.parseImplicitDate(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseWeekdayOfMonth(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parserDurationWithAgoAndLater(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseNumberWithMonth(source, referenceDate);
            }
            if (!innerResult.success) {
                innerResult = this.parseSingleNumber(source, referenceDate);
            }
            if (innerResult.success) {
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.formatDate(innerResult.futureValue);
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.DATE] = DateTimeFormatUtil.formatDate(innerResult.pastValue);
                resultValue = innerResult;
            }
        }
        let result = new DateTimeParseResult(extractorResult);
        result.value = resultValue;
        result.timexStr = resultValue ? resultValue.timex : '';
        result.resolutionStr = '';

        return result;
    }

    protected parseBasicRegexMatch(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        this.config.dateRegex.some(regex => {
            let offset = 0;
            let relativeStr = null;
            let match = RegExpUtility.getMatches(regex, trimmedSource).pop();
            if (!match) {
                match = RegExpUtility.getMatches(regex, this.config.dateTokenPrefix + trimmedSource).pop();
                if (match) {
                    offset = this.config.dateTokenPrefix.length;
                    relativeStr = match.groups('order').value;
                }

            }
            if (match) {
                let relativeRegex = RegExpUtility.getMatchEnd(this.config.strictRelativeRegex, source.substring(0, match.index), true);
                let isContainRelative = relativeRegex.success && match.index + match.length === trimmedSource.length;
                if ((match.index === offset && match.length === trimmedSource.length) || isContainRelative) {

                    if (match.index !== offset) {
                        relativeStr = relativeRegex.match.value;
                    }
                    result = this.matchToDate(match, referenceDate, relativeStr);
                    return true;
                }
            }
        });
        return result;
    }

    protected parseImplicitDate(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        // handle "on 12"
        let match = RegExpUtility.getMatches(this.config.onRegex, this.config.dateTokenPrefix + trimmedSource).pop();
        if (match && match.index === this.config.dateTokenPrefix.length && match.length === trimmedSource.length) {
            let day = 0;
            let month = referenceDate.getMonth();
            let year = referenceDate.getFullYear();
            let dayStr = match.groups('day').value;
            day = this.config.dayOfMonth.get(dayStr);

            result.timex = DateTimeFormatUtil.luisDate(-1, -1, day);

            let tryStr = DateTimeFormatUtil.luisDate(year, month, day);
            let tryDate = Date.parse(tryStr);
            let futureDate: Date;
            let pastDate: Date;

            if (tryDate && !isNaN(tryDate)) {
                futureDate = DateUtils.safeCreateFromMinValue(year, month, day);
                pastDate = DateUtils.safeCreateFromMinValue(year, month, day);
                if (futureDate < referenceDate) {
                    futureDate.setMonth(futureDate.getMonth() + 1);
                }

                if (pastDate >= referenceDate) {
                    pastDate.setMonth(pastDate.getMonth() - 1);
                }
            }
            else {
                futureDate = DateUtils.safeCreateFromMinValue(year, month + 1, day);
                pastDate = DateUtils.safeCreateFromMinValue(year, month - 1, day);
            }

            result.futureValue = futureDate;
            result.pastValue = pastDate;
            result.success = true;
            return result;
        }

        // handle "today", "the day before yesterday"
        match = RegExpUtility.getMatches(this.config.specialDayRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let swift = this.config.getSwiftDay(match.value);
            let today = DateUtils.safeCreateFromMinValue(referenceDate.getFullYear(), referenceDate.getMonth(), referenceDate.getDate());
            let value = DateUtils.addDays(today, swift);
            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "two days from tomorrow"
        match = RegExpUtility.getMatches(this.config.specialDayWithNumRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let swift = this.config.getSwiftDay(match.groups('day').value);
            let numErs = this.config.integerExtractor.extract(trimmedSource);
            let numOfDays = Number.parseInt(this.config.numberParser.parse(numErs[0]).value);

            let value = DateUtils.addDays(referenceDate, swift + numOfDays);
            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "two sundays from now"
        match = RegExpUtility.getMatches(this.config.relativeWeekDayRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let numErs = this.config.integerExtractor.extract(trimmedSource);
            let num = Number.parseInt(this.config.numberParser.parse(numErs[0]).value);
            let weekdayStr = match.groups('weekday').value.toLowerCase();
            let value = referenceDate;

            // Check whether the determined day of this week has passed.
            if (value.getDay() > this.config.dayOfWeek.get(weekdayStr)) {
                num--;
            }

            while (num-- > 0) {
                value = DateUtils.next(value, this.config.dayOfWeek.get(weekdayStr));
            }

            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "next Sunday"
        match = RegExpUtility.getMatches(this.config.nextRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.next(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "this Friday"
        match = RegExpUtility.getMatches(this.config.thisRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.this(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "last Friday", "last mon"
        match = RegExpUtility.getMatches(this.config.lastRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let value = DateUtils.last(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            result.timex = DateTimeFormatUtil.luisDateFromDate(value);
            result.futureValue = value;
            result.pastValue = value;
            result.success = true;
            return result;
        }

        // handle "Friday"
        match = RegExpUtility.getMatches(this.config.weekDayRegex, trimmedSource).pop();
        if (match && match.index === 0 && match.length === trimmedSource.length) {
            let weekdayStr = match.groups('weekday').value;
            let weekday = this.config.dayOfWeek.get(weekdayStr);
            let value = DateUtils.this(referenceDate, this.config.dayOfWeek.get(weekdayStr));

            if (weekday === 0) {
                weekday = 7;
            }
            if (weekday < referenceDate.getDay()) {
                value = DateUtils.next(referenceDate, weekday);
            }
            result.timex = 'XXXX-WXX-' + weekday;
            let futureDate = new Date(value);
            let pastDate = new Date(value);
            if (futureDate < referenceDate) {
                futureDate.setDate(value.getDate() + 7);
            }
            if (pastDate >= referenceDate) {
                pastDate.setDate(value.getDate() - 7);
            }

            result.futureValue = futureDate;
            result.pastValue = pastDate;
            result.success = true;
            return result;
        }

        // handle "for the 27th."
        match = RegExpUtility.getMatches(this.config.forTheRegex, trimmedSource).pop();
        if (match) {
            let dayStr = match.groups('DayOfMonth').value;
            let er = ExtractResult.getFromText(dayStr);
            let day = Number.parseInt(this.config.numberParser.parse(er).value);

            let month = referenceDate.getMonth();
            let year = referenceDate.getFullYear();

            result.timex = DateTimeFormatUtil.luisDate(-1, -1, day);
            let date = new Date(year, month, day);
            result.futureValue = date;
            result.pastValue = date;
            result.success = true;

            return result;
        }

        // handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
        match = RegExpUtility.getMatches(this.config.weekDayAndDayOfMonthRegex, trimmedSource).pop();
        if (match) {
            let dayStr = match.groups('DayOfMonth').value;
            let er = ExtractResult.getFromText(dayStr);
            let day = Number.parseInt(this.config.numberParser.parse(er).value);
            let month = referenceDate.getMonth();
            let year = referenceDate.getFullYear();

            // the validity of the phrase is guaranteed in the Date Extractor
            result.timex = DateTimeFormatUtil.luisDate(year, month, day);
            result.futureValue = new Date(year, month, day);
            result.pastValue = new Date(year, month, day);
            result.success = true;

            return result;
        }

        return result;
    }

    private parseNumberWithMonth(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let ambiguous = true;
        let result = new DateTimeResolutionResult();

        let ers = this.config.ordinalExtractor.extract(trimmedSource);
        if (!ers || ers.length === 0) {
            ers = this.config.integerExtractor.extract(trimmedSource);
        }
        if (!ers || ers.length === 0) {
            return result;
        }

        let num = Number.parseInt(this.config.numberParser.parse(ers[0]).value);
        let day = 1;
        let month = 0;

        let match = RegExpUtility.getMatches(this.config.monthRegex, trimmedSource).pop();
        if (match) {
            month = this.config.monthOfYear.get(match.value) - 1;
            day = num;
        }
        else {
            // handling relative month
            match = RegExpUtility.getMatches(this.config.relativeMonthRegex, trimmedSource).pop();
            if (match) {
                let monthStr = match.groups('order').value;
                let swift = this.config.getSwiftMonthOrYear(monthStr);
                let date = new Date(referenceDate);
                date.setMonth(referenceDate.getMonth() + swift);
                month = date.getMonth();
                day = num;
                ambiguous = false;
            }
        }

        // handling casesd like 'second Sunday'
        if (!match) {
            match = RegExpUtility.getMatches(this.config.weekDayRegex, trimmedSource).pop();
            if (match) {
                month = referenceDate.getMonth();
                // resolve the date of wanted week day
                let wantedWeekDay = this.config.dayOfWeek.get(match.groups('weekday').value);
                let firstDate = DateUtils.safeCreateFromMinValue(referenceDate.getFullYear(), referenceDate.getMonth(), 1);
                let firstWeekday = firstDate.getDay();
                let firstWantedWeekDay = new Date(firstDate);
                firstWantedWeekDay.setDate(firstDate.getDate() + ((wantedWeekDay > firstWeekday) ? wantedWeekDay - firstWeekday : wantedWeekDay - firstWeekday + 7));
                day = firstWantedWeekDay.getDate() + ((num - 1) * 7);
                ambiguous = false;
            }
        }

        if (!match) {
            return result;
        }

        let year = referenceDate.getFullYear();

        // for LUIS format value string
        let futureDate = DateUtils.safeCreateFromMinValue(year, month, day);
        let pastDate = DateUtils.safeCreateFromMinValue(year, month, day);

        if (ambiguous) {
            result.timex = DateTimeFormatUtil.luisDate(-1, month, day);
            if (futureDate < referenceDate) {
                futureDate.setFullYear(year + 1);
            }
            if (pastDate >= referenceDate) {
                pastDate.setFullYear(year - 1);
            }
        }
        else {
            result.timex = DateTimeFormatUtil.luisDate(year, month, day);
        }

        result.futureValue = futureDate;
        result.pastValue = pastDate;
        result.success = true;
        return result;
    }

    // handle cases like "the 27th". In the extractor, only the unmatched weekday and date will output this date.
    private parseSingleNumber(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();

        let er = this.config.ordinalExtractor.extract(trimmedSource).pop();
        if (!er || StringUtility.isNullOrEmpty(er.text)) {
            er = this.config.integerExtractor.extract(trimmedSource).pop();
        }
        if (!er || StringUtility.isNullOrEmpty(er.text)) {
            return result;
        }

        let day = Number.parseInt(this.config.numberParser.parse(er).value);
        let month = referenceDate.getMonth();
        let year = referenceDate.getFullYear();

        result.timex = DateTimeFormatUtil.luisDate(-1, -1, day);
        let pastDate = DateUtils.safeCreateFromMinValue(year, month, day);
        let futureDate = DateUtils.safeCreateFromMinValue(year, month, day);

        if (futureDate !== DateUtils.minValue() && futureDate < referenceDate) {
            futureDate.setMonth(month + 1);
        }
        if (pastDate !== DateUtils.minValue() && pastDate >= referenceDate) {
            pastDate.setMonth(month - 1);
        }

        result.futureValue = futureDate;
        result.pastValue = pastDate;
        result.success = true;
        return result;
    }

    protected parserDurationWithAgoAndLater(source: string, referenceDate: Date): DateTimeResolutionResult {
        return AgoLaterUtil.parseDurationWithAgoAndLater(
            source,
            referenceDate,
            this.config.durationExtractor,
            this.config.durationParser,
            this.config.unitMap,
            this.config.unitRegex,
            this.config.utilityConfiguration,
            AgoLaterMode.Date
        );
    }

    protected parseWeekdayOfMonth(source: string, referenceDate: Date): DateTimeResolutionResult {
        let trimmedSource = source.trim();
        let result = new DateTimeResolutionResult();
        let match = RegExpUtility.getMatches(this.config.weekDayOfMonthRegex, trimmedSource).pop();
        if (!match) {
            return result;
        }
        let cardinalStr = match.groups('cardinal').value;
        let weekdayStr = match.groups('weekday').value;
        let monthStr = match.groups('month').value;
        let noYear = false;
        let cardinal = this.config.isCardinalLast(cardinalStr) ? 5 : this.config.cardinalMap.get(cardinalStr);
        let weekday = this.config.dayOfWeek.get(weekdayStr);
        let month = referenceDate.getMonth();
        let year = referenceDate.getFullYear();
        if (StringUtility.isNullOrEmpty(monthStr)) {
            let swift = this.config.getSwiftMonthOrYear(trimmedSource);
            let temp = new Date(referenceDate);
            temp.setMonth(referenceDate.getMonth() + swift);
            month = temp.getMonth();
            year = temp.getFullYear();
        }
        else {
            month = this.config.monthOfYear.get(monthStr) - 1;
            noYear = true;
        }
        let value = this.computeDate(cardinal, weekday, month, year);
        if (value.getMonth() !== month) {
            cardinal -= 1;
            value.setDate(value.getDate() - 7);
        }
        let futureDate = value;
        let pastDate = value;
        if (noYear && futureDate < referenceDate) {
            futureDate = this.computeDate(cardinal, weekday, month, year + 1);
            if (futureDate.getMonth() !== month) {
                futureDate.setDate(futureDate.getDate() - 7);
            }
        }
        if (noYear && pastDate >= referenceDate) {
            pastDate = this.computeDate(cardinal, weekday, month, year - 1);
            if (pastDate.getMonth() !== month) {
                pastDate.setDate(pastDate.getDate() - 7);
            }
        }
        result.timex = ['XXXX', DateTimeFormatUtil.toString(month + 1, 2), 'WXX', weekday, '#' + cardinal].join('-');
        result.futureValue = futureDate;
        result.pastValue = pastDate;
        result.success = true;
        return result;
    }

    protected matchToDate(match: Match, referenceDate: Date, relativeStr: string): DateTimeResolutionResult {
        let result = new DateTimeResolutionResult();
        let yearStr = match.groups('year').value;
        let monthStr = match.groups('month').value;
        let dayStr = match.groups('day').value;
        let weekdayStr = match.groups('weekday').value;
        let month = 0;
        let day = 0;
        let year = 0;
        if (this.config.monthOfYear.has(monthStr) && this.config.dayOfMonth.has(dayStr)) {
            month = this.config.monthOfYear.get(monthStr) - 1;
            day = this.config.dayOfMonth.get(dayStr);
            if (!StringUtility.isNullOrEmpty(yearStr)) {
                year = Number.parseInt(yearStr, 10);
                if (year < 100 && year >= Constants.MinTwoDigitYearPastNum) {
                    year += 1900;
                }
                else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum) {
                    year += 2000;
                }
            }
        }
        let noYear = false;
        if (year === 0) {
            year = referenceDate.getFullYear();
            result.timex = DateTimeFormatUtil.luisDate(-1, month, day);
            if (!StringUtility.isNullOrEmpty(relativeStr)) {
                let swift = this.config.getSwiftMonthOrYear(relativeStr);
                if (!StringUtility.isNullOrEmpty(weekdayStr)) {
                    swift = 0;
                }
                year += swift;
            }
            else {
                noYear = true;
            }

        }
        else {
            result.timex = DateTimeFormatUtil.luisDate(year, month, day);
        }

        let futurePastDates = DateUtils.generateDates(noYear, referenceDate, year, month, day);

        result.futureValue = futurePastDates.future;
        result.pastValue = futurePastDates.past;
        result.success = true;
        return result;
    }

    private computeDate(cardinal: number, weekday: number, month: number, year: number) {
        let firstDay = new Date(year, month, 1);
        let firstWeekday = DateUtils.this(firstDay, weekday);
        let dayOfWeekOfFirstDay = firstDay.getDay();
        if (weekday === 0) {
            weekday = 7;
        }
        if (dayOfWeekOfFirstDay === 0) {
            dayOfWeekOfFirstDay = 7;
        }
        if (weekday < dayOfWeekOfFirstDay) {
            firstWeekday = DateUtils.next(firstDay, weekday);
        }
        firstWeekday.setDate(firstWeekday.getDate() + (7 * (cardinal - 1)));
        return firstWeekday;
    }
}
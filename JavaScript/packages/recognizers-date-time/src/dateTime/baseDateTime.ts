import { IExtractor, ExtractResult, RegExpUtility, StringUtility } from "@microsoft/recognizers-text";
import { Constants, TimeTypeConstants } from "./constants";
import { BaseNumberExtractor, BaseNumberParser } from "@microsoft/recognizers-text-number";
import { BaseDateExtractor, BaseDateParser } from "./baseDate"
import { BaseTimeExtractor, BaseTimeParser } from "./baseTime"
import { BaseDurationExtractor, BaseDurationParser } from "./baseDuration"
import { IDateTimeParser, DateTimeParseResult } from "./parsers"
import { DateTimeFormatUtil, Token, IDateTimeUtilityConfiguration, AgoLaterUtil, AgoLaterMode, DateTimeResolutionResult, StringMap } from "./utilities";

export interface IDateTimeExtractor {
    extract(input: string, refDate?: Date): Array<ExtractResult>
}

export interface IDateTimeExtractorConfiguration {
    datePointExtractor: IDateTimeExtractor
    timePointExtractor: IDateTimeExtractor
    durationExtractor: IDateTimeExtractor
    suffixRegex: RegExp
    nowRegex: RegExp
    timeOfTodayAfterRegex: RegExp
    simpleTimeOfTodayAfterRegex: RegExp
    nightRegex: RegExp
    timeOfTodayBeforeRegex: RegExp
    simpleTimeOfTodayBeforeRegex: RegExp
    specificEndOfRegex: RegExp
    unspecificEndOfRegex: RegExp
    unitRegex: RegExp
    utilityConfiguration: IDateTimeUtilityConfiguration
    isConnectorToken(source: string): boolean
}

export class BaseDateTimeExtractor implements IDateTimeExtractor {
    protected readonly extractorName = Constants.SYS_DATETIME_DATETIME;
    protected readonly config: IDateTimeExtractorConfiguration;

    constructor(config: IDateTimeExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string, refDate: Date): Array<ExtractResult> {
        if (!refDate) refDate = new Date();
        let referenceDate = refDate;

        let tokens: Array<Token> = new Array<Token>();
        tokens = tokens.concat(this.mergeDateAndTime(source, referenceDate));
        tokens = tokens.concat(this.basicRegexMatch(source));
        tokens = tokens.concat(this.timeOfTodayBefore(source, referenceDate));
        tokens = tokens.concat(this.timeOfTodayAfter(source, referenceDate));
        tokens = tokens.concat(this.specialTimeOfDate(source, referenceDate));
        tokens = tokens.concat(this.durationWithBeforeAndAfter(source, referenceDate));

        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    protected mergeDateAndTime(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.datePointExtractor.extract(source, refDate);
        if (ers.length < 1) return tokens;
        ers = ers.concat(this.config.timePointExtractor.extract(source, refDate));
        if (ers.length < 2) return tokens;
        ers = ers.sort((erA, erB) => erA.start < erB.start ? -1 : erA.start === erB.start ? 0 : 1);
        let i = 0;
        while (i < ers.length - 1) {
            let j = i + 1;
            while (j < ers.length && ExtractResult.isOverlap(ers[i], ers[j])) {
                j++;
            }
            if (j >= ers.length) break;
            if ((ers[i].type === Constants.SYS_DATETIME_DATE && ers[j].type === Constants.SYS_DATETIME_TIME) ||
                (ers[i].type === Constants.SYS_DATETIME_TIME && ers[j].type === Constants.SYS_DATETIME_DATE)) {
                let middleBegin = ers[i].start + ers[i].length;
                let middleEnd = ers[j].start;
                if (middleBegin > middleEnd) {
                    i = j + 1;
                    continue;
                }
                let middleStr = source.substr(middleBegin, middleEnd - middleBegin).trim().toLowerCase();
                if (this.config.isConnectorToken(middleStr)) {
                    let begin = ers[i].start;
                    let end = ers[j].start + ers[j].length;
                    tokens.push(new Token(begin, end));
                    i = j + 1;
                    continue;
                }
            }
            i = j;
        }
        tokens.forEach((token, index) => {
            let afterStr = source.substr(token.end);
            let match = RegExpUtility.getMatches(this.config.suffixRegex, afterStr);
            if (match && match.length > 0) {
                // TODO: verify element
                token.end += match[0].length;
            }
        });
        return tokens;
    }

    protected basicRegexMatch(source: string): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        RegExpUtility.getMatches(this.config.nowRegex, source)
        .forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length))
        });
        return tokens;
    }

    private timeOfTodayBefore(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.timePointExtractor.extract(source, refDate);
        ers.forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let innerMatches = RegExpUtility.getMatches(this.config.nightRegex, er.text);
            if (innerMatches && innerMatches.length > 0 && innerMatches[0].index === 0) {
                beforeStr = source.substr(0, er.start + innerMatches[0].length);
            }
            if (StringUtility.isNullOrWhitespace(beforeStr)) return;
            let matches = RegExpUtility.getMatches(this.config.timeOfTodayBeforeRegex, beforeStr);
            if (matches && matches.length > 0) {
                let begin = matches[0].index;
                let end = er.start + er.length;
                tokens.push(new Token(begin, end));
            }
        });
        RegExpUtility.getMatches(this.config.simpleTimeOfTodayBeforeRegex, source)
        .forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length))
        });
        return tokens;
    }

    private timeOfTodayAfter(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.timePointExtractor.extract(source, refDate);
        ers.forEach(er => {
            let afterStr = source.substr(er.start + er.length);
            if (StringUtility.isNullOrWhitespace(afterStr)) return;
            let matches = RegExpUtility.getMatches(this.config.timeOfTodayAfterRegex, afterStr);
            if (matches && matches.length > 0) {
                let begin = er.start;
                let end = er.start + er.length + matches[0].length
                tokens.push(new Token(begin, end));
            }
        });
        RegExpUtility.getMatches(this.config.simpleTimeOfTodayAfterRegex, source)
        .forEach(match => {
            tokens.push(new Token(match.index, match.index + match.length))
        });
        return tokens;
    }

    private specialTimeOfDate(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        let ers = this.config.datePointExtractor.extract(source, refDate);
        ers.forEach(er => {
            let beforeStr = source.substr(0, er.start);
            let beforeMatches = RegExpUtility.getMatches(this.config.specificEndOfRegex, beforeStr);
            if (beforeMatches && beforeMatches.length > 0) {
                tokens.push(new Token(beforeMatches[0].index, er.start + er.length))
            } else {
                let afterStr = source.substr(er.start + er.length);
                let afterMatches = RegExpUtility.getMatches(this.config.specificEndOfRegex, afterStr);
                if (afterMatches && afterMatches.length > 0) {
                    tokens.push(new Token(er.start, er.start + er.length + afterMatches[0].index + afterMatches[0].length))
                }
            }
        });

        RegExpUtility.getMatches(this.config.unspecificEndOfRegex, source).forEach(
            match => {
                tokens.push(new Token(match.index, match.index + match.length));
            }
        );

        return tokens;
    }

    private durationWithBeforeAndAfter(source: string, refDate: Date): Array<Token> {
        let tokens: Array<Token> = new Array<Token>();
        this.config.durationExtractor.extract(source, refDate).forEach(er => {
            let matches = RegExpUtility.getMatches(this.config.unitRegex, er.text);
            if (matches && matches.length > 0) {
                tokens = AgoLaterUtil.extractorDurationWithBeforeAndAfter(source, er, tokens, this.config.utilityConfiguration);
            }
        });
        return tokens;
    }
}

export interface IDateTimeParserConfiguration {
    tokenBeforeDate: string;
    tokenBeforeTime: string;
    dateExtractor: IDateTimeExtractor;
    timeExtractor: IDateTimeExtractor;
    dateParser: IDateTimeParser;
    timeParser: IDateTimeParser;
    cardinalExtractor: BaseNumberExtractor;
    numberParser: BaseNumberParser;
    durationExtractor: IDateTimeExtractor;
    durationParser: IDateTimeParser;
    nowRegex: RegExp;
    amTimeRegex: RegExp;
    pmTimeRegex: RegExp;
    simpleTimeOfTodayAfterRegex: RegExp;
    simpleTimeOfTodayBeforeRegex: RegExp;
    specificTimeOfDayRegex: RegExp;
    specificEndOfRegex: RegExp;
    unspecificEndOfRegex: RegExp;
    unitRegex: RegExp;
    unitMap: ReadonlyMap<string, string>;
    numbers: ReadonlyMap<string, number>;
    haveAmbiguousToken(text: string, matchedText: string): boolean;
    getMatchedNowTimex(text: string): { matched: boolean, timex: string };
    getSwiftDay(text: string): number;
    getHour(text: string, hour: number): number;
    utilityConfiguration: IDateTimeUtilityConfiguration;
}

export class BaseDateTimeParser implements IDateTimeParser {
    public static readonly ParserName = Constants.SYS_DATETIME_DATETIME; // "DateTime";
    protected readonly config: IDateTimeParserConfiguration;

    constructor(configuration: IDateTimeParserConfiguration) {
        this.config = configuration;
    }

    public parse(er: ExtractResult, refTime: Date): DateTimeParseResult {
        if (!refTime) refTime = new Date();
        let referenceTime = refTime;

        let value = null;
        if (er.type === BaseDateTimeParser.ParserName) {
            let innerResult = this.mergeDateAndTime(er.text, referenceTime);
            if (!innerResult.success) {
                innerResult = this.parseBasicRegex(er.text, referenceTime);
            }

            if (!innerResult.success) {
                innerResult = this.parseTimeOfToday(er.text, referenceTime);
            }

            if (!innerResult.success) {
                innerResult = this.parseSpecialTimeOfDate(er.text, referenceTime);
            }

            if (!innerResult.success) {
                innerResult = this.parserDurationWithAgoAndLater(er.text, referenceTime);
            }

            if (innerResult.success) {
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.DATETIME] = DateTimeFormatUtil.formatDateTime(innerResult.futureValue);
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.DATETIME] = DateTimeFormatUtil.formatDateTime(innerResult.pastValue);
                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er); {
            ret.value = value,
            ret.timexStr = value === null ? "" : value.timex,
            ret.resolutionStr = ""
        };
        return ret;
    }

    protected parseBasicRegex(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let trimmedText = text.trim().toLowerCase();

        // handle "now"
        let matches = RegExpUtility.getMatches(this.config.nowRegex, trimmedText);
        if (matches.length && matches[0].index === 0 && matches[0].length === trimmedText.length) {
            let getMatchedNowTimex = this.config.getMatchedNowTimex(trimmedText);
            ret.timex = getMatchedNowTimex.timex;
            ret.futureValue = ret.pastValue = referenceTime;
            ret.success = true;
            return ret;
        }

        return ret;
    }

    // merge a Date entity and a Time entity
    protected mergeDateAndTime(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();

        let er1 = this.config.dateExtractor.extract(text, referenceTime);
        if (er1.length === 0) {
            er1 = this.config.dateExtractor.extract(this.config.tokenBeforeDate + text, referenceTime);
            if (er1.length === 1) {
                er1[0].start -= this.config.tokenBeforeDate.length;
            }
            else {
                return ret;
            }
        }
        else {
            // this is to understand if there is an ambiguous token in the text. For some languages (e.g. spanish)
            // the same word could mean different things (e.g a time in the day or an specific day).
            if (this.config.haveAmbiguousToken(text, er1[0].text)) {
                return ret;
            }
        }

        let er2 = this.config.timeExtractor.extract(text, referenceTime);
        if (er2.length === 0) {
            // here we filter out "morning, afternoon, night..." time entities
            er2 = this.config.timeExtractor.extract(this.config.tokenBeforeTime + text, referenceTime);
            if (er2.length === 1) {
                er2[0].start -= this.config.tokenBeforeTime.length;
            }
            else {
                return ret;
            }
        }

        // handle case "Oct. 5 in the afternoon at 7:00"
        // in this case "5 in the afternoon" will be extract as a Time entity
        let correctTimeIdx = 0;
        while (correctTimeIdx < er2.length && ExtractResult.isOverlap(er2[correctTimeIdx], er1[0])) {
            correctTimeIdx++;
        }

        if (correctTimeIdx >= er2.length) {
            return ret;
        }

        let pr1 = this.config.dateParser.parse(er1[0], new Date(referenceTime.toDateString()))
        let pr2 = this.config.timeParser.parse(er2[correctTimeIdx], referenceTime);
        if (pr1.value === null || pr2.value === null) {
            return ret;
        }

        let futureDate = pr1.value.futureValue;
        let pastDate = pr1.value.pastValue;
        let time = pr2.value.futureValue;

        let hour = time.getHours();
        let min = time.getMinutes();
        let sec = time.getSeconds();

        // handle morning, afternoon
        if (RegExpUtility.getMatches(this.config.pmTimeRegex, text).length && hour < 12) {
            hour += 12;
        }
        else if (RegExpUtility.getMatches(this.config.amTimeRegex, text).length && hour >= 12) {
            hour -= 12;
        }

        let timeStr = pr2.timexStr;
        if (timeStr.endsWith("ampm")) {
            timeStr = timeStr.substring(0, timeStr.length - 4);
        }
        timeStr = "T" + DateTimeFormatUtil.toString(hour, 2) + timeStr.substring(3);
        ret.timex = pr1.timexStr + timeStr;

        let val = pr2.value;
        if (hour <= 12 && !RegExpUtility.getMatches(this.config.pmTimeRegex, text).length
            && !RegExpUtility.getMatches(this.config.amTimeRegex, text).length && val.comment) {
            ret.comment = "ampm";
        }
        ret.futureValue = new Date(futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), hour, min, sec);
        ret.pastValue = new Date(pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), hour, min, sec);
        ret.success = true;

        // change the value of time object
        pr2.timexStr = timeStr;
        if (!StringUtility.isNullOrEmpty(ret.comment)) {
            pr2.value.comment = ret.comment === "ampm" ? "ampm" : "";
        }

        // add the date and time object in case we want to split them
        ret.subDateTimeEntities = [pr1, pr2];

        return ret;
    }

    protected parseTimeOfToday(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let trimmedText = text.toLowerCase().trim();

        let hour = 0;
        let min = 0;
        let sec = 0;
        let timeStr: string;

        let wholeMatches = RegExpUtility.getMatches(this.config.simpleTimeOfTodayAfterRegex, trimmedText);
        if (!(wholeMatches.length && wholeMatches[0].length === trimmedText.length)) {
            wholeMatches = RegExpUtility.getMatches(this.config.simpleTimeOfTodayBeforeRegex, trimmedText);
        }

        if (wholeMatches.length && wholeMatches[0].length === trimmedText.length) {
            let hourStr = wholeMatches[0].groups("hour").value;
            if (!hourStr) {
                hourStr = wholeMatches[0].groups("hournum").value.toLowerCase();
                hour = this.config.numbers.get(hourStr);
            }
            else {
                hour = parseInt(hourStr, 10);
            }
            timeStr = "T" + DateTimeFormatUtil.toString(hour, 2);
        }
        else {
            let ers = this.config.timeExtractor.extract(trimmedText, referenceTime);
            if (ers.length !== 1) {
                ers = this.config.timeExtractor.extract(this.config.tokenBeforeTime + trimmedText, referenceTime);
                if (ers.length === 1) {
                    ers[0].start -= this.config.tokenBeforeTime.length;
                }
                else {
                    return ret;
                }
            }

            let pr = this.config.timeParser.parse(ers[0], referenceTime);
            if (pr.value === null) {
                return ret;
            }

            let time = pr.value.futureValue;

            hour = time.getHours();
            min = time.getMinutes();
            sec = time.getSeconds();
            timeStr = pr.timexStr;
        }


        let matches = RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, trimmedText);

        if (matches.length) {
            let matchStr = matches[0].value.toLowerCase();

            // handle "last", "next"
            let swift = this.config.getSwiftDay(matchStr);

            let date = new Date(referenceTime);
            date.setDate(date.getDate() + swift);

            // handle "morning", "afternoon"
            hour = this.config.getHour(matchStr, hour);

            // in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
            if (timeStr.endsWith("ampm")) {
                timeStr = timeStr.substring(0, timeStr.length - 4);
            }
            timeStr = "T" + DateTimeFormatUtil.toString(hour, 2) + timeStr.substring(3);

            ret.timex = DateTimeFormatUtil.formatDate(date) + timeStr;
            ret.futureValue = ret.pastValue = new Date(date.getFullYear(), date.getMonth(), date.getDate(), hour, min, sec);
            ret.success = true;
            return ret;
        }

        return ret;
    }

    private parseSpecialTimeOfDate(text: string, refDateTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        ret = this.parseUnspecificTimeOfDate(text, refDateTime);
        if (ret.success) {
            return ret;
        }

        let ers = this.config.dateExtractor.extract(text, refDateTime);
        if (ers.length !== 1) {
            return ret;
        }
        let beforeStr = text.substring(0, ers[0].start || 0);
        if (RegExpUtility.getMatches(this.config.specificEndOfRegex, beforeStr).length) {
            let pr = this.config.dateParser.parse(ers[0], refDateTime);
            let futureDate = new Date(pr.value.futureValue);
            let pastDate = new Date(pr.value.pastValue);
            ret = this.resolveEndOfDay(pr.timexStr, futureDate, pastDate);
            return ret;
        }

        return ret;
    }

    private parseUnspecificTimeOfDate(text: string, refDateTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let eod = RegExpUtility.getMatches(this.config.unspecificEndOfRegex, text);
        if (eod.length) {
            let futureDate = new Date(refDateTime);
            let pastDate = new Date(refDateTime);
            ret = this.resolveEndOfDay(DateTimeFormatUtil.formatDate(refDateTime), futureDate, pastDate);
        }

        return ret;
    }

    private resolveEndOfDay(timexPrefix: string, futureDate: Date, pastDate: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult()

        ret.timex = timexPrefix + "T23:59:59";
        futureDate.setHours(23, 59, 59, 0);
        ret.futureValue = futureDate;
        pastDate.setHours(23, 59, 59, 0);
        ret.pastValue = pastDate;
        ret.success = true;

        return ret;
    }

    // handle like "two hours ago"
    private parserDurationWithAgoAndLater(text: string, referenceTime: Date): DateTimeResolutionResult {
        return AgoLaterUtil.parseDurationWithAgoAndLater(
            text,
            referenceTime,
            this.config.durationExtractor,
            this.config.durationParser,
            this.config.unitMap,
            this.config.unitRegex,
            this.config.utilityConfiguration,
            AgoLaterMode.DateTime
        );
    }
}

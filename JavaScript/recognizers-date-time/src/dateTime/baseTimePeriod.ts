import { Constants, TimeTypeConstants } from "./constants";
import { IExtractor, ExtractResult, RegExpUtility, StringUtility } from "recognizers-text-number"
import { Token, FormatUtil, DateTimeResolutionResult, IDateTimeUtilityConfiguration } from "./utilities";
import { IDateTimeParser, DateTimeParseResult } from "./parsers"
import { BaseTimeExtractor, BaseTimeParser } from "./baseTime"

export interface ITimePeriodExtractorConfiguration {
    simpleCasesRegex: RegExp[];
    tillRegex: RegExp;
    timeOfDayRegex: RegExp;
    singleTimeExtractor: BaseTimeExtractor;
    getFromTokenIndex(text: string): { matched: boolean, index: number };
    hasConnectorToken(text: string): boolean;
    getBetweenTokenIndex(text: string): { matched: boolean, index: number };
}

export class BaseTimePeriodExtractor implements IExtractor {
    readonly extractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";
    readonly config: ITimePeriodExtractorConfiguration;

    constructor(config: ITimePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens: Array<Token> = new Array<Token>()
            .concat(this.matchSimpleCases(source))
            .concat(this.mergeTwoTimePoints(source))
            .concat(this.matchNight(source));
        let result = Token.mergeAllTokens(tokens, source, this.extractorName);
        return result;
    }

    private matchSimpleCases(text: string): Array<Token> {
        let ret = [];
        this.config.simpleCasesRegex.forEach(regex => {
            let matches = RegExpUtility.getMatches(regex, text);
            matches.forEach(match => {
                // is there "pm" or "am" ?
                let pmStr = match.groups("pm").value;
                let amStr = match.groups("am").value;
                let descStr = match.groups("desc").value;
                // check "pm", "am"
                if (pmStr || amStr || descStr) {
                    ret.push(new Token(match.index, match.index + match.length));
                }
            });
        });
        return ret;
    }

    private mergeTwoTimePoints(text: string): Array<Token> {
        let ret = [];
        let ers = this.config.singleTimeExtractor.extract(text);

        // merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"
        let idx = 0;
        while (idx < ers.length - 1) {
            let middleBegin = ers[idx].start + ers[idx].length || 0;
            let middleEnd = ers[idx + 1].start || 0;

            let middleStr = text.substring(middleBegin, middleEnd).trim().toLowerCase();
            let matches = RegExpUtility.getMatches(this.config.tillRegex, middleStr);
            // handle "{TimePoint} to {TimePoint}"
            if (matches.length > 0 && matches[0].index === 0 && matches[0].length === middleStr.length) {
                let periodBegin = ers[idx].start || 0;
                let periodEnd = (ers[idx + 1].start || 0) + (ers[idx + 1].length || 0);

                // handle "from"
                let beforeStr = text.substring(0, periodBegin).trim().toLowerCase();
                let fromIndex = this.config.getFromTokenIndex(beforeStr);
                if (fromIndex.matched) {
                    periodBegin = fromIndex.index;
                }

                ret.push(new Token(periodBegin, periodEnd));
                idx += 2;
                continue;
            }
            // handle "between {TimePoint} and {TimePoint}"
            if (this.config.hasConnectorToken(middleStr)) {
                let periodBegin = ers[idx].start || 0;
                let periodEnd = (ers[idx + 1].start || 0) + (ers[idx + 1].length || 0);

                // handle "between"
                let beforeStr = text.substring(0, periodBegin).trim().toLowerCase();
                let betweenIndex = this.config.getBetweenTokenIndex(beforeStr, );
                if (betweenIndex.matched) {
                    periodBegin = betweenIndex.index;
                    ret.push(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }
            }

            idx++;
        }

        return ret;
    }

    private matchNight(source: string): Array<Token> {
        let ret = [];
        let matches = RegExpUtility.getMatches(this.config.timeOfDayRegex, source);
        matches.forEach(match => {
            ret.push(new Token(match.index, match.index + match.length));
        });
        return ret;
    }
}

export interface ITimePeriodParserConfiguration {
    timeExtractor: BaseTimeExtractor;
    timeParser: BaseTimeParser;
    pureNumberFromToRegex: RegExp;
    pureNumberBetweenAndRegex: RegExp;
    timeOfDayRegex: RegExp;
    numbers: ReadonlyMap<string, number>;
    utilityConfiguration: IDateTimeUtilityConfiguration;
    getMatchedTimexRange(text: string): {
        matched: boolean, timex: string, beginHour: number, endHour: number, endMin: number
    };
}

export class BaseTimePeriodParser implements IDateTimeParser {
    public static readonly ParserName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";
    protected readonly config: ITimePeriodParserConfiguration;

    constructor(configuration: ITimePeriodParserConfiguration) {
        this.config = configuration;
    }

    public parse(er: ExtractResult, refTime?: Date): DateTimeParseResult {
        let referenceTime = refTime || new Date();
        let value = null;
        if (er.type === BaseTimePeriodParser.ParserName) {
            let innerResult = this.parseSimpleCases(er.text, referenceTime);
            if (!innerResult.success) {
                innerResult = this.mergeTwoTimePoints(er.text, referenceTime);
            }
            if (!innerResult.success) {
                innerResult = this.parseNight(er.text, referenceTime);
            }
            if (innerResult.success) {
                innerResult.futureResolution = new Map<string, string>(
                    [
                        [
                            TimeTypeConstants.START_TIME,
                            FormatUtil.formatTime(innerResult.futureValue.item1)
                        ],
                        [
                            TimeTypeConstants.END_TIME,
                            FormatUtil.formatTime(innerResult.futureValue.item2)
                        ]
                    ]);

                innerResult.pastResolution = new Map<string, string>(
                    [
                        [
                            TimeTypeConstants.START_TIME,
                            FormatUtil.formatTime(innerResult.pastValue.item1)
                        ],
                        [
                            TimeTypeConstants.END_TIME,
                            FormatUtil.formatTime(innerResult.pastValue.item2)
                        ]
                    ]);

                value = innerResult;
            }
        }

        let ret = new DateTimeParseResult(er);
        ret.value = value;
        ret.timexStr = value === null ? "" : value.timex;
        ret.resolutionStr = "";

        return ret;
    }

    private parseSimpleCases(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let year = referenceTime.getFullYear();
        let month = referenceTime.getMonth();
        let day = referenceTime.getDate();
        let trimmedText = text.trim().toLowerCase();

        let matches = RegExpUtility.getMatches(this.config.pureNumberFromToRegex, trimmedText);
        if (!matches.length) {
            matches = RegExpUtility.getMatches(this.config.pureNumberBetweenAndRegex, trimmedText);
        }

        if (matches.length && matches[0].index === 0) {
            // this "from .. to .." pattern is valid if followed by a Date OR "pm"
            let isValid = false;

            // get hours
            let hourGroup = matches[0].groups('hour');
            let hourStr = hourGroup.captures[0];

            let beginHour = this.config.numbers.get(hourStr);
            if (!beginHour) {
                beginHour = Number.parseInt(hourStr, 10);
            }

            hourStr = hourGroup.captures[1];

            let endHour = this.config.numbers.get(hourStr);
            if (!endHour) {
                endHour = Number.parseInt(hourStr, 10);
            }

            // parse "pm" 
            let leftDesc = matches[0].groups("leftDesc").value;
            let rightDesc = matches[0].groups("rightDesc").value;
            let pmStr = matches[0].groups("pm").value;
            let amStr = matches[0].groups("am").value;
            // The "ampm" only occurs in time, don't have to consider it here
            if (StringUtility.isNullOrWhitespace(leftDesc)) {
                let rightAmValid = !StringUtility.isNullOrEmpty(rightDesc) &&
                    RegExpUtility.getMatches(this.config.utilityConfiguration.amDescRegex, rightDesc.toLowerCase()).length;
                let rightPmValid = !StringUtility.isNullOrEmpty(rightDesc) &&
                    RegExpUtility.getMatches(this.config.utilityConfiguration.pmDescRegex, rightDesc.toLowerCase()).length;
                if (!StringUtility.isNullOrEmpty(amStr) || rightAmValid) {

                    if (beginHour >= 12) {
                        beginHour -= 12;
                    }
                    if (endHour >= 12) {
                        endHour -= 12;
                    }
                    isValid = true;
                }
                else if (!StringUtility.isNullOrEmpty(pmStr) || rightPmValid) {
                    if (beginHour < 12) {
                        beginHour += 12;
                    }
                    if (endHour < 12) {
                        endHour += 12;
                    }
                    isValid = true;
                }
            }

            if (isValid) {
                let beginStr = "T" + FormatUtil.toString(beginHour, 2);
                let endStr = "T" + FormatUtil.toString(endHour, 2);

                ret.timex = `(${beginStr},${endStr},PT${endHour - beginHour}H)`;

                ret.futureValue = ret.pastValue = {
                    item1: new Date(year, month, day, beginHour, 0, 0),
                    item2: new Date(year, month, day, endHour, 0, 0)
                };

                ret.success = true;

                return ret;
            }
        }
        return ret;
    }

    private mergeTwoTimePoints(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let ers = this.config.timeExtractor.extract(text);
        let pr1: DateTimeParseResult = null;
        let pr2: DateTimeParseResult = null;
        if (ers.length !== 2) {
            return ret;
        }

        pr1 = this.config.timeParser.parse(ers[0], referenceTime);
        pr2 = this.config.timeParser.parse(ers[1], referenceTime);

        if (pr1.value === null || pr2.value === null) {
            return ret;
        }

        let ampmStr1 = pr1.value.comment;
        let ampmStr2 = pr2.value.comment;
        let beginTime: Date = pr1.value.futureValue;
        let endTime: Date = pr2.value.futureValue;
        if (!StringUtility.isNullOrEmpty(ampmStr2) && ampmStr2.endsWith("ampm")
            && endTime <= beginTime && endTime.getHours() < 12) {
            endTime.setHours(endTime.getHours() + 12);
            pr2.value.futureValue = endTime;
        }

        ret.timex = `(${pr1.timexStr},${pr2.timexStr},PT${new Date(endTime.getTime() - beginTime.getTime()).getUTCHours()}H)`;
        ret.futureValue = ret.pastValue = { item1: beginTime, item2: endTime };
        ret.success = true;

        if (ampmStr1 && ampmStr1.endsWith("ampm") && ampmStr2 && ampmStr2.endsWith("ampm")) {
            ret.comment = "ampm";
        }

        ret.subDateTimeEntities = [pr1, pr2];
        return ret;
    }

    // parse "morning", "afternoon", "night"
    private parseNight(text: string, referenceTime: Date): DateTimeResolutionResult {
        let day = referenceTime.getDate();
        let month = referenceTime.getMonth();
        let year = referenceTime.getFullYear();
        let ret = new DateTimeResolutionResult();

        // extract early/late prefix from text
        let matches = RegExpUtility.getMatches(this.config.timeOfDayRegex, text);
        let hasEarly = false;
        let hasLate = false;
        if (matches.length) {
            if (!StringUtility.isNullOrEmpty(matches[0].groups("early").value)) {
                let early = matches[0].groups("early").value;
                text = text.replace(early, "");
                hasEarly = true;
                ret.comment = "early";
            }
            if (!hasEarly && !StringUtility.isNullOrEmpty(matches[0].groups("late").value)) {
                let late = matches[0].groups("late").value;
                text = text.replace(late, "");
                hasLate = true;
                ret.comment = "late";
            }
        }

        let timexRange = this.config.getMatchedTimexRange(text);
        if (!timexRange.matched) {
            return new DateTimeResolutionResult();
        }

        // modify time period if "early" or "late" is existed
        if (hasEarly) {
            timexRange.endHour = timexRange.beginHour + 2;
            // handling case: night end with 23:59
            if (timexRange.endMin === 59) {
                timexRange.endMin = 0;
            }
        }
        else if (hasLate) {
            timexRange.beginHour = timexRange.beginHour + 2;
        }

        ret.timex = timexRange.timex;

        ret.futureValue = ret.pastValue = {
            item1: new Date(year, month, day, timexRange.beginHour, 0, 0),
            item2: new Date(year, month, day, timexRange.endHour, timexRange.endMin, timexRange.endMin)
        };

        ret.success = true;

        return ret;
    }
}

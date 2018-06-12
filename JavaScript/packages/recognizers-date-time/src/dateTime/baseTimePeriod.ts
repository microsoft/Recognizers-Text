import { IExtractor, ExtractResult, RegExpUtility, StringUtility } from "@microsoft/recognizers-text";
import { Constants, TimeTypeConstants } from "./constants";
import { Token, FormatUtil, DateTimeResolutionResult, IDateTimeUtilityConfiguration, DateUtils, StringMap } from "./utilities";
import { IDateTimeParser, DateTimeParseResult } from "./parsers"
import { BaseTimeExtractor, BaseTimeParser } from "./baseTime"
import { IDateTimeExtractor } from "./baseDateTime"

export interface ITimePeriodExtractorConfiguration {
    simpleCasesRegex: RegExp[];
    tillRegex: RegExp;
    timeOfDayRegex: RegExp;
    generalEndingRegex: RegExp;
    singleTimeExtractor: IDateTimeExtractor;
    integerExtractor: IExtractor;
    getFromTokenIndex(text: string): { matched: boolean, index: number };
    hasConnectorToken(text: string): boolean;
    getBetweenTokenIndex(text: string): { matched: boolean, index: number };
}

export class BaseTimePeriodExtractor implements IDateTimeExtractor {
    readonly extractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";
    readonly config: ITimePeriodExtractorConfiguration;

    constructor(config: ITimePeriodExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string, refDate: Date): Array<ExtractResult> {
        if (!refDate) refDate = new Date();
        let referenceDate = refDate;

        let tokens: Array<Token> = new Array<Token>()
            .concat(this.matchSimpleCases(source))
            .concat(this.mergeTwoTimePoints(source, referenceDate))
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

    private mergeTwoTimePoints(text: string, refDate: Date): Array<Token> {
        let ret = [];
        let ers = this.config.singleTimeExtractor.extract(text, refDate);
        let numErs = this.config.integerExtractor.extract(text);

        // Check if it is an ending number
        if (numErs.length > 0)
        {
            let timeNumbers: Array<ExtractResult> = [];

            // check if it is a ending number
            let endingNumber = false;
            let num = numErs[numErs.length - 1];
            if (num.start + num.length == text.length)
            {
                endingNumber = true;
            }
            else
            {
                let afterStr = text.substr(num.start + num.length);
                let endingMatch = afterStr.match(this.config.generalEndingRegex);
                if (endingMatch)
                {
                    endingNumber = true;
                }
            }

            if (endingNumber)
            {
                timeNumbers.push(num);
            }

            let i = 0;
            let j = 0;
            while (i < numErs.length)
            {
                // find subsequent time point
                let numEndPoint = numErs[i].start + numErs[i].length;
                while (j < ers.length && ers[j].start <= numEndPoint)
                {
                    j++;
                }

                if (j >= ers.length)
                {
                    break;
                }

                // check connector string
                let midStr = text.substr(numEndPoint, ers[j].start - numEndPoint);
                let match = midStr.match(this.config.tillRegex);
                if (match && match[0].length === midStr.trim().length)
                {
                    timeNumbers.push(numErs[i]);
                }
                i++;
            }

            // check overlap
            for (let timeNum of timeNumbers)
            {
                let overlap = false;
                for (let er of ers)
                {
                    if (er.start <= timeNum.start && er.start + er.length >= timeNum.start)
                    {
                        overlap = true;
                    }
                }

                if (!overlap)
                {
                    ers.push(timeNum);
                }
            }

            ers = ers.sort((x, y) => (x.start - y.start));
        }
        
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

                // handle "between"
                let betweenIndex = this.config.getBetweenTokenIndex(beforeStr);
                if (betweenIndex.matched) {
                    periodBegin = betweenIndex.index;
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
    timeExtractor: IDateTimeExtractor;
    timeParser: BaseTimeParser;
    integerExtractor: IExtractor;
    pureNumberFromToRegex: RegExp;
    pureNumberBetweenAndRegex: RegExp;
    timeOfDayRegex: RegExp;
    tillRegex: RegExp;
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
                innerResult.futureResolution = {};
                innerResult.futureResolution[TimeTypeConstants.START_TIME] = FormatUtil.formatTime(innerResult.futureValue.item1);
                innerResult.futureResolution[TimeTypeConstants.END_TIME] = FormatUtil.formatTime(innerResult.futureValue.item2);
                innerResult.pastResolution = {};
                innerResult.pastResolution[TimeTypeConstants.START_TIME] = FormatUtil.formatTime(innerResult.pastValue.item1);
                innerResult.pastResolution[TimeTypeConstants.END_TIME] = FormatUtil.formatTime(innerResult.pastValue.item2);
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

                    if (endHour >= 12) {
                        endHour -= 12;
                    }

                    if (beginHour >= 12 && beginHour - 12 < endHour) {
                        beginHour -= 12;
                    }

                    // Resolve case like "11 to 3am"
                    if (beginHour < 12 && beginHour > endHour) {
                        beginHour += 12;
                    }

                    isValid = true;
                }
                else if (!StringUtility.isNullOrEmpty(pmStr) || rightPmValid) {
                    
                    if (endHour < 12) {
                        endHour += 12;
                    }

                    // Resolve case like "11 to 3pm"
                    if (beginHour + 12 < endHour) {
                        beginHour += 12;
                    }

                    isValid = true;
                }
            }

            if (isValid) {
                let beginStr = "T" + FormatUtil.toString(beginHour, 2);
                let endStr = "T" + FormatUtil.toString(endHour, 2);

                if (beginHour >= endHour) {
                    endHour += 24
                }

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
        let ers = this.config.timeExtractor.extract(text, referenceTime);
        let pr1: DateTimeParseResult = null;
        let pr2: DateTimeParseResult = null;

        let validTimeNumber = false;

        if (ers.length !== 2) {
            if (ers.length === 1) {
                let numErs = this.config.integerExtractor.extract(text);

                for (let num of numErs) {
                    let midStrBegin = 0, midStrEnd = 0;
                    // ending number
                    if (num.start > ers[0].start + ers[0].length)
                    {
                        midStrBegin = ers[0].start + ers[0].length;
                        midStrEnd = num.start - midStrBegin;
                    }
                    else if (num.start + num.length < ers[0].start)
                    {
                        midStrBegin = num.start + num.length;
                        midStrEnd = ers[0].start - midStrBegin;
                    }

                    // check if the middle string between the time point and the valid number is a connect string.
                    var middleStr = text.substr(midStrBegin, midStrEnd);
                    var tillMatch = middleStr.match(this.config.tillRegex);
                    if (tillMatch)
                    {
                        num.type = Constants.SYS_DATETIME_TIME;
                        ers.push(num);
                        validTimeNumber = true;
                        break;
                    }
                }

                ers = ers.sort((x, y) => (x.start - y.start));
            }

            if (!validTimeNumber) {
                return ret;
            }
        }

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
            && endTime <= beginTime && DateUtils.addHours(endTime, 12) > beginTime) {
            endTime = DateUtils.addHours(endTime, 12);
            pr2.value.futureValue = endTime;
            pr2.timexStr = `T${endTime.getHours()}`;
            if (endTime.getMinutes() > 0) {
                pr2.timexStr = `${pr2.timexStr}:${endTime.getMinutes()}`;
            }
        }

        if (!StringUtility.isNullOrEmpty(ampmStr1) && ampmStr1.endsWith("ampm")
            && endTime > DateUtils.addHours(beginTime, 12)) 
        {
            beginTime = DateUtils.addHours(beginTime, 12);
            pr1.value.futureValue = beginTime;
            pr1.timexStr = `T${beginTime.getHours()}`;
            if (beginTime.getMinutes() > 0) {
                pr1.timexStr = `${pr1.timexStr}:${beginTime.getMinutes()}`;
            }
        }

        if (endTime < beginTime) {
            endTime = DateUtils.addDays(endTime, 1);
        }

        let hours = DateUtils.totalHoursFloor(endTime, beginTime);
        let minutes = DateUtils.totalMinutesFloor(endTime, beginTime) % 60;
        ret.timex = `(${pr1.timexStr},${pr2.timexStr},PT` + 
                    (hours > 0 ? `${hours}H` : '') +
                    (minutes > 0 ? `${minutes}M` : '') + 
                    ')';
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

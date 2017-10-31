import { IMergedExtractorConfiguration, BaseMergedExtractor, IMergedParserConfiguration, BaseMergedParser } from "../baseMerged"
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseSetExtractor, BaseSetParser } from "../baseSet";
import { BaseHolidayExtractor, BaseHolidayParser } from "../baseHoliday";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration"
import { RegExpUtility, ExtractResult } from "recognizers-text-number";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { ChineseDurationExtractor, ChineseDurationParser } from "./durationConfiguration"
import { ChineseTimeExtractor, ChineseTimeParser } from "./timeConfiguration"
import { ChineseDateExtractor, ChineseDateParser } from "./dateConfiguration"
import { ChineseDateTimeExtractor, ChineseDateTimeParser } from "./dateTimeConfiguration"
import { ChineseTimePeriodExtractor, ChineseTimePeriodParser } from "./timePeriodConfiguration"
import { ChineseDatePeriodExtractor, ChineseDatePeriodParser } from "./datePeriodConfiguration"
import { ChineseDateTimePeriodExtractor, ChineseDateTimePeriodParser } from "./dateTimePeriodConfiguration"
import { ChineseSetExtractor, ChineseSetParser } from "./setConfiguration"
import { ChineseHolidayExtractorConfiguration, ChineseHolidayParser } from "./holidayConfiguration"
import { DateTimeOptions } from "../dateTimeRecognizer";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";
import { Constants, TimeTypeConstants } from "../constants";
import { FormatUtil, DateUtils, DateTimeResolutionResult } from "../utilities";
import isEqual = require('lodash.isequal');

class ChineseMergedExtractorConfiguration implements IMergedExtractorConfiguration {
    readonly dateExtractor: BaseDateExtractor
    readonly timeExtractor: ChineseTimeExtractor
    readonly dateTimeExtractor: BaseDateTimeExtractor
    readonly datePeriodExtractor: BaseDatePeriodExtractor
    readonly timePeriodExtractor: ChineseTimePeriodExtractor
    readonly dateTimePeriodExtractor: BaseDateTimePeriodExtractor
    readonly holidayExtractor: BaseHolidayExtractor
    readonly durationExtractor: ChineseDurationExtractor
    readonly setExtractor: BaseSetExtractor
    readonly afterRegex: RegExp
    readonly sinceRegex: RegExp
    readonly beforeRegex: RegExp
    readonly fromToRegex: RegExp
    readonly singleAmbiguousMonthRegex: RegExp
    readonly prepositionSuffixRegex: RegExp

    constructor() {
        this.dateExtractor = new ChineseDateExtractor();
        this.timeExtractor = new ChineseTimeExtractor();
        this.dateTimeExtractor = new ChineseDateTimeExtractor();
        this.datePeriodExtractor = new ChineseDatePeriodExtractor();
        this.timePeriodExtractor = new ChineseTimePeriodExtractor();
        this.dateTimePeriodExtractor = new ChineseDateTimePeriodExtractor();
        this.setExtractor = new ChineseSetExtractor();
        this.holidayExtractor = new BaseHolidayExtractor(new ChineseHolidayExtractorConfiguration());
        this.durationExtractor = new ChineseDurationExtractor();
    }
}

export class ChineseMergedExtractor extends BaseMergedExtractor {
    private readonly dayOfMonthRegex: RegExp;
    
    constructor(options: DateTimeOptions) {
        let config = new ChineseMergedExtractorConfiguration();
        super(config, options);
        this.dayOfMonthRegex = RegExpUtility.getSafeRegExp(`^\\d{1,2}号`, 'gi');
    }

    extract(source: string): Array<ExtractResult> {
        let result: Array<ExtractResult> = new Array<ExtractResult>();
        this.addTo(result, this.config.dateExtractor.extract(source), source);
        this.addTo(result, this.config.timeExtractor.extract(source), source);
        this.addTo(result, this.config.durationExtractor.extract(source), source);
        this.addTo(result, this.config.datePeriodExtractor.extract(source), source);
        this.addTo(result, this.config.dateTimeExtractor.extract(source), source);
        this.addTo(result, this.config.timePeriodExtractor.extract(source), source);
        this.addTo(result, this.config.dateTimePeriodExtractor.extract(source), source);
        this.addTo(result, this.config.setExtractor.extract(source), source);
        this.addTo(result, this.config.holidayExtractor.extract(source), source);
        this.addMod(result, source);

        result = result.sort((a, b) => a.start - b.start);
        return result;
    }

    protected addTo(destination: ExtractResult[], source: ExtractResult[], sourceStr: string) {
        source.forEach(er => {
            let isFound = false;
            let rmIndex = -1;
            let rmLength = 1;
            for (let index = 0; index < destination.length; index++) {
                if (ExtractResult.isOverlap(destination[index], er)) {
                    isFound = true;
                    if (er.length > destination[index].length) {
                        rmIndex = index;
                        let j = index + 1;
                        while (j < destination.length && ExtractResult.isOverlap(destination[j], er)) {
                            rmLength++;
                            j++;
                        }
                    }
                    break;
                }
            }
            if (!isFound) {
                destination.push(er);
            } else if (rmIndex >= 0) {
                destination.splice(rmIndex, rmLength);
                this.moveOverlap(destination, er);
                destination.splice(rmIndex, 0, er);
            }
        });
    }

    protected moveOverlap(destination: ExtractResult[], result: ExtractResult) {
        let duplicated = new Array<number>();
        for (let i = 0; i < destination.length; i++) {
            if (result.text.includes(destination[i].text)
            && (result.start === destination[i].start || result.start + result.length === destination[i].start + destination[i].length)) {
                duplicated.push(i);
            }
        }
        duplicated.forEach(index => destination.splice(index, 1));
    }

    // ported from CheckBlackList
    protected addMod(destination: ExtractResult[], source: string) {
        let result = new Array<ExtractResult>();
        destination = destination.filter(value => {
            let valueEnd = value.start + value.length;
            if (valueEnd !== source.length) {
                let lastChar = source.substr(valueEnd, 1);
                if (value.text.endsWith('周') && valueEnd < source.length && lastChar === '岁') {
                    return false;
                }
            }
            
            if (RegExpUtility.isMatch(this.dayOfMonthRegex, value.text)) {
                return false;
            }
            
            return true;
        });
    }
}

class ChineseMergedParserConfiguration implements IMergedParserConfiguration {
    readonly beforeRegex: RegExp;
    readonly afterRegex: RegExp;
    readonly sinceRegex: RegExp;
    readonly dateParser: BaseDateParser;
    readonly holidayParser: BaseHolidayParser;
    readonly timeParser: BaseTimeParser;
    readonly dateTimeParser: BaseDateTimeParser;
    readonly datePeriodParser: BaseDatePeriodParser;
    readonly timePeriodParser: BaseTimePeriodParser;
    readonly dateTimePeriodParser: BaseDateTimePeriodParser;
    readonly durationParser: BaseDurationParser;
    readonly setParser: BaseSetParser;

    constructor() {
        this.beforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.MergedBeforeRegex);
        this.afterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.MergedAfterRegex);
        this.sinceRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.MergedAfterRegex);
        
        this.dateParser = new ChineseDateParser();
        this.holidayParser = new ChineseHolidayParser();
        this.timeParser = new ChineseTimeParser();
        this.dateTimeParser = new ChineseDateTimeParser();
        this.datePeriodParser = new ChineseDatePeriodParser();
        this.timePeriodParser = new ChineseTimePeriodParser();
        this.dateTimePeriodParser = new ChineseDateTimePeriodParser();
        this.durationParser = new ChineseDurationParser();
        this.setParser = new ChineseSetParser();
    }
}

export class ChineseMergedParser extends BaseMergedParser {
    constructor() {
        let config = new ChineseMergedParserConfiguration();
        super(config, 0);
    }

    parse(er: ExtractResult, refTime?: Date): DateTimeParseResult | null {
        let referenceTime = refTime || new Date();
        let pr: DateTimeParseResult = null;

        // push, save teh MOD string
        let hasBefore = RegExpUtility.isMatch(this.config.beforeRegex, er.text);
        let hasAfter = RegExpUtility.isMatch(this.config.afterRegex, er.text);
        let hasSince = RegExpUtility.isMatch(this.config.sinceRegex, er.text);
        let modStr = '';

        if (er.type === Constants.SYS_DATETIME_DATE) {
            pr = this.config.dateParser.parse(er, referenceTime);
            if (pr.value === null || pr.value === undefined) {
                pr = this.config.holidayParser.parse(er, referenceTime);
            }
        }
        else if (er.type === Constants.SYS_DATETIME_TIME) {
            pr = this.config.timeParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_DATETIME) {
            pr = this.config.dateTimeParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_DATEPERIOD) {
            pr = this.config.datePeriodParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_TIMEPERIOD) {
            pr = this.config.timePeriodParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_DATETIMEPERIOD) {
            pr = this.config.dateTimePeriodParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_DURATION) {
            pr = this.config.durationParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_SET) {
            pr = this.config.setParser.parse(er, referenceTime);
        }
        else {
            return null;
        }

        // pop, restore the MOD string
        if (hasBefore && pr.value !== null) {
            let val = pr.value;
            val.mod = TimeTypeConstants.beforeMod;
            pr.value = val;
        }

        if (hasAfter && pr.value !== null) {
            let val = pr.value;
            val.mod = TimeTypeConstants.afterMod;
            pr.value = val;
        }

        if (hasSince && pr.value !== null) {
            let val = pr.value;
            val.mod = TimeTypeConstants.sinceMod;
            pr.value = val;
        }

        pr.value = this.dateTimeResolution(pr, hasBefore, hasAfter, hasSince);
        pr.type = `${this.parserTypeName}.${this.determineDateTimeType(er.type, hasBefore, hasAfter, hasSince)}`;

        return pr;
    }
}

export class ChineseFullMergedParser extends BaseMergedParser {
    constructor() {
        let config = new ChineseMergedParserConfiguration();
        super(config, 0);
    }

    parse(er: ExtractResult, refTime?: Date): DateTimeParseResult | null {
        let referenceTime = refTime || new Date();
        let pr: DateTimeParseResult = null;

        // push, save teh MOD string
        let hasBefore = false;
        let hasAfter = false;
        let modStr = "";
        let beforeMatch = RegExpUtility.getMatches(this.config.beforeRegex, er.text).pop();
        let afterMatch = RegExpUtility.getMatches(this.config.afterRegex, er.text).pop();
        if (beforeMatch) {
            hasBefore = true;
            er.start += beforeMatch.length;
            er.length -= beforeMatch.length;
            er.text = er.text.substring(beforeMatch.length);
            modStr = beforeMatch.value;
        }
        else if (afterMatch) {
            hasAfter = true;
            er.start += afterMatch.length;
            er.length -= afterMatch.length;
            er.text = er.text.substring(afterMatch.length);
            modStr = afterMatch.value;
        }

        if (er.type === Constants.SYS_DATETIME_DATE) {
            pr = this.config.dateParser.parse(er, referenceTime);
            if (pr.value === null || pr.value === undefined) {
                pr = this.config.holidayParser.parse(er, referenceTime);
            }
        }
        else if (er.type === Constants.SYS_DATETIME_TIME) {
            pr = this.config.timeParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_DATETIME) {
            pr = this.config.dateTimeParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_DATEPERIOD) {
            pr = this.config.datePeriodParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_TIMEPERIOD) {
            pr = this.config.timePeriodParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_DATETIMEPERIOD) {
            pr = this.config.dateTimePeriodParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_DURATION) {
            pr = this.config.durationParser.parse(er, referenceTime);
        }
        else if (er.type === Constants.SYS_DATETIME_SET) {
            pr = this.config.setParser.parse(er, referenceTime);
        }
        else {
            return null;
        }

        // pop, restore the MOD string
        if (hasBefore && pr.value !== null) {
            pr.length += modStr.length;
            pr.start -= modStr.length;
            pr.text = modStr + pr.text;
            let val = pr.value;
            val.mod = TimeTypeConstants.beforeMod;
            pr.value = val;
        }

        if (hasAfter && pr.value !== null) {
            pr.length += modStr.length;
            pr.start -= modStr.length;
            pr.text = modStr + pr.text;
            let val = pr.value;
            val.mod = TimeTypeConstants.afterMod;
            pr.value = val;
        }

        pr.value = this.dateTimeResolution(pr, hasBefore, hasAfter);
        pr.type = `${this.parserTypeName}.${this.determineDateTimeType(er.type, hasBefore, hasAfter)}`;

        return pr;
    }

    protected dateTimeResolution(slot: DateTimeParseResult, hasBefore: boolean, hasAfter: boolean, hasSince: boolean = false): Map<string, any> {
        if (!slot) return null;

        let result = new Map<string, any>();
        let resolutions = new Array<Map<string, string>>();

        let type = slot.type;
        let outputType = this.determineDateTimeType(type, hasBefore, hasAfter);
        let timex = slot.timexStr;

        let value: DateTimeResolutionResult = slot.value;
        if (!value) return null;

        let isLunar = value.isLunar;
        let mod = value.mod;
        let comment = value.comment;

        // the following should added to res first since the ResolveAmPm is using these fields
        this.addResolutionFields(result, Constants.TimexKey, timex);
        this.addResolutionFields(result, Constants.CommentKey, comment);
        this.addResolutionFields(result, Constants.ModKey, mod);
        this.addResolutionFields(result, Constants.TypeKey, outputType);

        let futureResolution = value.futureResolution;
        let pastResolution = value.pastResolution;

        let future = this.generateFromResolution(type, futureResolution, mod);
        let past = this.generateFromResolution(type, pastResolution, mod);

        let futureValues = Array.from(future.values()).sort();
        let pastValues = Array.from(past.values()).sort();
        if (isEqual(futureValues, pastValues)) {
            if (past.size > 0) this.addResolutionFieldsAny(result, Constants.ResolveKey, past);
        } else {
            if (past.size > 0) this.addResolutionFieldsAny(result, Constants.ResolveToPastKey, past);
            if (future.size > 0) this.addResolutionFieldsAny(result, Constants.ResolveToFutureKey, future);
        }

        if (comment && comment === 'ampm') {
            if (result.has('resolve')) {
                this.resolveAMPM(result, 'resolve');
            } else {
                this.resolveAMPM(result, 'resolveToPast');
                this.resolveAMPM(result, 'resolveToFuture');
            }
        }

        if (isLunar) {
            this.addResolutionFieldsAny(result, Constants.IsLunarKey, isLunar);
        }

        result.forEach((value, key) => {
            if (value instanceof Map) {
                let newValues = new Map<string, string>();

                this.addResolutionFields(newValues, Constants.TimexKey, timex);
                this.addResolutionFields(newValues, Constants.ModKey, mod);
                this.addResolutionFields(newValues, Constants.TypeKey, outputType);

                value.forEach((innerValue, innerKey) => {
                    newValues.set(innerKey, innerValue);
                });
                resolutions.push(newValues);
            }
        });

        if (past.size === 0 && future.size === 0) {
            resolutions.push(new Map<string, string>()
                .set('timex', timex)
                .set('type', outputType)
                .set('value', 'not resolved'));
        }
        return new Map<string, any>().set('values', resolutions);
    }

    protected determineDateTimeType(type: string, hasBefore: boolean, hasAfter: boolean, hasSince: boolean = false): string {
        if (hasBefore || hasAfter || hasSince) {
            if (type === Constants.SYS_DATETIME_DATE) return Constants.SYS_DATETIME_DATEPERIOD;
            if (type === Constants.SYS_DATETIME_TIME) return Constants.SYS_DATETIME_TIMEPERIOD;
            if (type === Constants.SYS_DATETIME_DATETIME) return Constants.SYS_DATETIME_DATETIMEPERIOD;
        }
        return type;
    }
}
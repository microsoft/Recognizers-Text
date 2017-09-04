import { Constants, TimeTypeConstants } from "./constants";
import { IExtractor, ExtractResult } from "../number/extractors"
import { IDateTimeParser, DateTimeParseResult } from "./parsers"
import { RegExpUtility, StringUtility } from "../utilities"
import { FormatUtil, DateUtils, DateTimeResolutionResult } from "./utilities"
import { BaseDateExtractor, BaseDateParser} from "./baseDate"
import { BaseTimeExtractor, BaseTimeParser} from "./baseTime"
import { BaseDatePeriodExtractor, BaseDatePeriodParser} from "./baseDatePeriod"
import { BaseTimePeriodExtractor, BaseTimePeriodParser} from "./baseTimePeriod"
import { BaseDateTimeExtractor, BaseDateTimeParser} from "./baseDateTime"
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser} from "./baseDateTimePeriod"
import { BaseSetExtractor, BaseSetParser} from "./baseSet"
import { BaseDurationExtractor, BaseDurationParser} from "./baseDuration"
import { BaseHolidayExtractor, BaseHolidayParser} from "./baseHoliday"

export enum DateTimeOptions {
    None, SkipFromToMerge
}

export interface IMergedExtractorConfiguration {
    dateExtractor: BaseDateExtractor
    timeExtractor: BaseTimeExtractor
    dateTimeExtractor: BaseDateTimeExtractor
    datePeriodExtractor: BaseDatePeriodExtractor
    timePeriodExtractor: BaseTimePeriodExtractor
    dateTimePeriodExtractor: BaseDateTimePeriodExtractor
    holidayExtractor: BaseHolidayExtractor
    durationExtractor: BaseDurationExtractor
    setExtractor: BaseSetExtractor
    afterRegex: RegExp
    beforeRegex: RegExp
    fromToRegex: RegExp
    singleAmbiguousMonthRegex: RegExp
    prepositionSuffixRegex: RegExp
}

export class BaseMergedExtractor implements IExtractor {
    private readonly config: IMergedExtractorConfiguration;
    private readonly options: DateTimeOptions;

    constructor(config: IMergedExtractorConfiguration, options: DateTimeOptions) {
        this.config = config;
        this.options = options;
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

    private addTo(destination: ExtractResult[], source: ExtractResult[], text: string) {
        source.forEach(value => {
            if (this.options === DateTimeOptions.SkipFromToMerge && this.shouldSkipFromMerge(value)) return;
            if (this.filterAmbiguousSingleWord(value, text)) return;
            let isFound = false;
            let overlapIndexes = new Array<number>();
            let firstIndex = -1;
            destination.forEach((dest, index) => {
                if (ExtractResult.isOverlap(dest, value)) {
                    if (firstIndex === -1) {
                        firstIndex = index;
                    }
                    isFound = true;
                    if (value.length > dest.length) {
                        overlapIndexes.push(index);
                    }
                }
            });
            if (!isFound) {
                destination.push(value)
            } else if (overlapIndexes.length) {
                let tempDst = new Array<ExtractResult>();
                for (let i = 0; i < destination.length; i++) {
                    if (overlapIndexes.indexOf(i) === -1) {
                        tempDst.push(destination[i]);
                    }
                }

                // insert at the first overlap occurence to keep the order
                tempDst.splice(firstIndex, 0, value);
                destination.length = 0;
                destination.push.apply(destination, tempDst);
            }
        });
    }

    private shouldSkipFromMerge(er: ExtractResult): boolean {
        return RegExpUtility.getMatches(this.config.fromToRegex, er.text).length > 0;
    }

    private filterAmbiguousSingleWord(er: ExtractResult, text: string): boolean {
        let matches = RegExpUtility.getMatches(this.config.singleAmbiguousMonthRegex, er.text.toLowerCase())
        if (matches.length) {
            let stringBefore = text.substring(0, er.start).replace(/\s+$/, '');
            matches = RegExpUtility.getMatches(this.config.prepositionSuffixRegex, stringBefore);
            if (!matches.length) {
                return true;
            }
        }
        return false;
    }

    private addMod(ers: ExtractResult[], source: string) {
        let lastEnd = 0;
        ers.forEach(er => {
            let beforeStr = source.substr(lastEnd, er.start).toLowerCase();
            let before = this.hasTokenIndex(beforeStr.trim(), this.config.beforeRegex);
            if (before.matched) {
                let modLength = beforeStr.length - before.index;
                er.length += modLength;
                er.start -= modLength;
                er.text = source.substr(er.start, er.length);
            }
            let after = this.hasTokenIndex(beforeStr.trim(), this.config.afterRegex);
            if (after.matched) {
                let modLength = beforeStr.length - after.index;
                er.length += modLength;
                er.start -= modLength;
                er.text = source.substr(er.start, er.length);
            }
        });
    }

    private hasTokenIndex(source: string, regex: RegExp): { matched: boolean, index: number } {
        let result = { matched: false, index: -1 };
        let match = RegExpUtility.getMatches(regex, source).pop();
        if (match) {
            result.matched = true
            result.index = match.index
        }
        return result;
    }
}

export interface IMergedParserConfiguration {
    beforeRegex: RegExp
    afterRegex: RegExp
    dateParser: BaseDateParser
    holidayParser: BaseHolidayParser
    timeParser: BaseTimeParser
    dateTimeParser: BaseDateTimeParser
    datePeriodParser: BaseDatePeriodParser
    timePeriodParser: BaseTimePeriodParser
    dateTimePeriodParser: BaseDateTimePeriodParser
    durationParser: BaseDurationParser
    setParser: BaseSetParser
}

export class BaseMergedParser implements IDateTimeParser {
    readonly parserTypeName = 'datetimeV2';

    private readonly config: IMergedParserConfiguration;

    private readonly dateMinValue = FormatUtil.formatDate(DateUtils.minValue());
    private readonly dateTimeMinValue = FormatUtil.formatDateTime(DateUtils.minValue());

    constructor(config: IMergedParserConfiguration) {
        this.config = config;
    }

    parse(er: ExtractResult, refTime?: Date): DateTimeParseResult | null {
        let referenceTime = refTime || new Date();
        let pr: DateTimeParseResult = null;

        // push, save teh MOD string
        let hasBefore = false;
        let hasAfter = false;
        let modStr = "";
        let beforeMatch = RegExpUtility.getMatches(this.config.beforeRegex, er.text).shift();
        let afterMatch = RegExpUtility.getMatches(this.config.afterRegex, er.text).shift();
        if (beforeMatch && beforeMatch.index === 0) {
            hasBefore = true;
            er.start += beforeMatch.length;
            er.length -= beforeMatch.length;
            er.text = er.text.substring(beforeMatch.length);
            modStr = beforeMatch.value;
        }
        else if (afterMatch && afterMatch.index === 0) {
            hasAfter = true;
            er.start += afterMatch.length;
            er.length -= afterMatch.length;
            er.text = er.text.substring(afterMatch.length);
            modStr = afterMatch.value;
        }

        if (er.type === Constants.SYS_DATETIME_DATE) {
            pr = this.config.dateParser.parse(er, referenceTime);
            if (pr.value === null) {
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

        // change the type at last for the after or before mode
        pr.type = `${this.parserTypeName}.${this.determineDateTimeType(er.type, hasBefore, hasAfter)}`;

        return pr;
    }

    protected getParseResult(extractorResult: ExtractResult, referenceDate: Date): DateTimeParseResult | null {
        let extractorType = extractorResult.type;
        if (extractorType === Constants.SYS_DATETIME_DATE) {
            let pr = this.config.dateParser.parse(extractorResult, referenceDate);
            if (!pr || !pr.value) return this.config.holidayParser.parse(extractorResult, referenceDate);
            return pr;
        }
        if (extractorType === Constants.SYS_DATETIME_TIME) {
            return this.config.timeParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_DATETIME) {
            return this.config.dateTimeParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_DATEPERIOD) {
            return this.config.datePeriodParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_TIMEPERIOD) {
            return this.config.timePeriodParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_DATETIMEPERIOD) {
            return this.config.dateTimePeriodParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_DURATION) {
            return this.config.durationParser.parse(extractorResult, referenceDate);
        }
        if (extractorType === Constants.SYS_DATETIME_SET) {
            return this.config.setParser.parse(extractorResult, referenceDate);
        }
        return null;
    }

    private determineDateTimeType(type: string, hasBefore: boolean, hasAfter: boolean): string {
        if (hasBefore || hasAfter) {
            if (type === Constants.SYS_DATETIME_DATE) return Constants.SYS_DATETIME_DATEPERIOD;
            if (type === Constants.SYS_DATETIME_TIME) return Constants.SYS_DATETIME_TIMEPERIOD;
            if (type === Constants.SYS_DATETIME_DATETIME) return Constants.SYS_DATETIME_DATETIMEPERIOD;
        }
        return type;
    }

    private dateTimeResolution(slot: DateTimeParseResult, hasBefore: boolean, hasAfter: boolean): Map<string, any> {
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

        if (!StringUtility.isNullOrEmpty(timex)) result.set('timex', timex);
        if (!StringUtility.isNullOrEmpty(comment)) result.set('Comment', comment);
        if (!StringUtility.isNullOrEmpty(mod)) result.set('Mod', mod);
        if (!StringUtility.isNullOrEmpty(type)) result.set('type', type);

        let futureResolution = value.futureResolution;
        let pastResolution = value.pastResolution;

        let future = this.generateFromResolution(type, futureResolution, mod);
        let past = this.generateFromResolution(type, pastResolution, mod);

        let futureValues = Array.from(future.values()).sort();
        let pastValues = Array.from(past.values()).sort();
        if (futureValues.length === pastValues.length && futureValues.every((v, i) => v === pastValues[i])) {
            if (past.size > 0) result.set('resolve', past);
        } else {
            if (past.size > 0) result.set('resolveToPast', past);
            if (future.size > 0) result.set('resolveToFuture', future);
        }

        if (comment && comment === 'ampm') {
            if (result.has('resolve')) {
                this.resolveAMPM(result, 'resolve');
            } else {
                this.resolveAMPM(result, 'resolveToPast');
                this.resolveAMPM(result, 'resolveToFuture');
            }
        }

        if (isLunar) result.set('isLunar', isLunar);

        result.forEach((value, key) => {
            if (value instanceof Map) {
                let newValues = new Map<string, string>();
                if (!StringUtility.isNullOrEmpty(timex)) newValues.set('timex', timex);
                if (!StringUtility.isNullOrEmpty(type)) newValues.set('type', type);
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

    private generateFromResolution(type: string, resolutions: Map<string, string>, mod: string): Map<string, string> {
        let result = new Map<string, string>();
        switch (type) {
            case Constants.SYS_DATETIME_DATETIME:
            this.addSingleDateTimeToResolution(resolutions, TimeTypeConstants.DATETIME, mod, result);
            break;
            case Constants.SYS_DATETIME_TIME:
            this.addSingleDateTimeToResolution(resolutions, TimeTypeConstants.TIME, mod, result);
            break;
            case Constants.SYS_DATETIME_DATE:
            this.addSingleDateTimeToResolution(resolutions, TimeTypeConstants.DATE, mod, result);
            break;
            case Constants.SYS_DATETIME_DURATION:
            if (resolutions.has(TimeTypeConstants.DURATION)) {
                result.set(TimeTypeConstants.VALUE, resolutions.get(TimeTypeConstants.DURATION));
            }
            break;
            case Constants.SYS_DATETIME_TIMEPERIOD:
            this.addPeriodToResolution(resolutions, TimeTypeConstants.START_TIME, TimeTypeConstants.END_TIME, mod, result);
            break;
            case Constants.SYS_DATETIME_DATEPERIOD:
            this.addPeriodToResolution(resolutions, TimeTypeConstants.START_DATE, TimeTypeConstants.END_DATE, mod, result);
            break;
            case Constants.SYS_DATETIME_DATETIMEPERIOD:
            this.addPeriodToResolution(resolutions, TimeTypeConstants.START_DATETIME, TimeTypeConstants.END_DATETIME, mod, result);
            break;
        }
        return result;
    }

    private addSingleDateTimeToResolution(resolutions: Map<string, string>, type: string, mod: string, result: Map<string, string>) {
        let key = TimeTypeConstants.VALUE;
        let value = resolutions.get(type);
        if (!value || this.dateMinValue === value || this.dateTimeMinValue === value) return;

        if (!StringUtility.isNullOrEmpty(mod)) {
            if (mod === TimeTypeConstants.beforeMod) key = TimeTypeConstants.END;
            else if (mod === TimeTypeConstants.afterMod) key = TimeTypeConstants.START;
        }
        result.set(key, value);
    }

    private addPeriodToResolution(resolutions: Map<string, string>, startType: string, endType: string, mod: string, result: Map<string, string>) {
        let start = resolutions.get(startType);
        let end = resolutions.get(endType);
        if (!StringUtility.isNullOrEmpty(mod)) {
            if (mod === TimeTypeConstants.beforeMod) {
                result.set(TimeTypeConstants.END, start);
                return;
            }
            if (mod === TimeTypeConstants.afterMod) {
                result.set(TimeTypeConstants.START, end);
                return;
            }
        }
        if (StringUtility.isNullOrEmpty(start) || StringUtility.isNullOrEmpty(end)) return;

        result.set(TimeTypeConstants.START, start);
        result.set(TimeTypeConstants.END, end);
    }

    private resolveAMPM(valuesMap: Map<string, any>, keyName: string) {
        if (!valuesMap.has(keyName)) return;

        let resolution: Map<string, string> = valuesMap.get(keyName);
        if (!resolution.has('timex')) return;

        let timex = resolution.get('timex');
        valuesMap.delete(keyName);
        valuesMap.set(keyName + 'Am', resolution);

        let resolutionPm = new Map<string, string>();
        switch (valuesMap.get('type')) {
            case Constants.SYS_DATETIME_TIME:
            resolutionPm.set(TimeTypeConstants.VALUE, FormatUtil.toPm(resolution.get(TimeTypeConstants.VALUE)));
            resolutionPm.set('timex', FormatUtil.toPm(timex));
            break;
            case Constants.SYS_DATETIME_DATETIME:
            let splitValue = resolution.get(TimeTypeConstants.VALUE).split(' ');
            resolutionPm.set(TimeTypeConstants.VALUE, `${splitValue[0]} ${FormatUtil.toPm(splitValue[1])}`);
            resolutionPm.set('timex', FormatUtil.allStringToPm(timex));
            break;
            case Constants.SYS_DATETIME_TIMEPERIOD:
            if (resolution.has(TimeTypeConstants.START)) resolutionPm.set(TimeTypeConstants.START, FormatUtil.toPm(resolution.get(TimeTypeConstants.START)));
            if (resolution.has(TimeTypeConstants.END)) resolutionPm.set(TimeTypeConstants.END, FormatUtil.toPm(resolution.get(TimeTypeConstants.END)));
            resolutionPm.set('timex', FormatUtil.allStringToPm(timex));
            break;
            case Constants.SYS_DATETIME_DATETIMEPERIOD:
            if (resolution.has(TimeTypeConstants.START)) {
                let splitValue = resolution.get(TimeTypeConstants.START).split(' ');
                resolutionPm.set(TimeTypeConstants.START, `${splitValue[0]} ${FormatUtil.toPm(splitValue[1])}`);
            }
            if (resolution.has(TimeTypeConstants.END)) {
                let splitValue = resolution.get(TimeTypeConstants.END).split(' ');
                resolutionPm.set(TimeTypeConstants.END, `${splitValue[0]} ${FormatUtil.toPm(splitValue[1])}`);
            }
            resolutionPm.set('timex', FormatUtil.allStringToPm(timex));
            break;
        }
        valuesMap.set(keyName + 'Pm', resolutionPm);
    }
}

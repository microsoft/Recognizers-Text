// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IExtractor, ExtractResult, MetaData } from "@microsoft/recognizers-text";
import { Constants, TimeTypeConstants } from "./constants";
import { RegExpUtility, StringUtility, BaseNumberExtractor } from "@microsoft/recognizers-text-number";
import { IDateTimeParser, DateTimeParseResult } from "./parsers";
import { DateTimeFormatUtil, DateUtils, DateTimeResolutionResult, Token, StringMap } from "./utilities";
import { BaseDateExtractor, BaseDateParser } from "./baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "./baseTime";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "./baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "./baseTimePeriod";
import { IDateTimeExtractor, BaseDateTimeExtractor, BaseDateTimeParser } from "./baseDateTime";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "./baseDateTimePeriod";
import { BaseSetExtractor, BaseSetParser } from "./baseSet";
import { BaseDurationExtractor, BaseDurationParser } from "./baseDuration";
import { BaseHolidayExtractor, BaseHolidayParser } from "./baseHoliday";
import isEqual = require('lodash.isequal');
import { DateTimeOptions } from "./dateTimeRecognizer";

export interface IMergedExtractorConfiguration {
    dateExtractor: IDateTimeExtractor
    timeExtractor: IDateTimeExtractor
    dateTimeExtractor: IDateTimeExtractor
    datePeriodExtractor: IDateTimeExtractor
    timePeriodExtractor: IDateTimeExtractor
    dateTimePeriodExtractor: IDateTimeExtractor
    holidayExtractor: IDateTimeExtractor
    durationExtractor: IDateTimeExtractor
    setExtractor: IDateTimeExtractor
    integerExtractor: BaseNumberExtractor
    afterRegex: RegExp
    beforeRegex: RegExp
    sinceRegex: RegExp
    fromToRegex: RegExp
    singleAmbiguousMonthRegex: RegExp
    ambiguousRangeModifierPrefix: RegExp
    potentialAmbiguousRangeRegex: RegExp
    prepositionSuffixRegex: RegExp
    numberEndingPattern: RegExp
    unspecificDatePeriodRegex: RegExp
    filterWordRegexList: RegExp[]
    AmbiguityFiltersDict: Map<RegExp, RegExp>
}

export class BaseMergedExtractor implements IDateTimeExtractor {
    protected readonly config: IMergedExtractorConfiguration;
    protected readonly options: DateTimeOptions;

    private static readonly NumberOrConnectorRegex: RegExp = new RegExp('^[0-9-]+$');

    constructor(config: IMergedExtractorConfiguration, options: DateTimeOptions) {
        this.config = config;
        this.options = options;
    }

    extract(source: string, refDate: Date): ExtractResult[] {
        if (!refDate) {
            refDate = new Date();
        }
        let referenceDate = refDate;

        let result: ExtractResult[] = new Array<ExtractResult>();
        this.addTo(result, this.config.dateExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.timeExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.datePeriodExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.durationExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.timePeriodExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.dateTimePeriodExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.dateTimeExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.setExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.holidayExtractor.extract(source, referenceDate), source);

        // this should be at the end since if need the extractor to determine the previous text contains time or not
        this.addTo(result, this.numberEndingRegexMatch(source, result), source);

        result = this.filterUnspecificDatePeriod(result);

        // Remove common ambiguous cases
        result = this.filterAmbiguity(result, source);

        this.addMod(result, source);

        // filtering
        if ((this.options & DateTimeOptions.Calendar) !== 0) {
            this.checkCalendarFilterList(result, source);
        }

        result = result.sort((a, b) => a.start - b.start);
        return result;
    }

    private checkCalendarFilterList(ers: ExtractResult[], text: string) {
        for (let er of ers.reverse()) {
            for (let negRegex of this.config.filterWordRegexList) {
                let match = RegExpUtility.getMatches(negRegex, er.text).pop();
                if (match) {
                    ers.splice(ers.indexOf(er));
                }
            }
        }
    }

    // handle cases like "move 3pm appointment to 4"
    private numberEndingRegexMatch(text: string, extractResults: ExtractResult[]): ExtractResult[] {
        let tokens = new Array<Token>();

        extractResults.forEach(extractResult => {
            if (extractResult.type === Constants.SYS_DATETIME_TIME
                || extractResult.type === Constants.SYS_DATETIME_DATETIME) {
                let stringAfter = text.substring(extractResult.start + extractResult.length);
                let match = RegExpUtility.getMatches(this.config.numberEndingPattern, stringAfter);
                if (match != null && match.length) {
                    let newTime = match[0].groups("newTime");
                    let numRes = this.config.integerExtractor.extract(newTime.value);
                    if (numRes.length === 0) {
                        return;
                    }

                    let startPosition = extractResult.start + extractResult.length + newTime.index;
                    tokens.push(new Token(startPosition, startPosition + newTime.length));
                }
            }
        });

        return Token.mergeAllTokens(tokens, text, Constants.SYS_DATETIME_TIME);
    }

    protected addTo(destination: ExtractResult[], source: ExtractResult[], text: string) {
        source.forEach(value => {
            if (this.options === DateTimeOptions.SkipFromToMerge && this.shouldSkipFromMerge(value)) {
                return;
            }

            let isFound = false;
            let overlapIndexes = new Array<number>();
            let firstIndex = -1;
            destination.forEach((dest, index) => {
                if (ExtractResult.isOverlap(dest, value)) {
                    isFound = true;
                    if (ExtractResult.isCover(dest, value)) {
                        if (firstIndex === -1) {
                            firstIndex = index;
                        }
                        overlapIndexes.push(index);
                    }
                    else {
                        return;
                    }
                }
            });
            if (!isFound) {
                destination.push(value);
            }
            else if (overlapIndexes.length) {
                let tempDst = new Array<ExtractResult>();
                for (let i = 0; i < destination.length; i++) {
                    if (overlapIndexes.indexOf(i) === -1) {
                        tempDst.push(destination[i]);
                    }
                }

                // insert at the first overlap occurrence to keep the order
                tempDst.splice(firstIndex, 0, value);
                destination.length = 0;
                destination.push.apply(destination, tempDst);
            }
        });
    }

    private shouldSkipFromMerge(er: ExtractResult): boolean {
        return RegExpUtility.getMatches(this.config.fromToRegex, er.text).length > 0;
    }

    private filterUnspecificDatePeriod (ers: ExtractResult[]): ExtractResult[] {
        ers = ers.filter(er => !RegExpUtility.isMatch(this.config.unspecificDatePeriodRegex, er.text));
        return ers;
    }

    private filterAmbiguity(extractResults: ExtractResult[], text: string): ExtractResult[] {
            if (this.config.AmbiguityFiltersDict != null) {
                for (let [regexKey, regexValue] of this.config.AmbiguityFiltersDict) {
                    extractResults.forEach(extractResult => {
                        if (RegExpUtility.isMatch(regexKey, extractResult.text)) {
                            let matches = RegExpUtility.getMatches(regexValue, text);
                            extractResults = extractResults.filter(er => !matches.some(m => m.index < er.start + er.length && m.index + m.length > er.start));
                        }
                    });
                }
            }

            // @TODO: Refactor to remove this method and use the general ambiguity filter approach
            extractResults = extractResults.filter(er => !(RegExpUtility.isMatch(BaseMergedExtractor.NumberOrConnectorRegex, er.text) &&
                    (text.substring(0, er.start).trim().endsWith("-") || text.substring((er.start + er.length)).trim().startsWith("-"))));

            return extractResults;
        }

    private filterAmbiguousSingleWord(er: ExtractResult, text: string): boolean {
        let matches = RegExpUtility.getMatches(this.config.singleAmbiguousMonthRegex, er.text.toLowerCase());
        if (matches.length) {
            let stringBefore = text.substring(0, er.start).replace(/\s+$/, '');
            matches = RegExpUtility.getMatches(this.config.prepositionSuffixRegex, stringBefore);
            if (!matches.length) {
                return true;
            }
        }
        return false;
    }

    protected addMod(ers: ExtractResult[], source: string) {
        let lastEnd = 0;
        ers.forEach(er => {
            let success = this.tryMergeModifierToken(er, this.config.beforeRegex, source);
            if (!success) {
                success = this.tryMergeModifierToken(er, this.config.afterRegex, source);
            }

            if (!success) {
                // SinceRegex in English contains the term "from" which is potentially ambiguous with ranges in the form "from X to Y"
                success = this.tryMergeModifierToken(er, this.config.sinceRegex, source, true);
            }
        });
    }

    private tryMergeModifierToken(er:ExtractResult, regex: RegExp, source: string, potentialAmbiguity:boolean = false): boolean {
        let beforeStr = source.substr(0, er.start).toLowerCase();

        // Avoid adding mod for ambiguity cases, such as "from" in "from ... to ..." should not add mod
        if (potentialAmbiguity && this.config.ambiguousRangeModifierPrefix &&
            RegExpUtility.isMatch(this.config.ambiguousRangeModifierPrefix, beforeStr)) {
            let matches = RegExpUtility.getMatches(this.config.potentialAmbiguousRangeRegex, source);
            if (matches.find(m => m.index < er.start + er.length && m.index + m.length > er.start)) {
                return false
            }
        }

        let token = this.hasTokenIndex(beforeStr.trim(), regex);
        if (token.matched) {
            let modLength = beforeStr.length - token.index;
            er.length += modLength;
            er.start -= modLength;
            er.text = source.substr(er.start, er.length);
            
            er.metaData = this.assignModMetadata(er.metaData);

            return true;
        }

        return false;
    }

    private assignModMetadata(metadata: MetaData): MetaData {

        if (metadata === undefined || metadata === null) {
            metadata = new MetaData();
            metadata.HasMod = true;
        } else {
            metadata.HasMod = true;
        }

        return metadata
    }

    private hasTokenIndex(source: string, regex: RegExp): { matched: boolean, index: number } {
        // This part is different from C# because no Regex RightToLeft option in JS
        let result = { matched: false, index: -1 };
        let matchResult = RegExpUtility.getMatches(regex, source);
        let match = matchResult[matchResult.length - 1];
        if (match && !source.substr(match.index + match.length).trim().length) {
            result.matched = true;
            result.index = match.index;
        }
        return result;
    }
}

export interface IMergedParserConfiguration {
    beforeRegex: RegExp
    afterRegex: RegExp
    sinceRegex: RegExp
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

    protected readonly config: IMergedParserConfiguration;
    private readonly options: DateTimeOptions;

    private readonly dateDefaultValue = DateTimeFormatUtil.formatDate(new Date(1, 0, 1, 0, 0, 0, 0));
    private readonly dateMinValue = DateTimeFormatUtil.formatDate(DateUtils.minValue());
    private readonly dateTimeMinValue = DateTimeFormatUtil.formatDateTime(DateUtils.minValue());

    constructor(config: IMergedParserConfiguration, options: DateTimeOptions) {
        this.config = config;
        this.options = options;
    }

    parse(er: ExtractResult, refTime?: Date): DateTimeParseResult | null {
        let referenceTime = refTime || new Date();
        let pr: DateTimeParseResult = null;

        // push, save teh MOD string
        let hasBefore = false;
        let hasAfter = false;
        let hasSince = false;
        let modStr = "";

        if (er.metaData !== null && er.metaData !== undefined && er.metaData.HasMod) {
            let beforeMatch = RegExpUtility.getMatches(this.config.beforeRegex, er.text).shift();
            let afterMatch = RegExpUtility.getMatches(this.config.afterRegex, er.text).shift();
            let sinceMatch = RegExpUtility.getMatches(this.config.sinceRegex, er.text).shift();
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
            else if (sinceMatch && sinceMatch.index === 0) {
                hasSince = true;
                er.start += sinceMatch.length;
                er.length -= sinceMatch.length;
                er.text = er.text.substring(sinceMatch.length);
                modStr = sinceMatch.value;
            }
        }         

        if (er.type === Constants.SYS_DATETIME_DATE) {
            if (er.metaData !== null && er.metaData !== undefined && er.metaData.IsHoliday) {
                pr = this.config.holidayParser.parse(er, referenceTime);
            }
            else {
                pr = this.config.dateParser.parse(er, referenceTime);
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
            val.mod = this.combineMod(val.mod, TimeTypeConstants.beforeMod);
            pr.value = val;
        }

        if (hasAfter && pr.value !== null) {
            pr.length += modStr.length;
            pr.start -= modStr.length;
            pr.text = modStr + pr.text;
            let val = pr.value;
            val.mod = this.combineMod(val.mod, TimeTypeConstants.afterMod);
            pr.value = val;
        }

        if (hasSince && pr.value !== null) {
            pr.length += modStr.length;
            pr.start -= modStr.length;
            pr.text = modStr + pr.text;
            let val = pr.value;
            val.mod = this.combineMod(val.mod, TimeTypeConstants.sinceMod);
            pr.value = val;
        }

        if ((this.options & DateTimeOptions.SplitDateAndTime) === DateTimeOptions.SplitDateAndTime
            && pr.value && pr.value.subDateTimeEntities != null) {
            pr.value = this.dateTimeResolutionForSplit(pr);
        }
        else {
            pr = this.setParseResult(pr, hasBefore, hasAfter, hasSince);
        }

        return pr;
    }

    public setParseResult(slot: DateTimeParseResult, hasBefore: boolean, hasAfter: boolean, hasSince: boolean): DateTimeParseResult {
        let hasRangeChangingMod = hasBefore || hasAfter || hasSince;
        slot.value = this.dateTimeResolution(slot, hasBefore, hasAfter, hasSince);
        // change the type at last for the after or before mode
        slot.type = `${this.parserTypeName}.${this.determineDateTimeType(slot.type, hasRangeChangingMod)}`;
        return slot;
    }

    protected getParseResult(extractorResult: ExtractResult, referenceDate: Date): DateTimeParseResult | null {
        let extractorType = extractorResult.type;
        if (extractorType === Constants.SYS_DATETIME_DATE) {
            if (extractorResult.metaData !== null && extractorResult.metaData !== undefined && extractorResult.metaData.IsHoliday) {
                return this.config.holidayParser.parse(extractorResult, referenceDate);
            }
            else {
                return this.config.dateParser.parse(extractorResult, referenceDate);
            }
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
    
    protected combineMod(originalMod: string, newMod: string): string {
        let combinedMod = newMod;
        if (originalMod) {
            combinedMod = newMod + "-" + originalMod;
        }
        
        return combinedMod;
    }

    protected determineDateTimeType(type: string, hasMod: boolean): string {
        if ((this.options & DateTimeOptions.SplitDateAndTime) === DateTimeOptions.SplitDateAndTime) {
            if (type === Constants.SYS_DATETIME_DATETIME) {
                return Constants.SYS_DATETIME_TIME;
            }
        }
        else {
            if (hasMod) {
                if (type === Constants.SYS_DATETIME_DATE) {
                    return Constants.SYS_DATETIME_DATEPERIOD;
                }
                if (type === Constants.SYS_DATETIME_TIME) {
                    return Constants.SYS_DATETIME_TIMEPERIOD;
                }
                if (type === Constants.SYS_DATETIME_DATETIME) {
                    return Constants.SYS_DATETIME_DATETIMEPERIOD;
                }
            }
        }
        return type;
    }

    protected determineSourceEntityType(sourceType: string, newType: string, hasMod: boolean): string | null {
        if (!hasMod) {
            return null;
        }

        if (newType !== sourceType) {
            return Constants.SYS_DATETIME_DATETIMEPOINT;
        }

        if (newType === Constants.SYS_DATETIME_DATEPERIOD) {
            return Constants.SYS_DATETIME_DATETIMEPERIOD;
        }
    }

    public dateTimeResolutionForSplit(slot: DateTimeParseResult): DateTimeParseResult[] {
        let results = new Array<DateTimeParseResult>();
        if (slot.value.subDateTimeEntities != null) {
            let subEntities = slot.value.subDateTimeEntities;
            for (let subEntity of subEntities) {
                let result = subEntity;
                result.start += slot.start;
                results.push(...this.dateTimeResolutionForSplit(result));
            }
        }
        else {
            slot.value = this.dateTimeResolution(slot, false, false, false);
            slot.type = `${this.parserTypeName}.${this.determineDateTimeType(slot.type, false)}`;
            results.push(slot);
        }
        return results;
    }

    protected dateTimeResolution(slot: DateTimeParseResult, hasBefore: boolean, hasAfter: boolean, hasSince: boolean): { [s: string]: StringMap[]; } {
        if (!slot) {
            return null;
        }

        let result = new Map<string, any>();
        let resolutions = new Array<StringMap>();

        let type = slot.type;
        let outputType = this.determineDateTimeType(type, hasBefore || hasAfter || hasSince);
        let sourceEntity = this.determineSourceEntityType(type, outputType, hasBefore || hasAfter || hasSince);
        let timex = slot.timexStr;

        let value: DateTimeResolutionResult = slot.value;
        if (!value) {
            return null;
        }

        let isLunar = value.isLunar;
        let mod = value.mod;
        let comment = value.comment;

        // the following should added to res first since the ResolveAmPm is using these fields
        this.addResolutionFieldsAny(result, Constants.TimexKey, timex);
        this.addResolutionFieldsAny(result, Constants.CommentKey, comment);
        this.addResolutionFieldsAny(result, Constants.ModKey, mod);
        this.addResolutionFieldsAny(result, Constants.TypeKey, outputType);
        this.addResolutionFieldsAny(result, Constants.IsLunarKey, isLunar ? String(isLunar) : "");

        let futureResolution = value.futureResolution;
        let pastResolution = value.pastResolution;

        let future = this.generateFromResolution(type, futureResolution, mod);
        let past = this.generateFromResolution(type, pastResolution, mod);

        let futureValues = Array.from(this.getValues(future)).sort();
        let pastValues = Array.from(this.getValues(past)).sort();
        if (isEqual(futureValues, pastValues)) {
            if (pastValues.length > 0) {
                this.addResolutionFieldsAny(result, Constants.ResolveKey, past);
            }
        }
        else {
            if (pastValues.length > 0) {
                this.addResolutionFieldsAny(result, Constants.ResolveToPastKey, past);
            }
            if (futureValues.length > 0) {
                this.addResolutionFieldsAny(result, Constants.ResolveToFutureKey, future);
            }
        }

        if (comment && comment === 'ampm') {
            if (result.has('resolve')) {
                this.resolveAMPM(result, 'resolve');
            }
            else {
                this.resolveAMPM(result, 'resolveToPast');
                this.resolveAMPM(result, 'resolveToFuture');
            }
        }

        if (comment && this.HasDoubleTimex(comment)) {
            this.processDoubleTimex(result, Constants.ResolveToFutureKey, Constants.ResolveToPastKey, timex);
        }

        result.forEach((value, key) => {
            if (this.isObject(value)) {
                // is "StringMap"
                let newValues = {};

                this.addResolutionFields(newValues, Constants.TimexKey, timex);
                this.addResolutionFields(newValues, Constants.ModKey, mod);
                this.addResolutionFields(newValues, Constants.TypeKey, outputType);
                this.addResolutionFields(newValues, Constants.IsLunarKey, isLunar ? String(isLunar) : "");
                this.addResolutionFields(newValues, Constants.SourceEntity, sourceEntity);

                Object.keys(value).forEach((innerKey) => {
                    newValues[innerKey] = value[innerKey];
                });

                resolutions.push(newValues);
            }
        });

        if (Object.keys(past).length === 0 && Object.keys(future).length === 0) {
            let o = {};
            o['timex'] = timex;
            o['type'] = outputType;
            o['value'] = 'not resolved';
            resolutions.push(o);
        }
        return {
            values: resolutions
        };
    }

    protected isObject(o: any) {
        return (!!o) && (o.constructor === Object);
    }

    protected addResolutionFieldsAny(dic: Map<string, any>, key: string, value: any) {
        if (value instanceof String) {
            if (!StringUtility.isNullOrEmpty(value as string)) {
                dic.set(key, value);
            }
        }
        else {
            dic.set(key, value);
        }
    }

    protected addResolutionFields(dic: StringMap, key: string, value: string) {
        if (!StringUtility.isNullOrEmpty(value)) {
            dic[key] = value;
        }
    }

    protected generateFromResolution(type: string, resolutions: StringMap, mod: string): StringMap {
        let result = {};
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
                if (resolutions.hasOwnProperty(TimeTypeConstants.DURATION)) {
                    result[TimeTypeConstants.VALUE] = resolutions[TimeTypeConstants.DURATION];
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

    private addSingleDateTimeToResolution(resolutions: StringMap, type: string, mod: string, result: StringMap) {
        let key = TimeTypeConstants.VALUE;
        let value = resolutions[type];
        if (!value || this.dateMinValue === value || this.dateTimeMinValue === value || value.startsWith(this.dateDefaultValue)) {
            return;
        }

        if (!StringUtility.isNullOrEmpty(mod)) {
            if (mod === TimeTypeConstants.beforeMod) {
                key = TimeTypeConstants.END;
            }
            else if (mod === TimeTypeConstants.afterMod) {
                key = TimeTypeConstants.START;
            }
            else if (mod === TimeTypeConstants.sinceMod) {
                key = TimeTypeConstants.START;
            }
        }

        result[key] = value;
    }

    private addPeriodToResolution(resolutions: StringMap, startType: string, endType: string, mod: string, result: StringMap) {
        let start = resolutions[startType];
        let end = resolutions[endType];
        if (start === Constants.InvalidDateString || end === Constants.InvalidDateString) {
            return;
        }

        if (!StringUtility.isNullOrEmpty(mod)) {
            // For the 'before' mod
            // 1. Cases like "Before December", the start of the period should be the end of the new period, not the start
            // 2. Cases like "More than 3 days before today", the date point should be the end of the new period
            if (mod.startsWith(TimeTypeConstants.beforeMod)) {
                if (!StringUtility.isNullOrEmpty(start) && !StringUtility.isNullOrEmpty(end) && !mod.endsWith(Constants.LATE_MOD)) {
                    result[TimeTypeConstants.END] = start;
                }
                else {
                    result[TimeTypeConstants.END] = end;
                }
                return;
            }

            // For the 'after' mod
            // 1. Cases like "After January". the end of the period should be the start of the new period, not the end
            // 2. Cases like "More than 3 days after today", the date point should be the start of the new period
            if (mod.startsWith(TimeTypeConstants.afterMod)) {
                if (!StringUtility.isNullOrEmpty(start) && !StringUtility.isNullOrEmpty(end) && !mod.endsWith(Constants.EARLY_MOD)) {
                    result[TimeTypeConstants.START] = end;
                }
                else {
                    result[TimeTypeConstants.START] = start;
                }
                return;
            }
            if (mod === TimeTypeConstants.sinceMod) {
                result[TimeTypeConstants.START] = start;
                return;
            }
        }

        if (StringUtility.isNullOrEmpty(start) || StringUtility.isNullOrEmpty(end)) {
            return;
        }

        result[TimeTypeConstants.START] = start;
        result[TimeTypeConstants.END] = end;
    }

    protected getValues(obj: any): any[] {
        return Object.keys(obj).map(key => obj[key]);
    }

    protected processDoubleTimex(resolutionDic: Map<string, any>, futureKey: string, pastKey: string, originTimex: string) {
        let timexes = originTimex.split(Constants.CompositeTimexDelimiter);

        if (!resolutionDic.has(futureKey) || !resolutionDic.has(pastKey) || timexes.length != 2)
        {
            return;
        }

        let futureResolution: StringMap = resolutionDic.get(futureKey);
        let pastResolution: StringMap = resolutionDic.get(pastKey);
        futureResolution[Constants.TimexKey] = timexes[0];
        pastResolution[Constants.TimexKey] = timexes[1];
    }
    
    private HasDoubleTimex(comment: string): boolean {
        return comment === Constants.Comment_DoubleTimex;
    }

    protected resolveAMPM(valuesMap: Map<string, any>, keyName: string) {
        if (!valuesMap.has(keyName)) {
            return;
        }

        let resolution: StringMap = valuesMap.get(keyName);
        if (!valuesMap.has('timex')) {
            return;
        }

        let timex = valuesMap.get('timex');
        valuesMap.delete(keyName);
        valuesMap.set(keyName + 'Am', resolution);

        let resolutionPm: StringMap = {};
        switch (valuesMap.get('type')) {
            case Constants.SYS_DATETIME_TIME:
                resolutionPm[TimeTypeConstants.VALUE] = DateTimeFormatUtil.toPm(resolution[TimeTypeConstants.VALUE]);
                resolutionPm['timex'] = DateTimeFormatUtil.toPm(timex);
                break;

            case Constants.SYS_DATETIME_DATETIME:
                let splitValue = resolution[TimeTypeConstants.VALUE].split(' ');
                resolutionPm[TimeTypeConstants.VALUE] = `${splitValue[0]} ${DateTimeFormatUtil.toPm(splitValue[1])}`;
                resolutionPm['timex'] = DateTimeFormatUtil.allStringToPm(timex);
                break;

            case Constants.SYS_DATETIME_TIMEPERIOD:
                if (resolution.hasOwnProperty(TimeTypeConstants.START)) {
                    resolutionPm[TimeTypeConstants.START] = DateTimeFormatUtil.toPm(resolution[TimeTypeConstants.START]);
                }
                if (resolution.hasOwnProperty(TimeTypeConstants.END)) {
                    resolutionPm[TimeTypeConstants.END] = DateTimeFormatUtil.toPm(resolution[TimeTypeConstants.END]);
                }
                resolutionPm['timex'] = DateTimeFormatUtil.allStringToPm(timex);
                break;

            case Constants.SYS_DATETIME_DATETIMEPERIOD:
                if (resolution.hasOwnProperty(TimeTypeConstants.START)) {
                    let splitValue = resolution[TimeTypeConstants.START].split(' ');
                    resolutionPm[TimeTypeConstants.START] = `${splitValue[0]} ${DateTimeFormatUtil.toPm(splitValue[1])}`;
                }

                if (resolution.hasOwnProperty(TimeTypeConstants.END)) {
                    let splitValue = resolution[TimeTypeConstants.END].split(' ');
                    resolutionPm[TimeTypeConstants.END] = `${splitValue[0]} ${DateTimeFormatUtil.toPm(splitValue[1])}`;
                }

                resolutionPm['timex'] = DateTimeFormatUtil.allStringToPm(timex);
                break;
        }

        valuesMap.set(keyName + 'Pm', resolutionPm);
    }
}

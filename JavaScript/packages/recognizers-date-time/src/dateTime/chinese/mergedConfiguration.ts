// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
declare var require: any

import { IMergedExtractorConfiguration, BaseMergedExtractor, IMergedParserConfiguration, BaseMergedParser } from "../baseMerged";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseTimeExtractor, BaseTimeParser } from "../baseTime";
import { BaseSetExtractor, BaseSetParser } from "../baseSet";
import { BaseHolidayExtractor, BaseHolidayParser } from "../baseHoliday";
import { BaseDatePeriodExtractor, BaseDatePeriodParser } from "../baseDatePeriod";
import { BaseTimePeriodExtractor, BaseTimePeriodParser } from "../baseTimePeriod";
import { BaseDateTimeExtractor, BaseDateTimeParser } from "../baseDateTime";
import { BaseDateTimePeriodExtractor, BaseDateTimePeriodParser } from "../baseDateTimePeriod";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { ExtractResult, RegExpUtility, MetaData } from "@microsoft/recognizers-text";
import { BaseNumberExtractor } from "@microsoft/recognizers-text-number";
import { ChineseDateTime } from "../../resources/chineseDateTime";
import { ChineseDurationExtractor, ChineseDurationParser } from "./durationConfiguration";
import { ChineseTimeExtractor, ChineseTimeParser } from "./timeConfiguration";
import { ChineseDateExtractor, ChineseDateParser } from "./dateConfiguration";
import { ChineseDateTimeExtractor, ChineseDateTimeParser } from "./dateTimeConfiguration";
import { ChineseTimePeriodExtractor, ChineseTimePeriodParser } from "./timePeriodConfiguration";
import { ChineseDatePeriodExtractor, ChineseDatePeriodParser } from "./datePeriodConfiguration";
import { ChineseDateTimePeriodExtractor, ChineseDateTimePeriodParser } from "./dateTimePeriodConfiguration";
import { ChineseSetExtractor, ChineseSetParser } from "./setConfiguration";
import { ChineseHolidayExtractorConfiguration, ChineseHolidayParser } from "./holidayConfiguration";
import { DateTimeOptions } from "../dateTimeRecognizer";
import { IDateTimeParser, DateTimeParseResult } from "../parsers";
import { Constants, TimeTypeConstants } from "../constants";
import { DateTimeFormatUtil, DateUtils, DateTimeResolutionResult, StringMap } from "../utilities";
const lodash = require('lodash');

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
    readonly integerExtractor: BaseNumberExtractor
    readonly afterRegex: RegExp
    readonly beforeRegex: RegExp
    readonly sinceRegex: RegExp
    readonly fromToRegex: RegExp
    readonly singleAmbiguousMonthRegex: RegExp
    readonly prepositionSuffixRegex: RegExp
    readonly ambiguousRangeModifierPrefix: RegExp
    readonly potentialAmbiguousRangeRegex: RegExp
    readonly numberEndingPattern: RegExp
    readonly unspecificDatePeriodRegex: RegExp
    readonly filterWordRegexList: RegExp[]
    readonly AmbiguityFiltersDict: Map<RegExp, RegExp>

    constructor(dmyDateFormat: boolean) {
        this.dateExtractor = new ChineseDateExtractor(dmyDateFormat);
        this.timeExtractor = new ChineseTimeExtractor();
        this.dateTimeExtractor = new ChineseDateTimeExtractor(dmyDateFormat);
        this.datePeriodExtractor = new ChineseDatePeriodExtractor(dmyDateFormat);
        this.timePeriodExtractor = new ChineseTimePeriodExtractor();
        this.dateTimePeriodExtractor = new ChineseDateTimePeriodExtractor(dmyDateFormat);
        this.setExtractor = new ChineseSetExtractor(dmyDateFormat);
        this.holidayExtractor = new BaseHolidayExtractor(new ChineseHolidayExtractorConfiguration());
        this.durationExtractor = new ChineseDurationExtractor();
        this.beforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ParserConfigurationBefore);
        this.afterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ParserConfigurationAfter);
    }
}

export class ChineseMergedExtractor extends BaseMergedExtractor {
    private readonly dayOfMonthRegex: RegExp;
    private readonly sincePrefixRegex: RegExp
    private readonly sinceSuffixRegex: RegExp

    constructor(options: DateTimeOptions, dmyDateFormat: boolean = false) {
        let config = new ChineseMergedExtractorConfiguration(dmyDateFormat);
        super(config, options);
        this.dayOfMonthRegex = RegExpUtility.getSafeRegExp(`^\\d{1,2}号`, 'gi');
        this.sincePrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ParserConfigurationSincePrefix);
        this.sinceSuffixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ParserConfigurationSinceSuffix);
    }

    extract(source: string, refDate: Date): ExtractResult[] {
        if (!refDate) {
            refDate = new Date();
        }
        let referenceDate = refDate;

        let result: ExtractResult[] = new Array<ExtractResult>();
        this.addTo(result, this.config.dateExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.timeExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.durationExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.datePeriodExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.dateTimeExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.timePeriodExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.dateTimePeriodExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.setExtractor.extract(source, referenceDate), source);
        this.addTo(result, this.config.holidayExtractor.extract(source, referenceDate), source);

        result = this.checkBlackList(result, source);

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
            }
            else if (rmIndex >= 0) {
                destination.splice(rmIndex, rmLength);
                destination.splice(0, destination.length, ...this.moveOverlap(destination, er));
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
        return destination.filter((_, i) => duplicated.indexOf(i) < 0);
    }

    // ported from CheckBlackList
    protected checkBlackList(destination: ExtractResult[], source: string) {
        return destination.filter(value => {
            let valueEnd = value.start + value.length;
            if (valueEnd !== source.length) {
                let lastChar = source.substr(valueEnd, 1);
                if (value.text.endsWith('周') && lastChar === '岁') {
                    return false;
                }
            }

            if (RegExpUtility.isMatch(this.dayOfMonthRegex, value.text)) {
                return false;
            }

            return true;
        });
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
                success = this.tryMergeModifierToken(er, this.sincePrefixRegex, source, true, true);
                success = this.tryMergeModifierToken(er, this.sinceSuffixRegex, source, false, true);
            }
        });
    }

    protected tryMergeModifierToken(er:ExtractResult, regex: RegExp, source: string, isPrefix: boolean = false, potentialAmbiguity:boolean = false): boolean {
        let subStr = isPrefix ? source.substr(0, er.start) : source.substr(er.start + er.length);

        // Avoid adding mod for ambiguity cases, such as "from" in "from ... to ..." should not add mod
        if (potentialAmbiguity && this.config.ambiguousRangeModifierPrefix &&
            RegExpUtility.isMatch(this.config.ambiguousRangeModifierPrefix, subStr)) {
            let matches = RegExpUtility.getMatches(this.config.potentialAmbiguousRangeRegex, source);
            if (matches.find(m => m.index < er.start + er.length && m.index + m.length > er.start)) {
                return false
            }
        }

        let token = this.hasTokenIndex(subStr.trim(), regex, isPrefix);
        if (token.matched) {
            let modLength = isPrefix ? subStr.length - token.index : token.index;
            er.length += modLength;
            er.start -= isPrefix ? modLength : 0;
            er.text = source.substr(er.start, er.length);
            
            er.metaData = this.assignModMetadata(er.metaData);

            return true;
        }

        return false;
    }

    protected assignModMetadata(metadata: MetaData): MetaData {

        if (metadata === undefined || metadata === null) {
            metadata = new MetaData();
            metadata.HasMod = true;
        } else {
            metadata.HasMod = true;
        }

        return metadata
    }

    protected hasTokenIndex(source: string, regex: RegExp, isPrefix: boolean = false): { matched: boolean, index: number } {
        // This part is different from C# because no Regex RightToLeft option in JS
        let result = { matched: false, index: -1 };
        let matchResult = RegExpUtility.getMatches(regex, source);
        let index = isPrefix ? matchResult.length - 1 : 0;
        let match = matchResult.length > 0 ? matchResult[index]: false;
        if (match) {
            let leftStr = isPrefix ? source.substr(match.index + match.length).trim() : source.substr(0, match.index).trim()
            if (!leftStr.length) {
                result.matched = true;
                result.index = match.index + (isPrefix ? 0 : match.length);
            }            
        }
        return result;
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

    constructor(dmyDateFormat: boolean) {
        this.beforeRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ParserConfigurationBefore);
        this.afterRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ParserConfigurationAfter);        

        this.dateParser = new ChineseDateParser(dmyDateFormat);
        this.holidayParser = new ChineseHolidayParser();
        this.timeParser = new ChineseTimeParser();
        this.dateTimeParser = new ChineseDateTimeParser(dmyDateFormat);
        this.datePeriodParser = new ChineseDatePeriodParser(dmyDateFormat);
        this.timePeriodParser = new ChineseTimePeriodParser();
        this.dateTimePeriodParser = new ChineseDateTimePeriodParser(dmyDateFormat);
        this.durationParser = new ChineseDurationParser();
        this.setParser = new ChineseSetParser(dmyDateFormat);
    }
}

export class ChineseMergedParser extends BaseMergedParser {
    constructor(dmyDateFormat: boolean) {
        let config = new ChineseMergedParserConfiguration(dmyDateFormat);
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

        let hasRangeChangingMod = hasBefore || hasAfter || hasSince;
        pr.value = this.dateTimeResolution(pr, hasBefore, hasAfter, hasSince);
        pr.type = `${this.parserTypeName}.${this.determineDateTimeType(er.type, hasRangeChangingMod)}`;

        return pr;
    }
}

export class ChineseFullMergedParser extends BaseMergedParser {
    private readonly sincePrefixRegex: RegExp;
    private readonly sinceSuffixRegex: RegExp;
    
    constructor(dmyDateFormat: boolean = false) {
        let config = new ChineseMergedParserConfiguration(dmyDateFormat);
        super(config, 0);
        this.sincePrefixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ParserConfigurationSincePrefix);
        this.sinceSuffixRegex = RegExpUtility.getSafeRegExp(ChineseDateTime.ParserConfigurationSinceSuffix);
    }

    parse(er: ExtractResult, refTime?: Date): DateTimeParseResult | null {
        let referenceTime = refTime || new Date();
        let pr: DateTimeParseResult = null;

        // push, save teh MOD string
        let hasBefore = false;
        let hasAfter = false;
        let hasSince = false;
        let modStr = '', modStrPrefix = '', modStrSuffix = '';
        let erLength = er.length;

        if (er.metaData !== null && er.metaData !== undefined && er.metaData.HasMod) {
            let beforeMatch = RegExpUtility.getMatches(this.config.beforeRegex, er.text).shift();
            let afterMatch = RegExpUtility.getMatches(this.config.afterRegex, er.text).shift();
            let sincePrefixMatch = RegExpUtility.getMatches(this.sincePrefixRegex, er.text).shift();
            let sinceSuffixMatch = RegExpUtility.getMatches(this.sinceSuffixRegex, er.text).shift();
            if (beforeMatch && beforeMatch.index  + beforeMatch.length === erLength) {
                hasBefore = true;
                er.length -= beforeMatch.length;
                er.text = er.text.substring(0, er.length);
                modStr = beforeMatch.value;
            }
            else if (afterMatch && afterMatch.index  + afterMatch.length === erLength) {
                hasAfter = true;
                er.length -= afterMatch.length;
                er.text = er.text.substring(0, er.length);
                modStr = afterMatch.value;
            }
            else {
                if (sincePrefixMatch && sincePrefixMatch.index === 0) {
                    hasSince = true;
                    er.start += sincePrefixMatch.length;
                    er.length -= sincePrefixMatch.length;
                    er.text = er.text.substring(sincePrefixMatch.length);
                    modStrPrefix = sincePrefixMatch.value;
                }

                if (sinceSuffixMatch && sinceSuffixMatch.index + sinceSuffixMatch.length === erLength) {
                    hasSince = true;
                    er.length -= sinceSuffixMatch.length;
                    er.text = er.text.substring(0, er.length);
                    modStrSuffix = sinceSuffixMatch.value;
                }
            }
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
            pr.text = pr.text + modStr;
            let val = pr.value;
            val.mod = this.combineMod(val.mod, TimeTypeConstants.beforeMod);
            pr.value = val;
        }

        if (hasAfter && pr.value !== null) {
            pr.length += modStr.length;
            pr.text = pr.text + modStr;
            let val = pr.value;
            val.mod = this.combineMod(val.mod, TimeTypeConstants.afterMod);
            pr.value = val;
        }

        if (hasSince && pr.value !== null) {
            pr.length += modStrPrefix.length + modStrSuffix.length;
            pr.start -= modStrPrefix.length;
            pr.text = modStrPrefix + pr.text + modStrSuffix;
            let val = pr.value;
            val.mod = this.combineMod(val.mod, TimeTypeConstants.sinceMod);
            pr.value = val;
        }

        let hasRangeChangingMod = hasBefore || hasAfter || hasSince;
        pr.value = this.dateTimeResolution(pr, hasRangeChangingMod);
        pr.type = `${this.parserTypeName}.${this.determineDateTimeType(er.type, hasRangeChangingMod)}`;

        return pr;
    }

    protected dateTimeResolution(slot: DateTimeParseResult, hasRangeChangingMod: boolean): { [s: string]: StringMap[]; } {
        if (!slot) {
            return null;
        }

        let result = new Map<string, any>();
        let resolutions = new Array<StringMap>();

        let type = slot.type;
        let outputType = this.determineDateTimeType(type, hasRangeChangingMod);
        let timex = slot.timexStr;
        let sourceEntity = this.determineSourceEntityType(type, outputType, hasRangeChangingMod);

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

        let futureResolution = value.futureResolution;
        let pastResolution = value.pastResolution;

        let future = this.generateFromResolution(type, futureResolution, mod);
        let past = this.generateFromResolution(type, pastResolution, mod);

        let futureValues = Array.from(this.getValues(future)).sort();
        let pastValues = Array.from(this.getValues(past)).sort();
        if (lodash.isEqual(futureValues, pastValues)) {
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

        result.forEach((value, key) => {
            if (this.isObject(value)) {
                // is "StringMap"
                let newValues = {};

                this.addResolutionFields(newValues, Constants.TimexKey, timex);
                this.addResolutionFields(newValues, Constants.ModKey, mod);
                this.addResolutionFields(newValues, Constants.TypeKey, outputType);
                this.addResolutionFields(newValues, Constants.SourceEntity, sourceEntity);
                if (isLunar) {
                    this.addResolutionFields(newValues, Constants.IsLunarKey, "True");
                }

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

    protected determineDateTimeType(type: string, hasMod: boolean): string {
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
        return type;
    }

    protected isDurationWithAgoAndLater(er: ExtractResult): boolean {
        return er.metaData && er.metaData.IsDurationWithAgoAndLater;
    }
}
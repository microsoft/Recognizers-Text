import { IMergedExtractorConfiguration, BaseMergedExtractor } from "../baseMerged"
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
import { ChineseDurationExtractor } from "./durationConfiguration"
import { ChineseTimeExtractor } from "./timeConfiguration"
import { ChineseDateExtractor } from "./dateConfiguration"
import { ChineseDateTimeExtractor } from "./dateTimeConfiguration"
import { ChineseTimePeriodExtractor } from "./timePeriodConfiguration"
import { ChineseDatePeriodExtractor } from "./datePeriodConfiguration"
import { ChineseDateTimePeriodExtractor } from "./dateTimePeriodConfiguration"
import { ChineseSetExtractor } from "./setConfiguration"
import { ChineseHolidayExtractorConfiguration } from "./holidayConfiguration"
import { DateTimeOptions } from "../dateTimeRecognizer";

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

    protected addTo(destination: ExtractResult[], source: ExtractResult[], sourceStr: string) {
        source.forEach(er => {
            let isFound = false;
            let rmIndex = -1;
            let rmLength = -1;
            for (let index; index < destination.length; index++) {
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
                destination = destination.splice(rmIndex, rmLength);
                this.moveOverlap(destination, er);
                destination = destination.splice(rmIndex, 0, er);
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
        duplicated.forEach(index => destination = destination.splice(index, 1));
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
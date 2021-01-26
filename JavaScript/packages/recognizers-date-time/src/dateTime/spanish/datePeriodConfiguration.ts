import { IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { BaseNumberParser, BaseNumberExtractor, SpanishIntegerExtractor, SpanishNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { IDatePeriodExtractorConfiguration, IDatePeriodParserConfiguration } from "../baseDatePeriod";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { SpanishDateExtractorConfiguration } from "./dateConfiguration";
import { SpanishDurationExtractorConfiguration } from "./durationConfiguration";
import { BaseDateTime } from "../../resources/baseDateTime";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { IDateTimeExtractor } from "../baseDateTime";

export class SpanishDatePeriodExtractorConfiguration implements IDatePeriodExtractorConfiguration {
    readonly simpleCasesRegexes: RegExp[];
    readonly illegalYearRegex: RegExp;
    readonly YearRegex: RegExp;
    readonly tillRegex: RegExp;
    readonly followedUnit: RegExp;
    readonly numberCombinedWithUnit: RegExp;
    readonly pastRegex: RegExp;
    readonly futureRegex: RegExp;
    readonly weekOfRegex: RegExp;
    readonly monthOfRegex: RegExp;
    readonly dateUnitRegex: RegExp;
    readonly inConnectorRegex: RegExp;
    readonly rangeUnitRegex: RegExp;
    readonly datePointExtractor: IDateTimeExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly numberParser: BaseNumberParser;
    readonly durationExtractor: IDateTimeExtractor;

    readonly fromRegex: RegExp;
    readonly RangeConnectorRegex: RegExp;
    readonly betweenRegex: RegExp;
    readonly nowRegex: RegExp

    constructor(dmyDateFormat: boolean) {
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleCasesRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.DayBetweenRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleCasesRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.DayBetweenRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.OneWordPeriodRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.MonthWithYearRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.MonthNumWithYearRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.YearRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfMonthRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfYearRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.MonthFrontBetweenRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegexYearFront),
            RegExpUtility.getSafeRegExp(SpanishDateTime.AllHalfYearRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.SeasonRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.RestOfDateRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.getSafeRegExp(SpanishDateTime.WeekWithWeekDayRangeRegex)
        ];
        this.illegalYearRegex = RegExpUtility.getSafeRegExp(BaseDateTime.IllegalYearRegex);
        this.YearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.YearRegex);
        this.tillRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TillRegex);
        this.followedUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.FollowedDateUnit);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.NumberCombinedWithDateUnit);
        this.pastRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FutureRegex);
        this.weekOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfRegex);
        this.monthOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthOfRegex);
        this.dateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateUnitRegex);
        this.inConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InConnectorRegex);
        this.rangeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeUnitRegex);

        this.fromRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromRegex);
        this.RangeConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeConnectorRegex);
        this.betweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BetweenRegex);
        this.nowRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NowRegex);

        this.datePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration(dmyDateFormat));
        this.integerExtractor = new SpanishIntegerExtractor();
        this.numberParser = new BaseNumberParser(new SpanishNumberParserConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
    }

    getFromTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.fromRegex, source);
    }

    getBetweenTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.betweenRegex, source);
    }

    hasConnectorToken(source: string): boolean {
        return RegExpUtility.getFirstMatchIndex(this.RangeConnectorRegex, source).matched;
    }
}

export class SpanishDatePeriodParserConfiguration implements IDatePeriodParserConfiguration {
    readonly dateExtractor: IDateTimeExtractor;
    readonly dateParser: BaseDateParser;
    readonly durationExtractor: IDateTimeExtractor;
    readonly durationParser: BaseDurationParser;
    readonly monthFrontBetweenRegex: RegExp;
    readonly betweenRegex: RegExp;
    readonly monthFrontSimpleCasesRegex: RegExp;
    readonly simpleCasesRegex: RegExp;
    readonly oneWordPeriodRegex: RegExp;
    readonly monthWithYear: RegExp;
    readonly monthNumWithYear: RegExp;
    readonly yearRegex: RegExp;
    readonly pastRegex: RegExp;
    readonly relativeRegex: RegExp;
    readonly futureRegex: RegExp;
    readonly inConnectorRegex: RegExp;
    readonly weekOfMonthRegex: RegExp;
    readonly weekOfYearRegex: RegExp;
    readonly quarterRegex: RegExp;
    readonly quarterRegexYearFront: RegExp;
    readonly allHalfYearRegex: RegExp;
    readonly seasonRegex: RegExp;
    readonly weekOfRegex: RegExp;
    readonly monthOfRegex: RegExp;
    readonly whichWeekRegex: RegExp;
    readonly restOfDateRegex: RegExp;
    readonly laterEarlyPeriodRegex: RegExp;
    readonly weekWithWeekDayRangeRegex: RegExp;
    readonly unspecificEndOfRangeRegex: RegExp;
    readonly tokenBeforeDate: string;
    readonly dayOfMonth: ReadonlyMap<string, number>;
    readonly monthOfYear: ReadonlyMap<string, number>;
    readonly cardinalMap: ReadonlyMap<string, number>;
    readonly seasonMap: ReadonlyMap<string, string>;
    readonly unitMap: ReadonlyMap<string, string>;

    readonly nextPrefixRegex: RegExp;
    readonly previousPrefixRegex: RegExp;
    readonly thisPrefixRegex: RegExp;
    readonly numberCombinedWithUnit: RegExp;

    readonly cardinalExtractor: IExtractor;
    readonly numberParser: BaseNumberParser;
    readonly nowRegex: RegExp

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.tokenBeforeDate = SpanishDateTime.TokenBeforeDate;
        this.cardinalExtractor = config.cardinalExtractor;
        this.numberParser = config.numberParser;
        this.durationExtractor = config.durationExtractor;
        this.dateExtractor = config.dateExtractor;
        this.durationParser = config.durationParser;
        this.dateParser = config.dateParser;

        this.monthFrontBetweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthFrontBetweenRegex);
        this.betweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DayBetweenRegex);
        this.monthFrontSimpleCasesRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthFrontSimpleCasesRegex);
        this.simpleCasesRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleCasesRegex);
        this.oneWordPeriodRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.OneWordPeriodRegex);
        this.monthWithYear = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthWithYearRegex);
        this.monthNumWithYear = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthNumWithYearRegex);
        this.yearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.YearRegex);
        this.relativeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RelativeRegex);
        this.pastRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FutureRegex);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.DurationNumberCombinedWithUnit);
        this.weekOfMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfMonthRegex);
        this.weekOfYearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfYearRegex);
        this.quarterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegex);
        this.quarterRegexYearFront = RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegexYearFront);
        this.allHalfYearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.AllHalfYearRegex);
        this.seasonRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SeasonRegex);
        this.whichWeekRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WhichWeekRegex);
        this.weekOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfRegex);
        this.monthOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthOfRegex);
        this.restOfDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RestOfDateRegex);
        this.laterEarlyPeriodRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.LaterEarlyPeriodRegex);
        this.weekWithWeekDayRangeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekWithWeekDayRangeRegex);
        this.unspecificEndOfRangeRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.UnspecificEndOfRangeRegex);

        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextPrefixRegex);
        this.previousPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PreviousPrefixRegex);
        this.thisPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ThisPrefixRegex);
        this.nowRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NowRegex);

        this.inConnectorRegex = config.utilityConfiguration.inConnectorRegex;
        this.unitMap = config.unitMap;
        this.cardinalMap = config.cardinalMap;
        this.dayOfMonth = config.dayOfMonth;
        this.monthOfYear = config.monthOfYear;
        this.seasonMap = config.seasonMap;
    }

    getSwiftDayOrMonth(source: string): number {
        let trimedText = source.trim().toLowerCase();
        let swift = 0;

        if (RegExpUtility.getFirstMatchIndex(this.nextPrefixRegex, trimedText).matched) {
            swift = 1;
        }

        if (RegExpUtility.getFirstMatchIndex(this.previousPrefixRegex, trimedText).matched) {
            swift = -1;
        }

        return swift;
    }

    getSwiftYear(source: string): number {
        let trimedText = source.trim().toLowerCase();
        let swift = -10;
        if (RegExpUtility.getFirstMatchIndex(this.nextPrefixRegex, trimedText).matched) {
            swift = 1;
        }

        if (RegExpUtility.getFirstMatchIndex(this.previousPrefixRegex, trimedText).matched) {
            swift = -1;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.thisPrefixRegex, trimedText).matched) {
            swift = 0;
        }

        return swift;
    }

    isFuture(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return RegExpUtility.getFirstMatchIndex(this.thisPrefixRegex, trimedText).matched ||
            RegExpUtility.getFirstMatchIndex(this.nextPrefixRegex, trimedText).matched;
    }

    isYearToDate(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return SpanishDateTime.YearToDateTerms.some(o => trimedText === o);
    }

    isMonthToDate(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return SpanishDateTime.MonthToDateTerms.some(o => trimedText === o);
    }

    isWeekOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return SpanishDateTime.WeekTerms.some(o => trimedText.endsWith(o)) &&
            !SpanishDateTime.WeekendTerms.some(o => trimedText.endsWith(o));
    }

    isWeekend(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return SpanishDateTime.WeekendTerms.some(o => trimedText.endsWith(o));
    }

    isMonthOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return SpanishDateTime.MonthTerms.some(o => trimedText.endsWith(o));
    }

    isYearOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return SpanishDateTime.YearTerms.some(o => trimedText.endsWith(o));
    }

    isLastCardinal(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return RegExpUtility.getFirstMatchIndex(this.previousPrefixRegex, trimedText).matched;
    }
}
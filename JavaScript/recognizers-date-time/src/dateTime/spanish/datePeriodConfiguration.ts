import { BaseNumberExtractor, IExtractor, SpanishIntegerExtractor, RegExpUtility, IParser } from "recognizers-text-number";
import { IDatePeriodExtractorConfiguration, IDatePeriodParserConfiguration } from "../baseDatePeriod";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { SpanishDateExtractorConfiguration } from "./dateConfiguration";
import { SpanishDurationExtractorConfiguration } from "./durationConfiguration";
import { SpanishDateTime } from "../../resources/spanishDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";

export class SpanishDatePeriodExtractorConfiguration implements IDatePeriodExtractorConfiguration {
    readonly simpleCasesRegexes: RegExp[];
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
    readonly datePointExtractor: BaseDateExtractor;
    readonly integerExtractor: BaseNumberExtractor;
    readonly durationExtractor: BaseDurationExtractor;

    readonly fromRegex: RegExp;
    readonly connectorAndRegex: RegExp;
    readonly betweenRegex: RegExp;

    constructor() {
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleCasesRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.DayBetweenRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.SimpleCasesRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.DayBetweenRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.OneWordPeriodRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.MonthWithYearRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.MonthNumWithYearRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.YearRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfMonthRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfYearRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.MonthFrontBetweenRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.MonthFrontSimpleCasesRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegexYearFront, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.SeasonRegex, "gis"),
            RegExpUtility.getSafeRegExp(SpanishDateTime.RestOfDateRegex, "gis")
        ];
        this.tillRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.TillRegex, "gis");
        this.followedUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.FollowedDateUnit, "gis");
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.NumberCombinedWithDateUnit, "gis");
        this.pastRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastRegex, "gis");
        this.futureRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FutureRegex, "gis");
        this.weekOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfRegex, "gis");
        this.monthOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthOfRegex, "gis");
        this.dateUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.DateUnitRegex, "gis");
        this.inConnectorRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.InConnectorRegex, "gis");
        this.rangeUnitRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RangeUnitRegex, "gis");

        this.fromRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FromRegex, "gis");
        this.connectorAndRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ConnectorAndRegex, "gis");
        this.betweenRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.BetweenRegex, "gis");

        this.datePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
        this.integerExtractor = new SpanishIntegerExtractor();
        this.durationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
    }

    getFromTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.fromRegex, source);
    }

    getBetweenTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.betweenRegex, source);
    }

    hasConnectorToken(source: string): boolean {
        return RegExpUtility.getFirstMatchIndex(this.connectorAndRegex, source).matched;
    }
}

export class SpanishDatePeriodParserConfiguration implements IDatePeriodParserConfiguration {
    readonly dateExtractor: BaseDateExtractor;
    readonly dateParser: BaseDateParser;
    readonly durationExtractor: BaseDurationExtractor;
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
    readonly futureRegex: RegExp;
    readonly inConnectorRegex: RegExp;
    readonly weekOfMonthRegex: RegExp;
    readonly weekOfYearRegex: RegExp;
    readonly quarterRegex: RegExp;
    readonly quarterRegexYearFront: RegExp;
    readonly seasonRegex: RegExp;
    readonly weekOfRegex: RegExp;
    readonly monthOfRegex: RegExp;
    readonly whichWeekRegex: RegExp;
    readonly restOfDateRegex: RegExp;
    readonly tokenBeforeDate: string;
    readonly dayOfMonth: ReadonlyMap<string, number>;
    readonly monthOfYear: ReadonlyMap<string, number>;
    readonly cardinalMap: ReadonlyMap<string, number>;
    readonly seasonMap: ReadonlyMap<string, string>;
    readonly unitMap: ReadonlyMap<string, string>;

    readonly nextPrefixRegex: RegExp;
    readonly pastPrefixRegex: RegExp;
    readonly thisPrefixRegex: RegExp;
    readonly numberCombinedWithUnit: RegExp;

    readonly cardinalExtractor: IExtractor;
    readonly numberParser: IParser;

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
        this.pastRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.FutureRegex);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(SpanishDateTime.DurationNumberCombinedWithUnit);
        this.weekOfMonthRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfMonthRegex);
        this.weekOfYearRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfYearRegex);
        this.quarterRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegex);
        this.quarterRegexYearFront = RegExpUtility.getSafeRegExp(SpanishDateTime.QuarterRegexYearFront);
        this.seasonRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.SeasonRegex);
        this.whichWeekRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WhichWeekRegex);
        this.weekOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.WeekOfRegex);
        this.monthOfRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.MonthOfRegex);
        this.restOfDateRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.RestOfDateRegex);

        this.nextPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.NextPrefixRegex);
        this.pastPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.PastPrefixRegex);
        this.thisPrefixRegex = RegExpUtility.getSafeRegExp(SpanishDateTime.ThisPrefixRegex);

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

        if (RegExpUtility.getFirstMatchIndex(this.pastPrefixRegex, trimedText).matched) {
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

        if (RegExpUtility.getFirstMatchIndex(this.pastPrefixRegex, trimedText).matched) {
            swift = -1;
        }
        else if (RegExpUtility.getFirstMatchIndex(this.thisPrefixRegex, trimedText).matched) {
            swift = 0;
        }

        return swift;
    }

    isFuture(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return RegExpUtility.getFirstMatchIndex(this.thisPrefixRegex, trimedText).matched
            || RegExpUtility.getFirstMatchIndex(this.nextPrefixRegex, trimedText).matched;
    }

    isYearToDate(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return trimedText === "año a la fecha"
            || trimedText === "años a la fecha";
    }

    isMonthToDate(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return trimedText === "mes a la fecha"
            || trimedText === "meses a la fecha";
    }

    isWeekOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return trimedText.endsWith("semana")
            && !trimedText.endsWith("fin de semana");
    }

    isWeekend(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return trimedText.endsWith("fin de semana");
    }

    isMonthOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return trimedText.endsWith("mes")
            || trimedText.endsWith("meses");
    }

    isYearOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return trimedText.endsWith("año")
            || trimedText.endsWith("años");
    }

    isLastCardinal(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return RegExpUtility.getFirstMatchIndex(this.pastPrefixRegex, trimedText).matched;
    }
}
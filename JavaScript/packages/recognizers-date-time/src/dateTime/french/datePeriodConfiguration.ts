import { IExtractor, IParser, RegExpUtility } from "@microsoft/recognizers-text";
import { BaseNumberParser, BaseNumberExtractor, FrenchIntegerExtractor, FrenchNumberParserConfiguration } from "@microsoft/recognizers-text-number";
import { IDatePeriodExtractorConfiguration, IDatePeriodParserConfiguration } from "../baseDatePeriod";
import { BaseDateExtractor, BaseDateParser } from "../baseDate";
import { BaseDurationExtractor, BaseDurationParser } from "../baseDuration";
import { FrenchDateExtractorConfiguration } from "./dateConfiguration";
import { FrenchDurationExtractorConfiguration } from "./durationConfiguration";
import { BaseDateTime } from "../../resources/baseDateTime";
import { FrenchDateTime } from "../../resources/frenchDateTime";
import { ICommonDateTimeParserConfiguration } from "../parsers";
import { IDateTimeExtractor } from "../baseDateTime";

export class FrenchDatePeriodExtractorConfiguration implements IDatePeriodExtractorConfiguration {
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
    readonly connectorAndRegex: RegExp;
    readonly beforeRegex: RegExp;

    readonly weekDayOfMonthRegex: RegExp;
    readonly nowRegex: RegExp

    constructor(dmyDateFormat: boolean) {
        this.simpleCasesRegexes = [
            RegExpUtility.getSafeRegExp(FrenchDateTime.SimpleCasesRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.BetweenRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.OneWordPeriodRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.MonthWithYear),
            RegExpUtility.getSafeRegExp(FrenchDateTime.MonthNumWithYear),
            RegExpUtility.getSafeRegExp(FrenchDateTime.YearRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayOfMonthRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.WeekOfYearRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.MonthFrontBetweenRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.MonthFrontSimpleCasesRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.QuarterRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.QuarterRegexYearFront),
            RegExpUtility.getSafeRegExp(FrenchDateTime.AllHalfYearRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.SeasonRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.LaterEarlyPeriodRegex),
            RegExpUtility.getSafeRegExp(FrenchDateTime.WeekWithWeekDayRangeRegex)
        ];
        this.illegalYearRegex = RegExpUtility.getSafeRegExp(BaseDateTime.IllegalYearRegex);
        this.YearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.YearRegex);
        this.tillRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.TillRegex);
        this.followedUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.FollowedDateUnit);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberCombinedWithDateUnit);
        this.pastRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PastSuffixRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextSuffixRegex);
        this.weekOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekOfRegex);
        this.monthOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthOfRegex);
        this.dateUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.DateUnitRegex);
        this.inConnectorRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.InConnectorRegex);
        this.rangeUnitRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RangeUnitRegex);
        this.weekDayOfMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekDayOfMonthRegex);

        this.fromRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.FromRegex);
        this.connectorAndRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.ConnectorAndRegex);
        this.beforeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BeforeRegex2);
        this.nowRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NowRegex);

        this.datePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration(dmyDateFormat));
        this.integerExtractor = new FrenchIntegerExtractor();
        this.numberParser = new BaseNumberParser(new FrenchNumberParserConfiguration());
        this.durationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
    }

    getFromTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.fromRegex, source);
    }

    getBetweenTokenIndex(source: string): { matched: boolean; index: number; } {
        return RegExpUtility.getFirstMatchIndex(this.beforeRegex, source);
    }

    hasConnectorToken(source: string): boolean {
        return RegExpUtility.getFirstMatchIndex(this.connectorAndRegex, source).matched;
    }
}

export class FrenchDatePeriodParserConfiguration implements IDatePeriodParserConfiguration {
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
    readonly relativeRegex: RegExp;
    readonly pastRegex: RegExp;
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
    readonly nextSuffixRegex: RegExp;
    readonly pastSuffixRegex: RegExp;
    readonly numberCombinedWithUnit: RegExp;
    readonly laterEarlyPeriodRegex: RegExp;
    readonly weekWithWeekDayRangeRegex: RegExp;

    readonly cardinalExtractor: IExtractor;
    readonly numberParser: BaseNumberParser;
    readonly nowRegex: RegExp

    constructor(config: ICommonDateTimeParserConfiguration) {
        this.tokenBeforeDate = FrenchDateTime.TokenBeforeDate;
        this.cardinalExtractor = config.cardinalExtractor;
        this.numberParser = config.numberParser;
        this.durationExtractor = config.durationExtractor;
        this.dateExtractor = config.dateExtractor;
        this.durationParser = config.durationParser;
        this.dateParser = config.dateParser;

        this.monthFrontBetweenRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthFrontBetweenRegex);
        this.betweenRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.BetweenRegex);
        this.monthFrontSimpleCasesRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthFrontSimpleCasesRegex);
        this.simpleCasesRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SimpleCasesRegex);
        this.oneWordPeriodRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.OneWordPeriodRegex);
        this.monthWithYear = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthWithYear);
        this.monthNumWithYear = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthNumWithYear);
        this.yearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.YearRegex);
        this.relativeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RelativeRegex);
        this.pastRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PastSuffixRegex);
        this.futureRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextSuffixRegex);
        this.numberCombinedWithUnit = RegExpUtility.getSafeRegExp(FrenchDateTime.NumberCombinedWithDurationUnit);
        this.weekOfMonthRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekOfMonthRegex);
        this.weekOfYearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekOfYearRegex);
        this.quarterRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.QuarterRegex);
        this.quarterRegexYearFront = RegExpUtility.getSafeRegExp(FrenchDateTime.QuarterRegexYearFront);
        this.allHalfYearRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.AllHalfYearRegex);
        this.seasonRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.SeasonRegex);
        this.whichWeekRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WhichWeekRegex);
        this.weekOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.WeekOfRegex);
        this.monthOfRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.MonthOfRegex);
        this.restOfDateRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.RestOfDateRegex);
        this.unspecificEndOfRangeRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.UnspecificEndOfRangeRegex);
        this.nowRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NowRegex);

        this.nextPrefixRegex = RegExpUtility.getSafeRegExp("(prochain|prochaine)\b");
        this.previousPrefixRegex = RegExpUtility.getSafeRegExp("(dernier)\b");
        this.thisPrefixRegex = RegExpUtility.getSafeRegExp("(ce|cette)\b");

        this.nextSuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.NextSuffixRegex);
        this.pastSuffixRegex = RegExpUtility.getSafeRegExp(FrenchDateTime.PastSuffixRegex);

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

        if (trimedText.endsWith("prochain") || trimedText.endsWith("prochaine")) {
            swift = 1;
        }

        if (trimedText.endsWith("dernière") ||
            trimedText.endsWith("dernières") ||
            trimedText.endsWith("derniere") ||
            trimedText.endsWith("dernieres")) {
            swift = -1;
        }

        return swift;
    }

    getSwiftYear(source: string): number {
        let trimedText = source.trim().toLowerCase();
        let swift = -10;
        if (trimedText.endsWith("prochain") || trimedText.endsWith("prochaine")) {
            swift = 1;
        }

        if (trimedText.endsWith("dernières") ||
            trimedText.endsWith("dernière") ||
            trimedText.endsWith("dernieres") ||
            trimedText.endsWith("derniere") ||
            trimedText.endsWith("dernier")) {
            swift = -1;
        }
        else if (trimedText.startsWith("cette")) {
            swift = 0;
        }

        return swift;
    }

    isFuture(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return FrenchDateTime.FutureStartTerms.some(o => trimedText.startsWith(o)) ||
            FrenchDateTime.FutureEndTerms.some(o => trimedText.endsWith(o));
    }

    isYearToDate(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return FrenchDateTime.YearToDateTerms.some(o => trimedText === o);
    }

    isMonthToDate(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return FrenchDateTime.MonthToDateTerms.some(o => trimedText === o);
    }

    isWeekOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return (FrenchDateTime.WeekTerms.some(o => trimedText.endsWith(o)) ||
            (FrenchDateTime.WeekTerms.some(o => trimedText.includes(o)) &&
                (RegExpUtility.isMatch(this.nextSuffixRegex, trimedText) ||
                    RegExpUtility.isMatch(this.pastSuffixRegex, trimedText)))) &&
            !FrenchDateTime.WeekendTerms.some(o => trimedText.endsWith(o));
    }

    isWeekend(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return FrenchDateTime.WeekendTerms.some(o => trimedText.endsWith(o));
    }

    isMonthOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return FrenchDateTime.MonthTerms.some(o => trimedText.endsWith(o));
    }

    isYearOnly(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return FrenchDateTime.YearTerms.some(o => trimedText.endsWith(o));
    }

    isLastCardinal(source: string): boolean {
        let trimedText = source.trim().toLowerCase();
        return FrenchDateTime.LastCardinalTerms.some(o => trimedText === o);
    }
}
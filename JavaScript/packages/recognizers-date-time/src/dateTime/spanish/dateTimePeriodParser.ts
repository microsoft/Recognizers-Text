import { RegExpUtility } from "recognizers-text-number";
import { BaseDateTimePeriodParser, IDateTimePeriodParserConfiguration } from "../baseDateTimePeriod";
import { DateTimeResolutionResult, DateUtils, FormatUtil } from "../utilities";
import { SpanishDateTime } from "../../resources/spanishDateTime";

export class SpanishDateTimePeriodParser extends BaseDateTimePeriodParser {
    constructor(config: IDateTimePeriodParserConfiguration) {
        super(config);
    }

    protected parseSpecificTimeOfDay(source: string, referenceDate: Date): DateTimeResolutionResult {

        let ret = new DateTimeResolutionResult();
        let trimedText = source.trim().toLowerCase();

        // handle morning, afternoon..
        let match = this.config.getMatchedTimeRange(trimedText);
        let beginHour = match.beginHour;
        let endHour = match.endHour;
        let endMin = match.endMin;
        let timeStr = match.timeStr;
        if (!match.success) {
            return ret;
        }

        let matches = RegExpUtility.getMatches(this.config.specificTimeOfDayRegex, trimedText);
        if (matches.length && matches[0].index === 0 && matches[0].length === trimedText.length) {
            let swift = this.config.getSwiftPrefix(trimedText);

            let date = DateUtils.addDays(referenceDate, swift)
            date.setHours(0, 0, 0, 0);
            let day = date.getDate();
            let month = date.getMonth();
            let year = date.getFullYear();;

            ret.timex = FormatUtil.formatDate(date) + timeStr;

            ret.pastValue = ret.futureValue = [
                DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, day, beginHour, 0, 0),
                DateUtils.safeCreateFromValue(DateUtils.minValue(), year, month, day, endHour, endMin, endMin),
            ];

            ret.success = true;
            return ret;
        }

        let startIndex = trimedText.indexOf(SpanishDateTime.Tomorrow) === 0 ? SpanishDateTime.Tomorrow.length : 0;

        // handle Date followed by morning, afternoon
        // Add handling code to handle morning, afternoon followed by Date
        // Add handling code to handle early/late morning, afternoon
        // TODO: use regex from config: match = this.config.TimeOfDayRegex.Match(trimedText.Substring(startIndex));
        matches = RegExpUtility.getMatches(RegExpUtility.getSafeRegExp(SpanishDateTime.TimeOfDayRegex), trimedText.substring(startIndex));
        if (matches.length) {
            let match = matches[0];
            let beforeStr = trimedText.substring(0, match.index + startIndex).trim();
            let ers = this.config.dateExtractor.extract(beforeStr, referenceDate);
            if (ers.length === 0) {
                return ret;
            }

            let pr = this.config.dateParser.parse(ers[0], referenceDate);

            let futureDate = (pr.value as DateTimeResolutionResult).futureValue;
            let pastDate = (pr.value as DateTimeResolutionResult).pastValue;

            ret.timex = pr.timexStr + timeStr;

            ret.futureValue = [
                DateUtils.safeCreateFromValue(DateUtils.minValue(), futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), beginHour, 0, 0),
                DateUtils.safeCreateFromValue(DateUtils.minValue(), futureDate.getFullYear(), futureDate.getMonth(), futureDate.getDate(), endHour, endMin, endMin)
            ];

            ret.pastValue = [
                DateUtils.safeCreateFromValue(DateUtils.minValue(), pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), beginHour, 0, 0),
                DateUtils.safeCreateFromValue(DateUtils.minValue(), pastDate.getFullYear(), pastDate.getMonth(), pastDate.getDate(), endHour, endMin, endMin)
            ];

            ret.success = true;

            return ret;
        }

        return ret;
    }
}
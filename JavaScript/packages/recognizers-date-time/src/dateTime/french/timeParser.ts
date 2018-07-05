import { RegExpUtility } from "@microsoft/recognizers-text";
import { BaseTimeParser, ITimeParserConfiguration } from "../baseTime";
import { DateTimeResolutionResult, DateUtils, FormatUtil } from "../utilities";
import { FrenchDateTime } from "../../resources/frenchDateTime";

export class FrenchTimeParser extends BaseTimeParser {
    constructor(config: ITimeParserConfiguration) {
        super(config);
    }

    internalParse(text: string, referenceTime: Date): DateTimeResolutionResult {

        let ret = super.internalParse(text, referenceTime);
        if (!ret.success) {
            ret = this.parseIsh(text, referenceTime);
        }

        return ret;
    }

    parseIsh(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let trimedText = text.trim().toLowerCase();

        let matches = RegExpUtility.getMatches(RegExpUtility.getSafeRegExp(FrenchDateTime.IshRegex), text);
        if (matches.length && matches[0].index === 0 && matches[0].length === trimedText.length) {
            let hourStr = matches[0].groups("hour").value;
            let hour = 12;
            if (hourStr) {
                hour = parseInt(hourStr, 10);
            }

            ret.timex = "T" + FormatUtil.toString(hour, 2);
            ret.futureValue =
                ret.pastValue =
                DateUtils.safeCreateFromMinValue(referenceTime.getFullYear(), referenceTime.getMonth(), referenceTime.getDate(), hour, 0, 0);
            ret.success = true;
        }

        return ret;
    }
}
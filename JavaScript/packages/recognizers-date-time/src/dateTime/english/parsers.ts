// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { BaseTimeParser, ITimeParserConfiguration } from "../baseTime";
import { EnglishTimeExtractorConfiguration } from "./timeConfiguration";
import { DateTimeResolutionResult, DateTimeFormatUtil } from "../utilities";
import { RegExpUtility } from "@microsoft/recognizers-text";

export class EnglishTimeParser extends BaseTimeParser {
    constructor(configuration: ITimeParserConfiguration) {
        super(configuration);
    }

    internalParse(text: string, referenceTime: Date): DateTimeResolutionResult {
        let innerResult = super.internalParse(text, referenceTime);
        if (!innerResult.success) {
            innerResult = this.parseIsh(text, referenceTime);
        }
        return innerResult;
    }

    // parse "noonish", "11-ish"
    private parseIsh(text: string, referenceTime: Date): DateTimeResolutionResult {
        let ret = new DateTimeResolutionResult();
        let trimmedText = text.toLowerCase().trim();

        let matches = RegExpUtility.getMatches(EnglishTimeExtractorConfiguration.ishRegex, trimmedText);
        if (matches.length > 0 && matches[0].length === trimmedText.length) {
            let hourStr = matches[0].groups("hour").value;
            let hour = 12;
            if (hourStr) {
                hour = Number.parseInt(hourStr, 10);
            }

            ret.timex = "T" + DateTimeFormatUtil.toString(hour, 2);
            ret.futureValue =
                ret.pastValue =
                new Date(referenceTime.getFullYear(), referenceTime.getMonth(), referenceTime.getDate(), hour, 0, 0);
            ret.success = true;
            return ret;
        }

        return ret;
    }
}

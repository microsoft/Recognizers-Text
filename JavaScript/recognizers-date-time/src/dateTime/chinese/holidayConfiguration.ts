import { IHolidayExtractorConfiguration } from "../baseHoliday"
import { RegExpUtility } from "recognizers-text-number";
import { DateUtils } from "../utilities";
import { ChineseDateTime } from "../../resources/chineseDateTime";

export class ChineseHolidayExtractorConfiguration implements IHolidayExtractorConfiguration {
    readonly holidayRegexes: RegExp[]
            
    constructor() {
        this.holidayRegexes = [
            RegExpUtility.getSafeRegExp(ChineseDateTime.HolidayRegexList1),
            RegExpUtility.getSafeRegExp(ChineseDateTime.HolidayRegexList2),
            RegExpUtility.getSafeRegExp(ChineseDateTime.LunarHolidayRegex)
        ];
    }
}
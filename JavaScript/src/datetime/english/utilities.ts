import { IDateTimeUtilityConfiguration} from "../utilities";
import { RegExpUtility } from "../../utilities"
import { EnglishDateTime } from "../../resources/englishDateTime"
import * as XRegExp from 'xregexp';

export class EnlighDatetimeUtilityConfiguration implements IDateTimeUtilityConfiguration {
    agoRegex: RegExp;
    laterRegex: RegExp;
    inConnectorRegex: RegExp;
    amDescRegex: RegExp;
    pmDescRegex: RegExp;
    amPmDescRegex: RegExp;
    constructor() {
        this.laterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex);
        this.agoRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex);
        this.inConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex);
        this.amDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmDescRegex);
        this.pmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmDescRegex);
        this.amPmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmPmDescRegex);
    }
}
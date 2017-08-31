import { IDateTimeUtilityConfiguration} from "../utilities";
import { RegExpUtility } from "../../utilities"
import { EnglishDateTime } from "../../resources/englishDateTime"
import * as XRegExp from 'xregexp';

export class EnlighDatetimeUtilityConfiguration implements IDateTimeUtilityConfiguration {
    readonly agoRegex: RegExp;
    readonly laterRegex: RegExp;
    readonly inConnectorRegex: RegExp;
    readonly rangeUnitRegex: RegExp;
    readonly amDescRegex: RegExp;
    readonly pmDescRegex: RegExp;
    readonly amPmDescRegex: RegExp;

    constructor() {
        this.laterRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.LaterRegex);
        this.agoRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AgoRegex);
        this.inConnectorRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.InConnectorRegex);
        this.rangeUnitRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.RangeUnitRegex);
        this.amDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmDescRegex);
        this.pmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.PmDescRegex);
        this.amPmDescRegex = RegExpUtility.getSafeRegExp(EnglishDateTime.AmPmDescRegex);
    }
}
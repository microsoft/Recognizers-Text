import { RegExpUtility } from "@microsoft/recognizers-text";
import { IURLExtractorConfiguration } from "../extractors";
import { ChineseURL } from "../../resources/chineseURL";

export class JapaneseURLExtractorConfiguration implements IURLExtractorConfiguration {
    readonly UrlRegex: RegExp;
    readonly IpUrlRegex: RegExp;

    constructor() {
        this.UrlRegex = RegExpUtility.getSafeRegExp(ChineseURL.UrlRegex);
        this.IpUrlRegex = RegExpUtility.getSafeRegExp(ChineseURL.IpUrlRegex);
    }
}
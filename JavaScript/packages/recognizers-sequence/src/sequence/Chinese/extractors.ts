import { RegExpUtility } from "@microsoft/recognizers-text";
import { IURLExtractorConfiguration } from "../extractors";
import { BaseURL } from "../../resources/baseURL";

export class ChineseURLExtractorConfiguration implements IURLExtractorConfiguration {
    readonly UrlRegex: RegExp;
    readonly UrlRegex2: RegExp;
    readonly IpUrlRegex: RegExp;

    constructor() {
        this.UrlRegex = RegExpUtility.getSafeRegExp(BaseURL.UnicodeUrlRegex);
        this.UrlRegex2 = RegExpUtility.getSafeRegExp(BaseURL.UrlRegex2);
        this.IpUrlRegex = RegExpUtility.getSafeRegExp(BaseURL.IpUrlRegex);
    }
}
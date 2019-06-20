import { RegExpUtility } from "@microsoft/recognizers-text";
import { IURLExtractorConfiguration, BasePhoneNumberExtractor, IPhoneNumberExtractorConfiguration } from "../extractors";
import { ChineseURL } from "../../resources/chineseURL";
import { ChinesePhoneNumbers } from "../../resources/chinesePhoneNumbers";

export class ChineseURLExtractorConfiguration implements IURLExtractorConfiguration {
    readonly UrlRegex: RegExp;
    readonly IpUrlRegex: RegExp;

    constructor() {
        this.UrlRegex = RegExpUtility.getSafeRegExp(ChineseURL.UrlRegex);
        this.IpUrlRegex = RegExpUtility.getSafeRegExp(ChineseURL.IpUrlRegex);
    }
}

export class ChinesePhoneNumberExtractorConfiguration implements IPhoneNumberExtractorConfiguration{
    readonly BRPhoneNumberRegex: RegExp;
    readonly GeneralPhoneNumberRegex: RegExp;
    readonly UKPhoneNumberRegex: RegExp;
    readonly DEPhoneNumberRegex: RegExp;
    readonly USPhoneNumberRegex: RegExp;
    readonly CNPhoneNumberRegex: RegExp;
    readonly DKPhoneNumberRegex: RegExp;
    readonly ITPhoneNumberRegex: RegExp;
    readonly NLPhoneNumberRegex: RegExp;
    readonly SpecialPhoneNumberRegex: RegExp;

    constructor() {
        this.BRPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.BRPhoneNumberRegex);
        this.GeneralPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.GeneralPhoneNumberRegex);
        this.UKPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.UKPhoneNumberRegex);
        this.DEPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.DEPhoneNumberRegex);
        this.USPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.USPhoneNumberRegex);
        this.CNPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.CNPhoneNumberRegex);
        this.DKPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.DKPhoneNumberRegex);
        this.ITPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.ITPhoneNumberRegex);
        this.NLPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.NLPhoneNumberRegex);
        this.SpecialPhoneNumberRegex = RegExpUtility.getSafeRegExp(ChinesePhoneNumbers.SpecialPhoneNumberRegex);
    }
}

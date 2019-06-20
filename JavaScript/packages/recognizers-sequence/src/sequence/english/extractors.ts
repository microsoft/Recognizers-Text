import { BasePhoneNumberExtractor, BaseIpExtractor, BaseMentionExtractor, BaseHashtagExtractor, BaseEmailExtractor, BaseURLExtractor, BaseGUIDExtractor, IPhoneNumberExtractorConfiguration } from "../extractors";
import { ExtractResult, RegExpUtility } from "@microsoft/recognizers-text";
import { BasePhoneNumbers } from "../../resources/basePhoneNumbers";
import { BaseURL } from "../../resources/baseURL";
import { IURLExtractorConfiguration } from "../extractors";

export class EnglishPhoneNumberExtractorConfiguration implements IPhoneNumberExtractorConfiguration{
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
        this.BRPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.BRPhoneNumberRegex);
        this.GeneralPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.GeneralPhoneNumberRegex);
        this.UKPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.UKPhoneNumberRegex);
        this.DEPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.DEPhoneNumberRegex);
        this.USPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.USPhoneNumberRegex);
        this.CNPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.CNPhoneNumberRegex);
        this.DKPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.DKPhoneNumberRegex);
        this.ITPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.ITPhoneNumberRegex);
        this.NLPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.NLPhoneNumberRegex);
        this.SpecialPhoneNumberRegex = RegExpUtility.getSafeRegExp(BasePhoneNumbers.SpecialPhoneNumberRegex);
    }
}

export class IpExtractor extends BaseIpExtractor{

}

export class MentionExtractor extends BaseMentionExtractor{

}

export class HashtagExtractor extends BaseHashtagExtractor{

}

export class EmailExtractor extends BaseEmailExtractor{

}

export class EnglishURLExtractorConfiguration implements IURLExtractorConfiguration {
    readonly UrlRegex: RegExp;
    readonly IpUrlRegex: RegExp;

    constructor() {
        this.UrlRegex = RegExpUtility.getSafeRegExp(BaseURL.UrlRegex);
        this.IpUrlRegex = RegExpUtility.getSafeRegExp(BaseURL.IpUrlRegex);
    }
}

export class GUIDExtractor extends BaseGUIDExtractor {

}
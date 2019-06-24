import { BasePhoneNumberExtractor, BaseIpExtractor, BaseMentionExtractor, BaseHashtagExtractor, BaseEmailExtractor, BaseURLExtractor, BaseGUIDExtractor, IPhoneNumberExtractorConfiguration } from "../extractors";
import { ExtractResult, RegExpUtility } from "@microsoft/recognizers-text";
import { BasePhoneNumbers } from "../../resources/basePhoneNumbers";
import { BaseURL } from "../../resources/baseURL";
import { IURLExtractorConfiguration } from "../extractors";

export class EnglishPhoneNumberExtractorConfiguration implements IPhoneNumberExtractorConfiguration{
    readonly WordBoundariesRegex: string;
    readonly NonWordBoundariesRegex: string;
    readonly EndWordBoundariesRegex: string;

    constructor() {
        this.WordBoundariesRegex = BasePhoneNumbers.WordBoundariesRegex;
        this.NonWordBoundariesRegex = BasePhoneNumbers.NonWordBoundariesRegex;
        this.EndWordBoundariesRegex = BasePhoneNumbers.EndWordBoundariesRegex;
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
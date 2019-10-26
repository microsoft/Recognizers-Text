import { BasePhoneNumberExtractor, IIpExtractorConfiguration, BaseMentionExtractor, BaseHashtagExtractor, BaseEmailExtractor, IURLExtractorConfiguration, BaseGUIDExtractor, IPhoneNumberExtractorConfiguration } from "../extractors";
import { ExtractResult, RegExpUtility } from "@microsoft/recognizers-text";
import { BasePhoneNumbers } from "../../resources/basePhoneNumbers";
import { BaseURL } from "../../resources/baseURL";
import { BaseIp } from "../../resources/baseIp";
import { EnglishPhoneNumbers } from "../../resources/EnglishPhoneNumbers";

export class EnglishPhoneNumberExtractorConfiguration implements IPhoneNumberExtractorConfiguration {
    readonly WordBoundariesRegex: string;
    readonly NonWordBoundariesRegex: string;
    readonly EndWordBoundariesRegex: string;
    readonly ColonPrefixCheckRegex: string;
    readonly ForbiddenPrefixMarkers: string[];
    readonly FalsePositivePrefixRegex: string;


    constructor() {
        this.WordBoundariesRegex = BasePhoneNumbers.WordBoundariesRegex;
        this.NonWordBoundariesRegex = BasePhoneNumbers.NonWordBoundariesRegex;
        this.EndWordBoundariesRegex = BasePhoneNumbers.EndWordBoundariesRegex;
        this.ForbiddenPrefixMarkers = BasePhoneNumbers.ForbiddenPrefixMarkers;
        this.ColonPrefixCheckRegex = BasePhoneNumbers.ColonPrefixCheckRegex;
        this.FalsePositivePrefixRegex = EnglishPhoneNumbers.FalsePositivePrefixRegex;
    }
}

export class EnglishIpExtractorConfiguration implements IIpExtractorConfiguration {
    readonly Ipv4Regex: RegExp;
    readonly Ipv6Regex: RegExp;

    constructor() {
        this.Ipv4Regex = RegExpUtility.getSafeRegExp(BaseIp.Ipv4Regex);
        this.Ipv6Regex = RegExpUtility.getSafeRegExp(BaseIp.Ipv6Regex);
    }
}

export class MentionExtractor extends BaseMentionExtractor {

}

export class HashtagExtractor extends BaseHashtagExtractor {

}

export class EmailExtractor extends BaseEmailExtractor {

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
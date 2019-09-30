import { RegExpUtility } from "@microsoft/recognizers-text";
import { IURLExtractorConfiguration, IIpExtractorConfiguration, IPhoneNumberExtractorConfiguration } from "../extractors";
import { ChineseURL } from "../../resources/chineseURL";
import { ChinesePhoneNumbers } from "../../resources/chinesePhoneNumbers";
import { ChineseIp } from "../../resources/chineseIp";

export class ChineseURLExtractorConfiguration implements IURLExtractorConfiguration {
    readonly UrlRegex: RegExp;
    readonly IpUrlRegex: RegExp;

    constructor() {
        this.UrlRegex = RegExpUtility.getSafeRegExp(ChineseURL.UrlRegex);
        this.IpUrlRegex = RegExpUtility.getSafeRegExp(ChineseURL.IpUrlRegex);
    }
}

export class ChinesePhoneNumberExtractorConfiguration implements IPhoneNumberExtractorConfiguration {
    readonly WordBoundariesRegex: string;
    readonly NonWordBoundariesRegex: string;
    readonly EndWordBoundariesRegex: string;
    readonly ColonPrefixCheckRegex: string;
    readonly ForbiddenPrefixMarkers: string[];

    constructor() {
        this.WordBoundariesRegex = ChinesePhoneNumbers.WordBoundariesRegex;
        this.NonWordBoundariesRegex = ChinesePhoneNumbers.NonWordBoundariesRegex;
        this.EndWordBoundariesRegex = ChinesePhoneNumbers.EndWordBoundariesRegex;
        this.ColonPrefixCheckRegex = ChinesePhoneNumbers.ColonPrefixCheckRegex;
        this.ForbiddenPrefixMarkers = ChinesePhoneNumbers.ForbiddenPrefixMarkers;
    }
}

export class ChineseIpExtractorConfiguration implements IIpExtractorConfiguration {
    readonly Ipv4Regex: RegExp;
    readonly Ipv6Regex: RegExp;

    constructor() {
        this.Ipv4Regex = RegExpUtility.getSafeRegExp(ChineseIp.Ipv4Regex);
        this.Ipv6Regex = RegExpUtility.getSafeRegExp(ChineseIp.Ipv6Regex);
    }
}

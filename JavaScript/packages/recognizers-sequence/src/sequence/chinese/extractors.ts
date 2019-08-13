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

export class ChinesePhoneNumberExtractorConfiguration implements IPhoneNumberExtractorConfiguration {
    readonly WordBoundariesRegex: string;
    readonly NonWordBoundariesRegex: string;
    readonly EndWordBoundariesRegex: string;
    readonly ColonBeginRegex: string;
    readonly ColonMarkers: string[];
    readonly BoundaryEndMarkers: string[];
    readonly BoundaryStartMarkers: string[];

    constructor() {
        this.WordBoundariesRegex = ChinesePhoneNumbers.WordBoundariesRegex;
        this.NonWordBoundariesRegex = ChinesePhoneNumbers.NonWordBoundariesRegex;
        this.EndWordBoundariesRegex = ChinesePhoneNumbers.EndWordBoundariesRegex;
        this.ColonBeginRegex = ChinesePhoneNumbers.ColonBeginRegex;
        this.ColonMarkers = ChinesePhoneNumbers.ColonMarkers;
        this.BoundaryEndMarkers = ChinesePhoneNumbers.BoundaryEndMarkers;
        this.BoundaryStartMarkers = ChinesePhoneNumbers.BoundaryStartMarkers;
    }
}

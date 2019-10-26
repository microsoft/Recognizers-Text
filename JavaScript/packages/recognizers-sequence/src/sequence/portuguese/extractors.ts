import { BasePhoneNumberExtractor, IIpExtractorConfiguration, BaseMentionExtractor, BaseHashtagExtractor, BaseEmailExtractor, IURLExtractorConfiguration, BaseGUIDExtractor, IPhoneNumberExtractorConfiguration } from "../extractors";
import { ExtractResult, RegExpUtility } from "@microsoft/recognizers-text";
import { BasePhoneNumbers } from "../../resources/basePhoneNumbers";
import { BaseURL } from "../../resources/baseURL";
import { BaseIp } from "../../resources/baseIp";
import { PortuguesePhoneNumbers } from "../../resources/PortuguesePhoneNumbers";

export class PortuguesePhoneNumberExtractorConfiguration implements IPhoneNumberExtractorConfiguration {
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
        this.FalsePositivePrefixRegex = PortuguesePhoneNumbers.FalsePositivePrefixRegex;
    }
}

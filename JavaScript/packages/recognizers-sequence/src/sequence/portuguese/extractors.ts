import { BasePhoneNumberExtractorConfiguration, IIpExtractorConfiguration, BaseMentionExtractor, BaseHashtagExtractor, BaseEmailExtractor, IURLExtractorConfiguration, BaseGUIDExtractor, IPhoneNumberExtractorConfiguration } from "../extractors";
import { ExtractResult, RegExpUtility } from "@microsoft/recognizers-text";
import { BasePhoneNumbers } from "../../resources/basePhoneNumbers";
import { BaseURL } from "../../resources/baseURL";
import { BaseIp } from "../../resources/baseIp";
import { PortuguesePhoneNumbers } from "../../resources/PortuguesePhoneNumbers";

export class PortuguesePhoneNumberExtractorConfiguration extends BasePhoneNumberExtractorConfiguration {
    readonly FalsePositivePrefixRegex: string;

    constructor() {
        super();
        this.FalsePositivePrefixRegex = PortuguesePhoneNumbers.FalsePositivePrefixRegex;
    }
}

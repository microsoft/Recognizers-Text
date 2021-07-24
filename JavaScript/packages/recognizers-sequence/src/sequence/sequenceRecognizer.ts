// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { Recognizer, IModel, Culture, ModelResult } from "@microsoft/recognizers-text";
import { PhoneNumberModel, IpAddressModel, MentionModel, HashtagModel, EmailModel, URLModel, GUIDModel } from "./models";
import { PhoneNumberParser, IpParser, MentionParser, HashtagParser, EmailParser, URLParser, GUIDParser } from "./english/parsers";
import { EnglishIpExtractorConfiguration, MentionExtractor, HashtagExtractor, EmailExtractor, EnglishURLExtractorConfiguration, GUIDExtractor, EnglishPhoneNumberExtractorConfiguration } from "./english/extractors";
import { ChineseURLExtractorConfiguration, ChinesePhoneNumberExtractorConfiguration, ChineseIpExtractorConfiguration } from "./chinese/extractors";
import { PortuguesePhoneNumberExtractorConfiguration } from "./portuguese/extractors";
import { BaseURLExtractor, BasePhoneNumberExtractor, BaseIpExtractor } from "./extractors";


export enum SequenceOptions {
    None = 0,
}

export function recognizePhoneNumber(query: string, culture: string, options: SequenceOptions = SequenceOptions.None): ModelResult[] {
    if (culture.toLowerCase().startsWith("zh-") || culture.toLowerCase().startsWith("ja-")) {
        return recognizeByModel(recognizer => recognizer.getPhoneNumberModel(), query, Culture.Chinese, options);
    }
    return recognizeByModel(recognizer => recognizer.getPhoneNumberModel(), query, culture, options);
}

export function recognizeIpAddress(query: string, culture: string, options: SequenceOptions = SequenceOptions.None): ModelResult[] {
    if (culture.toLowerCase().startsWith("zh-") || culture.toLowerCase().startsWith("ja-")) {
        return recognizeByModel(recognizer => recognizer.getIpAddressModel(), query, Culture.Chinese, options);
    }
    return recognizeByModel(recognizer => recognizer.getIpAddressModel(), query, culture, options);
}

export function recognizeMention(query: string, culture: string, options: SequenceOptions = SequenceOptions.None): ModelResult[] {
    return recognizeByModel(recognizer => recognizer.getMentionModel(), query, culture, options);
}

export function recognizeHashtag(query: string, culture: string, options: SequenceOptions = SequenceOptions.None): ModelResult[] {
    return recognizeByModel(recognizer => recognizer.getHashtagModel(), query, culture, options);
}

export function recognizeEmail(query: string, culture: string, options: SequenceOptions = SequenceOptions.None): ModelResult[] {
    return recognizeByModel(recognizer => recognizer.getEmailModel(), query, culture, options);
}

export function recognizeURL(query: string, culture: string, options: SequenceOptions = SequenceOptions.None): ModelResult[] {
    if (culture.toLowerCase().startsWith("zh-") || culture.toLowerCase().startsWith("ja-")) {
        return recognizeByModel(recognizer => recognizer.getURLModel(), query, Culture.Chinese, options);
    }
    return recognizeByModel(recognizer => recognizer.getURLModel(), query, culture, options);
}

export function recognizeGUID(query: string, culture: string, options: SequenceOptions = SequenceOptions.None): ModelResult[] {
    return recognizeByModel(recognizer => recognizer.getGUIDModel(), query, culture, options);
}

function recognizeByModel(getModelFunc: (n: SequenceRecognizer) => IModel, query: string, culture: string, options: SequenceOptions): ModelResult[] {
    let recognizer = new SequenceRecognizer(culture, options);
    let model = getModelFunc(recognizer);
    return model.parse(query);
}


export default class SequenceRecognizer extends Recognizer<SequenceOptions> {
    constructor(culture: string, options: SequenceOptions = SequenceOptions.None, lazyInitialization: boolean = false) {
        super(culture, options, lazyInitialization);
    }

    protected IsValidOptions(options: any): boolean {
        return options >= 0 && options <= SequenceOptions.None;
    }

    protected InitializeConfiguration() {
        this.registerModel("PhoneNumberModel", Culture.English, (options) => new PhoneNumberModel(
            new PhoneNumberParser(),
            new BasePhoneNumberExtractor(new EnglishPhoneNumberExtractorConfiguration())));
        this.registerModel("PhoneNumberModel", Culture.Chinese, (options) => new PhoneNumberModel(
            new PhoneNumberParser(),
            new BasePhoneNumberExtractor(new ChinesePhoneNumberExtractorConfiguration())));
        this.registerModel("PhoneNumberModel", Culture.Portuguese, (options) => new PhoneNumberModel(
            new PhoneNumberParser(),
            new BasePhoneNumberExtractor(new PortuguesePhoneNumberExtractorConfiguration())));

        this.registerModel("IpAddressModel", Culture.English, (options) => new IpAddressModel(
            new IpParser(),
            new BaseIpExtractor(new EnglishIpExtractorConfiguration())));
        this.registerModel("IpAddressModel", Culture.Chinese, (options) => new IpAddressModel(
            new IpParser(),
            new BaseIpExtractor(new ChineseIpExtractorConfiguration())));

        this.registerModel("MentionModel", Culture.English, (options) => new MentionModel(new MentionParser(), new MentionExtractor()));
        this.registerModel("HashtagModel", Culture.English, (options) => new HashtagModel(new HashtagParser(), new HashtagExtractor()));
        this.registerModel("EmailModel", Culture.English, (options) => new EmailModel(new EmailParser(), new EmailExtractor()));

        this.registerModel("URLModel", Culture.English, (options) => new URLModel(
            new URLParser(),
            new BaseURLExtractor(new EnglishURLExtractorConfiguration())));
        this.registerModel("URLModel", Culture.Chinese, (options) => new URLModel(
            new URLParser(),
            new BaseURLExtractor(new ChineseURLExtractorConfiguration())));

        this.registerModel("GUIDModel", Culture.English, (options) => new GUIDModel(new GUIDParser(), new GUIDExtractor()));
    }

    getPhoneNumberModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("PhoneNumberModel", culture, fallbackToDefaultCulture);
    }

    getIpAddressModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("IpAddressModel", culture, fallbackToDefaultCulture);
    }

    getMentionModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("MentionModel", culture, fallbackToDefaultCulture);
    }

    getHashtagModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("HashtagModel", culture, fallbackToDefaultCulture);
    }

    getEmailModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("EmailModel", culture, fallbackToDefaultCulture);
    }

    getURLModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("URLModel", culture, fallbackToDefaultCulture);
    }

    getGUIDModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("GUIDModel", culture, fallbackToDefaultCulture);
    }
} 
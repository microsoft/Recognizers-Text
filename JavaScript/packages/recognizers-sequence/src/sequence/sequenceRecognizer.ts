import { Recognizer, IModel, Culture, ModelResult } from "@microsoft/recognizers-text";
import { PhoneNumberModel, IpAddressModel } from "./models";
import { PhoneNumberParser, IpParser } from "./english/parsers";
import { PhoneNumberExtractor, IpExtractor } from "./english/extractors";


export enum SequenceOptions {
    None = 0,
}

export function recognizePhoneNumber(query: string, culture: string, options: SequenceOptions  = SequenceOptions .None): Array<ModelResult> {
    return recognizeByModel(recognizer => recognizer.getPhoneNumberModel(), query, culture, options);
}

export function recognizeIpAddress(query: string, culture: string, options: SequenceOptions  = SequenceOptions .None): Array<ModelResult> {
    return recognizeByModel(recognizer => recognizer.getIpAddressModel(), query, culture, options);
}

function recognizeByModel(getModelFunc: (n: SequenceRecognizer) => IModel, query: string, culture: string, options: SequenceOptions): Array<ModelResult> {
    let recognizer = new SequenceRecognizer(culture, options);
    let model = getModelFunc(recognizer);
    return model.parse(query);
}


export default class SequenceRecognizer extends Recognizer<SequenceOptions> {
    constructor(culture: string, options: SequenceOptions = SequenceOptions.None, lazyInitialization: boolean = false) {
        super(culture, options, lazyInitialization);
    }

    protected IsValidOptions(options: any): boolean {
        return options >= 0 && options <= SequenceOptions.None
    }
    
    protected InitializeConfiguration() {
        this.registerModel("PhoneNumberModel", Culture.English, (options) => new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor()));
        this.registerModel("IpAddressModel", Culture.English, (options) => new IpAddressModel(new IpParser(), new IpExtractor()));
    }

    getPhoneNumberModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("PhoneNumberModel", culture, fallbackToDefaultCulture);
    }

    getIpAddressModel(culture: string = null, fallbackToDefaultCulture: boolean = true): IModel {
        return this.getModel("IpAddressModel", culture, fallbackToDefaultCulture);
    }
} 
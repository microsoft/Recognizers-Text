import { IExtractor, ExtractResult, RegExpUtility, Match, StringUtility } from "@microsoft/recognizers-text";
import { BasePhoneNumbers } from "../resources/basePhoneNumbers";
import { BaseIp } from "../resources/baseIp";
import { BaseMention } from "../resources/baseMention";
import { BaseHashtag } from "../resources/baseHashtag";
import { BaseEmail } from "../resources/baseEmail";
import { BaseURL } from "../resources/baseURL";
import { BaseGUID } from "../resources/baseGUID";
import { Constants } from "./constants";

export abstract class BaseSequenceExtractor implements IExtractor {
    abstract regexes: Map<RegExp, string>;

    protected extractType: string;

    extract(source: string): ExtractResult[] {
        let results = new Array<ExtractResult>();

        if (StringUtility.isNullOrWhitespace(source)) {
            return results;
        }

        let matchSource = new Map<Match, string>();
        let matched = new Array<boolean>(source.length);

        // Traverse every match results to see each position in the text is matched or not.
        let collections = this.regexes.forEach((typeExtracted, regex) => {
            RegExpUtility.getMatches(regex, source).forEach(match => {
                if (!this.isValidMatch(match)) {
                    return;
                }

                for (let j = 0; j < match.length; j++) {
                    matched[match.index + j] = true;
                }

                // Keep Source Data for extra information
                matchSource.set(match, typeExtracted);
            });
        });

        // Form the extracted results from all the matched intervals in the text.
        let lastNotMatched = -1;
        for (let i = 0; i < source.length; i++) {
            if (matched[i]) {
                if (i + 1 === source.length || !matched[i + 1]) {
                    let start = lastNotMatched + 1;
                    let length = i - lastNotMatched;
                    let substr = source.substr(start, length);
                    let matchFunc = (o: Match) => o.index == start && o.length == length;

                    let srcMatch = Array.from(matchSource.keys()).find(matchFunc);
                    if (srcMatch) {
                        results.push({
                            start: start,
                            length: length,
                            text: substr,
                            type: this.extractType,
                            data: matchSource.has(srcMatch) ? matchSource.get(srcMatch) : null
                        });
                    }
                }
            }
            else {
                lastNotMatched = i;
            }
        }

        return results;
    }

    isValidMatch(match: Match): boolean {
        return true;
    }
}

export interface IPhoneNumberExtractorConfiguration {
    WordBoundariesRegex: string;
    NonWordBoundariesRegex: string;
    EndWordBoundariesRegex: string;
    ColonPrefixCheckRegex: string;
    FalsePositivePrefixRegex: string;
    ForbiddenPrefixMarkers: string[];
}

export class BasePhoneNumberExtractorConfiguration implements IPhoneNumberExtractorConfiguration {
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
        this.FalsePositivePrefixRegex = null;
    }
}

export class BasePhoneNumberExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;
    config: IPhoneNumberExtractorConfiguration;

    constructor(config: IPhoneNumberExtractorConfiguration) {
        super();
        this.config = config;
        let wordBoundariesRegex = config.WordBoundariesRegex;
        let nonWordBoundariesRegex = config.NonWordBoundariesRegex;
        let endWordBoundariesRegex = config.EndWordBoundariesRegex;
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.BRPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_BR)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.GeneralPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_GENERAL)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.UKPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_UK)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.DEPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_DE)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.USPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_US)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.CNPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_CN)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.DKPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_DK)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.ITPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_IT)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.NLPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_NL)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.SpecialPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_SPECIAL);
    }
    extract(source: string): ExtractResult[] {
        let ret = new Array<ExtractResult>();
        if (!source.match(BasePhoneNumbers.PreCheckPhoneNumberRegex)) {
            return ret;
        }
        let ers = super.extract(source);
        let formatIndicatorRegex = new RegExp(BasePhoneNumbers.FormatIndicatorRegex, "ig");
        let digitRegex = new RegExp("[0-9]");
        for (let er of ers) {
            let Digits = 0;
            for (let t of er.text) {
                if (t.match(digitRegex)) {
                    Digits++ ; 
                }
            }
            if ((Digits < 7 && er.data !== "ITPhoneNumber") || er.text.match(BasePhoneNumbers.SSNFilterRegex)) {
                continue;
            }
            if (Digits === 16 && !(er.text.substring(0, 1) === "+")) {
                continue;
            }
            if (Digits === 15) {
                let flag = false;
                let numSpans = er.text.split(" ");
                for (let numSpan of numSpans) {
                    let numSpanDigit = 0;
                    for (let t of numSpan) {
                        if (t.match(digitRegex)) {
                            numSpanDigit++;
                        }
                    }
                    if (numSpanDigit == 4 || numSpanDigit == 3) {
                        flag = false;
                    }
                    else {
                        flag = true;
                        break;
                    }
                }
                if (flag == false) {
                    continue;
                }
            }
            if (er.start + er.length < source.length) {
                let ch = source[ er.start+er.length ];
                if (BasePhoneNumbers.ForbiddenSuffixMarkers.indexOf(ch) !== -1){
                    continue;
                }
            }
            let ch = source[er.start - 1];
            let front = source.substring(0, er.start - 1);
            if (this.config.FalsePositivePrefixRegex && front.match(this.config.FalsePositivePrefixRegex)) {
                continue;
            }

            if (er.start !== 0) {
                if (BasePhoneNumbers.BoundaryMarkers.indexOf(ch) !== -1) {
                    if (BasePhoneNumbers.SpecialBoundaryMarkers.indexOf(ch) !== -1 &&
                        formatIndicatorRegex.test(er.text) &&
                        er.start >= 2) {
                        let chGap = source[er.start - 2];
                        if (chGap.match(digitRegex)) {
                            let match = front.match(BasePhoneNumbers.InternationDialingPrefixRegex);
                            if (match) {
                                let moveOffset = match[0].length + 1;
                                er.start = er.start - moveOffset;
                                er.length = er.length + moveOffset;
                                er.text = source.substring(er.start, er.start + er.length);
                                ret.push(er);
                            }
                        }
                        // Handle cases like "91a-677-0060".
                        else if (chGap <= 'z' && chGap >= 'a') {
                            continue;
                        }
                        else {
                            ret.push(er);
                        }
                    }
                    continue;
                }
                else if (this.config.ForbiddenPrefixMarkers.indexOf(ch) !== -1) {
                    // Handle "tel:123456".
                    if (BasePhoneNumbers.ColonMarkers.indexOf(ch) !== -1) {
                        // If the char before ':' is not letter, ignore it.
                        if (!front.match(this.config.ColonPrefixCheckRegex)) {
                            continue;
                        }
                    }
                    else {
                        continue;
                    }
                }
            }
            ret.push(er);
        }

        // filter hexadecimal address like 00 10 00 31 46 D9 E9 11
        let maskRegex = new RegExp(BasePhoneNumbers.PhoneNumberMaskRegex, "g");
        let m: RegExpExecArray | null;
        while (true) {
            m = maskRegex.exec(source);
            if (m == null) {
                break;
            }
            for (let i = ret.length - 1; i >= 0; --i) {
                if (ret[i].start >= m.index && ret[i].start + ret[i].length <= m.index + m[0].length) {
                    ret.splice(i, 1);
                }
            }
        }
        return ret;
    }
}

export interface IIpExtractorConfiguration {
    Ipv4Regex: RegExp;
    Ipv6Regex: RegExp;
}

export class BaseIpExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor(config: IIpExtractorConfiguration) {
        super();
        this.regexes = new Map<RegExp, string>()
            .set(config.Ipv4Regex, Constants.IP_REGEX_IPV4)
            .set(config.Ipv6Regex, Constants.IP_REGEX_IPV6);
    }

    extract(source: string): ExtractResult[] {
        let results = new Array<ExtractResult>();

        if (StringUtility.isNullOrWhitespace(source)) {
            return results;
        }

        let matchSource = new Map<Match, string>();
        let matched = new Array<boolean>(source.length);

        let collections = this.regexes.forEach((typeExtracted, regex) => {
            RegExpUtility.getMatches(regex, source).forEach(match => {
                for (let j = 0; j < match.length; j++) {
                    matched[match.index + j] = true;
                }

                // Keep Source Data for extra information
                matchSource.set(match, typeExtracted);
            });
        });

        let lastNotMatched = -1;
        for (let i = 0; i < source.length; i++) {
            if (matched[i]) {
                if (i + 1 === source.length || !matched[i + 1]) {
                    let start = lastNotMatched + 1;
                    let length = i - lastNotMatched;
                    let substr = source.substr(start, length);
                    if (substr.startsWith(Constants.IPV6_ELLIPSIS) && start > 0 && this.isLetterOrDigit(source[start - 1])) {
                        continue;
                    }
                    else if (substr.endsWith(Constants.IPV6_ELLIPSIS) && i + 1 < source.length && this.isLetterOrDigit(source[i + 1])) {
                        continue;
                    }

                    let matchFunc = (o: Match) => o.index === start && o.length === length;

                    let srcMatch = Array.from(matchSource.keys()).find(matchFunc);
                    if (srcMatch) {
                        results.push({
                            start: start,
                            length: length,
                            text: substr,
                            type: this.extractType,
                            data: matchSource.has(srcMatch) ? matchSource.get(srcMatch) : null
                        });
                    }
                }
            }
            else {
                lastNotMatched = i;
            }
        }

        return results;
    }

    isLetterOrDigit(c: string): boolean {
        return new RegExp("[0-9a-zA-z]").test(c);
    }
}

export class BaseMentionExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor() {
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseMention.MentionRegex), Constants.MENTION_REGEX);
    }
}

export class BaseHashtagExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor() {
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseHashtag.HashtagRegex), Constants.HASHTAG_REGEX);
    }
}

export class BaseEmailExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor() {
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseEmail.EmailRegex), Constants.EMAIL_REGEX);
        // EmailRegex2 will break the code as it's not supported in Javascript, comment out for now
        // .set(RegExpUtility.getSafeRegExp(BaseEmail.EmailRegex2), Constants.EMAIL_REGEX)
    }
}

export interface IURLExtractorConfiguration {
    UrlRegex: RegExp;
    IpUrlRegex: RegExp;
}

export class BaseURLExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;
    ambiguousTimeTerm: RegExp;

    constructor(config: IURLExtractorConfiguration) {
        super();
        this.regexes = new Map<RegExp, string>()
            .set(config.UrlRegex, Constants.URL_REGEX)
            .set(RegExpUtility.getSafeRegExp(BaseURL.UrlRegex2), Constants.URL_REGEX)
            .set(config.IpUrlRegex, Constants.URL_REGEX);
        this.ambiguousTimeTerm = RegExpUtility.getSafeRegExp(BaseURL.AmbiguousTimeTerm);
    }

    isValidMatch(match: Match): boolean {
        // For cases like "7.am" or "8.pm" which are more likely time terms.
        return !RegExpUtility.isMatch(this.ambiguousTimeTerm, match.value);
    }
}

export class BaseGUIDExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor() {
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseGUID.GUIDRegex), Constants.GUID_REGEX);
    }
}
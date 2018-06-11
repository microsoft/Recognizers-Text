import { IExtractor, ExtractResult, RegExpUtility, Match, StringUtility } from "@microsoft/recognizers-text";
import { BasePhoneNumbers } from "../resources/basePhoneNumbers";
import { BaseIp } from "../resources/baseIp";
import { BaseMention } from "../resources/baseMention";
import { BaseHashtag } from "../resources/baseHashtag";
import { BaseEmail } from "../resources/baseEmail";
import { BaseURL } from "../resources/baseURL";
import { Constants } from "./constants";

export abstract class BaseSequenceExtractor implements IExtractor {
    abstract regexes: Map<RegExp, string>;

    protected extractType: string;

    extract(source: string): Array<ExtractResult> {
        let results = new Array<ExtractResult>();

        if (StringUtility.isNullOrWhitespace(source)) {
            return results;
        }

        let matchSource = new Map<Match, string>();
        let matched = new Array<boolean>(source.length);

        //Traverse every match results to see each position in the text is matched or not.
        var collections = this.regexes.forEach((typeExtracted, regex) => {
            RegExpUtility.getMatches(regex, source).forEach(match => {
                for (var j = 0; j < match.length; j++) {
                    matched[match.index + j] = true;
                }

                // Keep Source Data for extra information
                matchSource.set(match, typeExtracted);
            })
        });

        //Form the extracted results from all the matched intervals in the text.
        let lastNotMatched = -1;
        for (var i = 0; i < source.length; i++) {
            if (matched[i]) {
                if (i + 1 == source.length || !matched[i + 1]) {
                    let start = lastNotMatched + 1;
                    let length = i - lastNotMatched;
                    let substr = source.substr(start, length);
                    let matchFunc = (o: Match) =>  o.index == start && o.length == length;

                    var srcMatch = Array.from(matchSource.keys()).find(matchFunc);
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
            else
            {
                lastNotMatched = i;
            }
        }

        return results;
    }
}

export class BasePhoneNumberExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor(){
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.BrazilPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_BRAZIL)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.GeneralPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_GENERAL)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.UkPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_UK)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.GermanyPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_GERMANY)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.USPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_US)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.CNPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_CN)
            .set(RegExpUtility.getSafeRegExp(BasePhoneNumbers.SpecialPhoneNumberRegex), Constants.PHONE_NUMBER_REGEX_SPECIAL)
    }
    extract(source: string): Array<ExtractResult> {
        let ers = super.extract(source)
        let ret = new Array<ExtractResult>()
        for (let er of ers) {
            let ch = source[er.start - 1];
            if (er.start === 0 || BasePhoneNumbers.SeparatorCharList.indexOf(ch) === -1) {
                ret.push(er); 
            }
        }
        return ret;
    }
}

export class BaseIpExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor(){
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseIp.Ipv4Regex), Constants.IP_REGEX_IPV4)
            .set(RegExpUtility.getSafeRegExp(BaseIp.Ipv6Regex), Constants.IP_REGEX_IPV6)
    }

    extract(source: string): Array<ExtractResult> {
        let results = new Array<ExtractResult>();

        if (StringUtility.isNullOrWhitespace(source)) {
            return results;
        }

        let matchSource = new Map<Match, string>();
        let matched = new Array<boolean>(source.length);
        
        var collections = this.regexes.forEach((typeExtracted, regex) => {
            RegExpUtility.getMatches(regex, source).forEach(match => {
                for (var j = 0; j < match.length; j++) {
                    matched[match.index + j] = true;
                }

                // Keep Source Data for extra information
                matchSource.set(match, typeExtracted);
            })
        });
        
        let lastNotMatched = -1;
        for (var i = 0; i < source.length; i++) {
            if (matched[i]) {
                if (i + 1 == source.length || !matched[i + 1]) {
                    let start = lastNotMatched + 1;
                    let length = i - lastNotMatched;
                    let substr = source.substr(start, length);
                    if (substr.startsWith(Constants.IPV6_ELLIPSIS) && start > 0 && this.isLetterOrDigit(source[start - 1])) {
                        continue;
                    }
                    else if (substr.endsWith(Constants.IPV6_ELLIPSIS) && i + 1 < source.length && this.isLetterOrDigit(source[i + 1])) {
                        continue;
                    }

                    let matchFunc = (o: Match) =>  o.index == start && o.length == length;

                    var srcMatch = Array.from(matchSource.keys()).find(matchFunc);
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
            else
            {
                lastNotMatched = i;
            }
        }

        return results;
    }

    isLetterOrDigit(c: string): boolean{
        return new RegExp("[0-9a-zA-z]").test(c);
    }
}

export class BaseMentionExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor(){
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseMention.MentionRegex), Constants.MENTION_REGEX)
    }
}

export class BaseHashtagExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor(){
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseHashtag.HashtagRegex), Constants.HASHTAG_REGEX)
    }
}

export class BaseEmailExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor(){
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseEmail.EmailRegex), Constants.EMAIL_REGEX)
    }
}

export class BaseURLExtractor extends BaseSequenceExtractor {
    regexes: Map<RegExp, string>;

    constructor(){
        super();
        this.regexes = new Map<RegExp, string>()
            .set(RegExpUtility.getSafeRegExp(BaseURL.UrlRegex), Constants.URL_REGEX)
            .set(RegExpUtility.getSafeRegExp(BaseURL.IpUrlRegex), Constants.URL_REGEX)
    }
}
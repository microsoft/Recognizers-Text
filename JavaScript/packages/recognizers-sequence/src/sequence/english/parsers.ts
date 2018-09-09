import { BasePhoneNumbers } from "../../resources/basePhoneNumbers";
import { BaseSequenceParser, BaseIpParser } from "../parsers";
import { ExtractResult, ParseResult, Match } from "@microsoft/recognizers-text";

export class PhoneNumberParser extends BaseSequenceParser {
    scoreUpperBound = 100;
    scoreLowerBound = 0;
    baseScore = 30;
    countryCodeAward = 40;
    areaCodeAward = 20;
    formattedAward = 10;
    lengthAward = 5;
    maxFormatIndicatorNum = 4;
    maxLengthAwardNum = 4;
    phoneNumberLengthBase = 8;

    ScorePhoneNumber(phoneNumberText: string): number{
        let score = this.baseScore;

        let countryCodeRegex = new RegExp(BasePhoneNumbers.CountryCodeRegex);
        let areaCodeRegex = new RegExp(BasePhoneNumbers.AreaCodeIndicatorRegex);
        let formatIndicatorRegex = new RegExp(BasePhoneNumbers.FormatIndicatorRegex, "ig");

        // Country code score or area code score 
        score += countryCodeRegex.test(phoneNumberText) ?
            this.countryCodeAward : areaCodeRegex.test(phoneNumberText) ? this.areaCodeAward : 0;

        // Formatted score
        if(formatIndicatorRegex.test(phoneNumberText)){
            var formatIndicatorCount = phoneNumberText.match(formatIndicatorRegex).length;
            score += Math.min(formatIndicatorCount, this.maxFormatIndicatorNum) * this.formattedAward;
        }

        // Length score
        score += Math.min((phoneNumberText.length - this.phoneNumberLengthBase), this.maxLengthAwardNum) * this.lengthAward; 

        return Math.max(Math.min(score, this.scoreUpperBound), this.scoreLowerBound) / this.scoreUpperBound;
    }

    parse(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        result.resolutionStr = extResult.text;
        result.value = this.ScorePhoneNumber(extResult.text);
        return result;
    }
}

export class IpParser extends BaseIpParser {
    
}

export class MentionParser extends BaseSequenceParser {
    
} 

export class HashtagParser extends BaseSequenceParser {
    
}

export class EmailParser extends BaseSequenceParser {
    
}

export class URLParser extends BaseSequenceParser {
    
}
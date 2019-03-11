import { BasePhoneNumbers } from "../../resources/basePhoneNumbers";
import { BaseSequenceParser, BaseIpParser } from "../parsers";
import { ExtractResult, ParseResult, Match } from "@microsoft/recognizers-text";
import { BaseGUID } from "../../resources/baseGUID";

export class PhoneNumberParser extends BaseSequenceParser {
    scoreUpperLimit = 100;
    scoreLowerLimit = 0;
    baseScore = 30;
    countryCodeAward = 40;
    areaCodeAward = 30;
    formattedAward = 20;
    lengthAward = 10;
    typicalFormatDeductionScore = 40;
    continueDigitDeductionScore = 10;
    tailSameDeductionScore = 10;
    continueFormatIndicatorDeductionScore = 20;
    wrongFormatIndicatorDeductionScore = 20;
    maxFormatIndicatorNum = 3;
    maxLengthAwardNum = 3;
    tailSameLimit = 2;
    phoneNumberLengthBase = 8;
    pureDigitLengthLimit = 11;
    completeBracketRegex = new RegExp("\\(.*\\)");
    singleBracketRegex = new RegExp("\\(|\\)");
    tailSameDigitRegex = new RegExp("([\\d])\\1{2,10}$");
    pureDigitRegex = new RegExp("^\\d*$");
    continueDigitRegex = new RegExp("\\d{5}\\d*", "ig");
    digitRegex = new RegExp("\\d", "ig");

    ScorePhoneNumber(phoneNumberText: string): number {
        let score = this.baseScore;

        let countryCodeRegex = new RegExp(BasePhoneNumbers.CountryCodeRegex);
        let areaCodeRegex = new RegExp(BasePhoneNumbers.AreaCodeIndicatorRegex);
        let formatIndicatorRegex = new RegExp(BasePhoneNumbers.FormatIndicatorRegex, "ig");
        let noAreaCodeUSphonenumbeRegex = new RegExp(BasePhoneNumbers.NoAreaCodeUSPhoneNumberRegex);

        // Country code score or area code score 
        score += countryCodeRegex.test(phoneNumberText) ?
            this.countryCodeAward : areaCodeRegex.test(phoneNumberText) ? this.areaCodeAward : 0;
        
        // Formatted score
        if (formatIndicatorRegex.test(phoneNumberText)) {
            var formatMathes = phoneNumberText.match(formatIndicatorRegex);
            var formatIndicatorCount = formatMathes.length;
            score += Math.min(formatIndicatorCount, this.maxFormatIndicatorNum) * this.formattedAward;
            score -= formatMathes.some(match => match.length > 1) ? this.continueFormatIndicatorDeductionScore : 0;
            if (this.singleBracketRegex.test(phoneNumberText) && !this.completeBracketRegex.test(phoneNumberText)) {
                score -= this.wrongFormatIndicatorDeductionScore;
            }
        }
       
        // Same tailing digit deduction
        if (this.tailSameDigitRegex.test(phoneNumberText)) {
            score -= (phoneNumberText.match(this.tailSameDigitRegex)[0].length - this.tailSameLimit) * this.tailSameDeductionScore;
           
        }
        
        // Length score
        if (this.digitRegex.test(phoneNumberText)) {
            score += Math.min((phoneNumberText.match(this.digitRegex).length - this.phoneNumberLengthBase),
                this.maxLengthAwardNum) * this.lengthAward;
        }
        
        // Pure digit deduction
        if (this.pureDigitRegex.test(phoneNumberText)) {
            score -= phoneNumberText.length > this.pureDigitLengthLimit ?
                (phoneNumberText.length - this.pureDigitLengthLimit) * this.lengthAward : 0;
        }
        
        // Special format deduction
        score -= BasePhoneNumbers.TypicalDeductionRegexList.some(o => new RegExp(o).test(phoneNumberText)) ? this.typicalFormatDeductionScore : 0;
        
        // Continue digit deduction
        if (this.continueDigitRegex.test(phoneNumberText)) {
            score -= Math.max(phoneNumberText.match(this.continueDigitRegex).length - 1, 0) * this.continueDigitDeductionScore;
        }

        // Special award for special USphonenumber, i.e. 223-4567 or 223 - 4567
        if (noAreaCodeUSphonenumbeRegex.test(phoneNumberText))
        {
            score += this.lengthAward * 1.5;
        }

        return Math.max(Math.min(score, this.scoreUpperLimit), this.scoreLowerLimit) / (this.scoreUpperLimit - this.scoreLowerLimit);
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

export class GUIDParser extends BaseSequenceParser {
    scoreUpperLimit = 100;
    scoreLowerLimit = 0;
    baseScore = 100;
    noBoundaryPenalty = 10;
    noFormatPenalty = 10;
    pureDigitPenalty = 15;
    pureDigitRegex = new RegExp("^\\d*$");
    formatRegex = new RegExp("-");

    ScoreGUID(GUIDText: string): number {
        let score = this.baseScore;

        let guidElementRegex = new RegExp(BaseGUID.GUIDRegexElement);

        if (guidElementRegex.test(GUIDText)){
            let elementMatch = GUIDText.match(guidElementRegex);
            let startIndex = elementMatch.index;
            let elementGUID = elementMatch[0];
            score -= startIndex == 0 ? this.noBoundaryPenalty : 0;
            score -= this.formatRegex.test(elementGUID) ? 0 : this.noFormatPenalty;
            score -= this.pureDigitRegex.test(GUIDText) ? this.pureDigitPenalty : 0;
        }

        return Math.max(Math.min(score, this.scoreUpperLimit), this.scoreLowerLimit) / (this.scoreUpperLimit - this.scoreLowerLimit);
    }

    parse(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        result.resolutionStr = extResult.text;
        result.value = this.ScoreGUID(extResult.text);
        return result;
    }
}
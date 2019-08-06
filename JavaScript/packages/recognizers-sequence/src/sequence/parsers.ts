import { IParser, ExtractResult, ParseResult } from "@microsoft/recognizers-text";

export class BaseSequenceParser implements IParser {
    parse(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        result.resolutionStr = extResult.text;
        return result;
    }
}

export class BaseIpParser extends BaseSequenceParser {
    parse(extResult: ExtractResult): ParseResult {
        let result = new ParseResult(extResult);
        result.resolutionStr = this.dropLeadingZeros(extResult.text);
        return result;
    }

    private dropLeadingZeros(text: string): string {
        let result = "";

        let number = "";
        for (let i = 0; i < text.length; i++) {
            let c = text[i];
            if (c == '.' || c == ':') {
                if (number != "") {
                    number = number == "0" ? number : number.replace(/^(0*)/, "");
                    number = number == "" ? "0" : number;
                    result += number;
                }
                result += text[i];
                number = "";
            }
            else {
                number += c.toString();
                if (i == text.length - 1) {
                    number = number == "0" ? number : number.replace(/^(0*)/, "");
                    number = number == "" ? "0" : number;
                    result += number;
                }
            }
        }

        return result;
    }
}
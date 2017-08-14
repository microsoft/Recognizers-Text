import { Constants } from "./constants";
import { IExtractor, ExtractResult } from "../number/extractors"
import { Match, RegExpUtility } from "../utilities";

export interface IDateExtractorConfiguration {
    dateRegexList: RegExp[]
}

export class BaseDateExtractor implements IExtractor {
    private readonly config: IDateExtractorConfiguration;

    constructor(config: IDateExtractorConfiguration) {
        this.config = config;
    }

    extract(source: string): Array<ExtractResult> {
        let tokens = new Array<any>();
        tokens.push(
            this.basicRegexMatch(source),
            this.implicitDate(source),
            this.numberWithMonth(source),
            this.durationWithBeforeAndAfter(source)
        );
        return tokens;
    }

    private basicRegexMatch(source: string): Array<any> {
        let ret = [];
        this.config.dateRegexList.forEach(regexp => {
            let matches = RegExpUtility.getMatches(regexp, source);
            matches.forEach(match => {
                ret.push({start: match.index, end: match.index + match.length});
            });
        });
        return ret;
    }

    private implicitDate(source: string): Array<any> {
        return null;
    }

    private numberWithMonth(source: string): Array<any> {
        return null;
    }

    private durationWithBeforeAndAfter(source: string): Array<any> {
        return null;
    }
}
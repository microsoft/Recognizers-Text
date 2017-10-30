
import { INumberParserConfiguration, BaseNumberParser, BasePercentageParser } from "./parsers";
import { Culture } from "../culture";
import { Constants } from "./constants";
import { ChineseNumberParser } from "./chinese/parsers";
import { ChineseNumberParserConfiguration } from "./chinese/parserConfiguration";

export enum AgnosticNumberParserType {
    Cardinal,
    Double,
    Fraction,
    Integer,
    Number,
    Ordinal,
    Percentage
}

export class AgnosticNumberParserFactory {
    static getParser(type: AgnosticNumberParserType, languageConfiguration: INumberParserConfiguration): BaseNumberParser {

        let isChinese = languageConfiguration.cultureInfo.code.toLowerCase() === Culture.Chinese;

        let parser: BaseNumberParser;

        if (isChinese) {
            parser = new ChineseNumberParser(languageConfiguration as ChineseNumberParserConfiguration);
        }
        else {
            parser = new BaseNumberParser(languageConfiguration);
        }

        switch (type) {
            case AgnosticNumberParserType.Cardinal:
                parser.supportedTypes = [Constants.SYS_NUM_CARDINAL, Constants.SYS_NUM_INTEGER, Constants.SYS_NUM_DOUBLE];
                break;
            case AgnosticNumberParserType.Double:
                parser.supportedTypes = [Constants.SYS_NUM_DOUBLE];
                break;
            case AgnosticNumberParserType.Fraction:
                parser.supportedTypes = [Constants.SYS_NUM_FRACTION];
                break;
            case AgnosticNumberParserType.Integer:
                parser.supportedTypes = [Constants.SYS_NUM_INTEGER];
                break;
            case AgnosticNumberParserType.Ordinal:
                parser.supportedTypes = [Constants.SYS_NUM_ORDINAL];
                break;
            case AgnosticNumberParserType.Percentage:
                if (!isChinese) {
                    parser = new BasePercentageParser(languageConfiguration);
                }
                break;
        }

        return parser;
    }
}
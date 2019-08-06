
import { INumberParserConfiguration, BaseNumberParser, BasePercentageParser } from "./parsers";
import { Culture } from "../culture";
import { Constants } from "./constants";
import { BaseCJKNumberParser } from "./cjkParsers";
import { ChineseNumberParserConfiguration } from "./chinese/parserConfiguration";
import { JapaneseNumberParserConfiguration } from "./japanese/parserConfiguration";

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
        let isJapanese = languageConfiguration.cultureInfo.code.toLowerCase() === Culture.Japanese;

        let parser: BaseNumberParser;

        if (isChinese) {
            parser = new BaseCJKNumberParser(languageConfiguration as ChineseNumberParserConfiguration);
        }
        else if (isJapanese) {
            parser = new BaseCJKNumberParser(languageConfiguration as JapaneseNumberParserConfiguration);
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
                if (!isChinese && !isJapanese) {
                    parser = new BasePercentageParser(languageConfiguration);
                }
                break;
        }

        return parser;
    }
}
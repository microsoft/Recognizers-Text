import { CultureInfo, Culture } from "../../culture";
import { Constants } from "../constants";
import { EnglishNumberWithUnitExtractorConfiguration, EnglishNumberWithUnitParserConfiguration } from "./base";

export class EnglishTemperatureExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {
    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_TEMPERATURE;

        this.suffixList = EnglishTemperatureExtractorConfiguration.temperatureSuffixList();
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = new Array<string>(
            "c",
            "f",
            "k");
    }


    static temperatureSuffixList(): ReadonlyMap<string, string> {
        return new Map<string, string>([
            [
                "F",
                "degrees fahrenheit|degree fahrenheit|deg fahrenheit|degs fahrenheit|fahrenheit|°f|degrees farenheit|degree farenheit|deg farenheit|degs farenheit|degrees f|degree f|deg f|degs f|farenheit|f"
            ],
            ["K", "k|kelvin"],
            ["R", "rankine|°r"],
            ["D", "delisle|°de"],
            [
                "C",
                "degrees celsius|degree celsius|deg celsius|degs celsius|celsius|degrees celcius|degree celcius|celcius|deg celcius|degs celcius|degrees centigrade|degree centigrade|centigrade|degrees centigrate|degree centigrate|degs centigrate|deg centigrate|centigrate|degrees c|degree c|deg c|degs c|°c|c"
            ],
            ["Degree", "degree|degrees|deg.|deg|°"]
        ]);
    }
}

export class EnglishTemperatureParserConfiguration extends EnglishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if(!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.BindDictionary(EnglishTemperatureExtractorConfiguration.temperatureSuffixList());
    }
}
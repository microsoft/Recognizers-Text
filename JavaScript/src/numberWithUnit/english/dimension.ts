import { CultureInfo, Culture } from "../../culture";
import { Constants } from "../constants";
import { EnglishNumberWithUnitExtractorConfiguration, EnglishNumberWithUnitParserConfiguration } from "./base";

export class EnglishDimensionExtractorConfiguration extends EnglishNumberWithUnitExtractorConfiguration {

    readonly suffixList: ReadonlyMap<string, string>;
    readonly prefixList: ReadonlyMap<string, string>;
    readonly ambiguousUnitList: ReadonlyArray<string>;
    readonly extractType: string;

    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.extractType = Constants.SYS_UNIT_DIMENSION;

        this.suffixList = EnglishDimensionExtractorConfiguration.dimensionSuffixList();
        this.prefixList = new Map<string, string>();
        this.ambiguousUnitList = new Array<string>(
            "barrel",
            "barrels",
            "grain",
            "pound",
            "stone",
            "yards",
            "yard",
            "cord",
            "dram",
            "feet",
            "foot",
            "gill",
            "knot",
            "peck",
            "cup",
            "fps",
            "pts");
    }

    static dimensionSuffixList(): ReadonlyMap<string, string> {
        return new Map<string, string>([
            // Length
            ["Meter", "m|meter|metre|meters|metres"],
            ["Kilometer", "km|kilometer|kilometer|kilometers|kilometres|kilo meter|kilo meters|kilo metres|kilo metre"],
            ["Decimeter", "dm|decimeter|decimeters|decimetre|decimetres|deci meter|deci meters|deci metres|deci metre"],
            ["Centimeter", "cm|centimeter|centimeters|centimetre|centimetres|centi meter|centi meters|centi metres|centi metre"],
            ["Micrometer", "mm|micrometer|micrometre|micrometers|micrometres|micro meter|micro meters|micro metres|micro metre"],
            ["Mile", "-mile|mile|miles"],
            ["Yard", "yard|yards"],
            ["Inch", "-inch|inch|inches"],
            ["Foot", "-foot|foot|feet|ft"],
            ["Light year", "light year|light-year|light years|light-years"],
            ["Pt", "pt|pts"],
            ["Picometer", "picometer|picometre|picometers|picometres|pico meter|picometers|pico metres|pico metre"],
            ["Nanometer", "nm|nanometer|nanometre|nanometers|nanometres|nano meter|nano meters|nano metres|nano metre"],
            // Speed
            ["Meter per second", "meters / second|m/s|meters per second|metres per second|meter per second|metre per second"],
            ["Kilometer per hour", "km/h|kilometres per hour|kilometers per hour|kilometer per hour|kilometers / hour"],
            ["Kilometer per minute", "km/min|kilometers per minute|kilometres per minute|kilometer per minute"],
            ["Kilometer per second", "km/s|kilometers per second|kilometres per second"],
            ["Mile per hour", "mph|miles per hour|mi/h|mile / hour|miles an hour"],
            ["Knot", "kt|knot|kn"],
            ["Foot per second", "ft/s|foot/s|feet per second|fps"],
            ["Foot per minute", "ft/min|foot/min|feet per minute"],
            ["Yard per minute", "yards per minute|yards / minute|yards/min"],
            ["Yard per second", "yards per second|yards / second|yards/s"],
            // Area
            ["Square meter", "m2|sq m|sq meter|sq meters|sq metres|sq metre|square meter|square meters|square metre|square metres"],
            ["Acre", "-acre|acre|acres"],
            // Volume
            ["Cubic meter", "m3|cubic meter|cubic meters|cubic metre|cubic metres"],
            ["Square kilometer", "square kilometers|square kilometer|square kilometres|square kilometre|sq kilometer|sq kilometers|sq kilometre|sq kilometres|km2"],
            ["Liter", "l|litre|liter|liters|litres"],
            ["Milliliter", "ml|mls|millilitre|milliliter|millilitres|milliliters"],
            ["Cubic feet", "cubic foot|cubic feet"],
            ["Volume", "cubic centimeter|cubic centimetre|cubic meter|cubic metre|fl oz|fluid ounce|ounce|oz|cup|hectoliter|hectolitre|decaliter|decalitre|dekaliter|dekalitre|deciliter|decilitre|cubic yard|cubic milliliter|cubic millilitre|cubic inch|cubic mile|teaspoon|tablespoon|fluid ounce|fluid dram|gill|pint|quart|minim|barrel|cord|peck|bushel|hogshead"],
            // Weight
            ["Kilogram", "kg|kilogram|kilograms|kilo|kilos"],
            ["Milligram", "mg|milligram|milligrams"],
            ["Barrel", "barrels|barrel"],
            ["Gallon", "-gallon|gallons|gallon"],
            ["Gram", "g|gram|grams"],
            ["Metric ton", "metric tons|metric ton"],
            ["Ton", "-ton|ton|tons|tonne|tonnes"],
            ["Pound", "pound|pounds|lb"],
            ["Ounce", "-ounce|ounce|oz|ounces"],
            ["Weight", "pennyweight|grain|british long ton|US short hundredweight|stone|dram"],
            // Information
            ["Bit", "-bit|bit"],
            ["Byte", "-byte|byte"],
            ["Kilobyte", "-kilobyte|-kilobytes|kilobyte|kB|kilobytes|kilo byte|kilo bytes|kByte"],
            ["Megabyte", "-megabyte|-megabytes|megabyte|mB|megabytes|mega byte|mega bytes|MByte"],
            ["Gigabyte", "-gigabyte|-gigabytes|gigabyte|gB|gigabytes|giga byte|giga bytes|GByte"],
            ["Terabyte", "-terabyte|-terabytes|terabyte|tB|terabytes|tera byte|tera bytes|TByte"],
            ["Petabyte", "-petabyte|-petabytes|petabyte|pB|petabytes|peta byte|peta bytes|PByte"]
        ]);
    }
}

export class EnglishDimensionParserConfiguration extends EnglishNumberWithUnitParserConfiguration {
    constructor(ci?: CultureInfo) {
        if (!ci) {
            ci = new CultureInfo(Culture.English);
        }

        super(ci);

        this.BindDictionary(EnglishDimensionExtractorConfiguration.dimensionSuffixList());
    }
}
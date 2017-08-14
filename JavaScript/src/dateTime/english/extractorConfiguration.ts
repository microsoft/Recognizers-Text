import { IDateExtractorConfiguration } from "../extractors";
import { EnglishDateTime } from "../../resources/englishDateTime";
import * as XRegExp from "xregexp";

export class EnglishDateExtractorConfiguration implements IDateExtractorConfiguration {
    readonly dateRegexList: RegExp[];

    constructor() {
        this.dateRegexList = [
            XRegExp(EnglishDateTime.DateExtractor1, "gis"),
            XRegExp(EnglishDateTime.DateExtractor2, "gis"),
            XRegExp(EnglishDateTime.DateExtractor3, "gis"),
            XRegExp(EnglishDateTime.DateExtractor4, "gis"),
            XRegExp(EnglishDateTime.DateExtractor5, "gis"),
            XRegExp(EnglishDateTime.DateExtractor6, "gis"),
            XRegExp(EnglishDateTime.DateExtractor7, "gis"),
            XRegExp(EnglishDateTime.DateExtractor8, "gis"),
            XRegExp(EnglishDateTime.DateExtractor9, "gis"),
            XRegExp(EnglishDateTime.DateExtractorA, "gis"),
        ];
    }
}
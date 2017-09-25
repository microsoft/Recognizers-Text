var _ = require('lodash');
var Constants = require('./constants');
var SupportedCultures = require('./cultures');
var DateTimeOptions = require('../compiled/dateTime/baseMerged').DateTimeOptions;

var LanguagesConfig = [
    'English'
    // 'Spanish'
];

var ExtractorTypes = {
    'Date':             ['Date'],
    'Time':             ['Time'],
    'DatePeriod':       ['DatePeriod'],
    'TimePeriod':       ['TimePeriod'],
    'DateTime':         ['DateTime'],
    'DateTimePeriod':   ['DateTimePeriod'],
    'Duration':         ['Duration'],
    'Holiday':          ['Holiday'],
    'Set':              ['Set'],
    'Merged':           ['Merged', DateTimeOptions.None],
    'MergedSkipFromTo': ['Merged', DateTimeOptions.SkipFromToMerge],
};

var extractorConfigs = LanguagesConfig
    .map(c => _.values(ExtractorTypes).map(cfg => ({ lang: c, cfg: cfg })))
    .reduce((a, b) => a.concat(b), []);                      // flatten

// [Eng-Date, Eng-Set, Eng-Merged, Eng-MergedSkipFromTo, ... ]
var extractorKeys = extractorConfigs.map(cfg => _.findKey(SupportedCultures, (c) => c.cultureName === cfg.lang) + '-' + _.findKey(ExtractorTypes, o => o === cfg.cfg))
var extractorObjects = extractorConfigs.map(cfg => createExtractor(cfg.lang, cfg.cfg[0], cfg.cfg[1]));

// { 'Eng-Date': {extractor}, 'Eng-Set': {extractor}, 'Eng-Merged': {extractor}, 'Eng-MergedSkipFromTo': ... }
module.exports =  _.zipObject(extractorKeys, extractorObjects);

function createExtractor(lang, extractor, options) {
    var extractorModuleName = '../compiled/dateTime/base' + extractor;
    var extractorTypeName = [Constants.Base, extractor, Constants.Extractor].join('');
    var ExtractorType = require(extractorModuleName)[extractorTypeName];
    if (!ExtractorType) {
        throw new Error(`Extractor Type ${extractorTypeName} was not found in module ${extractorModuleName}`);
    }

    var configModuleName = '../compiled/dateTime/' + lang.toLowerCase() + '/' + toCamelCase(extractor) + Constants.Configuration;
    var configTypeName = lang + extractor + Constants.ExtractorConfiguration;
    var ConfigType = require(configModuleName)[configTypeName];
    if (!ConfigType) {
        throw new Error(`Config Type ${configTypeName} was not found in module ${configModuleName}`);
    }

    return new ExtractorType(new ConfigType(), options);
}


function toCamelCase(name) {
    return name.substring(0, 1).toLowerCase() + name.substring(1);
}

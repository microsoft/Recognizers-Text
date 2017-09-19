var DateTimeOptions = require('../compiled/dateTime/baseMerged').DateTimeOptions;

module.exports = {
    // English
    'Eng-Date':             createExtractor('English', 'Date'),
    'Eng-Time':             createExtractor('English', 'Time'),
    'Eng-DatePeriod':       createExtractor('English', 'DatePeriod'),
    'Eng-TimePeriod':       createExtractor('English', 'TimePeriod'),
    'Eng-DateTime':         createExtractor('English', 'DateTime'),
    'Eng-DateTimePeriod':   createExtractor('English', 'DateTimePeriod'),
    'Eng-Duration':         createExtractor('English', 'Duration'),
    'Eng-Holiday':          createExtractor('English', 'Holiday'),
    'Eng-Set':              createExtractor('English', 'Set'),
    'Eng-Merged':           createExtractor('English', 'Merged', DateTimeOptions.None),
    'Eng-MergedSkipFromTo': createExtractor('English', 'Merged', DateTimeOptions.SkipFromToMerge)
};

function createExtractor(lang, extractor, options) {
    var extractorModuleName = '../compiled/dateTime/base' + extractor;
    var extractorTypeName = 'Base' + extractor + 'Extractor';
    var ExtractorType = require(extractorModuleName)[extractorTypeName];
    if (!ExtractorType) {
        throw new Error(`Extractor Type ${extractorTypeName} was not found in module ${extractorModuleName}`);
    }

    var configModuleName = '../compiled/dateTime/' + lang.toLowerCase() + '/' + extractor + 'Configuration';
    var configTypeName = lang + extractor + 'ExtractorConfiguration';
    var ConfigType = require(configModuleName)[configTypeName];
    if (!ConfigType) {
        throw new Error(`Config Type ${configTypeName} was not found in module ${configModuleName}`);
    }

    return new ExtractorType(new ConfigType(), options);
}

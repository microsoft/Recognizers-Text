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
    'Eng-MergedSkipFromTo': createExtractor('English', 'Merged', DateTimeOptions.SkipFromToMerge),

    // Spanish
    // 'Spa-Date':             createExtractor('Spanish', 'Date'),
    // 'Spa-Time':             createExtractor('Spanish', 'Time'),
    // 'Spa-DatePeriod':       createExtractor('Spanish', 'DatePeriod'),
    // 'Spa-TimePeriod':       createExtractor('Spanish', 'TimePeriod'),
    // 'Spa-DateTime':         createExtractor('Spanish', 'DateTime'),
    // 'Spa-DateTimePeriod':   createExtractor('Spanish', 'DateTimePeriod'),
    // 'Spa-Duration':         createExtractor('Spanish', 'Duration'),
    // 'Spa-Holiday':          createExtractor('Spanish', 'Holiday'),
    // 'Spa-Set':              createExtractor('Spanish', 'Set'),
    // 'Spa-Merged':           createExtractor('Spanish', 'Merged', DateTimeOptions.None),
    // 'Spa-MergedSkipFromTo': createExtractor('Spanish', 'Merged', DateTimeOptions.SkipFromToMerge)
};

function createExtractor(lang, extractor, options) {
    var extractorModuleName = '../compiled/dateTime/base' + extractor;
    var extractorTypeName = 'Base' + extractor + 'Extractor';
    var ExtractorType = require(extractorModuleName)[extractorTypeName];
    if (!ExtractorType) {
        throw new Error(`Extractor Type ${extractorTypeName} was not found in module ${extractorModuleName}`);
    }

    var configModuleName = '../compiled/dateTime/' + lang.toLowerCase() + '/' + toCamelCase(extractor) + 'Configuration';
    var configTypeName = lang + extractor + 'ExtractorConfiguration';
    var ConfigType = require(configModuleName)[configTypeName];
    if (!ConfigType) {
        throw new Error(`Config Type ${configTypeName} was not found in module ${configModuleName}`);
    }

    return new ExtractorType(new ConfigType(), options);
}


function toCamelCase(name) {
    return name.substring(0, 1).toLowerCase() + name.substring(1);
}

var EnglishCommonDateTimeParserConfiguration = require('../compiled/dateTime/english/baseConfiguration').EnglishCommonDateTimeParserConfiguration;

var englishCommonConfig = new EnglishCommonDateTimeParserConfiguration();

module.exports = {
    // English
    'Eng-Date':             createParser('English', 'Date', englishCommonConfig),
    'Eng-Time':             createParser('English', 'Time', englishCommonConfig),
    'Eng-DatePeriod':       createParser('English', 'DatePeriod', englishCommonConfig),
    'Eng-TimePeriod':       createParser('English', 'TimePeriod', englishCommonConfig),
    'Eng-DateTime':         createParser('English', 'DateTime', englishCommonConfig),
    'Eng-DateTimePeriod':   createParser('English', 'DateTimePeriod', englishCommonConfig),
    'Eng-Duration':         createParser('English', 'Duration', englishCommonConfig),
    'Eng-Holiday':          createParser('English', 'Holiday'),
    'Eng-Set':              createParser('English', 'Set', englishCommonConfig),
    'Eng-Merged':           createParser('English', 'Merged', englishCommonConfig)
};

function createParser(lang, parser, commonConfig) {
    var parserModuleName = '../compiled/dateTime/base' + parser;
    var parserTypeName = 'Base' + parser + 'Parser';
    var ParserType = require(parserModuleName)[parserTypeName];
    if (!ParserType) {
        throw new Error(`Parser Type ${parserTypeName} was not found in module ${parserModuleName}`);
    }

    var configModuleName = '../compiled/dateTime/' + lang.toLowerCase() + '/' + parser + 'Configuration';
    var configTypeName = lang + parser + 'ParserConfiguration';
    var ConfigType = require(configModuleName)[configTypeName];
    if (!ConfigType) {
        throw new Error(`Config Type ${configTypeName} was not found in module ${configModuleName}`);
    }

    var config = commonConfig
        ? new ConfigType(commonConfig)
        : new ConfigType();

    return new ParserType(config);
}

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
    'Eng-Merged':           createParser('English', 'Merged', englishCommonConfig),

    // Spanish
    // 'Spa-Date':             createParser('Spanish', 'Date', SpanishCommonConfig),
    // 'Spa-Time':             createParser('Spanish', 'Time', SpanishCommonConfig),
    // 'Spa-DatePeriod':       createParser('Spanish', 'DatePeriod', SpanishCommonConfig),
    // 'Spa-TimePeriod':       createParser('Spanish', 'TimePeriod', SpanishCommonConfig),
    // 'Spa-DateTime':         createParser('Spanish', 'DateTime', SpanishCommonConfig),
    // 'Spa-DateTimePeriod':   createParser('Spanish', 'DateTimePeriod', SpanishCommonConfig),
    // 'Spa-Duration':         createParser('Spanish', 'Duration', SpanishCommonConfig),
    // 'Spa-Holiday':          createParser('Spanish', 'Holiday'),
    // 'Spa-Set':              createParser('Spanish', 'Set', SpanishCommonConfig),
    // 'Spa-Merged':           createParser('Spanish', 'Merged', SpanishCommonConfig)
};

function createParser(lang, parser, commonConfig) {
    var parserModuleName = '../compiled/dateTime/base' + parser;
    var parserTypeName = 'Base' + parser + 'Parser';
    var ParserType = require(parserModuleName)[parserTypeName];
    if (!ParserType) {
        throw new Error(`Parser Type ${parserTypeName} was not found in module ${parserModuleName}`);
    }

    var configModuleName = '../compiled/dateTime/' + lang.toLowerCase() + '/' + toCamelCase(parser) + 'Configuration';
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

function toCamelCase(name) {
    return name.substring(0, 1).toLowerCase() + name.substring(1);
}

var fs = require('fs');
var path = require('path');
var keys = require('lodash.keys');
var SupportedCultures = require('./cultures');
var supportedLanguages = keys(SupportedCultures);
var specsPath = '../../Specs';

module.exports.readAll = function (type) {
    // get list of specs (.json)
    var specFiles = getSpecFilePaths(path.join(__dirname, specsPath))
        // Ignore specs which type is different from parameter 'type'
        .filter(s => s.indexOf(path.sep + type + path.sep) !== -1)
        // Ignore non-supported languages
        .filter(s => supportedLanguages.find(lang => s.indexOf(path.sep + lang + path.sep) !== -1));

    // invalidate require cache
    specFiles.forEach(s => delete require.cache[s]);

    // parse specs
    return specFiles
        .map(s => ({
            config: getSuiteConfig(s),
            specs: require(s)
        }))
        .reverse();
};

// helpers
function getSpecFilePaths(specsPath) {
    if (!fs.existsSync(specsPath)) {
        throw new Error(`Specs directory not found at ${path.resolve(specsPath)}`);
    }

    return fs
        .readdirSync(specsPath).map(s => path.join(specsPath, s))
        .filter(p => fs.lstatSync(p).isDirectory())
        .map(s1 => fs.readdirSync(s1).map(s2 => path.join(s1, s2)))
        .reduce((a, b) => a.concat(b), [])                      // flatten
        .filter(p => fs.lstatSync(p).isDirectory())
        .map(s1 => fs.readdirSync(s1).map(s2 => path.join(s1, s2)))
        .reduce((a, b) => a.concat(b), [])                      // flatten
        .filter(s => s.indexOf('.json') !== -1);
}

function getSuiteConfig(jsonPath) {
    var parts = jsonPath.split(path.sep).slice(-3);

    return {
        type: parts[0],
        subType: parts[2].split('.json')[0],
        language: parts[1]
    };
}
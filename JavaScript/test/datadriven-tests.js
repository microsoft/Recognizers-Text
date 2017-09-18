var specsPath = '../Specs';
var supportedLanguages = ['Eng'];

var fs = require('fs');
var path = require('path');

// get list of specs (.json)
var specFiles = getSpecFilePaths(specsPath)
    // Ignore non-supported languages
    .filter(s => supportedLanguages.find(l => s.indexOf('/' + l + '/') !== -1))

// parse specs
var specs = specFiles
    .map(s => ({
        suite: getTestSuite(s),
        specs: require(path.join('../', s))
    }));

console.log(specs)

function getSpecFilePaths(specsPath) {
    return fs
        .readdirSync(specsPath).map(s => path.join(specsPath, s))
        .map(s1 => fs.readdirSync(s1).map(s2 => path.join(s1, s2)))
        .reduce((a, b) => a.concat(b), [])                      // flatten
        .map(s1 => fs.readdirSync(s1).map(s2 => path.join(s1, s2)))
        .reduce((a, b) => a.concat(b), [])                      // flatten
        .filter(s => s.indexOf('.json') !== -1);
}

function getTestSuite(jsonPath) {
    var parts = jsonPath.split('/').slice(2);

    return {
        type: parts[0],
        subType: parts[2].split('.json')[0],
        language: parts[1]
    };
}
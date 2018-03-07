var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');
var InitializationTests = require('./initialization-tests');

// Recognizer initialization test
InitializationTests(describe);

// Data-driven tests
SpecRunner(describe, specs.readAll());
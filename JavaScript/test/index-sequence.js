var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');
var sequenceRecognizerInitialization = require('./initialization-tests/sequenceRecognizer');

// Recognizer initialization test
sequenceRecognizerInitialization(describe);
// Data-driven tests
SpecRunner(describe, specs.readAll('Sequence'));
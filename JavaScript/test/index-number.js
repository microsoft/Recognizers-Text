var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');
var numberRecognizerInitialization = require('./initialization-tests/numberRecognizer');

// Recognizer initialization test
numberRecognizerInitialization(describe);
// Data-driven tests
SpecRunner(describe, specs.readAll('Number'));
var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');
var numberWithUnitRecognizerInitialization = require('./initialization-tests/numberWithUnitRecognizer');

// Recognizer initialization test
numberWithUnitRecognizerInitialization(describe);
// Data-driven tests
SpecRunner(describe, specs.readAll('NumberWithUnit'));

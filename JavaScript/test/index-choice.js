var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');
var choiceRecognizerInitialization = require('./initialization-tests/choiceRecognizer');

// Recognizer initialization test
choiceRecognizerInitialization(describe);
// Data-driven tests
SpecRunner(describe, specs.readAll('Choice'));

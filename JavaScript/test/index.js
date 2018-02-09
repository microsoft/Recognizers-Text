var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');
var numberRecognizerEnglish = require('./numberRecognizer-english');
var dateTimeRecognizerEnglish = require('./dateTimeRecognizer-english');

// run
SpecRunner(describe, specs.readAll());
numberRecognizerEnglish(describe);
dateTimeRecognizerEnglish(describe);

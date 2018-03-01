var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');
var choiceRecognizerEnglish = require('./choiceRecognizer-english');
var choiceRecognizerInitialization = require('./choiceRecognizer-initialization');
var dateTimeRecognizerEnglish = require('./dateTimeRecognizer-english');
var dateTimeRecognizerInitialization = require('./dateTimeRecognizer-initialization');
var numberRecognizerEnglish = require('./numberRecognizer-english');
var numberRecognizerInitialization = require('./numberRecognizer-initialization');
var numberWithUnitRecognizerEnglish = require('./numberWithUnitRecognizer-english');
var numberWithUnitRecognizerInitialization = require('./numberWithUnitRecognizer-initialization');

// run
SpecRunner(describe, specs.readAll());
dateTimeRecognizerEnglish(describe);
dateTimeRecognizerInitialization(describe);
choiceRecognizerEnglish(describe);
choiceRecognizerInitialization(describe);
numberRecognizerEnglish(describe);
numberRecognizerInitialization(describe);
numberWithUnitRecognizerEnglish(describe);
numberWithUnitRecognizerInitialization(describe);
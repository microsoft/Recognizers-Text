var numberRecognizerInitialization = require('./numberRecognizer');
var numberWithUnitRecognizerInitialization = require('./numberWithUnitRecognizer');
var dateTimeRecognizerInitialization = require('./dateTimeRecognizer');
var choiceRecognizerInitialization = require('./choiceRecognizer');
var sequenceRecognizerInitialization = require('./sequenceRecognizer');

//run
module.exports = function (describe) {
    numberRecognizerInitialization(describe);
    numberWithUnitRecognizerInitialization(describe);
    dateTimeRecognizerInitialization(describe);
    choiceRecognizerInitialization(describe);
    sequenceRecognizerInitialization(describe);
}
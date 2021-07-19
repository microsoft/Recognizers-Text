// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');
var dateTimeRecognizerInitialization = require('./initialization-tests/dateTimeRecognizer');

// Recognizer initialization test
dateTimeRecognizerInitialization(describe);
// Data-driven tests
SpecRunner(describe, specs.readAll('DateTime'));

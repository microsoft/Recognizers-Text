var describe = require('ava-spec').describe;
var SpecRunner = require('./runner');
var specs = require('./specs');

// run
SpecRunner(describe, specs.readAll());
var runner = require('./runner');

window.onload = function() {
    mocha.setup('bdd');

    // wrap chais's describe/it API with the same api as AVA
    // http://chaijs.com/api/bdd/
    var wrapIt = {
        is: function(actual, expected, message) {
            expect(actual).to.eql(expected, message);
        },
        deepEqual: function(actual, expected, message) {
            expect(actual).to.eql(expected, message);
        }
    };

    var runnerDescribe = function(name, run) {
        describe(name, function() {
            run(function(name, run) {
                it(name, function() {
                    run(wrapIt);
                });
            });
        });
    };

    runner(runnerDescribe, specs);

    // run specs
    mocha.checkLeaks();
    mocha.run();
};


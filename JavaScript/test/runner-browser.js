var runner = require('./runner');

window.onload = function () {
    mocha.setup('bdd');

    // wrap expepect.js describe/it API with the same API as AVA
    // https://github.com/Automattic/expect.js/#api
    var wrapIt = {
        is: function (actual, expected, message) {
            expect(actual).to.eql(expected, message);
        },
        deepEqual: function (actual, expected, message) {
            expect(actual).to.eql(expected, message);
        }
    };

    var runnerDescribe = function (name, run) {
        describe(name, function () {
            run(function (name, run) {
                it(name, function () {
                    run(wrapIt);
                });
            });
        });
    };

    // register suites
    runner(runnerDescribe, specs);

    // run specs
    mocha.setup({ timeout: 5000 });
    mocha.run();
};


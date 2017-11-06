var runner = require('./runner');

window.onload = function () {
    mocha.setup('bdd');

    // wrap expepect.js describe/it API with the same API as AVA
    // https://github.com/Automattic/expect.js/#api
    var itWrap = function (name, run) {
        it(name, function () {
            run({
                is: function (actual, expected, message) {
                    expect(actual).to.eql(expected, message);
                },
                deepEqual: function (actual, expected, message) {
                    expect(actual).to.eql(expected, message);
                }
            });
        });
    }

    itWrap.skip = function(caseName, noFun) {
        console.log(`Skipping '${caseName}'`);
    };

    var runnerDescribe = function (name, run) {
        describe(name, function () {
            run(itWrap);
        });
    };

    // register suites
    runner(runnerDescribe, specs);

    // run specs
    mocha.setup({ timeout: 5000 });
    mocha.run();
};


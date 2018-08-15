// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const chaiAsPromised = require('chai-as-promised');

const https = require('https');
const querystring = require('querystring');

const { TimexProperty, resolver, creator } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;

chai.use(chaiAsPromised);
chai.should();

const callLUIS = function (utterance) {
    return new Promise(function(resolve, reject) {

        const currentDate = new Date();
        const timezoneOffset = currentDate.getTimezoneOffset() / 60;

        const args = {
            'subscription-key': 'SUBSCRIPTION-KEY',
            verbose: true,
            timezoneOffset: timezoneOffset,
            q: utterance
        };

        // this is a LUIS model that recognizes a single datetimeV2 entity
        const appId = 'APPLICATION-KEY';
        let url = 'https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/';
        url += appId;
        url += '?';
        url += querystring.stringify(args);

        const removeDuplicates = function (array) {
            var seen = new Set();
            return array.filter(item => { return seen.has(item) ? false : seen.add(item); });
        };

        https.get(url, (res) => {
            if (res.statusCode !== 200) {
                reject(`http status code ${res.statusCode}`);
            }
            else {
                res.setEncoding('utf8');
                let rawData = '';
                res.on('data', (chunk) => { rawData += chunk; });
                res.on('end', () => {
                    try {
                        const parsedData = JSON.parse(rawData);
                        // we only care about the distinct TIMEX fields in the resolution
                        // ...from them we can recreate everything else
                        const result = [];
                        for (const entity of parsedData.entities) {
                            result.push({
                                entity: entity.entity,
                                type: entity.type,
                                timex: removeDuplicates(entity.resolution.values.map((v) => v.timex))
                            });
                        }
                        resolve(result);
                    } catch (e) {
                        reject(e.message);
                    }
                });
            }
        });
    });
};

describe('With LUIS', function () {
    this.timeout(10000);
    afterEach(function (done) { setTimeout(done, 300); });
    describe('Datatypes', function () {
        describe.skip('Timex with LUIS', function () {
            // note remember to return the Promise or alternatively use chai's .notify(done)
            it('Calling LUIS and getting back a Timex (verify against next week)', function () {
                const today = new Date();
                // first ask LUIS to understand this...
                return callLUIS('Wednesday 4 OClock')
                    .then(function(result) {
                        const entity = result[0];
                        // secondly that was ambiguous (in both date and time aspects) so we will resolve further...
                        // - here we are using a single date constraint and a single time constraint
                        const constraints = [ creator.nextWeek(today), creator.evening ];
                        const resolutions = resolver.evaluate(entity.timex, constraints);
                        // in this particular case we expected a single solution with these constraints
                        resolutions.should.be.an('array').lengthOf(1);
                        // now we have a resolution we can pretty-print it to natural language 
                        const text = resolutions[0].toNaturalLanguage(today);
                        text.should.equal('next Wednesday 4PM');
                        // and why not have LUIS take a crack at understand that...
                        return callLUIS(text);
                    })
                    .then(function(result) {
                        // it should just return a TIMEX version of that concrete solution...
                        const entity = result[0];
                        const timex = new TimexProperty(entity.timex[0]);
                        if (timex.types.has('datetime')) {
                            // which we can convert back into natural language again...
                            const text = timex.toNaturalLanguage(today);
                            return (text.toUpperCase() === entity.entity.toUpperCase());
                        }
                        return Promise.resolve(false);
                    })
                    .should.eventually.equal(true);
            });
            it('Calling LUIS and getting back a Timex (verify against this week)', function () {
                const today = new Date();
                // first ask LUIS to understand this...
                return callLUIS('Wednesday 4 OClock')
                    .then(function(result) {
                        const entity = result[0];
                        // secondly that was ambiguous (in both date and time aspects) so we will resolve further...
                        // - here we are using a single date constraint and a single time constraint
                        const constraints = [ creator.thisWeek(today), creator.evening ];
                        const resolutions = resolver.evaluate(entity.timex, constraints);
                        // in this particular case we expected a single solution with these constraints
                        resolutions.should.be.an('array').lengthOf(1);
                        // now we have a resolution we can pretty-print it to natural language 
                        const text = resolutions[0].toNaturalLanguage(today);
                        switch (today.getDay()) {
                            case 2:
                                text.should.equal('tomorrow 4PM');
                                break;
                            case 3:
                                text.should.equal('today 4PM');
                                break;
                            case 4:
                                text.should.equal('yesterday 4PM');
                                break;
                            default:
                                text.should.equal('this Wednesday 4PM');
                                break;
                        }
                    })
                    .should.eventually.be.fulfilled;
            });
        });
    });
});

// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const { TimexProperty } = require('../index.js');
const resolver = require('../src/timexResolver.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex resolution', () => {
            describe('date', () => {
                it('definite', () => {
                    const today = new Date(2017, 8, 26, 15, 30, 0);
                    const resolution = resolver.resolve(['2017-09-28'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '2017-09-28');
                    resolution.values[0].should.have.property('type', 'date');
                    resolution.values[0].should.have.property('value', '2017-09-28');
                });
                it('Saturday', () => {
                    const today = new Date(2017, 8, 28, 15, 30, 0);
                    const resolution = resolver.resolve(['XXXX-WXX-6'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(2);
                    resolution.values[0].should.have.property('timex', 'XXXX-WXX-6');
                    resolution.values[0].should.have.property('type', 'date');
                    resolution.values[0].should.have.property('value', '2017-09-23');
                    resolution.values[1].should.have.property('timex', 'XXXX-WXX-6');
                    resolution.values[1].should.have.property('type', 'date');
                    resolution.values[1].should.have.property('value', '2017-09-30');
                });
            });
            describe('datetime', () => {
                it('Wednesday 4 Oclock', () => {
                    const today = new Date(2017, 8, 28, 15, 30, 0);
                    const resolution = resolver.resolve(['XXXX-WXX-3T04', 'XXXX-WXX-3T16'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(4);
                    resolution.values[0].should.have.property('timex', 'XXXX-WXX-3T04');
                    resolution.values[0].should.have.property('type', 'datetime');
                    resolution.values[0].should.have.property('value', '2017-09-27 04:00:00');
                    resolution.values[1].should.have.property('timex', 'XXXX-WXX-3T04');
                    resolution.values[1].should.have.property('type', 'datetime');
                    resolution.values[1].should.have.property('value', '2017-10-04 04:00:00');
                    resolution.values[2].should.have.property('timex', 'XXXX-WXX-3T16');
                    resolution.values[2].should.have.property('type', 'datetime');
                    resolution.values[2].should.have.property('value', '2017-09-27 16:00:00');
                    resolution.values[3].should.have.property('timex', 'XXXX-WXX-3T16');
                    resolution.values[3].should.have.property('type', 'datetime');
                    resolution.values[3].should.have.property('value', '2017-10-04 16:00:00');
                });
                it('Wednesday 4pm', () => {
                    const today = new Date(2017, 8, 28, 15, 30, 0);
                    const resolution = resolver.resolve(['XXXX-WXX-3T04'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(2);
                    resolution.values[0].should.have.property('timex', 'XXXX-WXX-3T04');
                    resolution.values[0].should.have.property('type', 'datetime');
                    resolution.values[0].should.have.property('value', '2017-09-27 04:00:00');
                    resolution.values[1].should.have.property('timex', 'XXXX-WXX-3T04');
                    resolution.values[1].should.have.property('type', 'datetime');
                    resolution.values[1].should.have.property('value', '2017-10-04 04:00:00');
                });
                it('next Wednesday 4am', () => {
                    const today = new Date(2017, 9, 7);
                    const resolution = resolver.resolve(['2017-10-11T04'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '2017-10-11T04');
                    resolution.values[0].should.have.property('type', 'datetime');
                    resolution.values[0].should.have.property('value', '2017-10-11 04:00:00');
                });
            });
            describe('duration', () => {
                it('2 years', () => {
                    const resolution = resolver.resolve(['P2Y']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'P2Y');
                    resolution.values[0].should.have.property('type', 'duration');
                    resolution.values[0].should.have.property('value', '63072000');
                });
                it('6 months', () => {
                    const resolution = resolver.resolve(['P6M']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'P6M');
                    resolution.values[0].should.have.property('type', 'duration');
                    resolution.values[0].should.have.property('value', '15552000');
                });
                it('3 weeks', () => {
                    const resolution = resolver.resolve(['P3W']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'P3W');
                    resolution.values[0].should.have.property('type', 'duration');
                    resolution.values[0].should.have.property('value', '1814400');
                });
                it('5 days', () => {
                    const resolution = resolver.resolve(['P5D']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'P5D');
                    resolution.values[0].should.have.property('type', 'duration');
                    resolution.values[0].should.have.property('value', '432000');
                });
                it('8 hours', () => {
                    const resolution = resolver.resolve(['PT8H']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'PT8H');
                    resolution.values[0].should.have.property('type', 'duration');
                    resolution.values[0].should.have.property('value', '28800');
                });
                it('15 minutes', () => {
                    const resolution = resolver.resolve(['PT15M']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'PT15M');
                    resolution.values[0].should.have.property('type', 'duration');
                    resolution.values[0].should.have.property('value', '900');
                });
                it('10 seconds', () => {
                    const resolution = resolver.resolve(['PT10S']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'PT10S');
                    resolution.values[0].should.have.property('type', 'duration');
                    resolution.values[0].should.have.property('value', '10');
                });
            });
            describe('daterange', () => {
                it('September', () => {
                    const today = new Date(2017, 8, 28);
                    const resolution = resolver.resolve(['XXXX-09'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(2);
                    resolution.values[0].should.have.property('timex', 'XXXX-09');
                    resolution.values[0].should.have.property('type', 'daterange');
                    resolution.values[0].should.have.property('start', '2016-09-01');
                    resolution.values[0].should.have.property('end', '2016-10-01');
                    resolution.values[1].should.have.property('timex', 'XXXX-09');
                    resolution.values[1].should.have.property('type', 'daterange');
                    resolution.values[1].should.have.property('start', '2017-09-01');
                    resolution.values[1].should.have.property('end', '2017-10-01');
                });
                it('Winter', () => {
                    const today = new Date(2017, 8, 28);
                    const resolution = resolver.resolve(['WI'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'WI');
                    resolution.values[0].should.have.property('type', 'daterange');
                    resolution.values[0].should.have.property('value', 'not resolved');
                });
                it('Last Week', () => {
                    const today = new Date(2019, 3, 30);
                    const resolution = resolver.resolve(['2019-W17'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '2019-W17');
                    resolution.values[0].should.have.property('type', 'daterange');
                    resolution.values[0].should.have.property('start', '2019-04-22');
                    resolution.values[0].should.have.property('end', '2019-04-29');
                });
                it('Last Month', () => {
                    const today = new Date(2019, 3, 30);
                    const resolution = resolver.resolve(['2019-03'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '2019-03');
                    resolution.values[0].should.have.property('type', 'daterange');
                    resolution.values[0].should.have.property('start', '2019-03-01');
                    resolution.values[0].should.have.property('end', '2019-04-01');
                });
                it('Last Year', () => {
                    const today = new Date(2019, 3, 30);
                    const resolution = resolver.resolve(['2018'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '2018');
                    resolution.values[0].should.have.property('type', 'daterange');
                    resolution.values[0].should.have.property('start', '2018-01-01');
                    resolution.values[0].should.have.property('end', '2019-01-01');
                });
                it('Last Three Weeks', () => {
                    const today = new Date(2019, 3, 30);
                    const resolution = resolver.resolve(['(2019-04-10,2019-05-01,P3W)'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '(2019-04-10,2019-05-01,P3W)');
                    resolution.values[0].should.have.property('type', 'daterange');
                    resolution.values[0].should.have.property('start', '2019-04-10');
                    resolution.values[0].should.have.property('end', '2019-05-01');
                });
            });
            describe('timerange', () => {
                it('4am to 8pm', () => {
                    const today = new Date();
                    const resolution = resolver.resolve(['(T04,T20,PT16H)'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '(T04,T20,PT16H)');
                    resolution.values[0].should.have.property('type', 'timerange');
                    resolution.values[0].should.have.property('start', '04:00:00');
                    resolution.values[0].should.have.property('end', '20:00:00');
                });
                it('morning', () => {
                    const today = new Date();
                    const resolution = resolver.resolve(['TMO'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'TMO');
                    resolution.values[0].should.have.property('type', 'timerange');
                    resolution.values[0].should.have.property('start', '08:00:00');
                    resolution.values[0].should.have.property('end', '12:00:00');
                });
                it('afternoon', () => {
                    const today = new Date();
                    const resolution = resolver.resolve(['TAF'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'TAF');
                    resolution.values[0].should.have.property('type', 'timerange');
                    resolution.values[0].should.have.property('start', '12:00:00');
                    resolution.values[0].should.have.property('end', '16:00:00');
                });
                it('evening', () => {
                    const today = new Date();
                    const resolution = resolver.resolve(['TEV'], today);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'TEV');
                    resolution.values[0].should.have.property('type', 'timerange');
                    resolution.values[0].should.have.property('start', '16:00:00');
                    resolution.values[0].should.have.property('end', '20:00:00');
                });
            });
            describe('datetimerange', () => {
                it('this morning', () => {
                    const resolution = resolver.resolve(['2017-10-07TMO']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '2017-10-07TMO');
                    resolution.values[0].should.have.property('type', 'datetimerange');
                    resolution.values[0].should.have.property('start', '2017-10-07 08:00:00');
                    resolution.values[0].should.have.property('end', '2017-10-07 12:00:00');
                });
                it('tonight', () => {
                    const resolution = resolver.resolve(['2018-03-18TNI']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '2018-03-18TNI');
                    resolution.values[0].should.have.property('type', 'datetimerange');
                    resolution.values[0].should.have.property('start', '2018-03-18 20:00:00');
                    resolution.values[0].should.have.property('end', '2018-03-18 24:00:00');
                });
                it('next monday 4am to next thursday 3pm', () => {
                    const resolution = resolver.resolve(['(2017-10-09T04,2017-10-12T15,PT83H)']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', '(2017-10-09T04,2017-10-12T15,PT83H)');
                    resolution.values[0].should.have.property('type', 'datetimerange');
                    resolution.values[0].should.have.property('start', '2017-10-09 04:00:00');
                    resolution.values[0].should.have.property('end', '2017-10-12 15:00:00');
                });
            });
            describe('time', () => {
                it('4am', () => {
                    const resolution = resolver.resolve(['T04']);
                    resolution.should.have.property('values').that.is.an('array').of.length(1);
                    resolution.values[0].should.have.property('timex', 'T04');
                    resolution.values[0].should.have.property('type', 'time');
                    resolution.values[0].should.have.property('value', '04:00:00');
                });
                it('4 oclock', () => {
                    const resolution = resolver.resolve(['T04', 'T16']);
                    resolution.should.have.property('values').that.is.an('array').of.length(2);
                    resolution.values[0].should.have.property('timex', 'T04');
                    resolution.values[0].should.have.property('type', 'time');
                    resolution.values[0].should.have.property('value', '04:00:00');
                    resolution.values[1].should.have.property('timex', 'T16');
                    resolution.values[1].should.have.property('type', 'time');
                    resolution.values[1].should.have.property('value', '16:00:00');
                });
            });
        });
    });
});

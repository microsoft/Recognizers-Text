// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');

const { TimexProperty, TimexSet } = require('../index.js');
const timexRelativeConvert = require('../src/timexRelativeConvert.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex relative convert to string', () => {
            describe('date', () => {
                describe('one day or less separation', () => {
                    it('today', () => {
                        const timex = new TimexProperty('2017-09-25');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('today');
                    });
                    it('tomorrow', () => {
                        const timex = new TimexProperty('2017-09-23');
                        const today = new Date(2017, 8, 22);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('tomorrow');
                    });
                    it('tomorrow cross year/month boundary', () => {
                        const timex = new TimexProperty('2018-01-01');
                        const today = new Date(2017, 11, 31);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('tomorrow');
                    });
                    it('yesterday', () => {
                        const timex = new TimexProperty('2017-09-21');
                        const today = new Date(2017, 8, 22);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('yesterday');
                    });
                    it('yesterday cross year/month boundary', () => {
                        const timex = new TimexProperty('2017-12-31');
                        const today = new Date(2018, 0, 1);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('yesterday');
                    });
                });
                describe('one week separation', () => {
                    it('this week', () => {
                        const timex = new TimexProperty('2017-10-18');
                        const today = new Date(2017, 9, 16);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this Wednesday');
                    });
                    it('this week cross year/month boundary', () => {
                        const timex = new TimexProperty('2017-11-03');
                        const today = new Date(2017, 9, 31);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this Friday');
                    });
                    it('next week', () => {
                        const timex = new TimexProperty('2017-09-27');
                        const today = new Date(2017, 8, 22);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next Wednesday');
                    });
                    it('next week cross year/month boundary', () => {
                        const timex = new TimexProperty('2018-01-05');
                        const today = new Date(2017, 11, 28);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next Friday');
                    });
                    it('last week', () => {
                        const timex = new TimexProperty('2017-09-14');
                        const today = new Date(2017, 8, 22);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('last Thursday');
                    });
                    it('last week cross year/month boundary', () => {
                        const timex = new TimexProperty('2017-12-25');
                        const today = new Date(2018, 0, 4);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('last Monday');
                    });
                });
            });
            describe('more than one week separation', () => {
                it('this week', () => {
                    const timex = new TimexProperty('2017-10-25');
                    const today = new Date(2017, 9, 9);
                    timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('25th October 2017');
                });
                it('next week', () => {
                    const timex = new TimexProperty('2017-10-04');
                    const today = new Date(2017, 8, 22);
                    timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('4th October 2017');
                });
                it('last week', () => {
                    const timex = new TimexProperty('2017-09-07');
                    const today = new Date(2017, 8, 22);
                    timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('7th September 2017');
                });
            });
            describe('datetime', () => {
                it('today', () => {
                    const timex = new TimexProperty('2017-09-25T16:00:00');
                    const today = new Date(2017, 8, 25);
                    timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('today 4PM');
                });
                it('tomorrow', () => {
                    const timex = new TimexProperty('2017-09-23T16:00:00');
                    const today = new Date(2017, 8, 22);
                    timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('tomorrow 4PM');
                });
                it('yesterday', () => {
                    const timex = new TimexProperty('2017-09-21T16:00:00');
                    const today = new Date(2017, 8, 22);
                    timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('yesterday 4PM');
                });
            });
            describe('daterange', () => {
                describe('week', () => {
                    it('this week', () => {
                        const timex = new TimexProperty('2017-W40');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this week');
                    });
                    it('next week', () => {
                        const timex = new TimexProperty('2017-W41');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next week');
                    });
                    it('last week', () => {
                        const timex = new TimexProperty('2017-W39');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('last week');
                    });
                    it('this week', () => {
                        const timex = new TimexProperty('2017-W41');
                        const today = new Date(2017, 9, 4);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this week');
                    });
                    it('next week', () => {
                        const timex = new TimexProperty('2017-W42');
                        const today = new Date(2017, 9, 4);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next week');
                    });
                    it('last week', () => {
                        const timex = new TimexProperty('2017-W40');
                        const today = new Date(2017, 9, 4);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('last week');
                    });
                });
                describe('weekend', () => {
                    it('this weekend', () => {
                        const timex = new TimexProperty('2017-W40-WE');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this weekend');
                    });
                    it('next weekend', () => {
                        const timex = new TimexProperty('2017-W41-WE');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next weekend');
                    });
                    it('last weekend', () => {
                        const timex = new TimexProperty('2017-W39-WE');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('last weekend');
                    });
                });
                describe('month', () => {
                    it('this month', () => {
                        const timex = new TimexProperty('2017-09');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this month');
                    });
                    it('next month', () => {
                        const timex = new TimexProperty('2017-10');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next month');
                    });
                    it('last month', () => {
                        const timex = new TimexProperty('2017-08');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('last month');
                    });
                });
                describe('year', () => {
                    it('this year', () => {
                        const timex = new TimexProperty('2017');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this year');
                    });
                    it('next year', () => {
                        const timex = new TimexProperty('2018');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next year');
                    });
                    it('last year', () => {
                        const timex = new TimexProperty('2016');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('last year');
                    });
                });
                describe('season', () => {
                    it('this summer', () => {
                        const timex = new TimexProperty('2017-SU');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this summer');
                    });
                    it('next summer', () => {
                        const timex = new TimexProperty('2018-SU');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next summer');
                    });
                    it('last summer', () => {
                        const timex = new TimexProperty('2016-SU');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('last summer');
                    });
                });
            });
            describe('datetimerange', () => {
                describe('partOfDay', () => {
                    it('this evening', () => {
                        const timex = new TimexProperty('2017-09-25TEV');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('this evening');
                    });
                    it('tonight', () => {
                        const timex = new TimexProperty('2017-09-25TNI');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('tonight');
                    });
                    it('tomorrow morning', () => {
                        const timex = new TimexProperty('2017-09-26TMO');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('tomorrow morning');
                    });
                    it('yesterday afternoon', () => {
                        const timex = new TimexProperty('2017-09-24TAF');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('yesterday afternoon');
                    });
                    it('next wednesday evening', () => {
                        const timex = new TimexProperty('2017-10-04TEV');
                        const today = new Date(2017, 8, 25);
                        timexRelativeConvert.convertTimexToStringRelative(timex, today).should.equal('next Wednesday evening');
                    });
                });
            });
        });
    });
});

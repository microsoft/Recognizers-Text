// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');

const { Timex, TimexSet } = require('../index.js');
const timexconvert = require('../src/timexConvert.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex convert to string', () => {
            describe('date', () => {
                it('complete date', () => {
                    const timex = new Timex('2017-05-29');
                    timexconvert.convertTimexToString(timex).should.equal('29th May 2017');
                });
                it('month and dayOfMonth', () => {
                    timexconvert.convertTimexToString(new Timex('XXXX-01-05')).should.equal('5th January');
                    timexconvert.convertTimexToString(new Timex('XXXX-02-05')).should.equal('5th Februrary');
                    timexconvert.convertTimexToString(new Timex('XXXX-03-05')).should.equal('5th March');
                    timexconvert.convertTimexToString(new Timex('XXXX-04-05')).should.equal('5th April');
                    timexconvert.convertTimexToString(new Timex('XXXX-05-05')).should.equal('5th May');
                    timexconvert.convertTimexToString(new Timex('XXXX-06-05')).should.equal('5th June');
                    timexconvert.convertTimexToString(new Timex('XXXX-07-05')).should.equal('5th July');
                    timexconvert.convertTimexToString(new Timex('XXXX-08-05')).should.equal('5th August');
                    timexconvert.convertTimexToString(new Timex('XXXX-09-05')).should.equal('5th September');
                    timexconvert.convertTimexToString(new Timex('XXXX-10-05')).should.equal('5th October');
                    timexconvert.convertTimexToString(new Timex('XXXX-11-05')).should.equal('5th November');
                    timexconvert.convertTimexToString(new Timex('XXXX-12-05')).should.equal('5th December');
                });
                it('month and dayOfMonth with correct abbreviation', () => {
                    timexconvert.convertTimexToString(new Timex('XXXX-06-01')).should.equal('1st June');
                    timexconvert.convertTimexToString(new Timex('XXXX-06-02')).should.equal('2nd June');
                    timexconvert.convertTimexToString(new Timex('XXXX-06-03')).should.equal('3rd June');
                    timexconvert.convertTimexToString(new Timex('XXXX-06-04')).should.equal('4th June');
                });
                it('dayOfWeek', () => {
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-1')).should.equal('Monday');
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-2')).should.equal('Tuesday');
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-3')).should.equal('Wednesday');
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-4')).should.equal('Thursday');
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-5')).should.equal('Friday');
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-6')).should.equal('Saturday');
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-7')).should.equal('Sunday');
                });
            });
            describe('time', () => {
                it('hours, minutes and seconds', () => {
                    timexconvert.convertTimexToString(new Timex('T17:30:05')).should.equal('5:30:05PM');
                    timexconvert.convertTimexToString(new Timex('T02:30:30')).should.equal('2:30:30AM');
                    timexconvert.convertTimexToString(new Timex('T00:30:30')).should.equal('12:30:30AM');
                    timexconvert.convertTimexToString(new Timex('T12:30:30')).should.equal('12:30:30PM');
                });
                it('hours and minutes', () => {
                    timexconvert.convertTimexToString(new Timex('T17:30')).should.equal('5:30PM');
                    timexconvert.convertTimexToString(new Timex('T17:00')).should.equal('5PM');
                    timexconvert.convertTimexToString(new Timex('T01:30')).should.equal('1:30AM');
                    timexconvert.convertTimexToString(new Timex('T01:00')).should.equal('1AM');
                });
                it('hours', () => {
                    timexconvert.convertTimexToString(new Timex('T00')).should.equal('midnight');
                    timexconvert.convertTimexToString(new Timex('T01')).should.equal('1AM');
                    timexconvert.convertTimexToString(new Timex('T02')).should.equal('2AM');
                    timexconvert.convertTimexToString(new Timex('T03')).should.equal('3AM');
                    timexconvert.convertTimexToString(new Timex('T04')).should.equal('4AM');
                    timexconvert.convertTimexToString(new Timex('T12')).should.equal('midday');
                    timexconvert.convertTimexToString(new Timex('T13')).should.equal('1PM');
                    timexconvert.convertTimexToString(new Timex('T14')).should.equal('2PM');
                    timexconvert.convertTimexToString(new Timex('T23')).should.equal('11PM');
                });
            });
            describe('now', () => {
                it('now', () => {
                    timexconvert.convertTimexToString(new Timex('PRESENT_REF')).should.equal('now');
                });
            });
            describe('datetime', () => {
                it('full datetime', () => {
                    timexconvert.convertTimexToString(new Timex('1984-01-03T18:30:45')).should.equal('6:30:45PM 3rd January 1984');
                    timexconvert.convertTimexToString(new Timex('2000-01-01T00')).should.equal('midnight 1st January 2000');
                    timexconvert.convertTimexToString(new Timex('1967-05-29T19:30:00')).should.equal('7:30PM 29th May 1967');
                });
                it('paricular time on particular day of week', () => {
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-3T16')).should.equal('4PM Wednesday');
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-5T18:30')).should.equal('6:30PM Friday');
                });
            });
            describe('daterange', () => {
                it('year', () => {
                    timexconvert.convertTimexToString(new Timex('2016')).should.equal('2016');
                });
                it('year season (e.g. "summer of 1999")', () => {
                    timexconvert.convertTimexToString(new Timex('1999-SU')).should.equal('summer 1999');
                });
                it('season', () => {
                    timexconvert.convertTimexToString(new Timex('SU')).should.equal('summer');
                    timexconvert.convertTimexToString(new Timex('WI')).should.equal('winter');
                });
                it('month', () => {
                    timexconvert.convertTimexToString(new Timex('XXXX-01')).should.equal('January');
                    timexconvert.convertTimexToString(new Timex('XXXX-05')).should.equal('May');
                    timexconvert.convertTimexToString(new Timex('XXXX-12')).should.equal('December');
                });
                it('month and year', () => {
                    timexconvert.convertTimexToString(new Timex('2018-05')).should.equal('May 2018');
                });
                it('week of month', () => {
                    timexconvert.convertTimexToString(new Timex('XXXX-01-W01')).should.equal('first week of January');
                    timexconvert.convertTimexToString(new Timex('XXXX-08-W03')).should.equal('third week of August');
                });
            });
            describe('TimeRange', () => {
                it('part of the day (daytime, night, morning etc.)', () => {
                    timexconvert.convertTimexToString(new Timex('TDT')).should.equal('daytime');
                    timexconvert.convertTimexToString(new Timex('TNI')).should.equal('night');
                    timexconvert.convertTimexToString(new Timex('TMO')).should.equal('morning');
                    timexconvert.convertTimexToString(new Timex('TAF')).should.equal('afternoon');
                    timexconvert.convertTimexToString(new Timex('TEV')).should.equal('evening');
                });
            });
            describe.skip('DateTimeRange', () => {
                it('friday evening', () => {
                    timexconvert.convertTimexToString(new Timex('XXXX-WXX-5TEV')).should.equal('Friday evening');
                });
                it('date and part of the day (morning, evening etc.)', () => {
                    timexconvert.convertTimexToString(new Timex('2017-09-07TNI')).should.equal('7th September 2017 night');
                });
                it('last 5 minutes', () => {
                    // date + time + duration
                    const timex = new Timex('(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)');
                    // TODO
                });
                it('wednesday to saturday', () => {
                    // date + duration
                    const timex = new Timex('(XXXX-WXX-3,XXXX-WXX-6,P3D)');
                    // TODO
                });
            });
            describe('Duration', () => {
                it('years', () => {
                    timexconvert.convertTimexToString(new Timex('P2Y')).should.equal('2 years');
                    timexconvert.convertTimexToString(new Timex('P1Y')).should.equal('1 year');
                });
                it('months', () => {
                    timexconvert.convertTimexToString(new Timex('P4M')).should.equal('4 months');
                    timexconvert.convertTimexToString(new Timex('P1M')).should.equal('1 month');
                    timexconvert.convertTimexToString(new Timex('P0M')).should.equal('0 months');
                });
                it('weeks', () => {
                    timexconvert.convertTimexToString(new Timex('P6W')).should.equal('6 weeks');
                    timexconvert.convertTimexToString(new Timex('P9.5W')).should.equal('9.5 weeks');
                });
                it('days', () => {
                    timexconvert.convertTimexToString(new Timex('P5D')).should.equal('5 days');
                    timexconvert.convertTimexToString(new Timex('P1D')).should.equal('1 day');
                });
                it('hours', () => {
                    timexconvert.convertTimexToString(new Timex('PT5H')).should.equal('5 hours');
                    timexconvert.convertTimexToString(new Timex('PT1H')).should.equal('1 hour');
                });
                it('minutes', () => {
                    timexconvert.convertTimexToString(new Timex('PT30M')).should.equal('30 minutes');
                    timexconvert.convertTimexToString(new Timex('PT1M')).should.equal('1 minute');
                });
                it('seconds', () => {
                    timexconvert.convertTimexToString(new Timex('PT45S')).should.equal('45 seconds');
                });
            });
            describe('Set', () => {
                it('every 2 days', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('P2D')).should.equal('every 2 days');
                });
                it('every week', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('P1W')).should.equal('every week');
                });
                it('every october', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('XXXX-10')).should.equal('every October');
                });
                it('every sunday', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('XXXX-WXX-7')).should.equal('every Sunday');
                });
                it('every day', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('P1D')).should.equal('every day');
                });
                it('every year', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('P1Y')).should.equal('every year');
                });
                it('every spring', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('SP')).should.equal('every spring');
                });
                it('each winter', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('WI')).should.equal('every winter');
                });
                it('every evening', () => {
                    timexconvert.convertTimexSetToString(new TimexSet('TEV')).should.equal('every evening');
                });
            });
        });
    });
});

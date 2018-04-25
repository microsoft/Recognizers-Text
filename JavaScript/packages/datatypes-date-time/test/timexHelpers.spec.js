// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const timexHelpers = require('../src/timexHelpers.js');
const { Time, TimexProperty } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex helpers', () => {
            describe('Timex expand to range of start and end Timex', () => {
                it('expandDateTimeRange (short range)', () => {
                    const timex = new TimexProperty('(2017-09-27,2017-09-29,P2D)');
                    const range = timexHelpers.expandDateTimeRange(timex);
                    (new TimexProperty(range.start)).timex.should.equal('2017-09-27');
                    (new TimexProperty(range.end)).timex.should.equal('2017-09-29');
                });
                it('expandDateTimeRange (long range)', () => {
                    const timex = new TimexProperty('(2006-01-01,2008-06-01,P882D)');
                    const range = timexHelpers.expandDateTimeRange(timex);
                    (new TimexProperty(range.start)).timex.should.equal('2006-01-01');
                    (new TimexProperty(range.end)).timex.should.equal('2008-06-01');
                });
                it('expandDateTimeRange (including time)', () => {
                    const timex = new TimexProperty('(2017-10-10T16:02:04,2017-10-10T16:07:04,PT5M)');
                    const range = timexHelpers.expandDateTimeRange(timex);
                    (new TimexProperty(range.start)).timex.should.equal('2017-10-10T16:02:04');
                    (new TimexProperty(range.end)).timex.should.equal('2017-10-10T16:07:04');
                });
                it('expandDateTimeRange (month)', () => {
                    const timex = new TimexProperty('2017-05');
                    const range = timexHelpers.expandDateTimeRange(timex);
                    (new TimexProperty(range.start)).timex.should.equal('2017-05-01');
                    (new TimexProperty(range.end)).timex.should.equal('2017-06-01');
                });
                it('expandDateTimeRange (year)', () => {
                    const timex = new TimexProperty('1999');
                    const range = timexHelpers.expandDateTimeRange(timex);
                    (new TimexProperty(range.start)).timex.should.equal('1999-01-01');
                    (new TimexProperty(range.end)).timex.should.equal('2000-01-01');
                });
                it('expandTimeRange', () => {
                    const timex = new TimexProperty('(T14,T16,PT2H)');
                    const range = timexHelpers.expandTimeRange(timex);
                    (new TimexProperty(range.start)).timex.should.equal('T14');
                    (new TimexProperty(range.end)).timex.should.equal('T16');
                });
            });
            describe('Timex expand to range of Date or Time objects', () => {
                it('dateRangeFromTimex', () => {
                    const timex = new TimexProperty('(2017-09-27,2017-09-29,P2D)');
                    const range = timexHelpers.dateRangeFromTimex(timex);
                    range.start.getTime().should.equal(new Date(2017,8,27).getTime());
                    range.end.getTime().should.equal(new Date(2017,8,29).getTime());
                });
                it('timeRangeFromTimex', () => {
                    const timex = new TimexProperty('(T14,T16,PT2H)');
                    const range = timexHelpers.timeRangeFromTimex(timex);
                    range.start.getTime().should.equal(new Time(14, 0, 0).getTime());
                    range.end.getTime().should.equal(new Time(16, 0, 0).getTime());
                });
            });
            describe('Timex convert to Date or Time objects', () => {
                it('dateFromTimex', () => {
                    const timex = new TimexProperty('2017-09-27');
                    const date = timexHelpers.dateFromTimex(timex);
                    date.getTime().should.equal(new Date(2017,8,27).getTime());
                });
                it('timeFromTimex', () => {
                    const timex = new TimexProperty('T00:05:00');
                    const time = timexHelpers.timeFromTimex(timex);
                    time.getTime().should.equal(new Time(0, 5, 0).getTime());
                });
            });
        });
    });
});

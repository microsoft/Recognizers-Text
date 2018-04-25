// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const timexFormat = require('../src/timexFormat.js');
const { TimexProperty } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex format', () => {
            describe('timexFormat helper function', () => {
                it('date', () => {
                    timexFormat.format({ year: 2017, month: 9, dayOfMonth: 27 }).should.equal('2017-09-27');
                    timexFormat.format({ dayOfWeek: 3 }).should.equal('XXXX-WXX-3');
                    timexFormat.format({ month: 12, dayOfMonth: 5 }).should.equal('XXXX-12-05');
                });
                it('time', () => {
                    timexFormat.format({ hour: 17, minute: 30, second: 45 }).should.equal('T17:30:45');
                    timexFormat.format({ hour: 5, minute: 6, second: 7 }).should.equal('T05:06:07');
                    timexFormat.format({ hour: 17, minute: 30, second: 0 }).should.equal('T17:30');
                    timexFormat.format({ hour: 23, minute: 0, second: 0 }).should.equal('T23');
                });
                it('duration', () => {
                    timexFormat.format({ years: 50 }).should.equal('P50Y');
                    timexFormat.format({ months: 6 }).should.equal('P6M');
                    timexFormat.format({ weeks: 3 }).should.equal('P3W');
                    timexFormat.format({ days: 5 }).should.equal('P5D');
                    timexFormat.format({ hours: 16 }).should.equal('PT16H');
                    timexFormat.format({ minutes: 32 }).should.equal('PT32M');
                    timexFormat.format({ seconds: 20 }).should.equal('PT20S');
                });
                it('present', () => {
                    timexFormat.format({ now: true }).should.equal('PRESENT_REF');
                });
                it('datetime', () => {
                    timexFormat.format({ dayOfWeek: 3, hour: 4, minute: 0, second: 0 }).should.equal('XXXX-WXX-3T04');
                    timexFormat.format({ year: 2017, month: 9, dayOfMonth: 27, hour: 11, minute: 41, second: 30 }).should.equal('2017-09-27T11:41:30');
                });
                it('daterange', () => {
                    timexFormat.format({ year: 2017 }).should.equal('2017');
                    timexFormat.format({ season: 'SU' }).should.equal('SU');
                    timexFormat.format({ year: 2017, season: 'WI' }).should.equal('2017-WI');
                    timexFormat.format({ year: 2017, month: 9 }).should.equal('2017-09');
                    timexFormat.format({ year: 2017, weekOfYear: 37 }).should.equal('2017-W37');
                    timexFormat.format({ year: 2017, weekOfYear: 37, weekend: true }).should.equal('2017-W37-WE');
                    timexFormat.format({ month: 5 }).should.equal('XXXX-05');
                });
                it('timerange', () => {
                    timexFormat.format({ partOfDay: 'EV' }).should.equal('TEV');
                });
                it('datetimerange', () => {
                    timexFormat.format({ year: 2017, month: 9, dayOfMonth: 27, partOfDay: 'EV' }).should.equal('2017-09-27TEV');
                });
            });
        });
    });
});

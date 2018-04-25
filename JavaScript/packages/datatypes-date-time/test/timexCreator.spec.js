// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const timexCreator = require('../src/timexCreator.js');
const timexFormat = require('../src/timexFormat.js');
const timexDateHelpers = require('../src/timexDateHelpers.js');
const { TimexProperty } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex creator', () => {
            describe('timex creator', () => {
                const today = new Date(2017, 9, 5);
                it('today', () => {
                    const d = new Date();
                    const expected = timexFormat.format({ year: d.getFullYear(), month: d.getMonth() + 1, dayOfMonth: d.getDate() });
                    timexCreator.today().should.equal(expected);
                });
                it('today(today)', () => {
                    timexCreator.today(today).should.equal('2017-10-05');
                });
                it('tomorrow', () => {
                    const d = new Date();
                    d.setDate(d.getDate() + 1);
                    const expected = timexFormat.format({ year: d.getFullYear(), month: d.getMonth() + 1, dayOfMonth: d.getDate() });
                    timexCreator.tomorrow().should.equal(expected);
                });
                it('tomorrow(today)', () => {
                    timexCreator.tomorrow(today).should.equal('2017-10-06');
                });
                it('yesterday', () => {
                    const d = new Date();
                    d.setDate(d.getDate() - 1);
                    const expected = timexFormat.format({ year: d.getFullYear(), month: d.getMonth() + 1, dayOfMonth: d.getDate() });
                    timexCreator.yesterday().should.equal(expected);
                });
                it('yesterday(today)', () => {
                    timexCreator.yesterday(today).should.equal('2017-10-04');
                });
                it('weekFromToday', () => {
                    const d = new Date();
                    const expected = timexFormat.format({ year: d.getFullYear(), month: d.getMonth() + 1, dayOfMonth: d.getDate(), days: 7});
                    timexCreator.weekFromToday().should.equal(expected);
                });
                it('weekFromToday(today)', () => {
                    timexCreator.weekFromToday(today).should.equal('(2017-10-05,2017-10-12,P7D)');
                });
                it('weekBackFromToday', () => {
                    const d = new Date();
                    d.setDate(d.getDate() - 7);
                    const expected = timexFormat.format({ year: d.getFullYear(), month: d.getMonth() + 1, dayOfMonth: d.getDate(), days: 7});
                    timexCreator.weekBackFromToday().should.equal(expected);
                });
                it('weekBackFromToday(today)', () => {
                    timexCreator.weekBackFromToday(today).should.equal('(2017-09-28,2017-10-05,P7D)');
                });
                it('nextWeek', () => {
                    const start = timexDateHelpers.dateOfNextDay(1, new Date());
                    const expected = timexFormat.format(Object.assign(TimexProperty.fromDate(start), { days: 7 }));
                    timexCreator.nextWeek().should.equal(expected);
                });
                it('nextWeek (today)', () => {
                    timexCreator.nextWeek(today).should.equal('(2017-10-09,2017-10-16,P7D)');
                });
                it('lastWeek', () => {
                    const start = timexDateHelpers.dateOfLastDay(1, new Date());
                    start.setDate(start.getDate() - 7);
                    const expected = timexFormat.format(Object.assign(TimexProperty.fromDate(start), { days: 7 }));
                    timexCreator.lastWeek().should.equal(expected);
                });
                it('lastWeek (today)', () => {
                    timexCreator.lastWeek(today).should.equal('(2017-09-25,2017-10-02,P7D)');
                });
                it('nextWeeksFromToday', () => {
                    const d = new Date();
                    const expected = timexFormat.format({ year: d.getFullYear(), month: d.getMonth() + 1, dayOfMonth: d.getDate(), days: 14});
                    timexCreator.nextWeeksFromToday(2).should.equal(expected);
                });
                it('nextWeeksFromToday (today)', () => {
                    timexCreator.nextWeeksFromToday(2, today).should.equal('(2017-10-05,2017-10-19,P14D)');
                });
            });
        });
    });
});


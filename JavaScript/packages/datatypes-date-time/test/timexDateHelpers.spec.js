// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const timexDateHelpers = require('../src/timexDateHelpers.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex date helpers', () => {
            it('tomorrow', () => {
                timexDateHelpers.tomorrow(new Date(2016, 11, 31)).getTime().should.equal((new Date(2017, 0, 1)).getTime());
                timexDateHelpers.tomorrow(new Date(2017, 0, 1)).getTime().should.equal((new Date(2017, 0, 2)).getTime());
                timexDateHelpers.tomorrow(new Date(2017, 1, 28)).getTime().should.equal((new Date(2017, 2, 1)).getTime());
                timexDateHelpers.tomorrow(new Date(2016, 1, 28)).getTime().should.equal((new Date(2016, 1, 29)).getTime());
            });
            it('yesterday', () => {
                timexDateHelpers.yesterday(new Date(2017, 0, 1)).getTime().should.equal((new Date(2016, 11, 31)).getTime());
                timexDateHelpers.yesterday(new Date(2017, 0, 2)).getTime().should.equal((new Date(2017, 0, 1)).getTime());
                timexDateHelpers.yesterday(new Date(2017, 2, 1)).getTime().should.equal((new Date(2017, 1, 28)).getTime());
                timexDateHelpers.yesterday(new Date(2016, 1, 29)).getTime().should.equal((new Date(2016, 1, 28)).getTime());
            });
            it('datePartEquals', () => {
                timexDateHelpers.datePartEquals(new Date(2017, 4, 29), new Date(2017, 4, 29)).should.be.true;
                timexDateHelpers.datePartEquals(new Date(2017, 4, 29, 19, 30, 0), new Date(2017, 4, 29)).should.be.true;
                timexDateHelpers.datePartEquals(new Date(2017, 4, 29), new Date(2017, 10, 15)).should.be.false;
            });
            it('isNextWeek', () => {
                const today = new Date(2017, 8, 25);
                timexDateHelpers.isNextWeek(new Date(2017, 9, 4), today).should.be.true;
                timexDateHelpers.isNextWeek(new Date(2017, 8, 27), today).should.be.false;
                timexDateHelpers.isNextWeek(today, today).should.be.false;
            });
            it('isLastWeek', () => {
                const today = new Date(2017, 8, 25);
                timexDateHelpers.isLastWeek(new Date(2017, 8, 20), today).should.be.true;
                timexDateHelpers.isLastWeek(new Date(2017, 8, 4), today).should.be.false;
                timexDateHelpers.isLastWeek(today, today).should.be.false;
            });
            it('weekOfyear', () => {
                timexDateHelpers.weekOfYear(new Date(2017, 0, 1)).should.equal(1);
                timexDateHelpers.weekOfYear(new Date(2017, 0, 2)).should.equal(2);
                timexDateHelpers.weekOfYear(new Date(2017, 1, 23)).should.equal(9);
                timexDateHelpers.weekOfYear(new Date(2017, 2, 15)).should.equal(12);
                timexDateHelpers.weekOfYear(new Date(2017, 8, 25)).should.equal(40);
                timexDateHelpers.weekOfYear(new Date(2017, 11, 31)).should.equal(53);
                timexDateHelpers.weekOfYear(new Date(2018, 0, 1)).should.equal(1);
                timexDateHelpers.weekOfYear(new Date(2018, 0, 2)).should.equal(1);
                timexDateHelpers.weekOfYear(new Date(2018, 0, 7)).should.equal(1);
                timexDateHelpers.weekOfYear(new Date(2018, 0, 8)).should.equal(2);
            });
            it('invariance', () => {
                const d = new Date(2017, 8, 25);
                const before = d.getTime();
                timexDateHelpers.tomorrow(d);
                timexDateHelpers.yesterday(d);
                timexDateHelpers.datePartEquals(new Date(), d);
                timexDateHelpers.datePartEquals(d, new Date());
                timexDateHelpers.isNextWeek(d, new Date());
                timexDateHelpers.isNextWeek(new Date(), d);
                timexDateHelpers.isLastWeek(new Date(), d);
                timexDateHelpers.weekOfYear(d);
                const after = d.getTime();
                expect(after).to.equal(before);
            });
            it('dateOfLastDay: Friday last week', () => {
                const day = 5;
                const date = new Date(2017, 8, 28);
                timexDateHelpers.datePartEquals(timexDateHelpers.dateOfLastDay(day, date), new Date(2017, 8, 22)).should.be.true;
            });
            it('dateOfNextDay: Wednesday next week', () => {
                const day = 3;
                const date = new Date(2017, 8, 28);
                timexDateHelpers.datePartEquals(timexDateHelpers.dateOfNextDay(day, date), new Date(2017, 9, 4)).should.be.true;
            });
            it('dateOfNextDay: today', () => {
                const day = 4;
                const date = new Date(2017, 8, 28);
                timexDateHelpers.datePartEquals(timexDateHelpers.dateOfNextDay(day, date), date).should.be.false;
            });
            it('datesMatchingDay', () => {
                const day = 4;
                const start = new Date(2017, 2, 1);
                const end = new Date(2017, 3, 1);
                const result = timexDateHelpers.datesMatchingDay(day, start, end);
                result.should.be.an('array').of.length(5);
                timexDateHelpers.datePartEquals(result[0], new Date(2017, 2, 2)).should.be.true;
                timexDateHelpers.datePartEquals(result[1], new Date(2017, 2, 9)).should.be.true;
                timexDateHelpers.datePartEquals(result[2], new Date(2017, 2, 16)).should.be.true;
                timexDateHelpers.datePartEquals(result[3], new Date(2017, 2, 23)).should.be.true;
                timexDateHelpers.datePartEquals(result[4], new Date(2017, 2, 30)).should.be.true;
            });
        });
    });
});

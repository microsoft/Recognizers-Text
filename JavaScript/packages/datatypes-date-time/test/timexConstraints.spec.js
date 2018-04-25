// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const timexConstraintsHelper = require('../src/timexConstraintsHelper.js');
const { Time, TimexProperty } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex constraints collapse', () => {
            describe('Date collapse helper function', () => {
                it('collapse pair', () => {
                    const ranges = [
                        { start: new Date(2017, 9, 2), end: new Date(2017, 9, 4) },
                        { start: new Date(2017, 9, 3), end: new Date(2017, 9, 6) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Date);
                    result.should.be.an('array').lengthOf(1);
                    result[0]['start'].getTime().should.equal((new Date(2017, 9, 3)).getTime());
                    result[0]['end'].getTime().should.equal((new Date(2017, 9, 4)).getTime());
                });
                it('collapse many', () => {
                    const ranges = [
                        { start: new Date(2017, 9, 2), end: new Date(2017, 9, 6) },
                        { start: new Date(2017, 9, 3), end: new Date(2017, 9, 17) },
                        { start: new Date(2017, 9, 4), end: new Date(2017, 9, 8) },
                        { start: new Date(2017, 9, 5), end: new Date(2017, 9, 15) },
                        { start: new Date(2017, 9, 1), end: new Date(2017, 9, 20) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Date);
                    result.should.be.an('array').lengthOf(1);
                    result[0]['start'].getTime().should.equal((new Date(2017, 9, 5)).getTime());
                    result[0]['end'].getTime().should.equal((new Date(2017, 9, 6)).getTime());
                });
                it('collapse - disjoint', () => {
                    const ranges = [
                        { start: new Date(2017, 9, 2), end: new Date(2017, 9, 6) },
                        { start: new Date(2017, 9, 3), end: new Date(2017, 9, 7) },
                        { start: new Date(2017, 9, 4), end: new Date(2017, 9, 8) },
                        { start: new Date(2017, 9, 10), end: new Date(2017, 9, 15) },
                        { start: new Date(2017, 9, 11), end: new Date(2017, 9, 14) },
                        { start: new Date(2017, 9, 12), end: new Date(2017, 9, 16) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Date);
                    result.should.be.an('array').lengthOf(2);
                    result[0]['start'].getTime().should.equal((new Date(2017, 9, 4)).getTime());
                    result[0]['end'].getTime().should.equal((new Date(2017, 9, 6)).getTime());
                    result[1]['start'].getTime().should.equal((new Date(2017, 9, 12)).getTime());
                    result[1]['end'].getTime().should.equal((new Date(2017, 9, 14)).getTime());
                });
                it('collapse - disjoint and sorted', () => {
                    const ranges = [
                        { start: new Date(2017, 9, 11), end: new Date(2017, 9, 14) },
                        { start: new Date(2017, 9, 2), end: new Date(2017, 9, 6) },
                        { start: new Date(2017, 9, 12), end: new Date(2017, 9, 16) },
                        { start: new Date(2017, 9, 4), end: new Date(2017, 9, 8) },
                        { start: new Date(2017, 9, 3), end: new Date(2017, 9, 7) },
                        { start: new Date(2017, 9, 10), end: new Date(2017, 9, 15) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Date);
                    result.should.be.an('array').lengthOf(2);
                    result[0]['start'].getTime().should.equal((new Date(2017, 9, 4)).getTime());
                    result[0]['end'].getTime().should.equal((new Date(2017, 9, 6)).getTime());
                    result[1]['start'].getTime().should.equal((new Date(2017, 9, 12)).getTime());
                    result[1]['end'].getTime().should.equal((new Date(2017, 9, 14)).getTime());
                });
                it('disjoint and sorted', () => {
                    const ranges = [
                        { start: new Date(2017, 1, 5), end: new Date(2017, 1, 10) },
                        { start: new Date(2017, 6, 24), end: new Date(2017, 7, 4) },
                        { start: new Date(2017, 2, 5), end: new Date(2017, 2, 10) },
                        { start: new Date(2017, 5, 29), end: new Date(2017, 5, 30) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Date);
                    result.should.be.an('array').lengthOf(4);
                    result[0]['start'].getTime().should.equal((new Date(2017, 1, 5)).getTime());
                    result[0]['end'].getTime().should.equal((new Date(2017, 1, 10)).getTime());
                    result[1]['start'].getTime().should.equal((new Date(2017, 2, 5)).getTime());
                    result[1]['end'].getTime().should.equal((new Date(2017, 2, 10)).getTime());
                    result[2]['start'].getTime().should.equal((new Date(2017, 5, 29)).getTime());
                    result[2]['end'].getTime().should.equal((new Date(2017, 5, 30)).getTime());
                    result[3]['start'].getTime().should.equal((new Date(2017, 6, 24)).getTime());
                    result[3]['end'].getTime().should.equal((new Date(2017, 7, 4)).getTime());
                });
            });
            describe('Time', () => {
                it('constructor(hour, minute, second)', () => {
                    const t = new Time(17, 30, 45);
                    t.should.have.property('hour', 17);
                    t.should.have.property('minute', 30);
                    t.should.have.property('second', 45);
                });
                it('constructor(value)', () => {
                    const t = new Time((9 * 3600000) + (28 * 60000) + (15 * 1000));
                    t.should.have.property('hour', 9);
                    t.should.have.property('minute', 28);
                    t.should.have.property('second', 15);
                });
                it('getTime()', () => {
                    const t = new Time(17, 30, 45);
                    t.getTime().should.equal((17 * 3600000) + (30 * 60000) + (45 * 1000));
                });
            });
            describe('Time collapse helper function', () => {
                it('collapse pair', () => {
                    const ranges = [
                        { start: new Time(9, 20, 0), end: new Time(12, 0, 1) },
                        { start: new Time(9, 30, 15), end: new Time(16, 0, 0) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Time);
                    result.should.be.an('array').lengthOf(1);
                    result[0]['start'].getTime().should.equal((new Time(9, 30, 15)).getTime());
                    result[0]['end'].getTime().should.equal((new Time(12, 0, 1)).getTime());
                });
                it('collapse many', () => {
                    const ranges = [
                        { start: new Time(9, 20, 0), end: new Time(17, 0, 0) },
                        { start: new Time(8, 45, 0), end: new Time(18, 0, 0) },
                        { start: new Time(7, 0, 0), end: new Time(14, 0, 0) },
                        { start: new Time(8, 0, 0), end: new Time(12, 0, 0) },
                        { start: new Time(6, 0, 0), end: new Time(10, 30, 0) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Time);
                    result.should.be.an('array').lengthOf(1);
                    result[0]['start'].getTime().should.equal((new Time(9, 20, 0)).getTime());
                    result[0]['end'].getTime().should.equal((new Time(10, 30, 0)).getTime());
                });
                it('collapse - disjoint', () => {
                    const ranges = [
                        { start: new Time(9, 0, 0), end: new Time(12, 0, 0) },
                        { start: new Time(13, 0, 0), end: new Time(17, 30, 0) },
                        { start: new Time(14, 0, 0), end: new Time(16, 30, 8) },
                        { start: new Time(15, 30, 0), end: new Time(15, 45, 0) },
                        { start: new Time(14, 20, 15), end: new Time(14, 20, 45) },
                        { start: new Time(10, 15, 0), end: new Time(10, 30, 0) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Time);
                    result.should.be.an('array').lengthOf(3);
                    result[0]['start'].getTime().should.equal((new Time(10, 15, 0)).getTime());
                    result[0]['end'].getTime().should.equal((new Time(10, 30, 0)).getTime());
                    result[1]['start'].getTime().should.equal((new Time(14, 20, 15)).getTime());
                    result[1]['end'].getTime().should.equal((new Time(14, 20, 45)).getTime());
                    result[2]['start'].getTime().should.equal((new Time(15, 30, 0)).getTime());
                    result[2]['end'].getTime().should.equal((new Time(15, 45, 0)).getTime());
                });
                it('collapse - disjoint and sorted', () => {
                    const ranges = [
                        { start: new Time(9, 0, 0), end: new Time(12, 0, 0) },
                        { start: new Time(15, 30, 0), end: new Time(15, 45, 0) },
                        { start: new Time(14, 20, 15), end: new Time(14, 20, 45) },
                        { start: new Time(10, 15, 0), end: new Time(10, 30, 0) },
                        { start: new Time(14, 0, 0), end: new Time(16, 30, 8) },
                        { start: new Time(13, 0, 0), end: new Time(17, 30, 0) }
                    ];
                    const result = timexConstraintsHelper.collapse(ranges, Time);
                    result.should.be.an('array').lengthOf(3);
                    result[0]['start'].getTime().should.equal((new Time(10, 15, 0)).getTime());
                    result[0]['end'].getTime().should.equal((new Time(10, 30, 0)).getTime());
                    result[1]['start'].getTime().should.equal((new Time(14, 20, 15)).getTime());
                    result[1]['end'].getTime().should.equal((new Time(14, 20, 45)).getTime());
                    result[2]['start'].getTime().should.equal((new Time(15, 30, 0)).getTime());
                    result[2]['end'].getTime().should.equal((new Time(15, 45, 0)).getTime());
                });
            });
        });
    });
});

// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const { Time, TimexProperty } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex', () => {
            describe('making from JavaScript Date', () => {
                it('fromDate', () => {
                    TimexProperty.fromDate(new Date(2017, 11, 5)).timex.should.equal('2017-12-05');
                });
                it('fromDateTime', () => {
                    TimexProperty.fromDateTime(new Date(2017, 11, 5, 23, 57, 35)).timex.should.equal('2017-12-05T23:57:35');
                });
            });
            describe('making from Time class', () => {
                it('fromTime', () => {
                    TimexProperty.fromTime(new Time(23, 59, 30)).timex.should.equal('T23:59:30');
                });
            });
            describe('roundtrip', () => {
                const roundtrip = function (timex) { (new TimexProperty(timex)).timex.should.equal(timex); };
                describe('date', () => {
                    it('various examples', () => {
                        roundtrip('2017-09-27');
                        roundtrip('XXXX-WXX-3');
                        roundtrip('XXXX-12-05');
                    });
                });
                describe('time', () => {
                    it('various examples', () => {
                        roundtrip('T17:30:45');
                        roundtrip('T05:06:07');
                        roundtrip('T17:30');
                        roundtrip('T23');
                    });
                });
                describe('duration', () => {
                    it('various examples', () => {
                        roundtrip('P50Y');
                        roundtrip('P6M');
                        roundtrip('P3W');
                        roundtrip('P5D');
                        roundtrip('PT16H');
                        roundtrip('PT32M');
                        roundtrip('PT20S');
                    });
                });
                describe('now', () => {
                    it('now', () => {
                        roundtrip('PRESENT_REF');
                    });
                });
                describe('datetime', () => {
                    it('various examples', () => {
                        roundtrip('XXXX-WXX-3T04');
                        roundtrip('2017-09-27T11:41:30');
                    });
                });
                describe('daterange', () => {
                    it('various examples', () => {
                        roundtrip('2017');
                        roundtrip('SU');
                        roundtrip('2017-WI');
                        roundtrip('2017-09');
                        roundtrip('2017-W37');
                        roundtrip('2017-W37-WE');
                        roundtrip('XXXX-05');
                    });
                });
                describe('daterange specified with (start,end,duration)', () => {
                    it ('various examples', () => {
                        roundtrip('(XXXX-WXX-3,XXXX-WXX-6,P3D)');
                        roundtrip('(XXXX-01-01,XXXX-08-05,P216D)');
                        roundtrip('(2017-01-01,2017-08-05,P216D)');
                    });
                });
                describe('daterange specified with (start,end,duration) (leap year)', () => {
                    it ('various examples', () => {
                        roundtrip('(2016-01-01,2016-08-05,P217D)');
                    });
                });
                describe('timerange', () => {
                    it('various examples', () => {
                        roundtrip('TEV');
                    });
                });
                describe('timerange specified with (start,end,duration)', () => {
                    it ('various examples', () => {
                        roundtrip('(T16,T19,PT3H)');
                    });
                });
                describe('datetimerange', () => {
                    it('various examples', () => {
                        roundtrip('2017-09-27TEV');
                    });
                });
                describe('datetimerange specified with (start,end,duration)', () => {
                    it ('various examples', () => {
                        roundtrip('(2017-09-08T21:19:29,2017-09-08T21:24:29,PT5M)');
                        roundtrip('(XXXX-WXX-3T16,XXXX-WXX-6T15,PT71H)');
                    });
                });
            });
            describe('toString', () => {
                it ('veryify call is delegated', () => {
                    (new TimexProperty('XXXX-05-05')).toString().should.equal('5th May');
                });
            });
            describe('toNaturalLanguage', () => {
                it ('veryify call is delegated', () => {
                    const today = new Date(2017, 9, 16);
                    (new TimexProperty('2017-10-17')).toNaturalLanguage(today).should.equal('tomorrow');
                });
            });
        });
    });
});

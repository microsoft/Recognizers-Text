// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const { TimexProperty, creator, resolver } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex resolution with range constraints', () => {
            describe('daterange', () => {
                it('definite', () => {
                    const constraints = [ { year: 2017, month: 9, dayOfMonth: 27, days: 2 } ];
                    resolver.evaluate(['2017-09-28'], constraints).map(t => t.timex).should.have.members(['2017-09-28']);
                });
                it('definite - constrainst expressed as timex', () => {
                    const constraints = [ '(2017-09-27,2017-09-29,P2D)' ];
                    resolver.evaluate(['2017-09-28'], constraints).map(t => t.timex).should.have.members(['2017-09-28']);
                });
                it('month and date', () => {
                    const constraints = [ { year: 2006, month: 1, dayOfMonth: 1, years: 2 } ];
                    resolver.evaluate(['XXXX-05-29'], constraints).map(t => t.timex).should.have.members(['2006-05-29','2007-05-29']);
                });
                it('month and date conditional on month/date', () => {
                    const constraints = [ '(2006-01-01,2008-06-01,P882D)' ];
                    resolver.evaluate(['XXXX-05-29'], constraints).map(t => t.timex).should.have.members(['2006-05-29','2007-05-29','2008-05-29']);
                });
                it('Saturdays in September', () => {
                    const constraints = [ '2017-09' ];
                    resolver.evaluate(['XXXX-WXX-6'], constraints).map(t => t.timex).should.have.members(['2017-09-02','2017-09-09','2017-09-16','2017-09-23','2017-09-30']);
                });
                it('Saturdays in September (expressed as range)', () => {
                    const constraints = [ '(2017-09-01,2017-10-01,P30D)'  ];
                    resolver.evaluate(['XXXX-WXX-6'], constraints).map(t => t.timex).should.have.members(['2017-09-02','2017-09-09','2017-09-16','2017-09-23','2017-09-30']);
                });
                it('year', () => {
                    const constraints = [ '2018' ];
                    resolver.evaluate(['XXXX-05-29'], constraints).map(t => t.timex).should.have.members(['2018-05-29']);
                });
                it('year (expressed as range)', () => {
                    const constraints = [ '(2018-01-01,2019-01-01,P365D)' ];
                    resolver.evaluate(['XXXX-05-29'], constraints).map(t => t.timex).should.have.members(['2018-05-29']);
                });
                it('multiple constraints', () => {
                    const constraints = [ '(2017-09-01,2017-09-08,P7D)', '(2017-10-01,2017-10-08,P7D)'  ];
                    resolver.evaluate(['XXXX-WXX-3'], constraints).map(t => t.timex).should.have.members(['2017-09-06','2017-10-04']);
                });
                it('multiple candidates with multiple constraints', () => {
                    const constraints = [ '(2017-09-01,2017-09-08,P7D)', '(2017-10-01,2017-10-08,P7D)'  ];
                    resolver.evaluate(['XXXX-WXX-2', 'XXXX-WXX-4'], constraints).map(t => t.timex).should.have.members(['2017-09-05','2017-09-07','2017-10-03','2017-10-05']);
                });
                it('multiple overlapping constraints', () => {
                    const constraints = [ '(2017-09-03,2017-09-07,P4D)', '(2017-09-01,2017-09-08,P7D)', '(2017-09-01,2017-09-16,P15D)'  ];
                    resolver.evaluate(['XXXX-WXX-3'], constraints).map(t => t.timex).should.have.members(['2017-09-06']);
                });
            });
            describe('timerange', () => {
                it('time within range', () => {
                    const constraints = [ { hour: 14, hours: 4 } ];
                    resolver.evaluate(['T16'], constraints).map(t => t.timex).should.have.members(['T16']);
                });
                it('multiple times within range', () => {
                    const constraints = [ { hour: 14, hours: 4 } ];
                    resolver.evaluate(['T12', 'T16', 'T16:30', 'T17', 'T18'], constraints).map(t => t.timex).should.have.members(['T16', 'T16:30', 'T17']);
                });
                it('time with overlapping ranges', () => {
                    const constraints = [ { hour: 16, hours: 4 } ];
                    resolver.evaluate(['T19'], constraints).map(t => t.timex).should.have.members(['T19']);
                    constraints.push({ hour: 14, hours: 4 });
                    resolver.evaluate(['T19'], constraints).should.be.an('array').lengthOf(0);
                    resolver.evaluate(['T17'], constraints).map(t => t.timex).should.have.members(['T17']);
                });
                it('multiple times with overlapping ranges', () => {
                    const constraints = [ { hour: 16, hours: 4 } ];
                    resolver.evaluate(['T19', 'T19:30'], constraints).map(t => t.timex).should.have.members(['T19', 'T19:30']);
                    constraints.push({ hour: 14, hours: 4 });
                    resolver.evaluate(['T19', 'T19:30'], constraints).map(t => t.timex).should.be.an('array').lengthOf(0);
                    resolver.evaluate(['T17', 'T17:30', 'T19:30'], constraints).map(t => t.timex).should.have.members(['T17', 'T17:30']);
                });
                it('filter duplicates', () => {
                    const constraints = [ { hour: 14, hours: 4 } ];
                    resolver.evaluate(['T16', 'T16', 'T16'], constraints).map(t => t.timex).should.have.lengthOf(1).members(['T16']);
                });
            });
            describe('carry through time', () => {
                it('definite', () => {
                    const constraints = [ { year: 2017, month: 9, dayOfMonth: 27, days: 2 } ];
                    resolver.evaluate(['2017-09-28T18:30:01'], constraints).map(t => t.timex).should.have.members(['2017-09-28T18:30:01']);
                });
                it('definite - constrainst expressed as timex', () => {
                    const constraints = [ '(2017-09-27,2017-09-29,P2D)' ];
                    resolver.evaluate(['2017-09-28T18:30:01'], constraints).map(t => t.timex).should.have.members(['2017-09-28T18:30:01']);
                });
                it('month and date', () => {
                    const constraints = [ { year: 2006, month: 1, dayOfMonth: 1, years: 2 } ];
                    resolver.evaluate(['XXXX-05-29T19:30'], constraints).map(t => t.timex).should.have.members(['2006-05-29T19:30','2007-05-29T19:30']);
                });
                it('month and date conditional on month/date', () => {
                    const constraints = [ '(2006-01-01,2008-06-01,P882D)' ];
                    resolver.evaluate(['XXXX-05-29T19:30'], constraints).map(t => t.timex).should.have.members(['2006-05-29T19:30','2007-05-29T19:30','2008-05-29T19:30']);
                });
                it('Saturdays in September', () => {
                    const constraints = [ '(2017-09-01,2017-10-01,P30D)'  ];
                    resolver.evaluate(['XXXX-WXX-6T01:00:00'], constraints).map(t => t.timex).should.have.members(['2017-09-02T01','2017-09-09T01','2017-09-16T01','2017-09-23T01','2017-09-30T01']);
                });
                it('multiple constraints', () => {
                    const constraints = [ '(2017-09-01,2017-09-08,P7D)', '(2017-10-01,2017-10-08,P7D)'  ];
                    resolver.evaluate(['XXXX-WXX-3T01:02'], constraints).map(t => t.timex).should.have.members(['2017-09-06T01:02','2017-10-04T01:02']);
                });
            });
            describe('combined daterange and timerange', () => {
                it('day of week constrained by next week and any time', () => {
                    const constraints = [
                        { year: 2017, month: 10, dayOfMonth: 5, days: 7 },
                        { hour: 0, minute: 0, second: 0, hours: 24 }
                    ];
                    resolver.evaluate(['XXXX-WXX-3T04', 'XXXX-WXX-3T16'], constraints).map(t => t.timex).should.have.members(['2017-10-11T04', '2017-10-11T16']);
                });
                it('day of week constrained by next week and business hours', () => {
                    const constraints = [
                        { year: 2017, month: 10, dayOfMonth: 5, days: 7 },
                        { hour: 12, minute: 0, second: 0, hours: 8 }
                    ];
                    resolver.evaluate(['XXXX-WXX-3T04', 'XXXX-WXX-3T16'], constraints).map(t => t.timex).should.have.members(['2017-10-11T16']);
                });
            });
            describe('adding times', () => {
                it('add specific time to date', () => {
                    const constraints = [
                        new TimexProperty('2017'),
                        new TimexProperty('T19:30:00')
                    ];
                    resolver.evaluate(['XXXX-05-29'], constraints).map(t => t.timex).should.have.members(['2017-05-29T19:30']);
                });
                it('add specific times to date', () => {
                    const constraints = [
                        new TimexProperty('2017'),
                        new TimexProperty('T19:30:00'),
                        new TimexProperty('T20:01:01'),
                    ];
                    resolver.evaluate(['XXXX-05-29'], constraints).map(t => t.timex).should.have.members(['2017-05-29T19:30', '2017-05-29T20:01:01']);
                });
            });
            describe('duration', () => {
                it('duration evaluated with a specific datetime results in that datetime plus the duration', () => {
                    const constraints = [
                        new TimexProperty('2017-12-05T19:30:00')
                    ];
                    resolver.evaluate(['PT5M'], constraints).map(t => t.timex).should.have.members(['2017-12-05T19:35']);
                });
                it('duration evaluated with a specific time results in that time plus the duration', () => {
                    const constraints = [
                        new TimexProperty('T19:30:00')
                    ];
                    resolver.evaluate(['PT5M'], constraints).map(t => t.timex).should.have.members(['T19:35']);
                });
                it('duration evaluated with no constraints currently results in no solutions', () => {
                    const constraints = [
                    ];
                    resolver.evaluate(['PT5M'], constraints).map(t => t.timex).should.have.length(0);
                });
                it('duration evaluated with no datetime or time constraints currently results in no solutions', () => {
                    const constraints = [
                        { year: 2017, month: 10, dayOfMonth: 5, days: 7 }
                    ];
                    resolver.evaluate(['PT5M'], constraints).map(t => t.timex).should.have.length(0);
                });
            });
            describe('candidate includes a timerange', () => {
                it('basic resolve day against daterange', () => {
                    const constraints = [
                        new TimexProperty('(2018-06-04,2018-06-11,P7D)'),
                        new TimexProperty('(2018-06-11,2018-06-18,P7D)'),
                        creator.evening
                    ];
                    resolver.evaluate(['XXXX-WXX-7'], constraints).map(t => t.timex).should.have.members(['2018-06-10T16', '2018-06-17T16']);
                });
                it('no time constraint', () => {
                    const constraints = [
                        new TimexProperty('(2018-06-04,2018-06-11,P7D)'),
                        new TimexProperty('(2018-06-11,2018-06-18,P7D)')
                    ];
                    resolver.evaluate(['XXXX-WXX-7TEV'], constraints).map(t => t.timex).should.have.members(['2018-06-10TEV', '2018-06-17TEV']);
                });
                it('overlapping constraint 1', () => {
                    const constraints = [
                        new TimexProperty('(2018-06-04,2018-06-11,P7D)'),
                        new TimexProperty('(2018-06-11,2018-06-18,P7D)'),
                        new TimexProperty('(T18,T22,PT4H)')
                    ];
                    resolver.evaluate(['XXXX-WXX-7TEV'], constraints).map(t => t.timex).should.have.members(['2018-06-10T18', '2018-06-17T18']);
                });
                it('overlapping constraint 2', () => {
                    const constraints = [
                        new TimexProperty('(2018-06-04,2018-06-11,P7D)'),
                        new TimexProperty('(2018-06-11,2018-06-18,P7D)'),
                        new TimexProperty('(T15,T19,PT4H)')
                    ];
                    resolver.evaluate(['XXXX-WXX-7TEV'], constraints).map(t => t.timex).should.have.members(['2018-06-10T16', '2018-06-17T16']);
                });
                it('non overlapping constraint', () => {
                    const constraints = [
                        new TimexProperty('(2018-06-04,2018-06-11,P7D)'),
                        new TimexProperty('(2018-06-11,2018-06-18,P7D)'),
                        creator.morning
                    ];
                    resolver.evaluate(['XXXX-WXX-7TEV'], constraints).map(t => t.timex).should.have.length(0);
                });
                it('sunday evening as the candidate and the constraint', () => {
                    const constraints = [
                        new TimexProperty('(2018-06-04,2018-06-11,P7D)'),
                        new TimexProperty('(2018-06-11,2018-06-18,P7D)'),
                        creator.evening
                    ];
                    resolver.evaluate(['XXXX-WXX-7TEV'], constraints).map(t => t.timex).should.have.members(['2018-06-10T16', '2018-06-17T16']);
                });
            });
        });
    });
});

// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');

const { TimexProperty, resolver, creator } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Timex integrate resolution with constants and conversion', () => {
            describe('with constants', () => {
                const today = new Date(2017, 9, 5);
                it('day of week constrained by next week in the evening', () => {
                    const constraints = [ creator.weekFromToday(today), creator.evening ];
                    const resolutions = resolver.evaluate(['XXXX-WXX-3T04', 'XXXX-WXX-3T16'], constraints);
                    resolutions.map(t => t.timex).should.be.an('array').lengthOf(1).members(['2017-10-11T16']);
                    const text = resolutions[0].toNaturalLanguage(today);
                    text.should.equal('next Wednesday 4PM');
                });
            });
        });
        describe('Timex integrate resolution with incremental updates', () => {
            describe('date + year (daterange) + time (time)', () => {
                const today = new Date(2017, 9, 5);
                it('day of week constrained by next year and adding a specific time', () => {
                    const constraints = [ new TimexProperty('T17:00:00'), new TimexProperty('2018') ];
                    const resolutions = resolver.evaluate(['XXXX-03-15'], constraints);
                    resolutions.map(t => t.timex).should.be.an('array').lengthOf(1).members(['2018-03-15T17']);
                    const text = resolutions[0].toNaturalLanguage(today);
                    text.should.equal('15th March 2018 5PM');
                });
            });
        });
    });
});

// Copyright (c) Microsoft Corporation. All rights reserved.

const chai = require('chai');
const { Time } = require('../index.js');

const assert = chai.assert;
const expect = chai.expect;
chai.should();

describe('No Network', () => {
    describe('Datatypes', () => {
        describe('Time', () => {
            it('constructor', () => {
                const t = new Time(23, 45, 32);
                t.hour.should.equal(23);
                t.minute.should.equal(45);
                t.second.should.equal(32);
            });
            it('getTime', () => {
                (new Time(23, 45, 32)).getTime().should.equal(85532000);
            });
        });
    });
});

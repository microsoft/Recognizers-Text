// Copyright (c) Microsoft Corporation. All rights reserved.

const Timex = require('./timex.js').Timex;

class TimexSet {
    constructor (timex) {
        this.timex = new Timex(timex);
    }
}

module.exports.TimexSet = TimexSet;

// Copyright (c) Microsoft Corporation. All rights reserved.

const TimexProperty = require('./timexProperty.js').TimexProperty;

class TimexSet {
    constructor (timex) {
        this.timex = new TimexProperty(timex);
    }
}

module.exports.TimexSet = TimexSet;

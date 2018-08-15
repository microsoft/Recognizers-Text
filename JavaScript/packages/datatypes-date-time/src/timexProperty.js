// Copyright (c) Microsoft Corporation. All rights reserved.

const timexParsing = require('./timexParsing.js');
const timexInference = require('./timexInference.js');
const timexFormat = require('./timexFormat.js');
const timexConvert = require('./timexConvert.js');
const timexRelativeConvert = require('./timexRelativeConvert.js');

class TimexProperty {
    constructor (timex) {
        if (typeof timex === 'string') {
            timexParsing.parseString(timex, this);
        }
        else {
            timexParsing.fromObject(timex, this);
        }
        // TODO: constructing a Timex from a Timex should be very cheap
    }

    get timex() {
        return timexFormat.format(this);
    }

    get types () {
        return timexInference.infer(this);
    }

    toString () {
        return timexConvert.convertTimexToString(this);
    }

    // TODO: consider [locales[, options]] similar to Date.toLocaleString([locales[, options]])
    toNaturalLanguage (referenceDate) {
        return timexRelativeConvert.convertTimexToStringRelative(this, referenceDate);
    }

    static fromDate (date) {
        return new TimexProperty({
            year: date.getFullYear(),
            month: date.getMonth() + 1,
            dayOfMonth: date.getDate()
        });
    }
    
    static fromDateTime (date) {
        return new TimexProperty({
            year: date.getFullYear(),
            month: date.getMonth() + 1,
            dayOfMonth: date.getDate(),
            hour: date.getHours(),
            minute: date.getMinutes(),
            second: date.getSeconds()
        });
    }
    
    static fromTime (time) {
        return new TimexProperty(time);
    }
}

module.exports.TimexProperty = TimexProperty;

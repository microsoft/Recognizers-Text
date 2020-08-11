// Copyright (c) Microsoft Corporation. All rights reserved.

const timexregex = require('./timexRegex.js');

const parseString = function (timex, obj) {
    // a reference to the present
    if (timex === 'PRESENT_REF') {
        obj.now = true;
    }
    // duration
    else if (timex.startsWith('P')) {
        extractDuration(timex, obj);
    }
    // range indicated with start and end dates and a duration
    else if (timex.startsWith('(') && timex.endsWith(')')) {
        extractStartEndRange(timex, obj);
    }
    // date and time and their respective ranges
    else {
        extractDateTime(timex, obj);
    }
};

const extractDuration = function (s, obj) {
    const extracted = {};
    timexregex.extract('period', s, extracted);
    if (extracted.dateUnit) {
        obj[{ Y: 'years', M: 'months', W: 'weeks', D: 'days' }[extracted.dateUnit]] = extracted.amount;
    }
    else if (extracted.timeUnit) {
        obj[{ H: 'hours', M: 'minutes', S: 'seconds' }[extracted.timeUnit]] = extracted.amount;
    }
};

const extractStartEndRange = function (s, obj) {
    const parts = s.substring(1, s.length - 1).split(',');
    if (parts.length === 3) {
        extractDateTime(parts[0], obj);
        extractDuration(parts[2], obj);
    }
};

const extractDateTime = function (s, obj) {
    const indexOfT = s.indexOf('T');
    if (indexOfT === -1) {
        timexregex.extract('date', s, obj);
    }
    else {
        timexregex.extract('date', s.substr(0, indexOfT), obj);
        timexregex.extract('time', s.substr(indexOfT), obj);
    }
};

const fromObject = function (source, obj) {
    Object.assign(obj, source);
    if ('hour' in obj) {
        if (!('minute' in obj)) {
            obj.minute = 0;
        }
        if (!('second' in obj)) {
            obj.second = 0;
        }
    }
};

module.exports = {
    parseString: parseString,
    fromObject: fromObject
};

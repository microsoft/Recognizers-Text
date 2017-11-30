// Copyright (c) Microsoft Corporation. All rights reserved.

const value = function (s) { return s; };
const isTrue = function () { return true; };
const zero = function () { return 0; };

const timexRegex = {

    date: [
        // date
        { regex: /^(\d\d\d\d)-(\d\d)-(\d\d)$/, props: { year: Number, month: Number, dayOfMonth: Number } },
        { regex: /^XXXX-WXX-(\d)$/, props: { dayOfWeek: Number } },
        { regex: /^XXXX-(\d\d)-(\d\d)$/, props: { month: Number, dayOfMonth: Number } },
        // daterange
        { regex: /^(\d\d\d\d)$/, props: { year: Number } },
        { regex: /^(\d\d\d\d)-(\d\d)$/, props: { year: Number, month: Number } },
        { regex: /^(SP|SU|FA|WI)$/, props: { season: value } },
        { regex: /^(\d\d\d\d)-(SP|SU|FA|WI)$/, props: { year: Number, season: value } },
        { regex: /^(\d\d\d\d)-W(\d\d)$/, props: { year: Number, weekOfYear: Number } },
        { regex: /^(\d\d\d\d)-W(\d\d)-WE$/, props: { year: Number, weekOfYear: Number, weekend: isTrue } },
        { regex: /^XXXX-(\d\d)$/, props: { month: Number } },
        { regex: /^XXXX-(\d\d)-W(\d\d)$/, props: { month: Number, weekOfMonth: Number } },
        { regex: /^XXXX-(\d\d)-WXX-(\d)-(\d)$/, props: { month: Number, weekOfMonth: Number, dayOfWeek: Number } }
    ],

    time: [
        // time
        { regex: /^T(\d\d)$/, props: { hour: Number, minute: zero, second: zero } },
        { regex: /^T(\d\d):(\d\d)$/, props: { hour: Number, minute: Number, second: zero } },
        { regex: /^T(\d\d):(\d\d):(\d\d)$/, props: { hour: Number, minute: Number, second: Number } },
        // timerange
        { regex: /^T(DT|NI|MO|AF|EV)$/, props: { partOfDay: value } }
    ],

    period: [
        { regex: /^P(\d*\.?\d+)(Y|M|W|D)$/, props: { amount: Number, dateUnit: value } },
        { regex: /^PT(\d*\.?\d+)(H|M|S)$/, props: { amount: Number, timeUnit: value } }
    ]
};

const tryExtract = function (entry, timex, result) {
    const regexResult = timex.match(entry.regex);
    if (!regexResult) {
        return false;
    }
    let index = 1;
    for (const name in entry.props) {
        const val = regexResult[index++];
        result[name] = entry.props[name](val);
    }
    return true;
};

const extract = function (name, timex, result) {
    for (const entry of timexRegex[name]) {
        if (tryExtract(entry, timex, result)) {
            return true;
        }
    }
    return false;
};

module.exports.extract = extract;

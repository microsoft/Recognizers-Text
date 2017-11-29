// Copyright (c) Microsoft Corporation. All rights reserved.

const fixedFormatNumber = require('./timexDateHelpers.js').fixedFormatNumber;

const dateValue = function (obj) {
    if (obj.year !== undefined && obj.month !== undefined && obj.dayOfMonth !== undefined) {
        return `${fixedFormatNumber(obj.year, 4)}-${fixedFormatNumber(obj.month, 2)}-${fixedFormatNumber(obj.dayOfMonth, 2)}`;
    }
    return '';
};

const timeValue = function (obj) {
    if (obj.hour !== undefined && obj.minute !== undefined && obj.second !== undefined) {
        return `${fixedFormatNumber(obj.hour, 2)}:${fixedFormatNumber(obj.minute, 2)}:${fixedFormatNumber(obj.second, 2)}`;
    }
    return '';
};

const datetimeValue = function (obj) {
    return `${dateValue(obj)} ${timeValue(obj)}`;
};

const durationValue = function (obj) {
    if (obj.years !== undefined) {
        return (31536000 * obj.years).toString();
    }
    if (obj.months !== undefined) {
        return (2592000 * obj.months).toString();
    }
    if (obj.weeks !== undefined) {
        return (604800 * obj.weeks).toString();
    }
    if (obj.days !== undefined) {
        return (86400 * obj.days).toString();
    }
    if (obj.hours !== undefined) {
        return (3600 * obj.hours).toString();
    }
    if (obj.minutes !== undefined) {
        return (60 * obj.minutes).toString();
    }
    if (obj.seconds !== undefined) {
        return obj.seconds.toString();
    }
    return '';
};

module.exports = {
    dateValue: dateValue, 
    timeValue: timeValue,
    datetimeValue: datetimeValue,
    durationValue: durationValue
};

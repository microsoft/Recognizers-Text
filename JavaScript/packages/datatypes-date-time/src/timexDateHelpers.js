// Copyright (c) Microsoft Corporation. All rights reserved.

const cloneDate = function (date) {
    const result = new Date();
    result.setTime(date.getTime());
    return result;
};

const tomorrow = function (date) {
    const result = cloneDate(date);
    result.setDate(result.getDate() + 1);
    return result;
};

const yesterday = function (date) {
    const result = cloneDate(date);
    result.setDate(result.getDate() - 1);
    return result;
};

const datePartEquals = function (dateX, dateY) {
    return (dateX.getFullYear() === dateY.getFullYear())
        && (dateX.getMonth() === dateY.getMonth())
        && (dateX.getDate() === dateY.getDate());
};

const isDateInWeek = function (date, startOfWeek) {
    let d = cloneDate(startOfWeek);
    for (let i=0; i<7; i++) {
        if (datePartEquals(date, d)) {
            return true;
        }
        d = tomorrow(d);
    }
    return false;
};

const isThisWeek = function (date, referenceDate) {
    const startOfThisWeek = cloneDate(referenceDate);
    startOfThisWeek.setDate(startOfThisWeek.getDate() - startOfThisWeek.getDay());
    return isDateInWeek(date, startOfThisWeek);
};

const isNextWeek = function (date, referenceDate) {
    const startOfNextWeek = cloneDate(referenceDate);
    startOfNextWeek.setDate(startOfNextWeek.getDate() + (7 - startOfNextWeek.getDay()));
    return isDateInWeek(date, startOfNextWeek);
};

const isLastWeek = function (date, referenceDate) {
    const startOfLastWeek = cloneDate(referenceDate);
    startOfLastWeek.setDate(startOfLastWeek.getDate() - (7 + startOfLastWeek.getDay()));
    return isDateInWeek(date, startOfLastWeek);
};

const weekOfYear = function (date) {
    const ds = new Date(date.getFullYear(), 0);
    const de = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    let weeks = 1;
    while (ds.getTime() < de.getTime()) {
        const jsDayOfWeek = ds.getDay();
        const isoDayOfWeek = jsDayOfWeek == 0 ? 7 : jsDayOfWeek; 
        if (isoDayOfWeek === 7) {
            weeks++;
        }
        ds.setDate(ds.getDate() + 1);
    }
    return weeks;
};

const fixedFormatNumber = function (n, size) {
    const s = n.toString();
    let zeros = '';
    const np = size - s.length;
    for (let i=0; i<np; i++) {
        zeros += '0';
    }
    return `${zeros}${s}`;
};

const dateOfLastDay = function (day, referenceDate) {
    const result = cloneDate(referenceDate);
    result.setDate(result.getDate() - 1);
    while (result.getDay() !== day) {
        result.setDate(result.getDate() - 1);
    }
    return result;
};

const dateOfNextDay = function (day, referenceDate) {
    const result = cloneDate(referenceDate);
    do {
        result.setDate(result.getDate() + 1);
    }
    while (result.getDay() !== day);
    return result;
};

const datesMatchingDay = function (day, start, end) {
    const result = [];
    const d = cloneDate(start);
    while (!datePartEquals(d, end)) {
        if (d.getDay() === day) {
            result.push(cloneDate(d));
        }
        d.setDate(d.getDate() + 1);
    }
    return result;
};

module.exports = {
    tomorrow: tomorrow,
    yesterday: yesterday,
    datePartEquals: datePartEquals,
    isThisWeek: isThisWeek,
    isNextWeek: isNextWeek,
    isLastWeek: isLastWeek,
    weekOfYear: weekOfYear,
    fixedFormatNumber: fixedFormatNumber,
    dateOfLastDay: dateOfLastDay,
    dateOfNextDay: dateOfNextDay,
    datesMatchingDay: datesMatchingDay
};

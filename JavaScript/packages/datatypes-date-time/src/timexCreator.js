// Copyright (c) Microsoft Corporation. All rights reserved.

const timexHelpers = require('./timexHelpers.js');
const timexDateHelpers = require('./timexDateHelpers.js');
const TimexProperty = require('./timexProperty.js').TimexProperty;

const today = function (date) {
    return TimexProperty.fromDate(date || new Date()).timex;
};

const tomorrow = function (date) {
    const d = (date === undefined) ? new Date() : new Date(date.getTime());
    d.setDate(d.getDate() + 1);
    return TimexProperty.fromDate(d).timex;
};

const yesterday = function (date) {
    const d = (date === undefined) ? new Date() : new Date(date.getTime());
    d.setDate(d.getDate() - 1);
    return TimexProperty.fromDate(d).timex;
};

const weekFromToday = function (date) {
    const d = (date === undefined) ? new Date() : new Date(date.getTime());
    return (new TimexProperty(Object.assign(TimexProperty.fromDate(d), { days: 7 }))).timex;
};

const weekBackFromToday = function (date) {
    const d = (date === undefined) ? new Date() : new Date(date.getTime());
    d.setDate(d.getDate() - 7);
    return (new TimexProperty(Object.assign(TimexProperty.fromDate(d), { days: 7 }))).timex;
};

const thisWeek = function (date) {
    const d = (date === undefined) ? new Date() : new Date(date.getTime());
    d.setDate(d.getDate() - 7);
    const start = timexDateHelpers.dateOfNextDay(1, d);
    return (new TimexProperty(Object.assign(TimexProperty.fromDate(start), { days: 7 }))).timex;
};

const nextWeek = function (date) {
    const d = (date === undefined) ? new Date() : new Date(date.getTime());
    const start = timexDateHelpers.dateOfNextDay(1, d);
    return (new TimexProperty(Object.assign(TimexProperty.fromDate(start), { days: 7 }))).timex;
};

const lastWeek = function (date) {
    const d = (date === undefined) ? new Date() : new Date(date.getTime());
    const start = timexDateHelpers.dateOfLastDay(1, d);
    start.setDate(start.getDate() - 7);
    return (new TimexProperty(Object.assign(TimexProperty.fromDate(start), { days: 7 }))).timex;
};

const nextWeeksFromToday = function (n, date) {
    const d = (date === undefined) ? new Date() : new Date(date.getTime());
    return (new TimexProperty(Object.assign(TimexProperty.fromDate(d), { days: 7 * n }))).timex;
};

// The following constants are consistent with the Recognizer results
const monday = 'XXXX-WXX-1';
const tuesday = 'XXXX-WXX-2';
const wednesday = 'XXXX-WXX-3';
const thursday = 'XXXX-WXX-4';
const friday = 'XXXX-WXX-5';
const saturday = 'XXXX-WXX-6';
const sunday = 'XXXX-WXX-7';
const morning = '(T08,T12,PT4H)';
const afternoon = '(T12,T16,PT4H)';
const evening = '(T16,T20,PT4H)';
const daytime = '(T08,T18,PT10H)';

module.exports = {
    today: today,
    tomorrow: tomorrow,
    yesterday: yesterday,
    weekFromToday: weekFromToday,
    weekBackFromToday: weekBackFromToday,
    thisWeek: thisWeek,
    nextWeek: nextWeek,
    lastWeek: lastWeek,
    nextWeeksFromToday: nextWeeksFromToday,
    monday: monday,
    tuesday: tuesday,
    wednesday: wednesday,
    thursday: thursday,
    friday: friday,
    saturday: saturday,
    sunday: sunday,
    morning: morning,
    afternoon: afternoon,
    evening: evening,
    daytime: daytime
};

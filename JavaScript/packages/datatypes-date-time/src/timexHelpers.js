// Copyright (c) Microsoft Corporation. All rights reserved.

const Time = require('./time.js').Time;
const timexInference = require('./timexInference.js');

const cloneDateTime = function (timex) {
    const result = Object.assign({}, timex);
    delete result.years;
    delete result.months;
    delete result.weeks;
    delete result.days;
    delete result.hours;
    delete result.minutes;
    delete result.seconds;
    return result;
};

const cloneDuration = function (timex) {
    const result = Object.assign({}, timex);
    delete result.year;
    delete result.month;
    delete result.dayOfMonth;
    delete result.dayOfWeek;
    delete result.weekOfYear;
    delete result.weekOfMonth;
    delete result.season;
    delete result.hour;
    delete result.minute;
    delete result.second;
    delete result.weekend;
    delete result.partOfDay;
    return result;
};

const timexDateAdd = function (start, duration) {
    if ('dayOfWeek' in start) {
        const end = Object.assign({}, start);
        if ('days' in duration) {
            end.dayOfWeek += duration.days;
        }
        return end;
    }
    if ('month' in start && 'dayOfMonth' in start) {
        if ('days' in duration) {
            if ('year' in start) {
                const d = new Date(start.year, start.month - 1, start.dayOfMonth, 0, 0, 0);
                for (let i=0; i < duration.days; i++) {
                    d.setDate(d.getDate() + 1);
                }
                return { year: d.getFullYear(), month: d.getMonth() + 1, dayOfMonth: d.getDate() };
            }
            else {
                const d = new Date(2001, start.month - 1, start.dayOfMonth, 0, 0, 0);
                for (let i=0; i < duration.days; i++) {
                    d.setDate(d.getDate() + 1);
                }
                return { month: d.getMonth() + 1, dayOfMonth: d.getDate() };
            }
        }
        if ('years' in duration) {
            if ('year' in start) {
                return { year: start.year + duration.years, month: start.month, dayOfMonth: start.dayOfMonth };
            }
        }
        if ('months' in duration) {
            if ('month' in start) {
                return { year: start.year, month: start.month + duration.months, dayOfMonth: start.dayOfMonth };
            }
        }
    }
    return start;
};

const timexTimeAdd = function (start, duration) {
    if ('hours' in duration) {
        const result = Object.assign({}, start);
        result.hour += duration.hours;
        if (result.hour > 23) {
            const days = Math.floor(result.hour / 24);
            const hour = result.hour % 24;
            result.hour = hour;
            if ('year' in result && 'month' in result && 'dayOfMonth' in result) {
                const d = new Date(result.year, result.month - 1, result.dayOfMonth, 0, 0, 0);
                for (let i=0; i<days; i++) {
                    d.setDate(d.getDate() + 1);
                }
                result.year = d.getFullYear();
                result.month = d.getMonth() + 1;
                result.dayOfMonth = d.getDate();
                return result;
            }
            if ('dayOfWeek' in result) {
                result.dayOfWeek += days;
                return result;
            }
        }
        return result;
    }
    if ('minutes' in duration) {
        const result = Object.assign({}, start);
        result.minute += duration.minutes;
        if (result.minute > 59) {
            result.hour++;
            result.minute = 0;
        }
        return result;
    }
    return start;
};

const timexDateTimeAdd = function (start, duration) {
    return timexTimeAdd(timexDateAdd(start, duration), duration);
};

const expandDateTimeRange = function (timex) {
    const types = ('types' in timex) ? timex.types : timexInference.infer(timex);
    if (types.has('duration')) {
        const start = cloneDateTime(timex);
        const duration = cloneDuration(timex);
        return { start: start, end: timexDateTimeAdd(start, duration), duration: duration };
    }
    else {
        if ('year' in timex) {
            const range = { start: { year: timex.year }, end: {} };
            if ('month' in timex) {
                range.start.month = timex.month;
                range.start.dayOfMonth = 1;
                range.end.year = timex.year;
                range.end.month = timex.month + 1;
                range.end.dayOfMonth = 1;
            }
            else {
                range.start.month = 1;
                range.start.dayOfMonth = 1;
                range.end.year = timex.year + 1;
                range.end.month = 1;
                range.end.dayOfMonth = 1;
            }
            return range;
        }
    }
    return { start: {}, end: {} };
};

const timeAdd = function (start, duration) {
    const hours = duration.hours || 0;
    const minutes = duration.minutes || 0;
    const seconds = duration.seconds || 0;
    return { hour: start.hour + hours, minute: start.minute + minutes, second: start.second + seconds };
};

const expandTimeRange = function (timex) {

    if (!timex.types.has('timerange'))
    {
        throw new exception('argument must be a timerange');
    }

    if (timex.partOfDay !== undefined) {
        switch (timex.partOfDay) {
            case 'DT':
                timex = { hour: 8, minute: 0, second: 0, hours: 10, minutes: 0, seconds: 0 };
                break;
            case 'MO':
                timex = { hour: 8, minute: 0, second: 0, hours: 4, minutes: 0, seconds: 0 };
                break;
            case 'AF':
                timex = { hour: 12, minute: 0, second: 0, hours: 4, minutes: 0, seconds: 0 };
                break;
            case 'EV':
                timex = { hour: 16, minute: 0, second: 0, hours: 4, minutes: 0, seconds: 0 };
                break;
            case 'NI':
                timex = { hour: 20, minute: 0, second: 0, hours: 4, minutes: 0, seconds: 0 };
                break;
            default:
                throw new exception('unrecognized part of day timerange');
        }
    }

    const start = { hour: timex.hour, minute: timex.minute, second: timex.second };
    const duration = cloneDuration(timex);
    return { start: start, end: timeAdd(start, duration), duration: duration };
};

const dateFromTimex = function (timex) {
    const year = 'year' in timex ? timex.year : 2001;
    const month = 'month' in timex ? timex.month - 1 : 0;
    const date = 'dayOfMonth' in timex ? timex.dayOfMonth : 1;
    const hour = 'hour' in timex ? timex.hour : 0;
    const minute = 'minute' in timex ? timex.minute : 0;
    const second = 'second' in timex ? timex.second : 0;
    return new Date(year, month, date, hour, minute, second);
};

const timeFromTimex = function (timex) {
    const hour = timex.hour || 0;
    const minute = timex.minute || 0;
    const second = timex.second || 0;
    return new Time(hour, minute, second);
};

const dateRangeFromTimex = function (timex) {
    const expanded = expandDateTimeRange(timex);
    return { start: dateFromTimex(expanded.start), end: dateFromTimex(expanded.end) };
};

const timeRangeFromTimex = function (timex) {
    const expanded = expandTimeRange(timex);
    return { start: timeFromTimex(expanded.start), end: timeFromTimex(expanded.end) };
};

module.exports = {
    expandDateTimeRange: expandDateTimeRange,
    expandTimeRange: expandTimeRange,
    dateFromTimex: dateFromTimex,
    timeFromTimex: timeFromTimex,
    dateRangeFromTimex: dateRangeFromTimex,
    timeRangeFromTimex: timeRangeFromTimex,
    timexTimeAdd: timexTimeAdd,
    timexDateTimeAdd: timexDateTimeAdd
};

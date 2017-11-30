// Copyright (c) Microsoft Corporation. All rights reserved.

const timexInference = require('./timexInference.js');
const fixedFormatNumber = require('./timexDateHelpers.js').fixedFormatNumber;
const timexHelpers = require('./timexHelpers.js');

const formatDuration = function (timex) {
    if ('years' in timex) {
        return `P${timex.years}Y`;
    }
    if ('months' in timex) {
        return `P${timex.months}M`;
    }
    if ('weeks' in timex) {
        return `P${timex.weeks}W`;
    }
    if ('days' in timex) {
        return `P${timex.days}D`;
    }
    if ('hours' in timex) {
        return `PT${timex.hours}H`;
    }
    if ('minutes' in timex) {
        return `PT${timex.minutes}M`;
    }
    if ('seconds' in timex) {
        return `PT${timex.seconds}S`;
    }
    return '';
};

const formatTime = function (timex) {
    if (timex.minute === 0 && timex.second === 0) {
        return `T${fixedFormatNumber(timex.hour, 2)}`;
    }
    if (timex.second === 0) {
        return `T${fixedFormatNumber(timex.hour, 2)}:${fixedFormatNumber(timex.minute, 2)}`;
    }
    return `T${fixedFormatNumber(timex.hour, 2)}:${fixedFormatNumber(timex.minute, 2)}:${fixedFormatNumber(timex.second, 2)}`;
};

const formatDate = function (timex) {
    if ('year' in timex && 'month' in timex && 'dayOfMonth' in timex) {
        return `${fixedFormatNumber(timex.year, 4)}-${fixedFormatNumber(timex.month, 2)}-${fixedFormatNumber(timex.dayOfMonth, 2)}`;
    }
    if ('month' in timex && 'dayOfMonth' in timex) {
        return `XXXX-${fixedFormatNumber(timex.month, 2)}-${fixedFormatNumber(timex.dayOfMonth, 2)}`;
    }
    if ('dayOfWeek' in timex) {
        return `XXXX-WXX-${timex.dayOfWeek}`;
    }
    return '';
};

const formatDateRange = function (timex) {
    if ('year' in timex && 'weekOfYear' in timex && 'weekend' in timex) {
        return `${fixedFormatNumber(timex.year, 4)}-W${fixedFormatNumber(timex.weekOfYear, 2)}-WE`;
    }
    if ('year' in timex && 'weekOfYear' in timex) {
        return `${fixedFormatNumber(timex.year, 4)}-W${fixedFormatNumber(timex.weekOfYear, 2)}`;
    }
    if ('year' in timex && 'season' in timex) {
        return `${fixedFormatNumber(timex.year, 4)}-${timex.season}`;
    }
    if ('season' in timex) {
        return `${timex.season}`;
    }
    if ('year' in timex && 'month' in timex) {
        return `${fixedFormatNumber(timex.year, 4)}-${fixedFormatNumber(timex.month, 2)}`;
    }
    if ('year' in timex) {
        return `${fixedFormatNumber(timex.year, 4)}`;
    }
    if ('month' in timex && 'weekOfMonth' in timex && 'dayOfWeek' in timex) {
        return `XXXX-${fixedFormatNumber(timex.month, 2)}-WXX-${timex.weekOfMonth}-${timex.dayOfWeek}`;
    }
    if ('month' in timex && 'weekOfMonth' in timex) {
        return `XXXX-${fixedFormatNumber(timex.month, 2)}-WXX-${timex.weekOfMonth}`;
    }
    if ('month' in timex) {
        return `XXXX-${fixedFormatNumber(timex.month, 2)}`;
    }
    return '';
};

const formatTimeRange = function (timex) {
    if ('partOfDay' in timex) {
        return `T${timex.partOfDay}`;
    }
    return '';
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

const format = function(timex) {

    const types = ('types' in timex) ? timex.types : timexInference.infer(timex);

    if (types.has('present')) {
        return 'PRESENT_REF';
    }
    if ((types.has('datetimerange') || types.has('daterange') || types.has('timerange')) && types.has('duration')) {
        const range = timexHelpers.expandDateTimeRange(timex);
        return `(${format(range.start)},${format(range.end)},${format(range.duration)})`;
    }
    if (types.has('datetimerange')) {
        return `${formatDate(timex)}${formatTimeRange(timex)}`;
    }
    if (types.has('daterange')) {
        return `${formatDateRange(timex)}`;
    }
    if (types.has('timerange')) {
        return `${formatTimeRange(timex)}`;
    }
    if (types.has('datetime')) {
        return `${formatDate(timex)}${formatTime(timex)}`;
    }
    if (types.has('duration')) {
        return `${formatDuration(timex)}`;
    }
    if (types.has('date')) {
        return `${formatDate(timex)}`;
    }
    if (types.has('time')) {
        return `${formatTime(timex)}`;
    }
    return '';
};

module.exports.format = format;

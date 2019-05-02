// Copyright (c) Microsoft Corporation. All rights reserved.

const TimexProperty = require('./timexProperty.js').TimexProperty;
const timexValue = require('./timexValue.js');
const timexInference = require('./timexInference.js');
const timexHelpers = require('./timexHelpers.js');

const dateOfLastDay = require('./timexDateHelpers.js').dateOfLastDay;
const dateOfNextDay = require('./timexDateHelpers.js').dateOfNextDay;

const resolveDefiniteTime = function (timex, date) {
    return [{ timex: timex.timex, type: 'datetime', value: `${timexValue.dateValue(timex)} ${timexValue.timeValue(timex)}` }];
};

const resolveDefinite = function (timex, date) {
    return [{ timex: timex.timex, type: 'date', value: timexValue.dateValue(timex) }];
};

const lastDateValue = function (timex, date) {
    if (timex.month !== undefined && timex.dayOfMonth !== undefined) {
        return timexValue.dateValue({ year: date.getFullYear() - 1, month: timex.month, dayOfMonth: timex.dayOfMonth });
    }
    if (timex.dayOfWeek !== undefined) {
        const day = timex.dayOfWeek === 7 ? 0 : timex.dayOfWeek;
        const result = dateOfLastDay(day, date);
        return timexValue.dateValue({ year: result.getFullYear(), month: result.getMonth() + 1, dayOfMonth: result.getDate() });
    }
};

const nextDateValue = function (timex, date) {
    if (timex.month !== undefined && timex.dayOfMonth !== undefined) {
        return timexValue.dateValue({ year: date.getFullYear(), month: timex.month, dayOfMonth: timex.dayOfMonth });
    }
    if (timex.dayOfWeek !== undefined) {
        const day = timex.dayOfWeek === 7 ? 0 : timex.dayOfWeek;
        const result = dateOfNextDay(day, date);
        return timexValue.dateValue({ year: result.getFullYear(), month: result.getMonth() + 1, dayOfMonth: result.getDate() });
    }
};

const resolveDate = function (timex, date) {
    return [
        { timex: timex.timex, type: 'date', value: lastDateValue(timex, date) },
        { timex: timex.timex, type: 'date', value: nextDateValue(timex, date) }
    ];
};

const resolveTime = function (timex) {
    return [ { timex: timex.timex, type: 'time', value: timexValue.timeValue(timex) } ];
};

const resolveDuration = function (timex) {
    return [ { timex: timex.timex, type: 'duration', value: timexValue.durationValue(timex) }];
};

const weekDateRange = function (year, weekOfYear) {
    var dateInWeek = new Date(year, 0, 1);
    dateInWeek.setDate(dateInWeek.getDate() + ((weekOfYear - 1) * 7));

    var start = dateOfLastDay(1, dateInWeek);
    dateInWeek.setDate(dateInWeek.getDate() + 7);
    var end = dateOfLastDay(1, dateInWeek);

    return {
        start: timexValue.dateValue({ year: start.getFullYear(), month: start.getMonth() + 1, dayOfMonth: start.getDate() }),
        end: timexValue.dateValue({ year: end.getFullYear(), month: end.getMonth() + 1, dayOfMonth: end.getDate() })
    }
}

const monthDateRange = function (year, month) {
    return {
        start: timexValue.dateValue({ year: year, month: month, dayOfMonth: 1 }),
        end: timexValue.dateValue({ year: year, month: month + 1, dayOfMonth: 1 })
    };
};

const yearDateRange = function (year) {
    return {
        start: timexValue.dateValue({ year: year, month: 1, dayOfMonth: 1 }),
        end: timexValue.dateValue({ year: year + 1, month: 1, dayOfMonth: 1 })
    };
}

const resolveDateRange = function (timex, date) {
    if ('season' in timex) {
        return [{ timex: timex.timex, type: 'daterange', value: 'not resolved' }];
    }
    else {
        if (timex.year !== undefined && timex.month !== undefined) {
            const dateRange = monthDateRange(timex.year, timex.month);
            return [{ timex: timex.timex, type: 'daterange', start: dateRange.start, end: dateRange.end }];
        }
        if (timex.year !== undefined && timex.weekOfYear !== undefined) {
            const dateRange = weekDateRange(timex.year, timex.weekOfYear);
            return [{ timex: timex.timex, type: 'daterange', start: dateRange.start, end: dateRange.end }];
        }
        if (timex.month !== undefined) {
            const y = date.getFullYear();
            const lastYearDateRange = monthDateRange(y - 1, timex.month);
            const thisYearDateRange = monthDateRange(y, timex.month);
            
            return [
                { timex: timex.timex, type: 'daterange', start: lastYearDateRange.start, end: lastYearDateRange.end },
                { timex: timex.timex, type: 'daterange', start: thisYearDateRange.start, end: thisYearDateRange.end }
            ];
        }
        if (timex.year !== undefined) {
            const dateRange = yearDateRange(timex.year);
            return [{ timex: timex.timex, type: 'daterange', start: dateRange.start, end: dateRange.end }];
        }
        return [];
    }
};

const partOfDayTimeRange = function (timex) {
    switch (timex.partOfDay) {
        case 'MO': return { start: '08:00:00', end: '12:00:00' };
        case 'AF': return { start: '12:00:00', end: '16:00:00' };
        case 'EV': return { start: '16:00:00', end: '20:00:00' };
        case 'NI': return { start: '20:00:00', end: '24:00:00' };
    }
    return { start: 'not resolved', end: 'not resolved' };
};

const resolveTimeRange = function (timex, date) {
    if ('partOfDay' in timex) {
        const range = partOfDayTimeRange(timex);
        return [{ timex: timex.timex, type: 'timerange', start: range.start, end: range.end }];
    }
    else {
        const range = timexHelpers.expandTimeRange(timex);
        return [{
            timex: timex.timex,
            type: 'timerange',
            start: timexValue.timeValue(range.start),
            end: timexValue.timeValue(range.end)
        }];
    }
};

const resolveDateTime = function (timex, date) {
    const resolvedDates = resolveDate(timex, date);
    for (const resolved of resolvedDates) {
        resolved.type = 'datetime';
        resolved.value = `${resolved.value} ${timexValue.timeValue(timex)}`; 

    }
    return resolvedDates;
};

const resolveDateTimeRange = function (timex) {
    if ('partOfDay' in timex) {
        const date = timexValue.dateValue(timex);
        const timeRange = partOfDayTimeRange(timex);
        return [{
            timex: timex.timex,
            type: 'datetimerange',
            start: `${date} ${timeRange.start}`,
            end: `${date} ${timeRange.end}`
        }];
    }
    else {
        const range = timexHelpers.expandDateTimeRange(timex);
        return [{ 
            timex: timex.timex,
            type: 'datetimerange', 
            start: `${timexValue.dateValue(range.start)} ${timexValue.timeValue(range.start)}`,
            end: `${timexValue.dateValue(range.end)} ${timexValue.timeValue(range.end)}`
        }];
    }
};

const resolveDefiniteDateRange = function (timex) {
    var range = timexHelpers.expandDateTimeRange(timex);
    return [{ 
        timex: timex.timex,
        type: 'daterange', 
        start: `${timexValue.dateValue(range.start)}`,
        end: `${timexValue.dateValue(range.end)}`
    }];
};

const resolveTimex = function (timex, date) {

    const types = ('types' in timex) ? timex.types : timexInference.infer(timex);

    if (types.has('datetimerange')) {
        return resolveDateTimeRange(timex);
    }
    if (types.has('definite') && types.has('time')) {
        return resolveDefiniteTime(timex, date);
    }
    if (types.has('definite') && types.has('daterange')) {
        return resolveDefiniteDateRange(timex, date);
    }
    if (types.has('definite')) {
        return resolveDefinite(timex, date);
    }
    if (types.has('daterange')) {
        return resolveDateRange(timex, date);
    }
    if (types.has('timerange')) {
        return resolveTimeRange(timex);
    }
    if (types.has('datetime')) {
        return resolveDateTime(timex, date);
    }
    if (types.has('duration')) {
        return resolveDuration(timex);
    }
    if (types.has('date')) {
        return resolveDate(timex, date);
    }
    if (types.has('time')) {
        return resolveTime(timex);
    }
    return [];
};

const resolve = function (timexArray, date) {
    const resolution = { values: [] };
    for (const timex of timexArray) {
        const t = new TimexProperty(timex);
        const r = resolveTimex(t, date);
        Array.prototype.push.apply(resolution.values, r);        
    }
    return resolution;
};

module.exports = {
    resolve: resolve
};

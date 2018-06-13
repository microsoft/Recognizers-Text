// Copyright (c) Microsoft Corporation. All rights reserved.

const timexHelpers = require('./timexHelpers.js');
const timexDateHelpers = require('./timexDateHelpers.js');
const timexConstraintsHelper = require('./timexConstraintsHelper.js');
const Time = require('./time.js').Time;
const TimexProperty = require('./timexProperty.js').TimexProperty;

const resolveDefiniteAgainstConstraint = function (timex, constraint) {
    const timexDate = timexHelpers.dateFromTimex(timex);
    if (timexDate.getTime() >= constraint.start.getTime() && timexDate.getTime() < constraint.end.getTime()) {
        return [ timex.timex ];
    }
    return [];
};

const resolveDefinite = function (timex, constraints) {
    const result = [];
    for (const constraint of constraints) {
        Array.prototype.push.apply(result, resolveDefiniteAgainstConstraint(timex, constraint));
    }
    return result;
};

const resolveDateAgainstConstraint = function (timex, constraint) {
    if ('month' in timex && 'dayOfMonth' in timex) {
        const result = [];
        for (let year = constraint.start.getFullYear(); year <= constraint.end.getFullYear(); year++) {
            const r = resolveDefiniteAgainstConstraint(new TimexProperty(Object.assign({}, timex, { year: year })), constraint);
            if (r.length > 0) {
                result.push(r[0]);
            }
        }
        return result;
    }
    if ('dayOfWeek' in timex) {
        const day = timex.dayOfWeek === 7 ? 0 : timex.dayOfWeek;
        const dates = timexDateHelpers.datesMatchingDay(day, constraint.start, constraint.end);
        const result = [];
        for (const d of dates) {
            const t = Object.assign({}, timex);
            delete t.dayOfWeek;
            const r = new TimexProperty(Object.assign({}, t, { year: d.getFullYear(), month: d.getMonth() + 1, dayOfMonth: d.getDate() }));
            result.push(r.timex);
        }
        return result;
    }
    return [];
};

const resolveDate = function (timex, constraints) {
    const result = [];
    for (const constraint of constraints) {
        Array.prototype.push.apply(result, resolveDateAgainstConstraint(timex, constraint));
    }
    return result;
};

const resolveTimeAgainstConstraint = function (timex, constraint) {
    const t = new Time(timex.hour, timex.minute, timex.second);
    if (t.getTime() >= constraint.start.getTime() && t.getTime() < constraint.end.getTime()) {
        return [ timex.timex ];
    }
    return [];
};

const resolveTime = function (timex, constraints) {
    const result = [];
    for (const constraint of constraints) {
        Array.prototype.push.apply(result, resolveTimeAgainstConstraint(timex, constraint));
    }
    return result;
};

const removeDuplicates = function (array) {
    var seen = new Set();
    return array.filter(item => { return seen.has(item) ? false : seen.add(item); });
};

const resolveByDateRangeConstraints = function (candidates, timexConstraints) {
    
    const dateRangeConstraints = timexConstraints
        .filter((timex) => {
            return timex.types.has('daterange'); })
        .map((timex) => {
            return timexHelpers.dateRangeFromTimex(timex);
        });
    const collapsedDateRanges = timexConstraintsHelper.collapse(dateRangeConstraints, Date);

    if (collapsedDateRanges.length === 0) {
        return candidates;
    }

    const resolution = [];
    for (const timex of candidates) {
        const r = resolveDate(new TimexProperty(timex), collapsedDateRanges);
        Array.prototype.push.apply(resolution, r);        
    }

    return removeDuplicates(resolution);
};

const resolveByTimeConstraints = function (candidates, timexConstraints) {
    
    const times = timexConstraints
        .filter((timex) => {
            return timex.types.has('time'); })
        .map((timex) => {
            return timexHelpers.timeFromTimex(timex);
        });

    if (times.length === 0) {
        return candidates;
    }

    const resolution = [];
    for (const timex of candidates.map(t => new TimexProperty(t))) {
        if (timex.types.has('date') && !timex.types.has('time')) {
            for (const time of times) {
                timex.hour = time.hour;
                timex.minute = time.minute;
                timex.second = time.second;
                resolution.push(timex.timex);
            }
        }
        else {
            resolution.push(timex.timex);
        }
    }
    return removeDuplicates(resolution);
};

const resolveByTimeRangeConstraints = function (candidates, timexConstraints) {

    const timeRangeConstraints = timexConstraints
        .filter((timex) => {
            return timex.types.has('timerange'); })
        .map((timex) => {
            return timexHelpers.timeRangeFromTimex(timex);
        });
    const collapsedTimeRanges = timexConstraintsHelper.collapse(timeRangeConstraints, Time);

    if (collapsedTimeRanges.length === 0) {
        return candidates;
    }

    const resolution = [];
    for (const timex of candidates) {
        const t = new TimexProperty(timex);
        if (t.types.has('timerange')) {
            const r = resolveTimeRange(t, collapsedTimeRanges);
            Array.prototype.push.apply(resolution, r);
        }
        else if (t.types.has('time')) {
            const r = resolveTime(t, collapsedTimeRanges);
            Array.prototype.push.apply(resolution, r);
        }
    }

    return removeDuplicates(resolution);
};

const resolveTimeRange = function (timex, constraints) {

    const candidate = timexHelpers.timeRangeFromTimex(timex);

    const result = [];
    for (const constraint of constraints) {

        if (timexConstraintsHelper.isOverlapping(candidate, constraint)) {

            const start = Math.max(candidate.start.getTime(), constraint.start.getTime());
            const time = new Time(start);

            // TODO: refer to comments in C# - consider first classing this clone/overwrite behavior
            const resolved = new TimexProperty(timex.timex);
            delete resolved.partOfDay;
            delete resolved.seconds;
            delete resolved.minutes;
            delete resolved.hours;
            resolved.second = time.second;
            resolved.minute = time.minute;
            resolved.hour = time.hour;

            result.push(resolved.timex);
        }
    }
    return result;
};

const resolveDuration = function (candidate, constraints) {
    const results = [];
    for (const constraint of constraints) {
        if (constraint.types.has('datetime')) {
            results.push(new TimexProperty(timexHelpers.timexDateTimeAdd(constraint, candidate)));
        }
        else if (constraint.types.has('time')) {
            results.push(new TimexProperty(timexHelpers.timexTimeAdd(constraint, candidate)));
        }
    }
    return results;
};

const resolveDurations = function (candidates, constraints) {
    const results = [];
    for (const candidate of candidates) {
        const timex = new TimexProperty(candidate);
        if (timex.types.has('duration')) {
            const r = resolveDuration(timex, constraints);
            for (const resolved of r) {
                results.push(resolved.timex);
            }
        }
        else {
            results.push(candidate);
        }
    }
    return results;
};

const evaluate = function (candidates, constraints) {
    const timexConstraints = constraints.map((x) => { return new TimexProperty(x); });
    const candidatesWithDurationsResolved = resolveDurations(candidates, timexConstraints);
    const candidatesAccordingToDate = resolveByDateRangeConstraints(candidatesWithDurationsResolved, timexConstraints);
    const candidatesWithAddedTime = resolveByTimeConstraints(candidatesAccordingToDate, timexConstraints);
    const candidatesFilteredByTime = resolveByTimeRangeConstraints(candidatesWithAddedTime, timexConstraints);
    const timexResults = candidatesFilteredByTime.map((x) => { return new TimexProperty(x); });
    return timexResults;
};

module.exports = {
    evaluate: evaluate
};

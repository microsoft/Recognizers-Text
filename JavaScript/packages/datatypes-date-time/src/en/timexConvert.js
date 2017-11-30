// Copyright (c) Microsoft Corporation. All rights reserved.

const timexConstants = require('./timexConstants.js');
const timexInference = require('../timexInference.js');

const convertDate = function(timex) {
    if ('dayOfWeek' in timex) {
        return timexConstants.days[timex.dayOfWeek - 1];
    }
    const month = timexConstants.months[timex.month - 1];
    const date = timex.dayOfMonth.toString();
    const abbreviation = timexConstants.dateAbbreviation[date.slice(-1)];
    if ('year' in timex) {
        return `${date}${abbreviation} ${month} ${timex.year}`.trim();
    }
    return `${date}${abbreviation} ${month}`;
};

const convertTime = function(timex) {
    if (timex.hour === 0 && timex.minute === 0 && timex.second === 0) {
        return 'midnight';
    }
    if (timex.hour === 12 && timex.minute === 0 && timex.second === 0) {
        return 'midday';
    }
    const pad = function (s) { return (s.length === 1) ? '0' + s : s; };
    const hour = (timex.hour === 0) ? '12' : (timex.hour > 12) ? (timex.hour - 12).toString() : timex.hour.toString();
    const minute = (timex.minute === 0 && timex.second === 0) ? '' : ':' + pad(timex.minute.toString());
    const second = (timex.second === 0) ? '' : ':' + pad(timex.second.toString());
    const period = timex.hour < 12 ? 'AM' : 'PM';
    return `${hour}${minute}${second}${period}`;
};

const convertDurationPropertyToString = function (timex, property, includeSingleCount) {
    const propertyName = property + 's';
    const value = timex[propertyName];
    if (value !== undefined) {
        if (value === 1) {
            return includeSingleCount ? '1 ' + property : property;
        }
        else {
            return `${value} ${property}s`;
        }
    }
    return false;
};

const convertTimexDurationToString = function (timex, includeSingleCount) {
    return convertDurationPropertyToString(timex, 'year', includeSingleCount)
        || convertDurationPropertyToString(timex, 'month', includeSingleCount)
        || convertDurationPropertyToString(timex, 'week', includeSingleCount)
        || convertDurationPropertyToString(timex, 'day', includeSingleCount)
        || convertDurationPropertyToString(timex, 'hour', includeSingleCount)
        || convertDurationPropertyToString(timex, 'minute', includeSingleCount)
        || convertDurationPropertyToString(timex, 'second', includeSingleCount);
};

const convertDuration = function(timex) {
    return convertTimexDurationToString(timex, true);
};

const convertDateRange = function(timex) {
    const season = ('season' in timex) ? timexConstants.seasons[timex.season] : '';
    const year = ('year' in timex) ? timex.year.toString() : '';
    if ('weekOfYear' in timex) {
        if (timex.weekend) {
            return '';
        }
        else {
            return '';
        }
    }
    if ('month' in timex) {
        const month = `${timexConstants.months[timex.month - 1]}`;
        if ('weekOfMonth' in timex) {
            return `${timexConstants.weeks[timex.weekOfMonth - 1]} week of ${month}`;
        }
        else {
            return `${month} ${year}`.trim();
        }
    }
    return `${season} ${year}`.trim();
};

const convertTimeRange = function(timex) {
    return timexConstants.dayParts[timex.partOfDay];
};

const convertDateTime = function(timex) {
    return `${convertTime(timex)} ${convertDate(timex)}`;
};

const convertDateTimeRange = function(timex) {
    if (timex.types.has('timerange')) {
        return `${convertDate(timex)} ${convertTimeRange(timex)}`;
    }
    // date + time + duration
    // - OR - 
    // date + duration
    return '';
};

const convertTimexToString = function (timex) {

    const types = ('types' in timex) ? timex.types : timexInference.infer(timex);

    if (types.has('present')) {
        return 'now';
    }
    if (types.has('datetimerange')) {
        return convertDateTimeRange(timex);
    }
    if (types.has('daterange')) {
        return convertDateRange(timex);
    }
    if (types.has('duration')) {
        return convertDuration(timex);
    }
    if (types.has('timerange')) {
        return convertTimeRange(timex);
    }

    // TODO: where appropriate delegate most the formatting delegate to Date.toLocaleString(options)
    if (types.has('datetime')) {
        return convertDateTime(timex);
    }
    if (types.has('date')) {
        return convertDate(timex);
    }
    if (types.has('time')) {
        return convertTime(timex);
    }
    return '';
};

const convertTimexSetToString = function(timexSet) {

    const timex = timexSet.timex;
    if (timex.types.has('duration')) {
        return `every ${convertTimexDurationToString(timex, false)}`;
    }
    else {
        return `every ${convertTimexToString(timex)}`;
    }
};

module.exports = {
    convertDate: convertDate,
    convertTime: convertTime,
    convertTimexToString: convertTimexToString,
    convertTimexSetToString: convertTimexSetToString
};

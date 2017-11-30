// Copyright (c) Microsoft Corporation. All rights reserved.

const timexDateHelpers = require('../timexDateHelpers.js');
const timexInference = require('../timexInference.js');
const timexConstants = require('./timexConstants.js');
const timexConvert = require('./timexConvert.js');

const getDateDay = function (day) {
    const index = (day === 0) ? 6 : day - 1;
    return timexConstants.days[index];
};

const convertDate = function(timex, date) {
    if ('year' in timex && 'month' in timex && 'dayOfMonth' in timex) {
        const timexDate = new Date(timex.year, timex.month - 1, timex.dayOfMonth);

        if (timexDateHelpers.datePartEquals(timexDate, date)) {
            return 'today';
        }
        const tomorrow = timexDateHelpers.tomorrow(date);
        if (timexDateHelpers.datePartEquals(timexDate, tomorrow)) {
            return 'tomorrow';
        }
        const yesterday = timexDateHelpers.yesterday(date);
        if (timexDateHelpers.datePartEquals(timexDate, yesterday)) {
            return 'yesterday';
        }
        if (timexDateHelpers.isThisWeek(timexDate, date)) {
            return `this ${getDateDay(timexDate.getDay())}`;
        }
        if (timexDateHelpers.isNextWeek(timexDate, date)) {
            return `next ${getDateDay(timexDate.getDay())}`;
        }
        if (timexDateHelpers.isLastWeek(timexDate, date)) {
            return `last ${getDateDay(timexDate.getDay())}`;
        }
    }
    return timexConvert.convertDate(timex);
};

const convertDateTime = function (timex, date) {
    return `${convertDate(timex, date)} ${timexConvert.convertTime(timex)}`;
};

const convertDateRange = function(timex, date) {
    if ('year' in timex) {
        const year = date.getFullYear();
        if (timex.year === year) {
            if ('weekOfYear' in timex) {
                const thisWeek = timexDateHelpers.weekOfYear(date);
                if (thisWeek === timex.weekOfYear) {
                    return timex.weekend ? 'this weekend' : 'this week';
                }
                if (thisWeek === timex.weekOfYear + 1) {
                    return timex.weekend ? 'last weekend' : 'last week';
                }
                if (thisWeek === timex.weekOfYear - 1) {
                    return timex.weekend ? 'next weekend' : 'next week';
                }
            }
            if ('month' in timex) {
                const isoMonth = date.getMonth() + 1;
                if (timex.month === isoMonth) {
                    return 'this month';
                }
                if (timex.month === isoMonth + 1) {
                    return 'next month';
                }
                if (timex.month === isoMonth - 1) {
                    return 'last month';
                }
            }
            return ('season' in timex) ? `this ${timexConstants.seasons[timex.season]}` : 'this year';
        }
        if (timex.year === year + 1) {
            return ('season' in timex) ? `next ${timexConstants.seasons[timex.season]}` : 'next year';
        }
        if (timex.year === year - 1) {
            return ('season' in timex) ? `last ${timexConstants.seasons[timex.season]}` : 'last year';
        }
    }
    return '';
};

const convertDateTimeRange = function(timex, date) {
    if ('year' in timex && 'month' in timex && 'dayOfMonth' in timex) {
        const timexDate = new Date(timex.year, timex.month - 1, timex.dayOfMonth);

        if ('partOfDay' in timex) {
            if (timexDateHelpers.datePartEquals(timexDate, date)) {
                if (timex.partOfDay === 'NI') {
                    return 'tonight';
                }
                else {
                    return `this ${timexConstants.dayParts[timex.partOfDay]}`;
                }
            }
            const tomorrow = timexDateHelpers.tomorrow(date);
            if (timexDateHelpers.datePartEquals(timexDate, tomorrow)) {
                return `tomorrow ${timexConstants.dayParts[timex.partOfDay]}`;
            }
            const yesterday = timexDateHelpers.yesterday(date);
            if (timexDateHelpers.datePartEquals(timexDate, yesterday)) {
                return `yesterday ${timexConstants.dayParts[timex.partOfDay]}`;
            }

            if (timexDateHelpers.isNextWeek(timexDate, date)) {
                return `next ${getDateDay(timexDate.getDay())} ${timexConstants.dayParts[timex.partOfDay]}`;
            }

            if (timexDateHelpers.isLastWeek(timexDate, date)) {
                return `last ${getDateDay(timexDate.getDay())} ${timexConstants.dayParts[timex.partOfDay]}`;
            }
        }
    }
    return '';
};

const convertTimexToStringRelative = function (timex, date) {

    const types = ('types' in timex) ? timex.types : timexInference.infer(timex);
    
    if (types.has('datetimerange')) {
        return convertDateTimeRange(timex, date);
    }
    if (types.has('daterange')) {
        return convertDateRange(timex, date);
    }
    if (types.has('datetime')) {
        return convertDateTime(timex, date);
    }
    if (types.has('date')) {
        return convertDate(timex, date);
    }

    return timexConvert.convertTimexToString(timex);
};

module.exports.convertTimexToStringRelative = convertTimexToStringRelative;

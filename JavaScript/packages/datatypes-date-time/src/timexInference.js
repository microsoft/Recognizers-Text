// Copyright (c) Microsoft Corporation. All rights reserved.

const isPresent = function (obj) {
    return obj.now === true;
};

const isDuration = function (obj) {
    return 'years' in obj || 'months' in obj || 'weeks' in obj || 'days' in obj 
        || 'hours' in obj || 'minutes' in obj || 'seconds' in obj;
};

const isTime = function (obj) {
    return 'hour' in obj && 'minute' in obj && 'second' in obj;
};

const isDate = function (obj) {
    return ('month' in obj && 'dayOfMonth' in obj) || 'dayOfWeek' in obj;
};

const isTimeRange = function (obj) {
    return 'partOfDay' in obj;
};

const isDateRange = function (obj) {
    return ('year' in obj && !('dayOfMonth' in obj))
        || ('year' in obj && 'month' in obj && !('dayOfMonth' in obj))
        || ('month' in obj && !('dayOfMonth' in obj))
        || 'season' in obj
        || 'weekOfYear' in obj
        || 'weekOfMonth' in obj;
};

const isDefinite = function (obj) {
    return 'year' in obj &&  'month' in obj && 'dayOfMonth' in obj;
};

const infer = function (obj) {
    const types = new Set();
    if (isPresent(obj)) {
        types.add('present');
    }
    if (isDefinite(obj)) {
        types.add('definite');
    }
    if (isDate(obj)) {
        types.add('date');
    }
    if (isDateRange(obj)) {
        types.add('daterange');
    }
    if (isDuration(obj)) {
        types.add('duration');
    }
    if (isTime(obj)) {
        types.add('time');
    }
    if (isTimeRange(obj)) {
        types.add('timerange');
    }
    if (types.has('present')) {
        types.add('date');
        types.add('time');
    }
    if (types.has('time') && types.has('duration')) {
        types.add('timerange');
    }
    if (types.has('date') && types.has('time')) {
        types.add('datetime');
    }
    if (types.has('date') && types.has('duration')) {
        types.add('daterange');
    }
    if (types.has('datetime') && types.has('duration')) {
        types.add('datetimerange');
    }
    if (types.has('date') && types.has('timerange')) {
        types.add('datetimerange');
    }
    return types;
};

module.exports.infer = infer;

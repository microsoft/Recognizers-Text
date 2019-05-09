// Copyright (c) Microsoft Corporation. All rights reserved.

module.exports = {
    Time: require('./src/time.js').Time,
    TimexProperty: require('./src/timexProperty.js').TimexProperty,
    TimexSet: require('./src/timexSet.js').TimexSet,
    creator: require('./src/timexCreator.js'),
    resolver: require('./src/timexRangeResolver.js'),
    valueResolver: require('./src/timexResolver.js')
};


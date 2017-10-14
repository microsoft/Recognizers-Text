var _ = require('lodash');

// Helpers
module.exports.toObject = function toObject(map) {
    if (!map) return undefined;
    var keys = Array.from(map.keys());
    var values = Array.from(map.values()).map(asString);
    return _.zipObject(keys, values);
}

function asString(o) {
    if (!o) return o;

    if (_.isNumber(o)) {
        return o.toString();
    }

    if (_.isDate(o)) {
        var isoDate = new Date(o.getTime() - o.getTimezoneOffset() * 60000).toISOString();
        var parts = isoDate.split('T');
        var time = parts[1].split('.')[0].replace('00:00:00', '');
        return [parts[0], time].join(' ').trim();
    }


    // JS min Date is 1901, while .NET is 0001
    if (o === '1901-01-01') {
        return '0001-01-01';
    }

    return o;
}
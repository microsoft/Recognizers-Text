// Copyright (c) Microsoft Corporation. All rights reserved.

const isOverlapping = function (r1, r2) {
    return r1.end.getTime() > r2.start.getTime() && r1.start.getTime() <= r2.start.getTime()
        || r1.start.getTime() < r2.end.getTime() && r1.start.getTime() >= r2.start.getTime();
};

const collapseOverlapping = function (r1, r2, T) {
    return {
        start: new T(Math.max(r1.start.getTime(), r2.start.getTime())),
        end: new T(Math.min(r1.end.getTime(), r2.end.getTime()))
    };
};

const innerCollapse = function (ranges, T) {
    if (ranges.length === 1) {
        return false;
    }
    for (let i=0; i<ranges.length; i++) {
        const r1 = ranges[i];
        for (let j=i+1; j<ranges.length; j++) {
            const r2 = ranges[j];
            if (isOverlapping(r1, r2)) {
                ranges.splice(i, 1);
                ranges.splice(j - 1, 1);
                ranges.push(collapseOverlapping(r1, r2, T));
                return true;
            }
        }
    }
    return false;
};

const collapse = function (ranges, T) {
    const r = ranges.slice(0);
    while (innerCollapse(r, T))
        ;
    r.sort((a, b) => a.start.getTime() - b.start.getTime());
    return r;
};

module.exports = {
    collapse: collapse,
    isOverlapping: isOverlapping
};

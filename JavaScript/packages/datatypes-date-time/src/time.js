// Copyright (c) Microsoft Corporation. All rights reserved.

class Time {
    constructor(hour, minute, second) {
        if (arguments.length === 1) {
            this.hour = Math.floor(hour / 3600000);
            this.minute = Math.floor((hour - (this.hour * 3600000)) / 60000);
            this.second = (hour - (this.hour * 3600000) - (this.minute * 60000)) / 1000;
        }
        else {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }
    }

    getTime () {
        return (this.second * 1000) + (this.minute * 60000) + (this.hour * 3600000);
    }
}

module.exports.Time = Time;
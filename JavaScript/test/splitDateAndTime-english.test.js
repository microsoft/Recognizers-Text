
var describe = require('ava-spec').describe;
var Culture = require('../compiled/culture').Culture;
var Constants = require('../compiled/dateTime/constants').Constants;
var DateTimeOptions = require("../compiled/dateTime/baseMerged").DateTimeOptions;
var DateTimeRecognizer = require('../compiled/dateTime/dateTimeRecognizer').default;

describe('Split Date And Time Count .', it => {
    let model = DateTimeRecognizer.getSingleCultureInstance(Culture.English, DateTimeOptions.SplitDateAndTime).getDateTimeModel();
    let referenceDate = new Date(2016, 10, 7);

    basicTestCount(it, model, referenceDate, "schedule a meeting tomorrow from 5pm to 7pm", 3);
    basicTestCount(it, model, referenceDate, "schedule a meeting today from 5pm to 7pm", 3);
    basicTestCount(it, model, referenceDate, "schedule a meeting next Monday from 5pm to 7pm", 3);

    basicTestCount(it, model, referenceDate, "schedule a meeting from 5pm to 7pm tomorrow", 3);
    basicTestCount(it, model, referenceDate, "schedule a meeting from 5pm to 7pm today", 3);
    basicTestCount(it, model, referenceDate, "schedule a meeting from 5pm to 7pm next Monday", 3);

    //from 5 to 7pm is a time period which is extracted by a whole regex but not merge of two time points
    basicTestCount(it, model, referenceDate, "schedule a meeting tomorrow from 5 to 7pm", 2);

    basicTestCount(it, model, referenceDate, "schedule a meeting from Sep.1st to Sep.5th", 2);
    basicTestCount(it, model, referenceDate, "schedule a meeting from July the 5th to July the 8th", 2);

    basicTestCount(it, model, referenceDate, "schedule a meeting from 5:30 to 7:00", 2);
    basicTestCount(it, model, referenceDate, "schedule a meeting from 5pm to 7pm", 2);
    basicTestCount(it, model, referenceDate, "schedule a meeting from 5am to 7pm", 2);

    basicTestCount(it, model, referenceDate, "schedule a meeting 2 hours later", 1);
    basicTestCount(it, model, referenceDate, "schedule a meeting 2 days later", 1);
    basicTestCount(it, model, referenceDate, "I had 2 minutes ago", 1);

    basicTestCount(it, model, referenceDate, "schedule a meeting tomorrow at 7pm", 2);
    basicTestCount(it, model, referenceDate, "schedule a meeting tomorrow morning at 7pm", 2);
});

describe('Split Date And Time Type Name .', it => {
    let model = DateTimeRecognizer.getSingleCultureInstance(Culture.English, DateTimeOptions.SplitDateAndTime).getDateTimeModel();
    let referenceDate = new Date(2016, 10, 7);
    
    basicTestType(it, model, referenceDate,"I'll be out next hour", Constants.SYS_DATETIME_DURATION);
    basicTestType(it, model, referenceDate,"I'll be out next 5 minutes", Constants.SYS_DATETIME_DURATION);
    basicTestType(it, model, referenceDate,"I'll be out next 3 days", Constants.SYS_DATETIME_DURATION);
    basicTestType(it, model, referenceDate,"schedule a meeting now", Constants.SYS_DATETIME_TIME);
    basicTestType(it, model, referenceDate,"schedule a meeting tongiht at 7", Constants.SYS_DATETIME_TIME);
    basicTestType(it, model, referenceDate,"schedule a meeting tongiht at 7pm", Constants.SYS_DATETIME_TIME);
    basicTestType(it, model, referenceDate,"schedule a meeting 2 hours later", Constants.SYS_DATETIME_DURATION);
});

function basicTestCount(it, model, referenceDate, text, numberOfEntities) {
    it(text, t => {
        let results = model.parse(text, referenceDate);
        t.is(numberOfEntities, results.length);
    });
}

function basicTestType(it, model, referenceDate, text, typeName) {
    it(text, t => {
        let results = model.parse(text, referenceDate);
        t.is(1, results.length);
        t.is(typeName, results[0].typeName.substr('datetimeV2.'.length));
    });
}


var describe = require('ava-spec').describe;
var Culture = require('../compiled/culture').Culture;
var Constants = require('../compiled/dateTime/constants').Constants;
var DateTimeRecognizer = require('../compiled/dateTime/dateTimeRecognizer').default;

describe('Single Culture Date Extract', it => {
    let model = DateTimeRecognizer.getSingleCultureInstance(Culture.English).getDateTimeModel();

    basicTest(it, model, "I'll go back now", "now");
});

function basicTest(it, model, text, expected) {
    it(text, t => {
        let pr = model.parse(text);
        t.is(1, pr.length);
        t.is(expected, pr[0].text);
        
        let values = pr[0].resolution.get("values");
        t.is(Constants.SYS_DATETIME_DATETIME, values[0].get("type"));
    });
}

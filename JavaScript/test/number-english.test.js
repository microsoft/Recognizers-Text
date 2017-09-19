var NumberRecognizer = require('../compiled/number/numberRecognizer').default;
var Culture = require('../compiled/culture').Culture;
var CultureInfo = require('../compiled/culture').CultureInfo;
var BigNumber = require('bignumber.js');
var describe = require('ava-spec').describe;

var EnglishCultureInfo = new CultureInfo(Culture.English);

describe('Ordinal Model .', it => {
    let model = NumberRecognizer.instance.getOrdinalModel(Culture.English, false);

    basicTest(it, model, "three trillionth", "3000000000000")

    multiTest(it, model, "a trillionth", 0) // TODO: review!

    basicTest(it, model, "a hundred trillionth", "100000000000000")

    basicTest(it, model, "11th", "11")

    basicTest(it, model, "21st", "21")

    basicTest(it, model, "30th", "30")

    basicTest(it, model, "2nd", "2")

    basicTest(it, model, "eleventh", "11")

    basicTest(it, model, "twentieth", "20")

    basicTest(it, model, "twenty-fifth", "25")

    basicTest(it, model, "twenty-first", "21")

    basicTest(it, model, "one hundred twenty fifth", "125")

    basicTest(it, model, "one hundred twenty-fifth", "125")

    basicTest(it, model, "trillionth", "1000000000000")

    basicTest(
        it,
        model,
        "twenty-one trillion and three hundred twenty second",
        "21000000000322"
    )

    basicTest(it, model, "two hundredth", "200")
});

describe('Number Model .', it => {
    let model = NumberRecognizer.instance.getNumberModel(Culture.English, false);

    wrappedTest(it, model, "192.", "192", "192")

    // TODO: Review, "DoubleDecimalPointRegex" Regex should not match parts of the IP address
    // multiTest(it, model, "192.168.1.2", 4)

    multiTest(it, model, "the 180.25ml liquid", 0)

    multiTest(it, model, "the 180ml liquid", 0)

    multiTest(it, model, " 29km Road ", 0)

    multiTest(it, model, " the May 4th ", 0)

    multiTest(it, model, "the .25ml liquid", 0)

    basicTest(it, model, ".08", "0.08")

    multiTest(it, model, "an", 0)

    multiTest(it, model, "a", 0)

    basicTest(it, model, ".23456000", "0.23456")

    basicTest(it, model, "4.800", "4.8")

    basicTest(it, model, "one hundred and three and two thirds", EnglishCultureInfo.format(103 + 2 / 3));

    basicTest(it, model, '16.4 million', '16400000');

    basicTest(it, model, '48.2 million', '48200000');

    basicTest(it, model, '48 million', '48000000');

    basicTest(it, model, "sixteen", "16")

    basicTest(it, model, "two thirds", EnglishCultureInfo.format(2 / 3))

    basicTest(it, model, "one hundred and sixteen", "116")

    basicTest(it, model, "one hundred and six", "106")

    basicTest(it, model, "one hundred and sixty-one", "161")

    basicTest(it, model, "a trillionth", "1E-12")

    basicTest(it, model, "a hundred trillionths", "1E-10")

    basicTest(it, model, " half a  dozen", "6")

    basicTest(it, model, " 3 dozens", "36")

    basicTest(it, model, "a dozen", "12")

    basicTest(it, model, " three dozens ", "36")

    basicTest(it, model, " three hundred and two dozens", "324")

    basicTest(it, model, "1,234,567", "1234567")

    multiTest(it, model, "1, 234, 567", 3)

    basicTest(it, model, "20,000", "20000")

    basicTest(it, model, "9.2321312", "9.2321312")

    basicTest(it, model, "-9.2321312", "-9.2321312")

    basicTest(it, model, " -9.2321312", "-9.2321312")

    basicTest(it, model, " -1", "-1")

    basicTest(it, model, "-4/5", "-0.8")

    basicTest(it, model, "- 1 4/5", "-1.8")

    basicTest(it, model, "three", "3")

    basicTest(it, model, " 123456789101231", "123456789101231")

    basicTest(it, model, "-123456789101231", "-123456789101231")

    basicTest(it, model, " -123456789101231", "-123456789101231")

    basicTest(it, model, "1", "1")

    basicTest(it, model, "10k", "10000")

    basicTest(it, model, "10G", "10000000000")

    basicTest(it, model, "- 10  k", "-10000")

    basicTest(it, model, "2 million", "2000000")

    basicTest(it, model, "1 trillion", "1000000000000")

    basicTest(it, model, " three ", "3")

    basicTest(it, model, "one trillion", "1000000000000")

    basicTest(it, model, "twenty-one trillion", "21000000000000")

    basicTest(it, model, "twenty-one trillion three hundred", "21000000000300")

    basicTest(it, model, "twenty-one trillion and three hundred", "21000000000300")

    basicTest(it, model, "fifty - two", "52")

    basicTest(it, model, "fifty   two", "52")

    basicTest(it, model, "Three hundred  and  thirty one", "331")

    basicTest(it, model, "two hundred and two thousand", "202000")

    basicTest(it, model, "two  thousand  and  two hundred", "2200")

    basicTest(it, model, " 2.33 k", "2330")

    basicTest(it, model, " two hundred point zero three", "200.03")

    basicTest(it, model, " two hundred point seventy-one", "200.71")

    basicTest(it, model, "1e10", "10000000000")

    basicTest(it, model, "1.1^23", "8.95430243255239")

    basicTest(it, model, " 322 hundred ", "32200")

    basicTest(it, model, "three", "3")

    basicTest(it, model, "seventy", "70")

    basicTest(it, model, "fifty-two", "52")

    basicTest(it, model, "2  1/4", "2.25")

    basicTest(it, model, "3/4", "0.75")

    basicTest(it, model, "one eighth", "0.125")

    basicTest(it, model, "five eighths", "0.625")

    basicTest(it, model, "a half", "0.5")

    basicTest(it, model, "three quarters", "0.75")

    basicTest(it, model, "twenty and three fifths", "20.6")

    basicTest(it, model, "twenty-three fifths", "4.6")

    basicTest(it, model, "twenty and three and three fifths", "23.6")

    basicTest(
        it,
        model,
        "one million two thousand two hundred three fifths",
        "200440.6"
    )

    basicTest(it, model, "one and a half", "1.5")

    basicTest(it, model, "one and a fourth", "1.25")

    basicTest(it, model, "five and a quarter", "5.25")

    basicTest(it, model, "one hundred and three quarters", "100.75")

    basicTest(it, model, "a hundredth", "0.01")

    basicTest(it, model, "1.1^+23", "8.95430243255239")

    basicTest(it, model, "2.5^-1", "0.4")

    basicTest(it, model, "-2500^-1", "-0.0004")

    basicTest(it, model, "-1.1^+23", "-8.95430243255239")

    basicTest(it, model, "-2.5^-1", "-0.4")

    basicTest(it, model, "-1.1^--23", "-8.95430243255239")

    basicTest(it, model, "-127.32e13", "-1.2732E+15")

    basicTest(it, model, "12.32e+14", "1.232E+15")

    basicTest(it, model, "-12e-1", "-1.2")

    basicTest(it, model, "1.2b", "1200000000")
});

describe('Fraction Model', it => {
    var model = NumberRecognizer.instance.getNumberModel(Culture.English, false);

    basicTest(it, model, "a fifth", "0.2");

    basicTest(it, model, "a trillionth", "1E-12");

    basicTest(it, model, "a hundred thousand trillionths", "1E-07");

    basicTest(it, model, "one fifth", "0.2");

    basicTest(it, model, "three fifths", "0.6");

    basicTest(it, model, "twenty fifths", "4");

    basicTest(it, model, "twenty-three fifths", "4.6");

    basicTest(it, model, "three and a fifth", "3.2");

    basicTest(it, model, "twenty one fifths", "4.2");

    basicTest(it, model, "a twenty-first", EnglishCultureInfo.format(1 / 21));

    basicTest(it, model, "one twenty-fifth", EnglishCultureInfo.format(1 / 25));

    basicTest(it, model, "three twenty-firsts", EnglishCultureInfo.format(3 / 21));

    basicTest(it, model, "three twenty firsts", EnglishCultureInfo.format(3 / 21));

    basicTest(it, model, "twenty twenty fifths", "0.8");

    // act like Google
    basicTest(it, model, "one hundred and thirty fifths", EnglishCultureInfo.format(130 / 5));

    basicTest(it, model, "one hundred thirty fifths", EnglishCultureInfo.format(100 / 35));

    basicTest(it, model, "one hundred thirty two fifths", EnglishCultureInfo.format(132 / 5));

    basicTest(it, model, "one hundred thirty-two fifths", EnglishCultureInfo.format(132 / 5));

    basicTest(it, model, "one hundred and thirty-two fifths", EnglishCultureInfo.format(132 / 5));

    basicTest(it, model, "one hundred and thirty and two fifths", EnglishCultureInfo.format(130 + 2 / 5));

    basicTest(it, model, "one hundred thirty-fifths", EnglishCultureInfo.format(100 / 35));

    basicTest(it, model, "one one hundred fifth", EnglishCultureInfo.format(new BigNumber(1).dividedBy(105)));

    basicTest(it, model, "one one hundred and fifth", EnglishCultureInfo.format(new BigNumber(1).dividedBy(105)));

    basicTest(it, model, "one hundred one thousand fifths", EnglishCultureInfo.format(100 / 1005));

    basicTest(it, model, "one over three", EnglishCultureInfo.format(1 / 3));

    basicTest(it, model, "1 over twenty-one", EnglishCultureInfo.format(1 / 21));

    basicTest(it, model, "1 over one hundred and twenty one", EnglishCultureInfo.format(1 / 121));

    basicTest(it, model, "1 over three", EnglishCultureInfo.format(1 / 3));

    basicTest(it, model, "1 over 3", EnglishCultureInfo.format(1 / 3));

    basicTest(it, model, "one over 3", EnglishCultureInfo.format(1 / 3));

    basicTest(it, model, "one over 20", EnglishCultureInfo.format(1 / 20));

    basicTest(it, model, "one over twenty", EnglishCultureInfo.format(1 / 20));

    basicTest(it, model, "one over one hundred", EnglishCultureInfo.format(1 / 100));

    basicTest(it, model, "one over one hundred and twenty five", EnglishCultureInfo.format(1 / 125));

    basicTest(it, model, "ninety - five hundred fifths", EnglishCultureInfo.format(9500 / 5));
});

describe('Percent Model', it => {
    var model = NumberRecognizer.instance.getPercentageModel(Culture.English, false);

    basicTest(it, model,
        "100%", "100%");

    basicTest(it, model,
        " 100% ", "100%");

    basicTest(it, model,
        " 100 percent", "100%");

    basicTest(it, model,
        " 100 percentage", "100%");

    basicTest(it, model,
        "240 percent", "240%");

    basicTest(it, model,
        "twenty percent", "20%");

    basicTest(it, model,
        "thirty percentage", "30%");

    basicTest(it, model,
        "one hundred percent", "100%");

    basicTest(it, model,
        "one hundred percents", "100%");

    basicTest(it, model,
        "percents of twenty", "20%");

    basicTest(it, model,
        "percent of 10", "10%");

    basicTest(it, model,
        "per cent of twenty-two", "22%");

    basicTest(it, model,
        "per cent of 210", "210%");

    basicTest(it, model,
        "10 percent", "10%");
});

function basicTest(it, model, source, value) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, 1);
        t.is(result[0].text, source.trim());
        t.is(result[0].resolution["value"], value);
    });
}

function wrappedTest(it, model, source, extractSrc, value) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, 1);
        t.is(result[0].text, extractSrc);
        t.is(result[0].resolution["value"], value);
    });
}

function multiTest(it, model, source, count) {
    it(source, t => {
        t.not(model, null);
        let result = model.parse(source);
        t.is(result.length, count);
    });
}
import { NumberRecognizer, Culture } from "recognizers-text-number";
import { NumberWithUnitRecognizer } from "recognizers-text-number-with-unit";
import { DateTimeRecognizer } from "recognizers-text-datetime";

let numberRecognizer = NumberRecognizer.instance.getNumberModel(Culture.English);
let numberWithUnitRecognizer = NumberWithUnitRecognizer.instance.getDimensionModel(Culture.English);
let dateTimeRecognizer = DateTimeRecognizer.instance.getDateTimeModel(Culture.English);

let utterance = "I have twenty kg of apples for tomorrow";

let num = numberRecognizer.parse(utterance);
let numWithUnit = numberWithUnitRecognizer.parse(utterance);
let datetime = dateTimeRecognizer.parse(utterance);

console.log(JSON.stringify(num));
console.log(JSON.stringify(numWithUnit));
console.log(JSON.stringify(datetime));


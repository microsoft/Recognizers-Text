"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const recognizers_text_number_1 = require("recognizers-text-number");
let numberRecognizer = recognizers_text_number_1.NumberRecognizer.instance.getNumberModel(recognizers_text_number_1.Culture.English);
let data = numberRecognizer.parse("I have twenty apples");
console.log(JSON.stringify(data));
//# sourceMappingURL=index.js.map
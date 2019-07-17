import { Culture as BaseCulture, CultureInfo as BaseCultureInfo } from "@microsoft/recognizers-text";
import trimEnd = require("lodash.trimend");
import { BigNumber } from 'bignumber.js/bignumber';
import { LongFormatType } from "./number/models";

export class Culture extends BaseCulture {

  static readonly supportedCultures: Array<Culture> = [
    new Culture("English", Culture.English, new LongFormatType(',', '.')),
    new Culture("EnglishOthers", Culture.EnglishOthers, new LongFormatType(',', '.')),
    new Culture("Chinese", Culture.Chinese, null),
    new Culture("Spanish", Culture.Spanish, new LongFormatType('.', ',')),
    new Culture("Portuguese", Culture.Portuguese, new LongFormatType('.', ',')),
    new Culture("French", Culture.French, new LongFormatType('.', ',')),
    new Culture("Japanese", Culture.Japanese, new LongFormatType(',', '.'))
  ]

  readonly longFormat: LongFormatType

  private constructor(cultureName: string, cultureCode: string, longFormat: LongFormatType) {
    super(cultureName, cultureCode);
    this.longFormat = longFormat;
  }
}

export class CultureInfo extends BaseCultureInfo {
  format(value: number | BigNumber): string {

    let bigNumber = new BigNumber(value);
    let s: string;
    if (bigNumber.decimalPlaces()) {
      s = bigNumber.precision(15, BigNumber.ROUND_HALF_UP).toString();
    } else {
      s = bigNumber.toString().toUpperCase();
    }

    if (s.indexOf('.') > -1) {
      // trim leading 0 from decimal places
      s = trimEnd(s, '0');
    }

    if (s.indexOf('e-') > -1) {
      // mimic .NET behavior by adding leading 0 to exponential. E.g.: 1E-07
      let p = s.split('e-');
      p[1] = p[1].length === 1 ? ('0' + p[1]) : p[1];
      s = p.join('E-')
    }

    // TODO: Use BigNumber.toFormat instead
    let culture = Culture.supportedCultures.find(c => c.cultureCode === this.code);
    if(culture && culture.longFormat) {
      return s
        .split(',')
        .map(t => t.split('.').join(culture.longFormat.decimalsMark))
        .join(culture.longFormat.thousandsMark);
    }

    return s;
  }
}
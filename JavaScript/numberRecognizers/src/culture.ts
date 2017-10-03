import trimEnd = require("lodash.trimend");
import { BigNumber } from 'bignumber.js';
import { LongFormatType } from "./number/models";

export class Culture {

  static readonly English: string = "en-us"
  static readonly Chinese: string = "zh-cn"
  static readonly Spanish: string = "es-es"
  static readonly Portuguese: string = "pt-br"
  static readonly French: string = "fr-fr"

  static readonly supportedCultures: Array<Culture> = [
    new Culture("English", Culture.English, new LongFormatType(',', '.')),
    new Culture("Chinese", Culture.Chinese, null),
    new Culture("Spanish", Culture.Spanish, new LongFormatType('.', ',')),
    new Culture("Portuguese", Culture.Portuguese, null),
    new Culture("French", Culture.French, null)
  ]

  readonly cultureName: string
  readonly cultureCode: string
  readonly longFormat: LongFormatType

  private constructor(cultureName: string, cultureCode: string, longFormat: LongFormatType) {
    this.cultureName = cultureName
    this.cultureCode = cultureCode
    this.longFormat = longFormat;
  }

  static getSupportedCultureCodes(): Array<string> {
    return Culture.supportedCultures.map(c => c.cultureCode)
  }
}

export class CultureInfo {
  readonly code: string;

  static getCultureInfo(cultureCode: string): CultureInfo {
    return new CultureInfo(cultureCode);
  }

  constructor(cultureName: string) {
    this.code = cultureName;
  }

  format(value: number | BigNumber): string {

    let bigNumber = new BigNumber(value);
    let s: string;
    if (bigNumber.decimalPlaces()) {
      s = bigNumber.toDigits(15, BigNumber.ROUND_HALF_UP).toString();
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
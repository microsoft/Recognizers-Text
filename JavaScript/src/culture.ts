import * as IntlPolyfill from 'intl';

export class Culture {
  static readonly English: string = "en-us"
  static readonly Chinese: string = "zh-cn"
  static readonly Spanish: string = "es-es"
  static readonly Portuguese: string = "pt-br"
  static readonly French: string = "fr-fr"

  static readonly supportedCultures: Array<Culture> = [
    new Culture("English", Culture.English),
    new Culture("Chinese", Culture.Chinese),
    new Culture("Spanish", Culture.Spanish),
    new Culture("Portuguese", Culture.Portuguese),
    new Culture("French", Culture.French)
  ]

  readonly cultureName: string
  readonly cultureCode: string

  private constructor(cultureName: string, cultureCode: string) {
    this.cultureName = cultureName
    this.cultureCode = cultureCode
  }

  static getSupportedCultureCodes(): Array<string> {
    return Culture.supportedCultures.map(c => c.cultureCode)
  }
}

export class CultureInfo {
  readonly name: string;

  static getCultureInfo(cultureName: string): CultureInfo {
    return new CultureInfo(cultureName);
  }

  constructor(cultureName: string) {
    this.name = cultureName;
  }

  format(value: number): string {
    return NumberUtility.format(value, this);
  }
}

class NumberUtility {
  static format(value: number, culture: CultureInfo): string {
    // Default option settings to mimic NET SDK locale behavior
    let dotNetDefaultFractionDigits = 14;
    let dotNetDefaultUseGrouping = false;

    let decimalPlaces = Math.min(NumberUtility.decimalPlaces(value), dotNetDefaultFractionDigits);
    let polyfillResult = IntlPolyfill.NumberFormat(culture.name, {
      useGrouping: dotNetDefaultUseGrouping,
      maximumFractionDigits: decimalPlaces
    }).format(value);
    let toStringResult = value.toString().toUpperCase();

    return (polyfillResult.length <= toStringResult.length) ? polyfillResult : toStringResult;
  }

  private static decimalPlaces(n) {
    // Make sure it is a number and use the builtin number -> string.
    let s = "" + (+n);
    // Pull out the fraction and the exponent.
    let match = /(?:\.(\d+))?(?:[eE]([+\-]?\d+))?$/.exec(s);
    // NaN or Infinity or integer.
    // We arbitrarily decide that Infinity is integral.
    if (!match) { return 0; }
    // Count the number of digits in the fraction and subtract the
    // exponent to simulate moving the decimal point left by exponent places.
    // 1.234e+2 has 1 fraction digit and '234'.length -  2 == 1
    // 1.234e-2 has 5 fraction digit and '234'.length - -2 == 5
    return Math.max(
      0,  // lower limit.
      (match[1] == '0' ? 0 : (match[1] || '').length)  // fraction length
      - (+match[2] || 0));  // exponent
  }
}
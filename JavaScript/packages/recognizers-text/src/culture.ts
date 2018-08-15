export class Culture {

  static readonly English: string = "en-us"
  static readonly Chinese: string = "zh-cn"
  static readonly Spanish: string = "es-es"
  static readonly Portuguese: string = "pt-br"
  static readonly French: string = "fr-fr"
  static readonly German: string = "de-de"
  static readonly Japanese: string = "ja-jp"
  static readonly Dutch: string = "nl-nl"

  static readonly supportedCultures: Array<Culture> = [
    new Culture("English", Culture.English),
    new Culture("Chinese", Culture.Chinese),
    new Culture("Spanish", Culture.Spanish),
    new Culture("Portuguese", Culture.Portuguese),
    new Culture("French", Culture.French),
    new Culture("German", Culture.German),
    new Culture("Japanese", Culture.Japanese),
    new Culture("Dutch", Culture.Dutch)
  ]

  readonly cultureName: string
  readonly cultureCode: string

  protected constructor(cultureName: string, cultureCode: string) {
    this.cultureName = cultureName
    this.cultureCode = cultureCode
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
}
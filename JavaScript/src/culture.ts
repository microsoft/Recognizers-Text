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
  // TODO: implement
  readonly name: string;

  constructor(cultureName: string) {
    this.name = cultureName;
  }
}
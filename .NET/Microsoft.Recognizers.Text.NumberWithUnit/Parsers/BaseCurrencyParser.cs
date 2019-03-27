using System.Collections.Generic;

using Microsoft.Recognizers.Text.NumberWithUnit.Utilities;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class BaseCurrencyParser : IParser
    {
        private readonly NumberWithUnitParser numberWithUnitParser;

        public BaseCurrencyParser(BaseNumberWithUnitParserConfiguration config)
        {
            this.Config = config;
            numberWithUnitParser = new NumberWithUnitParser(config);
        }

        protected BaseNumberWithUnitParserConfiguration Config { get; }

        public ParseResult Parse(ExtractResult extResult)
        {
            ParseResult pr;

            if (extResult.Data is List<ExtractResult>)
            {
                pr = MergeCompoundUnit(extResult);
            }
            else
            {
                pr = numberWithUnitParser.Parse(extResult);
                var value = pr.Value as UnitValue;

                Config.CurrencyNameToIsoCodeMap.TryGetValue(value?.Unit, out var mainUnitIsoCode);
                if (string.IsNullOrEmpty(mainUnitIsoCode) || mainUnitIsoCode.StartsWith(Constants.FAKE_ISO_CODE_PREFIX))
                {
                    pr.Value = new UnitValue
                    {
                        Unit = value?.Unit,
                        Number = value?.Number,
                    };
                }
                else
                {
                    pr.Value = new CurrencyUnitValue
                    {
                        Unit = value?.Unit,
                        Number = value?.Number,
                        IsoCurrency = mainUnitIsoCode,
                    };
                }
            }

            return pr;
        }

        private static void ResolveText(List<ParseResult> prs, string source, int bias)
        {
            foreach (var parseResult in prs)
            {
                if (parseResult.Start != null && parseResult.Length != null)
                {
                    parseResult.Text = source.Substring((int)parseResult.Start - bias, (int)parseResult.Length);
                }
            }
        }

        private static bool CheckUnitsStringContains(string fractionUnitCode, string fractionUnitsString)
        {
            var unitsMap = new Dictionary<string, string>();
            DictionaryUtils.BindUnitsString(unitsMap, string.Empty, fractionUnitsString);
            return unitsMap.ContainsKey(fractionUnitCode);
        }

        private string GetResolutionStr(object value)
        {
            return Config.CultureInfo != null ?
                ((double)value).ToString(Config.CultureInfo) :
                value.ToString();
        }

        private ParseResult MergeCompoundUnit(ExtractResult compoundResult)
        {
            var results = new List<ParseResult>();
            var compoundUnit = (List<ExtractResult>)compoundResult.Data;

            var count = 0;
            ParseResult result = null;
            var numberValue = 0.0;
            var mainUnitValue = string.Empty;
            string mainUnitIsoCode = string.Empty;
            string fractionUnitsString = string.Empty;

            for (var idx = 0; idx < compoundUnit.Count; idx++)
            {
                var extractResult = compoundUnit[idx];
                var parseResult = numberWithUnitParser.Parse(extractResult);
                var parseResultValue = parseResult.Value as UnitValue;
                var unitValue = parseResultValue?.Unit;

                // Process a new group
                if (count == 0)
                {
                    if (!extractResult.Type.Equals(Constants.SYS_UNIT_CURRENCY))
                    {
                        continue;
                    }

                    // Initialize a new result
                    result = new ParseResult
                    {
                        Start = extractResult.Start,
                        Length = extractResult.Length,
                        Text = extractResult.Text,
                        Type = extractResult.Type,
                    };

                    mainUnitValue = unitValue;
                    numberValue = double.Parse(parseResultValue?.Number);
                    result.ResolutionStr = parseResult.ResolutionStr;

                    Config.CurrencyNameToIsoCodeMap.TryGetValue(unitValue, out mainUnitIsoCode);

                    // If the main unit can't be recognized, finish process this group.
                    if (string.IsNullOrEmpty(mainUnitIsoCode))
                    {
                        result.Value = new UnitValue
                        {
                            Number = GetResolutionStr(numberValue),
                            Unit = mainUnitValue,
                        };
                        results.Add(result);
                        result = null;
                        continue;
                    }

                    Config.CurrencyFractionMapping.TryGetValue(mainUnitIsoCode, out fractionUnitsString);
                }
                else
                {
                    // Match pure number as fraction unit.
                    if (extractResult.Type.Equals(Constants.SYS_NUM))
                    {
                        numberValue += (double)parseResult.Value * (1.0 / 100);
                        result.ResolutionStr += ' ' + parseResult.ResolutionStr;
                        result.Length = parseResult.Start + parseResult.Length - result.Start;
                        count++;
                        continue;
                    }

                    Config.CurrencyFractionCodeList.TryGetValue(unitValue, out var fractionUnitCode);
                    Config.CurrencyFractionNumMap.TryGetValue(parseResultValue?.Unit, out var fractionNumValue);

                    if (!string.IsNullOrEmpty(fractionUnitCode) && fractionNumValue != 0 &&
                        CheckUnitsStringContains(fractionUnitCode, fractionUnitsString))
                    {
                        numberValue += double.Parse(parseResultValue?.Number) *
                                       (1.0 / fractionNumValue);
                        result.ResolutionStr += ' ' + parseResult.ResolutionStr;
                        result.Length = parseResult.Start + parseResult.Length - result.Start;
                    }
                    else
                    {
                        // If the fraction unit doesn't match the main unit, finish process this group.
                        if (result != null)
                        {
                            if (string.IsNullOrEmpty(mainUnitIsoCode) ||
                                mainUnitIsoCode.StartsWith(Constants.FAKE_ISO_CODE_PREFIX))
                            {
                                result.Value = new UnitValue
                                {
                                    Number = GetResolutionStr(numberValue),
                                    Unit = mainUnitValue,
                                };
                            }
                            else
                            {
                                result.Value = new CurrencyUnitValue
                                {
                                    Number = GetResolutionStr(numberValue),
                                    Unit = mainUnitValue,
                                    IsoCurrency = mainUnitIsoCode,
                                };
                            }

                            results.Add(result);
                            result = null;
                        }

                        count = 0;
                        idx -= 1;
                        continue;
                    }
                }

                count++;
            }

            if (result != null)
            {
                if (string.IsNullOrEmpty(mainUnitIsoCode) ||
                    mainUnitIsoCode.StartsWith(Constants.FAKE_ISO_CODE_PREFIX))
                {
                    result.Value = new UnitValue
                    {
                        Number = GetResolutionStr(numberValue),
                        Unit = mainUnitValue,
                    };
                }
                else
                {
                    result.Value = new CurrencyUnitValue
                    {
                        Number = GetResolutionStr(numberValue),
                        Unit = mainUnitValue,
                        IsoCurrency = mainUnitIsoCode,
                    };
                }

                results.Add(result);
            }

            ResolveText(results, compoundResult.Text, (int)compoundResult.Start);

            return new ParseResult
            {
                Value = results,
            };
        }
    }
}
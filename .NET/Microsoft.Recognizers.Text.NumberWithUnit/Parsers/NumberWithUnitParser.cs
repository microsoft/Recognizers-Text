// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Recognizers.Text.NumberWithUnit.Utilities;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitParser : IParser
    {
        public NumberWithUnitParser(INumberWithUnitParserConfiguration config)
        {
            this.Config = config;
        }

        protected INumberWithUnitParserConfiguration Config { get; private set; }

        public static void AddIfNotContained(List<string> unitKeys, string unit)
        {
            var add = true;
            foreach (var unitKey in unitKeys)
            {
                if (unitKey.Contains(unit))
                {
                    add = false;
                    break;
                }
            }

            if (add)
            {
                unitKeys.Add(unit);
            }
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            var ret = new ParseResult(extResult);

            ExtractResult numberResult;
            ExtractResult halfResult;

            if (extResult.Data is List<ExtractResult> && this.Config as English.DimensionParserConfiguration != null)
            {
                return MergeCompoundUnit(extResult);
            }

            if (extResult.Data is ExtractResult unitResult)
            {
                numberResult = unitResult;
                halfResult = null;
            }
            else if (extResult.Type.Equals(Constants.SYS_NUM, StringComparison.Ordinal))
            {
                ret.Value = Config.InternalNumberParser.Parse(extResult).Value;
                return ret;
            }
            else if (extResult.Data is System.Collections.IList && ((List<ExtractResult>)extResult.Data).Count == 2)
            {
                numberResult = ((List<ExtractResult>)extResult.Data)[0];
                halfResult = ((List<ExtractResult>)extResult.Data)[1];
            }
            else
            {
                // If there is no unitResult, means there is just unit
                numberResult = new ExtractResult { Start = -1, Length = 0, Text = string.Empty };
                halfResult = null;
            }

            // Key contains units
            var key = extResult.Text;
            var unitKeyBuild = new StringBuilder();
            var unitKeys = new List<string>();
            for (var i = 0; i <= key.Length; i++)
            {
                if (i == key.Length)
                {
                    if (unitKeyBuild.Length != 0)
                    {
                        AddIfNotContained(unitKeys, unitKeyBuild.ToString().Trim());
                    }
                }
                else if (i == numberResult.Start)
                {
                    // numberResult.start is a relative position
                    if (unitKeyBuild.Length != 0)
                    {
                        AddIfNotContained(unitKeys, unitKeyBuild.ToString().Trim());
                        unitKeyBuild.Clear();
                    }

                    var o = numberResult.Start + numberResult.Length - 1;
                    if (o != null)
                    {
                        i = (int)o;
                    }
                }
                else
                {
                    unitKeyBuild.Append(key[i]);
                }
            }

            // By default, unit type depends on last unit in suffix,
            // but in certain cultures (e.g. Japanese) it depends on first unit in suffix
            var lastUnit = Config.CheckFirstSuffix ? unitKeys.First() : unitKeys.Last();

            if (halfResult != null)
            {
                lastUnit = lastUnit.Substring(0, lastUnit.Length - halfResult.Text.Length).Trim();
            }

            var normalizedLastUnit = lastUnit.ToLowerInvariant();
            if (!string.IsNullOrEmpty(Config.ConnectorToken) && normalizedLastUnit.StartsWith(Config.ConnectorToken, StringComparison.Ordinal))
            {
                normalizedLastUnit = normalizedLastUnit.Substring(Config.ConnectorToken.Length).Trim();
                lastUnit = lastUnit.Substring(Config.ConnectorToken.Length).Trim();
            }

            // Delete brackets
            normalizedLastUnit = DeleteBracketsIfExisted(normalizedLastUnit);
            lastUnit = DeleteBracketsIfExisted(lastUnit);

            if (!string.IsNullOrWhiteSpace(key) && Config.UnitMap != null)
            {
                if (Config.UnitMap.TryGetValue(lastUnit, out var unitValue) ||
                    Config.UnitMap.TryGetValue(normalizedLastUnit, out unitValue))
                {

                    var numValue = string.IsNullOrEmpty(numberResult.Text) ?
                                            null :
                                            this.Config.InternalNumberParser.Parse(numberResult);

                    var resolutionStr = numValue?.ResolutionStr;

                    if (halfResult != null)
                    {
                        var halfValue = this.Config.InternalNumberParser.Parse(halfResult);
                        resolutionStr += halfValue?.ResolutionStr.Substring(1);
                    }

                    // In certain cultures the unit can be split around the number,
                    // e.g. in Japanese "秒速100メートル" ('speed per second 100 meters' = 100m/s).
                    // Here prefix and suffix are combined in order to parse the unit correctly.
                    if (unitValue == Constants.SPLIT_UNIT && unitKeys.Count > 1 && this.Config.CheckFirstSuffix)
                    {
                        if (Config.UnitMap.TryGetValue(lastUnit + unitKeys[1], out var allUnitValue) ||
                            Config.UnitMap.TryGetValue(unitKeys[1], out allUnitValue) ||
                            Config.UnitMap.TryGetValue(unitKeys[1].ToLowerInvariant(), out allUnitValue))
                        {
                            unitValue = allUnitValue;
                        }
                    }

                    if (unitValue == Constants.SPLIT_UNIT)
                    {
                        return ret;
                    }

                    ret.Value = new UnitValue
                    {
                        Number = resolutionStr,
                        Unit = unitValue,
                    };

                    ret.ResolutionStr = $"{numValue?.ResolutionStr} {unitValue}".Trim();

                    if (extResult.Type.Equals(Constants.SYS_UNIT_DIMENSION, StringComparison.Ordinal) &&
                        this.Config.TypeList.TryGetValue(unitValue, out var unitType))
                    {
                        ret.Type = unitType;
                    }
                }
            }

            ret.Text = ret.Text.ToLowerInvariant();

            return ret;
        }

        private static string DeleteBracketsIfExisted(string unit)
        {
            bool hasBrackets = false;

            if (unit.StartsWith("(", StringComparison.Ordinal) && unit.EndsWith(")", StringComparison.Ordinal))
            {
                hasBrackets = true;
            }
            else if (unit.StartsWith("[", StringComparison.Ordinal) && unit.EndsWith("]", StringComparison.Ordinal))
            {
                hasBrackets = true;
            }
            else if (unit.StartsWith("{", StringComparison.Ordinal) && unit.EndsWith("}", StringComparison.Ordinal))
            {
                hasBrackets = true;
            }
            else if (unit.StartsWith("<", StringComparison.Ordinal) && unit.EndsWith(">", StringComparison.Ordinal))
            {
                hasBrackets = true;
            }

            if (hasBrackets)
            {
                unit = unit.Substring(1, unit.Length - 2);
            }

            return unit;
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

        /// <summary>
        /// Parsing compounded result, like 5 foot 3 inch.
        /// </summary>
        /// <param name="compoundResult">Extracted compounded result.</param>
        /// <returns>Parsed compounded result.</returns>
        private ParseResult MergeCompoundUnit(ExtractResult compoundResult)
        {
            var results = new List<ParseResult>();
            var compoundUnit = (List<ExtractResult>)compoundResult.Data;

            var count = 0;
            ParseResult result = null;
            double? numberValue = null;
            var mainUnitValue = string.Empty;
            string mainUnitIsoCode = string.Empty;
            string fractionUnitsString = string.Empty;

            for (var idx = 0; idx < compoundUnit.Count; idx++)
            {
                var extractResult = compoundUnit[idx];
                var parseResult = this.Parse(extractResult);
                var parseResultValue = parseResult.Value as UnitValue;
                var unitValue = parseResultValue?.Unit;

                // Process a new group
                if (count == 0)
                {
                    if (!extractResult.Type.Equals(Constants.SYS_UNIT_DIMENSION, StringComparison.Ordinal))
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

                    if (parseResultValue?.Number != null)
                    {
                        numberValue = double.Parse(parseResultValue.Number, CultureInfo.InvariantCulture);
                    }

                    result.ResolutionStr = parseResult.ResolutionStr;
                    English.DimensionParserConfiguration.LengthUnitToSubUnitMap.TryGetValue(mainUnitValue, out fractionUnitsString);
                }
                else
                {
                    long fractionNumValue = 0;
                    string fractionUnit = parseResultValue is null ? null : parseResultValue.Unit;
                    English.DimensionParserConfiguration.LengthSubUnitFractionalRatios.TryGetValue(parseResultValue?.Unit, out fractionNumValue);

                    if (!string.IsNullOrEmpty(fractionUnit) && fractionNumValue != 0 &&
                        CheckUnitsStringContains(fractionUnit, fractionUnitsString))
                    {
                        numberValue += double.Parse(parseResultValue?.Number, CultureInfo.InvariantCulture) *
                                       (1.0 / fractionNumValue);
                        result.ResolutionStr += ' ' + parseResult.ResolutionStr;
                        result.Length = parseResult.Start + parseResult.Length - result.Start;
                    }
                    else
                    {
                        // If the fraction unit doesn't match the main unit, finish process this group.
                        if (result != null)
                        {
                            result = CreateResult(result, mainUnitIsoCode, numberValue, mainUnitValue);
                            results.Add(result);
                            result = null;
                        }

                        count = 0;
                        idx -= 1;
                        numberValue = null;
                        continue;
                    }
                }

                count++;
            }

            if (result != null)
            {
                result = CreateResult(result, mainUnitIsoCode, numberValue, mainUnitValue);
                results.Add(result);
            }

            ResolveText(results, compoundResult.Text, (int)compoundResult.Start);

            return new ParseResult
            {
                Value = results,
            };
        }

        private ParseResult CreateResult(ParseResult result, string mainUnitIsoCode, object numberValue, string mainUnitValue)
        {
            if (string.IsNullOrEmpty(mainUnitIsoCode) ||
                mainUnitIsoCode.StartsWith(Constants.FAKE_ISO_CODE_PREFIX, StringComparison.Ordinal))
            {
                result.Value = new UnitValue
                {
                    Number = GetResolutionStr(numberValue),
                    Unit = mainUnitValue,
                };
                if (result.Type.Equals(Constants.SYS_UNIT_DIMENSION, StringComparison.Ordinal) &&
                        this.Config.TypeList.TryGetValue(mainUnitValue, out var unitType))
                {
                    result.Type = unitType;
                }
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

            return result;
        }

        private string GetResolutionStr(object value)
        {
            // Nothing to resolve. This happens when the entity is a currency name only (no numerical value).
            if (value == null)
            {
                return null;
            }

            return Config.CultureInfo != null ?
                ((double)value).ToString(Config.CultureInfo) :
                value.ToString();
        }

    }
}
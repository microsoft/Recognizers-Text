// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
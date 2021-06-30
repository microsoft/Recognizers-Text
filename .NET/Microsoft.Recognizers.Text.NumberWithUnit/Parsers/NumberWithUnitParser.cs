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

            // Unit type depends on last unit in suffix
            var lastUnit = unitKeys.Last();
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

            if (!string.IsNullOrWhiteSpace(key) && Config.UnitMap != null)
            {
                if (Config.UnitMap.TryGetValue(lastUnit, out var unitValue) ||
                    Config.UnitMap.TryGetValue(normalizedLastUnit, out unitValue))
                {
                    var numValue = string.IsNullOrEmpty(numberResult.Text) ?
                        null :
                        this.Config.InternalNumberParser.Parse(numberResult);
                    var resolution_str = numValue?.ResolutionStr;
                    if (halfResult != null)
                    {
                        var halfValue = this.Config.InternalNumberParser.Parse(halfResult);
                        resolution_str += halfValue?.ResolutionStr.Substring(1);
                    }

                    ret.Value = new UnitValue
                    {
                        Number = resolution_str,
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
    }
}
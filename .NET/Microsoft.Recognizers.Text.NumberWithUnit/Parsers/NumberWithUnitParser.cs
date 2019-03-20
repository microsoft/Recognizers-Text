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

            if (extResult.Data is ExtractResult unitResult)
            {
                numberResult = unitResult;
            }
            else if (extResult.Type.Equals(Constants.SYS_NUM))
            {
                ret.Value = Config.InternalNumberParser.Parse(extResult).Value;
                return ret;
            }
            else
            {
                // If there is no unitResult, means there is just unit
                numberResult = new ExtractResult { Start = -1, Length = 0 };
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
            var normalizedLastUnit = lastUnit.ToLowerInvariant();
            if (!string.IsNullOrEmpty(Config.ConnectorToken) && normalizedLastUnit.StartsWith(Config.ConnectorToken))
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

                    ret.Value = new UnitValue
                    {
                        Number = numValue?.ResolutionStr,
                        Unit = unitValue,
                    };
                    ret.ResolutionStr = $"{numValue?.ResolutionStr} {unitValue}".Trim();
                }
            }

            ret.Text = ret.Text.ToLowerInvariant();

            return ret;
        }
    }
}
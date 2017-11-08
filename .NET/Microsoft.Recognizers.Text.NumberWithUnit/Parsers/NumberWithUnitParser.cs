using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitParser : IParser
    {
        protected readonly INumberWithUnitParserConfiguration config;

        public NumberWithUnitParser(INumberWithUnitParserConfiguration config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            var ret = new ParseResult(extResult);
            ExtractResult numberResult;
            var unitResult = extResult.Data as ExtractResult;
            if (unitResult != null)
            {
                numberResult = unitResult;
            }
            else // if there is no unitResult, means there is just unit
            {
                numberResult = new ExtractResult { Start = -1, Length = 0 };
            }

            // key contains units
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

            /* Unit type depends on last unit in suffix.*/
            var lastUnit = unitKeys.Last().ToLowerInvariant();
            if ((!string.IsNullOrEmpty(this.config.ConnectorToken)) && lastUnit.StartsWith(this.config.ConnectorToken))
            {
                lastUnit = lastUnit.Substring(this.config.ConnectorToken.Length).Trim();
            }

            if (!string.IsNullOrWhiteSpace(key) && (this.config.UnitMap != null) && this.config.UnitMap.ContainsKey(lastUnit))
            {
                var unitValue = this.config.UnitMap[lastUnit];
                var numValue = (string.IsNullOrEmpty(numberResult.Text)) ? null : this.config.InternalNumberParser.Parse(numberResult);
                ret.Value = new UnitValue
                {
                    Number = numValue?.ResolutionStr,
                    Unit = unitValue
                };
                ret.ResolutionStr = $"{numValue?.ResolutionStr} {unitValue}".Trim();
            }
            return ret;
        }

        public void AddIfNotContained(List<string> unitKeys, string unit)
        {
            bool add = true;
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
    }

    public class UnitValue
    {
        public string Number = "";
        public string Unit = "";
    }
}
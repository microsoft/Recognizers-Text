using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class BaseMergedDimensionsParser : IParser
    {
        private const int DefaultFractionalSubunit = 1;

        private readonly NumberWithUnitParser numberWithUnitParser;

        public BaseMergedDimensionsParser(BaseNumberWithUnitParserConfiguration config)
        {
            this.Config = config;
            numberWithUnitParser = new NumberWithUnitParser(config);
        }

        protected BaseNumberWithUnitParserConfiguration Config { get; }

        public ParseResult Parse(ExtractResult extResult)
        {
            var pr = MergeCompoundUnit(extResult);

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

        private string GetResolutionStr(object value)
        {
            // Nothing to resolve. This happens when the entity is a dimension name only (no numerical value).
            if (value == null)
            {
                return null;
            }

            return Config.CultureInfo != null ?
                ((double)value).ToString(Config.CultureInfo) :
                value.ToString();
        }

        private ParseResult CreateResult(ParseResult result, object numberValue, string mainUnitValue)
        {
            result.Value = new UnitValue
            {
                Number = GetResolutionStr(numberValue),
                Unit = mainUnitValue,
            };

            return result;
        }

        private ParseResult MergeCompoundUnit(ExtractResult compoundResult)
        {
            var results = new List<ParseResult>();
            var compoundUnit = (List<ExtractResult>)compoundResult.Data;

            var count = 0;
            ParseResult result = null;
            double? numberValue = null;
            var mainUnitValue = string.Empty;
            IEnumerable<double> mainUnitMultiplier = new List<double> { 0, 0 };
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
                        numberValue = double.Parse(parseResultValue.Number);
                    }

                    result.ResolutionStr = parseResult.ResolutionStr;

                    // Extract main unit identifier and multiplier.
                    this.Config.DimensionUnitMultiplesMap.TryGetValue(unitValue, out mainUnitMultiplier);

                    // If the main unit can't be recognized, finish process this group.
                    if (mainUnitMultiplier == null)
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
                }
                else
                {
                    // Match pure number as fraction unit.
                    if (extractResult.Type.Equals(Constants.SYS_NUM, StringComparison.Ordinal))
                    {
                        var fractionMaxValue = DefaultFractionalSubunit;
                        if ((double)parseResult.Value < fractionMaxValue)
                        {
                            numberValue += (double)parseResult.Value * (1.0 / fractionMaxValue);
                            result.ResolutionStr += ' ' + parseResult.ResolutionStr;
                            result.Length = parseResult.Start + parseResult.Length - result.Start;
                        }

                        count++;
                        continue;
                    }

                    // Extract fraction unit identifier and multiplier.
                    this.Config.DimensionUnitMultiplesMap.TryGetValue(unitValue, out var fractionUnitCode);

                    // Check if the fraction identifier matches the main identifier and the fraction multiplier is smaller than the main multiplier.
                    if (fractionUnitCode != null && fractionUnitCode.ElementAt(0) == mainUnitMultiplier.ElementAt(0) &&
                        fractionUnitCode.ElementAt(1) < mainUnitMultiplier.ElementAt(1))
                    {
                        numberValue += double.Parse(parseResultValue?.Number) *
                                       (fractionUnitCode.ElementAt(1) / mainUnitMultiplier.ElementAt(1));
                        result.ResolutionStr += ' ' + parseResult.ResolutionStr;
                        result.Length = parseResult.Start + parseResult.Length - result.Start;
                    }
                    else
                    {
                        // If the fraction unit doesn't match the main unit, finish process this group.
                        if (result != null)
                        {
                            result = CreateResult(result, numberValue, mainUnitValue);
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
                result = CreateResult(result, numberValue, mainUnitValue);
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
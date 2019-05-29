using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.Number.Italian
{
    public class ItalianNumberParserConfiguration : BaseNumberParserConfiguration
    {
        public ItalianNumberParserConfiguration(NumberOptions options)
            : this()
        {
            this.Options = options;
        }

        public ItalianNumberParserConfiguration()
            : this(new CultureInfo(Culture.Italian))
        {
        }

        public ItalianNumberParserConfiguration(CultureInfo ci)
        {
            this.LangMarker = NumbersDefinitions.LangMarker;
            this.CultureInfo = ci;

            this.DecimalSeparatorChar = NumbersDefinitions.DecimalSeparatorChar;
            this.FractionMarkerToken = NumbersDefinitions.FractionMarkerToken;
            this.NonDecimalSeparatorChar = NumbersDefinitions.NonDecimalSeparatorChar;
            this.HalfADozenText = NumbersDefinitions.HalfADozenText;
            this.WordSeparatorToken = NumbersDefinitions.WordSeparatorToken;

            this.WrittenDecimalSeparatorTexts = NumbersDefinitions.WrittenDecimalSeparatorTexts;
            this.WrittenGroupSeparatorTexts = NumbersDefinitions.WrittenGroupSeparatorTexts;
            this.WrittenIntegerSeparatorTexts = NumbersDefinitions.WrittenIntegerSeparatorTexts;
            this.WrittenFractionSeparatorTexts = NumbersDefinitions.WrittenFractionSeparatorTexts;

            this.CardinalNumberMap = NumbersDefinitions.CardinalNumberMap.ToImmutableDictionary();
            this.OrdinalNumberMap = NumbersDefinitions.OrdinalNumberMap.ToImmutableDictionary();
            this.RelativeReferenceOffsetMap = NumbersDefinitions.RelativeReferenceOffsetMap.ToImmutableDictionary();
            this.RelativeReferenceRelativeToMap = NumbersDefinitions.RelativeReferenceRelativeToMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();
            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.Singleline);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexOptions.Singleline);
            this.FractionPrepositionRegex = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexOptions.Singleline);
        }

        public string NonDecimalSeparatorText { get; private set; }

        /*public override long ResolveCompositeNumber(string numberStr)
        {
            if (this.OrdinalNumberMap.ContainsKey(numberStr))
            {
                return this.OrdinalNumberMap[numberStr];
            }

            if (this.CardinalNumberMap.ContainsKey(numberStr))
            {
                return this.CardinalNumberMap[numberStr];
            }

            long value = 0;
            long prevValue = 0;
            long finalValue = 0;
            var strBuilder = new StringBuilder();
            for (int i = 0; i < numberStr.Length; i++)
            {
                strBuilder.Append(numberStr[i]);

                if (this.CardinalNumberMap.ContainsKey(strBuilder.ToString()) || ((this.CardinalNumberMap.ContainsKey(string.Concat(strBuilder.ToString(), 'i')) || this.CardinalNumberMap.ContainsKey(string.Concat(strBuilder.ToString(), 'a'))) && i + 1 < numberStr.Length && (numberStr[i + 1] == 'o' || numberStr[i + 1] == 'u')))
                {
                    if (!this.CardinalNumberMap.ContainsKey(strBuilder.ToString()))
                    {
                        if (this.CardinalNumberMap.ContainsKey(string.Concat(strBuilder.ToString(), 'i')))
                        {
                            strBuilder.Append('i');
                        }

                        if (this.CardinalNumberMap.ContainsKey(string.Concat(strBuilder.ToString(), 'a')))
                        {
                            strBuilder.Append('a');
                        }
                    }

                    value = this.CardinalNumberMap[strBuilder.ToString()];
                    if (prevValue > 0 && value > prevValue)
                    {
                        value = (prevValue * value) - prevValue;
                    }

                    finalValue += value;
                    if (prevValue < 1000)
                    {
                        prevValue = value + prevValue;
                    }
                    else
                    {
                        prevValue = value;
                    }

                    strBuilder.Clear();
                }

                else if (this.OrdinalNumberMap.ContainsKey(strBuilder.ToString()) || ((this.OrdinalNumberMap.ContainsKey(string.Concat(strBuilder.ToString(), 'i')) || this.OrdinalNumberMap.ContainsKey(string.Concat(strBuilder.ToString(), 'a'))) && i + 1 < numberStr.Length && (numberStr[i + 1] == 'o' || numberStr[i + 1] == 'u')))
                {
                    if (!this.OrdinalNumberMap.ContainsKey(strBuilder.ToString()))
                    {
                        if (this.OrdinalNumberMap.ContainsKey(string.Concat(strBuilder.ToString(), 'i')))
                        {
                            strBuilder.Append('i');
                        }

                        if (this.OrdinalNumberMap.ContainsKey(string.Concat(strBuilder.ToString(), 'a')))
                        {
                            strBuilder.Append('a');
                        }
                    }

                    value = this.OrdinalNumberMap[strBuilder.ToString()];
                    if (prevValue > 0 && value > prevValue)
                    {
                        value = (prevValue * value) - prevValue;
                    }

                    finalValue += value;
                    if (prevValue < 1000)
                    {
                        prevValue = value + prevValue;
                    }
                    else
                    {
                        prevValue = value;
                    }

                    strBuilder.Clear();
                }
            }

            return finalValue;
        }*/
    }
}

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.NumberWithUnit.Utilities;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public interface INumberWithUnitParserConfiguration
    {
        IDictionary<string, string> UnitMap { get; }

        IDictionary<string, long> CurrencyFractionNumMap { get; }

        CultureInfo CultureInfo { get; }

        IParser InternalNumberParser { get; }

        IExtractor InternalNumberExtractor { get; }

        string ConnectorToken { get; }

        void BindDictionary(IDictionary<string, string> dictionary);
    }

    public abstract class BaseNumberWithUnitParserConfiguration : INumberWithUnitParserConfiguration
    {
        protected BaseNumberWithUnitParserConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;
            this.UnitMap = new Dictionary<string, string>();
            this.CurrencyFractionNumMap = BaseCurrency.CurrencyFractionalRatios.ToImmutableDictionary();
            this.CurrencyFractionMapping = BaseCurrency.CurrencyFractionMapping.ToImmutableDictionary();
            this.CurrencyNameToIsoCodeMap = new Dictionary<string, string>();
            this.CurrencyFractionCodeList = new Dictionary<string, string>();
        }

        public IDictionary<string, string> UnitMap { get; }

        public IDictionary<string, long> CurrencyFractionNumMap { get; }

        public IDictionary<string, string> CurrencyFractionMapping { get; }

        public CultureInfo CultureInfo { get; }

        public abstract IParser InternalNumberParser { get; }

        public abstract IExtractor InternalNumberExtractor { get; }

        public abstract string ConnectorToken { get; }

        public IDictionary<string, string> CurrencyNameToIsoCodeMap { get; set; }

        public IDictionary<string, string> CurrencyFractionCodeList { get; set; }

        public void BindDictionary(IDictionary<string, string> dictionary)
        {
           DictionaryUtils.BindDictionary(dictionary, UnitMap);
        }
    }
}
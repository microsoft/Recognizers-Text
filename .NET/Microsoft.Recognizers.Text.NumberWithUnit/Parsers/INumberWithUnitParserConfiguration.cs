using System.Collections.Generic;
using System.Globalization;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public interface INumberWithUnitParserConfiguration
    {
        IDictionary<string, string> UnitMap { get; }

        #region Language settings

        CultureInfo CultureInfo { get; }

        IParser InternalNumberParser { get; }

        IExtractor InternalNumberExtractor { get; }

        string ConnectorToken { get; }

        #endregion

        void BindDictionary(IDictionary<string, string> dictionary);
    }

    public abstract class BaseNumberWithUnitParserConfiguration : INumberWithUnitParserConfiguration
    {
        public IDictionary<string, string> UnitMap { get; }

        public CultureInfo CultureInfo { get; }

        public abstract IParser InternalNumberParser { get; }

        public abstract IExtractor InternalNumberExtractor { get; }

        public abstract string ConnectorToken { get; }

        protected BaseNumberWithUnitParserConfiguration(CultureInfo ci)
        {
            this.CultureInfo = ci;
            this.UnitMap = new Dictionary<string, string>();
        }
        
        public void BindDictionary(IDictionary<string, string> dictionary)
        {
            if (dictionary == null) return;

            foreach (var pair in dictionary)
            {
                if (string.IsNullOrEmpty(pair.Key))
                {
                    continue;
                }
                var key = pair.Key;
                var values = pair.Value.Trim().Split('|');

                foreach (var token in values)
                {
                    if (string.IsNullOrWhiteSpace(token) || UnitMap.ContainsKey(token))
                    {
                        continue;
                    }
                    UnitMap.Add(token, key);
                }
            }
        }
    }
}
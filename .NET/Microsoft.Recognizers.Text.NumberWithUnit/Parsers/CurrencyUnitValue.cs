using System.Collections.Generic;

using Microsoft.Recognizers.Text.NumberWithUnit.Utilities;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class CurrencyUnitValue
    {
        private string number = string.Empty;
        private string unit = string.Empty;
        private string isoCurrency = string.Empty;

        public string Number { get => number; set => number = value; }

        public string Unit { get => unit; set => unit = value; }

        public string IsoCurrency { get => isoCurrency; set => isoCurrency = value; }
    }
}
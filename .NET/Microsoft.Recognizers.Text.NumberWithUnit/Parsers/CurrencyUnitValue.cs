using System.Collections.Generic;

using Microsoft.Recognizers.Text.NumberWithUnit.Utilities;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class CurrencyUnitValue : UnitValue
    {
        public string IsoCurrency { get; set; } = string.Empty;
    }
}
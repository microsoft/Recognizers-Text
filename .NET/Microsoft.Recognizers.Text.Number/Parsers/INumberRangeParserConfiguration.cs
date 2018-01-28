using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.Number
{
    public interface INumberRangeParserConfiguration
    {

        #region Internal Parsers

        IExtractor NumberExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IParser NumberParser { get; }

        #endregion

    }
}

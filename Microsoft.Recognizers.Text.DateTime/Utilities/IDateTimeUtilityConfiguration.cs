using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public interface IDateTimeUtilityConfiguration
    {
        List<string> AgoStringList { get; }

        List<string> LaterStringList { get; }

        List<string> InStringList { get; }
    }
}

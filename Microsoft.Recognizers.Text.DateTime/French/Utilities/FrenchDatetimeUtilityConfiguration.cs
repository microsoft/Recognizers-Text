using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French.Utilities
{
    public class FrenchDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public static readonly List<string> AgoStringList = new List<string>
        {
            "ago",
        };

        public static readonly List<string> LaterStringList = new List<string>
        {
            "plus tard",
            // "from now"
        };

        public static readonly List<string> InStringList = new List<string>
        {
            // two propositions - "en" and "dans" which differ depending on meaning/grammer/context 
            "in",
        };

        List<string> IDateTimeUtilityConfiguration.AgoStringList => AgoStringList;
        List<string> IDateTimeUtilityConfiguration.LaterStringList => LaterStringList;
        List<string> IDateTimeUtilityConfiguration.InStringList => InStringList;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Utilities
{
    public class SpanishDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public static readonly List<string> AgoStringList = new List<string>
        {

        };

        public static readonly List<string> LaterStringList = new List<string>
        {

        };

        public static readonly List<string> InStringList = new List<string>
        {

        };

        List<string> IDateTimeUtilityConfiguration.AgoStringList => AgoStringList;
        List<string> IDateTimeUtilityConfiguration.LaterStringList => LaterStringList;
        List<string> IDateTimeUtilityConfiguration.InStringList => InStringList;
    }
}

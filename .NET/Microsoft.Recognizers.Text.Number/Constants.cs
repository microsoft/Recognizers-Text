﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Recognizers.Text.Number
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:Field names should not contain underscore", Justification = "Constant names are written in upper case so they can be readily distinguished from camel case variable names.")]
    public static class Constants
    {
        public const string SYS_NUM_CARDINAL = "builtin.num.cardinal";
        public const string SYS_NUM_DOUBLE = "builtin.num.double";
        public const string SYS_NUM_FRACTION = "builtin.num.fraction";
        public const string SYS_NUM_INTEGER = "builtin.num.integer";
        public const string SYS_NUM = "builtin.num";
        public const string SYS_NUM_ORDINAL = "builtin.num.ordinal";
        public const string SYS_NUM_PERCENTAGE = "builtin.num.percentage";
        public const string SYS_NUMRANGE = "builtin.num.numberrange";

        // Model type name
        public const string MODEL_NUMBER = "number";
        public const string MODEL_NUMBERRANGE = "numberrange";
        public const string MODEL_ORDINAL = "ordinal";
        public const string MODEL_ORDINAL_RELATIVE = "ordinal.relative";
        public const string MODEL_PERCENTAGE = "percentage";

        // NARROW NO-BREAK SPACE
        public const char NO_BREAK_SPACE = '\u202f';

        // Language Markers - ISO 639-2 B
        public const string ENGLISH = "Eng";
        public const string CHINESE = "Chi";
        public const string FRENCH = "Fre";
        public const string GERMAN = "Ger";
        public const string JAPANESE = "Jpn";
        public const string PORTUGUESE = "Por";
        public const string SPANISH = "Spa";
        public const string DUTCH = "Dut";
        public const string KOREAN = "Kor";
        public const string ITALIAN = "Ita";
        public const string SWEDISH = "Swe";
        public const string BULGARIAN = "Bul";
        public const string TURKISH = "Tur";
        public const string HINDI = "Hin";
        public const string ARABIC = "Ara";

        /// <summary>
        /// Substitute for language markers for Japanese.
        /// </summary>
        public const string JAPANESE_SUBS = "Jap";

        // Regex Prefixes / Suffixes
        public const string FRACTION_PREFIX = "Frac";
        public const string DOUBLE_PREFIX = "Double";
        public const string INTEGER_PREFIX = "Integer";
        public const string ORDINAL_PREFIX = "Ordinal";
        public const string PERCENT_PREFIX = "Percent";
        public const string NUMBER_SUFFIX = "Num";
        public const string POWER_SUFFIX = "Pow";
        public const string SPECIAL_SUFFIX = "Spe";

        // Ordinal.relative attribute values
        public const string RELATIVE_START = "start";
        public const string RELATIVE_END = "end";
        public const string RELATIVE_CURRENT = "current";

        // Groups' names for named groups in regexes
        public const string RelativeOrdinalGroupName = "relativeOrdinal";

        // Number subtypes
        public const string INTEGER = "integer";
        public const string DECIMAL = "decimal";
        public const string FRACTION = "fraction";
        public const string POWER = "power";

        public static readonly HashSet<string> ValidSubTypes = new HashSet<string>()
        {
            INTEGER,
            DECIMAL,
            FRACTION,
            POWER,
        };
    }
}
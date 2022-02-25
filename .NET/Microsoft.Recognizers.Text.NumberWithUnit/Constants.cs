// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310: CSharp.Naming : Field names must not contain underscores.", Justification = "Constant names are written in upper case so they can be readily distinguished from camel case variable names.")]
    public static class Constants
    {
        public const string SYS_UNIT_DIMENSION = "builtin.unit.dimension";
        public const string SYS_UNIT = "builtin.unit";
        public const string SYS_UNIT_AGE = "builtin.unit.age";
        public const string SYS_UNIT_AREA = "builtin.unit.area";
        public const string SYS_UNIT_CURRENCY = "builtin.unit.currency";
        public const string SYS_UNIT_LENGTH = "builtin.unit.length";
        public const string SYS_UNIT_SPEED = "builtin.unit.speed";
        public const string SYS_UNIT_TEMPERATURE = "builtin.unit.temperature";
        public const string SYS_UNIT_VOLUME = "builtin.unit.volume";
        public const string SYS_UNIT_WEIGHT = "builtin.unit.weight";
        public const string SYS_UNIT_ANGLE = "builtin.unit.angle";
        public const string SYS_NUM = "builtin.num";

        public const string INFORMATION = "Information";
        public const string AREA = "Area";
        public const string LENGTH = "Length";
        public const string SPEED = "Speed";
        public const string VOLUME = "Volume";
        public const string WEIGHT = "Weight";
        public const string ANGLE = "Angle";

        // For cases like '2:00 pm', both 'pm' and '00 pm' are not dimension
        public const string AMBIGUOUS_TIME_TERM = BaseUnits.AmbiguousTimeTerm;

        // In certain cultures the unit can be split around the number,
        // e.g. in Japanese "秒速100メートル" ('speed per second 100 meters' = 100m/s).
        // This flag tells the recognizer to combine prefix and suffix in order to parse the unit correctly.
        public const string PARTIAL_UNIT = "Partial unit";

        // For currencies without ISO codes, we use internal values prefixed by '_'.
        // These values should never be present in parse output.
        public const string FAKE_ISO_CODE_PREFIX = "_";

        public static readonly HashSet<string> ValidSubTypes = new HashSet<string>()
        {
            INFORMATION,
            AREA,
            LENGTH,
            SPEED,
            VOLUME,
            WEIGHT,
            ANGLE,
        };
    }
}
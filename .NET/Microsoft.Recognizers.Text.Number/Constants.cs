// ReSharper disable InconsistentNaming

namespace Microsoft.Recognizers.Text.Number
{
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
        public const string MODEL_PERCENTAGE = "percentage";

        // NARROW NO-BREAK SPACE
        public const char NO_BREAK_SPACE = '\u202f';

        // Fraction
        public const string FRACTION_IN_PURENUMBER = "FracNum";
        public const string FRACTION_WITH_CONNECTOR = "FracEng";
    }
}
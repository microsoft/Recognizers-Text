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

        // Number range regex type
        public const string TWONUM = "TwoNum";
        public const string TWONUMBETWEEN = "TwoNumBetween";
        public const string TWONUMTILL = "TwoNumTill";
        public const string MORE = "More";
        public const string LESS = "Less";
        public const string EQUAL = "Equal";

        // Brackets and comma for number range resolution value
        public const char LEFT_OPEN = '(';
        public const char RIGHT_OPEN = ')';
        public const char LEFT_CLOSED = '[';
        public const char RIGHT_CLOSED = ']';
        public const char COMMA = ',';
    }
}
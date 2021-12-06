// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Recognizers.Text.Number
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310: CSharp.Naming : Field names must not contain underscores.", Justification = "Constant names are written in upper case so they can be readily distinguished from camel case variable names.")]
    public static class NumberRangeConstants
    {
        // Number range regex type
        public const string TWONUM = "TwoNum";
        public const string TWONUMBETWEEN = "TwoNumBetween";
        public const string TWONUMTILL = "TwoNumTill";
        public const string TWONUMCLOSED = "TwoNumClosed";
        public const string MORE = "More";

        // 以下为中文的“三万多”构造一个专门的属性
        public const string ABOUT = "About";

        public const string LESS = "Less";
        public const string EQUAL = "Equal";

        // Brackets and comma for number range resolution value
        public const char LEFT_OPEN = '(';
        public const char RIGHT_OPEN = ')';
        public const char LEFT_CLOSED = '[';
        public const char RIGHT_CLOSED = ']';
        public const char INTERVAL_SEPARATOR = ',';

        // Invalid number
        public const int INVALID_NUM = -1;
    }
}

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Recognizers.Text.Choice
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310: CSharp.Naming : Field names must not contain underscores.", Justification = "Constant names are written in upper case so they can be readily distinguished from camel case variable names.")]
    public static class Constants
    {
        public const string SYS_BOOLEAN_TRUE = "boolean_true";
        public const string SYS_BOOLEAN_FALSE = "boolean_false";

        // Model type name
        public const string MODEL_BOOLEAN = "boolean";
    }
}
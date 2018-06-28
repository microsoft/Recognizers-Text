using System;

namespace Microsoft.Recognizers.Text.Utilities
{
    public static class EnumUtils
    {

        public static bool IsFlagDefined(Enum en)
        {

            // For enums with FlagsAttribute, IsDefined() doesn't work.
            // ToString() will return a comma-separated list of enum string values if defined and an int if not.
            return (!int.TryParse(en.ToString(), out int val));
        }

        public static T Convert<T>(int value) where T : struct 
        {

            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException("Invalid Enum Type. " + typeof(T).ToString() + "  must be an Enum.");
            }

            Type enumType = typeof(T);
            var returnEnum = (T)Enum.ToObject(enumType, value);

            if (IsFlagDefined((Enum)(object)returnEnum))
            {
                return returnEnum;
            }
            else
            { 
                throw new ArgumentOutOfRangeException(value.ToString(), "Bad configuration parameter value.");
            }
        }

    }
}

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Recognizers.Text.Sequence
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310: CSharp.Naming : Field names must not contain underscores.", Justification = "Constant names are written in upper case so they can be readily distinguished from camel case variable names.")]
    public static class Constants
    {
        public const string SYS_PHONE_NUMBER = "builtin.phonenumber";

        public const string SYS_IP = "builtin.ip";

        public const string SYS_MENTION = "builtin.mention";

        public const string SYS_HASHTAG = "builtin.hashtag";

        public const string SYS_EMAIL = "builtin.email";

        public const string SYS_URL = "builtin.url";

        public const string SYS_GUID = "builtin.guid";

        // Model type name
        public const string MODEL_PHONE_NUMBER = "phonenumber";

        public const string MODEL_IP = "ip";

        public const string MODEL_MENTION = "mention";

        public const string MODEL_HASHTAG = "hashtag";

        public const string MODEL_EMAIL = "email";

        public const string MODEL_URL = "url";

        public const string MODEL_GUID = "guid";

        public const string IP_REGEX_IPV4 = "ipv4";

        public const string IP_REGEX_IPV6 = "ipv6";

        public const string IPV6_ELLIPSIS = "::";

        public const string PHONE_NUMBER_REGEX_GENERAL = "GeneralPhoneNumber";

        public const string PHONE_NUMBER_REGEX_BR = "BRPhoneNumber";

        public const string PHONE_NUMBER_REGEX_UK = "UKPhoneNumber";

        public const string PHONE_NUMBER_REGEX_DE = "DEPhoneNumber";

        public const string PHONE_NUMBER_REGEX_US = "USPhoneNumber";

        public const string PHONE_NUMBER_REGEX_CN = "CNPhoneNumber";

        public const string PHONE_NUMBER_REGEX_DK = "DKPhoneNumber";

        public const string PHONE_NUMBER_REGEX_IT = "ITPhoneNumber";

        public const string PHONE_NUMBER_REGEX_NL = "NLPhoneNumber";

        public const string PHONE_NUMBER_REGEX_SPECIAL = "SpecialPhoneNumber";

        public const string MENTION_REGEX = "Mention";

        public const string HASHTAG_REGEX = "Hashtag";

        public const string EMAIL_REGEX = "Email";

        public const string URL_REGEX = "Url";

        public const string GUID_REGEX = "Guid";
    }
}
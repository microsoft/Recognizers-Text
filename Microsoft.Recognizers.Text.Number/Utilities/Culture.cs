using System.Linq;

namespace Microsoft.Recognizers.Text.Number.Utilities
{
    public sealed class Culture
    {
        public const string English = "en-us";
        public const string Chinese = "zh-cn";
        public const string Spanish = "es-es";

        public string CultureName;
        public string CultureCode;

        public static readonly Culture[] SupportedCultures = new Culture[] {
            new Culture("English", English),
            new Culture("Chinese", Chinese),
            new Culture("Spanish", Spanish),
        };

        private Culture(string cultureName, string cultureCode)
        {
            this.CultureName = cultureName;
            this.CultureCode = cultureCode;
        }

        public static string[] GetSupportedCultureCodes()
        {
            return SupportedCultures.Select(c => c.CultureCode).ToArray();
        }
    }
}

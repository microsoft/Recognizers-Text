using System.Linq;

namespace Microsoft.Recognizers.Text
{
    public sealed class Culture
    {
        public const string English = "en-us";
        public const string Chinese = "zh-cn";
        public const string Spanish = "es-es";
        public const string Portuguese = "pt-br";
        public const string French = "fr-fr";
        public const string German = "de-de";
        public const string Japanese = "ja-jp";
        public const string Dutch = "nl-nl";
            
        public string CultureName;
        public string CultureCode;

        public static readonly Culture[] SupportedCultures = {
            new Culture("English", English),
            new Culture("Chinese", Chinese),
            new Culture("Spanish", Spanish),
            new Culture("Portuguese", Portuguese),
            new Culture("French", French),
            new Culture("German", German),
            new Culture("Japanese", Japanese),
            new Culture("Dutch", Dutch)
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

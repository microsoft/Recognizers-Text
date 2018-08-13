using System.Linq;

namespace Microsoft.Recognizers.Text
{
    public sealed class Culture
    {
        public const string English = "en-us";
        public const string EnglishOthers = "en-*";
        public const string Chinese = "zh-cn";
        public const string Spanish = "es-es";
        public const string Portuguese = "pt-br";
        public const string French = "fr-fr";
        public const string German = "de-de";
        public const string Japanese = "ja-jp";
        public const string Dutch = "nl-nl";
        public const string Korean = "ko-kr";

        public readonly string CultureName;
        public readonly string CultureCode;

        public static readonly Culture[] SupportedCultures = {
            new Culture("EnglishOthers", EnglishOthers),
            new Culture("English", English),
            new Culture("Chinese", Chinese),
            new Culture("Spanish", Spanish),
            new Culture("Portuguese", Portuguese),
            new Culture("French", French),
            new Culture("German", German),
            new Culture("Japanese", Japanese),
            new Culture("Dutch", Dutch),
            new Culture("Korean", Korean)
        };

        private static readonly string[] SupportedCultureCodes = SupportedCultures.Select(c => c.CultureCode).ToArray();
        
        private Culture(string cultureName, string cultureCode)
        {
            this.CultureName = cultureName;
            this.CultureCode = cultureCode;
        }

        public static string[] GetSupportedCultureCodes()
        {
            return SupportedCultureCodes;
        }

        // If the input culture code isn't in the supported cultures list,
        // the first culture with the same prefix as the input one will be returned.
        // Otherwise, the original input culture code will be returned.
        // e.g. "en-029"->"en-*" "vo-id"->"vo-id" "en-us"->"en-us"
        public static string MapToMoreSpecificLanguage(string cultureCode)
        {
            cultureCode = cultureCode.ToLowerInvariant();

            if (SupportedCultureCodes.All(o => o != cultureCode))
            {
                var fallbackCultureCodes = SupportedCultureCodes
                    .Where(o => o.EndsWith("*") && cultureCode.StartsWith(o.Split('-').First())).ToList();

                if (fallbackCultureCodes.Count == 1)
                {
                    return fallbackCultureCodes.First();
                }
            }

            return cultureCode;
        }
    }
}

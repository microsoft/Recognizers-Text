using System;
using System.Linq;

namespace Microsoft.Recognizers.Text
{
    public sealed class Culture
    {
        public const string Arabic = "ar-*";
        public const string English = "en-us";
        public const string EnglishOthers = "en-*";
        public const string Chinese = "zh-cn";
        public const string Spanish = "es-es";
        public const string SpanishMexican = "es-mx"; // Temporary workaround for language variant config issue
        public const string Portuguese = "pt-br";
        public const string French = "fr-fr";
        public const string German = "de-de";
        public const string Italian = "it-it";
        public const string Japanese = "ja-jp";
        public const string Dutch = "nl-nl";
        public const string Korean = "ko-kr";
        public const string Swedish = "sv-se";
        public const string Bulgarian = "bg-bg";
        public const string Turkish = "tr-tr";
        public const string Hindi = "hi-in";

        public static readonly Culture[] SupportedCultures =
        {
            new Culture("Arabic", Arabic),
            new Culture("EnglishOthers", EnglishOthers),
            new Culture("English", English),
            new Culture("Chinese", Chinese),
            new Culture("Spanish", Spanish),
            new Culture("SpanishMexican", SpanishMexican),
            new Culture("Portuguese", Portuguese),
            new Culture("French", French),
            new Culture("German", German),
            new Culture("Italian", Italian),
            new Culture("Japanese", Japanese),
            new Culture("Dutch", Dutch),
            new Culture("Korean", Korean),
            new Culture("Swedish", Swedish),
            new Culture("Bulgarian", Bulgarian),
            new Culture("Turkish", Turkish),
            new Culture("Hindi", Hindi),
        };

        private static readonly string[] SupportedCultureCodes = SupportedCultures.Select(c => c.CultureCode).ToArray();

        private Culture(string cultureName, string cultureCode)
        {
            this.CultureName = cultureName;
            this.CultureCode = cultureCode;
        }

        public string CultureName { get; private set; }

        public string CultureCode { get; private set; }

        public static string[] GetSupportedCultureCodes()
        {
            return SupportedCultureCodes;
        }

        // If the input culture code isn't in the supported cultures list,
        // the first culture with the same prefix as the input one will be returned.
        // Otherwise, the original input culture code will be returned.
        // e.g. "en-029"->"en-*" "vo-id"->"vo-id" "en-us"->"en-us"
        public static string MapToNearestLanguage(string cultureCode)
        {
            cultureCode = cultureCode.ToLowerInvariant();

            if (SupportedCultureCodes.All(o => o != cultureCode))
            {
                // Handle cases like EnglishOthers with cultureCode "en-*"
                var fallbackCultureCodes = SupportedCultureCodes
                    .Where(o => o.EndsWith("*", StringComparison.Ordinal) &&
                                cultureCode.StartsWith(o.Split('-').First(), StringComparison.Ordinal)).ToList();

                if (fallbackCultureCodes.Count == 1)
                {
                    return fallbackCultureCodes.First();
                }

                // If there is no cultureCode like "-*", map only the prefix
                // For example, "es-mx" will be mapped to "es-es"
                fallbackCultureCodes = SupportedCultureCodes
                    .Where(o => cultureCode.StartsWith(o.Split('-').First(), StringComparison.Ordinal)).ToList();

                if (fallbackCultureCodes.Any())
                {
                    return fallbackCultureCodes.First();
                }
            }

            return cultureCode;
        }
    }
}

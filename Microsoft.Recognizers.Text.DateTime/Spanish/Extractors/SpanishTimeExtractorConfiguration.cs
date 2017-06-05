using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishTimeExtractorConfiguration : ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------

        public static readonly Regex DescRegex = new Regex(@"(?<desc>pm\b|am\b|p\.m\.|a\.m\.)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourNumRegex =
            new Regex(@"(?<hournum>cero|una|dos|tres|cuatro|cinco|seis|siete|ocho|nueve|diez|once|doce)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MinuteNumRegex =
            new Regex(
                @"(?<minnum>un|dos|tres|cuatro|cinco|seis|siete|ocho|nueve|diez|once|doce|trece|catorce|quince|dieciseis|diecisiete|dieciocho|diecinueve|veinte|treinta|cuarenta|cincuenta)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 2: middle level component
        // --------------------------------------
        // handle "... en punto"
        public static readonly Regex OclockRegex = new Regex(@"(?<oclock>en\s+punto)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... tarde"
        public static readonly Regex PmRegex =
            new Regex(@"(?<pm>((por|de|a|en)\s+la)\s+(tarde|noche))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... de la mañana"
        public static readonly Regex AmRegex = new Regex(@"(?<am>((por|de|a|en)\s+la)\s+(mañana|madrugada))",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "y media ..." "menos cuarto ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(
                $@"(?<lth>((\s+y\s+)?cuarto|(\s*)menos cuarto|(\s+y\s+)media|{
                    BaseTimeExtractor.MinuteRegex.ToString().Replace("min", "deltamin")}(\s+(minuto|minutos|min|mins))|{
                    MinuteNumRegex.ToString().Replace("minnum", "deltaminnum")}(\s+(minuto|minutos|min|mins))))", 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TensTimeRegex =
            new Regex("(?<tens>diez|veint(i|e)|treinta|cuarenta|cincuenta)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "seis treinta", "seis veintiuno", "seis menos diez"
        public static readonly Regex EngTimeRegex =
            new Regex(
                $@"(?<engtime>{HourNumRegex}\s*((y|menos)\s+)?({MinuteNumRegex}|({TensTimeRegex}((\s*y\s+)?{MinuteNumRegex})?)))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public static readonly Regex TimePrefix =
            new Regex(
                $@"(?<prefix>{LessThanOneHour}(\s+(pasad[ao]s)\s+(de\s+las|las)?|\s+(para|antes\s+de)?\s+(las?))?)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeSuffix =
            new Regex($@"(?<suffix>({LessThanOneHour}\s+)?({AmRegex}|{PmRegex}|{OclockRegex}))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BasicTime =
            new Regex(
                $@"(?<basictime>{EngTimeRegex}|{HourNumRegex}|{BaseTimeExtractor.HourRegex}:{BaseTimeExtractor.MinuteRegex}(:{BaseTimeExtractor.SecondRegex})?|{BaseTimeExtractor.HourRegex})", 
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        // part 3: regex for time
        // --------------------------------------
        // handle "a las cuatro" "a las 3"
        public static readonly Regex AtRegex =
            new Regex(string.Format(@"\b(?<=\b(a las?)\s+)({2}|{0}|{1})\b", HourNumRegex, BaseTimeExtractor.HourRegex, EngTimeRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConnectNumRegex =
            new Regex(
                $@"{BaseTimeExtractor.HourRegex
                    }(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\s*{
                    DescRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeRegexList =
        {
            // (tres min pasadas las)? siete|7|(siete treinta) pm
            new Regex(
                $@"(\b{TimePrefix}\s+)?({EngTimeRegex}|{HourNumRegex}|{BaseTimeExtractor.HourRegex})\s*({DescRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres min pasadas las)? 3:00(:00)? (pm)?
            new Regex(
                $@"(\b{TimePrefix}\s+)?(T)?{BaseTimeExtractor.HourRegex}(\s*)?:(\s*)?{BaseTimeExtractor.MinuteRegex}((\s*)?:(\s*)?{BaseTimeExtractor.SecondRegex})?((\s*{DescRegex})|\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres min pasadas las)? 3.00 (pm)?
            new Regex($@"(\b{TimePrefix}\s+)?{BaseTimeExtractor.HourRegex}\.{BaseTimeExtractor.MinuteRegex}(\s*{DescRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),
            
            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex($@"\b(({DescRegex}?)|({BasicTime}?)({DescRegex}?))({TimePrefix}\s*)({HourNumRegex}|{BaseTimeExtractor.HourRegex})?(\s+{TensTimeRegex}(\s+y\s+)?{MinuteNumRegex}?)?({OclockRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres min pasadas las) (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex($@"\b({TimePrefix}|{BasicTime}{TimePrefix})\s+(\s*{DescRegex})?{BasicTime}?\s*{TimeSuffix}\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (cinco treinta|siete|7|7:00(:00)?) (pm)? (de la noche)
            new Regex($@"{BasicTime}(\s*{DescRegex})?\s+{TimeSuffix}\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (En la noche) a las (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex($@"\b{TimeSuffix}\s+a\s+las\s+{BasicTime}((\s*{DescRegex})|\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (En la noche) (cinco treinta|siete|7|7:00(:00)?) (pm)?
            new Regex($@"\b{TimeSuffix}\s+{BasicTime}((\s*{DescRegex})|\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),
            
            //once (y)? veinticinco
            new Regex($@"\b(?<engtime>{HourNumRegex}\s+({TensTimeRegex}\s*)?(y\s+)?{MinuteNumRegex}?)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            new Regex($@"(a\s+la|al)\s+(madrugada|mañana|medio\s*d[ií]a|tarde|noche)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (tres menos veinte) (pm)?
            new Regex($@"\b({EngTimeRegex})({DescRegex}?)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 340pm
            ConnectNumRegex
        };

        Regex ITimeExtractorConfiguration.IshRegex => null;

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;
    }
}

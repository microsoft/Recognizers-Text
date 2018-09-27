﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//     
//     Generation parameters:
//     - DataFilename: Patterns\Spanish\Spanish-Numbers.yaml
//     - Language: Spanish
//     - ClassName: NumbersDefinitions
// </auto-generated>
//------------------------------------------------------------------------------
namespace Microsoft.Recognizers.Definitions.Spanish
{
	using System;
	using System.Collections.Generic;

	public static class NumbersDefinitions
	{
		public const string LangMarker = "Spa";
		public const string HundredsNumberIntegerRegex = @"(cuatrocient[ao]s|trescient[ao]s|seiscient[ao]s|setecient[ao]s|ochocient[ao]s|novecient[ao]s|doscient[ao]s|quinient[ao]s|(?<!por\s+)(cien(to)?))";
		public const string RoundNumberIntegerRegex = @"(mil millones|mil|millones|mill[oó]n|billones|bill[oó]n|trillones|trill[oó]n|cuatrillones|cuatrill[oó]n|quintillones|quintill[oó]n|sextillones|sextill[oó]n|septillones|septill[oó]n)";
		public const string ZeroToNineIntegerRegex = @"(cuatro|cinco|siete|nueve|cero|tres|seis|ocho|dos|un[ao]?)";
		public const string TenToNineteenIntegerRegex = @"(diecisiete|diecinueve|diecis[eé]is|dieciocho|catorce|quince|trece|diez|once|doce)";
		public const string TwentiesIntegerRegex = @"(veinticuatro|veinticinco|veintisiete|veintinueve|veintitr[eé]s|veintis[eé]is|veintiocho|veintid[oó]s|ventiun[ao]|veinti[uú]n[oa]?|veinte)";
		public const string TensNumberIntegerRegex = @"(cincuenta|cuarenta|treinta|sesenta|setenta|ochenta|noventa)";
		public const string NegativeNumberTermsRegex = @"^[.]";
		public static readonly string NegativeNumberSignRegex = $@"^({NegativeNumberTermsRegex}\s+).*";
		public const string DigitsNumberRegex = @"\d|\d{1,3}(\.\d{3})";
		public static readonly string BelowHundredsRegex = $@"(({TenToNineteenIntegerRegex}|{TwentiesIntegerRegex}|({TensNumberIntegerRegex}(\s+y\s+{ZeroToNineIntegerRegex})?))|{ZeroToNineIntegerRegex})";
		public static readonly string BelowThousandsRegex = $@"({HundredsNumberIntegerRegex}(\s+{BelowHundredsRegex})?|{BelowHundredsRegex})";
		public static readonly string SupportThousandsRegex = $@"(({BelowThousandsRegex}|{BelowHundredsRegex})\s+{RoundNumberIntegerRegex}(\s+{RoundNumberIntegerRegex})?)";
		public static readonly string SeparaIntRegex = $@"({SupportThousandsRegex}(\s+{SupportThousandsRegex})*(\s+{BelowThousandsRegex})?|{BelowThousandsRegex})";
		public static readonly string AllIntRegex = $@"({SeparaIntRegex}|mil(\s+{BelowThousandsRegex})?)";
		public const string PlaceHolderPureNumber = @"\b";
		public const string PlaceHolderDefault = @"\D|\b";
		public static readonly Func<string, string> NumbersWithPlaceHolder = (placeholder) => $@"(((?<!\d+\s*)-\s*)|(?<=\b))\d+(?!(,\d+[a-zA-Z]))(?={placeholder})";
		public const string NumbersWithSuffix = @"(((?<=\W|^)-\s*)|(?<=\b))\d+\s*(k|M|T|G)(?=\b)";
		public static readonly string RoundNumberIntegerRegexWithLocks = $@"(?<=\b)({DigitsNumberRegex})+\s+{RoundNumberIntegerRegex}(?=\b)";
		public const string NumbersWithDozenSuffix = @"(((?<=\W|^)-\s*)|(?<=\b))\d+\s+docenas?(?=\b)";
		public static readonly string AllIntRegexWithLocks = $@"((?<=\b){AllIntRegex}(?=\b))";
		public static readonly string AllIntRegexWithDozenSuffixLocks = $@"(?<=\b)(((media\s+)?\s+docena)|({AllIntRegex}\s+(y|con)\s+)?({AllIntRegex}\s+docenas?))(?=\b)";
		public const string SimpleRoundOrdinalRegex = @"(mil[eé]simo|millon[eé]sim[oa]|billon[eé]sim[oa]|trillon[eé]sim[oa]|cuatrillon[eé]sim[oa]|quintillon[eé]sim[oa]|sextillon[eé]sim[oa]|septillon[eé]sim[oa])";
		public const string OneToNineOrdinalRegex = @"(primer[oa]|segund[oa]|tercer[oa]|cuart[oa]|quint[oa]|sext[oa]|s[eé]ptim[oa]|octav[oa]|noven[oa])";
		public const string TensOrdinalRegex = @"(nonag[eé]sim[oa]|octog[eé]sim[oa]|septuag[eé]sim[oa]|sexag[eé]sim[oa]|quincuag[eé]sim[oa]|cuadrag[eé]sim[oa]|trig[eé]sim[oa]|vig[eé]sim[oa]|d[eé]cim[oa])";
		public const string HundredOrdinalRegex = @"(cent[eé]sim[oa]|ducent[eé]sim[oa]|tricent[eé]sim[oa]|cuadringent[eé]sim[oa]|quingent[eé]sim[oa]|sexcent[eé]sim[oa]|septingent[eé]sim[oa]|octingent[eé]sim[oa]|noningent[eé]sim[oa])";
		public const string SpecialUnderHundredOrdinalRegex = @"(und[eé]cim[oa]|duod[eé]cimo|decimoctav[oa])";
		public static readonly string UnderHundredOrdinalRegex = $@"((({TensOrdinalRegex}(\s)?)?{OneToNineOrdinalRegex})|{TensOrdinalRegex}|{SpecialUnderHundredOrdinalRegex})";
		public static readonly string UnderThousandOrdinalRegex = $@"((({HundredOrdinalRegex}(\s)?)?{UnderHundredOrdinalRegex})|{HundredOrdinalRegex})";
		public static readonly string OverThousandOrdinalRegex = $@"(({AllIntRegex})([eé]sim[oa]))";
		public static readonly string ComplexOrdinalRegex = $@"(({OverThousandOrdinalRegex}(\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex})";
		public static readonly string SufixRoundOrdinalRegex = $@"(({AllIntRegex})({SimpleRoundOrdinalRegex}))";
		public static readonly string ComplexRoundOrdinalRegex = $@"((({SufixRoundOrdinalRegex}(\s)?)?{ComplexOrdinalRegex})|{SufixRoundOrdinalRegex})";
		public static readonly string AllOrdinalRegex = $@"{ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex}";
		public const string OrdinalSuffixRegex = @"(?<=\b)(\d*(1r[oa]|2d[oa]|3r[oa]|4t[oa]|5t[oa]|6t[oa]|7m[oa]|8v[oa]|9n[oa]|0m[oa]|11[vm][oa]|12[vm][oa]))(?=\b)";
		public static readonly string OrdinalNounRegex = $@"(?<=\b){AllOrdinalRegex}(?=\b)";
		public static readonly string SpecialFractionInteger = $@"((({AllIntRegex})i?({ZeroToNineIntegerRegex})|({AllIntRegex}))a?v[oa]s?)";
		public const string FractionNotationRegex = @"(((?<=\W|^)-\s*)|(?<=\b))\d+[/]\d+(?=(\b[^/]|$))";
		public const string FractionNotationWithSpacesRegex = @"(((?<=\W|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))";
		public static readonly string FractionNounRegex = $@"(?<=\b)({AllIntRegex}\s+((y|con)\s+)?)?({AllIntRegex})(\s+((y|con)\s)?)((({AllOrdinalRegex})s?|({SpecialFractionInteger})|({SufixRoundOrdinalRegex})s?)|medi[oa]s?|tercios?)(?=\b)";
		public static readonly string FractionNounWithArticleRegex = $@"(?<=\b)({AllIntRegex}\s+(y\s+)?)?(un|un[oa])(\s+)(({AllOrdinalRegex})|({SufixRoundOrdinalRegex})|(y\s+)?medi[oa]s?)(?=\b)";
		public static readonly string FractionPrepositionRegex = $@"(?<=\b)(?<numerator>({AllIntRegex})|((?<!\.)\d+))\s+sobre\s+(?<denominator>({AllIntRegex})|((\d+)(?!\.)))(?=\b)";
		public static readonly string AllPointRegex = $@"((\s+{ZeroToNineIntegerRegex})+|(\s+{AllIntRegex}))";
		public static readonly string AllFloatRegex = $@"{AllIntRegex}(\s+(coma|con)){AllPointRegex}";
		public static readonly Func<string, string> DoubleDecimalPointRegex = (placeholder) => $@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+,)))\d+,\d+(?!(,\d+))(?={placeholder})";
		public static readonly Func<string, string> DoubleWithoutIntegralRegex = (placeholder) => $@"(?<=\s|^)(?<!(\d+)),\d+(?!(,\d+))(?={placeholder})";
		public const string DoubleWithMultiplierRegex = @"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\,)))\d+,\d+\s*(K|k|M|G|T)(?=\b)";
		public static readonly string DoubleWithRoundNumber = $@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\,)))\d+,\d+\s+{RoundNumberIntegerRegex}(?=\b)";
		public static readonly string DoubleAllFloatRegex = $@"((?<=\b){AllFloatRegex}(?=\b))";
		public const string DoubleExponentialNotationRegex = @"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+,)))(\d+(,\d+)?)e([+-]*[1-9]\d*)(?=\b)";
		public const string DoubleCaretExponentialNotationRegex = @"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+,)))(\d+(,\d+)?)\^([+-]*[1-9]\d*)(?=\b)";
		public static readonly string NumberWithPrefixPercentage = $@"(?<!%)({BaseNumbers.NumberReplaceToken})(\s*)(%(?!{BaseNumbers.NumberReplaceToken})|(por ciento|por cien)\b)";
		public const string CurrencyRegex = @"(((?<=\W|^)-\s*)|(?<=\b))\d+\s*(B|b|m|t|g)(?=\b)";
		public const char DecimalSeparatorChar = ',';
		public const string FractionMarkerToken = "sobre";
		public const char NonDecimalSeparatorChar = '.';
		public const string HalfADozenText = "seis";
		public const string WordSeparatorToken = "y";
		public static readonly string[] WrittenDecimalSeparatorTexts = { "coma", "con" };
		public static readonly string[] WrittenGroupSeparatorTexts = { "punto" };
		public static readonly string[] WrittenIntegerSeparatorTexts = { "y" };
		public static readonly string[] WrittenFractionSeparatorTexts = { "con" };
		public const string HalfADozenRegex = @"media\s+docena";
		public const string DigitalNumberRegex = @"((?<=\b)(mil|millones|mill[oó]n|billones|bill[oó]n|trillones|trill[oó]n|docenas?)(?=\b))|((?<=(\d|\b))(k|t|m|g)(?=\b))";
		public static readonly Dictionary<string, long> CardinalNumberMap = new Dictionary<string, long>
		{
			{ "cero", 0 },
			{ "un", 1 },
			{ "una", 1 },
			{ "uno", 1 },
			{ "dos", 2 },
			{ "tres", 3 },
			{ "cuatro", 4 },
			{ "cinco", 5 },
			{ "seis", 6 },
			{ "siete", 7 },
			{ "ocho", 8 },
			{ "nueve", 9 },
			{ "diez", 10 },
			{ "once", 11 },
			{ "doce", 12 },
			{ "docena", 12 },
			{ "docenas", 12 },
			{ "trece", 13 },
			{ "catorce", 14 },
			{ "quince", 15 },
			{ "dieciseis", 16 },
			{ "dieciséis", 16 },
			{ "diecisiete", 17 },
			{ "dieciocho", 18 },
			{ "diecinueve", 19 },
			{ "veinte", 20 },
			{ "ventiuna", 21 },
			{ "ventiuno", 21 },
			{ "veintiun", 21 },
			{ "veintiún", 21 },
			{ "veintiuno", 21 },
			{ "veintiuna", 21 },
			{ "veintidos", 22 },
			{ "veintidós", 22 },
			{ "veintitres", 23 },
			{ "veintitrés", 23 },
			{ "veinticuatro", 24 },
			{ "veinticinco", 25 },
			{ "veintiseis", 26 },
			{ "veintiséis", 26 },
			{ "veintisiete", 27 },
			{ "veintiocho", 28 },
			{ "veintinueve", 29 },
			{ "treinta", 30 },
			{ "cuarenta", 40 },
			{ "cincuenta", 50 },
			{ "sesenta", 60 },
			{ "setenta", 70 },
			{ "ochenta", 80 },
			{ "noventa", 90 },
			{ "cien", 100 },
			{ "ciento", 100 },
			{ "doscientas", 200 },
			{ "doscientos", 200 },
			{ "trescientas", 300 },
			{ "trescientos", 300 },
			{ "cuatrocientas", 400 },
			{ "cuatrocientos", 400 },
			{ "quinientas", 500 },
			{ "quinientos", 500 },
			{ "seiscientas", 600 },
			{ "seiscientos", 600 },
			{ "setecientas", 700 },
			{ "setecientos", 700 },
			{ "ochocientas", 800 },
			{ "ochocientos", 800 },
			{ "novecientas", 900 },
			{ "novecientos", 900 },
			{ "mil", 1000 },
			{ "millon", 1000000 },
			{ "millón", 1000000 },
			{ "millones", 1000000 },
			{ "billon", 1000000000000 },
			{ "billón", 1000000000000 },
			{ "billones", 1000000000000 },
			{ "trillon", 1000000000000000000 },
			{ "trillón", 1000000000000000000 },
			{ "trillones", 1000000000000000000 }
		};
		public static readonly Dictionary<string, long> OrdinalNumberMap = new Dictionary<string, long>
		{
			{ "primero", 1 },
			{ "primera", 1 },
			{ "primer", 1 },
			{ "segundo", 2 },
			{ "segunda", 2 },
			{ "medio", 2 },
			{ "media", 2 },
			{ "tercero", 3 },
			{ "tercera", 3 },
			{ "tercer", 3 },
			{ "tercio", 3 },
			{ "cuarto", 4 },
			{ "cuarta", 4 },
			{ "quinto", 5 },
			{ "quinta", 5 },
			{ "sexto", 6 },
			{ "sexta", 6 },
			{ "septimo", 7 },
			{ "septima", 7 },
			{ "octavo", 8 },
			{ "octava", 8 },
			{ "noveno", 9 },
			{ "novena", 9 },
			{ "decimo", 10 },
			{ "decima", 10 },
			{ "undecimo", 11 },
			{ "undecima", 11 },
			{ "duodecimo", 12 },
			{ "duodecima", 12 },
			{ "decimotercero", 13 },
			{ "decimotercera", 13 },
			{ "decimocuarto", 14 },
			{ "decimocuarta", 14 },
			{ "decimoquinto", 15 },
			{ "decimoquinta", 15 },
			{ "decimosexto", 16 },
			{ "decimosexta", 16 },
			{ "decimoseptimo", 17 },
			{ "decimoseptima", 17 },
			{ "decimoctavo", 18 },
			{ "decimoctava", 18 },
			{ "decimonoveno", 19 },
			{ "decimonovena", 19 },
			{ "vigesimo", 20 },
			{ "vigesima", 20 },
			{ "trigesimo", 30 },
			{ "trigesima", 30 },
			{ "cuadragesimo", 40 },
			{ "cuadragesima", 40 },
			{ "quincuagesimo", 50 },
			{ "quincuagesima", 50 },
			{ "sexagesimo", 60 },
			{ "sexagesima", 60 },
			{ "septuagesimo", 70 },
			{ "septuagesima", 70 },
			{ "octogesimo", 80 },
			{ "octogesima", 80 },
			{ "nonagesimo", 90 },
			{ "nonagesima", 90 },
			{ "centesimo", 100 },
			{ "centesima", 100 },
			{ "ducentesimo", 200 },
			{ "ducentesima", 200 },
			{ "tricentesimo", 300 },
			{ "tricentesima", 300 },
			{ "cuadringentesimo", 400 },
			{ "cuadringentesima", 400 },
			{ "quingentesimo", 500 },
			{ "quingentesima", 500 },
			{ "sexcentesimo", 600 },
			{ "sexcentesima", 600 },
			{ "septingentesimo", 700 },
			{ "septingentesima", 700 },
			{ "octingentesimo", 800 },
			{ "octingentesima", 800 },
			{ "noningentesimo", 900 },
			{ "noningentesima", 900 },
			{ "milesimo", 1000 },
			{ "milesima", 1000 },
			{ "millonesimo", 1000000 },
			{ "millonesima", 1000000 },
			{ "billonesimo", 1000000000000 },
			{ "billonesima", 1000000000000 }
		};
		public static readonly Dictionary<string, long> PrefixCardinalMap = new Dictionary<string, long>
		{
			{ "dos", 2 },
			{ "tres", 3 },
			{ "cuatro", 4 },
			{ "cinco", 5 },
			{ "seis", 6 },
			{ "siete", 7 },
			{ "ocho", 8 },
			{ "nueve", 9 },
			{ "diez", 10 },
			{ "once", 11 },
			{ "doce", 12 },
			{ "trece", 13 },
			{ "catorce", 14 },
			{ "quince", 15 },
			{ "dieciseis", 16 },
			{ "dieciséis", 16 },
			{ "diecisiete", 17 },
			{ "dieciocho", 18 },
			{ "diecinueve", 19 },
			{ "veinte", 20 },
			{ "ventiuna", 21 },
			{ "veintiun", 21 },
			{ "veintiún", 21 },
			{ "veintidos", 22 },
			{ "veintitres", 23 },
			{ "veinticuatro", 24 },
			{ "veinticinco", 25 },
			{ "veintiseis", 26 },
			{ "veintisiete", 27 },
			{ "veintiocho", 28 },
			{ "veintinueve", 29 },
			{ "treinta", 30 },
			{ "cuarenta", 40 },
			{ "cincuenta", 50 },
			{ "sesenta", 60 },
			{ "setenta", 70 },
			{ "ochenta", 80 },
			{ "noventa", 90 },
			{ "cien", 100 },
			{ "doscientos", 200 },
			{ "trescientos", 300 },
			{ "cuatrocientos", 400 },
			{ "quinientos", 500 },
			{ "seiscientos", 600 },
			{ "setecientos", 700 },
			{ "ochocientos", 800 },
			{ "novecientos", 900 }
		};
		public static readonly Dictionary<string, long> SuffixOrdinalMap = new Dictionary<string, long>
		{
			{ "milesimo", 1000 },
			{ "millonesimo", 1000000 },
			{ "billonesimo", 1000000000000 }
		};
		public static readonly Dictionary<string, long> RoundNumberMap = new Dictionary<string, long>
		{
			{ "mil", 1000 },
			{ "milesimo", 1000 },
			{ "millon", 1000000 },
			{ "millón", 1000000 },
			{ "millones", 1000000 },
			{ "millonesimo", 1000000 },
			{ "billon", 1000000000000 },
			{ "billón", 1000000000000 },
			{ "billones", 1000000000000 },
			{ "billonesimo", 1000000000000 },
			{ "trillon", 1000000000000000000 },
			{ "trillón", 1000000000000000000 },
			{ "trillones", 1000000000000000000 },
			{ "trillonesimo", 1000000000000000000 },
			{ "docena", 12 },
			{ "docenas", 12 },
			{ "k", 1000 },
			{ "m", 1000000 },
			{ "g", 1000000000 },
			{ "t", 1000000000000 }
		};
		public const string AmbiguousFractionConnectorsRegex = @"^[.]";
	}
}
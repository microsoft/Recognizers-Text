namespace Microsoft.Recognizers.Resources
{
	using System;
	using System.Collections.Generic;

	public static class BaseNumbers
	{
		public const string NumberReplaceToken = "@builtin.num";
		public static readonly Func<string, string, string> IntegerRegexDefinition = (placeholder, thousandsmark) => $@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!(\d+\.|\d+,))))\d{{1,3}}({thousandsmark}\d{{3}})+(?={placeholder})";
		public static readonly Func<string, string, string, string> DoubleRegexDefinition = (placeholder, thousandsmark, decimalmark) => $@"(((?<!\d+\s*)-\s*)|((?<=\b)(?<!\d+\.|\d+,)))\d{{1,3}}({thousandsmark}\d{{3}})+{decimalmark}\d+(?={placeholder})";
		public const string PlaceHolderDefault = "\\D|\\b";
	}
}
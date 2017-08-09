namespace Microsoft.Recognizers.Resources
{
	using System;
	using System.Collections.Generic;

	public static class CommonNumeric
	{
		public const string NumberReplaceToken = "@builtin.num";
		public const string IntegerTemplateRegex = "(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!(\\d+\\.|\\d+,))))\\d{{1,3}}({0}\\d{{3}})+";
		public const string DoubleTemplateRegex = "(((?<!\\d+\\s*)-\\s*)|((?<=\\b)(?<!\\d+\\.|\\d+,)))\\d{{1,3}}({0}\\d{{3}})+{1}\\d+";
		public const string PlaceHolderDefault = "\\D|\\b";
	}
}
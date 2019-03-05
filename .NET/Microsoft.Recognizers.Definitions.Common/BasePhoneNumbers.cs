//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//     
//     Generation parameters:
//     - DataFilename: Patterns\Base-PhoneNumbers.yaml
//     - Language: NULL
//     - ClassName: BasePhoneNumbers
// </auto-generated>
//------------------------------------------------------------------------------
namespace Microsoft.Recognizers.Definitions
{
	using System;
	using System.Collections.Generic;

	public static class BasePhoneNumbers
	{
		public const string NumberReplaceToken = @"@builtin.phonenumber";
		public const string GeneralPhoneNumberRegex = @"(\b(((\d[\s]?){4,12}))(-?[\d\s?]{3}\d)(?!-)\b)|(\(\d{5}\)\s?\d{5,6})|\+\d{2}\(\d\)\d{10}";
		public const string BRPhoneNumberRegex = @"(((\B\(\s?))\d{2,3}(\s?\))|(\b\d{2,3}))\s?\d{4,5}-?\d{3,5}(?!-)\b";
		public const string UKPhoneNumberRegex = @"(((\b(00)|\B\+)\s?)?(\b\d{2}\s?)?((\s?\(0\)[-\s]?|\b|(?<=(\b^#)\d{2}))\d{2,5}|\(0\d{3,4}\))[/-]?\s?(\d{5,8}|\d{3,4}[-\s]?\d{3,4})(?!-)\b)";
		public const string DEPhoneNumberRegex = @"((\+\d{2}\s?((\(0\))?\d\s?)?|\b)(\d{2,4}\s?[-/]?[\s\d]{7,10}\d)(?!-)\b)";
		public const string USPhoneNumberRegex = @"(((((\B\+)|\b)1(\s|-)?)|\b)?(\d{3}\)[-\s]?|\(\d{3}\)[-\s]?|\b\d{3}\s?[-\.]?\s?)|\b)\d{3}\s?[-\.]?\s?\d{4}(\s?(x|X|ext)\s?\d{3,5})?(?!(-\s?\d))\b";
		public const string CNPhoneNumberRegex = @"((\b00\s?)?\+?86\s?-?\s?)?(((\b|(?<=86))\d{2,5}\s?-?\s?|\(\d{2,5}\)\s?)\d{4}\s?-?\s?\d{4}(\s?-?\s?\d{4})?|(\b|(?<=86))\d{3}\s?-?\s?\d{4}\s?-?\s?\d{4})(?!-)\b";
		public const string DKPhoneNumberRegex = @"((\(\s?(\+\s?|00)45\s?\)\s?)|(((?<!\d)\+\s?|\b00)45\s?)|\b)(\s?\(0\)\s?)?((\d{8})|(\d{4}\s?-?\s?\d{4,6})|((\d{2}[\s-]){3}\d{2})|(\d{2}\s?-?\s?\d{3}\s?-?\s?\d{3}))(?!-)\b";
		public const string ITPhoneNumberRegex = @"((\(\s?(\+\s?|00)39\s?\)\s?)|(((?<!\d)\+\s?|\b00)39\s?)|\b)((0[\d\s-]{4,12}\d)|(3[\d\s-]{7,12}\d))(?!-)\b";
		public const string NLPhoneNumberRegex = @"((((\(\s?(\+\s?|00)31\s?\)\s?)|(((?<!\d)\+\s?|\b00)31\s?))?(((\b|(?<=31))0?\d{1,3}|\(\s?0?\d{1,3}\s?\)|\(0\)[-\s]?\d{1,3})\s?-?[\d\s]{5,11}\d))|\b\d{10,12})(?!-)\b";
		public const string SpecialPhoneNumberRegex = @"\b(\d{3,4}[/-]\d{1,4}[/-]\d{3,4})\b";
		public const string SpecialUSPhoneNumberRegex = @"(?<!(-|-\s|\d|\)|\)\s|\.))\d{3}\s?[-\.]\s?\d{4}(?!(-\s?\d))\b";
		public static readonly IList<string> TypicalDeductionRegexList = new List<string>
		{
			@"^\d{3}-\d{2}-\d{4}$",
			@"^\d{5}-\d{4}$"
		};
		public const string PhoneNumberMaskRegex = @"([0-9A-E]{2}(\s[0-9A-E]{2}){7})";
		public const string CountryCodeRegex = @"^(\(\s?(\+\s?|00)\d{1,3}\s?\)|(\+\s?|00)\d{1,3})";
		public const string AreaCodeIndicatorRegex = @"\(";
		public const string FormatIndicatorRegex = @"(\s|-|/|\.)+";
		public static readonly IList<char> SpecialBoundaryMarkers = new List<char>
		{
			'-',
			' '
		};
		public static readonly IList<char> BoundaryMarkers = new List<char>
		{
			'-',
			'.',
			'/',
			'+',
			'#',
			'*'
		};
	}
}
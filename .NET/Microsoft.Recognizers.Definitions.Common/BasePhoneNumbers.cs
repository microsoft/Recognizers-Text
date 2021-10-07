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
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// ------------------------------------------------------------------------------

namespace Microsoft.Recognizers.Definitions
{
    using System;
    using System.Collections.Generic;

    public static class BasePhoneNumbers
    {
      public const string NumberReplaceToken = @"@builtin.phonenumber";
      public const string WordBoundariesRegex = @"\b";
      public const string NonWordBoundariesRegex = @"\B";
      public const string EndWordBoundariesRegex = @"\b";
      public const string PreCheckPhoneNumberRegex = @"(\d{1,4}.){2,4}\s?\d{2,3}";
      public static readonly Func<string, string, string> GeneralPhoneNumberRegex = (WordBoundariesRegex, EndWordBoundariesRegex) => $@"({WordBoundariesRegex}(((\d[\s]?){{4,12}}))(-?[\d\s?]{{3}}\d)(?!-){EndWordBoundariesRegex})|(\(\d{{5}}\)\s?\d{{5,6}})|\+\d{{2}}\(\d\)\d{{10}}";
      public static readonly Func<string, string, string, string> BRPhoneNumberRegex = (WordBoundariesRegex, NonWordBoundariesRegex, EndWordBoundariesRegex) => $@"((\(\s?(\+\s?|00)55\s?\)\s?)|(((?<!\d)\+\s?|{WordBoundariesRegex}00)55\s?)|{WordBoundariesRegex})?((({NonWordBoundariesRegex}\(\s?))\d{{2,3}}(\s?\))|({WordBoundariesRegex}\d{{2,3}}))\s?\d{{4,5}}-?\d{{3,5}}(?!-){EndWordBoundariesRegex}";
      public static readonly Func<string, string, string, string> UKPhoneNumberRegex = (WordBoundariesRegex, NonWordBoundariesRegex, EndWordBoundariesRegex) => $@"((({WordBoundariesRegex}(00)|{NonWordBoundariesRegex}\+)\s?)?({WordBoundariesRegex}\d{{2}}\s?)?((\s?\(0\)[-\s]?|{WordBoundariesRegex}|(?<=(\b^#)\d{{2}}))\d{{2,5}}|\(0\d{{3,4}}\))[/-]?\s?(\d{{5,8}}|\d{{3,4}}[-\s]?\d{{3,4}})(?!-){EndWordBoundariesRegex})";
      public static readonly Func<string, string, string> DEPhoneNumberRegex = (WordBoundariesRegex, EndWordBoundariesRegex) => $@"((\+\d{{2}}\s?((\(0\))?\d\s?)?|{WordBoundariesRegex})(\d{{2,4}}\s?[-/]?[\s\d]{{7,10}}\d)(?!-){EndWordBoundariesRegex})";
      public static readonly Func<string, string, string, string> USPhoneNumberRegex = (WordBoundariesRegex, NonWordBoundariesRegex, EndWordBoundariesRegex) => $@"((((({NonWordBoundariesRegex}\+)|{WordBoundariesRegex})1(\s|-)?)|{WordBoundariesRegex})?(\d{{3}}\)[-\s]?|\(\d{{3}}\)[-\.\s]?|{WordBoundariesRegex}\d{{3}}\s?[-\.]?\s?)|{WordBoundariesRegex})[2-9]\d{{2}}\s?[-\.]?\s?\d{{4}}(\s?(x|X|ext)\s?\d{{3,5}})?(?!(-\s?\d)){EndWordBoundariesRegex}";
      public static readonly Func<string, string, string> CNPhoneNumberRegex = (WordBoundariesRegex, EndWordBoundariesRegex) => $@"(({WordBoundariesRegex}00\s?)?\+?(86|82|81)\s?-?\s?)?((({WordBoundariesRegex}|(?<=(86|82|81)))\d{{2,5}}\s?-?\s?|\(\d{{2,5}}\)\s?)\d{{4}}\s?-?\s?\d{{4}}(\s?-?\s?\d{{4}})?|(\b|(?<=(86|82|81)))\d{{3}}\s?-?\s?\d{{4}}\s?-?\s?\d{{4}})(?!-){EndWordBoundariesRegex}";
      public static readonly Func<string, string, string> DKPhoneNumberRegex = (WordBoundariesRegex, EndWordBoundariesRegex) => $@"((\(\s?(\+\s?|00)45\s?\)\s?)|(((?<!\d)\+\s?|\b00)45\s?)|{WordBoundariesRegex})(\s?\(0\)\s?)?((\d{{8}})|(\d{{4}}\s?-?\s?\d{{4,6}})|((\d{{2}}[\s-]){{3}}\d{{2}})|(\d{{2}}\s?-?\s?\d{{3}}\s?-?\s?\d{{3}}))(?!-){EndWordBoundariesRegex}";
      public static readonly Func<string, string, string> ITPhoneNumberRegex = (WordBoundariesRegex, EndWordBoundariesRegex) => $@"((\(\s?(\+\s?|00)39\s?\)\s?)|(((?<!\d)\+\s?|\b00)39\s?)|{WordBoundariesRegex})((0[\d-]{{4,12}}\d)|(3[\d-]{{7,12}}\d)|(0[\d\s]{{4,12}}\d)|(3[\d\s]{{7,12}}\d))(?!-){EndWordBoundariesRegex}";
      public static readonly Func<string, string, string> NLPhoneNumberRegex = (WordBoundariesRegex, EndWordBoundariesRegex) => $@"((((\(\s?(\+\s?|00)31\s?\)\s?)|(((?<!\d)\+\s?|{WordBoundariesRegex}00)31\s?))?((({WordBoundariesRegex}|(?<=31))0?\d{{1,3}}|\(\s?0?\d{{1,3}}\s?\)|\(0\)[-\s]?\d{{1,3}})((-?[\d]{{5,11}})|(\s[\d\s]{{5,11}}))\d))|\b\d{{10,12}})(?!-){EndWordBoundariesRegex}";
      public static readonly Func<string, string, string> SpecialPhoneNumberRegex = (WordBoundariesRegex, EndWordBoundariesRegex) => $@"({WordBoundariesRegex}(\d{{3,4}}[/-]\d{{1,4}}[/-]\d{{3,4}}){EndWordBoundariesRegex})";
      public const string NoAreaCodeUSPhoneNumberRegex = @"(?<!(-|-\s|\d|\)|\)\s|\.))[2-9]\d{2}\s?[-\.]\s?\d{4}(?!(-\s?\d))\b";
      public const string InternationDialingPrefixRegex = @"0(0|11)$";
      public static readonly IList<string> TypicalDeductionRegexList = new List<string>
        {
            @"^\d{5}-\d{4}$",
            @"\)\.",
            @"^0(0|11)(-)"
        };
      public const string PhoneNumberMaskRegex = @"([0-9a-e]{2}(\s[0-9a-e]{2}){7})";
      public const string CountryCodeRegex = @"^(\(\s?(\+\s?|00)\d{1,3}\s?\)|(\+\s?|00)\d{1,3})";
      public const string AreaCodeIndicatorRegex = @"\(";
      public const string FormatIndicatorRegex = @"(\s|-|/|\.)+";
      public static readonly IList<char> ColonMarkers = new List<char>
        {
            ':'
        };
      public const string ColonPrefixCheckRegex = @"(([a-z])\s*$)";
      public static readonly Dictionary<string, string> AmbiguityFiltersDict = new Dictionary<string, string>
        {
            { @"^\d{4}-\d{4}$", @"omb(\s*(no(\.)?|number|#))?:?\s+\d{4}-?\d{4}" }
        };
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
      public static readonly IList<char> ForbiddenPrefixMarkers = new List<char>
        {
            ',',
            ':',
            '%'
        };
      public static readonly IList<char> ForbiddenSuffixMarkers = new List<char>
        {
            '/',
            '+',
            '#',
            '*',
            ':',
            '%'
        };
      public const string SSNFilterRegex = @"^\d{3}-\d{2}-\d{4}$";
    }
}
// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

export namespace BasePhoneNumbers {
	export const NumberReplaceToken = '@builtin.phonenumber';
	export const GeneralPhoneNumberRegex = `(\\b((\\d[\\s]?){7,15}\\d)(?!-)\\b)`;
	export const BRPhoneNumberRegex = `(((\\B\\(\\s?))\\d{2,3}(\\s?\\))|(\\b\\d{2,3}))\\s?\\d{4,5}-?\\d{3,5}(?!-)\\b`;
	export const UKPhoneNumberRegex = `(((\\b(00)|\\B\\+)\\s?)?\\b\\d{2}\\s?(\\(0\\))?\\d{3,5}[/-]?\\s?(\\d{7,8}|\\d{3}\\s?\\d{3})(?!-)\\b)`;
	export const DEPhoneNumberRegex = `(\\b(\\d{3,4}\\s?[-/]?(\\s?\\d){7,8})(?!-)\\b)`;
	export const USPhoneNumberRegex = `((((\\B\\+)|\\b)1(\\s|-)?)|\\b)?(\\(\\d{3}\\)\\s?|\\b\\d{3}\\s?-?\\s?)\\d{3}\\s?-?\\s?\\d{4}(?!-)\\b`;
	export const CNPhoneNumberRegex = `((\\b00\\s?)?\\+?86\\s?-?\\s?)?((\\d{2,5}\\s?-?\\s?|\\(\\d{2,5}\\)\\s?)\\d{4}\\s?-?\\s?\\d{4}(\\s?-?\\s?\\d{4})?|\\d{3}\\s?-?\\s?\\d{4}\\s?-?\\s?\\d{4})(?!-)\\b`;
	export const DKPhoneNumberRegex = `((\\(\\s?(\\+\\s?|00)45\\s?\\)\\s?)|(((?<!\\d)\\+\\s?|\\b00)45\\s?)|(?<![\\+\\*])\\b)((\\d{8})|(\\d{4}\\s?-?\\s?\\d{4})|((\\d{2}[\\s-]){3}\\d{2})|(\\d{2}\\s?-?\\s?\\d{3}\\s?-?\\s?\\d{3}))(?!-)\\b`;
	export const ITPhoneNumberRegex = `((\\(\\s?(\\+\\s?|00)39\\s?\\)\\s?)|(((?<!\\d)\\+\\s?|\\b00)39\\s?)|(?<![\\+\\*])\\b)((0[\\d\\s-]{4,10}\\d)|(3[\\d\\s-]{7,9}\\d))(?!-)\\b`;
	export const NLPhoneNumberRegex = `((((\\(\\s?(\\+\\s?|00)31\\s?\\)\\s?)|(((?<!\\d)\\+\\s?|\\b00)31\\s?)|(?<![\\+\\*])\\b)((0?\\d{1,3}\\s?-?\\s?|\\(\\s?0?\\d{0,3}\\s?\\)\\s?)[\\d\\s]{5,11}\\d))|\\b\\d{10,12})(?!-)\\b`;
	export const SpecialPhoneNumberRegex = `\\b(\\d{3,4}[/-]\\d{1,4}[/-]\\d{3,4})\\b`;
	export const CountryCodeRegex = `^(\\(\\s?(\\+\\s?|00)\\d{1,3}\\s?\\)|(\\+\\s?|00)\\d{1,3})`;
	export const AreaCodeIndicatorRegex = `\\(`;
	export const FormatIndicatorRegex = `(\\s|-|/)`;
	export const SeparatorCharList = [ '-','.','/' ];
}

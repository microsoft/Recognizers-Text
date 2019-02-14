//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//     
//     Generation parameters:
//     - DataFilename: Patterns\Spanish\Spanish-Choice.yaml
//     - Language: Spanish
//     - ClassName: ChoiceDefinitions
// </auto-generated>
//------------------------------------------------------------------------------
namespace Microsoft.Recognizers.Definitions.Spanish
{
	using System;
	using System.Collections.Generic;

	public static class ChoiceDefinitions
	{
		public const string LangMarker = @"Spa";
		public const string TokenizerRegex = @"[^\w\d\u00E0-\u00FC]";
		public const string TrueRegex = @"\b(verdad|verdadero|sí|sip|s|si|cierto|por supuesto|ok)\b|(\uD83D\uDC4D|\uD83D\uDC4C)";
		public const string FalseRegex = @"\b(falso|no|nop|n|no)\b|(\uD83D\uDC4E|\u270B|\uD83D\uDD90)";
	}
}
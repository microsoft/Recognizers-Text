// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// ------------------------------------------------------------------------------

export namespace PortugueseChoice {
    export const LangMarker = `Por`;
    export const TokenizerRegex = `[^\\w\\d\\u00E0-\\u00FC]`;
    export const SkinToneRegex = `(\\uD83C\\uDFFB|\\uD83C\\uDFFC|\\uD83C\\uDFFD|\\uD83C\\uDFFE|\\uD83C\\uDFFF)`;
    export const TrueRegex = `\\b(verdade|verdadeir[oa]|sim|isso|claro|ok)\\b|(\\uD83D\\uDC4D|\\uD83D\\uDC4C)${SkinToneRegex}?`;
    export const FalseRegex = `\\b(falso|n[aã]o|incorreto|nada disso)\\b|(\\uD83D\\uDC4E|\\u270B|\\uD83D\\uDD90)${SkinToneRegex}?`;
}

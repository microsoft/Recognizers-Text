//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//
//     Generation parameters:
//     - DataFilename: Patterns\Chinese\Chinese-Choice.yaml
//     - Language: Chinese
//     - ClassName: ChoiceDefinitions
// </auto-generated>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// ------------------------------------------------------------------------------

namespace Microsoft.Recognizers.Definitions.Chinese
{
    using System;
    using System.Collections.Generic;

    public static class ChoiceDefinitions
    {
      public const string LangMarker = @"Chs";
      public const string TokenizerRegex = @"[^\u3040-\u30ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff\uff66-\uff9f]";
      public const string SkinToneRegex = @"(\uD83C\uDFFB|\uD83C\uDFFC|\uD83C\uDFFD|\uD83C\uDFFE|\uD83C\uDFFF)";
      public static readonly string TrueRegex = $@"(好[的啊呀嘞哇]|没问题|可以|中|好|同意|行|是的|是|对)|(\uD83D\uDC4D|\uD83D\uDC4C){SkinToneRegex}?";
      public static readonly string FalseRegex = $@"(不行|不好|拒绝|否定|不中|不可以|不是的|不是|不对|不)|(\uD83D\uDC4E|\u270B|\uD83D\uDD90){SkinToneRegex}?";
    }
}
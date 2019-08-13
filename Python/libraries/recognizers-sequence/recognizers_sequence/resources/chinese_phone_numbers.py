# ------------------------------------------------------------------------------
# <auto-generated>
#     This code was generated by a tool.
#     Changes to this file may cause incorrect behavior and will be lost if
#     the code is regenerated.
# </auto-generated>
#
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.
# ------------------------------------------------------------------------------

# pylint: disable=line-too-long


class ChinesePhoneNumbers:
    NumberReplaceToken = '@builtin.phonenumber'
    WordBoundariesRegex = f'(\\b|(?<=[\\u0800-\\u9FFF]))'
    NonWordBoundariesRegex = f'(\\B|(?<=[\\u0800-\\u9FFF]))'
    EndWordBoundariesRegex = f'(\\b|(?=[\\u0800-\\u9FFF]))'
    ColonMarkers = [r':', r'：']
    ColonBeginRegex = f'(([A-Za-z]|[\\u4E00-\\u9FA5])\\s*$)'
    BoundaryStartMarkers = [r'-', r'.', r'/', r'+', r'#', r'*', r':', r'：', r'%']
    BoundaryEndMarkers = [r'/', r'+', r'#', r'*', r':', r'：', r'%']
# pylint: enable=line-too-long

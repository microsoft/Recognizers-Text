﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Utilities
{
    public static class DictionaryUtils
    {
        // Safely bind dictionary which contains several key-value pairs to the destination dictionary.
        // This function is used to bind all the prefix and suffix for units.
        public static void BindDictionary(IDictionary<string, string> dictionary, IDictionary<string, string> sourceDictionary)
        {
            if (dictionary == null)
            {
                return;
            }

            foreach (var pair in dictionary)
            {
                if (string.IsNullOrEmpty(pair.Key))
                {
                    continue;
                }

                BindUnitsString(sourceDictionary, pair.Key, pair.Value);
            }
        }

        // Bind keys in a string which contains words separated by '|'.
        public static void BindUnitsString(IDictionary<string, string> sourceDictionary, string key, string source)
        {
            var values = source.Trim().Split('|');

            foreach (var token in values)
            {
                if (string.IsNullOrWhiteSpace(token) || (sourceDictionary.ContainsKey(token) && sourceDictionary[token].Equals(key, StringComparison.Ordinal)))
                {
                    continue;
                }

                // This segment of code is going to break if there're duplicated key-values in the resource files.
                // Those duplicates should be fixed before committing.
                try
                {
                    sourceDictionary.Add(token, key);
                }
                catch (ArgumentException ae)
                {
                    throw new ArgumentException(ae.Message + ": " + token);
                }
            }
        }
    }
}
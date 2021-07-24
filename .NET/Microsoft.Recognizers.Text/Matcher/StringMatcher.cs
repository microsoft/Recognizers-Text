﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class StringMatcher
    {
        private readonly ITokenizer tokenizer;

        public StringMatcher(MatchStrategy matchStrategy = MatchStrategy.TrieTree, ITokenizer tokenizer = null)
        {
            this.tokenizer = tokenizer ?? new SimpleTokenizer();
            switch (matchStrategy)
            {
                case MatchStrategy.AcAutomaton:
                    Matcher = new AcAutomaton<string>();
                    break;
                case MatchStrategy.TrieTree:
                    Matcher = new TrieTree<string>();
                    break;
                default:
                    throw new ArgumentException($"Unsupported match strategy: {matchStrategy}");
            }
        }

        private IMatcher<string> Matcher { get; }

        public void Init(IEnumerable<string> values)
        {
            Init(values, values.Select(v => v).ToArray());
        }

        public void Init(IEnumerable<string> values, string[] ids)
        {
            var tokenizedValues = GetTokenizedText(values);
            Init(tokenizedValues, ids);
        }

        public void Init(Dictionary<string, List<string>> valuesDictionary)
        {
            var values = new List<string>();
            var ids = new List<string>();
            foreach (var item in valuesDictionary)
            {
                var id = item.Key;
                foreach (var value in item.Value)
                {
                    values.Add(value);
                    ids.Add(id);
                }
            }

            var tokenizedValues = GetTokenizedText(values);
            Init(tokenizedValues, ids.ToArray());
        }

        public void Init(IEnumerable<string>[] tokenizedValues, string[] ids)
        {
            Matcher.Init(tokenizedValues, ids);
        }

        // Return token based entity result
        public IEnumerable<MatchResult<string>> Find(IEnumerable<string> tokenizedQuery)
        {
            return Matcher.Find(tokenizedQuery);
        }

        public IEnumerable<MatchResult<string>> Find(string queryText)
        {
            var queryTokens = tokenizer.Tokenize(queryText);
            var tokenizedQueryText = queryTokens.Select(t => t.Text);

            foreach (var r in Find(tokenizedQueryText))
            {
                var startToken = queryTokens[r.Start];
                var endToken = queryTokens[r.Start + r.Length - 1];
                var start = startToken.Start;
                var length = endToken.End - startToken.Start;
                var resultText = queryText.Substring(start, length);

                yield return new MatchResult<string>()
                {
                    Start = start,
                    Length = length,
                    Text = resultText,
                    CanonicalValues = r.CanonicalValues,
                };
            }
        }

        private IEnumerable<string>[] GetTokenizedText(IEnumerable<string> values)
        {
            return values.Select(t => tokenizer.Tokenize(t).Select(i => i.Text)).ToArray();
        }
    }
}

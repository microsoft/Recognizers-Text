using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class StringMatcher
    {
        private readonly ITokenizer Tokenizer;
        private IMatcher<string> Matcher { get; set; }

        public StringMatcher(MatchStrategy matchStrategy = MatchStrategy.TrieTree, ITokenizer tokenizer = null)
        {
            Tokenizer = tokenizer ?? new SimpleTokenizer();
            switch (matchStrategy)
            {
                case MatchStrategy.AcAutomaton:
                    Matcher = new AcAutomaton<string>();
                    break;
                case MatchStrategy.TrieTree:
                    Matcher = new TrieTree<string>();
                    break;
                default:
                    throw new ArgumentException($"Unsupported match strategy: {matchStrategy.ToString()}");
            }
        }

        public void Init(IEnumerable<string> values)
        {
            Init(values, values.Select(v => v.ToString()).ToArray());
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

        private IEnumerable<string>[] GetTokenizedText(IEnumerable<string> values)
        {
            return values.Select(t => Tokenizer.Tokenize(t).Select(i => i.Text)).ToArray();
        }

        // Return token based entity result
        public IEnumerable<MatchResult<string>> Find(IEnumerable<string> tokenizedQuery)
        {
            return Matcher.Find(tokenizedQuery);
        }

        public IEnumerable<MatchResult<string>> Find(string queryText)
        {
            var queryTokens = Tokenizer.Tokenize(queryText);
            var tokenizedQueryText = queryTokens.Select(t => t.Text);

            foreach (var r in Find(tokenizedQueryText))
            {
                var startToken = queryTokens[r.Start];
                var endToken = queryTokens[r.Start + r.Length - 1];
                var start = startToken.Start;
                var length = endToken.End - startToken.Start;
                var rtext = queryText.Substring(start, length);

                yield return new MatchResult<string>()
                {
                    Start = start,
                    Length = length,
                    Text = rtext,
                    CanonicalValues = r.CanonicalValues
                };
            }
        }
    }
}

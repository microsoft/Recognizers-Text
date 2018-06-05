using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.Matcher
{
    public class StringMatcher : AcAutomaton<string>
    {
        private readonly ITokenizer Tokenizer;

        public StringMatcher(IEnumerable<string> values, ITokenizer tokenizer = null)
        {
            Tokenizer = tokenizer ?? new SimpleTokenizer();

            BatchInsert(GetTokenizedText(values), values.ToArray());
            Build();
        }

        public StringMatcher(Dictionary<string, List<string>> valuesDictionary, ITokenizer tokenizer = null)
        {
            Tokenizer = tokenizer ?? new SimpleTokenizer();
            
            foreach (var item in valuesDictionary)
            {
                var id = item.Key;
                foreach (var value in item.Value)
                {
                    Insert(Tokenizer.Tokenize(value).Select(v => v.Text), id);
                }
            }

            Build();
        }

        public StringMatcher(IEnumerable<string> values, string[] ids, ITokenizer tokenizer = null)
        {
            Tokenizer = tokenizer ?? new SimpleTokenizer();
            BatchInsert(GetTokenizedText(values), ids);
            Build();
        }

        public StringMatcher(IEnumerable<string>[] values, string[] ids, ITokenizer tokenizer = null)
        {
            Tokenizer = tokenizer ?? new SimpleTokenizer();
            BatchInsert(values, ids);
            Build();
        }

        private IEnumerable<string>[] GetTokenizedText(IEnumerable<string> values)
        {
            return values.Select(t => Tokenizer.Tokenize(t).Select(i => i.Text)).ToArray();
        }

        public List<MatchResult> Find(string queryText)
        {
            var matches = new List<MatchResult>();

            if (string.IsNullOrWhiteSpace(queryText))
            {
                return matches;
            }

            var queryTokens = Tokenizer.Tokenize(queryText);
            var tokenizedQueryText = queryTokens.Select(t => t.Text);

            matches = Find(tokenizedQueryText).Select(r => {
                var segments = queryTokens.GetRange(r.Start, r.Length);
                var start = segments.First().Start;
                var length = segments.Last().End - segments.First().Start;
                var rtext = queryText.Substring(start, length);

                return new MatchResult()
                {
                    Start = start,
                    Length = length,
                    Text = rtext,
                    Values = r.Values
                };
            }).ToList();

            return matches;
        }
    }
}

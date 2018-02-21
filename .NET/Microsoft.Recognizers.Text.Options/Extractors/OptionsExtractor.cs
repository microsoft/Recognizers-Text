using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Options.Extractors
{
    public class OptionsExtractor : IExtractor
    {
        private readonly IChoiceExtractorConfiguration config;

        public OptionsExtractor(IChoiceExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            var results = new List<ExtractResult>();
            var trimmedText = text.ToLower();

            if (string.IsNullOrEmpty(text))
            {
                return results;
            }

            var partialResults = new List<ExtractResult>();
            var sourceTokens = Tokenize(trimmedText);
            var globalTopScore = 0.0;
            var globalTopResultIndex = 0;

            foreach (var item in config.MapRegexes)
            {
                var matches = item.Key.Matches(trimmedText);
                var topScore = 0.0;
                foreach (Match match in matches)
                {
                    var matchToken = Tokenize(match.Value);
                    for (int i = 0; i < sourceTokens.Count; i++)
                    {
                        var score = MatchValue(sourceTokens, matchToken, i);
                        topScore = Math.Max(topScore, score);
                    }

                    if (topScore > 0.0)
                    {
                        var start = match.Index;
                        var length = match.Length;
                        partialResults.Add(new ExtractResult()
                        {
                            Start = start,
                            Length = length,
                            Text = text.Substring(start, length).Trim(),
                            Type = item.Value,
                            Data = new
                            {
                                Source = text,
                                Score = topScore
                            }
                        });
                        if (topScore > globalTopScore)
                        {
                            globalTopScore = topScore;
                            globalTopResultIndex = partialResults.Count - 1;
                        }
                    }
                }
            }

            if (partialResults.Count == 0)
            {
                return results;
            }


            if (config.OnlyTopMatch)
            {
                results.Add(partialResults[globalTopResultIndex]);
            }
            else
            {
                results = partialResults.OrderBy(l1 => l1.Start).ToList();
            }

            return results;
        }

        private double MatchValue(IList<string> sourceTokens, IList<string> matchToken, int i)
        {
            return 0.5;
        }

        private IList<string> Tokenize(string trimmedText)
        {
            return new string[] { "yes" };
        }
    }
}
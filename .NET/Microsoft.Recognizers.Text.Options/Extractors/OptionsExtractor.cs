﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NeoSmart.Unicode;

namespace Microsoft.Recognizers.Text.Options.Extractors
{

    public class OptionsExtractDataResult
    {
        public IEnumerable<ExtractResult> OtherMatches { get; set; }
        public string Source { get; set; }
        public double Score { get; set; }
    }

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
                            Data = new OptionsExtractDataResult()
                            {
                                Source = text,
                                Score = topScore,
                                OtherMatches = new List<ExtractResult>()
                            } 
                        });
                    }
                }
            }

            if (partialResults.Count == 0)
            {
                return results;
            }

            partialResults = partialResults.OrderBy(l1 => l1.Start).ToList();

            if (config.OnlyTopMatch)
            {
                var topScore = 0.0;
                var topResultIndex = 0;
                for (int i = 0; i < partialResults.Count; i++)
                {
                    var data = partialResults[i].Data as OptionsExtractDataResult;
                    if (data.Score > topScore)
                    {
                        topScore = data.Score;
                        topResultIndex = i;
                    }
                }

                var topResultData = (partialResults[topResultIndex].Data as OptionsExtractDataResult);
                topResultData.OtherMatches = partialResults;
                results.Add(partialResults[topResultIndex]);
                partialResults.RemoveAt(topResultIndex);
            }
            else
            {
                results = partialResults;
            }

            return results;
        }

        private double MatchValue(IEnumerable<string> source, IEnumerable<string> match, int startPosition)
        {
            double matched = 0;
            var totalDeviation = 0;
            foreach (var token in match)
            {
                var pos = IndexOfToken(source.ToList(), token, startPosition);
                if (pos >= 0)
                {
                    var distance = matched > 0 ? pos - startPosition : 0;
                    if (distance <= config.MaxDistance)
                    {
                        matched++;
                        totalDeviation += distance;
                        startPosition = pos + 1;
                    }
                }
            }

            var score = 0.0;
            if (matched > 0 && (matched == match.Count() || config.AllowPartialMatch))
            {
                var completeness = matched / match.Count();
                var accuracy = completeness * (matched / (matched + totalDeviation));
                var initialScore = accuracy * (matched / source.Count());

                score = 0.4 + (0.6 * initialScore);
            }
            return score;
        }

        private static int IndexOfToken(List<string> tokens, string token, int startPos)
        {
            if (tokens.Count <= startPos) return -1;
            return tokens.FindIndex(startPos, x => x == token);
        }

        private IList<string> Tokenize(string text)
        {
            var tokens = new List<string>();
            var letters = text.Letters();

            var token = string.Empty;
            foreach (var letter in letters)
            {
                var codePoint = letter.Codepoints().FirstOrDefault();
                if (codePoint > 0xFFFF)
                {
                    // Character is in a Supplementary Unicode Plane. This is where emoji live so
                    // we're going to just break each character in this range out as its own token.
                    tokens.Add(letter);
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        tokens.Add(token);
                        token = string.Empty;
                    }
                }
                else if (!(config.TokenRegex.IsMatch(letter) || string.IsNullOrWhiteSpace(letter)))
                {
                    token = token + letter;
                }
                else if(!string.IsNullOrWhiteSpace(token))
                {
                    tokens.Add(token);
                    token = string.Empty;
                }
            }
            if (!string.IsNullOrWhiteSpace(token))
            {
                tokens.Add(token);
                token = string.Empty;
            }

            return tokens;
        }
    }
}
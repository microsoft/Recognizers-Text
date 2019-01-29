using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class BasePercentageExtractor : IExtractor
    {
        protected static readonly string NumExtType = Constants.SYS_NUM; // @sys.num

        protected static readonly string FracNumExtType = Constants.SYS_NUM_FRACTION;

        private readonly BaseNumberExtractor numberExtractor;

        public BasePercentageExtractor(BaseNumberExtractor numberExtractor)
        {
            this.numberExtractor = numberExtractor;
        }

        protected string ExtractType { get; set; } = Constants.SYS_NUM_PERCENTAGE;

        protected virtual NumberOptions Options { get; } = NumberOptions.None;

        protected ImmutableHashSet<Regex> Regexes { get; set; }

        /// <summary>
        /// extractor the percentage entities from the sentence.
        /// </summary>
        /// <param name="source">sentence.</param>
        /// <returns>List of percentage entities from the sentence source.</returns>
        public List<ExtractResult> Extract(string source)
        {
            var originSource = source;

            // preprocess the source sentence via extracting and replacing the numbers in it
            source = PreprocessStrWithNumberExtracted(originSource, out var positionMap, out var numExtResults);

            var allMatches = new List<MatchCollection>();

            // match percentage with regexes
            foreach (var regex in Regexes)
            {
                allMatches.Add(regex.Matches(source));
            }

            var matched = new bool[source.Length];
            for (var i = 0; i < source.Length; i++)
            {
                matched[i] = false;
            }

            foreach (var matches in allMatches)
            {
                foreach (Match match in matches)
                {
                    for (var j = 0; j < match.Length; j++)
                    {
                        matched[j + match.Index] = true;
                    }
                }
            }

            var result = new List<ExtractResult>();
            var last = -1;

            // get index of each matched results
            for (var i = 0; i < source.Length; i++)
            {
                if (matched[i])
                {
                    if (i + 1 == source.Length || matched[i + 1] == false)
                    {
                        int start = last + 1;
                        int length = i - last;
                        string substr = source.Substring(start, length);
                        ExtractResult er = new ExtractResult
                        {
                            Start = start,
                            Length = length,
                            Text = substr,
                            Type = ExtractType,
                        };
                        result.Add(er);
                    }
                }
                else
                {
                    last = i;
                }
            }

            // post-processing, restoring the extracted numbers
            PostProcessing(result, originSource, positionMap, numExtResults);

            return result;
        }

        /// <summary>
        /// read the rules.
        /// </summary>
        /// <param name="regexStrs">rule list.</param>
        /// <param name="ignoreCase">.</param>
        /// <returns>Immutable HashSet of regex.</returns>
        protected static ImmutableHashSet<Regex> BuildRegexes(HashSet<string> regexStrs, bool ignoreCase = false)
        {
            var regexes = new HashSet<Regex>();

            foreach (var regexStr in regexStrs)
            {
                // var sl = "(?=\\b)(" + regexStr + ")(?=(s?\\b))";
                var options = RegexOptions.Singleline;
                if (ignoreCase)
                {
                    options = options | RegexOptions.IgnoreCase;
                }

                Regex regex = new Regex(regexStr, options);

                regexes.Add(regex);
            }

            return regexes.ToImmutableHashSet();
        }

        protected abstract ImmutableHashSet<Regex> InitRegexes();

        /// <summary>
        /// replace the @sys.num to the real patterns, directly modifies the ExtractResult.
        /// </summary>
        /// <param name="results">extract results after number extractor.</param>
        /// <param name="originSource">the sentence after replacing the @sys.num, Example: @sys.num %.</param>
        private void PostProcessing(
            List<ExtractResult> results,
            string originSource,
            Dictionary<int, int> positionMap,
            IList<ExtractResult> numExtResults)
        {
            string replaceNumText = "@" + NumExtType;
            string replaceFracNumText = "@" + FracNumExtType;

            for (int i = 0; i < results.Count; i++)
            {
                int start = (int)results[i].Start;
                int end = start + (int)results[i].Length;
                string str = results[i].Text;
                var data = new List<(string, ExtractResult)>();

                string replaceText;
                if ((Options & NumberOptions.PercentageMode) != 0 && str.Contains(replaceFracNumText))
                {
                    replaceText = replaceFracNumText;
                }
                else
                {
                    replaceText = replaceNumText;
                }

                if (positionMap.ContainsKey(start) && positionMap.ContainsKey(end))
                {
                    int originStart = positionMap[start];
                    int originLenth = positionMap[end] - originStart;
                    results[i].Start = originStart;
                    results[i].Length = originLenth;
                    results[i].Text = originSource.Substring(originStart, originLenth);

                    int numStart = str.IndexOf(replaceText, StringComparison.Ordinal);
                    if (numStart != -1)
                    {
                        if (positionMap.ContainsKey(numStart))
                        {
                            for (int j = i; j < numExtResults.Count; j++)
                            {
                                if ((results[i].Start.Equals(numExtResults[j].Start) ||
                                     results[i].Start + results[i].Length ==
                                     numExtResults[j].Start + numExtResults[j].Length) &&
                                    results[i].Text.Contains(numExtResults[j].Text))
                                {
                                    data.Add((numExtResults[j].Text, numExtResults.ElementAt(j)));
                                }
                            }
                        }
                    }
                }

                if ((Options & NumberOptions.PercentageMode) != 0)
                {
                    // deal with special cases like "<fraction number> of" and "one in two" in percentageMode
                    if (str.Contains(replaceFracNumText) || data.Count > 1)
                    {
                        results[i].Data = data;
                    }
                    else if (data.Count == 1)
                    {
                        results[i].Data = data.First();
                    }
                }
                else if (data.Count == 1)
                {
                    results[i].Data = data.First();
                }
            }
        }

        /// <summary>
        /// get the number extractor results and convert the extracted numbers to @sys.num, so that the regexes can work.
        /// </summary>
        /// <param name="str">sentence to process.</param>
        /// <param name="positionMap">position Map.</param>
        /// <param name="numExtResults">number extractor result.</param>
        /// <returns>return according type "builtin.num" or "builtin.num.percentage".</returns>
        private string PreprocessStrWithNumberExtracted(
            string str,
            out Dictionary<int, int> positionMap,
            out IList<ExtractResult> numExtResults)
        {
            positionMap = new Dictionary<int, int>();

            numExtResults = numberExtractor.Extract(str);
            string replaceNumText = "@" + NumExtType;
            string replaceFracText = "@" + FracNumExtType;
            bool percentModeEnabled = (Options & NumberOptions.PercentageMode) != 0;

            // @TODO potential cause of GC
            var match = new int[str.Length];
            var strParts = new List<Tuple<int, int>>();
            int start, end;
            for (int i = 0; i < str.Length; i++)
            {
                match[i] = 0;
            }

            for (int i = 0; i < numExtResults.Count; i++)
            {
                var extraction = numExtResults[i];
                start = (int)extraction.Start;
                end = (int)extraction.Length + start;
                for (var j = start; j < end; j++)
                {
                    if (match[j] == 0)
                    {
                        if (percentModeEnabled && extraction.Data.ToString().StartsWith("Frac"))
                        {
                            match[j] = -(i + 1);
                        }
                        else
                        {
                            match[j] = i + 1;
                        }
                    }
                }
            }

            start = 0;
            for (int i = 1; i < str.Length; i++)
            {
                if (match[i] != match[i - 1])
                {
                    strParts.Add(new Tuple<int, int>(start, i - 1));
                    start = i;
                }
            }

            strParts.Add(new Tuple<int, int>(start, str.Length - 1));

            string ret = string.Empty;
            int index = 0;
            foreach (var strPart in strParts)
            {
                start = strPart.Item1;
                end = strPart.Item2;
                int type = match[start];

                if (type == 0)
                {
                    // subsequence which won't be extracted
                    ret += str.Substring(start, end - start + 1);
                    for (int i = start; i <= end; i++)
                    {
                        positionMap.Add(index++, i);
                    }
                }
                else
                {
                    // subsequence which will be extracted as number, type is negative for fraction number extraction
                    var replaceText = type > 0 ? replaceNumText : replaceFracText;
                    ret += replaceText;
                    for (int i = 0; i < replaceText.Length; i++)
                    {
                        positionMap.Add(index++, start);
                    }
                }
            }

            positionMap.Add(index, str.Length);

            return ret;
        }
    }
}
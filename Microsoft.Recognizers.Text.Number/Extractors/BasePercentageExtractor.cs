using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Extractors
{
    public abstract class BasePercentageExtractor : IExtractor
    {
        private readonly BaseNumberExtractor numberExtractor;

        protected static readonly string numExtType = Constants.SYS_NUM; //@sys.num
        protected string ExtractType = Constants.SYS_NUM_PERCENTAGE;

        private ImmutableHashSet<Regex> Regexes { get; }

        public BasePercentageExtractor(BaseNumberExtractor numberExtractor)
        {
            this.numberExtractor = numberExtractor;

            this.Regexes = this.InitRegexes();
        }

        protected abstract ImmutableHashSet<Regex> InitRegexes();

        /// <summary>
        /// extractor the percentage entities from the sentence
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<ExtractResult> Extract(string source)
        {
            string originSource = source;
            var positionMap = default(Dictionary<int, int>);
            var numExtResults = default(IList<ExtractResult>);

            // preprocess the source sentence via extracting and replacing the numbers in it
            source = this.PreprocessStrWithNumberExtracted(originSource, out positionMap, out numExtResults);

            List<MatchCollection> allMatches = new List<MatchCollection>();
            // match percentage with regexes
            foreach (Regex regex in Regexes)
            {
                allMatches.Add(regex.Matches(source));
            }

            bool[] matched = new bool[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                matched[i] = false;
            }

            for (int i = 0; i < allMatches.Count; i++)
            {
                foreach (Match match in allMatches[i])
                {
                    for (int j = 0; j < match.Length; j++)
                    {
                        matched[j + match.Index] = true;
                    }
                }
            }

            List<ExtractResult> result = new List<ExtractResult>();
            int last = -1;
            //get index of each matched results
            for (int i = 0; i < source.Length; i++)
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
                            Type = ExtractType
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
            this.PostProcessing(result, originSource, positionMap, numExtResults);

            return result;
        }

        /// <summary>
        /// read the rules
        /// </summary>
        /// <param name="regexStrs">rule list</param>
        /// <param name="ignoreCase"></param>
        protected static ImmutableHashSet<Regex> BuildRegexes(HashSet<string> regexStrs, bool ignoreCase = true)
        {
            var _regexes = new HashSet<Regex>();

            foreach (var regexStr in regexStrs)
            {
                var sl = "(?=\\b)(" + regexStr + ")(?=(s?\\b))";
                Regex regex;
                if (ignoreCase)
                {
                    regex = new Regex(regexStr, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                }
                else
                {
                    regex = new Regex(regexStr, RegexOptions.Singleline);
                }
                _regexes.Add(regex);
            }

            return _regexes.ToImmutableHashSet();
        }

        /// <summary>
        /// replace the @sys.num to the real patterns, directly modifies the ExtractResult
        /// </summary>
        /// <param name="results">extract results after number extractor</param>
        /// <param name="originSource">the sentense after replacing the @sys.num, Example: @sys.num %</param>
        private void PostProcessing(List<ExtractResult> results, string originSource, Dictionary<int, int> positionMap, IList<ExtractResult> numExtResults)
        {
            string replaceText = "@" + numExtType;
            for (int i = 0; i < results.Count; i++)
            {
                int start = (int)results[i].Start;
                int end = start + (int)results[i].Length;
                string str = results[i].Text;
                if (positionMap.ContainsKey(start) && positionMap.ContainsKey(end))
                {
                    int originStart = positionMap[start];
                    int originLenth = positionMap[end] - originStart;
                    results[i].Start = originStart;
                    results[i].Length = originLenth;
                    results[i].Text = originSource.Substring(originStart, originLenth);
                    int numStart = str.IndexOf(replaceText);
                    if (numStart != -1)
                    {
                        int numOriginStart = start + numStart;
                        if (positionMap.ContainsKey(numStart))
                        {
                            var dataKey = originSource.Substring(positionMap[numOriginStart],
                                positionMap[numOriginStart + replaceText.Length] - positionMap[numOriginStart]);

                            for (int j = i; j < numExtResults.Count; j++)
                            {
                                if (results[i].Start.Equals(numExtResults[j].Start) && results[i].Text.Contains(numExtResults[j].Text))
                                {
                                    results[i].Data = new KeyValuePair<string, ExtractResult>(dataKey, numExtResults.ElementAt(j));
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// get the number extractor results and convert the extracted numbers to @sys.num, so that the regexes can work
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string PreprocessStrWithNumberExtracted(string str, out Dictionary<int, int> positionMap, out IList<ExtractResult> numExtResults)
        {
            positionMap = new Dictionary<int, int>();

            numExtResults = numberExtractor.Extract(str);
            string replaceText = "@" + numExtType;

            int[] match = new int[str.Length];
            List<Tuple<int, int>> strParts = new List<Tuple<int, int>>();
            int start, end;
            for (int i = 0; i < str.Length; i++)
            {
                match[i] = -1;
            }

            for (int i = 0; i < numExtResults.Count; i++)
            {
                var extraction = numExtResults[i];
                string subtext = extraction.Text;
                start = (int)extraction.Start;
                end = (int)extraction.Length + start;
                for (int j = start; j < end; j++)
                {
                    if (match[j] == -1)
                    {
                        match[j] = i;
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

            string ret = "";
            int index = 0;
            foreach (var strPart in strParts)
            {
                start = strPart.Item1;
                end = strPart.Item2;
                int type = match[start];
                if (type == -1)
                {
                    ret += str.Substring(start, end - start + 1);
                    for (int i = start; i <= end; i++)
                    {
                        positionMap.Add(index++, i);
                    }
                }
                else
                {
                    string originalText = str.Substring(start, end - start + 1);
                    ret += replaceText;
                    for (int i = 0; i < replaceText.Length; i++)
                    {
                        positionMap.Add(index++, start);
                    }
                }
            }

            positionMap.Add(index++, str.Length);

            return ret;
        }
    }
}

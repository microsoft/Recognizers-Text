package com.microsoft.recognizers.text.number.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.number.Constants;
import com.microsoft.recognizers.text.number.NumberOptions;
import org.javatuples.Pair;

import java.util.*;
import java.util.regex.MatchResult;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public abstract class BasePercentageExtractor implements IExtractor {

    private final BaseNumberExtractor numberExtractor;

    protected abstract Set<Pattern> getRegexes();

    protected NumberOptions getOptions() { return NumberOptions.None; }

    protected String getExtractType() {
        return Constants.SYS_NUM_PERCENTAGE;
    }

    protected static final String NumExtType = Constants.SYS_NUM;

    protected static final String FracNumExtType = Constants.SYS_NUM_FRACTION;

    protected BasePercentageExtractor(BaseNumberExtractor numberExtractor) {
        this.numberExtractor = numberExtractor;
    }

    public List<ExtractResult> extract(String source) {

        String originSource = source;

        // preprocess the source sentence via extracting and replacing the numbers in it
        PreProcessResult preProcessResult = preProcessStrWithNumberExtracted(originSource);
        source = preProcessResult.string;
        Map<Integer, Integer> positionMap = preProcessResult.positionMap;
        List<ExtractResult> numExtResults = preProcessResult.numExtractResults;

        List<Matcher> allMatches = new ArrayList<>();
        // match percentage with regexes
        for(Pattern regex : getRegexes())
        {
            allMatches.add(regex.matcher(source));
        }

        boolean[] matched = new boolean[source.length()];
        Arrays.fill(matched, false);
        
        for(Matcher matcher : allMatches) 
        {
            while(matcher.find()) {
                MatchResult r = matcher.toMatchResult();
                int start = r.start();
                int end = r.end();
                int length = end - start;
                for(int j = 0; j < length; j++) {
                    matched[j + start] = true;
                }
            }
        }

        List<ExtractResult> result = new ArrayList<>();
        int last = -1;

        // get index of each matched results
        for (int i = 0; i < source.length(); i++)
        {
            if (matched[i])
            {
                if (i + 1 == source.length() || !matched[i + 1])
                {
                    int start = last + 1;
                    int length = i - last;
                    String substr = source.substring(start, start + length);
                    ExtractResult er = new ExtractResult(start, length, substr, getExtractType(), null);
                    result.add(er);
                }
            }
            else
            {
                last = i;
            }
        }

        // post-processing, restoring the extracted numbers
        postProcessing(result, originSource, positionMap, numExtResults);

        return result;
    }

    private void postProcessing(List<ExtractResult> results, String originSource, Map<Integer,Integer> positionMap, List<ExtractResult> numExtResults) {
        String replaceNumText = "@" + NumExtType;
        String replaceFracNumText = "@" + FracNumExtType;

        for (int i = 0; i < results.size(); i++)
        {
            int start = results.get(i).start;
            int end = start + results.get(i).length;
            String str = results.get(i).text;
            List<Pair<String, ExtractResult>> data = new ArrayList<>();

            String replaceText;
            if ((getOptions().ordinal() & NumberOptions.PercentageMode.ordinal()) != 0 && str.contains(replaceFracNumText))
            {
                replaceText = replaceFracNumText;
            }
            else
            {
                replaceText = replaceNumText;
            }

            if (positionMap.containsKey(start) && positionMap.containsKey(end))
            {
                int originStart = positionMap.get(start);
                int originLength = positionMap.get(end) - originStart;
                results.set(i, new ExtractResult(originStart, originLength, originSource.substring(originStart, originLength + originStart), results.get(i).type, null));

                int numStart = str.indexOf(replaceText);
                if (numStart != -1)
                {
                    if (positionMap.containsKey(numStart))
                    {
                        for (int j = i; j < numExtResults.size(); j++)
                        {
                            ExtractResult r = results.get(i);
                            ExtractResult n = numExtResults.get(j);
                            if ((r.start.equals(n.start) ||
                                    r.start + r.length ==
                                            n.start + n.length) &&
                                    r.text.contains(n.text))
                            {
                                data.add(Pair.with(n.text, n));
                            }
                        }
                    }
                }
            }

            if ((getOptions().ordinal() & NumberOptions.PercentageMode.ordinal()) != 0)
            {
                // deal with special cases like "<fraction number> of" and "one in two" in percentageMode
                if (str.contains(replaceFracNumText) || data.size() > 1)
                {
                    ExtractResult r = results.get(i);
                    results.set(i, new ExtractResult(r.start, r.length, r.text, r.type, data));
                }
                else if (data.size() == 1)
                {
                    ExtractResult r = results.get(i);
                    results.set(i, new ExtractResult(r.start, r.length, r.text, r.type, data.get(0)));
                }
            }
            else if (data.size() == 1)
            {
                ExtractResult r = results.get(i);
                results.set(i, new ExtractResult(r.start, r.length, r.text, r.type, data.get(0)));
            }
        }
    }

    private PreProcessResult preProcessStrWithNumberExtracted(String input) {

        Map<Integer, Integer> positionMap = new HashMap<>();
        List<ExtractResult> numExtractResults = numberExtractor.extract(input);

        String replaceNumText = "@" + NumExtType;
        String replaceFracText = "@" + FracNumExtType;
        boolean percentModeEnabled = (getOptions().ordinal() & NumberOptions.PercentageMode.ordinal()) != 0;

        //@TODO potential cause of GC
        int[] match = new int[input.length()];
        Arrays.fill(match, 0);
        List<Pair<Integer, Integer>> strParts = new ArrayList<>();
        int start, end;

        for (int i = 0; i < numExtractResults.size(); i++)
        {
            ExtractResult extraction = numExtractResults.get(i);
            start = extraction.start;
            end = extraction.length + start;
            for (int j = start; j < end; j++)
            {
                if (match[j] == 0)
                {
                    if (percentModeEnabled && extraction.data.toString().startsWith("Frac"))
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
        for (int i = 1; i < input.length(); i++)
        {
            if (match[i] != match[i - 1])
            {
                strParts.add(Pair.with(start, i -1));
                start = i;
            }
        }

        strParts.add(Pair.with(start, input.length() - 1));

        String ret = "";
        int index = 0;
        for(Pair<Integer, Integer> strPart : strParts)
        {
            start = strPart.getValue0();
            end = strPart.getValue1();
            int type = match[start];

            if (type == 0)
            {
                // subsequence which won't be extracted
                ret += input.substring(start, end + 1);
                for (int i = start; i <= end; i++)
                {
                    positionMap.put(index++, i);
                }
            }
            else
            {
                // subsequence which will be extracted as number, type is negative for fraction number extraction
                String replaceText = type > 0 ? replaceNumText : replaceFracText;
                ret += replaceText;
                for (int i = 0; i < replaceText.length(); i++)
                {
                    positionMap.put(index++, start);
                }
            }
        }

        positionMap.put(index, input.length());

        return new PreProcessResult(ret, positionMap, numExtractResults);
    }

    protected static Set<Pattern> buildRegexes(Set<String> regexStrs) {
        return buildRegexes(regexStrs, true);
    }

    protected static Set<Pattern> buildRegexes(Set<String> regexStrs, boolean ignoreCase) {

        Set<Pattern> regexes = new HashSet<>();
        for(String regexStr : regexStrs)
        {
            //var sl = "(?=\\b)(" + regexStr + ")(?=(s?\\b))";

            int options = 0;
            if (ignoreCase)
            {
                options = options | Pattern.CASE_INSENSITIVE;
            }

            Pattern regex = Pattern.compile(regexStr, options);

            regexes.add(regex);
        }

        return Collections.unmodifiableSet(regexes);
    }
}

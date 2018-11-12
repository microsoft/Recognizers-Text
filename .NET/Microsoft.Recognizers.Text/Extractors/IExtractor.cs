using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public interface IExtractor
    {
        List<ExtractResult> Extract(string input);
    }

    public class ExtractResult
    {
        public ExtractResult()
        {

        }

        public ExtractResult(int start, int length, string text, string type)
        {
            Start = start;
            Length = length;
            Text = text;
            Type = type;
        }

        public int? Start { get; set; } = null;
        public int? Length { get; set; } = null;
        public string Text { get; set; } = null;
        public string Type { get; set; } = null;
        public object Data { get; set; } = null;
        public Metadata Metadata { get; set; } = null;
    }

    public class Metadata
    {
        // For cases like "from 2014 to 2018", the period end "2018" could be inclusive or exclusive
        // For extraction, we only mark this flag to avoid future duplicate judgement, whether to include the period end or not is not determined in the extraction step
        public bool PossiblyIncludePeriodEnd = false;

        // For cases like "2015年以前" (usually regards as "before 2015" in English), "5天以前" (usually regards as "5 days ago" in English) in Chinese, we need to decide whether this is a "Date with Mode" or "Duration with Before and After". We use this flag to avoid duplicate judgement both in the Extraction step and Parse step.
        // Currently, this flag is only used in Chinese DateTime as other languages don't have this ambiguity cases.
        public bool IsDurationWithBeforeAndAfter = false;
    }
}
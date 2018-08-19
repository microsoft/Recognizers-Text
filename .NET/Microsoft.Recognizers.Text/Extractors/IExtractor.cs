using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public interface IExtractor
    {
        List<ExtractResult> Extract(string input);
    }

    public class ExtractResult
    {
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
    }
}
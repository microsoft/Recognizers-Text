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
}
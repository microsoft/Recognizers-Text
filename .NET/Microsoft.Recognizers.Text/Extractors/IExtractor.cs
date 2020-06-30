using System.Collections.Generic;

using Microsoft.Recognizers.Text.InternalCache;

namespace Microsoft.Recognizers.Text
{
    public interface IExtractor
    {
        List<ExtractResult> Extract(string input);
    }

    public class ExtractResult : ICloneableType<ExtractResult>
    {
        public string Issuer { get; set; } = null;

        public int? Start { get; set; } = null;

        public int? Length { get; set; } = null;

        public string Text { get; set; } = null;

        public string Type { get; set; } = null;

        public object Data { get; set; } = null;

        public Metadata Metadata { get; set; } = null;

        public bool Validation { get; set; } = false;

        public ExtractResult Clone()
        {
            return (ExtractResult)MemberwiseClone();
        }

    }
}
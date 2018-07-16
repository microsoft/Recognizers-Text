namespace Microsoft.Recognizers.Text
{
    public interface IParser
    {
        ParseResult Parse(ExtractResult extResult);
    }

    public class ParseResult : ExtractResult
    {
        public ParseResult()
        {
        }

        public ParseResult(ExtractResult er)
        {
            Length = er.Length;
            Start = er.Start;
            Data = er.Data;
            Text = er.Text;
            Type = er.Type;
            Metadata = er.Metadata;
        }

        public Metadata Metadata { get; set; } = null;

        //Value is for resolution. 
        //e.g. 1000 for "one thousand".
        //The resolutions are different for different parsers.
        //Therefore, we use object here.
        public object Value { get; set; } = null;

        //Output the value in string format.
        //It is used in some parsers.
        public string ResolutionStr { get; set; } = "";

    }
}
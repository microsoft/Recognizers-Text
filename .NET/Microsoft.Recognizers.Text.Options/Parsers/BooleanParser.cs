namespace Microsoft.Recognizers.Text.Options.Parsers
{
    public class BooleanParser : OptionsParser<bool>
    {
        public BooleanParser(): base(new BooleanParserConfiguration())
        {

        }
    }
}
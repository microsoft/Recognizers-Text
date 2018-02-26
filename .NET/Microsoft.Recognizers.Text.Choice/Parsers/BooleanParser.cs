namespace Microsoft.Recognizers.Text.Choice.Parsers
{
    public class BooleanParser : OptionsParser<bool>
    {
        public BooleanParser(): base(new BooleanParserConfiguration())
        {

        }
    }
}
namespace Microsoft.Recognizers.Text.Options.Parsers
{
    class BooleanParser : OptionsParser<bool>
    {
        public BooleanParser(): base(new BooleanParserConfiguration())
        {

        }
    }
}
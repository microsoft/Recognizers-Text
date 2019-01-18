namespace Microsoft.Recognizers.Text.Choice
{
    public class BooleanParser : OptionsParser<bool>
    {
        public BooleanParser()
               : base(new BooleanParserConfiguration())
        {
        }
    }
}
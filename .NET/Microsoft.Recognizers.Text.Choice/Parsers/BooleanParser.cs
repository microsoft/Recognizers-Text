namespace Microsoft.Recognizers.Text.Choice
{
    public class BooleanParser : ChoiceParser<bool>
    {
        public BooleanParser()
               : base(new BooleanParserConfiguration())
        {
        }
    }
}
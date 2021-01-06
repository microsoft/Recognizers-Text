namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EmailParser : BaseSequenceParser
    {

        private BaseSequenceConfiguration config;

        public EmailParser(BaseSequenceConfiguration config)
        {
            this.config = config;
        }
    }
}

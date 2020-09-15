namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseSequenceConfiguration : ISequenceConfiguration
    {
        public BaseSequenceConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

    }
}

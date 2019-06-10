namespace Microsoft.Recognizers.Text.Sequence
{
    public class SequenceConfiguration : ISequenceConfiguration
    {
        public SequenceConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

    }
}

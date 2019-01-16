namespace Microsoft.Recognizers.Text
{
    public class ExtendedModelResult : ModelResult
    {
        // Parameter Key
        public static readonly string ParentTextKey = "parentText";

        public ExtendedModelResult()
        {
        }

        public ExtendedModelResult(ModelResult modelResult)
        {
            Start = modelResult.Start;
            End = modelResult.End;
            TypeName = modelResult.TypeName;
            Resolution = modelResult.Resolution;
            Text = modelResult.Text;
        }

        public string ParentText { get; set; }
    }
}

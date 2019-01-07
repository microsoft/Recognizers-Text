namespace Microsoft.Recognizers.Text.Number
{

    public class TypeTag
    {
        public string Name { get; }

        public int Priority { get; }

        public TypeTag(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }
    }
}

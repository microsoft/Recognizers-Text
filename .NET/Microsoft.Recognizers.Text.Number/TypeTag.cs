namespace Microsoft.Recognizers.Text.Number
{
    public class TypeTag
    {
        public TypeTag(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }

        public string Name { get; }

        public int Priority { get; }
    }
}

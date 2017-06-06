namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer<T>
    {
        protected static readonly string DefaultCulture = Culture.English;

        protected static T ModelInstances;

        public static T GetModels()
        {
            return ModelInstances;
        }
    }
}

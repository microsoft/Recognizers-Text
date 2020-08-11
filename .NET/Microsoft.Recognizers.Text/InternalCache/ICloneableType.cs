namespace Microsoft.Recognizers.Text.InternalCache
{
    public interface ICloneableType<T>
    {
        T Clone();
    }
}

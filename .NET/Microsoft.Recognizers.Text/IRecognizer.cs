namespace Microsoft.Recognizers.Text
{
    public interface IRecognizer
    {
        IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture = true);

        bool TryGetModel<TModel>(string culture, out IModel model, bool fallbackToDefaultCulture = true);

        bool ContainsModel<TModel>(string culture, bool fallbackToDefaultCulture = true);
    }
}

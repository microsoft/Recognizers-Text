namespace Microsoft.Recognizers.Text
{
    public interface IRecognizer
    {
        IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture = true, string options = "");

        bool TryGetModel<TModel>(string culture, out IModel model, bool fallbackToDefaultCulture = true, string options = "");

        bool ContainsModel<TModel>(string culture, bool fallbackToDefaultCulture = true, string options = "");
    }
}

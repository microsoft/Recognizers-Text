namespace Microsoft.Recognizers.Text.Number
{
    public interface INumberRangeParserConfiguration
    {

        #region Internal Parsers

        IExtractor NumberExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IParser NumberParser { get; }

        #endregion

    }
}

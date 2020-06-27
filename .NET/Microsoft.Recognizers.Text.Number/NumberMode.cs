namespace Microsoft.Recognizers.Text.Number
{
    public enum NumberMode
    {
        /// <summary>
        /// Default is for datetime
        /// </summary>
        Default,

        /// <summary>
        /// Add 67.5 billion and million support.
        /// </summary>
        Currency,

        /// <summary>
        /// Don't extract number from cases like 16ml
        /// </summary>
        PureNumber,

        /// <summary>
        /// Unit is for unit
        /// </summary>
        Unit,
    }
}
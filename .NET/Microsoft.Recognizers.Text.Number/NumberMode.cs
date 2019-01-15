namespace Microsoft.Recognizers.Text.Number
{
    public enum NumberMode
    {
        /// <summary>
        /// Default is for unit and datetime
        /// </summary>
        Default,

        /// <summary>
        /// Add 67.5 billion & million support.
        /// </summary>
        Currency,

        /// <summary>
        /// Don't extract number from cases like 16ml
        /// </summary>
        PureNumber,
    }
}
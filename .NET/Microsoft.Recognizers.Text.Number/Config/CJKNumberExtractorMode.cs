namespace Microsoft.Recognizers.Text.Number.Config
{

    /// <summary>
    /// These modes only apply to CJK NumberExtractors.
    /// The default mode utilizes an allow list to avoid extracting numbers in ambiguous/undesired combinations of Chinese/Japanese ideograms.
    /// ExtractAll mode is to be used in cases where extraction should be more aggressive (e.g. in Units extraction).
    /// </summary>
    public enum CJKNumberExtractorMode
    {
        /// <summary>
        /// Number extraction with an allow list that filters what numbers to extract.
        /// </summary>
        Default,

        /// <summary>
        /// Extract all number-related terms aggressively.
        /// </summary>
        ExtractAll,
    }

}

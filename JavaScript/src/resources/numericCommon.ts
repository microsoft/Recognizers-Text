export namespace CommonNumeric {
    export const NumberReplaceToken = "@builtin.num";

    // export const DottedNumbersWithPlaceHolder = (placeholder: string): string => String.raw`(((?<!\d+\s*)-\s*)|(?<=\b))\d{1,3}(,\d{3})+(?=${placeholder})`;
    export const IntegerTemplateRegex = String.raw`(((?!\d+\s*)-\s*)|(?=\b))\d{1,3}({0}\d{3})+`;
    export const DoubleTemplateRegex = String.raw`(((?!\d+\s*)-\s*)|((?=\b)(?!\d+\.)))\d{1,3}({0}\d{3})+{1}\d+`;
}
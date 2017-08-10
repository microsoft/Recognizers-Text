export namespace BaseNumbers {
	export const NumberReplaceToken = '@builtin.num';
	export const IntegerRegexDefinition = (placeholder: string, thousandsmark: string) => { return `(((?!\\d+\\s*)-\\s*)|(?=\\b))\\d{1,3}(${thousandsmark}\\d{3})+(?=${placeholder})`; }
	export const DoubleRegexDefinition = (placeholder: string, thousandsmark: string, decimalmark: string) => { return `(((?!\\d+\\s*)-\\s*)|((?=\\b)(?!\\d+\\.)))\\d{1,3}(${thousandsmark}\\d{3})+${decimalmark}\\d+(?=${placeholder})`; }
	export const PlaceHolderDefault = '\\\\D|\\\\b';
}

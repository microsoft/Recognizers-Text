import { IModel, ModelResult, ParseResult, IExtractor, IParser, QueryProcessor } from "@microsoft/recognizers-text";
import { UnitValue, UnitValueIso } from "./parsers";

export enum CompositeEntityType {
    Age,
    Currency,
    Dimension,
    Temperature
}

export abstract class AbstractNumberWithUnitModel implements IModel {
    protected extractorParsersMap: Map<IExtractor, IParser>;

    abstract modelTypeName: string;

    constructor(extractorParsersMap: Map<IExtractor, IParser>) {
        this.extractorParsersMap = extractorParsersMap;
    }

    parse(query: string): Array<ModelResult> {
        query = QueryProcessor.preProcess(query, true);

        let extractionResults = new Array<ModelResult>();
        for (let kv of this.extractorParsersMap.entries()) {
            let extractor = kv[0];
            let parser = kv[1];
            let extractResults = extractor.extract(query);
            let parseResults: Array<ParseResult> = [];
            for (let i = 0; i < extractResults.length; i++) {
                let r = parser.parse(extractResults[i]);
                if (r.value !== null) {
                    if (r.value instanceof Array) {
                        for (let j = 0; j < r.value.length; j++) {
                            parseResults.push(r.value[j]);
                        }
                    } else {
                        parseResults.push(r);
                    }
                }
            }
            let modelResults = parseResults.map(o =>
                ({
                    start: o.start,
                    end: o.start + o.length - 1,
                    resolution: this.getResolution(o.value),
                    text: o.text,
                    typeName: this.modelTypeName
                } as ModelResult));

            modelResults.forEach(result => {
                let bAdd = true;

                extractionResults.forEach(extractionResult => {
                    if (extractionResult.start === result.start && extractionResult.end === result.end) {
                        bAdd = false;
                    }
                });

                if (bAdd) {
                    extractionResults.push(result);
                }
            });
        }

        return extractionResults;
    }

    private getResolution(data: any): any {
        if(typeof data === 'undefined') return null;

        let result =  typeof data === "string"
            ? { value: data.toString() }
            : { value: (data as UnitValue).number, unit: (data as UnitValue).unit };

        if ((data as UnitValueIso).isoCurrency) {
            result['isoCurrency'] = data.isoCurrency;
        }

        return result;
    }
}

export class AgeModel extends AbstractNumberWithUnitModel {
    modelTypeName: string = "age"
}

export class CurrencyModel extends AbstractNumberWithUnitModel {
    modelTypeName: string = "currency";
}

export class DimensionModel extends AbstractNumberWithUnitModel {
    modelTypeName: string = "dimension";
}

export class TemperatureModel extends AbstractNumberWithUnitModel {
    modelTypeName: string = "temperature"
}
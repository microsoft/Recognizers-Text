import { Culture } from "./culture";
import { match, cache } from "xregexp";
import { StringUtility } from "./utilities";

export interface IModel {
    readonly modelTypeName: string
    parse(query: string): Array<ModelResult>
}

export class ModelResult {
    text: string
    start: number
    end: number
    typeName: string
    resolution: { [key: string]: any }
}

export class ExtendedModelResult extends ModelResult {
    parentText: string

    constructor(source: ModelResult = null) {
        super()
        if (source) {
            this.text = source.text;
            this.start = source.start;
            this.end = source.end;
            this.typeName = source.typeName;
            this.resolution = source.resolution;
        }
    }
}

class ModelFactoryKey<TModelOptions> {
    culture: string;
    modelType: string;
    options: TModelOptions;
    constructor(culture: string, modelType: string, options: TModelOptions = null) {
        this.culture = culture ? culture.toLowerCase() : null;
        this.modelType = modelType;
        this.options = options;
    }

    public toString(): string {
        return JSON.stringify(this);
    }

    public static fromString<TModelOptions>(key: string): ModelFactoryKey<TModelOptions> {
        return JSON.parse(key) as ModelFactoryKey<TModelOptions>;
    }
}

export class ModelFactory<TModelOptions> {
    static readonly fallbackCulture: string = Culture.English;

    private modelFactories: Map<string, (options: TModelOptions) => IModel> = new Map<string, (options: TModelOptions) => IModel>();

    private static cache: Map<string, IModel> = new Map<string, IModel>();

    getModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean, options: TModelOptions): IModel {

        let result = this.tryGetModel(modelTypeName, culture, options);
        if (!result.containsModel && fallbackToDefaultCulture) {
            result = this.tryGetModel(modelTypeName, ModelFactory.fallbackCulture, options);
        }

        if (result.containsModel) {
            return result.model;
        }

        throw new Error(`Could not find Model with the specified configuration: ${culture},${modelTypeName}`);
    }

    tryGetModel(modelTypeName: string, culture: string, options: TModelOptions): { containsModel: boolean; model?: IModel } {
        let cacheResult = this.getModelFromCache(modelTypeName, culture, options);
        if (cacheResult) return { containsModel: true, model: cacheResult };

        let key = this.generateKey(modelTypeName, culture);
        if (this.modelFactories.has(key)) {
            let model = this.modelFactories.get(key)(options);
            this.registerModelInCache(modelTypeName, culture, options, model);
            return { containsModel: true, model: model };
        }

        return { containsModel: false };
    }

    registerModel(modelTypeName: string, culture: string, modelCreator: (options: TModelOptions) => IModel) {
        let key = this.generateKey(modelTypeName, culture);
        if (this.modelFactories.has(key)) {
            throw new Error(`${culture}-${modelTypeName} has already been registered.`);
        }

        this.modelFactories.set(key, modelCreator);
    }

    initializeModels(targetCulture: string, options: TModelOptions) {
        this.modelFactories.forEach((value, key) => {
            let modelFactoryKey = ModelFactoryKey.fromString<TModelOptions>(key);
            if (StringUtility.isNullOrEmpty(targetCulture) || modelFactoryKey.culture === targetCulture) {
                this.tryGetModel(modelFactoryKey.modelType, modelFactoryKey.culture, modelFactoryKey.options)
            }
        });
    }

    private generateKey(modelTypeName: string, culture: string): string {
        return new ModelFactoryKey(culture, modelTypeName).toString();
    }

    private getModelFromCache(modelTypeName: string, culture: string, options: TModelOptions): IModel {
        let key = this.generateCacheKey(modelTypeName, culture, options);
        return ModelFactory.cache.get(key);
    }

    private registerModelInCache(modelTypeName: string, culture: string, options: TModelOptions, model: IModel) {
        let key = this.generateCacheKey(modelTypeName, culture, options);
        ModelFactory.cache.set(key, model);
    }

    private generateCacheKey(modelTypeName: string, culture: string, options: TModelOptions): string {
        return new ModelFactoryKey<TModelOptions>(culture, modelTypeName, options).toString();
    }
}
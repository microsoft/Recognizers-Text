import { Culture } from "./culture";
import { match, cache } from "xregexp";

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

export class ModelFactory<TModelOptions> {
    static readonly defaultCulture: string = Culture.English;

    private modelFactories: Map<string, (options: TModelOptions) => IModel> = new Map<string, (options: TModelOptions) => IModel>();

    private static cache: Map<string, IModel> = new Map<string, IModel>();

    getModel(modelTypeName: string, culture: string, options: TModelOptions): IModel {
        let cacheResult = this.getModelFromCache(modelTypeName, culture, options);
        if (cacheResult)  return cacheResult as IModel; 
        
        let result = this.tryGetModel(modelTypeName, culture, options);
        if (!result.containsModel) {
            throw new Error(`No IModel instance for ${culture}-${modelTypeName}`);
        }

        let model = result.model as IModel;
        this.registerModelInCache(modelTypeName, culture, options, model);
        return model;
    }

    tryGetModel(modelTypeName: string, culture: string, options: TModelOptions): { containsModel: boolean; model?: IModel } {
        let model: IModel;
        let ret: boolean = true;
        let key = this.generateKey(modelTypeName, culture);
        if (!this.modelFactories.has(key)) {
            ret = false;
        }

        if (ret) {
            return { containsModel: true, model: this.modelFactories.get(key)(options) };
        }

        return { containsModel: false };
    }

    registerModel(modelTypeName: string, culture: string, modelCreator: (options: TModelOptions) => IModel) {
        let key = this.generateKey(modelTypeName, culture);
        if (this.modelFactories.has(key)) {
            throw new Error(`${culture}-${modelTypeName} has been registered.`);
        }

        this.modelFactories.set(key, modelCreator);
    }

    private generateKey(modelTypeName: string, culture: string): string {
        return `${culture.toLowerCase()}-${modelTypeName}`;
    }

    private getModelFromCache(modelTypeName: string, culture: string, options: TModelOptions): IModel{
        let key = this.generateCacheKey(modelTypeName, culture, options);
        return ModelFactory.cache.get(key);
    }
    
    private registerModelInCache(modelTypeName: string, culture: string, options: TModelOptions, model: IModel) {
        let key = this.generateCacheKey(modelTypeName, culture, options);
        ModelFactory.cache.set(key, model);
    }
    
    private generateCacheKey(modelTypeName: string, culture: string, options: TModelOptions): string {
        return `${culture.toLowerCase()}-${modelTypeName}-${options.toString()}`;
    }
}
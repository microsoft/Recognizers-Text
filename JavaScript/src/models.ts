import { Culture } from "./culture";

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

export class ModelContainer {
    static readonly defaultCulture: string = Culture.English;

    private modelInstances: Map<string, IModel> = new Map<string, IModel>();

    getModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean = true): IModel {
        let result = this.tryGetModel(modelTypeName, culture, fallbackToDefaultCulture);
        if (!result.containsModel) {
            throw new Error(`No IModel instance for ${culture}-${modelTypeName}`);
        }

        return result.model as IModel;
    }

    tryGetModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean = true): { containsModel: boolean; model?: IModel } {
        let model: IModel;
        let ret: boolean = true;
        let key = this.generateKey(modelTypeName, culture);
        if (!this.modelInstances.has(key)) {
            if (fallbackToDefaultCulture) {
                culture = ModelContainer.defaultCulture;
                key = this.generateKey(modelTypeName, culture);
            }

            if (!this.modelInstances.has(key)) {
                ret = false;
            }
        }

        if (ret) {
            return { containsModel: true, model: this.modelInstances.get(key) };
        }

        return { containsModel: false };
    }

    containsModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean = true): boolean {
        return this.tryGetModel(modelTypeName, culture, fallbackToDefaultCulture).containsModel;
    }

    registerModel(modelTypeName: string, culture: string, model: IModel) {
        let key = this.generateKey(modelTypeName, culture);
        if (this.modelInstances.has(key)) {
            throw new Error(`${culture}-${modelTypeName} has been registered.`);
        }

        this.modelInstances.set(key, model);
    }

    registerModels(models: Map<string, IModel>, culture: string) {
        for (let key in models.keys()) {
            let model: IModel = models.get(key) as IModel;
            this.registerModel(key, culture, model);
        }
    }

    private generateKey(modelTypeName: string, culture: string): string {
        return `${culture.toLowerCase()}-${modelTypeName}`;
    }
}
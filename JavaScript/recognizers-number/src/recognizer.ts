import { IModel, ModelContainer } from "./models"

export interface IRecognizer {
  getModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean): void

  tryGetModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean): { containsModel: boolean; model?: IModel }

  containsModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean): boolean
}

export abstract class Recognizer implements IRecognizer {
  private readonly modelContainer: ModelContainer = new ModelContainer();

  getModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean = true): IModel {
    return this.modelContainer.getModel(modelTypeName, culture, fallbackToDefaultCulture);
  }

  tryGetModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean = true): { containsModel: boolean; model?: IModel } {
    return this.modelContainer.tryGetModel(modelTypeName, culture, fallbackToDefaultCulture);
  }

  containsModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean = true): boolean {
    return this.modelContainer.containsModel(modelTypeName, culture, fallbackToDefaultCulture);
  }

  registerModel(modelTypeName: string, culture: string, model: IModel) {
    this.modelContainer.registerModel(modelTypeName, culture, model);
  }

  registerModels(models: Map<string, IModel>, culture: string) {
    this.modelContainer.registerModels(models, culture);
  }
}
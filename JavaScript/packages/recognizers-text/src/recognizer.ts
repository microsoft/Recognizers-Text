import { IModel, ModelFactory } from "./models"

export abstract class Recognizer<TModelOptions> {
  public readonly RecognizerOptions: TModelOptions;
  public readonly RecognizerCulture: string;

  private readonly modelFactory: ModelFactory<TModelOptions> = new ModelFactory<TModelOptions>();

  protected constructor(culture: string, options: TModelOptions) {
    this.RecognizerCulture = culture;
    this.RecognizerOptions = options;
    this.InitializeConfiguration();
  }

  protected abstract InitializeConfiguration();

  getModel(modelTypeName: string): IModel {
    return this.modelFactory.getModel(modelTypeName, this.RecognizerCulture, this.RecognizerOptions);
  }

  registerModel(modelTypeName: string, culture: string, modelCreator: (options: TModelOptions) => IModel) {
    this.modelFactory.registerModel(modelTypeName, culture, modelCreator);
  }
}
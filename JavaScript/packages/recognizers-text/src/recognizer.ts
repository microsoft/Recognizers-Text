import { IModel, ModelFactory } from "./models"

export abstract class Recognizer<TRecognizerOptions> {
  public readonly RecognizerOptions: TRecognizerOptions;
  public readonly RecognizerCulture: string;

  private readonly modelFactory: ModelFactory<TRecognizerOptions> = new ModelFactory<TRecognizerOptions>();

  protected constructor(culture: string, options: TRecognizerOptions) {
    this.RecognizerCulture = culture;
    this.RecognizerOptions = options;
    this.InitializeConfiguration();
  }

  protected abstract InitializeConfiguration();

  getModel(modelTypeName: string): IModel {
    return this.modelFactory.getModel(modelTypeName, this.RecognizerCulture, this.RecognizerOptions);
  }

  registerModel(modelTypeName: string, culture: string, modelCreator: (options: TRecognizerOptions) => IModel) {
    this.modelFactory.registerModel(modelTypeName, culture, modelCreator);
  }
}
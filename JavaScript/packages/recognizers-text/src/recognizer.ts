import { IModel, ModelFactory } from "./models"

export abstract class Recognizer<TRecognizerOptions> {
  public readonly RecognizerOptions: TRecognizerOptions;
  public readonly TargetCulture: string;

  private readonly modelFactory: ModelFactory<TRecognizerOptions> = new ModelFactory<TRecognizerOptions>();

  protected constructor(culture: string, options: TRecognizerOptions) {
    this.TargetCulture = culture;
    this.RecognizerOptions = options;
    this.InitializeConfiguration();
  }

  protected abstract InitializeConfiguration();

  getModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean): IModel {
    return this.modelFactory.getModel(modelTypeName, culture || this.TargetCulture, fallbackToDefaultCulture, this.RecognizerOptions);
  }

  registerModel(modelTypeName: string, culture: string, modelCreator: (options: TRecognizerOptions) => IModel) {
    this.modelFactory.registerModel(modelTypeName, culture, modelCreator);
  }
}
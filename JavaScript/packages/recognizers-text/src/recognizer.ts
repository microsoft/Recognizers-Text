import { IModel, ModelFactory } from "./models"

export abstract class Recognizer<TRecognizerOptions> {
  public readonly Options: TRecognizerOptions;
  public readonly TargetCulture: string;

  private readonly modelFactory: ModelFactory<TRecognizerOptions> = new ModelFactory<TRecognizerOptions>();

  protected constructor(targetCulture: string, options: TRecognizerOptions, lazyInitialization: boolean) {
    this.TargetCulture = targetCulture;
    this.Options = options;
    this.InitializeConfiguration();

    if (!lazyInitialization) {
      this.initializeModels(targetCulture, options)
    }
  }

  protected abstract InitializeConfiguration();

  getModel(modelTypeName: string, culture: string, fallbackToDefaultCulture: boolean): IModel {
    return this.modelFactory.getModel(modelTypeName, culture || this.TargetCulture, fallbackToDefaultCulture, this.Options);
  }

  registerModel(modelTypeName: string, culture: string, modelCreator: (options: TRecognizerOptions) => IModel) {
    this.modelFactory.registerModel(modelTypeName, culture, modelCreator);
  }

  private initializeModels(targetCulture: string, options: TRecognizerOptions) {
    this.modelFactory.initializeModels(targetCulture, options);
  }
}
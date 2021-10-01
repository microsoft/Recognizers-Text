// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IModel, ModelFactory } from "./models";

export abstract class Recognizer<TRecognizerOptions> {
  public readonly Options: TRecognizerOptions;
  public readonly TargetCulture: string;

  private readonly modelFactory: ModelFactory<TRecognizerOptions> = new ModelFactory<TRecognizerOptions>();

  protected constructor(targetCulture: string, options: TRecognizerOptions, lazyInitialization: boolean);
  protected constructor(targetCulture: string, options: any, lazyInitialization: boolean) {
    if (!this.IsValidOptions(options)) {
      throw new Error(`${options} is not a valid options value.`);
    }
    this.TargetCulture = targetCulture;
    this.Options = options;
    this.InitializeConfiguration();

    if (!lazyInitialization) {
      this.initializeModels(targetCulture, options);
    }
  }

  protected abstract InitializeConfiguration();

  protected abstract IsValidOptions(options): boolean;

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
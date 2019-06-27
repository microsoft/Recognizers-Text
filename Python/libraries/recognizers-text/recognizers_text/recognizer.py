from abc import ABC, abstractmethod
from typing import Generic, Callable
from .model import T_MODEL_OPTIONS, ModelFactory, Model


class Recognizer(Generic[T_MODEL_OPTIONS], ABC):
    def __init__(self, target_culture: str, options: T_MODEL_OPTIONS, lazy_initialization: bool):
        self.target_culture: str = target_culture
        self.options: T_MODEL_OPTIONS = options
        self.model_factory: ModelFactory = ModelFactory()
        self.initialize_configuration()

        if lazy_initialization:
            self.initialize_models()

    @abstractmethod
    def initialize_configuration(self):
        raise NotImplementedError

    def get_model(self, model_type_name: str, culture: str, fallback_to_default_culture: bool) -> Model:
        if culture is None:
            culture = self.target_culture
        return self.model_factory.get_model(model_type_name, culture, fallback_to_default_culture, self.options)

    def register_model(self, model_type_name: str, culture: str, model_ctor: Callable[[T_MODEL_OPTIONS], Model]):
        self.model_factory.register_model(model_type_name, culture, model_ctor)

    def initialize_models(self):
        self.model_factory.initialize_models(self.target_culture, self.options)

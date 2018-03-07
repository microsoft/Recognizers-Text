from abc import ABC, abstractmethod, abstractproperty
from typing import List, Dict, Generic, TypeVar, Callable, Tuple, NamedTuple, Optional
from collections import namedtuple

from .culture import Culture

TModelOptions = TypeVar('TModelOptions')

class ModelResult():
    def __init__(self):
        self.text: str
        self.start: int
        self.end: int
        self.type_name: str
        self.resolution: Dict[str, object]

class Model(ABC):
    @abstractproperty
    def model_type_name(self) -> str:
        raise NotImplementedError

    @abstractmethod
    def parse(self, query: str) -> List[ModelResult]:
        raise NotImplementedError

cache_key=namedtuple('cache_key', ['model_type', 'culture', 'options'])
model_ctor_key=namedtuple('model_ctor_key', ['model_type', 'culture'])

class ModelFactory(Generic[TModelOptions]):
    __fallback_to_default_culture=Culture.English
    __cache: Dict[NamedTuple("key", [("model_type", str), ("culture", str), ("options", TModelOptions)]), Model]

    def __init__(self):
        self.model_factories: Dict[NamedTuple("key", [("model_type", str), ("culture", str)]), Callable[[TModelOptions], Model]]

    def get_model(self, model_type_name: str, culture: str, fallback_to_default_culture: bool, options: TModelOptions) -> Model:
        result=self.try_get_model(model_type_name, culture, options)
        if result is None and fallback_to_default_culture is True:
            result=self.try_get_model(model_type_name, ModelFactory.__fallback_to_default_culture, options)
        if result is not None:
            return result
        raise NotImplementedError

    def register_model(self, model_type_name: str, culture: str, model_ctor: Callable[[TModelOptions], Model]):
        key=model_ctor_key(model_type=model_type_name, culture=culture)
        if key in self.model_factories:
            raise ValueError
        self.model_factories[key]=model_ctor
    
    def try_get_model(self, model_type_name: str, culture: str, options: TModelOptions) -> Optional[Model]:
        cacheResult=self.get_model_from_cache(model_type_name, culture, options)
        if cacheResult is not None:
            return cacheResult
        key=self.generate_key(model_type_name, culture)
        model_ctor=self.model_factories.get(key, None)
        if model_ctor is not None:
            model=model_ctor(options)
            self.register_model_in_cache(model_type_name, culture, options, model)
            return model
        return None

    def get_model_from_cache(self, model_type_name: str, culture: str, options: TModelOptions) -> Model:
        key=cache_key(model_type=model_type_name, culture=culture, options=options)
        return ModelFactory.__cache.get(key, None)

    def register_model_in_cache(self, model_type_name: str, culture: str, options: TModelOptions, model: Model):
        key=cache_key(model_type=model_type_name, culture=culture, options=options)
        ModelFactory.__cache[key] = model

    def initialize_models(self, target_culture: str, options: TModelOptions):
        for key in self.model_factories:
            if target_culture is None or target_culture is key.culture:
                self.try_get_model(key.model_type, key.culture, key.options)
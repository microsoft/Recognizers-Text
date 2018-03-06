from abc import ABC, abstractmethod
from typing import List, Dict, Generic, TypeVar, Callable, Tuple, NamedTuple
from collections import namedtuple

from .culture import Culture

TModelOptions = TypeVar('TModelOptions')


class ModelResult():
    def __init__(self):
        self.text: str
        self.start: int
        self.end: int
        self.typeName: str
        self.resolution: Dict[str, object]

class Model(ABC):
    @abstractmethod
    def parse(self, query: str) -> List[ModelResult]:
        raise NotImplementedError

class ModelFactoryKey(Generic[TModelOptions]):
    def __init__(self, model_type: str, culture: str, options: TModelOptions):
        self.model_type: str=model_type
        self.culture: str=culture
        self.options: TModelOptions=options

class ModelFactory(Generic[TModelOptions]):
    __fallback_to_default_culture=Culture.English
    __cache: Dict[ModelFactoryKey, Model]=dict()

    def __init__(self):
        self.model_factories: Dict[ModelFactoryKey, Callable[[TModelOptions], Model]]=dict()

    def get_model(self, model_type_name: str, culture: str, fallback_to_default_culture: str, options: TModelOptions) -> Model:
        result=self.try_get_model(model_type_name, culture, options)
        if result.containsModel and fallback_to_default_culture:
            result=self.try_get_model(model_type_name, ModelFactory.__fallback_to_default_culture, options)
        if result.containsModel:
            return result.model
        raise NotImplementedError
    
    def try_get_model(self, model_type_name: str, culture: str, options: TModelOptions):
        result=namedtuple('result', ['containsModel', 'model'])
        cacheResult=self.get_model_from_cache(model_type_name, culture, options)
        if cacheResult is not None:
            return result(True,cacheResult)
        key=self.generate_key(model_type_name, culture)
        modelCtor=self.model_factories.get(key)
        if modelCtor is not None:
            model=modelCtor(options)
            self.register_model_in_cache(model_type_name, culture, options, model)
            return result(True,model)
        return(False,None)

    def get_model_from_cache(self, model_type_name: str, culture: str, options: TModelOptions) -> Model:
        key = ModelFactoryKey(model_type_name, culture, options)
        return ModelFactory.__cache.get(key)
#  Copyright (c) Microsoft Corporation. All rights reserved.
#  Licensed under the MIT License.

from abc import ABC, abstractmethod
from enum import Flag
from typing import List, Dict, Generic, TypeVar, Callable, Optional, Union, Any
from collections import namedtuple

from .culture import Culture

T_MODEL_OPTIONS = TypeVar('T_MODEL_OPTIONS', bound=Flag)


class ModelResult(object):

    # Raw text extracted (lowered)
    text: str
    # Start character index
    start: int
    # End character index (include)
    end: int
    # Value type
    type_name: str
    # More detail extracted result (e.g. value in int)
    resolution: Union[Dict[str, object], List[Dict[str, object]]]

    def __repr__(self) -> str:
        value = self.resolution.get('value', None)

        if value:
            return '{} ({})'.format(self.text, value)

        return self.text

    def __str__(self) -> str:
        return '{}: {} [{}, {}]'.format(
            self.type_name, self.text, self.start, self.end
        )

    def get_dict(self) -> Dict[str, Any]:
        """
        Note: Key value naming follows .NET version
        Reminder: self.__dict__, deep copy on self.resolution
        """
        return {
            'Text': self.text,
            'Start': self.start,
            'End': self.end,
            'TypeName': self.type_name,
            'Resolution': self.resolution.copy()
        }


class Model(ABC):
    @property
    @abstractmethod
    def model_type_name(self) -> str:
        raise NotImplementedError

    @abstractmethod
    def parse(self, query: str) -> List[ModelResult]:
        raise NotImplementedError


CacheKey = namedtuple('CacheKey', ['model_type', 'culture', 'options'])
ModelCtorKey = namedtuple('ModelCtorKey', ['model_type', 'culture'])


class ModelFactory(Generic[T_MODEL_OPTIONS]):
    __fallback_to_default_culture = Culture.English
    __cache: Dict[CacheKey, Model] = dict()

    def __init__(self):
        self.model_factories: Dict[ModelCtorKey,
                                   Callable[[T_MODEL_OPTIONS], Model]] = dict()

    def get_model(self, model_type_name: str, culture: str, fallback_to_default_culture: bool, options: T_MODEL_OPTIONS) -> Model:
        result = self.try_get_model(model_type_name, culture, options)
        if result is None and fallback_to_default_culture is True:
            result = self.try_get_model(
                model_type_name, ModelFactory.__fallback_to_default_culture, options)
        if result is not None:
            return result
        raise ValueError(
            f'Could not find Model with the specified configuration: {culture},{model_type_name}')

    def register_model(self, model_type_name: str, culture: str, model_ctor: Callable[[T_MODEL_OPTIONS], Model]):
        key = ModelCtorKey(model_type=model_type_name, culture=culture)
        if key in self.model_factories:
            raise ValueError
        self.model_factories[key] = model_ctor

    def try_get_model(self, model_type_name: str, culture: str, options: T_MODEL_OPTIONS) -> Optional[Model]:
        cache_result = self.get_model_from_cache(
            model_type_name, culture, options)
        if cache_result is not None:
            return cache_result
        key = ModelCtorKey(model_type=model_type_name, culture=culture)
        model_ctor = self.model_factories.get(key, None)
        if model_ctor is not None:
            model = model_ctor(options)
            self.register_model_in_cache(
                model_type_name, culture, options, model)
            return model
        return None

    def get_model_from_cache(self, model_type_name: str, culture: str, options: T_MODEL_OPTIONS) -> Model:
        key = CacheKey(model_type=model_type_name,
                       culture=culture, options=options)
        return ModelFactory.__cache.get(key, None)

    def register_model_in_cache(self, model_type_name: str, culture: str, options: T_MODEL_OPTIONS, model: Model):
        key = CacheKey(model_type=model_type_name,
                       culture=culture, options=options)
        ModelFactory.__cache[key] = model

    def initialize_models(self, target_culture: str, options: T_MODEL_OPTIONS):
        for key in self.model_factories:
            if target_culture is None or target_culture is key.culture:
                self.try_get_model(key.model_type, key.culture, options)

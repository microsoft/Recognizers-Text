from abc import ABC, abstractclassmethod

class Model(ABC):
    @abstractclassmethod
    def parse(query: str): list

class ModelResult():
    pass
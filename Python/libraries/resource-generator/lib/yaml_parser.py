from ruamel.yaml import YAML

class SimpleRegex:
    yaml_tag = u'!simpleRegex'
    @classmethod
    def from_yaml(cls, constructor, node):
        return node.value

class NestedRegex:
    yaml_tag = u'!nestedRegex'
    @classmethod
    def from_yaml(cls, constructor, node):
        return node.value

class ParamsRegex:
    yaml_tag = u'!paramsRegex'
    @classmethod
    def from_yaml(cls, constructor, node):
        return node.value

class Dictionary:
    yaml_tag = u'!dictionary'
    @classmethod
    def from_yaml(cls, constructor, node):
        return node.value

class List:
    yaml_tag = u'!list'
    @classmethod
    def from_yaml(cls, constructor, node):
        return node.value

class Char:
    yaml_tag = u'!char'
    @classmethod
    def from_yaml(cls, constructor, node):
        return node.value

def parse(content: str):
    yaml=YAML(typ='safe')
    yaml.register_class(SimpleRegex)
    yaml.register_class(NestedRegex)
    yaml.register_class(ParamsRegex)
    yaml.register_class(Dictionary)
    yaml.register_class(List)
    yaml.register_class(Char)
    return yaml.load(content)
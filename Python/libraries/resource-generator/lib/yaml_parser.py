from ruamel.yaml import YAML


class SimpleRegex:
    yaml_tag = u'!simpleRegex'

    def __init__(self, def_):
        self.def_ = def_

    @classmethod
    def from_yaml(cls, constructor, node):
        def_ = None
        for key, value in node.value:
            if key.value == 'def':
                def_ = value.value
        return cls(def_)


class NestedRegex:
    yaml_tag = u'!nestedRegex'

    def __init__(self, def_, references):
        self.def_ = def_
        self.references = references

    @classmethod
    def from_yaml(cls, constructor, node):
        def_, references = None, []
        for key, value in node.value:
            if key.value == 'def':
                def_ = value.value
            if key.value == 'references':
                for reference in value.value:
                    references.append(reference.value)
        return cls(def_, references)


class ParamsRegex:
    yaml_tag = u'!paramsRegex'

    def __init__(self, def_, params):
        self.def_ = def_
        self.params = params

    @classmethod
    def from_yaml(cls, constructor, node):
        def_, params = None, []
        for key, value in node.value:
            if key.value == 'def':
                def_ = value.value
            if key.value == 'params':
                for param in value.value:
                    params.append(param.value)
        return cls(def_, params)


class Dictionary:
    yaml_tag = u'!dictionary'

    def __init__(self, key_type, value_type, entries):
        self.key_type = key_type
        self.value_type = value_type
        self.entries = entries

    @classmethod
    def from_yaml(cls, constructor, node):
        key_type, value_type, entries = None, None, dict()
        for key, value in node.value:
            if key.value == 'types':
                key_type = value.value[0].value
                value_type = value.value[1].value
            if key.value == 'entries':
                for entry in value.value:
                    entries[entry[0].value] = entry[1].value
        return cls(key_type, value_type, entries)


class List:
    yaml_tag = u'!list'

    def __init__(self, type_, entries):
        self.type_ = type_
        self.entries = entries

    @classmethod
    def from_yaml(cls, constructor, node):
        type_, entries = None, []
        for key, value in node.value:
            if key.value == 'types':
                type_ = value.value[0].value
            if key.value == 'entries':
                for entry in value.value:
                    entries.append(entry.value)
        return cls(type_, entries)


class Char:
    yaml_tag = u'!char'

    @classmethod
    def from_yaml(cls, constructor, node):
        return node.value


class Bool:
    yaml_tag = u'!bool'

    @classmethod
    def from_yaml(cls, constructor, node):
        return bool(node.value)


def parse(content: str) -> dict:
    yaml = YAML(typ='safe')
    yaml.register_class(SimpleRegex)
    yaml.register_class(NestedRegex)
    yaml.register_class(ParamsRegex)
    yaml.register_class(Dictionary)
    yaml.register_class(List)
    yaml.register_class(Char)
    yaml.register_class(Bool)
    return yaml.load(content)

import abc
import json
from .yaml_parser import SimpleRegex, NestedRegex, ParamsRegex, Dictionary, List


class CodeWriter:
    def __init__(self, name):
        self.name = name

    @abc.abstractmethod
    def write(self):
        pass


class DefaultWriter(CodeWriter):
    def __init__(self, name, definition):
        CodeWriter.__init__(self, name)
        self.definition = sanitize(definition)

    def write(self):
        return f'{self.name} = \'{self.definition}\''


class BooleanWriter(CodeWriter):
    def __init__(self, name, definition):
        CodeWriter.__init__(self, name)
        self.definition = definition

    def write(self):
        return f'{self.name} = {self.definition}'


class SimpleRegexWriter(CodeWriter):
    def __init__(self, name, definition):
        CodeWriter.__init__(self, name)
        self.definition = sanitize(definition)

    def write(self):
        return f'{self.name} = f\'{self.definition}\''


class NestedRegexWriter(SimpleRegexWriter):
    def __init__(self, name, definition, references):
        CodeWriter.__init__(self, name)
        self.definition = sanitize(definition, None, references)


class ParamsRegexWriter(SimpleRegexWriter):
    def __init__(self, name, definition, params):
        CodeWriter.__init__(self, name)
        self.definition = sanitize(definition, None, params)
        self.params = ', '.join(params)

    def write(self):
        spaces = '    '
        return f'\ndef {self.name}({self.params}):\n{spaces}return f\'{self.definition}\''


class DictionaryWriter(CodeWriter):
    def __init__(self, name, key_type, value_type, entries):
        CodeWriter.__init__(self, name)
        self.entries = []

        for key, value in entries.items():
            key = create_entry(key, key_type)
            if isinstance(value, list):
                value = f"[{', '.join(map(lambda x: json.dumps(x.value), value))}]"
            else:
                value = create_entry(value, value_type)
            self.entries.append(f'({key}, {value})')

    def write(self):
        spaces = ' ' * ((len(f'{self.name} = dict([') + 4) - 4)
        joined_entries = f',\n{spaces}'.join(self.entries)
        return f'{self.name} = dict([{joined_entries}])'


class ArrayWriter(CodeWriter):
    def __init__(self, name, value_type, entries):
        CodeWriter.__init__(self, name)
        self.entries = []
        value_type = to_python_type(value_type)

        value_quote = '\'' if value_type == 'string' else ''

        for value in entries:
            value = value.replace('\'', '\\\'')
            self.entries.append(f'r{value_quote}{value}{value_quote}')

    def write(self):
        joined_entries = ', '.join(self.entries)
        return f'{self.name} = [{joined_entries}]'


def sanitize(value: str, value_type=None, tokens=None):
    value = value.replace('{', '{{').replace('}', '}}')
    if tokens:
        for token in tokens:
            value = value.replace(f'{{{token}}}', token)

    try:
        stringified = json.dumps(value, ensure_ascii=False)
    except:
        stringified = '"' + value + '"'

    return stringified[1:len(stringified) - 1].replace("'", r"\'")


def create_entry(entry, entry_type: str) -> str:
    p_type = to_python_type(entry_type)
    if p_type == 'string':
        quote = '"'
        entry = entry.replace('\\', r'\\').replace('"', r'\"')
    elif p_type == 'bool':
        quote = ""
        entry = bool(entry)
    else:
        quote = ""
    return f'{quote}{entry}{quote}'


def to_python_type(type_: str) -> str:
    if type_ == 'long':
        return 'float'
    elif type_ == 'char':
        return 'string'
    elif type_ == 'bool':
        return 'bool'
    else:
        return type_


def generate_code(root):
    lines = []
    for token_name in root:
        token = root[token_name]
        if isinstance(token, SimpleRegex):
            lines.append(SimpleRegexWriter(token_name, token.def_))
        elif type(token) is NestedRegex:
            lines.append(NestedRegexWriter(
                token_name, token.def_, token.references))
        elif type(token) is ParamsRegex:
            lines.append(ParamsRegexWriter(
                token_name, token.def_, token.params))
        elif type(token) is Dictionary:
            lines.append(DictionaryWriter(
                token_name, token.key_type, token.value_type, token.entries))
        elif type(token) is List:
            lines.append(ArrayWriter(token_name, token.type_, token.entries))
        elif isinstance(token, list):
            inferred_type = 'string'
            lines.append(ArrayWriter(token_name, inferred_type, token))
        elif isinstance(token, bool):
            lines.append(BooleanWriter(token_name, token))
        else:
            lines.append(DefaultWriter(token_name, str(token)))

    return lines

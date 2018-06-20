import abc, json, re
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
        return f'{self.name} = lambda {self.params}: f\'{self.definition}\''

class DictionaryWriter(CodeWriter):
    def __init__(self, name, key_type, value_type, entries):
        CodeWriter.__init__(self, name)
        self.entries = []
        key_type = to_python_type(key_type)
        value_type = to_python_type(value_type)

        key_quote = '\'' if key_type=='string' else ''
        value_quote = '\'' if value_type=='string' else ''
        for key, value in entries.items():
            k = key.replace(r"\'", '\'').replace('\'', r"\'")
            if isinstance(value, list):
                value = ', '.join(map(lambda x: json.dumps(x.value).replace("'", r"\'").replace('"', "'"), value))
                v = f'[{value}]'
            else:
                v = value.replace(r"\'", '\'').replace('\'', r"\'")
            self.entries.append(f'({key_quote}{k}{key_quote}, {value_quote}{v}{value_quote})')

    def write(self):
        spaces = ' ' * (len(f'{self.name} = dict([')+4)
        joined_entries = f',\n{spaces}'.join(self.entries)
        return f'{self.name} = dict([{joined_entries}])'

class ArrayWriter(CodeWriter):
    def __init__(self, name, value_type, entries):
        CodeWriter.__init__(self, name)
        self.entries = []
        value_type = to_python_type(value_type)

        value_quote = '\'' if value_type=='string' else ''
        for value in entries:
            self.entries.append(f'{value_quote}{value}{value_quote}')
    
    def write(self):
        joined_entries = ', '.join(self.entries)
        return f'{self.name} = [{joined_entries}]'

def sanitize(value: str, value_type = None, tokens = None):
    value = value.replace('{','{{').replace('}','}}')
    if tokens:
        for token in tokens:
            value = value.replace(f'{{{token}}}', token)

    try:
        stringified = json.dumps(value, ensure_ascii=False)
    except:
        stringified = '"' + value + '"'

    return stringified[1:len(stringified)-1].replace("'", r"\'")

def to_python_type(type_: str) -> str:
    if type_ == 'long':
        return 'float'
    elif type_ == 'char':
        return 'string'
    else:
        return type_

def generate_code(root):
    lines = []
    for token_name in root:
        token = root[token_name]
        if type(token) is SimpleRegex:
            lines.append(SimpleRegexWriter(token_name, token.def_))
        elif type(token) is NestedRegex:
            lines.append(NestedRegexWriter(token_name, token.def_, token.references))
        elif type(token) is ParamsRegex:
            lines.append(ParamsRegexWriter(token_name, token.def_, token.params))
        elif type(token) is Dictionary:
            lines.append(DictionaryWriter(token_name, token.key_type, token.value_type, token.entries))
        elif type(token) is List:
            lines.append(ArrayWriter(token_name, token.type_, token.entries))
        elif type(token) is list:
            inferred_type = 'string'
            lines.append(ArrayWriter(token_name, inferred_type, token))
        else:
            lines.append(DefaultWriter(token_name, str(token)))

    return lines

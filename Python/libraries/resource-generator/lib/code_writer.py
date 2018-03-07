import abc
from .yaml_parser import SimpleRegex

class CodeWriter:
    def __init__(self, name):
        self.name = name

    @abc.abstractmethod
    def write(self):
        pass

class SimpleRegexWriter(CodeWriter):
    def __init__(self, name, definition):
        CodeWriter.__init__(self, name)
        self.definition = definition

    def write(self):
        return f'{self.name} = f\'{self.definition}\' '


def generate_code(root):
    lines = []
    for token_name in root:
        token = root[token_name]
        if type(token) is SimpleRegex:
            lines.append(SimpleRegexWriter(token_name, token.def_))

    return lines

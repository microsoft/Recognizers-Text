
from .yaml_parser import parse

def generate(yaml_file_path: str, py_file_name: str, header: str, footer: str):
    yaml_raw = open(yaml_file_path, encoding="utf-8")
    yaml_object = parse(yaml_raw)


    print('generate():')
    print(yaml_object)

import sys, json, os
from collections import namedtuple
from lib.base_code_generator import generate

RESOURCES_PATH = ['..', '..', '..', 'Patterns']

class Startup:

    def main(self, argv) -> int:
        resource_definition_path = argv[1]
        specs = json.load(open(resource_definition_path), object_hook=lambda d: namedtuple('X', d.keys())(*d.values()))
        output_path = os.path.join(os.path.dirname(resource_definition_path), specs.outputPath)

        for config in specs.configFiles:
            input_file_path = os.path.join(*RESOURCES_PATH, *config.input) + '.yaml'
            output_file_path = os.path.join(output_path, config.output) + '.py'
            print(f'{ os.path.basename(input_file_path) } => { os.path.basename(output_file_path) }')
            print
            try:
                generate(input_file_path, output_file_path, '\n'.join(config.header), '\n'.join(config.footer))
            except Exception as ex:
                print(f'Error while creating the resource { os.path.basename(output_file_path) }:', ex)


if __name__ == '__main__':
    app = Startup()
    app.main(sys.argv)

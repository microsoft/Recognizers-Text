import * as generator from "./lib/base-code-generator";
import { join, basename } from "path";

class resourceConfig {
    input: Array<string>;
    output: string;
    header: Array<string>;
    footer: Array<string>;
}

class resourceDefinitions {
    outputPath: string;
    configFiles: Array<resourceConfig>;
}

const resourcesPath = '../../../Patterns/';

class Startup {
    public static main(): number {
        let resourceDefinitionPath = process.argv[2];
        let specs: resourceDefinitions = require(resourceDefinitionPath);
        let outputPath = specs.outputPath;
        specs.configFiles.forEach(config => {
            let inputFilePath = join(resourcesPath, ...config.input).concat('.yaml');
            let outputFilePath = join(outputPath, config.output).concat('.ts');
            console.log(`${ basename(inputFilePath) } => ${ basename(outputFilePath) }`);
            try {
                generator.generate(inputFilePath, outputFilePath, config.header.join('\n') , config.footer.join('\n'))
            } catch (err) {
                console.log(`Error while creating the resource ${ basename(outputFilePath) }`, err.toString())
            }
        });

        return 0;
    }
}

Startup.main();
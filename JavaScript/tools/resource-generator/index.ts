import * as generator from "./lib/base-code-generator";

let resourcesPath = '../Patterns/';
let outputPath = "./src/resources/";

let configs = [
    // COMMON
    {
        yaml: `${resourcesPath}Base-Numbers.yaml`,
        output: `${outputPath}numericBase.ts`,
        header: `export namespace BaseNumbers {`,
        footer: `}`
    },
    // ENGLISH NUMERIC
    {
        yaml: `${resourcesPath}/English/English-Numbers.yaml`,
        output: `${outputPath}numericEnglish.ts`,
        header:
        `import { BaseNumbers } from "./numericBase";
export namespace EnglishNumeric {`,
        footer: `}`
    }
];

class Startup {
    public static main(): number {
        configs.forEach(config => {
            generator.generate(config.yaml, config.output, config.header, config.footer);
        });

        return 0;
    }
}

Startup.main();
import * as generator from "./lib/base-code-generator";

let resourcesPath = '../../Patterns/';
let outputPath = "./src/resources/";

let configs = [
    // COMMON NUMERIC
    {
        yaml: `${resourcesPath}Base-Numbers.yaml`,
        output: `${outputPath}baseNumbers.ts`,
        header: `export namespace BaseNumbers {`,
        footer: `}`
    },
    // ENGLISH NUMERIC
    {
        yaml: `${resourcesPath}/English/English-Numbers.yaml`,
        output: `${outputPath}englishNumeric.ts`,
        header:
        `import { BaseNumbers } from "./baseNumbers";
export namespace EnglishNumeric {`,
        footer: `}`
    },
    // SPANISH NUMERIC
    {
        yaml: `${resourcesPath}/Spanish/Spanish-Numbers.yaml`,
        output: `${outputPath}spanishNumeric.ts`,
        header:
        `import { BaseNumbers } from "./baseNumbers";
export namespace SpanishNumeric {`,
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
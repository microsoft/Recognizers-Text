import * as generator from "./lib/base-code-generator";

let resourcesPath = '../Common/';
let outputPath = "./src/resources/";

let configs = [
    // COMMON
    {
        yaml: `${resourcesPath}Common-Numeric.yaml`,
        output: `${outputPath}numericCommon.ts`,
        header: `export namespace CommonNumeric {`,
        footer: `}`
    },
    // ENGLISH NUMERIC
    {
        yaml: `${resourcesPath}Numeric-English.yaml`,
        output: `${outputPath}numericEnglish.ts`,
        header:
        `import { CommonNumeric } from "./numericCommon";
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
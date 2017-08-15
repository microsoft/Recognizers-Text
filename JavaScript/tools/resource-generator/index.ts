import * as generator from "./lib/base-code-generator";

let resourcesPath = '../Patterns/';
let outputPath = "./src/resources/";

let configs = [
    // COMMON NUMERIC
    {
        yaml: `${resourcesPath}Base-Numbers.yaml`,
        output: `${outputPath}baseNumbers.ts`,
        header: `export namespace BaseNumbers {`,
        footer: `}`
    },
    // COMMON DATE TIME
    {
        yaml: `${resourcesPath}Base-DateTime.yaml`,
        output: `${outputPath}baseDateTime.ts`,
        header: `export namespace BaseDateTime {`,
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
    // ENGLISH NUMERIC WITH UNIT
    {
        yaml: `${resourcesPath}/English/English-NumbersWithUnit.yaml`,
        output: `${outputPath}englishNumericWithUnit.ts`,
        header:
        `import { BaseNumbers } from "./baseNumbers";
export namespace EnglishNumericWithUnit {`,
        footer: `}`
    },
    // ENGLISH DATE TIME WITH UNIT
    {
        yaml: `${resourcesPath}/English/English-DateTime.yaml`,
        output: `${outputPath}englishDateTime.ts`,
        header:
        `import { BaseDateTime } from "./baseDateTime";
export namespace EnglishDateTime {`,
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
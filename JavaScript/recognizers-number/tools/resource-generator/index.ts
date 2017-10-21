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
    },
    // PORTUGUESE NUMERIC
    {
        yaml: `${resourcesPath}/Portuguese/Portuguese-Numbers.yaml`,
        output: `${outputPath}portugueseNumeric.ts`,
        header:
        `import { BaseNumbers } from "./baseNumbers";
export namespace PortugueseNumeric {`,
        footer: `}`
    },
    // CHINESE NUMERIC
    {
        yaml: `${resourcesPath}/Chinese/Chinese-Numbers.yaml`,
        output: `${outputPath}chineseNumeric.ts`,
        header:
        `import { BaseNumbers } from "./baseNumbers";
export namespace ChineseNumeric {`,
        footer: `}`
    },
    // FRENCH NUMERIC
    {
        yaml: `${resourcesPath}/French/French-Numbers.yaml`,
        output: `${outputPath}frenchNumeric.ts`,
        header:
        `import { BaseNumbers } from "./baseNumbers";
export namespace FrenchNumeric {`,
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
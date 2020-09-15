import { DataTypes } from "./data-types";

export abstract class CodeWriter {
    readonly name: string;
    abstract write(): string;

    constructor(name: string) {
        this.name = name;
    }
}

class DefaultWriter extends CodeWriter {
    readonly definition: string;

    constructor(name: string, definition: string) {
        super(name);
        this.definition = sanitizeDefinition(definition);
    }

    write() {
        return `export const ${this.name} = \`${this.definition}\`;`;
    }
}

class BooleanWriter extends CodeWriter {
    readonly definition: boolean;

    constructor(name: string, definition: boolean) {
        super(name);
        this.definition = definition;
    }

    write() {
        return `export const ${this.name} = ${this.definition};`;
    }
}

class SimpleRegexWriter extends CodeWriter {
    readonly definition: string;

    constructor(name: string, definition: string) {
        super(name);
        this.definition = sanitizeDefinition(definition, 'regex');
    }

    write() {
        return `export const ${this.name} = \`${this.definition}\`;`;
    }
}

class NestedRegexWriter extends CodeWriter {
    readonly definition: string;

    constructor(name: string, definition: string, references: string[]) {
        super(name);
        references.forEach((value, index) => {
            let regex = new RegExp(`{${value}}`, 'g');
            let token = `\${${value}}`;
            definition = definition.replace(regex, token);
        });
        this.definition = sanitizeDefinition(definition, 'regex');
    }

    write() {
        return `export const ${this.name} = \`${this.definition}\`;`;
    }
}

class ParamsRegexWriter extends CodeWriter {
    readonly definition: string;
    readonly params: string;

    constructor(name: string, definition: string, params: string[]) {
        super(name);
        params.forEach((value, index) => {
            let regex = new RegExp(`{${value}}`, 'g');
            let token = `\${${value}}`;
            definition = definition.replace(regex, token);
        });
        this.params = params.join(': string, ').concat(': string');
        this.definition = sanitizeDefinition(definition, 'regex');
    }

    write() {
        return `export const ${this.name} = (${this.params}) => { return \`${this.definition}\`; }`;
    }
}

class DictionaryWriter extends CodeWriter {
    readonly keyType: string;
    readonly valueType: string;
    readonly entries: string[];

    constructor(name: string, keyType: string, valueType: string, entries: Record<string, any>) {
        super(name);
        this.entries = [];

        this.keyType = toJsType(keyType);
        this.valueType = toJsType(valueType);

        let valueQuote1: string;
        let valueQuote2: string;
        if (this.valueType.endsWith("[]")) {
            valueQuote1 = "[";
            valueQuote2 = "]";
        }
        else {
            valueQuote1 = valueQuote2 = this.valueType === 'number' ? '' : '"';
        }

        for (let propName in entries) {
            this.entries.push(`["${sanitizeElement(propName, this.keyType)}", ${valueQuote1}${sanitizeElement(entries[propName], this.valueType)}${valueQuote2}]`);
        }
    }

    write() {
        return `export const ${this.name}: ReadonlyMap<${this.keyType}, ${this.valueType}> = new Map<${this.keyType}, ${this.valueType}>([${this.entries.join(',')}]);`;
    }
}

function sanitizeElement(value: string, valueType: string): string {
    return sanitizeValue(value, valueType, false);
}

function sanitizeDefinition(value: string, valueType: string = null): string {
    
    if (!valueType) {
        valueType = typeof value;
    }

    return sanitizeValue(value, valueType, true);
}

function sanitizeValue(value: string, valueType: string, directUse: boolean = false): string {

    if (valueType === 'number' || valueType === 'boolean') {
        return value;
    }

    let stringified = JSON.stringify(value);

    // Escape backtick, only if value is directly used, as code generation will output values as backticked strings.
    if (directUse) {
        stringified = stringified.replace(/(?<!(\\))`/gi, '\\`');
    }

    return stringified.slice(1, stringified.length - 1);
}

function toJsType(type: string): string {
    switch (type) {
        case 'char': return 'string';
        case 'long':
        case 'double':
        case 'int': return 'number';
        case 'long[]':
        case 'int[]':
        case 'double[]': return 'number[]';
        default: return type;
    }
}

class ArrayWriter extends CodeWriter {
    readonly valueType: string;
    readonly entries: string[];

    constructor(name: string, entries: any[]) {
        super(name);
        this.entries = [];
        this.valueType = typeof (entries[0]);
        entries.forEach(element => {
            this.entries.push(`"${sanitizeElement(element, typeof element)}"`);
        });
    }

    write() {
        return `export const ${this.name} = [ ${this.entries.join(',')} ];`;
    }
}

export function GenerateCode(root: any): CodeWriter[] {

    let lines: CodeWriter[] = [];

    for (let tokenName in root) {
        
        let token = root[tokenName];
        if (token instanceof DataTypes.SimpleRegex) {
            lines.push(new SimpleRegexWriter(tokenName, token.def));
        }
        else if (token instanceof DataTypes.NestedRegex) {
            lines.push(new NestedRegexWriter(tokenName, token.def, token.references));
        }
        else if (token instanceof DataTypes.ParamsRegex) {
            lines.push(new ParamsRegexWriter(tokenName, token.def, token.params));
        }
        else if (token instanceof DataTypes.Dictionary) {
            lines.push(new DictionaryWriter(tokenName, token.keyType, token.valueType, token.entries));
        }
        else if (token instanceof DataTypes.List) {
            lines.push(new ArrayWriter(tokenName, token.entries));
        }
        else if (token instanceof Array) {
            lines.push(new ArrayWriter(tokenName, token));
        }
        else if (typeof token === "boolean") {
            lines.push(new BooleanWriter(tokenName, token));
        }
        else {
            lines.push(new DefaultWriter(tokenName, token));
        }
    };

    return lines;
}
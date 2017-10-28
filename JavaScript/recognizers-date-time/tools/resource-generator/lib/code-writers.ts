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

    constructor (name: string, definition: string) {
        super(name);
        this.definition = sanitize(definition);
    }

    write() {
        return `export const ${this.name} = '${this.definition}';`;
    }
}

class SimpleRegexWriter extends CodeWriter {
    readonly definition: string;

    constructor (name: string, definition: string) {
        super(name);
        this.definition = sanitize(definition);
    }

    write() {
        return `export const ${this.name} = \`${this.definition}\`;`;
    }
}

class NestedRegexWriter extends CodeWriter {
    readonly definition: string;

    constructor (name: string, definition: string, references: string[]) {
        super(name);
        references.forEach((value, index) => {
            let regex = new RegExp(`{${value}}`, 'g');
            let token = `\${${value}}`;
            definition = definition.replace(regex, token);
        });
        this.definition = sanitize(definition);
    }

    write() {
        return `export const ${this.name} = \`${this.definition}\`;`;
    }
}

class ParamsRegexWriter extends CodeWriter {
    readonly definition: string;
    readonly params: string;

    constructor (name: string, definition: string, params: string[]) {
        super(name);
        params.forEach((value, index) => {
            let regex = new RegExp(`{${value}}`, 'g');
            let token = `\${${value}}`;
            definition = definition.replace(regex, token);
        });
        this.params = params.join(': string, ').concat(': string');
        this.definition = sanitize(definition);
    }

    write() {
        return `export const ${this.name} = (${this.params}) => { return \`${this.definition}\`; }`
    }
}

class DictionaryWriter extends CodeWriter {
    readonly keyType: string;
    readonly valueType: string;
    readonly entries: string[];

    constructor(name: string, keyType: string, valueType: string, entries: Object) {
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
        
        for(let propName in entries) {
            this.entries.push(`["${propName}", ${valueQuote1}${sanitize(entries[propName], this.valueType)}${valueQuote2}]`);
        }
    }

    write() {
        return `export const ${this.name}: ReadonlyMap<${this.keyType}, ${this.valueType}> = new Map<${this.keyType}, ${this.valueType}>([${this.entries.join(',')}]);`
    }
}


function sanitize(value: string, valueType: string = null) : string {
    if(valueType === 'number') return value;

    let stringified = JSON.stringify(value);
    return stringified.slice(1, stringified.length - 1);
}

function toJsType(type: string): string {
    switch(type) {
        case 'char': return 'string';
        case 'long':
        case 'double':
        case 'int': return 'number';
        default: return type;
    }
}

class ArrayWriter extends CodeWriter {
    readonly valueType: string;
    readonly entries: string[];

    constructor(name: string, entries: Array<any>) {
        super(name);
        this.entries = [];
        this.valueType = typeof(entries[0]);
        entries.forEach(element => {
            this.entries.push(`'${sanitize(element)}'`)
        });
    }

    write() {
        return `export const ${this.name} = [ ${this.entries.join(',')} ];`
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
        else {
            lines.push(new DefaultWriter(tokenName, token));
        }
    };
    return lines;
}
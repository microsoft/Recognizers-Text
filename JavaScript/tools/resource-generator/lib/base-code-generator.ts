import * as YamlParser from "./yaml-parser";
import { GenerateCode, CodeWriter } from "./code-writers";
import { readFileSync, createWriteStream, write, WriteStream } from "fs";
import { EOL } from "os";

export function generate(yamlFilePath: string, tsFileName: string, header: string, footer: string) {
    let yamlRaw = readFileSync(yamlFilePath, 'utf8');
    let yamlObj = YamlParser.parse(yamlRaw);
    let code = GenerateCode(yamlObj);
    let file = createWriteStream(tsFileName);
    file.write(header + EOL);
    for(let line in code) {
        file.write('\t' + code[line].write() + EOL);
    }
    file.write(footer + EOL);
    file.end();
}
import * as YamlParser from "./yaml-parser";
import { GenerateCode, CodeWriter } from "./code-writers";
import { readFileSync, createWriteStream, write, WriteStream } from "fs";
import { EOL } from "os";

export function generate(yamlFilePath: string, tsFileName: string, header: string, footer: string) {
    var yamlRaw = readFileSync(yamlFilePath, 'utf8');
    var yamlObj = YamlParser.parse(yamlRaw);
    var code = GenerateCode(yamlObj);
    var file = createWriteStream(tsFileName);
    file.write(header + EOL);
    for(var line in code) {
        file.write('\t' + code[line].write() + EOL);
    }
    file.write(footer + EOL);
    file.end();
}
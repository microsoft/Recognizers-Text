import * as YamlParser from "./yaml-parser";
import { GenerateCode, CodeWriter } from "./code-writers";
import { readFileSync, createWriteStream, write, WriteStream } from "fs";
import { EOL } from "os";

const headerComment =
`// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------`;

export function generate(yamlFilePath: string, tsFileName: string, header: string, footer: string) {

    let yamlRaw = readFileSync(yamlFilePath, 'utf8');
    let yamlObj = YamlParser.parse(yamlRaw);
    let code = GenerateCode(yamlObj);
    let file = createWriteStream(tsFileName);
    file.write(headerComment + EOL + EOL);
    file.write(header + EOL);
    for (let line in code) {
        file.write('\t' + code[line].write() + EOL);
    }
    file.write(footer + EOL);
    file.end();
}
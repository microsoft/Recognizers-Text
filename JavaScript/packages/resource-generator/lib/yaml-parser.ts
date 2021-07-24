// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import * as Yaml from "js-yaml";
import { DataTypes } from "./data-types";

const SimpleRegexYamlType = new Yaml.Type('!simpleRegex', {
    kind: 'mapping',
    construct: (data) => DataTypes.getSimpleRegex(data)
});

const NestedRegexYamlType = new Yaml.Type('!nestedRegex', {
    kind: 'mapping',
    construct: (data) => DataTypes.getNestedRegex(data)
});

const ParamsRegexYamlType = new Yaml.Type('!paramsRegex', {
    kind: 'mapping',
    construct: (data) => DataTypes.getParamsRegex(data)
});

const DictionaryYamlType = new Yaml.Type('!dictionary', {
    kind: 'mapping',
    construct: (data) => DataTypes.getDictionary(data)
});

const ListYamlType = new Yaml.Type('!list', {
    kind: 'mapping',
    construct: (data) => DataTypes.getList(data)
});

const CharYamlType = new Yaml.Type('!char', {
    kind: 'scalar',
    construct: (data) => DataTypes.getCharacter(data)
});

const BooleanYamlType = new Yaml.Type('!bool', {
    kind: 'scalar',
    construct: (data) => DataTypes.getBoolean(data)
});

const SCHEMA = Yaml.Schema.create([SimpleRegexYamlType, NestedRegexYamlType, ParamsRegexYamlType, DictionaryYamlType, ListYamlType, CharYamlType, BooleanYamlType]);
const yamlOptions = { schema: SCHEMA };

export function parse(fileStream: string) {
    return Yaml.load(fileStream, yamlOptions);
}
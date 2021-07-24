// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { StringUtility } from "@microsoft/recognizers-text";

export class DictionaryUtils {
    static bindDictionary(dictionary: ReadonlyMap<string, string>, source: Map<string, string>) {
        if (dictionary === null) {
            return;
        }

        dictionary.forEach((value, key) => {
            if (StringUtility.isNullOrEmpty(key)) {
                return;
            }

            this.bindUnitsString(source, key, value);
        });
    }

    static bindUnitsString(dictionary: Map<string, string>, key: string, source: string) {
        let values = source.trim().split('|');
        values.forEach(token => {

            if (StringUtility.isNullOrWhitespace(token) || dictionary.has(token)) {
                return;
            }

            dictionary.set(token, key);
        });
    }
}
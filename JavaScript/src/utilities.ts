import * as XRegExp from "xregexp";

export class FormatUtility {
    static preProcess(query: string, toLower: boolean = true): string {
        if (toLower) {
            query = query.toLowerCase()
        }

        return query
            .replace(/０/g, "0")
            .replace(/１/g, "1")
            .replace(/２/g, "2")
            .replace(/３/g, "3")
            .replace(/４/g, "4")
            .replace(/５/g, "5")
            .replace(/６/g, "6")
            .replace(/７/g, "7")
            .replace(/８/g, "8")
            .replace(/９/g, "9")
            .replace(/：/g, ":")
            .replace(/－/g, "-")
            .replace(/，/g, ",")
            .replace(/／/g, "/")
            .replace(/Ｇ/g, "G")
            .replace(/Ｍ/g, "M")
            .replace(/Ｔ/g, "T")
            .replace(/Ｋ/g, "K")
            .replace(/ｋ/g, "k")
            .replace(/．/g, ".")
            .replace(/（/g, "(")
            .replace(/）/g, ")")
    }
}

export function isNullOrWhitespace(input: string): boolean {
    return !input || !input.trim();
}

export function isWhitespace(input: string): boolean {
    return input && !input.trim();
}

export interface Match {
    index: number;
    length: number;
    value: string;
    groups: { [id: string]: string }; //Map<string, string>;
}

export class RegExpUtility {
    static getMatches(regex: RegExp, source: string): Array<Match> {
        let matches = new Array<Match>();
        XRegExp.forEach(source, regex, match => {
            let positiveLookbehind = [];
            let negativeLookbehind = [];
            let groups = {};
            Object.keys(match).forEach(key => {
                if (!key.includes('__')) return;
                if (key.startsWith('plb') && match[key]) {
                    positiveLookbehind.push({key:key, value:match[key]});
                    return;
                }
                if (key.startsWith('nlb') && match[key]) {
                    negativeLookbehind.push({key:key, value:match[key]});
                    return;
                }
                groups[key.substr(0, key.lastIndexOf('__'))] = match[key];
            });
            
            let value = match[0];
            let index = match.index;
            let length = value.length;

            if (positiveLookbehind && positiveLookbehind.length > 0 && match[0].indexOf(positiveLookbehind[0].value) ===  0) {
                value = value.substr(positiveLookbehind[0].value.length)
                index += positiveLookbehind[0].value.length
                length -= positiveLookbehind[0].value.length
            }
            if (negativeLookbehind && negativeLookbehind.length > 0) return;
            matches.push({
                    value: value,
                    index: index,
                    length: length,
                    groups: groups
                });
        });
        return matches;
    }

    private static tokenizer = XRegExp('\\?<(?<token>\\w+)>', 'gis');

    private static sanitizeGroups(source: string): string {
        let index = 0;
        let replacer = XRegExp.replace(source, this.tokenizer, function(match, token) {
            return match.replace(token, `${token}__${index++}`);
        });
        return replacer;
    }

    static getSafeRegExp(source: string, flags: string): RegExp {
        let sanitizedSource = this.sanitizeGroups(source);
        return XRegExp(sanitizedSource, flags);
    }
}
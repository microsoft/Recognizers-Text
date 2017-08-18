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

export interface Match {
    index: number;
    length: number;
    value: string;
    groups: ReadonlyMap<string, string>;
}

export class RegExpUtility {
    static getMatches(regex: RegExp, source: string): Array<Match> {
        let matches = new Array<Match>();

        let pos = 0;
        let m = XRegExp.exec(source, regex);
        while (m) {
            matches.push({
                value: m[0],
                index: m.index,
                length: m[0].length,
                groups: RegExpUtility.getSanitizedGroups(m)
            });
            pos = m.index + m[0].length;
            m = XRegExp.exec(source, regex, pos)
        }

        return matches;
    }

    static getSafeRegExp(source: string, flags: string): RegExp {
        let sanitizedSource = this.sanitizeGroups(source);
        return XRegExp(sanitizedSource, flags);
    }

    private static getSanitizedGroups(match: any): Map<string, string> {
        let groups = new Map<string, string>();
        Object.getOwnPropertyNames(match).forEach(sanitizedName => {
            let indexPos = sanitizedName.indexOf("_");
            if (indexPos > 0) {
                let name = sanitizedName.substring(0, indexPos);
                groups[name] = match[sanitizedName];
            }
        });

        return groups;
    }

    private static tokenizer = XRegExp('\\?<(?<token>\\w+)>', 'gis');

    private static sanitizeGroups(source: string): string {
        let index = 0;
        let replacer = XRegExp.replace(source, this.tokenizer, function(match, token) {
            return match.replace(token, `${token}_${index++}`);
        });
        return replacer;
    }
}
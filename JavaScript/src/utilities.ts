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

export interface Match {
    index: number;
    length: number;
    value: string;
}

export class RegExpUtility {
    static getMatches(regex: RegExp, source: string): Array<Match> {
        let matches = new Array<Match>();

        let m;
        regex.lastIndex = 0;
        do {
            m = regex.exec(source);
            if (m) {
                matches.push({
                    value: m[0],
                    index: m.index,
                    length: m[0].length
                });
            }
        } while (m);

        return matches;
    }

    static tokenizer = XRegExp('\\?<(?<token>\\w+)>', 'gis');

    static sanitizeGroups(source: string): string {
        let index = 0;
        let replacer = XRegExp.replace(source, this.tokenizer, function(match, token) {
            return match.replace(token, token + index++);
        });
        return replacer;
    }
}
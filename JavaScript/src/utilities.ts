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

export class StringUtility {
    static isNullOrWhitespace(input: string): boolean {
        return !input || !input.trim();
    }

    static isNullOrEmpty(input: string): boolean {
        return !input || input === '';
    }

    static isWhitespace(input: string): boolean {
        return input && input !== '' && !input.trim();
    }

    static insertInto(input: string, value: string, index: number): string {
        return input.substr(0, index) + value + input.substr(index);
    }
}

export class Match {
    constructor (index: number, length: number, value: string, groups) {
        this.index = index;
        this.length = length;
        this.value = value;
        this.innerGroups = groups;
    }

    index: number;
    length: number;
    value: string;
    private innerGroups: { [id: string]: { value: string, captures: string[] } };

    groups(key: string): { value: string, captures: string[] } {
        return this.innerGroups[key] ? this.innerGroups[key] : { value: '', captures: [] };
    }
}

export class RegExpUtility {
    static getMatches(regex: RegExp, source: string): Array<Match> {
        let matches = new Array<Match>();
        XRegExp.forEach(source, regex, match => {
            let positiveLookbehinds = [];
            let nlbCount = 0;
            let nlbFound = 0;
            let groups = { };

            Object.keys(match).forEach(key => {
                if (!key.includes('__')) return;
                if (key.startsWith('plb') && match[key]) {
                    positiveLookbehinds.push({key:key, value:match[key]});
                    return;
                }
                if (key.startsWith('nlb')) {
                    nlbCount++;
                    if (match[key]) nlbFound++;
                    return;
                }

                let groupKey = key.substr(0, key.lastIndexOf('__'));

                if (!groups[groupKey]) groups[groupKey] = { value: '', captures: [] };

                if (match[key]) {
                    groups[groupKey].value = match[key];
                    groups[groupKey].captures.push(match[key]);
                }
            });
            
            let value = match[0];
            let index = match.index;
            let length = value.length;

            if (positiveLookbehinds && positiveLookbehinds.length > 0 && value.indexOf(positiveLookbehinds[0].value) ===  0) {
                value = value.substr(positiveLookbehinds[0].value.length)
                index += positiveLookbehinds[0].value.length
                length -= positiveLookbehinds[0].value.length
            }
            if (nlbCount > 0 && nlbCount === nlbFound) return;
            matches.push(new Match(index, length, value, groups));
        });
        return matches;
    }

    private static matchGroup = XRegExp(String.raw`\?<(?<name>\w+)>`, 'gis');
    private static matchPositiveLookbehind = XRegExp(String.raw`\(\?<=`, 'gis');
    private static matchNegativeLookbehind = XRegExp(String.raw`\(\?<!`, 'gis');

    private static sanitizeGroups(source: string): string {
        let index = 0;
        let result = XRegExp.replace(source, this.matchGroup, (match, name) => match.replace(name, `${name}__${index++}`));
        index = 0;
        result = XRegExp.replace(result, this.matchPositiveLookbehind, () => `(?<plb__${index++}>`);
        index = 0;
        result = XRegExp.replace(result, this.matchNegativeLookbehind, () => `(?<nlb__${index++}>`);
        let closePos = 0;
        let startPos = result.indexOf('(?<nlb__', closePos);
        while (startPos >= 0) {
            closePos = this.getClosePos(result, startPos);
            result = StringUtility.insertInto(result, '?', closePos + 1);
            startPos = result.indexOf('(?<nlb__', closePos);
        }
        return result;
    }

    private static getClosePos(source: string, startPos: number): number {
        let counter = 1;
        let closePos = startPos;
        while (counter > 0 && closePos < source.length) {
            let c = source[++closePos];
            if (c === '(') counter++;
            else if (c === ')') counter--;
        }
        return closePos;
    }

    static getSafeRegExp(source: string, flags?: string): RegExp {
        let sanitizedSource = this.sanitizeGroups(source);
        return XRegExp(sanitizedSource, flags || 'gis');
    }
}
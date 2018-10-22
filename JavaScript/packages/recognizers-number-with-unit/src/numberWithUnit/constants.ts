import { BaseUnits } from "../resources/baseUnits";

export class Constants {
    static readonly SYS_UNIT: string = "builtin.unit";
    static readonly SYS_UNIT_DIMENSION: string = "builtin.unit.dimension";
    static readonly SYS_UNIT_AGE: string = "builtin.unit.age";
    static readonly SYS_UNIT_AREA: string = "builtin.unit.area";
    static readonly SYS_UNIT_CURRENCY: string = "builtin.unit.currency";
    static readonly SYS_UNIT_LENGTH: string = "builtin.unit.length";
    static readonly SYS_UNIT_SPEED: string = "builtin.unit.speed";
    static readonly SYS_UNIT_TEMPERATURE: string = "builtin.unit.temperature";
    static readonly SYS_UNIT_VOLUME: string = "builtin.unit.volume";
    static readonly SYS_UNIT_WEIGHT: string = "builtin.unit.weight";
    static readonly SYS_NUM: string = "builtin.num";

    // For cases like '2:00 pm', both 'pm' and '00 pm' are not dimension
    static readonly AMBIGUOUS_TIME_TERM: string = BaseUnits.AmbiguousTimeTerm;

    // For currencies without ISO codes, we use internal values prefixed by '_'.
    // These values should never be present in parse output.
    static readonly FAKE_ISO_CODE_PREFIX: string = "_";
}
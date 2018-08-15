class Constants:
    SYS_UNIT: str = 'builtin.unit'
    SYS_UNIT_DIMENSION: str = 'builtin.unit.dimension'
    SYS_UNIT_AGE: str = 'builtin.unit.age'
    SYS_UNIT_AREA: str = 'builtin.unit.area'
    SYS_UNIT_CURRENCY: str = 'builtin.unit.currency'
    SYS_UNIT_LENGTH: str = 'builtin.unit.length'
    SYS_UNIT_SPEED: str = 'builtin.unit.speed'
    SYS_UNIT_TEMPERATURE: str = 'builtin.unit.temperature'
    SYS_UNIT_VOLUME: str = 'builtin.unit.volume'
    SYS_UNIT_WEIGHT: str = 'builtin.unit.weight'
    SYS_NUM: str = "builtin.num";

    # For currencies without ISO codes, we use internal values prefixed by '_'. 
    # These values should never be present in parse output.
    FAKE_ISO_CODE_PREFIX = '_'
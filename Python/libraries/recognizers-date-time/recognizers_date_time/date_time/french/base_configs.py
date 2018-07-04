from typing import Pattern
from recognizers_text.utilities import RegExpUtility
from ...resources.french_date_time import FrenchDateTime
from ..base_date import DateTimeUtilityConfiguration

class FrenchDateTimeUtilityConfiguration(DateTimeUtilityConfiguration):
    @property
    def ago_regex(self) -> Pattern:
        return self._ago_regex

    @property
    def later_regex(self) -> Pattern:
        return self._later_regex

    @property
    def in_connector_regex(self) -> Pattern:
        return self._in_connector_regex

    @property
    def range_unit_regex(self) -> Pattern:
        return self._range_unit_regex

    @property
    def am_desc_regex(self) -> Pattern:
        return self._am_desc_regex

    @property
    def pm_desc__regex(self) -> Pattern:
        return self._pm_desc__regex

    @property
    def am_pm_desc_regex(self) -> Pattern:
        return self._am_pm_desc_regex

    def __init__(self):
        self._later_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.LaterRegex)
        self._ago_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.AgoPrefixRegex)
        self._in_connector_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.InConnectorRegex)
        self._range_unit_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.RangeUnitRegex)
        self._am_desc_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.AmDescRegex)
        self._pm_desc__regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.PmDescRegex)
        self._am_pm_desc_regex = RegExpUtility.get_safe_reg_exp(FrenchDateTime.AmPmDescRegex)

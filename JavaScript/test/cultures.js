var Culture = require('../compiled/culture').Culture;

// List of supported cultures
module.exports = {
    'Eng': Culture.supportedCultures.find(c => c.cultureCode === Culture.English),
    'Spa': Culture.supportedCultures.find(c => c.cultureCode === Culture.Spanish)
};
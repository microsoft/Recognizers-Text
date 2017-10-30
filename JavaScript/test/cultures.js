var Culture = require('recognizers-text-number').Culture;

// List of supported cultures
module.exports = {
    'English': Culture.supportedCultures.find(c => c.cultureCode === Culture.English),
    'Spanish': Culture.supportedCultures.find(c => c.cultureCode === Culture.Spanish),
    'Chinese': Culture.supportedCultures.find(c => c.cultureCode === Culture.Chinese),
    'Portuguese': Culture.supportedCultures.find(c => c.cultureCode === Culture.Portuguese),
    'French': Culture.supportedCultures.find(c => c.cultureCode === Culture.French)
};
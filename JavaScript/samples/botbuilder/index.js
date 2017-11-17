// Our Number and DateTime Recognizer models
var Recognizers = require('recognizers-text');
var numberModel = Recognizers.NumberRecognizer.instance.getNumberModel(Recognizers.Culture.English);
var dateModel = Recognizers.DateTimeRecognizer.instance.getDateTimeModel(Recognizers.Culture.English);

// This loads the environment variables from the .env file
require('dotenv-extended').load();

var _ = require('lodash');
var builder = require('botbuilder');
var restify = require('restify');

// Setup Restify Server
var server = restify.createServer();
server.listen(process.env.port || process.env.PORT || 3978, function () {
    console.log('%s listening to %s', server.name, server.url);
});

// Create chat bot and listen to messages
var connector = new builder.ChatConnector({
    appId: process.env.MICROSOFT_APP_ID,
    appPassword: process.env.MICROSOFT_APP_PASSWORD
});
server.post('/api/messages', connector.listen());

var bot = new builder.UniversalBot(connector, [
    function (session) {
        // Welcome message
        var welcomeCard = new builder.HeroCard(session)
            .title('Welcome to Contoso Roses')
            .subtitle('These are the roses you are looking for!')
            .images([
                new builder.CardImage(session)
                    .url('https://placeholdit.imgix.net/~text?txtsize=56&txt=Contoso%20Roses&w=640&h=330')
                    .alt('Contoso Roses')
            ]);

        session.send(new builder.Message(session).addAttachment(welcomeCard));

        // Prompt for amount of roses
        var promptMessage = [
            'How many roses do you want to send?',
            'Some valid options are:',
            ' - A dozen',
            ' - 22',
            ' - Just one rose'];

        session.beginDialog('ask-amount', { prompt: promptMessage.join('\n\n') });
    },

    function (session, results) {
        // results.response contains the prompt returned value
        var amount = results.response;
        var amountMsg = session.ngettext(`I'll send just one rose.`, `I'll send ${amount} roses.`, amount);
        session.send(`Great! ${amountMsg}`);

        // store amount
        session.dialogData.amount = amount;

        // Prompt for delivery date
        var promptMessage = [
            `When do you want to receive the ${session.ngettext('rose', 'roses', amount)}?`,
            'Some valid options are:',
            ' - Tomorrow morning',
            ' - 12/30/2017',
            ' - 9PM Tomorrow',
            ' - Five hours from now',
            ' - From 9AM to 10AM tomorrow'];
        session.beginDialog('ask-date', { prompt: promptMessage.join('\n\n') });
    },

    function (session, results) {
        // results.response contains the date (or array of dates) returned from the prompt
        var momentOrRange = results.response;

        var amount = session.dialogData.amount;
        var nRoses = session.ngettext(`just one rose`, `${amount} roses`, amount);
        session.send(`Thank you! I'll deliver ${nRoses} ${momentOrRangeToString(momentOrRange)}.`);

        // TODO: It should continue to a checkout dialog or page
        session.send('Have a nice day!');
        session.endDialog();
    }
]);

// Ask for amount of roses and validate input
bot.dialog('ask-amount', new builder.Prompt().onRecognize((context, callback) => {
    var input = context.message.text || '';

    // Parse user input as is
    var results = numberModel.parse(input);
    console.log('numberModel parse results: ', results);

    // Care for the first result only
    if (results.length && results[0].typeName === 'number') {
        var first = results[0];
        var resolution = parseFloat(first.resolution.value);
        if (resolution % 1 === 0) {
            // no decimal part detected, good!
            return callback(null, 1, resolution);
        } else {
            // decimal part detected
            context.dialogData.options.prompt = 'I need to send whole roses, not fractions of them. How many would you like to send?';
        }
    } else {
        context.dialogData.options.prompt = 'I\'m sorry, that doesn\'t seem to be a valid quantity';
    }

    // return with score 0 to re-prompt
    callback(null, 0);
}));

var DateValidationErros = {
    'past_value': 'I\'m sorry, but I need at least an hour to deliver.\n\n $moment$ is no good for me.\n\nWhat other moment suits you best?',
    'default': 'I\'m sorry, that doesn\'t seem to be a valid delivery date and time'
};

// Ask for delivery date and validate input
bot.dialog('ask-date', new builder.Prompt().onRecognize((context, callback) => {
    var result = validateAndExtract(context.message.text || '');
    console.log('ask-date-result:', result);

    if (result.valid) {
        // return value to calling dialog
        return callback(null, 1, result.value);
    }

    // Set error message and re-prompt;
    var errorTemplate = DateValidationErros[result.error] || DateValidationErros.default;
    context.dialogData.options.prompt = errorTemplate.replace('$moment$', momentOrRangeToString(result.value, ''));
    callback(null, 0);
}));

// Send welcome when conversation with bot is started, by initiating the root dialog
bot.on('conversationUpdate', function (message) {
    if (message.membersAdded) {
        message.membersAdded.forEach(function (identity) {
            if (identity.id === message.address.bot.id) {
                bot.beginDialog(message.address, '/');
            }
        });
    }
});

// log any bot errors into the console
bot.on('error', function (e) {
    console.log('And error ocurred', e);
});

// Date Helpers
function validateAndExtract(input) {

    var results = dateModel.parse(input);

    // Log the results
    console.log('dateModel.parse() results', results);

    // Check there are valid results
    if (results.length && results[0].typeName.startsWith('datetimeV2')) {
        // The DateTime model can return several resolution types (https://github.com/Microsoft/Recognizers-Text/blob/master/JavaScript/recognizers-date-time/src/dateTime/constants.ts#L2-L9)
        // We only care for those with a date, date and time, or date time period:
        // date, daterange, datetime, datetimerange

        var first = results[0];
        var subType = first.typeName.split('.')[1];
        var resolutionValues = first.resolution && first.resolution.values;

        if (!resolutionValues) {
            // no resolution values
            return {
                valid: false
            }
        }

        if (subType.includes('date') && !subType.includes('range')) {
            // a date (or date & time) or multiple
            var moments = resolutionValues.map(m => new Date(m.value));
            var moment = moments.find(isFuture) || moments[0];              // Look for the first future moment; default to first resolution
            if (isFuture(moment)) {
                // a future moment, valid!
                return {
                    valid: true,
                    value: moment
                };
            }

            // a past moment
            return {
                valid: false,
                error: 'past_value',
                value: moment,
            }
        } else if (subType.includes('date') && subType.includes('range')) {
            // range
            var from = new Date(resolutionValues[0].start);
            var to = new Date(resolutionValues[0].end);
            if (!isNaN(from.getTime()) && !isNaN(to.getTime())) {
                if (isFuture(from) && isFuture(to)) {
                    // future
                    return {
                        valid: true,
                        value: [from, to]
                    };
                }

                // past
                return {
                    valid: false,
                    error: 'past_value',
                    value: [from, to]
                };
            }
        }
    }

    return {
        valid: false
    };
}

function isFuture(date) {
    // at least one hour
    var anHour = 1000 * 60 * 60;
    return date.getTime() > (Date.now() + anHour);
}

function momentOrRangeToString(moment, momentPrefix) {
    momentPrefix = momentPrefix !== undefined ? momentPrefix : 'on ';
    if (_.isDate(moment)) {
        return momentPrefix + moment.toLocaleString('en-US');
    } else if (_.isArray(moment)) {
        return 'from ' + moment.map(m => momentOrRangeToString(m, '')).join(' to ');
    }

    return 'not supported';
}

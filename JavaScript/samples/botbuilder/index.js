// Our Number and DateTime Recognizer models
var Recognizers = require('recognizers-text');
var numberModel = Recognizers.NumberRecognizer.instance.getNumberModel(Recognizers.Culture.English);
var dateModel = Recognizers.DateTimeRecognizer.instance.getDateTimeModel(Recognizers.Culture.English);

// This loads the environment variables from the .env file
require('dotenv-extended').load();

var builder = require('botbuilder');
var restify = require('restify');
var helpers = require('./helpers');

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

        // Prompt for amout of roses
        var promptMessage = [
            'How many roses do you want to send?',
            'Some valid options are:',
            ' - A dozen',
            ' - 22',
            ' - Just one rose'];

        session.beginDialog('ask-amount', { prompt: promptMessage.join('\n\n') });
    },

    function (session, results) {
        var amount = results.response;
        var amountMsg = session.ngettext(`I'll send just one rose.`, `I'll send ${amount} roses.`, amount);
        session.send(`Great! ${amountMsg}`);

        // store amount
        session.dialogData.amount = amount;

        // Prompt for delivery date
        var promptMessage = [
            'When do you want to delivery to be sent?',
            'Some valid options are:',
            ' - ASAP',
            ' - Tomorrow morning',
            ' - 12/30/2017',
            ' - 9PM Tomorrow'];

        session.beginDialog('ask-date', { prompt: promptMessage.join('\n\n') });
    }
]);


// Ask for amount of roses and validate input
bot.dialog('ask-amount', new builder.Prompt().onRecognize((context, callback) => {
    var input = context.message.text || '';
    var results = numberModel.parse(input);
    console.log('numberModel parse results', results);

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

    callback(null, 0);
}));

// Ask for delivery date and validate input
bot.dialog('ask-date', new builder.Prompt().onRecognize((context, callback) => {
    var input = context.message.text || '';
    var results = dateModel.parse(input);
    console.log('dateModel parser results: ', results.length);
    results.forEach((r, ix) => {
        console.log(`result[${ix}]:`, r);
        if (r.resolution && r.resolution.get) {
            console.log(`result[${ix}].resolution:`, r.resolution.get('values').map(helpers.toObject))
        }
    });

    if (results.length && results[0].typeName.startsWith('datetimeV2')) {
        // The DateTime model can return several resolution types (https://github.com/Microsoft/Recognizers-Text/blob/master/JavaScript/recognizers-date-time/src/dateTime/constants.ts#L2-L9)
        // We only care for those with a date, date and time, or date time period:
        // date, daterange, datetime, datetimerange
        var first = results[0];
        if(first.typeName.split('.')[1].includes('date')) {
            console.log("*******************************", first.resolution.get("values"));
            // callback(null, 1, resolution)
            // return;

        }
    }

    context.dialogData.options.prompt = 'I\'m sorry, that doesn\'t seem to be a valid delivery date and time';
    context.dialogData.options.prompt = 'I\'m sorry, this is not yet implemented';
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
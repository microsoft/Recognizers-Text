# .NET Samples

The best way to learn how to use something is through examples. The .NET solution contains a simple sample to get you started.

## Simple Console ([source](./SimpleConsole))

This sample demonstrate the combination of all Recognizers to extract possible values from the user's input.

The important piece is the `Microsoft.Recognizers.Text` NuGet package, which you'll need to add using:

````
Install-Package Microsoft.Recognizers.Text
````

Then, the sample uses the recognize methods of each available Recognizer. We need to do so by passing the Culture code we'll want to detect. E.g.: `en-us`.

So far, the available recognition methods are:

````C#
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.Sequence;

// Use English for the Recognizers culture
var culture = Culture.English;

// Number recognizer will find any number from the input
// E.g "I have two apples" will return "2".
NumberRecognizer.RecognizeNumber(query, culture);

// Ordinal number recognizer will find any ordinal number
// E.g "eleventh" will return "11".
NumberRecognizer.RecognizeOrdinal(query, culture);

// Percentage recognizer will find any number presented as percentage
// E.g "one hundred percents" will return "100%"
NumberRecognizer.RecognizePercentage(query, culture);

// Number Range recognizer will find any cardinal or ordinal number range
// E.g. "between 2 and 5" will return "(2,5)"
NumberRecognizer.RecognizeNumberRange(query, culture);

// Age recognizer will find any age number presented
// E.g "After ninety five years of age, perspectives change" will return "95 Year"
NumberWithUnitRecognizer.RecognizeAge(query, culture);

// Currency recognizer will find any currency presented
// E.g "Interest expense in the 1988 third quarter was $ 75.3 million" will return "75300000 Dollar"
NumberWithUnitRecognizer.RecognizeCurrency(query, culture);

// Dimension recognizer will find any dimension presented
// E.g "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours." will return "6 Mile"
NumberWithUnitRecognizer.RecognizeDimension(query, culture);

// Temperature recognizer will find any temperature presented
// E.g "Set the temperature to 30 degrees celsius" will return "30 C"
NumberWithUnitRecognizer.RecognizeTemperature(query, culture);

// Datetime recognizer This model will find any Date even if its write in coloquial language 
// E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
DateTimeRecognizer.RecognizeDateTime(query, culture);

// PhoneNumber recognizer will find any phone number presented
// E.g "My phone number is ( 19 ) 38294427."
SequenceRecognizer.RecognizePhoneNumber(query, culture);

//IP recognizer will find any Ipv4/Ipv6 presented
// E.g "My Ip is 8.8.8.8"
SequenceRecognizer.RecognizeIpAddress(query, culture);

//Boolean recognizer will find yes/no like responses, including emoji -
// E.g "yup, I need that" will return "True"
ChoiceRecognizer.RecognizeBoolean(query, culture);
````

All these methods accept an input string and culture, and returns an **IEnumerable** of [ModelResult](../Microsoft.Recognizers.Text/Models/ModelResult.cs):

Alternatively, you can obtain model instances that can be re-used for recognizing multiple inputs:

````C#
// Number model
var recognizer = new NumberRecognizer("en-us");
var model = recognizer.GetNumberModel();

// Parse input using Number model
var result = model.Parse("I have twenty apples");

// result is:
// [
// 	{
// 		"start": 7,
// 		"end": 12,
// 		"resolution": {
// 			"value": "20"
// 		},
// 		"text": "twenty",
// 		"typeName": "number"
// 	}
// ]
````

## BotBuilder Sample ([source](./BotBuilder))

This sample demonstrate how the Recognizers can be used with a BotBuilder Bot to parse user input. The bot provides a basic experience for ordering roses, it starts by asking the amount of roses and then asks for a delivery date and time.

To test the sample:
- Launch the [BotFramework Emulator](https://github.com/Microsoft/BotFramework-Emulator/releases).
- File -> Open bot and navigate to <your_project_folder>/Samples/BotBuilder folder.
- Select BotBuilderRecognizerBot.bot file.

Once connected, the bot will send a welcome message. You can start the order flow by sending any message to the bot.

In order to validate user input, Recognizers-Text are used: [`Recognizers-Text.Number`](./Recognizers-Text/.NET/Microsoft.Recognizers.Text.Number/NumberRecognizer.cs) and [`Recognizers-Text.DateTime`](/Recognizers-Text/.NET/Microsoft.Recognizers.Text.Number/DateTimeRecognizer.cs).

This is a copy of the [DeliveryDialog](./BotBuilder/Dialogs/DeliveryDialog.cs) _ValidateQuantity_ method which validates that the user input is an Integer, between 1 and 100 and not a decimal number:

````C#
	private async Task<bool> ValidateQuantity(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var result = promptContext.Recognized.Value ?? string.Empty;
            var results = NumberRecognizer.RecognizeNumber(result, culture);

            if (results.Count == 0)
            {
                await promptContext.Context.SendActivityAsync(InvalidQuantityErrorMessage);
                return false;
            }

            if (results.First().TypeName == "number" && double.TryParse(results.First().Resolution["value"].ToString(), out double value))
            {
                // Validate number
                if ((value < 1) || (value % 1 != 0))
                {
                    await promptContext.Context.SendActivityAsync(InvalidQuantityErrorMessage);
                    return false;
                }

                if (value > 100)
                {
                    await promptContext.Context.SendActivityAsync(InvalidOverQuantityErrorMessage);
                    return false;
                }

                var quantityRoses = Convert.ToInt32(results.First().Resolution["value"]);
                var quantityMessage = quantityRoses == 1
                ? "I'll send just one rose."
                : $"I'll send {quantityRoses} roses.";
                promptContext.Recognized.Value = quantityRoses.ToString();
                await promptContext.Context.SendActivityAsync(quantityMessage);
                return true;
            }
            else
            {
                await promptContext.Context.SendActivityAsync(InvalidQuantityErrorMessage);
                return false;
            }
        }
````

The Prompts are added in a waterfall Dialog:

````C#
	public DeliveryDialog(IStatePropertyAccessor<DeliveryState> userProfileStateAccessor, ILoggerFactory loggerFactory)
            : base(nameof(DeliveryDialog))
        {
            UserProfileAccessor = userProfileStateAccessor ?? throw new ArgumentNullException(nameof(userProfileStateAccessor));
            // Add control flow dialogs
            var waterfallSteps = new WaterfallStep[]
            {
                    InitializeStateStepAsync,
                    PromptForQuantityStepAsync,
                    PromptForDateStepAsync,
                    DisplayDeliveryStateStepAsync,
            };
            AddDialog(new WaterfallDialog(ProfileDialog, waterfallSteps));
            AddDialog(new TextPrompt(QuantityPrompt, ValidateQuantity));
            AddDialog(new TextPrompt(DatePrompt, ValidateDate));
        }
````

Asking the user for a specific delivery time may require special parsing, like extracting both date and time from the user input, or even obtain a range of dates and times.

The [`DeliveryDialog`](./BotBuilder/Dialogs/DeliveryDialog.cs) _ValidateDate_ method does exactly that. It will prompt the user for a possible delivery time, parse the user's input and extract, at least, one of these available return values using the DateTime Recognizer:

 - date
 - daterange
 - datetime
 - datetimerange

(These are the DateTime Recognizer types that contains *date* information)

> NOTE: The DateTime Recognizer uses LUIS datetimeV2 subtypes. For a full list, please visit [LUIS prebuilt entities - Subtypes of datetimeV2](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/pre-builtentities#subtypes-of-datetimev2).

## Recognizer Function ([source](./RecognizerFunction))
This sample is a variant of the SimpleConsole, which you can deploy as a Web API using Azure Functions (serverless). It is a combination of all Recognizers to extract possible values from the user's input. 

You can pass the `text` (required) and culture (optional) properties to the WebAPI using GET or POST. The response will be a JSON array containing all entities. 

Example URL:
```
https://[app-name].azurewebsites.net/api/entities?text=[INPUT TEXT]&culture=[CULTURE CODE]
```

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

Then, launch the [BotFramework Emulator](https://github.com/Microsoft/BotFramework-Emulator/releases) and connect it to **http://127.0.0.1:3979/api/messages**.

Once connected, the bot will send a welcome message. You can start the order flow by sending any message to the bot.

In order to validate user input, Custom Prompts Dialogs are used: [`QuantityPrompt`](./BotBuilder/Dialogs/QuantityPrompt.cs) and [`DeliveryPrompt`](./BotBuilder/Dialogs/DeliveryPrompt.cs).

The minimum required code for a Custom Prompt is the `TryParse` method. This is a copy of the [QuantityPrompt](./BotBuilder/Dialogs/QuantityPrompt.cs) `TryParse` method, which validates that the user input is an Integer, between 1 and 100 and not a decimal number:

````C#
protected override bool TryParse(IMessageActivity message, out int result)
{
    result = 0;

    // Get Number for the specified culture
    var results = NumberRecognizer.RecognizeNumber(message.Text, this.culture);
    if (results.Count > 0)
    {
        if (results.First().TypeName == "number" &&
            double.TryParse(results.First().Resolution["value"].ToString(), out double value))
        {
            // Validate number
            if (value < 1)
            {
                this.promptOptions.DefaultRetry = "I need to deliver at least one rose =)";
                return false;
            }
            else if (value > 100)
            {
                this.promptOptions.DefaultRetry = "You cannot order more than 100 roses per day. Sorry!";
                return false;
            }
            else if (value % 1 != 0)
            {
                this.promptOptions.DefaultRetry = "I need to send whole roses, not fractions of them. How many would you like to send?";
                return false;
            }

            // return as Int
            result = Convert.ToInt32(value);
            return true;
        }
    }

    // No parse results
    this.promptOptions.DefaultRetry = "I'm sorry, that doesn't seem to be a valid quantity";
    return false;
}
````

From our existing dialog, we can invoke this prompt as follows:

````C#
[Serializable]
public class RootDialog : IDialog<object>
{
    public async Task StartAsync(IDialogContext context)
    {
        context.Wait(this.MessageReceivedAsync);
    }

    private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
    {
        // Welcome message
        // ...

        // Prompt for amount of roses
        var prompt = new QuantityPrompt(GetCurrentCultureCode());
        context.Call(prompt, this.OnQuantitySelected);
    }

    private async Task OnQuantitySelected(IDialogContext context, IAwaitable<int> result)
    {
        try
        {
            var quantity = await result;
            var quantityMessage = quantity == 1
                ? "I'll send just one rose."
                : $"I'll send {quantity} roses.";
            await context.PostAsync(quantityMessage);

            // Store amount
            context.ConversationData.SetValue("quantity", quantity);

            // Prompt for delivery date
            // ...

        }
        catch (TooManyAttemptsException)
        {
            await context.PostAsync("Restarting now...");
            context.Wait(this.MessageReceivedAsync);
        }
    }

    // ...
}
````

Asking the user for a specific delivery time may require special parsing, like extracting both date and time from the user input, or even obtain a range of dates and times.

The [`DeliveryPrompt`](./BotBuilder/Dialogs/DeliveryPrompt.cs) does exactly that. It will prompt the user for a possible delivery time, parse the user's input and extract, at least, one of these avaliable return values using the DateTime Recognizer:

 - date
 - daterange
 - datetime
 - datetimerange

(These are the DateTime Recognizer types that contains *date* information)

> NOTE: The DateTime Recognizer uses LUIS datetimeV2 subtypes. For a full list, please visit [LUIS prebuilt entities - Subtypes of datetimeV2](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/pre-builtentities#subtypes-of-datetimev2).

This prompt uses a helper method to call the DateTime Recognizer, to validate the subtype and check the selected delivery moment is at least one hour from now:

````C#
public static Extraction ValidateAndExtract(string input)
{
    // Get DateTime for the specified culture
    var results = DateTimeRecognizer.RecognizeDateTime(input, culture);

    // Check there are valid results
    if (results.Count > 0 && results.First().TypeName.StartsWith("datetimeV2"))
    {
        // The DateTime model can return several resolution types (https://github.com/Microsoft/Recognizers-Text/blob/master/.NET/Microsoft.Recognizers.Text.DateTime/Constants.cs#L7-L15)
        // We only care for those with a date, date and time, or date time period:
        // date, daterange, datetime, datetimerange

        var first = results.First();
        var resolutionValues = (IList<Dictionary<string, string>>)first.Resolution["values"];

        var subType = first.TypeName.Split('.').Last();
        if (subType.Contains("date") && !subType.Contains("range"))
        {
            // a date (or date & time) or multiple
            var moment = resolutionValues.Select(v => DateTime.Parse(v["value"])).FirstOrDefault();
            if (IsFuture(moment))
            {
                // a future moment, valid!
                return new Extraction
                {
                    IsValid = true,
                    Values = new[] { moment }
                };
            }

            // a past moment
            return new Extraction
            {
                IsValid = false,
                Values = new[] { moment },
                ErrorMessage = PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(moment))
            };
        }
        else if (subType.Contains("date") && subType.Contains("range"))
        {
            // range
            var from = DateTime.Parse(resolutionValues.First()["start"]);
            var to = DateTime.Parse(resolutionValues.First()["end"]);
            if (IsFuture(from) && IsFuture(to))
            {
                // future
                return new Extraction
                {
                    IsValid = true,
                    Values = new[] { from, to }
                };
            }

            var values = new[] { from, to };
            return new Extraction
            {
                IsValid = false,
                Values = values,
                ErrorMessage = PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(values))
            };
        }
    }

    return new Extraction
    {
        IsValid = false,
        Values = Enumerable.Empty<DateTime>(),
        ErrorMessage = "I'm sorry, that doesn't seem to be a valid delivery date and time"
    };
}

public static bool IsFuture(DateTime date)
{
    // at least one hour
    return date > DateTime.Now.AddHours(1);
}
````

We use the helper function from Custom Prompt form the [`TryParse` method](./BotBuilder/Dialogs/DeliveryPrompt.cs#L33):

````C#
protected override bool TryParse(IMessageActivity message, out IEnumerable<DateTime> result)
{
    var extraction = ValidateAndExtract(message.Text, this.culture);
    if (!extraction.IsValid)
    {
        this.promptOptions.DefaultRetry = extraction.ErrorMessage;
    }

    result = extraction.Values;
    return extraction.IsValid;
}
````

Finally, this is how you call the prompt and obtain the date (or dates) back:

````C#
private async Task OnQuantitySelected(IDialogContext context, IAwaitable<int> result)
{
        // ...
        
        // Prompt for delivery date
        var prompt = new DeliveryPrompt(GetCurrentCultureCode());
        context.Call(prompt, this.OnDeliverySelected);
}

private async Task OnDeliverySelected(IDialogContext context, IAwaitable<IEnumerable<DateTime>> result)
{
    try
    {
        // "result" contains the date (or array of dates) returned from the prompt
        var momentOrRange = await result;
        var quantity = context.ConversationData.GetValue<int>("quantity");
        var nRoses = quantity == 1 ? "Just one rose" : $"{quantity} roses";
        var text = $"Thank you! I'll deliver {nRoses} {DeliveryPrompt.MomentOrRangeToString(momentOrRange)}.";

        await context.PostAsync(text);

        // TODO: It should continue to a checkout dialog or page
        await context.PostAsync("Have a nice day!");
    }
    catch (TooManyAttemptsException)
    {
        await context.PostAsync("Restarting now...");
    }
    finally
    {
        context.Wait(this.MessageReceivedAsync);
    }
}
````

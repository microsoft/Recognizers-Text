using Microsoft.Recognizers.Text.Number;

namespace BotBuilderRecognizerSample
{
    using System;
    using System.Linq;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class QuantityPrompt : Prompt<int, int>
    {
        public const string QuantityPromptMessage =
            "How many roses do you want to send?\n\n" +
            "Some valid options are:\n\n" +
            " - A dozen\n\n" +
            " - 22\n\n" +
            " - Just one rose";

        private readonly string culture;

        public QuantityPrompt(string culture) : base(new PromptOptions<int>(QuantityPromptMessage, attempts: 5))
        {
            this.culture = culture;
        }

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
    }
}

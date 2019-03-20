using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;

namespace BotBuilderRecognizerBot
{
    public class DeliveryDialog : ComponentDialog
    {
        // User state for delivery dialog
        private const string DeliveryStateProperty = "deliveryState";
        private const string QuantityValue = "deliveryQuantity";
        private const string DateValue = "deliveryDate";

        // Prompts
        private const string QuantityPrompt = "quantityPrompt";
        private const string DatePrompt = "datePrompt";

        // Dialog IDs
        private const string ProfileDialog = "profileDialog";

        // Error messages
        private const string InvalidQuantityErrorMessage = "I'm sorry, that doesn't seem to be a valid quantity of roses. Please, try again";
        private const string InvalidOverQuantityErrorMessage = "I'm sorry, you can send a maximum of 100 roses in one day. Please, try again";
        private const string InvalidDateErrorMessage = "I'm sorry, that doesn't seem to be a valid delivery date and time. Please, try again";
        private const string PastValueErrorMessage = "I'm sorry, but I need at least an hour to deliver.\n\n $moment$ is no good for me.\n\nWhat other moment suits you best?";

        private readonly string culture;
        private IEnumerable<DateTime> values;

        public DeliveryDialog(IStatePropertyAccessor<DeliveryState> userProfileStateAccessor, string culture, ILoggerFactory loggerFactory)
            : base(nameof(DeliveryDialog))
        {
            UserProfileAccessor = userProfileStateAccessor ?? throw new ArgumentNullException(nameof(userProfileStateAccessor));

            this.culture = culture;

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

        public IStatePropertyAccessor<DeliveryState> UserProfileAccessor { get; }

        public static bool IsFuture(DateTime date)
        {
            // at least one hour
            return date > DateTime.Now.AddHours(1);
        }

        public static string MomentOrRangeToString(IEnumerable<DateTime> moments, string momentPrefix = "on ")
        {
            if (moments.Count() == 1)
            {
                return MomentOrRangeToString(moments.First(), momentPrefix);
            }

            return "from " + string.Join(" to ", moments.Select(m => MomentOrRangeToString(m, string.Empty)));
        }

        public static string MomentOrRangeToString(DateTime moment, string momentPrefix = "on ")
        {
            return momentPrefix + moment.ToString();
        }

        private async Task<DialogTurnResult> InitializeStateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await UserProfileAccessor.SetAsync(stepContext.Context, new DeliveryState());
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> PromptForQuantityStepAsync(
                                                WaterfallStepContext stepContext,
                                                CancellationToken cancellationToken)
        {
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context);

            // if we have everything we need, confirm delivery user and return.
            if (deliveryState != null && !string.IsNullOrWhiteSpace(deliveryState.Quantity) && !string.IsNullOrWhiteSpace(deliveryState.Date))
            {
                return await ConfirmDelivery(stepContext);
            }

            if (string.IsNullOrWhiteSpace(deliveryState.Quantity))
            {
                // prompt for quantity, if missing
                var opts = new PromptOptions
                {
                    Prompt = new Activity
                    {
                        Type = ActivityTypes.Message,
                        Text = "How many roses do you want to send?",
                    },
                };
                return await stepContext.PromptAsync(QuantityPrompt, opts);
            }
            else
            {
                return await stepContext.NextAsync();
            }
        }

        private async Task<DialogTurnResult> PromptForDateStepAsync(
                                                        WaterfallStepContext stepContext,
                                                        CancellationToken cancellationToken)
        {
            // Get the current profile object from user state
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context);

            var quantity = stepContext.Result as string;
            if (string.IsNullOrWhiteSpace(deliveryState.Quantity))
            {
                // save quantity.
                deliveryState.Quantity = quantity;
                await UserProfileAccessor.SetAsync(stepContext.Context, deliveryState);
            }

            if (string.IsNullOrWhiteSpace(deliveryState.Date))
            {
                var opts = new PromptOptions
                {
                    Prompt = new Activity
                    {
                        Type = ActivityTypes.Message,
                        Text = "When do you want to receive the delivery?",
                    },
                };
                return await stepContext.PromptAsync(DatePrompt, opts);
            }
            else
            {
                return await stepContext.NextAsync();
            }
        }

        private async Task<DialogTurnResult> DisplayDeliveryStateStepAsync(
                                                    WaterfallStepContext stepContext,
                                                    CancellationToken cancellationToken)
        {
            // Get the current profile object from user state.
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context);

            var dates = values;

            if (string.IsNullOrWhiteSpace(deliveryState.Date))
            {
                // save date
                deliveryState.Date = MomentOrRangeToString(dates);
                await UserProfileAccessor.SetAsync(stepContext.Context, deliveryState);
            }

            return await ConfirmDelivery(stepContext);
        }

        /// <summary>
        /// Validator function to verify if the quantity the user entered gets recognized.
        /// </summary>
        /// <param name="promptContext">Context for this prompt.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
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

        /// <summary>
        /// Validator function to verify if date the user entered gets recognized.
        /// </summary>
        /// <param name="promptContext">Context for this prompt.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        private async Task<bool> ValidateDate(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var dates = promptContext.Recognized.Value;

            if (dates is null)
            {
                await promptContext.Context.SendActivityAsync(InvalidDateErrorMessage);
                return false;
            }

            var results = DateTimeRecognizer.RecognizeDateTime(dates, culture);

            if (results.Count <= 0 || !results.First().TypeName.StartsWith("datetimeV2"))
            {
                await promptContext.Context.SendActivityAsync(InvalidDateErrorMessage);
                return false;
            }

            // The DateTime model can return several resolution types (https://github.com/Microsoft/Recognizers-Text/blob/master/.NET/Microsoft.Recognizers.Text.DateTime/Constants.cs#L7-L14)
            // We only care for those with a date, date and time, or date time period:
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
                    this.values = new[] { moment };
                    return true;
                }

                // a past moment
                await promptContext.Context.SendActivityAsync(PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(moment)));
                return false;
            }
            else if (subType.Contains("date") && subType.Contains("range"))
            {
                // range
                var from = DateTime.Parse(resolutionValues.First()["start"]);
                var to = DateTime.Parse(resolutionValues.First()["end"]);
                this.values = new[] { from, to };
                if (IsFuture(from) && IsFuture(to))
                {
                    return true;
                }

                // a past moment
                await promptContext.Context.SendActivityAsync(PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(values)));
                return false;
            }

            await promptContext.Context.SendActivityAsync(InvalidDateErrorMessage);
            return false;
        }

        /// <summary>
        /// Helper function to confirm the information in DeliveryState.
        /// </summary>
        private async Task<DialogTurnResult> ConfirmDelivery(WaterfallStepContext stepContext)
        {
            var context = stepContext.Context;
            var deliveryState = await UserProfileAccessor.GetAsync(context);

            var confirmMessage = deliveryState.Quantity == "1"
                ? $"Thank you! I'll deliver one rose {deliveryState.Date}."
                : $"Thank you! I'll deliver {deliveryState.Quantity} roses {deliveryState.Date}.";

            // Display the profile information and end dialog.
            await context.SendActivityAsync(confirmMessage);
            return await stepContext.EndDialogAsync();
        }
    }
}

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

namespace BotBuiderV4
{
    public class DeliveryDialog : ComponentDialog
    {
        // User state for greeting dialog
        private const string GDeliveryStateProperty = "deliveryState";
        private const string QuantityValue = "deliveryQuantity";
        private const string DateValue = "deliveryDate";

        // Prompts names
        private const string QuantityPrompt = "quantityPrompt";
        private const string DatePrompt = "datePrompt";

        // Dialog IDs
        private const string ProfileDialog = "profileDialog";

        private const string PastValueErrorMessage = "I'm sorry, but I need at least an hour to deliver.\n\n $moment$ is no good for me.";

        public static bool IsFuture(DateTime date)
        {
            // at least one hour
            return date > DateTime.Now.AddHours(1);
        }

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

        public IStatePropertyAccessor<DeliveryState> UserProfileAccessor { get; }

        private async Task<DialogTurnResult> InitializeStateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context, () => null);
            //if (deliveryState == null)
            //{
            //    var deliveryStateOpt = stepContext.Options as DeliveryState;
            //    if (deliveryStateOpt != null)
            //    {
            //        await UserProfileAccessor.SetAsync(stepContext.Context, deliveryStateOpt);
            //    }
            //    else
            //    {
                    await UserProfileAccessor.SetAsync(stepContext.Context, new DeliveryState());
            //    }
            //}

            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> PromptForQuantityStepAsync(
                                                WaterfallStepContext stepContext,
                                                CancellationToken cancellationToken)
        {
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context);

            // if we have everything we need, greet user and return.
            if (deliveryState != null && !string.IsNullOrWhiteSpace(deliveryState.Quantity) && !string.IsNullOrWhiteSpace(deliveryState.Date))
            {
                return await ConfirmDelivery(stepContext);
            }

            if (string.IsNullOrWhiteSpace(deliveryState.Quantity))
            {
                // prompt for name, if missing
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
            // Save quantity, if prompted.
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context);

            var quantity = stepContext.Result as string;
            if (string.IsNullOrWhiteSpace(deliveryState.Quantity))
            {
                // set quantity.
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
            // Save Date, if prompted.
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context);

            var date = stepContext.Result as string;
            if (string.IsNullOrWhiteSpace(deliveryState.Date) &&
                !string.IsNullOrWhiteSpace(date))
            {
                // set date
                deliveryState.Date = date;
                await UserProfileAccessor.SetAsync(stepContext.Context, deliveryState);
            }

            return await ConfirmDelivery(stepContext);
        }

        /// <summary>
        /// Validator function to verify if the user name meets required constraints.
        /// </summary>
        /// <param name="promptContext">Context for this prompt.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        private async Task<bool> ValidateQuantity(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var result = promptContext.Recognized.Value ?? string.Empty;
            var results = NumberRecognizer.RecognizeNumber(result, "en-us");

            if (results.Count == 0)
            {
                await promptContext.Context.SendActivityAsync($"Sorry, that doesn't seem to be a valid quantity of roses. Please, try again");
                return false;
            }

            if (results.First().TypeName == "number" && double.TryParse(results.First().Resolution["value"].ToString(), out double value))
            {
                // Validate number
                if ((value < 0) || (value % 1 != 0) )
                {
                    await promptContext.Context.SendActivityAsync($"Sorry, that doesn't seem to be a valid quantity of roses. Please, try again");
                    return false;
                }

                // return as string
                var quantityRoses = Convert.ToInt32(results.First().Resolution["value"]);
                var quantityMessage = quantityRoses == 1
                ? "I'll send just one rose."
                : $"I'll send {quantityRoses} roses.";
                await promptContext.Context.SendActivityAsync(quantityMessage);
                //promptContext.Recognized.Value = quantityRoses.ToString();
                return true;
            }
            else
            {
                await promptContext.Context.SendActivityAsync($"Sorry, that doesn't seem to be a valid quantity of roses. Please, try again");
                return false;
            }

        }

        /// <summary>
        /// Validator function to verify if city meets required constraints.
        /// </summary>
        /// <param name="promptContext">Context for this prompt.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        private async Task<bool> ValidateDate(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var result = promptContext.Recognized.Value;

            if (result is null)
            {
                await promptContext.Context.SendActivityAsync("I'm sorry, that doesn't seem to be a valid delivery date and time");
                return false;
            }

            var results = DateTimeRecognizer.RecognizeDateTime(result, "en-us");

            if (results.Count > 0 && results.First().TypeName == "datetimeV2.date")
            {
                // The DateTime model can return several resolution types (https://github.com/Microsoft/Recognizers-Text/blob/master/.NET/Microsoft.Recognizers.Text.DateTime/Constants.cs#L7-L14)
                // We only care for those with a date, date and time, or date time period:
                var first = results.First();
                var resolutionValues = (IList<Dictionary<string, string>>)first.Resolution["values"];

                var subType = first.TypeName.Split('.').Last();
                if (subType.Contains("date"))
                {
                    // a date (or date & time) or multiple
                    var moment = resolutionValues.Select(v => DateTime.Parse(v["value"])).FirstOrDefault();
                    if (IsFuture(moment))
                    {
                        // a future moment, valid!
                        //promptContext.Recognized.Value = $"Thank you! I'll deliver the roses on {moment}.";
                        promptContext.Recognized.Value = moment.ToString();
                        return true;
                    }

                    // a past moment
                    //promptContext.Recognized.Value = PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(moment));
                    promptContext.Context.SendActivityAsync(PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(moment)));
                    return false;
                }
            }

            promptContext.Context.SendActivityAsync("I'm sorry, that doesn't seem to be a valid delivery date and time");
            return false;

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

        // Helper function to confirm the information in DeliveryState.
        private async Task<DialogTurnResult> ConfirmDelivery(WaterfallStepContext stepContext)
        {
            var context = stepContext.Context;
            var deliveryState = await UserProfileAccessor.GetAsync(context);

            // Display the profile information and end dialog.
            await context.SendActivityAsync($"Thank you! I'll deliver {deliveryState.Quantity} roses on {deliveryState.Date}.");
            return await stepContext.EndDialogAsync();
        }
    }
}

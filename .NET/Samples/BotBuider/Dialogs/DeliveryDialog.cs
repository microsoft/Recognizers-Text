using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;

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
        private const string InvalidDateErrorMessage = "I'm sorry, that doesn't seem to be a valid delivery date and time. Please, try again";
        private const string PastValueErrorMessage = "I'm sorry, but I need at least an hour to deliver.\n\n $moment$ is no good for me.\n\nWhat other moment suits you best?";

        private IEnumerable<DateTime> values;

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
            AddDialog(new NumberPrompt<int>(QuantityPrompt, ValidateQuantity));
            AddDialog(new DateTimePrompt(DatePrompt, ValidateDate));
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
                // WaterfallStep always finishes with the end of the Waterfall or with another dialog; here it is a Prompt Dialog.
                // Running a prompt here means the next WaterfallStep will be run when the users response is received.
                return await stepContext.PromptAsync(QuantityPrompt, new PromptOptions { Prompt = MessageFactory.Text("How many roses do you want to send?") }, cancellationToken);
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

            var quantity = stepContext.Result.ToString(); // as string;
            if (string.IsNullOrWhiteSpace(deliveryState.Quantity))
            {
                // save quantity.
                deliveryState.Quantity = quantity;
                await UserProfileAccessor.SetAsync(stepContext.Context, deliveryState);
            }

            if (string.IsNullOrWhiteSpace(deliveryState.Date))
            {
                // WaterfallStep always finishes with the end of the Waterfall or with another dialog, here it is a Prompt Dialog.
                return await stepContext.PromptAsync(DatePrompt, new PromptOptions { Prompt = MessageFactory.Text("When do you want to receive the delivery?") }, cancellationToken);
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
        /// Validator function to verify if the quantity the user entered is valid.
        /// </summary>
        /// <param name="promptContext">Context for this prompt.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        private async Task<bool> ValidateQuantity(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            var quantityRoses = Convert.ToDouble(promptContext.Recognized.Value);

            // Validate number
            if ((quantityRoses < 1) || (quantityRoses % 1 != 0))
            {
                await promptContext.Context.SendActivityAsync(InvalidQuantityErrorMessage);
                return false;
            }

            var quantityMessage = quantityRoses == 1
            ? "I'll send just one rose."
            : $"I'll send {quantityRoses} roses.";
            promptContext.Recognized.Value = Convert.ToInt32(quantityRoses);
            await promptContext.Context.SendActivityAsync(quantityMessage);
            return true;
        }

        /// <summary>
        /// Validator function to verify if the date the user entered is valid.
        /// </summary>
        /// <param name="promptContext">Context for this prompt.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        private async Task<bool> ValidateDate(PromptValidatorContext<IList<DateTimeResolution>> promptContext, CancellationToken cancellationToken)
        {
            var dates = promptContext.Recognized.Value;

            if (dates is null)
            {
                await promptContext.Context.SendActivityAsync(InvalidDateErrorMessage);
                return false;
            }

            if (dates.First().Start is null)
            {
                var moment = DateTime.Parse(dates.First().Value);

                if (IsFuture(moment))
                {
                    this.values = new[] { moment };
                    return true;
                }

                // a past moment
                await promptContext.Context.SendActivityAsync(PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(moment)));
                return false;
            }
            else
            {
                // range
                var from = DateTime.Parse(dates.First().Start);
                var to = DateTime.Parse(dates.First().End);
                this.values = new[] { from, to };

                if (IsFuture(from) && IsFuture(to))
                {
                    return true;
                }

                await promptContext.Context.SendActivityAsync(PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(values)));
                return false;
            }
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

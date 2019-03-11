using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace BotBuilderRecognizerBotv4.Dialogs
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
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context, () => null);
            if (deliveryState == null)
            {
                var deliveryStateOpt = stepContext.Options as DeliveryState;
                if (deliveryStateOpt != null)
                {
                    await UserProfileAccessor.SetAsync(stepContext.Context, deliveryStateOpt);
                }
                else
                {
                    await UserProfileAccessor.SetAsync(stepContext.Context, new DeliveryState());
                }
            }

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
                return await GreetUser(stepContext);
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
            // Save name, if prompted.
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context);

            var quantity = stepContext.Result as string;
            if (string.IsNullOrWhiteSpace(deliveryState.Quantity))
            {
                // Capitalize and set name.
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
                        Text = $"Great! I'll send {deliveryState.Quantity} roses. When do you want to receive the delivery?",
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
            // Save city, if prompted.
            var deliveryState = await UserProfileAccessor.GetAsync(stepContext.Context);

            var date = stepContext.Result as string;
            if (string.IsNullOrWhiteSpace(deliveryState.Date) &&
                !string.IsNullOrWhiteSpace(date))
            {
                // capitalize and set city
                deliveryState.Date = date;
                await UserProfileAccessor.SetAsync(stepContext.Context, deliveryState);
            }

            return await GreetUser(stepContext);
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
            // Validate that the user entered a minimum length for their name.
            return true;
            
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
            // Validate that the user entered a minimum lenght for their name
            return true;

        }

        // Helper function to greet user with information in GreetingState.
        private async Task<DialogTurnResult> GreetUser(WaterfallStepContext stepContext)
        {
            var context = stepContext.Context;
            var deliveryState = await UserProfileAccessor.GetAsync(context);

            // Display their profile information and end dialog.
            await context.SendActivityAsync($"Hi {deliveryState.Quantity}, from {deliveryState.Date}, nice to meet you!");
            return await stepContext.EndDialogAsync();
        }
    }
}

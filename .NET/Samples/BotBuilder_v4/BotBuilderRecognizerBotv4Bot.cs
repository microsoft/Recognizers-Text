// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BotBuilderRecognizerBotv4.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;

namespace BotBuilderRecognizerSample
{
    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service. Transient lifetime services are created
    /// each time they're requested. For each Activity received, a new instance of this
    /// class is created. Objects that are expensive to construct, or have a lifetime
    /// beyond the single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class BotBuilderRecognizerBotv4Bot : IBot
    {
        private const string WelcomeText = "Welcome to Contoso Roses. These are the roses you are looking for! Type anything to get started";

        private const string PastValueErrorMessage = "I'm sorry, but I need at least an hour to deliver.\n\n $moment$ is no good for me.\n\nWhat other moment suits you best?";

        //private readonly IStatePropertyAccessor<DeliveryState> _accessors;

        private readonly IStatePropertyAccessor<DeliveryState> _deliveryStateAccessor;
        private readonly IStatePropertyAccessor<DialogState> _dialogStateAccessor;
        private readonly UserState _userState;
        private readonly ConversationState _conversationState;

        private int quantityRoses;

        public static bool IsFuture(DateTime date)
        {
            // at least one hour
            return date > DateTime.Now.AddHours(1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BotBuilderRecognizerBotv4Bot"/> class.
        /// </summary>
        /// <param name="accessors">The state accessors this instance will be needing at runtime.</param>
        public BotBuilderRecognizerBotv4Bot(UserState userState, ConversationState conversationState, ILoggerFactory loggerFactory)
        {
            _userState = userState ?? throw new ArgumentNullException(nameof(userState));
            _conversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));

            _deliveryStateAccessor = _userState.CreateProperty<DeliveryState>(nameof(DeliveryState));
            _dialogStateAccessor = _conversationState.CreateProperty<DialogState>(nameof(DialogState));


            Dialogs = new DialogSet(_dialogStateAccessor);
            Dialogs.Add(new DeliveryDialog(_deliveryStateAccessor, loggerFactory));
        }

        private DialogSet Dialogs { get; set; }

        /// <summary>
        /// This controls what happens when an <see cref="Activity"/> gets sent to the bot.
        /// </summary>
        /// <param name="turnContext">Provides the <see cref="ITurnContext"/> for the turn of the bot.</param>
        /// <param name="cancellationToken" >(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>>A <see cref="Task"/> representing the operation result of the Turn operation.</returns>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            var activity = turnContext.Activity;
            var dialogContext = await Dialogs.CreateContextAsync(turnContext);

            // We are only interested in Message Activities.
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var results = await dialogContext.ContinueDialogAsync();

                // If the DialogTurnStatus is Empty we should start a new dialog.
                if (!dialogContext.Context.Responded)
                {
                    // examine results from active dialog
                    switch (results.Status)
                    {
                        case DialogTurnStatus.Empty:
                            await dialogContext.BeginDialogAsync(nameof(DeliveryDialog));
                            break;
                        case DialogTurnStatus.Complete:
                            await dialogContext.EndDialogAsync();
                            break;

                        default:
                            await dialogContext.CancelAllDialogsAsync();
                            break;
                    }
                }
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (turnContext.Activity.MembersAdded != null)
                {
                    // Send a welcome message to the user and tell them what actions they may perform to use this bot
                    await SendWelcomeMessageAsync(turnContext, cancellationToken);
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
            }

            // Save the new turn count into the conversation state.
            await _conversationState.SaveChangesAsync(turnContext);
            await _userState.SaveChangesAsync(turnContext);
        }

        /// <summary>
        /// This is an example of a custom validator. This example can be directly used on a float NumberPrompt.
        /// Returning true indicates the recognized value is acceptable. Returning false will trigger re-prompt behavior.
        /// </summary>
        /// <param name="promptContext">The <see cref="PromptValidatorContext"/> gives the validator code access to the runtime, including the recognized value and the turn context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the operation result of the Turn operation.</returns>
        public Task<bool> QuantityValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var result = promptContext.Recognized.Value;

            if (result is null)
            {
                return Task.FromResult(false);
            }

            var results = NumberRecognizer.RecognizeNumber(result, "en-us");

            if (results.Count == 0)
            {
                return Task.FromResult(false);
            }

            if (results.First().TypeName == "number" && double.TryParse(results.First().Resolution["value"].ToString(), out double value))
            {
                // Validate number
                if (value < 0)
                {
                    return Task.FromResult(false);
                }

                // return as string
                quantityRoses = Convert.ToInt32(results.First().Resolution["value"]);
                var quantityMessage = quantityRoses == 1
                ? "I'll send just one rose."
                : $"I'll send {quantityRoses} roses.";
                promptContext.Recognized.Value = quantityMessage;
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> DeliveryValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var result = promptContext.Recognized.Value;

            if (result is null)
            {
                promptContext.Recognized.Value = "I'm sorry, that doesn't seem to be a valid delivery date and time";
                return Task.FromResult(false);
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
                        promptContext.Recognized.Value = $"Thank you! I'll deliver the roses on {moment}.";
                        isDeliverySet = true;
                        return Task.FromResult(true);
                    }

                    // a past moment
                    promptContext.Recognized.Value = PastValueErrorMessage.Replace("$moment$", MomentOrRangeToString(moment));
                    return Task.FromResult(true);
                }
            }

            promptContext.Recognized.Value = "I'm sorry, that doesn't seem to be a valid delivery date and time";
            return Task.FromResult(false);
        }

        /// <summary>
        /// On a conversation update activity sent to the bot, the bot will
        /// send a message to the any new user(s) that were added.
        /// </summary>
        /// <param name="turnContext">Provides the <see cref="ITurnContext"/> for the turn of the bot.</param>
        /// <param name="cancellationToken" >(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>>A <see cref="Task"/> representing the operation result of the Turn operation.</returns>
        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(WelcomeText, cancellationToken: cancellationToken);
                }
            }
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
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;

namespace BotBuiderV4
{
    /// <summary>
    /// Main entry point and orchestration for bot.
    /// </summary>
    public class BotBuiderV4Bot : IBot
    {
        private const string WelcomeText = "Welcome to Contoso Roses. These are the roses you are looking for! Type anything to get started";

        private readonly IStatePropertyAccessor<DeliveryState> _deliveryStateAccessor;

        private readonly IStatePropertyAccessor<DialogState> _dialogStateAccessor;

        private readonly UserState _userState;

        private readonly ConversationState _conversationState;

        private readonly BotServices _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotBuiderV4Bot"/> class.
        /// <param name="botServices">Bot services.</param>
        /// <param name="accessors">Bot State Accessors.</param>
        /// </summary>
        public BotBuiderV4Bot(BotServices services, UserState userState, ConversationState conversationState, ILoggerFactory loggerFactory)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _userState = userState ?? throw new ArgumentNullException(nameof(userState));
            _conversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));

            _deliveryStateAccessor = _userState.CreateProperty<DeliveryState>(nameof(DeliveryState));
            _dialogStateAccessor = _conversationState.CreateProperty<DialogState>(nameof(DialogState));

            Dialogs = new DialogSet(_dialogStateAccessor);
            Dialogs.Add(new DeliveryDialog(_deliveryStateAccessor, loggerFactory));
        }

        private DialogSet Dialogs { get; set; }

        /// <summary>
        /// Run every turn of the conversation. Handles orchestration of messages.
        /// <param name="turnContext">Bot Turn Context.</param>
        /// <param name="cancellationToken">Task CancellationToken.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// </summary>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;

            // Create a dialog context
            var dialogContext = await Dialogs.CreateContextAsync(turnContext);

            if (activity.Type == ActivityTypes.Message)
            {
                // Continue the current dialog
                var dialogResult = await dialogContext.ContinueDialogAsync();

                // if no one has responded,
                if (!dialogContext.Context.Responded)
                {
                    // examine results from active dialog
                    switch (dialogResult.Status)
                    {
                        case DialogTurnStatus.Empty:
                            await dialogContext.BeginDialogAsync(nameof(DeliveryDialog));
                            break;

                        case DialogTurnStatus.Waiting:
                            // The active dialog is waiting for a response from the user, so do nothing.
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
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (activity.MembersAdded != null)
                {
                    // Iterate over all new members added to the conversation.
                    foreach (var member in activity.MembersAdded)
                    {
                        // Greet anyone that was not the target (recipient) of this message.
                        if (member.Id != activity.Recipient.Id)
                        {
                            var response = CreateResponse(activity, WelcomeText);
                            await dialogContext.Context.SendActivityAsync(response);
                        }
                    }
                }
            }

            await _conversationState.SaveChangesAsync(turnContext);
            await _userState.SaveChangesAsync(turnContext);
        }

        // Create a message response.
        private Activity CreateResponse(Activity activity, string message)
        {
            var response = activity.CreateReply();
            response.Text = message;
            return response;
        }
    }
}

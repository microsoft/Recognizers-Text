// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
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
        private const string WelcomeText = "This bot will introduce you to prompt validations using Recognizers. Type anything to get started";

        private readonly BotBuilderRecognizerBotv4Accessors _accessors;

        /// <summary>
        /// The <see cref="DialogSet"/> that contains all the Dialogs that can be used at runtime.
        /// </summary>
        private readonly DialogSet _dialogs;

        //private int value;

        /// <summary>
        /// Initializes a new instance of the <see cref="PromptValidationsBot"/> class.
        /// </summary>
        /// <param name="accessors">The state accessors this instance will be needing at runtime.</param>
        public BotBuilderRecognizerBotv4Bot(BotBuilderRecognizerBotv4Accessors accessors)
        {
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
            _dialogs = new DialogSet(accessors.ConversationDialogState);
            _dialogs.Add(new TextPrompt("number", CustomPromptValidatorAsync));
        }

        /// <summary>
        /// This controls what happens when an <see cref="Activity"/> gets sent to the bot.
        /// </summary>
        /// <param name="turnContext">Provides the <see cref="ITurnContext"/> for the turn of the bot.</param>
        /// <param name="cancellationToken" >(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>>A <see cref="Task"/> representing the operation result of the Turn operation.</returns>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            // We are only interested in Message Activities.
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Run the DialogSet - let the framework identify the current state of the dialog from
                // the dialog stack and figure out what (if any) is the active dialog.
                var dialogContext = await _dialogs.CreateContextAsync(turnContext, cancellationToken);
                var results = await dialogContext.ContinueDialogAsync(cancellationToken);

                // If the DialogTurnStatus is Empty we should start a new dialog.
                if (results.Status == DialogTurnStatus.Empty)
                {
                    // A prompt dialog can be started directly on from the DialogContext. The prompt text is given in the PromptOptions.
                    // We have defined a RetryPrompt here so this will be used. Otherwise the Prompt text will be repeated.
                    await dialogContext.PromptAsync(
                        "number",
                        new PromptOptions
                        {
                            Prompt = MessageFactory.Text("Please enter a number."),
                            RetryPrompt = MessageFactory.Text("That wasn't a valid number. Please try again."),
                        },
                        cancellationToken);
                }

                // We had a dialog run (it was the prompt) now it is Complete.
                else //if (results.Status == DialogTurnStatus.Complete)
                {
                    // Check for a result.
                    if (results.Result != null)
                    {
                        // And finish by sending a message to the user. Next time ContinueAsync is called it will return DialogTurnStatus.Empty.
                        await turnContext.SendActivityAsync(MessageFactory.Text(results.Result.ToString()),cancellationToken);
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
            await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        /// <summary>
        /// This is an example of a custom validator. This example can be directly used on a float NumberPrompt.
        /// Returning true indicates the recognized value is acceptable. Returning false will trigger re-prompt behavior.
        /// </summary>
        /// <param name="promptContext">The <see cref="PromptValidatorContext"/> gives the validator code access to the runtime, including the recognized value and the turn context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the operation result of the Turn operation.</returns>
        public Task<bool> CustomPromptValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var result = promptContext.Recognized.Value;

            if (result != null)
            {
                var results = NumberRecognizer.RecognizeNumber(result, "en-us");

                if (results.Count > 0)
                {
                    if (results.First().TypeName == "number" && double.TryParse(results.First().Resolution["value"].ToString(), out double value))
                    {
                        // Validate number
                        if (value < 1)
                        {
                            promptContext.Recognized.Value = "I need to deliver at least one rose =)";
                            return Task.FromResult(true);
                        }
                        else if (value > 100)
                        {
                            promptContext.Recognized.Value = "You cannot order more than 100 roses per day. Sorry!";
                            return Task.FromResult(true);
                        }
                        else if (value % 1 != 0)
                        {
                            promptContext.Recognized.Value = "I need to send whole roses, not fractions of them. How many would you like to send?";
                            return Task.FromResult(true);
                        }
                        // return as string
                        promptContext.Recognized.Value = $"Thank you, I have your number as {results.First().Resolution["value"].ToString()}.";
                        return Task.FromResult(true);
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
            else
            {
                return Task.FromResult(false);
            }
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
                    await turnContext.SendActivityAsync(
                        $"Welcome to BotBuilderRecognizerBot_v4! {WelcomeText}",
                        cancellationToken: cancellationToken);
                }
            }
        }
    }
}

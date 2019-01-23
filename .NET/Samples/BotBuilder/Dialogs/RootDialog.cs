#pragma warning disable 1998

namespace BotBuilderRecognizerSample
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private static string GetCurrentCultureCode()
        {
            // Use English as default culture since this sample bot that does not include any localization resources
            // Thread.CurrentThread.CurrentUICulture.IetfLanguageTag.ToLower() can be used to obtain the user's preferred culture
            return "en-us";
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            // Welcome message
            var welcome = context.MakeMessage();
            welcome.Attachments.Add(new HeroCard
            {
                Title = "Welcome to Contoso Roses",
                Subtitle = "These are the roses you are looking for!",
                Images = new List<CardImage> { new CardImage("https://placeholdit.imgix.net/~text?txtsize=56&txt=Contoso%20Roses&w=640&h=330", "Contoso Roses") },
            }.ToAttachment());
            await context.PostAsync(welcome);

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
                var prompt = new DeliveryPrompt(GetCurrentCultureCode());
                context.Call(prompt, this.OnDeliverySelected);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Restarting now...");
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task OnDeliverySelected(IDialogContext context, IAwaitable<IEnumerable<DateTime>> result)
        {
            try
            {
                // "result" contains the date (or array of dates) returned from the prompt
                var momentOrRange = await result;
                var quantity = context.ConversationData.GetValue<int>("quantity");
                var quantityRoses = quantity == 1 ? "Just one rose" : $"{quantity} roses";
                var text = $"Thank you! I'll deliver {quantityRoses} {DeliveryPrompt.MomentOrRangeToString(momentOrRange)}.";

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
    }
}

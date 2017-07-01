using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading;
using KitSchmidt.Forms;

namespace KitSchmidt.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var messageReceived = activity.Text;           

            if (messageReceived == ConstantStrings.NewEvent)
            {
                await context.Forward(NewEventDialog.MakeNewEventDialog(), NewEventDialog.ResumeAfterNewEventDialog, activity, CancellationToken.None);
            }
            else
            {
                var reply = new Activity(type: ActivityTypes.Message);
                reply = DefaultActivity(context, activity);

                await context.PostAsync(reply.AsMessageActivity());
                context.Wait(MessageReceivedAsync);
            }
        }

        private static Activity DefaultActivity(IDialogContext context, Activity activity)
        {
            Activity reply = activity.CreateReply("Hey there! What would you like to do?");
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Markdown;
            reply.ReplyToId = context.Activity.Recipient.Id;

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction() { Title = ConstantStrings.NewEvent, Type = ActionTypes.ImBack, Value = ConstantStrings.NewEvent }
                    }
            };

            return reply;
        }
    }
}
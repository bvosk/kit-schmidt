using Autofac;
using KitSchmidt.App_Start;
using KitSchmidt.Common;
using KitSchmidt.Common.DAL;
using KitSchmidt.Common.DAL.Models;
using KitSchmidt.DAL;
using KitSchmidt.DAL.Models;
using KitSchmidt.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KitSchmidt.Dialogs
{
    [Serializable]
    public class MainMenuDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var activity = context.Activity;
            var reply = PresentMenu(context);
            await context.PostAsync(reply.AsMessageActivity());

            context.Wait(MenuSelectionReceived);
        }

        private static IMessageActivity PresentMenu(IDialogContext context)
        {
            var reply = context.MakeMessage();
            reply.Text = "What would you like to do?";

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction() { Title = Constants.NewEvent, Type = ActionTypes.ImBack, Value = Constants.NewEvent }
                    }
            };
            return reply;
        }

        private async Task MenuSelectionReceived(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text == Constants.NewEvent)
            {
                await context.Forward(NewEventDialog.MakeNewEventDialog(), NewEventDialog.ResumeAfterNewEventDialog, activity);
            }
            else
            {
                await context.PostAsync("Sorry, that's not a valid menu option. Here are the options again.");
                await context.PostAsync(PresentMenu(context));
            }
        }

        public static async Task ResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Let me know if there's anything else I can help you with.");
            context.Done(new object());
        }
    }
}
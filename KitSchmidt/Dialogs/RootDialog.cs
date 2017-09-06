using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading;
using KitSchmidt.Forms;
using KitSchmidt.Common.DAL;
using KitSchmidt.DAL;
using KitSchmidt.Common.DAL.Models;
using Microsoft.Bot.Builder.Internals.Fibers;

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

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            context.Call(new GreetUserDialog(), GreetUserDialog.AfterGreetUser);
        }
    }
}
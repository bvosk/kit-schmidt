using KitSchmidt.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KitSchmidt.Dialogs
{
    public class NewEventDialog
    {
        public static IDialog<EventPoll> MakeNewEventDialog()
        {
            return Chain.From(() => FormDialog.FromForm(EventPoll.BuildForm));
        }

        public static async Task ResumeAfterNewEventDialog(IDialogContext context, IAwaitable<EventPoll> result)
        {
            // Store the value that NewOrderDialog returned. 
            // (At this point, new order dialog has finished and returned some value to use within the root dialog.)
            var newEvent = await result;

            await context.PostAsync($"New order dialog just told me there's an event called {newEvent.Name} happening on {newEvent.Date.ToShortDateString()}!");

            // Again, wait for the next message from the user.
            context.Done("ResumeResult");
        }
    }
}
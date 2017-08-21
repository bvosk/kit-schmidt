using KitSchmidt.App_Start;
using KitSchmidt.Common.DAL;
using KitSchmidt.DAL;
using KitSchmidt.DAL.Models;
using KitSchmidt.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
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
            var eventCardAttachment = NewEventHeroCard(newEvent).ToAttachment();
            var reply = context.MakeMessage();

            // Store test
            var messageDataService = new MessageDataService(new KitContext());
            messageDataService.SaveMessage(new Message
            {
                Text = "This is a test",
                User = "Brian"
            });

            reply.Attachments.Add(eventCardAttachment);
            await context.PostAsync(reply);

            // Again, wait for the next message from the user.
            context.Done(newEvent);
        }

        private static HeroCard NewEventHeroCard(EventPoll newEvent)
        {
            return new HeroCard()
            {
                Title = $"{newEvent.Name}",
                Subtitle = $"{newEvent.Date.ToString("g")}",
                Images = new List<CardImage>
                {
                    new CardImage()
                    {
                        Url = ConstantStrings.EventImageUrl
                    }
                }
            };
        }
    }
}
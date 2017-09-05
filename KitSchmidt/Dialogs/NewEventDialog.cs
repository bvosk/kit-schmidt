using Autofac;
using KitSchmidt.App_Start;
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
    public class NewEventDialog
    {
        public static IDialog<EventForm> MakeNewEventDialog()
        {
            return Chain.From(() => FormDialog.FromForm(EventForm.BuildForm));
        }

        public static async Task ResumeAfterNewEventDialog(IDialogContext context, IAwaitable<EventForm> result)
        {
            // Store the value that NewOrderDialog returned. 
            // (At this point, new order dialog has finished and returned some value to use within the root dialog.)
            var newEvent = await result;

            // Store event
            //var eventDataService = new EventDataService(new KitContext());
            //eventDataService.SaveEvent(new Event
            //{
            //    Name = newEvent.Name,
            //    Date = newEvent.Date,
            //    Description = newEvent.Description,
            //    CoordinatorId = context.Activity.From.Id
            //});

            var eventCardAttachment = NewEventHeroCard(newEvent).ToAttachment();
            var reply = context.MakeMessage();
            reply.Attachments.Add(eventCardAttachment);
            await context.PostAsync(reply);

            context.Done(newEvent);
        }

        private static HeroCard NewEventHeroCard(EventForm newEvent)
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
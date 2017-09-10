using KitSchmidt.Common;
using KitSchmidt.Common.DAL.Models;
using KitSchmidt.DAL;
using KitSchmidt.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Create new event from form results
            var newEventForm = await result;
            var newEvent = new Event
            {
                Name = newEventForm.Name,
                Date = newEventForm.Date,
                Description = newEventForm.Description
            };

            // Store event
            var userId = context.UserData.GetValue<User>("user").Id;
            var dbContext = new KitContext();
            var user = dbContext.Users.Include(u => u.Events).First(u => u.Id == userId);
            user.Events.Add(newEvent);
            await dbContext.SaveChangesAsync();

            // Display event card confirmation
            var eventCardAttachment = NewEventHeroCard(newEvent).ToAttachment();
            var reply = context.MakeMessage();
            reply.Attachments.Add(eventCardAttachment);
            await context.PostAsync(reply);

            // Send event RSVP requests
            ConnectorClient connector = new ConnectorClient(new Uri(context.Activity.ServiceUrl));
            var members = await connector.Conversations.GetActivityMembersAsync(context.Activity.Conversation.Id, context.Activity.Id);
            foreach(var member in members)
            {
                var newConversation = await connector.Conversations.CreateDirectConversationAsync(
                    new ChannelAccount("KitSchmidt-Bot", "Kit Schmidt"),
                    new ChannelAccount(member.Id),
                    new Activity(
                        type: ActivityTypes.Message,
                        text: $"{user.Name} created a new event called {newEvent.Name}. Would you like to go?"));
            }

            context.Done(newEvent);
        }

        private static HeroCard NewEventHeroCard(Event newEvent)
        {
            return new HeroCard()
            {
                Title = $"{newEvent.Name}",
                Subtitle = $"{newEvent.Date.ToString("g")}",
                Images = new List<CardImage>
                {
                    new CardImage()
                    {
                        Url = Constants.EventImageUrl
                    }
                }
            };
        }
    }
}
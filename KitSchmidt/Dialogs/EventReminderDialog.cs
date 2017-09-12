using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using KitSchmidt.Common.DAL.Models;
using KitSchmidt.Utilitites;
using Microsoft.Bot.Connector;

namespace KitSchmidt.Dialogs
{
    internal class EventReminderDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var upcomingEvent = context.Activity.AsMessageActivity().Value as Event;

            var eventCardAttachment = Utilities.EventHeroCard(upcomingEvent).ToAttachment();
            var reminder = context.MakeMessage();
            reminder.Recipient = new ChannelAccount(upcomingEvent.Coordinator.UserId);
            reminder.From = new ChannelAccount("KitSchmidt", "Kit Schmidt");
            reminder.Text = $"Don't forget about {upcomingEvent.Coordinator.Name}'s upcoming event!";
            reminder.Attachments.Add(eventCardAttachment);

            await context.PostAsync(reminder);

            context.Done(new object());
        }
    }
}
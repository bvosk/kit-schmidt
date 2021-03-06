﻿using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using KitSchmidt.Utilitites;
using Microsoft.Bot.Connector;
using KitSchmidt.DAL;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace KitSchmidt.Dialogs
{
    [Serializable]
    public class EventReminderDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var upcomingEventId = int.Parse(context.Activity.AsMessageActivity().Attachments[0].Content.ToString());
            var upcomingEvent = new KitContext()
                .Events
                .Include(e => e.Coordinator)
                .FirstOrDefault(e => e.Id == upcomingEventId);
            if (upcomingEvent == null)
            {
                context.Reset();
            }

            var eventCardAttachment = Utilities.EventHeroCard(upcomingEvent).ToAttachment();

            var client = new ConnectorClient(new Uri(upcomingEvent.ServiceUrl));
            var botAccount = new ChannelAccount("KitSchmidt", "Kit Schmidt");
            var userAccount = new ChannelAccount(upcomingEvent.Coordinator.UserId);
            string conversationId = "";

            conversationId = (await client.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;

            var reminder = new Activity
            {
                Type = ActivityTypes.Message,
                Recipient = userAccount,
                From = botAccount,
                Text = $"Don't forget about {upcomingEvent.Coordinator.Name}'s upcoming event!",
                Conversation = new ConversationAccount(id: conversationId)
            };
            reminder.Attachments.Add(eventCardAttachment);

            await client.Conversations.SendToConversationAsync(reminder);

            await context.FlushAsync(CancellationToken.None);
            context.Reset();
        }
    }
}
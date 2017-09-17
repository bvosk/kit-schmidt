using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using KitSchmidt.DAL;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using KitSchmidt.Common;
using System.Collections.Generic;
using KitSchmidt.Common.DAL.Models;

namespace KitSchmidt.ProactiveCloudEngine
{
    public static class GetMessages
    {
        // [TimerTrigger("0 */5 * * * *")]
        [FunctionName("GetMessages")]
        public static async Task Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"Proactive cloud engine executed at: {DateTime.Now}");

            var directLineSecret = "8mro3m2-O_0.cwA.kHQ.j6pa4HpvkyZVs-pNaT3ZXHjZdTY_26jAYC_2x1kDzPU";
            var client = new DirectLineClient(secretOrToken: directLineSecret);

            var conversation = await client.Conversations.StartConversationAsync();

            var dbContext = new KitContext();
            var upcomingEvents = dbContext.Events
                .Include(e => e.Coordinator)
                // Occurs within the next hour
                .Where(e => (e.Date - DateTime.Now).Hours == 0)
                // Reminder has not already been sent
                .Where(e => !e.ReminderSent)
                .ToList();

            foreach(var upcomingEvent in upcomingEvents)
            {
                var reminderActivity = new Activity
                {
                    From = new ChannelAccount(Constants.PceId, "Proactive Cloud Engine"),
                    Type = ActivityTypes.Message,
                    Attachments = new List<Attachment>
                    {
                        new Attachment
                        {
                            Name = "Event",
                            ContentType = "Event",
                            Content = upcomingEvent.Id
                        }
                    }
                };

                var response = await client.Conversations.PostActivityAsync(conversation.ConversationId, reminderActivity);

                dbContext.Update(upcomingEvent);
                upcomingEvent.ReminderSent = true;

                log.Info($"Sent an event reminder for {upcomingEvent.Name}");
                log.Info($"Got this response from bot: { response?.Id?.ToString() ?? "null response" }");
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
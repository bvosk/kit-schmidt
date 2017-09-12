using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;
using KitSchmidt.DAL;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using KitSchmidt.Common;

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
                .Where(e => (DateTime.Now - e.Date).Hours < 1)
                .ToList();

            foreach(var upcomingEvent in upcomingEvents)
            {
                var reminderActivity = new Activity
                {
                    From = new ChannelAccount(Constants.PceId, "Proactive Cloud Engine"),
                    Type = ActivityTypes.Message
                };
                reminderActivity.Attachments.Add(new Attachment
                {
                    Name = "Event",
                    ContentType = "Event",
                    Content = upcomingEvent
                });

                var response = await client.Conversations.PostActivityAsync(conversation.ConversationId, reminderActivity);
                log.Info($"Sent an event reminder for {upcomingEvent.Name}");
                log.Info($"Got this response from bot: { response?.Id?.ToString() ?? "null response" }");
            }
        }
    }
}
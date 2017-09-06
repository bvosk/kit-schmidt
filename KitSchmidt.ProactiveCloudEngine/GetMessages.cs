using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Bot.Connector.DirectLine;
using System.Threading.Tasks;

namespace KitSchmidt.ProactiveCloudEngine
{
    public static class GetMessages
    {
        // [TimerTrigger("0 */5 * * * *")]
        [FunctionName("GetMessages")]
        public static async Task Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var directLineSecret = "8mro3m2-O_0.cwA.kHQ.j6pa4HpvkyZVs-pNaT3ZXHjZdTY_26jAYC_2x1kDzPU";
            var client = new DirectLineClient(secretOrToken: directLineSecret, );
            var conversation = await client.Conversations.StartConversationAsync();
            var testActivity = new Activity
            {
                From = new ChannelAccount(name: "Proactive-Engine"),
                Value = new { Field1 = "Hi", Field2 = "Hello" }
            };

            var response = await client.Conversations.PostActivityAsync(conversation.ConversationId, testActivity);
            log.Info($"Got response from bot: { response.ToString() }");
        }
    }
}
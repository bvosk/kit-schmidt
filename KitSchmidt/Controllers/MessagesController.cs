﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using KitSchmidt.Common;
using System;
using Microsoft.Bot.Builder.Internals.Fibers;
using KitSchmidt.DAL;
using KitSchmidt.DAL.Models;

namespace KitSchmidt
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            // Log message
            var dbContext = new KitContext();
            dbContext.Messages.Add(new Message
            {
                Type = activity.Type,
                From = activity.From.Name,
                To = activity.Recipient.Name,
                Text = activity.Text,
                InputDate = DateTime.Now
            });
            await dbContext.SaveChangesAsync();

            if (activity.Type == ActivityTypes.Message)
            {
                if (activity.From.Id == Constants.PceId)
                {
                    // Process event reminder from PCE
                    await Conversation.SendAsync(activity, () => new Dialogs.EventReminderDialog());
                }
                else
                {
                    try
                    {
                        await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                    }
                    catch (Exception Ex)
                    {
                        if (Ex.GetType() == typeof(InvalidNeedException))
                        {
                            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                            Activity reply = activity.CreateReply("Sorry, I'm having some difficulties here. I have to reboot myself. Lets start over.");
                            await connector.Conversations.ReplyToActivityAsync(reply);
                            StateClient stateClient = activity.GetStateClient();
                            await stateClient.BotState.DeleteStateForUserAsync(activity.ChannelId, activity.From.Id);
                        }
                    }
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
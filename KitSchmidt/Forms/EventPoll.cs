using Microsoft.Bot.Builder.FormFlow;
using System;

namespace KitSchmidt.Forms
{
    [Serializable]
    public class EventPoll
    {
        public String Name { get; set; }
        public DateTime Date { get; set; }
        public enum InvitePolicy { OthersWelcome, AskMe, InvitationOnly }
        [Optional]
        public string Description { get; set; }

        public static IForm<EventPoll> BuildForm()
        {
            return new FormBuilder<EventPoll>()
                .Message("Please give me some information about the event")
                .Build();
        }
    }
}
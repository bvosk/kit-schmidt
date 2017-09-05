using Microsoft.Bot.Builder.FormFlow;
using System;

namespace KitSchmidt.Forms
{
    [Serializable]
    public class EventForm
    {
        public String Name { get; set; }
        public DateTime Date { get; set; }
        [Optional]
        public string Description { get; set; }

        public static IForm<EventForm> BuildForm()
        {
            return new FormBuilder<EventForm>()
                .Message("Please give me some information about the event")
                .Build();
        }
    }
}
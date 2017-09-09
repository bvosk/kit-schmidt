using System;

namespace KitSchmidt.DAL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public DateTime InputDate { get; set; }
    }
}
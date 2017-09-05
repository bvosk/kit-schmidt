using System.Collections.Generic;

namespace KitSchmidt.Common.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string ChannelId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public List<Event> Events { get; set; }
    }
}

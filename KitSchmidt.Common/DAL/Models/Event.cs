using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitSchmidt.Common.DAL.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public String Description { get; set; }
        public User Coordinator { get; set; }
        public string ServiceUrl { get; set; }
        public bool ReminderSent { get; set; }
    }
}

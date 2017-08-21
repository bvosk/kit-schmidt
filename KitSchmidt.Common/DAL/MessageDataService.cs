using KitSchmidt.DAL;
using KitSchmidt.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitSchmidt.Common.DAL
{
    public class MessageDataService : IMessageDataService
    {
        private KitContext _kitContext;

        public MessageDataService(KitContext kitContext)
        {
            _kitContext = kitContext;
        }
        public void SaveMessage(Message message)
        {
            _kitContext.Messages.Add(message);
            _kitContext.SaveChanges();
        }
    }
}
